using Newtonsoft.Json;
using nucs.JsonSettings;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoolSDR.Class
{
    public class AppSettings : JsonSettings
    {
        private string m_FilePath;
        public override string FileName { get => m_FilePath; set => m_FilePath = value; }
        #region Settings

        //  public string SomeProperty { get; set; }
        // public System.Drawing.Rectangle MainWindowPosition { get; set; }
        // public FormWindowState MainWindowState { get; set; }

        //public Dictionary<string, object> Dictionary { get; set; } = new Dictionary<string, object>();
        // public int SomeNumberWithDefaultValue { get; set; } = 1;
        [JsonIgnore] public char ImIgnoredAndIWontBeSavedOrLoaded { get; set; }

        #endregion
        [JsonIgnore]
        SettingsBag m_SettingsBag;

        public SettingsBag DynamicProperties
        {
            get => m_SettingsBag;
            set => m_SettingsBag = value;
        }

        public int ActiveTab { get; set; }
        public AppSettings() { }
        [JsonIgnore]
        public static string LastFilePathRequest { get; set; }
        private AppSettings(string fileName) : base(fileName)
        {
            // Save this first in case we have an error, and want to delete the file
            LastFilePathRequest = fileName;
            base.Load();
            if (m_SettingsBag == null)
            {
                m_SettingsBag = new SettingsBag();
            }
        }


        public static AppSettings MySettings
        {
            // Assertion means you did not call MySettingsCreate(), or it failed. I don't want to check for you every time.
            get { Debug.Assert(m_settings != null); return m_settings; }
        }
        private static AppSettings m_settings;
        public static AppSettings MySettingsCreate(string folder)
        {
            if (m_settings == null)
            {
                string name = App.Name;
                Debug.Assert(!string.IsNullOrEmpty(name));
                string filepath = Path.Combine(folder, name);
                if (!filepath.EndsWith(".json"))
                {
                    filepath += ".json";
                }
                Debug.Assert(filepath.Contains(name));
                Debug.Assert(Directory.Exists(folder));
                m_settings = new AppSettings(filepath);

            }

            return m_settings;
        }


        public new void Save()
        {
            base.Save();
        }


        public T Get<T>(string key)
        {
            string s = m_SettingsBag.Get<string>(key);
            if (string.IsNullOrEmpty(s))
            {
                return default(T);
            }
            T ret = JsonConvert.DeserializeObject<T>(s);
            return ret;
        }

        public void Set<T>(T thing, string key)
        {
            string s = JsonConvert.SerializeObject(thing);
            m_SettingsBag.Set(key, s);
        }


    }

}
