using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using nucs.JsonSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Thetis;

namespace CoolSDR
{
    public class JSONSettingsBase : JsonSettings, IDisposable
    {
        private readonly string m_FilePath;
        private bool m_bLoading = false;


        [OnError] // An Example of how to handle Deserialization errors
        internal void OnError(StreamingContext context, ErrorContext errorContext)
        {
            SerialiserError(context, errorContext);
        }

        protected virtual void SerialiserError(StreamingContext context, ErrorContext errorContext)
        {
            Common.LogString("SerialiserError " + errorContext.Error.ToString());
            if (this.HadSettingsToLoad)
            {
                errorContext.Handled = true; // prevents error being raised
                MakeBackup();
            }
        }

        public override string FileName
        {
            get => m_FilePath;
            set
            {
                if (!m_bLoading && value != m_FilePath)
                    throw new NotSupportedException("Setting the Filename publically is prohibited. Use the constructor!");
            }
        }

        [JsonIgnore]
        public bool HadSettingsToLoad { get; private set; }
        [JsonIgnore]
        bool AvoidAutoSave { get; set; }
        [JsonIgnore]
        protected bool disposed = false;
        ~JSONSettingsBase()
        {
            if (!AvoidAutoSave) Save();
        }

        public new void Dispose()
        {
            if (disposed) return;
            base.Dispose();
            if (!AvoidAutoSave) Save();
            disposed = true;
        }

        public JSONSettingsBase() : base() { }


        public JSONSettingsBase(string folder, string fileName, bool loadNow = true)
        {
            lock (this) // multithreaded @ startup
            {
                Debug.Assert(!String.IsNullOrEmpty(folder));
                Debug.Assert(!String.IsNullOrEmpty(fileName));
                Debug.Assert(Directory.Exists(folder));
                if (!fileName.EndsWith(".json"))
                {
                    fileName += ".json";
                }

                m_FilePath = Path.Combine(folder, fileName);
                this.HadSettingsToLoad = File.Exists(m_FilePath);
                if (loadNow) Load();

            }


        }

        [JsonIgnore] // This is only present if the load failed, and the file was backed up here.
        public string BackedUpFilePath { get; private set; } = "";

        public string MakeBackup(string backupFullPath = "")
        {
            Debug.Assert(!String.IsNullOrEmpty(m_FilePath));
            if (string.IsNullOrEmpty(backupFullPath))
            {
                var n = Common.Now();
                string suffix = "--BAD--" + n.ToUnixTimeSeconds().ToString();
                var f = m_FilePath.LastIndexOf(".");
                if (f > 4)
                {
                    var first_part = m_FilePath.Substring(0, f);
                    var second_part = m_FilePath.Substring(f);
                    backupFullPath = first_part + suffix + second_part;
                    BackedUpFilePath = backupFullPath;
                }
                else
                {
                    backupFullPath = m_FilePath + ".failed";
                }

            }

            try
            {

                File.Copy(m_FilePath, backupFullPath, true);
                File.Delete(m_FilePath);
            }
            catch (Exception e)
            {
                Common.LogException(e, true, "Error when trying to back up");
            }

            return backupFullPath;
        }

        public new void Load()
        {
            m_bLoading = true;
            Debug.Assert(!string.IsNullOrEmpty(m_FilePath));

            try
            {
                base.Load(m_FilePath); // Note: this does NOT fail if the file does not exist; it creates a new one.
            }
            catch (Exception e)
            {
                try
                {
                    File.Copy(m_FilePath, m_FilePath + ".failed", true);
                    File.Delete(m_FilePath);
                    throw new Exception("The loaded json file, at:\n" + m_FilePath + " was bad.\n\nIt was renamed to:\n" + m_FilePath + ".failed");
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
