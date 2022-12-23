using CoolSDR.BandsTypes.NamedBands;
using CoolSDR.Class;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SharpDX.Direct3D11;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Thetis;

namespace CoolSDR.BandsTypes
{



    public abstract class BandsBase : JSONSettingsBase
    {
        protected override void SerialiserError(StreamingContext context, ErrorContext errorContext)
        {
            Common.LogString("JSON error: " + errorContext.Error.ToString());
            // 
            if (Region != IARURegion.None)
            {
                if (base.HadSettingsToLoad)
                {
                    MessageBox.Show("There was an error reading " + this.ID + " settings from file:\n"
                        + base.FileName + "\n\nand so the defaults will be used here. The old file will be backed up to:\n"
                        + base.MakeBackup() + "\n\n\nDetails: " + errorContext.Error.Message, App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Common.LogString("This causes us to re-create the bands, for " + this.ID);
                MakeBands(true);
                errorContext.Handled = true;
            }
        }

        [JsonIgnore]
        public abstract BandCollectionTypes CollectionType { get; }

        public abstract string ID { get; }

        public IARURegion CurrentRegion
        {
            get => Region;
            set { SetRegion(value); }
        }

        public bool BroadbandTX
        {
            get;
            set;
        }

        public List<BandLimits> Bands
        {
            get;
            set;
        }
        protected abstract void MakeBands(bool force = false);
        private IARURegion m_Region = IARURegion.None;

        public IARURegion Region
        {
            get => m_Region;
            set { SetRegion(value); }
        }

        public BandsBase(string folder, string filename, bool loadSettings = false) : base(folder, filename, loadSettings)
        {

        }
        protected BandsBase() { } // to please JSONSettings

        public void SetRegion(IARURegion region, bool forceMakeBands = false)
        {
            if (region < IARURegion.FIRST || region >= IARURegion.LAST)
            {
                region = IARURegion.Europe;
            }
            m_Region = region;
            if (forceMakeBands) MakeBands(true);
        }


        [JsonIgnore]
        protected bool Constructed { get; set; }

        public bool RemoveBand(BandLimits band)
        {
            bool ret = Bands.Remove(band);
            this.Save();
            return ret;
        }
    }

    public class UserDefinedBands : BandsBase
    {
        [JsonIgnore] public override BandCollectionTypes CollectionType { get => BandCollectionTypes.Custom; }
        public override string ID { get { return "UserDefinedBands"; } }

        protected UserDefinedBands() { } // to please json
        ~UserDefinedBands() { }

        public UserDefinedBands(string folder, string filename) : base(folder, filename, false)
        {

            base.Load();
            if (this.Region == IARURegion.None)
            {
                Region = IARURegion.Europe;
                MakeUserDefinedBands(true);
            }
            Constructed = true;
            if (!this.HadSettingsToLoad)
            {
                this.Save();
            }

            if (!this.HadSettingsToLoad || Bands == null)
            {
                MakeBands(false);
                Save();
            }
        }

        protected override void MakeBands(bool force = false)
        {
            MakeUserDefinedBands(force);
        }

        private void MakeUserDefinedBands(bool force = false)
        {

            if (force || Bands == null || Bands.Count == 0)
            {
                Bands = new List<BandLimits>
                {
                    new CustomBand("Another-Example-Echo-Charlie", 6.200, 6.900,Modes.ModeKind.All),
                    new CustomBand("User-defined WARC 17m", 18.068, 18.168,Modes.ModeKind.All)
                };
            }

        }



    }

    public class StandardBands : BandsBase
    {
        [JsonIgnore]
        public override BandCollectionTypes CollectionType { get => BandCollectionTypes.Standard; }
        public override string ID { get { return "StandardBands"; } }

        protected StandardBands() { } // to please json
        public StandardBands(string folder, string filename) : base(folder, filename)
        {
            base.Load();
            if (this.Region == IARURegion.None)
            {
                Region = IARURegion.Europe;
                MakeBands(true);
            }

            if (!this.HadSettingsToLoad || Bands == null)
            {
                MakeBands(false);
                Save();
            }

            Constructed = true;
        }


        protected override void MakeBands(bool force = false)
        {
            MakeStandardBands(Region, force);
        }

        private void Add60mChannels(BandLimits sixty, IARURegion region)
        {
            // if we had an exact country instead of just IARU region,
            // we could be more precise here:
            var col = sixty.SubBands;

            switch (region)
            {
                case IARURegion.Europe:
                    col.Add(new CustomBand("UK 60m Bandlet 1", 5258.5,5264, Modes.ModeKind.CW));
                    col.Add(new CustomBand("UK 60m Bandlet 2", 5.276, 5.284, Modes.ModeKind.USB));
                    col.Add(new CustomBand("UK 60m Bandlet 3", 5.2885, 5292, Modes.ModeKind.CW));
                    col.Add(new CustomBand("UK 60m Bandlet 4", 5.298, 5.307, Modes.ModeKind.All));
                    col.Add(new CustomBand("UK 60m Bandlet 5", 5.313, 5.323, Modes.ModeKind.USB| Modes.ModeKind.AM));
                    col.Add(new CustomBand("UK 60m Bandlet 6", 5.333, 5.338, Modes.ModeKind.USB));
                    col.Add(new CustomBand("UK 60m Bandlet 7", 5.354, 5.358, Modes.ModeKind.All));
                    col.Add(new CustomBand("UK 60m Bandlet 8", 5.362, 5.3745, Modes.ModeKind.All));
                    col.Add(new CustomBand("UK 60m Bandlet 9", 5.378, 5.382, Modes.ModeKind.USB));
                    col.Add(new CustomBand("UK 60m Bandlet 10",5.395, 5.401, Modes.ModeKind.USB));
                    col.Add(new CustomBand("UK 60m Bandlet 11",5.4035,5.4065, Modes.ModeKind.USB));
                    break;
                case IARURegion.Americas:
                    col.Add(new CustomBand(5.3320, "60m Bandlet 1", 2800, Modes.ModeKind.All));
                    col.Add(new CustomBand(5.3480, "60m Bandlet 2", 2800, Modes.ModeKind.All));
                    col.Add(new CustomBand(5.3585, "60m Bandlet 3", 2800, Modes.ModeKind.All));
                    col.Add(new CustomBand(5.3730, "60m Bandlet 4", 2800, Modes.ModeKind.All));
                    col.Add(new CustomBand(5.4050, "60m Bandlet 5", 2800, Modes.ModeKind.All));
                    break;
                default:
                    col.Add(new CustomBand(5.3320, "60m Bandlet 1", 2800, Modes.ModeKind.All));
                    col.Add(new CustomBand(5.3480, "60m Bandlet 2", 2800, Modes.ModeKind.All));
                    col.Add(new CustomBand(5.3585, "60m Bandlet 3", 2800, Modes.ModeKind.All));
                    col.Add(new CustomBand(5.3730, "60m Bandlet 4", 2800, Modes.ModeKind.All));
                    col.Add(new CustomBand(5.4050, "60m Bandlet 5", 2800, Modes.ModeKind.All));
                    break;
            }
        }

        public void MakeStandardBands(IARURegion region, bool force = false)
        {

            if (force || Bands == null || Bands.Count == 0)
            {

                Bands = new List<BandLimits>();
                {
                    Bands.Add(new NamedBands.FiveTonBand());
                    Bands.Add(new NamedBands.TopBand(region));
                    Bands.Add(new NamedBands.Eighty(region));
                    Bands.Add(new NamedBands.Forty(region));
                    Bands.Add(new NamedBands.Sixty(region));
                    BandLimits bl = Bands[Bands.Count - 1];
                    Debug.Assert(bl.UniqueName == "60m");
                    Add60mChannels(bl, region);

                    Bands.Add(new NamedBands.Thirty(region));
                    Bands.Add(new NamedBands.Twenty(region));
                    Bands.Add(new NamedBands.Fifteen(region));
                    Bands.Add(new NamedBands.Ten(region));
                    if (region != IARURegion.Americas)
                        Bands.Add(new NamedBands.Six(region));
                    Bands.Add(new NamedBands.Four(region));
                };

            }
            return;
        }


        public BandLimits CurrentBand
        {
            get;
            set;
        }
    }

    public enum BandCollectionTypes
    {
        Standard = 0,
        Custom = 1,
    }
    public  class BandsCollection : JSONSettingsBase
    {


        protected BandsCollection() { }
        public BandsCollection(string folder, string filename) : base(folder, filename, false)
        {

            base.Load();


            try
            {
                m_HamBands = new StandardBands(folder, filename + "Standard");
            }
            catch (Exception ex)
            {
                Common.LogException(ex, true, "Error when loading Standard BandsCollection from file means we load the defaults");
                m_HamBands = new StandardBands(folder, filename + "Standard");
            }
            try
            {
                m_UserBands = new UserDefinedBands(folder, filename + "UserDefined");

            }
            catch (Exception ex)
            {
                Common.LogException(ex, true, "Error when loading User BandsCollection from file means we load the defaults");
                m_UserBands = new UserDefinedBands(folder, "UserDefined" + filename);

            }
            if (m_HamBands != null)
            {
                m_HamBands.BroadbandTX = m_btx;
                m_HamBands.SetRegion(m_region, false);
            }

            if (m_UserBands != null)
            {
                m_UserBands.BroadbandTX = m_btx;
                m_UserBands.SetRegion(m_region, false);
            }

#if DEBUG
            if (m_UserBands != null && m_HamBands != null)
            {
                TDD();
            }
#endif

        }

#if DEBUG
        private void TDD()
        {
            double f = 1.8;
            if (CurrentRegion == IARURegion.Europe)
            {
                Debug.Assert(FindBandForFrequency(f) == null);
                f = 1.810;
                Debug.Assert(FindBandForFrequency(f) != null);
                f = 0.648;
                Debug.Assert(FindBandForFrequency(f) == null);
                f = 1.908;
                var b = FindBandForFrequency(f);
                Debug.Assert(b != null);
                Debug.Assert(b.UniqueName == "Top Band");
                Debug.Assert(b.CollectionType == BandCollectionTypes.Standard);

                f = 6.3;
                b = FindBandForFrequency(f);
                Debug.Assert(b.CollectionType == BandCollectionTypes.Custom);
                Debug.Assert(b.UniqueName == "Another-Example-Echo-Charlie");

                f = 5.317;
                b = FindBandForFrequency(f);
                BandLimits sub = FindSubBandForFrequency(f, b);
                Debug.Assert(sub != null);

            }
        }
#endif

       public BandLimits FindSubBandForFrequency(double f, BandLimits MainBand)
       {
            if (MainBand.SubBands == null) return null;
            if (MainBand.SubBands.Count == 0) return null;
            foreach (BandLimits bl in MainBand.SubBands)
            {
                if (f >= bl.MinFreq && f <= bl.MaxFreq)
                {
                    return bl;
                }
            }
            return null;
        }

        [JsonIgnore]
        private readonly StandardBands m_HamBands;

        [JsonIgnore]
        private readonly UserDefinedBands m_UserBands;

        [JsonIgnore]
        public StandardBands HamBands { get => m_HamBands; }
        [JsonIgnore]
        public UserDefinedBands UserBands { get => m_UserBands; }

        private bool m_btx;

        [JsonRequired]
        public bool BroadbandTX
        {
            get
            {
                if (m_HamBands == null) return m_btx; // called by JSON constructor
                return m_HamBands.BroadbandTX;
            }
            set
            {
                m_btx = value;
                if (m_HamBands != null)
                {
                    m_HamBands.BroadbandTX = value;
                    m_HamBands.Save();
                }

                if (m_UserBands != null)
                {
                    m_UserBands.BroadbandTX = value;
                    m_UserBands.Save();
                }

                if (m_UserBands != null && m_HamBands != null) // avoid doing it when the settings are loading.
                    this.Save();
            }
        }


        IARURegion m_region = IARURegion.Europe;
        public IARURegion CurrentRegion
        {
            get
            {
                if (m_HamBands == null)
                    return m_region; // called by json
                if (m_HamBands != null)
                    return m_HamBands.CurrentRegion;
                return m_region;
            }
            set
            {

                m_HamBands?.SetRegion(value, true);
                m_UserBands?.SetRegion(value, true);
                m_region = value;
            }
        }
        private BandLimits findBandFreq(double f, List<BandLimits> bands)
        {
            foreach (BandLimits b in bands)
            {
                if (f >= b.MinFreq && f <= b.MaxFreq)
                    return b;
            }
            return null;
        }
        public BandLimits FindBandForFrequency(double f)
        {
            var b = findBandFreq(f, m_UserBands.Bands);
            if (b == null)
            {
                b = findBandFreq(f, m_HamBands.Bands);
            }
            return b;
        }
    }

}
