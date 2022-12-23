// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace CoolComponents
{

    public class FrequencyManager
    {


        public const Int64 MEGA = 1000000;
        public const Int64 KILO = MEGA / 1000;
        public const Int64 HERTZ = 1;
        private const Int64 DEFAULT_MIN = 50 * KILO;
        private const Int64 DEFAULT_MAX = 50 * MEGA;
        private const Int64 DEFAULT_FREQ = 648 * KILO;
        private Int64 m_freq = DEFAULT_FREQ;



        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public IVfo Buddy
        {
            get;
            set;
        }

        private Int64 m_min = DEFAULT_MIN;
        [Description("Minimum FreqManager allowed, Hz"), Category("Cool")]
        public Int64 Min
        {
            get => m_min;
            set => m_min = value;
        }


        [Description("Maximum FreqManager allowed, Hz"), Category("Cool")]
        public Int64 Max { get; set; } = DEFAULT_MAX;

        public double MaxFreqMHz
        {
            get
            {
                double ret = (double)Max / (double)MEGA;
                return ret;
            }

        }

        public double MinFreqMHz
        {
            get
            {
                double ret = (double)Min / (double)MEGA;
                return ret;
            }

        }


        public FrequencyManager()
        {
            FreqInHz = DEFAULT_FREQ;
            Tests();
        }

        [Description("The current frequency, in Hz"), Category("Cool")]
        public Int64 FreqInHz
        {
            set
            {
                var mhz = (ushort)(value / MEGA);
                MHzPart = mhz;
                var tmp = value - (mhz * MEGA);
                var khz = (ushort)(tmp / KILO);
                kHzPart = khz;
                tmp = value - ((mhz * MEGA) + (khz * KILO));
                HzPart = (ushort)tmp;
                m_freq = value;
                if (m_freq < Min)
                {
                    FreqInHz = Max;

                }
                if (m_freq > Max)
                {
                    FreqInHz = Min;
                }
                // double freq_mhz = (double)(m_freq / FrequencyManager.MEGA);
                if (this.Buddy != null)
                    Buddy.FrequencyChanged(m_freq / (double)MEGA);
            }
            get => m_freq;
        }

        public double FreqInMHz
        {
            get => (double)(double)m_freq / (double)MEGA;
            set
            {
                long myval = (long)value * (long)MEGA;
                this.FreqInHz = myval;
            }
        }

#pragma warning disable IDE1006

        [Description("Just the MHz part of the current frequency"), Category("Cool")]
        public int MHzPart { get; private set; }
        [Description("Just the kHz part of the current frequency"), Category("Cool")]
        public int kHzPart { get; private set; }
        [Description("Just the Hz part of the current frequency"), Category("Cool")]
        public int HzPart { get; private set; }

        [Description("Mhz, int the format xx"), Category("Cool")]
        public string DisplayMHz { get => MHzPart.ToString("00", CultureInfo.InvariantCulture); }
        [Description("khz, int the format xxx"), Category("Cool")]
        public string DisplaykHz { get => kHzPart.ToString("000", CultureInfo.InvariantCulture); }
        [Description("khz, int the format xxx"), Category("Cool")]
        public string DisplayHz { get => HzPart.ToString("000", CultureInfo.InvariantCulture); }

        internal void Tests()
        {
#if DEBUG
            var f = this;
            f.FreqInHz = 27_555_000;
            Debug.Assert(f.MHzPart == 27);
            Debug.Assert(f.kHzPart == 555);
            Debug.Assert(f.HzPart == 0);

            f.FreqInHz = 20_000_000;
            Debug.Assert(f.MHzPart == 20);
            Debug.Assert(f.kHzPart == 0);
            Debug.Assert(f.HzPart == 0);

            f.FreqInHz = 1_908_000;
            Debug.Assert(f.MHzPart == 1);
            Debug.Assert(f.kHzPart == 908);
            Debug.Assert(f.HzPart == 0);

            f.FreqInHz = 3_615_000;
            Debug.Assert(f.MHzPart == 3);
            Debug.Assert(f.kHzPart == 615);
            Debug.Assert(f.HzPart == 0);

            f.FreqInHz = 3_615_025;
            Debug.Assert(f.MHzPart == 3);
            Debug.Assert(f.kHzPart == 615);
            Debug.Assert(f.HzPart == 25);

            // checked wrapping corner cases:
            Int64 tmp = f.Max;
            f.FreqInHz = tmp + 1;
            Debug.Assert(f.FreqInHz == f.Min);

            tmp = f.Min;
            f.FreqInHz = tmp - 1;
            Debug.Assert(f.FreqInHz == f.Max);
#endif
        }
    }

    public interface IVfo
    {
        void FrequencyChanged(double newFreqMHz);
    }




}
