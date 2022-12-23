using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thetis;

namespace CoolSDR.Radio
{
    public class Transmitter : RadioDSPTX
    {
        [JsonIgnore]
        //  private Filters filters;
        //[JsonIgnore]
        public int[] tx_step_attenuator_by_band
        {
            get; private set;
        }
        private void InitAttens()
        {
            tx_step_attenuator_by_band = new int[(int)Band.LAST];
            for (int i = 0; i < (int)Band.LAST; i++)
            {
                tx_step_attenuator_by_band[i] = 31;
            }
        }

        protected Transmitter() { } // for JSONSettings
        public Transmitter(int index, uint whichThread, string AppFolder, string fileName) : base(index, whichThread, AppFolder, fileName) { InitAttens(); }

    }
}
