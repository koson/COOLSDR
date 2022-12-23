using CoolSDR.BandsTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolSDR.Modes;

namespace CoolSDR.BandsTypes
{
    public enum IARURegion
    {
        None = 0,
        FIRST = None,
        Europe = 1,
        AfricaEuropeMiddleEastNAsia = 1,
        Americas = 2,
        AsiaAndPacific = 3,
        BroadbandTX = 4,
        LAST
    }
 
    public class RegionConversion
    {
        public static string IARURegionToString(IARURegion r)
        {
            int i = (int)r;
            if (i <= 0)
            {
                return "Broadband TX";
            }
            if (i == (int)IARURegion.Europe)
            {
                return "EU";
            }
            if (i == (int)IARURegion.Americas)
            {
                return "North/South America";
            }
            if (i == (int)IARURegion.AsiaAndPacific)
            {
                return "Asia/Pacific";
            }
            return "Broadband TX";
        }
    }

    public abstract class BandLimits
    {
       public BandCollectionTypes CollectionType { get; set; } = BandCollectionTypes.Standard;
        public ModeKind AllowedModes { get; set; } = ModeKind.All;

        public ModeKind GetDefaultMode()
        {
            ModeKind binMode = ModeKind.CW | ModeKind.DIGI;
            if (this.MinFreq <= 0.5)
            {
                return binMode;
            }

            if (this.MaxFreq < 10)
            {
                if (this.MinFreq >= 5 && this.MaxFreq<= 6)
                {
                    return ModeKind.USB;
                }
                return ModeKind.LSB;
            }
            else
            {
                return ModeKind.USB;
            }

        }

        // only allow TX on the exact bands specified.
        public bool Channelised { get; set; } = false;
        public double MinFreq
        {
            get;
            set;
        }
        public double MaxFreq
        {
            get;
            set;
        }
        public bool TXAllowed
        {
            get;
            set;
        } = true;

        public string UniqueName
        {
            get;
            set;
        }
        public List<BandLimits> SubBands
        {
            get;
            protected set;
        } = new List<BandLimits>();

        [JsonConstructor]
        public BandLimits(BandCollectionTypes kind, string uniqueName,
            double fmin, double fmax, ModeKind allowedModes, bool txAllowed = true,  bool channelised = false)
        {
            CollectionType = kind;
            UniqueName = uniqueName;
            MinFreq = fmin; MaxFreq = fmax;
            Channelised = channelised;
            AllowedModes = allowedModes;
            Debug.Assert(fmax >= fmin); // spot frequencies are ok
            Debug.Assert(!string.IsNullOrEmpty(uniqueName));
            TXAllowed = txAllowed;
        }

        public void AddSubBand(BandLimits b)
        {
            SubBands.Add(b);
        }

        [JsonIgnore] public string StringSummary
        {
            get
            {
                return UniqueName + " (" + MinFreq + " -> " + MaxFreq + " MHz)";
            }
        }

        public new string ToString()
        {
            return UniqueName + ": " + MinFreq.ToString() + " - " + MaxFreq.ToString() + " MHz";
        }
    }

    namespace NamedBands
    {
        public class FiveTonBand : BandLimits
        {
            public FiveTonBand() : base(BandCollectionTypes.Standard,"Five Ton", 0.472, 0.479, ModeKind.USB|ModeKind.DIGI|ModeKind.AM) { }

        }

        public class TopBand : BandLimits
        {
            public TopBand(IARURegion region) : base(BandCollectionTypes.Standard, "Top Band", 1.8, 2.0, ModeKind.All)
            {
                if (region == IARURegion.Americas)
                    MinFreq = 1.8;
                else
                    MinFreq = 1.810;
                MaxFreq = 2.000;
            }
        }

        public class Eighty : BandLimits
        {
            public Eighty(IARURegion region) : base(BandCollectionTypes.Standard, "80m", 3.6, 3.8, ModeKind.All)
            {
                if (region == IARURegion.Americas)
                    MaxFreq = 3.9;
                else
                    MaxFreq = 3.8;
                MinFreq = 3;
            }
        }

        public class Forty : BandLimits
        {
            public Forty(IARURegion region) : base(BandCollectionTypes.Standard, "40m", 7, 7.1, ModeKind.All)
            {
                if (region == IARURegion.Americas)
                    MaxFreq = 7.3;
                else
                    MaxFreq = 7.2;
                MinFreq = 7;
            }
        }

        public class Sixty : BandLimits
        {
            public Sixty(IARURegion region) : base(BandCollectionTypes.Standard, "60m", 5.26125, 5.405, ModeKind.All)
            {
                if (region != IARURegion.Europe)
                {
                    base.MinFreq = 5.3320;

                }
            }
        }

        public class Thirty : BandLimits
        {
            public Thirty(IARURegion region) : base(BandCollectionTypes.Standard, "30m", 10.1, 10.150, ModeKind.All)
            {
            }
        }

        public class Twenty : BandLimits
        {
            public Twenty(IARURegion region) : base(BandCollectionTypes.Standard, "20m", 14.0, 14.3, ModeKind.All) { }
        }

        public class Fifteen : BandLimits
        {
            public Fifteen(IARURegion region) : base(BandCollectionTypes.Standard, "15m", 21, 21.450, ModeKind.All) { }
        }

        public class Ten : BandLimits
        {
            public Ten(IARURegion region) : base(BandCollectionTypes.Standard, "10m", 28, 30, ModeKind.All) { }
        }
        public class Six : BandLimits
        {
            public Six(IARURegion region) : base(BandCollectionTypes.Standard, "6m", 50, 54, ModeKind.All)
            {
                if (region != IARURegion.Americas)
                    MaxFreq = 54;
            }
        }
        public class Four : BandLimits
        {
            public Four(IARURegion region) : base(BandCollectionTypes.Standard, "4m", 70, 70.5, ModeKind.All)
            {
            }
        }

        public class CustomBand : BandLimits
        {
            [JsonConstructor]
            public CustomBand(string uniqueName,
                double fmin, double fmax, ModeKind allowedModes, bool txAllowed = true, bool channelised = false)
                : base(BandCollectionTypes.Custom, uniqueName, fmin, fmax, allowedModes, txAllowed, channelised) { }

            public CustomBand(double f, string name, int widthHz, ModeKind allowedModes) :
                base(BandCollectionTypes.Custom, name,  f, f + ((double)widthHz / 1000000.0), allowedModes)
            {

            }
        }
    }

}
