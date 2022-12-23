using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thetis;

namespace CoolSDR.Radio
{
    public class Receiver : RadioDSPRX
    {
        //[JsonIgnore]
        // private Filters filters;
        protected Receiver() { } // for JSONSettings
        public Receiver(int index1, int index2, uint t, string AppFolder, uint rx, string fileName)
            : base(index1, index2, t, AppFolder, rx, fileName)
        {
            InitPreamps();

            SyncDSP();

        }

        private void SyncDSP()
        {
            base.Update = true;
            base.Update = false;
        }

        [JsonIgnore]
        public PreampMode[] rx_preamp_by_band { get; private set; }
        [JsonIgnore]
        public int[] rx_step_attenuator_by_band { get; private set; }

        private void InitPreamps()
        {
            rx_preamp_by_band = new PreampMode[(int)Band.LAST];
            rx_preamp_by_band = new PreampMode[(int)Band.LAST];
            rx_step_attenuator_by_band = new int[(int)Band.LAST];

            for (int i = 0; i < (int)Band.LAST; i++)
            {
                switch ((Band)i)
                {
                    default:
                        rx_preamp_by_band[i] = PreampMode.HPSDR_ON;
                        rx_step_attenuator_by_band[i] = 0;
                        break;
                }


            }
        }
    }
}
