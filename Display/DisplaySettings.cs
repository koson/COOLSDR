using Newtonsoft.Json;
using nucs.JsonSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thetis;

namespace CoolSDR.Class
{
    public class DisplaySettings : JsonSettings
    {
        private const HPSDRModel DEFAULT_MODEL = HPSDRModel.HERMES;
        private string m_FilePath;
        private bool m_bLoading = false;
        public override string FileName
        {
            get => m_FilePath;
            set
            {
                if (!m_bLoading)
                    throw new NotSupportedException("Setting the Filename publically is prohibited. Use the constructor!");
                m_FilePath = value;
            }
        }

        [JsonIgnore]
        public bool HadSettingsToLoad { get; private set; }


        public Thetis.HPSDRModel Model { get; set; } = HPSDRModel.HERMES;

        public DisplaySettings(string folder, string fileName = "DisplaySettings.js")
        {
            m_bLoading = true;
            Debug.Assert(folder != null);
            Debug.Assert(Directory.Exists(folder));
            Debug.Assert(fileName != null);
            FileName = Path.Combine(folder, fileName);
            Debug.Assert(Directory.Exists(folder));
            m_FilePath = Path.Combine(folder, fileName);
            this.HadSettingsToLoad = File.Exists(m_FilePath);
            m_bLoading = true;

            try
            {
                base.Load(m_FilePath); // Note: this does NO fail if the file does not exist; it creates a new one.
            }
            catch (Exception e)
            {
                try
                {
                    File.Copy(m_FilePath, m_FilePath + ".failed", true);
                    File.Delete(m_FilePath);
                    throw e;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                throw e;
            }
            finally
            {
                m_bLoading = false;
            }


        }
    }
}
