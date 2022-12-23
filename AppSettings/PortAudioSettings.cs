using Newtonsoft.Json;
using nucs.JsonSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CoolSDR.Class
{

    public class PortAudioSettings : JSONSettingsBase
    {



        public int SampleRate { get; set; }
        public int BufferSize { get; set; }
        public string API { get; set; }
        public string InputDevice { get; set; }
        public string OutputDevice { get; set; }
        public bool AudioExclusive { get; set; }
        public int APIIndex { get; set; }

        public PortAudioSettings() { }

        public new void Save()
        {
            base.Save();
        }

        public PortAudioSettings(string folder, string fileName = "PortAudio.json") : base(folder, fileName)
        {
        }

    }
}
