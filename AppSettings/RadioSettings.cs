using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolSDR
{
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

    //namespace CoolSDR
    //{
    public class RadioSettings : JsonSettings
    {
        private string m_FilePath;
        private bool m_bLoading = false;
        public override string FileName
        {
            get => m_FilePath;
            set
            {
                if (!m_bLoading && value != m_FilePath)
                    throw new NotSupportedException("Setting the Filename publically is prohibited. Use the constructor!");
            }
        }

        public Thetis.RadioProtocol LastSuccessfulProtocol { get; set; } = Thetis.RadioProtocol.Auto;
        public HPSDRModel RadioModel { get; set; } = HPSDRModel.HERMES;

        public Thetis.RadioProtocol Protocol
        {
            get;
            set;
        } = RadioProtocol.Auto;
        public bool UseStaticIP { get; set; } = false;

        [JsonIgnore]
        public bool HadSettingsToLoad { get; private set; }
        [JsonIgnore]
        public bool AvoidAutoSave { get; set; } = false;
        [JsonIgnore]
        protected bool disposed = false;
        ~RadioSettings()
        {
            if (disposed) return;
            if (!AvoidAutoSave) Save();
        }

        public new void Dispose()
        {
            if (disposed) return;
            base.Dispose();
            if (!AvoidAutoSave) Save();
            disposed = true;
        }

        [JsonIgnore]
        public static readonly string DEFAULT_IP_ADDRESS = "192.168.1.22";

        public string IPAddress { get; set; } = DEFAULT_IP_ADDRESS;

        public RadioSettings() : base() { }

        public RadioSettings(string folder, string fileName)
        {

            Debug.Assert(!String.IsNullOrEmpty(folder));
            Debug.Assert(!String.IsNullOrEmpty(fileName));
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

    //}
}
