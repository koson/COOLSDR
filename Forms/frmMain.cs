using CoolSDR.Class;
using CoolSDR.BandsTypes;
using CoolSDR.CustomUserControls;
using MaterialSkin2DotNet;
using MaterialSkin2DotNet.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thetis;
using System.Windows.Navigation;
using CoolComponents;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using ConsoleType = CoolSDR.Forms.FrmMain;

namespace CoolSDR.Forms
{


    public partial class FrmMain : MaterialForm
    {

        public BandsTypes.BandsCollection Bands
        {
            get => this.ucBands1.Bands;

        }

        public VFOControl VFO { get => this.coolVFO1; }


        public SpecRX specRX
        {
            get;
            set;
        }
        public PSForm psform
        {
            get;
            private set;
        }
        public static bool FreqCalibrationRunning;
        private FormPosition MyPositioner;

        private void UpdateStatusLabel()
        {
            var lbl = this.StatusLabel1;
            string powerText = this.PowerIsOn ? "Power is ON" : "Power is OFF";
            string vacText = this.VACEnabled ? "VAC is ENABLED" : "VAC is DISABLED";
            if (this.VACEnabled && this.PowerIsOn)
            {
                vacText = "VAC SR: " + Audio.CurrentStreamInfo.sampleRate.ToString()
                    + "|Input ms:" + ((int)(Audio.CurrentStreamInfo.inputLatency * 1000)).ToString()
                    + "|Output ms: " + ((int)(Audio.CurrentStreamInfo.outputLatency * 1000)).ToString()
                    + "|";
            }
            string radioText = this.CurrentHPSDRModel.ToString();
            string IPText = CurrentRadio.IpAddress.ToString();
            NetworkIO.GetMetisIPAddr();

            lbl.Text = "Radio: " + radioText + "; IP: " + IPText + "; " + vacText + "; " + powerText;

        }

        public System.Windows.Forms.ComboBox comboPreamp
        {
            get
            {
                return this.ComboPreamp;
            }
        }

        public System.Windows.Forms.ComboBox comboMeterTXMode
        {
            get
            {
                return this.ComboMeterTXMode;
            }
        }


        //void waitfor(ref bool which, string what)
        //{

        //    int slept = 0;
        //    int dotcounter = 0;
        //    string progress = what + " ...";
        //    string orig_progress = progress;

        //    while (which)
        //    {
        //        if (slept == 0)
        //        {
        //            ShowLoadProgress(progress);
        //        }
        //        else if (slept % 10 == 0)

        //        {
        //            progress += ".";
        //            dotcounter++;
        //            ShowLoadProgress(progress);
        //            if (dotcounter > 10)
        //            {
        //                dotcounter = 0;
        //                progress = orig_progress;
        //            }

        //        }
        //        Thread.Sleep(1);
        //        Application.DoEvents();
        //        slept += 1;
        //        if (GotClosed)
        //        {
        //            return;
        //        }
        //    }
        //}

        void InitPS()
        {
            if (psform == null)
            {
                psform = new PSForm(this);
            }
        }

        internal System.Windows.Forms.NumericUpDown udRX1StepAttData
        {
            get
            {
                return UDRX1StepAttData;

            }
        }



        void WaitForRadio()
        {

            Debug.Assert(RadioInitTask != null);

            StringBuilder dots = new StringBuilder();
            dots.Append("...");
            while (!RadioInitTask.IsCompleted && !GotClosed)
            {

                Thread.Sleep(15);
                Application.DoEvents();
                if (GotClosed) break;
                dots.Append('.');
                ShowLoadProgress("Waiting for Radio initialisation " + dots.ToString());
                if (dots.Length > 150) { dots.Clear(); dots.Append("..."); }
            }

            //MyGenSettings = new GenericSettings(this, this.radio);
        }

        void WaitForPortAudio()
        {
            Debug.Assert(PAInitTask != null);
            StringBuilder dots = new StringBuilder();
            dots.Append("...");
            while (!PAInitTask.IsCompleted && !GotClosed)
            {

                Thread.Sleep(15);
                Application.DoEvents();
                if (GotClosed) break;
                dots.Append('.');
                ShowLoadProgress("Waiting for PortAudio initialisation " + dots.ToString());
                if (dots.Length > 150) { dots.Clear(); dots.Append("..."); }
            }
        }

        void WaitForDisplay()
        {
            Debug.Assert(this.DrawDisplayTask != null);
            StringBuilder dots = new StringBuilder();
            dots.Append("...");
            while (!Display.dx_running && !GotClosed)
            {
                Thread.Sleep(1);
                Application.DoEvents();
                if (GotClosed) break;
                dots.Append('.');
                ShowLoadProgress("Waiting for Display to be ready " + dots.ToString());
                if (dots.Length > 150) { dots.Clear(); dots.Append("..."); }
            }

        }

        void WaitForStartupTasks()
        {
            WaitForRadio();
            WaitForPortAudio();
            WaitForDisplay();
            if (GotClosed)
            {
                // can we handle user closing whilst we were waiting??
                Debug.Print("Form closed whilst waiting for radio or portaudio");
            }

        }

        internal SDRRadio CurrentRadio
        {
            get; set;
        }

        //fixme: legacy
        internal SDRRadio radio
        {
            get => CurrentRadio; set
            {
                CurrentRadio = value;
            }
        }

        private int sample_rate_tx = 192000;
        public int SampleRateTX
        {
            get { return sample_rate_tx; }
            set
            {
                // fixme
                DSPMode rx1_dsp_mode = DSPMode.FIRST;
                sample_rate_tx = value;
                Audio.SampleRateTX = value;
                Display.SampleRateTX = value;
                cmaster.SetXmtrChannelOutrate(0, value, cmaster.MONMixState);

                switch (rx1_dsp_mode)
                {
                    case DSPMode.SPEC: break;
                }

                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANASCOPE:
                        //Display.CalcTXDisplayFreq();
                        //btnDisplayPanCenter.PerformClick();
                        // Fixme
                        break;
                    case DisplayMode.SPECTRUM:
                    case DisplayMode.HISTOGRAM:
                        // UpdateTXSpectrumDisplayVars(); 
                        break;
                }
            }
        }

        private int block_size1;
        public int BlockSize1
        {
            get { return block_size1; }
            set
            {
                block_size1 = value;
                Audio.BlockSize = value;
            }
        }


        private int block_size_rx2;
        public int BlockSizeRX2
        {
            get { return block_size_rx2; }
            set
            {
                block_size_rx2 = value;
                Audio.BlockSizeRX2 = value;
            }
        }

        // 'RX1 Sample Rate on tphpsdr on setup form in Thetis
        // private void comboAudioSampleRate1_SelectedIndexChanged(
        //         object sender, System.EventArgs e)
        private void ChangeRX1SampleRate(int value)
        {
            //if (comboAudioSampleRate1.SelectedIndex < 0) return;
            var console = this;
            int old_rate = SampleRateRX1;
            int new_rate = value;
            bool was_enabled = console.RX2Enabled; // true;

            var prot = NetworkIO.CurrentRadioProtocol;
            if (prot == RadioProtocol.Auto)
            {
                Debug.Assert(initializing);
                if (radio.Settings.LastSuccessfulProtocol != RadioProtocol.Auto)
                {
                    prot = radio.Settings.LastSuccessfulProtocol;
                }
                else
                {
                    prot = RadioProtocol.Protocol2;
                }
                // we cannot know for sure which one when we are setting up,
                // so this is why we need a call as we power on, after we have
                // connected on the network.

            }

            bool was_running = Audio.Status[0].state || Audio.Status[1].state;

            if (new_rate != old_rate || initializing)
            {
                switch (prot)
                {
                    case RadioProtocol.Protocol2:
                        // turn OFF the DSP channels so they get flushed out (must
                        // do while data is flowing to get slew-down and flush)
                        WDSP.SetChannelState(WDSP.id(0, 1), 0, 0);
                        WDSP.SetChannelState(WDSP.id(0, 0), 0, 1);
                        Thread.Sleep(10);

                        // remove the RX1 main and sub audio streams from the mix
                        // set
                        unsafe
                        {
                            cmaster.SetAAudioMixStates((void*)0, 0, 3, 0);
                        }

                        // disable VAC
                        if (console.PowerOn && console.VACEnabled && !initializing)
                            Audio.EnableVAC1(false);

                        // turn OFF the DDC(s)
                        NetworkIO.EnableRx(0, 0);
                        NetworkIO.EnableRx(1, 0);
                        NetworkIO.EnableRx(2, 0);

                        // wait for all inflight packets to arrive - need to
                        // experiment with this delay value
                        Thread.Sleep(20);

                        // flush any buffers where these old-rate packets may be
                        // stored ... this is done via operations in SetXcmInrate()

                        // in the property SampleRate1:
                        //    (1) the new samplerate will be sent to the DDC
                        //    (2) SetXcmInrate() is called by Audio.SampleRate1
                        //    which is called by console.SampleRate1
                        console.SampleRateRX1 = new_rate;

                        // set the corresponding new buffer size
                        int new_size = cmaster.GetBuffSize(new_rate);
                        console.BlockSize1 = new_size;

                        // set rate and size for the display too; could move this to
                        // SetXcmInrate() in cmaster.c
                        console.specRX.GetSpecRX(0).SampleRate = new_rate;
                        console.specRX.GetSpecRX(0).BlockSize = new_size;

                        if (was_enabled)
                        {
                            // turn on the DDC(s)
                            console.UpdateDDCs(true);
                        }
                        else
                            console.UpdateDDCs(false);
                        // wait for samples at the new rate to be received
                        Thread.Sleep(1);
                        // add the audio streams to the mix set
                        unsafe
                        {
                            cmaster.SetAAudioMixStates((void*)0, 0, 3, 3);
                        }

                        // enable VAC
                        if (console.PowerOn && console.VACEnabled && !initializing)
                            Audio.EnableVAC1(true);

                        // turn ON the DSP channels
                        int w_enable = 0;
                        if (was_enabled) w_enable = 1;
                        WDSP.SetChannelState(WDSP.id(0, 0), w_enable, 0);
                        if (console.radio.GetDSPRX(0, 1).Active)
                            WDSP.SetChannelState(WDSP.id(0, 1), w_enable, 0);

                        // calculate and display the new bin_width
                        double bin_width = (double)new_rate
                            / (double)console.specRX.GetSpecRX(0).FFTSize;
                        // lblDisplayBinWidth.Text = bin_width.ToString("N3");

                        // be sure RX2 sample rate setting is enabled, UNLESS it's a
                        // 10E or 100B
                        if (console.CurrentHPSDRModel == HPSDRModel.ANAN10E
                            || console.CurrentHPSDRModel == HPSDRModel.ANAN100B)
                        {
                            // if it's a 10E/100B, set RX2 sample_rate equal to RX1
                            // rate
                            //comboAudioSampleRateRX2.Enabled = false;
                            // comboAudioSampleRateRX2.SelectedIndex
                            //    = comboAudioSampleRate1.SelectedIndex;
                        }
                        else
                        {
                            //   comboAudioSampleRateRX2.Enabled = true;
                        }

                        break;
                    case RadioProtocol.Protocol1:
                        if (was_running)
                        {
                            // turn OFF the RX DSP channels so they get flushed out
                            // (must do while data is flowing to get slew-down and
                            // flush)
                            WDSP.SetChannelState(3, 0, 0); // RX2_sub
                            WDSP.SetChannelState(2, 0, 0); // RX2_main
                            WDSP.SetChannelState(1, 0, 0); // RX1_sub
                            WDSP.SetChannelState(0, 0, 1); // RX1_main
                            Thread.Sleep(10);

                            // remove the RX1 and RX2 (main and sub) audio streams from
                            // the mix set
                            unsafe
                            {
                                cmaster.SetAAudioMixStates((void*)0, 0, 15, 0);
                            }

                            // disable VAC
                            if (console.PowerOn && console.VACEnabled && !initializing)
                                Audio.EnableVAC1(false);
                            if (console.PowerOn && console.VAC2Enabled && !initializing)
                                Audio.EnableVAC2(false);

                            // turn OFF the DDC(s) [SendStopToMetis]
                            NetworkIO.SendStopToMetis();

                            // wait for all inflight packets to arrive - need to
                            // experiment with this delay value
                            Thread.Sleep(25);

                            // flush any buffers where these old-rate packets may be
                            // stored ... this is done via operations in SetXcmInrate()
                            // in the property SampleRate1:  SetXcmInrate() is called by
                            // Audio.SampleRate1 which is called by console.SampleRate1
                        }
                        console.SampleRateRX1 = new_rate;
                        console.SampleRateRX2 = new_rate;
                        // set protocol_1 network software sample rate
                        NetworkIO.SetDDCRate(0, new_rate);

                        // set the corresponding new buffer size
                        int new_size1 = cmaster.GetBuffSize(new_rate);
                        console.BlockSize1 = new_size1;
                        console.BlockSizeRX2 = new_size1;

                        // set rate and size for the display too; could move this to
                        // SetXcmInrate() in cmaster.c
                        console.specRX.GetSpecRX(0).SampleRate = new_rate;
                        console.specRX.GetSpecRX(0).BlockSize = new_size1;
                        console.specRX.GetSpecRX(1).SampleRate = new_rate;
                        console.specRX.GetSpecRX(1).BlockSize = new_size1;

                        if (was_running)
                        {
                            // turn on the DDC(s) [SendStartToMetis]
                            NetworkIO.SendStartToMetis();
                            // wait for samples at the new rate to be received
                            Thread.Sleep(5);
                        }

                        // add the audio streams to the mix set
                        unsafe
                        {
                            cmaster.SetAAudioMixStates((void*)0, 0, 3, 3);
                        }
                        if (console.RX2Enabled)
                        {
                            unsafe
                            {
                                cmaster.SetAAudioMixState((void*)0, 0, 2, true);
                            }
                        }

                        if (was_running)
                        {
                            // enable VAC
                            if (console.PowerOn && console.VACEnabled && !initializing)
                                Audio.EnableVAC1(true);
                            if (console.PowerOn && console.VAC2Enabled && !initializing)
                                Audio.EnableVAC2(true);

                            // turn ON the DSP channels
                            WDSP.SetChannelState(0, 1, 0); // RX1_main
                            if (console.radio.GetDSPRX(0, 1).Active)
                                WDSP.SetChannelState(1, 1, 0); // RX1_sub
                            if (console.RX2Enabled)
                                WDSP.SetChannelState(2, 1, 0); // RX2_main
                        }

                        // calculate and display the new bin_width
                        double bin_width1 = (double)new_rate
                            / (double)console.specRX.GetSpecRX(0).FFTSize;
                        // lblDisplayBinWidth.Text = bin_width1.ToString("N3");
                        //lblRX2DisplayBinWidth.Text = bin_width1.ToString("N3");

                        // set displayed RX2 rate equal to RX1 Rate
                        // comboAudioSampleRateRX2.Enabled = false;
                        //comboAudioSampleRateRX2.SelectedIndex
                        //    = comboAudioSampleRate1.SelectedIndex;

                        break;
                }
            }
        }

        void InitApp()
        {
            PicDisplay.Enabled = false;
            UseWaitCursor = true;


            ShowLoadProgress("Loading Display ...");
            InitDisplay();


            ShowLoadProgress("Initialising BandsTypes ...");
            ucBands1.Init();
            WaitForStartupTasks();

            if (GotClosed)
            {
                return;
            }

            ChangeRX1SampleRate(192000);
            // channelmaster stuff to align with Thetis:
            this.SampleRateRX1 = 192000;
            int new_size = cmaster.GetBuffSize(192000);
            BlockSize1 = new_size;

            this.SampleRateTX = 192000;

            ShowLoadProgress("Loading PureSignal ...");
            InitPS();


            ShowLoadProgress("Checking network ...");
            try
            {
                Debug.Assert(this.CurrentRadio != null);
                NetworkIO.InitNetwork(this, CurrentRadio);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nPlease check your network, cannot start CoolSDR", "CoolSDR Fatal Error");
                Application.Exit();
                return;
            }


            // if this throws, it is goodnight Vienna
            ucRadio1.Init(this, this.CurrentRadio);


            ShowLoadProgress("Loading PortAudio Devices and APIs ...");
            ucAudio1.Init();


            this.coolVFO1.IVFO = CurrentRadio.Tuning;
            ShowPoweredUp();



            ShowLoadProgress("Ready");
            // Thread.Sleep(250);
            tabMnu.SelectedIndex = AppSettings.MySettings.ActiveTab;
            pnlLoadProgress.Visible = false;
            pnlTop.Enabled = true;
            picDisplay.Enabled = true;
            UseWaitCursor = false;
            tabMnu.UseWaitCursor = false;

            // this.materialTabSelector1.TabIndexChanged += new System.EventHandler(this.mnuTabIndexChanged);
            this.tabMnu.Deselecting += tabMnuDeselecting;
            this.tabMnu.Selecting += tabMnuSelecting;
            this.tabMnu.SelectedIndexChanged += tabMnuSelectedIndexChanged;


            initializing = false;

        }

        public ucRadio UCRadio
        {
            get => this.ucRadio1;
        }
        internal class DummyException : Exception
        {
            internal DummyException(string dummyText = "This is a dummy test exception") : base(dummyText) { }
        }

        public FrmMain()
        {




            try
            {
                Win32.TimeBeginPeriod(0);
                initializing = true;
                InitializeComponent();

                Program.console = this;
                Audio.console = this;
                this.pnlLoadProgress.Visible = true;
                this.pnlLoadProgress.BringToFront();
                this.PicDisplay.Resize += this.picDisplayResize;


                this.StatusLabel1.Text = "Please wait while " + App.Name + " starts up...";
                // Create a material theme manager and add the form to manage (this)
                MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
                materialSkinManager.AddFormToManage(this);
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

                // Configure color schema
                materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800,
                   Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);


                theConsole = this;
                Common.console = this;
                this.Text = "CoolSDR, built on: " + Thetis.TitleBar.BuildDate().ToString();

                pbLoading.ForeColor = this.SkinManager.ColorScheme.AccentColor;
                pbLoading.ForeColor = pbLoading.ForeColor;

                TryLoadAppSettings();
                ShowLoadProgress("Initiating PortAudio startup and Radio creation ...");

                if (!GotClosed)
                {

                    Task t = Task.Factory.StartNew(() =>
                    {
                        Thread.CurrentThread.Name = "InitPortAudioThread";
                        PortAudioForCoolSDR.PA_Initialize();
                    });
                    PAInitTask = t;

                }

                MyPositioner = new FormPosition(this);
                chkPower.CheckedChanged += this.chkPowerCheckedChanged;
                this.Visible = false;

                coolVFO1.Font = new Font("Arial", 28, FontStyle.Regular);




            }
            catch (Exception e)
            {
                Common.LogException(e, true);
            }
            finally
            {
                Win32.TimeEndPeriod(0);
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
        }

        private void TryLoadAppSettings()
        {
            try
            {
                AppSettings.MySettingsCreate(App.GetDataFolder());
            }
            catch (Exception e)
            {
                Common.LogException(e);
                var fp = AppSettings.LastFilePathRequest;
                Common.LogString("Could not load AppSettings, so deleting file:\n" +
                   fp + "\nand trying again...");
                if (File.Exists(fp))
                {
                    File.Delete(fp);
                    try
                    {
                        // and now if it fails, it must be all over.
                        AppSettings.MySettingsCreate(App.GetDataFolder());
                    }
                    catch (Exception bailEx)
                    {
                        Common.LogException(bailEx, true, "Fatal: Cannot create Application Settings file");
                        Application.Exit();
                    }


                }
                else
                {
                    Exception ex = new Exception("Impossible to create a settings object, from file: "
                        + fp + "\n\nThis error is fatal\n" + e.Message);
                    Common.LogException(e, true);
                    throw ex;
                }
            }
        }

        private void UpdateRXDisplayVars(int l, int h)
        {
            if (Display.CurrentDisplayMode != DisplayMode.SPECTRUM
                && Display.CurrentDisplayMode != DisplayMode.HISTOGRAM
                && Display.CurrentDisplayMode != DisplayMode.SPECTRASCOPE)
                return;
            int low = 0, high = 0;
            const int extra = 1000;
            const int little_extra = 500;
            int spec_blocksize = CurrentRadio.GetDSPRX(0, 0).BufferSize;
            var rx1_dsp_mode = this.RX1DSPMode;
            switch (rx1_dsp_mode)
            {
                case DSPMode.LSB:
                    low = l - extra;
                    high = 0;
                    break;
                case DSPMode.CWL:
                case DSPMode.DIGL:
                    low = l - little_extra;
                    high = 0;
                    break;
                case DSPMode.USB:
                    low = 0;
                    high = h + extra;
                    break;
                case DSPMode.CWU:
                case DSPMode.DIGU:
                    low = 0;
                    high = h + little_extra;
                    break;
                default:
                    low = l - extra;
                    high = h + extra;
                    break;
            }

            Display.RXSpectrumDisplayLow = low;
            Display.RXSpectrumDisplayHigh = high;
            specRX.GetSpecRX(0).CalcSpectrum(low, high, spec_blocksize, 48000);
        }

        public void SelectRX1VarFilter(int high, int low)
        {
            if (rx1_filter == Filter.VAR1) return;
            if (rx1_filter == Filter.VAR2) return;

            // save current filter bounds, reset to var, set filter bounds
            //int high = (int)udFilterHighValue;
            //int low = (int)udFilterLowValue;
            radFilterVar1Checked = true;
            // SetFilter(Filter.VAR1);
            UpdateRX1Filters(low, high);
        }

        public void SelectRX2VarFilter()
        {
            if (rx2_filter == Filter.VAR1) return;
            if (rx2_filter == Filter.VAR2) return;

            // save current filter bounds, reset to var, set filter bounds
            int high = (int)udRX2FilterHighValue;
            int low = (int)udRX2FilterLowValue;
            radRX2FilterVar1Checked = true;
            // SetFilter(Filter.VAR1);
            UpdateRX2Filters(low, high);
        }

        public void UpdateRX1Filters(int low, int high)
        {
            var rx1_dsp_mode = this.RX1DSPMode;
            switch (rx1_dsp_mode)
            {
                case DSPMode.LSB:
                case DSPMode.DIGL:
                case DSPMode.CWL:
                    if (low > high - 10) low = high - 10;
                    break;
                case DSPMode.USB:
                case DSPMode.DIGU:
                case DSPMode.CWU:
                    if (high < low + 10) high = low + 10;
                    break;
                case DSPMode.AM:
                case DSPMode.SAM:
                case DSPMode.DSB:
                    if (high < low + 20)
                    {
                        if (Math.Abs(high) < Math.Abs(low))
                            high = low + 20;
                        else
                            low = high - 20;
                    }
                    break;
                case DSPMode.FM:
                    if (CurrentRadio.GetDSPTX(0).TXFMDeviation == 5000)
                    {
                        low = -8000;
                        high = 8000;
                        lblFilterLabelText = "16k";
                    }
                    else if (CurrentRadio.GetDSPTX(0).TXFMDeviation == 2500)
                    {
                        low = -4000;
                        high = 4000;
                        lblFilterLabelText = "8k";
                    }
                    break;
            }

            if (low < -9999) low = -9999;
            if (high > 9999) high = 9999;
            if (high < -9999) high = -9999;
            if (low > 9999) low = 9999;

            // send the settings to the DSP
            if (low == high) return;

            {
                CurrentRadio.GetDSPRX(0, 0).SetRXFilter(low, high);
                CurrentRadio.GetDSPRX(0, 1).SetRXFilter(low, high);
            }

            // send the setting to the display
            Display.RX1FilterLow = low;
            Display.RX1FilterHigh = high;

            // update var filter controls
            udFilterLowValue = low;
            udFilterHighValue = high;

            // update Filter Shift
            ptbFilterShiftUpdate(low, high);

            // update Filter Width
            ptbFilterWidthUpdate(low, high);

            // Update Display data if not in panadapter mode
            if (!CurrentRadio.GetDSPRX(0, 0).SpectrumPreFilter)
                UpdateRXDisplayVars(low, high);

            // update display
            // if (!chkPower.Checked)
            //   Display.DrawBackground();

            // reset average and peak
            switch (Display.CurrentDisplayMode)
            {
                case DisplayMode.SPECTRUM:
                case DisplayMode.HISTOGRAM:
                case DisplayMode.SPECTRASCOPE:
                case DisplayMode.PANADAPTER:
                case DisplayMode.WATERFALL:
                case DisplayMode.PANAFALL:
                    if (chkDisplayAVGChecked) Display.ResetRX1DisplayAverage();
                    if (chkDisplayPeakChecked) Display.ResetRX1DisplayPeak();
                    break;
            }

            // set XIT step rate
            if ((high - low) > 250)
            {
                udXITIncrement = 10;
                udRITIncrement = 10;
            }
            else
            {
                udXITIncrement = 5;
                udRITIncrement = 5;
            }

            /*/
            if (filterRX1Form != null && !filterRX1Form.IsDisposed)
            {
                if (filterRX1Form.DSPMode == rx1_dsp_mode)
                    filterRX1Form.CurrentFilter = rx1_filter;
            }
            /*/
        }

        private void ptbFilterWidthUpdate(int low, int high)
        {
            throw new NotImplementedException();
        }

        private void ptbFilterShiftUpdate(int low, int high)
        {
            throw new NotImplementedException();
        }

        public void UpdateRX2Filters(int low, int high)
        {
            var rx2_dsp_mode = this.RX2DSPMode;
            switch (rx2_dsp_mode)
            {
                case DSPMode.LSB:
                case DSPMode.DIGL:
                case DSPMode.CWL:
                    if (low > high - 10) low = high - 10;
                    break;
                case DSPMode.USB:
                case DSPMode.DIGU:
                case DSPMode.CWU:
                    if (high < low + 10) high = low + 10;
                    break;
                case DSPMode.AM:
                case DSPMode.SAM:
                case DSPMode.DSB:
                    if (high < low + 20)
                    {
                        if (Math.Abs(high) < Math.Abs(low))
                            high = low + 20;
                        else
                            low = high - 20;
                    }
                    break;
                case DSPMode.FM:
                    if (CurrentRadio.GetDSPTX(0).TXFMDeviation == 5000)
                    {
                        low = -8000;
                        high = 8000;
                    }
                    else if (CurrentRadio.GetDSPTX(0).TXFMDeviation == 2500)
                    {
                        low = -4000;
                        high = 4000;
                    }
                    break;
            }

            if (low < -9999) low = -9999;
            if (high > 9999) high = 9999;
            if (high < -9999) high = -9999;
            if (low > 9999) low = 9999;
            // if (low < -14999)
            //   low = -14999;
            // if (high > 14999)
            //  high = 14999;
            // if (low == high) high = low + 25;
            // System.Console.WriteLine("updf lo: " + low + " hi: " + high);

            // send the settings to the DSP
            if (low == high) return;

            // send the settings to the DSP
            radio.GetDSPRX(1, 0).SetRXFilter(low, high);
            radio.GetDSPRX(1, 1).SetRXFilter(low, high);

            // send the setting to the display
            Display.RX2FilterLow = low;
            Display.RX2FilterHigh = high;

            // update var filter controls
            udRX2FilterLowValue = low;
            udRX2FilterHighValue = high;

            // update display
            //  if (!chkPower.Checked)
            //   Display.DrawBackground();
            /*/
            if (filterRX2Form != null && !filterRX2Form.IsDisposed)
            {
                if (filterRX2Form.DSPMode == rx2_dsp_mode)
                    filterRX2Form.CurrentFilter = rx2_filter;
            }
            /*/
        }

        private Filter rx1_filter = Filter.FIRST;


        private DisplayHelper m_displayHelper;
        public DisplayHelper DisplayHelper
        {
            get
            {
                return m_displayHelper;
            }
        }

        public MaterialSlider ptbPWR
        {
            get { return sldPower; }
        }

        private bool amp_protect = false;
        public bool AmpProtect
        {
            get { return amp_protect; }
            set { amp_protect = value; }
        }

        private void picDisplay_Click
            (object sender, EventArgs e)
        {

        }

        public PictureBox PicDisplay
        {
            get { return picDisplay; }
        }

        public bool PowerOn
        {
            get { return chkPower.Checked; }
            set { chkPower.Checked = value; }
        }

        public bool PowerIsOn
        {
            get => chkPower.Checked;
        }
        public bool RX2Enabled { get; set; } = false;

        public int RX1DisplayGridX
        {
            get;
            set;
        }

        public int RX1DisplayGridW
        {
            get;
            set;
        }
        public int RX2DisplayGridX
        {
            get;
            set;
        }

        public int RX2DisplayGridW
        {
            get;
            set;
        }
        public FRSRegion CurrentRegion
        {
            get;
            set;
        }



        public bool VFOSync
        {
            get;
            set;
        }


        public double VFOBFreq
        {
            get;
            set;
        }


        private double ModeFreqOffset(DSPMode mode)
        {
            switch (mode)
            {
                case DSPMode.LSB:
                case DSPMode.DIGL: return (1500 * 1e-6);
                case DSPMode.USB:
                case DSPMode.DRM:
                case DSPMode.DIGU: return (-1500 * 1e-6);
                case DSPMode.DSB:
                case DSPMode.CWL:
                case DSPMode.CWU:
                case DSPMode.FM:
                case DSPMode.AM:
                case DSPMode.SAM: return 0;
                default: return 0;
            }
        }

        public DSPMode RX1DSPMode { get; set; } = DSPMode.LSB;

        public bool RX1IsIn60m()
        {
            double freq = VFOAFreq;
            return (freq >= 5.1 && freq <= 5.5);
        }

        public bool RX1IsOn60mChannel(Channel c)
        {
            var rx1_dsp_mode = RX1DSPMode;
            double freq = VFOAFreq - ModeFreqOffset(rx1_dsp_mode);
            freq = Math.Round(freq, 6); // in mhz

            return (c.Freq == freq);
        }
        public bool RX1IsIn60mChannel(Channel c)
        {
            double freq = VFOAFreq;
            freq = Math.Round(freq, 6); // in mhz

            return (freq >= c.Low) && (freq <= c.High);
        }

        public bool RX1IsIn60mChannel()
        {
            foreach (Channel c in Channels60m)
            {
                if (RX1IsIn60mChannel(c)) return true;
            }

            return false; // nothing matched, return false
        }
        public bool RX1IsOn60mChannel()
        {
            foreach (Channel c in Channels60m)
            {
                if (RX1IsOn60mChannel(c)) return true;
            }

            return false; // nothing matched, return false
        }

        public bool VFOATX { get; set; } = true;
        public bool VFOBTX { get; set; } = false;
        public bool FullDuplex { get; set; }

        public void UpdateTXDDSFreq(double f = -1)
        {
            CurrentRadio.Tuning.UpdateTXDDSFreq(f);
        }


        private static List<Channel> channels_60m = new List<Channel>();
        public static List<Channel> Channels60m
        {
            get { return channels_60m; }
        }

        public bool RX2IsOn60mChannel(Channel c)
        {
            var rx2_dsp_mode = RX2DSPMode;
            double freq = VFOBFreq - ModeFreqOffset(rx2_dsp_mode);
            freq = Math.Round(freq, 6); // in mhz

            return (c.Freq == freq);
        }
        public bool RX2IsIn60mChannel(Channel c)
        {
            double freq = VFOBFreq;
            freq = Math.Round(freq, 6);

            return (freq >= c.Low) && (freq <= c.High);
        }
        public bool RX2IsOn60mChannel()
        {
            foreach (Channel c in Channels60m)
            {
                if (RX2IsOn60mChannel(c)) return true;
            }

            return false; // nothing matched, return false
        }
        public bool RX2IsIn60mChannel()
        {
            foreach (Channel c in Channels60m)
            {
                if (RX2IsIn60mChannel(c)) return true;
            }

            return false; // nothing matched, return false
        }

        AGCMode m_RX1agcMode;
        public AGCMode RX1AGCMode
        {
            get { return m_RX1agcMode; }
            set
            {
                m_RX1agcMode = value;
                //comboAGC.SelectedIndex = (int)value;
                //lblAGCLabel.Text = "AGC: " + comboAGC.Text;
            }
        }

        AGCMode m_RX2agcMode;
        public AGCMode RX2AGCMode
        {
            get { return m_RX2agcMode; }
            set
            {
                m_RX2agcMode = value;
                //comboAGC.SelectedIndex = (int)value;
                //lblAGCLabel.Text = "AGC: " + comboAGC.Text;
            }
        }

        public int AGCFixedGain
        {
            get;
            set;
            /*/
            get
            {
                if (udDSPAGCFixedGaindB != null)
                    return (int)udDSPAGCFixedGaindB.Value;
                else
                    return -1;
            }
            set
            {
                if (udDSPAGCFixedGaindB != null) udDSPAGCFixedGaindB.Value = value;
            }
            /*/
        }

        public int AGCRX2MaxGain
        {
            get; set;
            /*/
            get
            {
                if (udDSPAGCRX2MaxGaindB != null)
                    return (int)udDSPAGCRX2MaxGaindB.Value;
                else
                    return -1;
            }
            set
            {
                if (udDSPAGCRX2MaxGaindB != null)
                    udDSPAGCRX2MaxGaindB.Value = value;
            }
            /*/
        }
        public int AGCRX2FixedGain { get; internal set; }
        public int DisplayFPS { get; internal set; } = 30;
        public System.Drawing.Image PicDisplayBackgroundImage { get; internal set; }
        public bool MOX { get; internal set; }

        public HPSDRModel CurrentHPSDRModel
        {
            get => radio.CurrentRadio;
            set => radio.CurrentRadio = value;

        }
        private bool alexpresent = true;
        public bool AlexPresent
        {
            get { return alexpresent; }
        }
        public bool disable_hpf_on_tx = false;

        private double fwc_dds_freq = 14;
        private double vfo_offset = 0.0;
        public double FWCDDSFreq
        {
            get { return fwc_dds_freq; }
            set
            {
                fwc_dds_freq = value;

                double f = fwc_dds_freq + vfo_offset;
                VFO.FreqManager.FreqInMHz = f;
            }
        }



        public bool VAC2Enabled { get; internal set; }
        public bool PSState { get; internal set; }
        private bool m_bAttontx = true;
        public bool ATTOnTX
        {
            get { return m_bAttontx; }
            set
            {
                m_bAttontx = value;

                if (PowerOn)
                {
                    if (m_bAttontx)
                        NetworkIO.SetTxAttenData(
                            this.CurrentRadio.tx_step_attenuator_by_band[(int)CurrentRadio.Tuning.rx1_band]);
                    else
                        NetworkIO.SetTxAttenData(0);
                }
            }
        }

        public int ATTOONTX
        {
            /*/
            get
            {

                if (udATTOnTX != null)
                    return (int)udATTOnTX.Value;
                else
                    return -1;
                          }
            set
            {

                if (udATTOnTX != null)
                {
                    if (value > 31) value = 31;
                    udATTOnTX.Value = value;
                }

            }
            /*/
            get; set;

        }
        public bool IsSetupFormNull { get; internal set; }
        public bool ATTOnTXChecked { get; internal set; }
        public bool TTgenrun { get; internal set; }

        private static FrmMain theConsole = null;
        internal readonly bool MicBoost = false;

        public static FrmMain getConsole()
        {

            return theConsole;
        }

        private float rx1_display_cal_offset; // display calibration offset
                                              // per volume setting in dB
        public float RX1DisplayCalOffset
        {
            get { return rx1_display_cal_offset; }
            set
            {
                rx1_display_cal_offset = value;
                // RX2DisplayCalOffset = value;
            }
        }


        private bool DataFlowing = false;
        unsafe internal void UpdateAAudioMixerStates()
        {
            int RX1 = 1 << WDSP.id(0, 0);
            int RX1S = 1 << WDSP.id(0, 1);
            int RX2 = 1 << WDSP.id(2, 0);
            int MON = 1 << WDSP.id(1, 0);
            int RX2EN;
            if (rx2_enabled)
                RX2EN = 1 << WDSP.id(2, 0);
            else
                RX2EN = 0;

            var diversity2 = false;

            // DDCs
            if (chkPower.Checked)
            {
                if (!mox)
                {

                    if (!diversity2)
                    {
                        if (!psform.PSEnabled)
                        {
                            cmaster.SetAAudioMixStates((void*)0, 0,
                                RX1 + RX1S + RX2 + MON,
                                RX1 + RX1S + RX2EN + MON);
                            cmaster.SetAntiVOXSourceStates(
                                0, RX1 + RX1S + RX2, RX1 + RX1S + RX2EN);
                        }
                        else
                        {
                            cmaster.SetAAudioMixStates((void*)0, 0,
                                RX1 + RX1S + RX2 + MON,
                                RX1 + RX1S + RX2EN + MON);
                            cmaster.SetAntiVOXSourceStates(
                                0, RX1 + RX1S + RX2, RX1 + RX1S + RX2EN);
                        }
                    }
                    else
                    {
                        if (!psform.PSEnabled)
                        {
                            cmaster.SetAAudioMixStates((void*)0, 0,
                                RX1 + RX1S + RX2 + MON, RX1 + RX1S + MON);
                            cmaster.SetAntiVOXSourceStates(
                                0, RX1 + RX1S + RX2, RX1 + RX1S);
                        }
                        else
                        {
                            cmaster.SetAAudioMixStates((void*)0, 0,
                                RX1 + RX1S + RX2 + MON, RX1 + RX1S + MON);
                            cmaster.SetAntiVOXSourceStates(
                                0, RX1 + RX1S + RX2, RX1 + RX1S);
                        }
                    }
                }
                else
                {
                    if (!diversity2)
                    {
                        if (!psform.PSEnabled)
                        {
                            cmaster.SetAAudioMixStates((void*)0, 0,
                                RX1 + RX1S + RX2 + MON,
                                RX1 + RX1S + RX2EN + MON);
                            cmaster.SetAntiVOXSourceStates(
                                0, RX1 + RX1S + RX2, RX1 + RX1S + RX2EN);
                        }
                        else
                        {
                            cmaster.SetAAudioMixStates(
                                (void*)0, 0, RX1 + RX1S + RX2 + MON, MON);
                            cmaster.SetAntiVOXSourceStates(
                                0, RX1 + RX1S + RX2, 0);
                        }
                    }
                    else
                    {
                        if (!psform.PSEnabled)
                        {
                            cmaster.SetAAudioMixStates((void*)0, 0,
                                RX1 + RX1S + RX2 + MON, RX1 + RX1S + MON);
                            cmaster.SetAntiVOXSourceStates(
                                0, RX1 + RX1S + RX2, RX1 + RX1S);
                        }
                        else
                        {
                            cmaster.SetAAudioMixStates(
                                (void*)0, 0, RX1 + RX1S + RX2 + MON, MON);
                            cmaster.SetAntiVOXSourceStates(
                                0, RX1 + RX1S + RX2, 0);
                        }
                    }
                }
                cmaster.MONMixState = true;
            }
            else
            {
                cmaster.MONMixState = false;
                cmaster.SetAAudioMixStates(
                    (void*)0, 0, RX1 + RX1S + RX2 + MON, 0);
                cmaster.SetAntiVOXSourceStates(0, RX1 + RX1S + RX2, 0);
            }

        }

        internal void ForcePureSignalAutoCalDisable()
        {
            throw new NotImplementedException();
        }

        public void InfoBarFeedbackLevel(int level, bool bFeedbackLevelOk,
        bool bCorrectionsBeingApplied, bool bCalibrationAttemptsChanged, Color feedbackColour)
        {
            throw new NotImplementedException();
        }

        private Color button_selected_color = Color.Yellow;
        private Color rx1_band_color = Color.Yellow;

        public float[] rx1_preamp_offset; // offset values for each preamp mode in
                                          // dB
        public float[] rx2_preamp_offset; // offset values for each preamp mode in
                                          // dB
        public float rx1_meter_cal_offset; // multimeter calibration offset per
                                           // volume setting in dB
        public float rx2_meter_cal_offset; // multimeter calibration offset per
                                           // volume setting in dB
        public float[] rx_meter_cal_offset_by_radio;
        public float[] rx_display_cal_offset_by_radio;





        public Color ButtonSelectedColor
        {
            get { return button_selected_color; }
            set
            {
                button_selected_color = value;
                rx1_band_color = button_selected_color;
                // CheckSelectedButtonColor();
            }
        }

        public bool BypassVACWhenPlayingRecording { get; internal set; }
        public bool WavePlayback { get; internal set; }
        public DSPMode RX2DSPMode { get; internal set; } = DSPMode.LSB;

        private string m_sAppDataPath;
        public string AppDataPath
        {
            get
            {
                if (String.IsNullOrEmpty(m_sAppDataPath))
                {
                    m_sAppDataPath = App.GetDataFolder();

                }
                return m_sAppDataPath;
            }
            internal set
            {
                if (string.IsNullOrEmpty(value))
                    m_sAppDataPath = App.GetDataFolder();
                else
                    m_sAppDataPath = value;

                Debug.Assert(Directory.Exists(m_sAppDataPath));
            }

        }
        public bool TXEQ { get; internal set; }
        public bool CPDR { get; internal set; }
        public bool DX { get; internal set; }
        public bool CFCEnabled { get; internal set; }
        public bool PhaseRotEnabled { get; internal set; }
        public bool MON { get; internal set; }
        public bool QuickPlay { get; internal set; }
        public Color TxtLeftForeColor { get; internal set; }
        public Color TxtRightBackColor { get; internal set; }
        public Color TxtCenterBackColor { get; internal set; }
        public int ATTONTX { get; internal set; }
        public int RX1XVTRIndex { get; internal set; }
        public string HPSDRNetworkIPAddr
        {
            get => radio.IpAddress;
        }
        // public int band_60m_register { get; private set; }

        private bool Shownb4 { get; set; }
        public bool display_duplex { get; internal set; }
        public bool RIT { get; internal set; }
        public int RITValue { get; internal set; }
        public bool XIT { get; internal set; }
        public int XITValue { get; internal set; }
        public double cw_pitch { get; internal set; }
        public double center_rx2_frequency { get; internal set; }
        public bool EnableMultiRX { get; internal set; }
        public int FilterLowValue { get; internal set; }
        public int FilterHighValue { get; internal set; }
        public double VFOASubFreq { get; internal set; }
        public bool SplitDisplay { get; internal set; }
        public int TXFilterLow { get; internal set; }
        public int TXFilterHigh { get; internal set; }
        public bool VFOSplit { get; internal set; }
        public ClickTuneMode CurrentClickTuneMode { get; internal set; }

        protected override void OnVisibleChanged(EventArgs e)
        {
            Debug.Print("OnVisChanged, Visible " + base.Visible);
            base.OnVisibleChanged(e);
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            if (!Shownb4)
            {
                Shownb4 = true;
                VFO.Font = new Font("Arial", 24);
                Refresh();
                //Application.DoEvents();
            }
        }

        private List<TuneStep> tune_step_list = new List<TuneStep>(); // A list of available tuning steps
        public List<TuneStep> TuneStepList
        {
            get { return tune_step_list; }
        }

        public string txtVFOBFreq { get; internal set; }
        public float digl_click_tune_offset { get; internal set; }
        public float digu_click_tune_offset { get; internal set; }
        public bool snap_to_click_tuning { get; internal set; }
        public int CurrentTuneStepHz { get; internal set; }



        const double DEFAULT_FREQ = 0.648;
        // private double m_VFOAFreq = DEFAULT_FREQ;
        public double VFOAFreq
        {
            get { return coolVFO1.ValueInMhz; }
            set
            {

                var v =
                    (long)((double)value *
                    (double)CoolComponents.FrequencyManager.MEGA);
                coolVFO1.FreqManager.FreqInHz = v;
                UpdateVFOASub();
            }
        }
        public string txtVFOAFreq { get => this.coolVFO1.Value.ToString(); internal set => this.coolVFO1.Value = long.Parse(value); }
        public bool ClickTuneFilter { get; internal set; }
        public bool PureSignalEnabled { get; internal set; }
        public int ptbSquelchMaximum { get; internal set; }
        public object ptbSquelch { get; internal set; }
        public int ptbSquelchMinimum { get; internal set; }
        public int ptbSquelchValue { get; internal set; }
        public int ptbNoiseGateMaximum { get; internal set; }
        public int ptbNoiseGateMinimum { get; internal set; }
        public int ptbNoiseGateValue { get; internal set; }
        public string lblDisplayCursorOffset { get; internal set; }
        public string lblDisplayCursorPower { get; internal set; }
        public string lblDisplayCursorFreq { get; internal set; }
        public int AGCMaxGain { get; internal set; }
        public int SetAGCRX2HangThreshold { get; internal set; }
        public int AGCHangThreshold { get; internal set; }
        private int sample_rate_rx1 = 48000;

        public void CalcDisplayFreq()
        {
            if (Display.CurrentDisplayMode != DisplayMode.PANADAPTER
                && Display.CurrentDisplayMode != DisplayMode.WATERFALL
                && Display.CurrentDisplayMode != DisplayMode.PANAFALL
                && Display.CurrentDisplayMode != DisplayMode.PANASCOPE)
                return;

            if (!initializing) specRX.GetSpecRX(0).initAnalyzer();
            if (current_display_engine == DisplayEngine.GDI_PLUS)
            {
                picDisplay.Invalidate();
            }
            // else
            //{
            //    Display.RefreshPanadapterGrid = true;
            //}
        }
        public int SampleRateRX1
        {
            get { return sample_rate_rx1; }
            set
            {
                sample_rate_rx1 = value;
                RadioDSP.SampleRate = value;
                Audio.SampleRate1 = value;
                Display.SampleRateRX1 = value;
                var rx1_dsp_mode = RX1DSPMode;
                switch (rx1_dsp_mode)
                {
                    case DSPMode.SPEC: //MyGenSettings.SetRX1Mode(DSPMode.SPEC); break;
                        Debug.Assert(false);
                        break;
                }

                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANASCOPE:
                        CalcDisplayFreq();
                        // btnDisplayPanCenter.PerformClick();
                        break;
                    case DisplayMode.SPECTRUM:
                    case DisplayMode.HISTOGRAM: break;//UpdateRXSpectrumDisplayVars(); break;
                }
            }
        }
        private int sample_rate_rx2 = 48000;
        public int SampleRateRX2
        {
            get { return sample_rate_rx2; }
            set
            {
                sample_rate_rx2 = value;

                Audio.SampleRateRX2 = value;
                Display.SampleRateRX2 = value;
                // fixme
                DSPMode rx1_dsp_mode = DSPMode.FIRST;

                switch (rx1_dsp_mode)
                {
                    case DSPMode.SPEC:
                        // SetRX1Mode(DSPMode.SPEC); 
                        break;
                }

                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANASCOPE:

                        // CalcRX2DisplayFreq();
                        // btnDisplayPanCenter.PerformClick();
                        break;
                    case DisplayMode.SPECTRUM:
                    case DisplayMode.HISTOGRAM:
                        //UpdateRXSpectrumDisplayVars(); 
                        break;
                }
            }
        }
        public int udFilterHighValue { get; private set; }
        public int udFilterLowValue { get; private set; }
        public bool radFilterVar1Checked { get; private set; }
        public string lblFilterLabelText { get; private set; }
        public bool chkDisplayAVGChecked { get; private set; }
        public bool chkDisplayPeakChecked { get; private set; }
        public int udXITIncrement { get; private set; }
        public int udRITIncrement { get; private set; }
        public Filter rx2_filter { get; private set; }
        public int udRX2FilterHighValue { get; private set; }
        public int udRX2FilterLowValue { get; private set; }
        public bool radRX2FilterVar1Checked { get; private set; }
        public bool VFOBTXChecked { get; internal set; }
        public double CenterFrequency { get; internal set; }
        public double CenterRX2Frequency { get; internal set; }
        public Task DrawDisplayTask { get; private set; }


        internal void ChangeTuneStepUp()
        {
            tune_step_index = (tune_step_index + 1) % tune_step_list.Count;
            //txtWheelTune.Text = tune_step_list[tune_step_index].Name;
            //lblStepValue.Text = txtWheelTune.Text;
        }

        internal void ChangeTuneStepDown()
        {
            tune_step_index = (tune_step_index - 1 + tune_step_list.Count)
                % tune_step_list.Count;
            //txtWheelTune.Text = tune_step_list[tune_step_index].Name;
            //lblStepValue.Text = txtWheelTune.Text;
        }


        private void picDisplay_Paint(object sender, PaintEventArgs e)
        {
            switch (current_display_engine)
            {
                case DisplayEngine.GDI_PLUS: Display.RenderGDIPlus(ref e); break;
                case DisplayEngine.DIRECT_X: break;
            }
        }


        private int tune_step_index;
        private DisplayEngine current_display_engine = DisplayEngine.DIRECT_X;
        // private ThreadPriority m_tpDisplayThreadPriority = ThreadPriority.Normal;
        // private bool m_bValidatingFreq = false;

        private void picDisplay_MouseDown
            (object sender, MouseEventArgs e)
        {
            m_displayHelper.picDisplayMouseDown(sender, e);
        }



        internal void setPSOnOff(object value)
        {
            throw new NotImplementedException();
        }

        private void picDisplay_DoubleClick(object sender, EventArgs e)
        {
            DisplayHelper.picDisplayDoubleClick(sender, e);
        }

        private void picDisplay_MouseLeave(object sender, EventArgs e)
        {
            DisplayHelper.picDisplayMouseLeave(sender, e);
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            DisplayHelper.picDisplayMouseMove(sender, e);
        }

        private void picDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            DisplayHelper.picDisplayMouseUp(sender, e);
        }

        void ShowLoadProgress(string s)
        {
            this.lblLoadProgress.Text = s;
            lblLoadProgress.Refresh();
        }
        private void frmMain_Shown(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            while (Opacity < 1 && !GotClosed)
            {
                Opacity += 0.02;
                Thread.Sleep(1);
                Application.DoEvents();
            }
            if (GotClosed) return;
            try
            {
                using (Process p = Process.GetCurrentProcess())
                    p.PriorityClass = ProcessPriorityClass.High;

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal; // disturb the audio as little as possible
            }
            catch (Exception ex)
            {
                Common.LogException(ex, false, "Failed hoiking process priority");

            }

            InitApp();
            this.Cursor = Cursors.Default;

        }


        void InitDisplay()
        {
            FrmMain console = this;
            ShowLoadProgress("Starting display ...");
            m_displayHelper = new DisplayHelper();
            Display.console = this;
            Display.Target = this.picDisplay;
            Display.InitDX2D();
            Display.Target = this.picDisplay; // yes, I know, twice: Chicken and egg here
            DisplayHelper.CurrentDisplayEngine = DisplayEngine.DIRECT_X;

            // orig from samplerate combo selection changed
            console.specRX.GetSpecRX(0).BlockSize = 256;


            DrawDisplayTask = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "DisplayThread";
                DisplayHelper.RunDisplay();
            });


        }


        private void chkPowerCheckedChanged(object sender, EventArgs e)
        {
            var old_cursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            PowerOnOff();
            coolVFO1.Value = 648000;
            this.Cursor = old_cursor;
        }

        void PowerOnOff()
        {
            bool on = chkPower.Checked;
            if (on) DoPowerOn();
            else
                DoPowerOff();
        }
        public HPSDRHW CurrentHPSDRHardware { get => radio.CurrentHPSDRHardware; set => radio.CurrentHPSDRHardware = value; }


        public bool initializing { get; set; }
        public bool mox { get; set; }
        bool rx2_enabled { get; set; }

        private int rx_adc_ctrl1 = 4;
        public int RXADCCtrl1
        {
            get { return rx_adc_ctrl1; }
            set
            {
                rx_adc_ctrl1 = value;
                // UpdateRXADCCtrl();
                UpdateDDCs(rx2_enabled);
            }
        }

        private int rx_adc_ctrl2 = 0;
        public int RXADCCtrl2
        {
            get { return rx_adc_ctrl2; }
            set
            {
                rx_adc_ctrl2 = value;
                // UpdateRXADCCtrl();
                UpdateDDCs(rx2_enabled);
            }
        }

        // Diversity operation is on RX1; therefore, the 'rx1_rate' will be
        // used as the diversity rate;
        public void UpdateDDCs(bool rx2_enabled)
        {
            if (initializing) return;
            int DDCEnable = 0;
            int DDC0 = 1, DDC1 = 2, DDC2 = 4, DDC3 = 8;
            int SyncEnable = 0;
            int[] Rate = new int[8];
            int i;
            int rx1_rate = SampleRateRX1;
            int rx2_rate = SampleRateRX2;
            int ps_rate = cmaster.PSrate;
            int P1_DDCConfig = 0;
            int P1_diversity = 0;
            int P1_rxcount = 0;
            int nddc = 0;
            int cntrl1 = 0;
            int cntrl2 = 0;
            bool diversity2 = false;
            if (diversity2) P1_diversity = 1;

            switch (CurrentRadio.Tuning.CurrentHPSDRModel)
            {
                case HPSDRModel.ANAN100D:
                case HPSDRModel.ANAN200D:
                case HPSDRModel.ORIONMKII:
                case HPSDRModel.ANAN7000D:
                case HPSDRModel.ANAN8000D:
                    P1_rxcount = 5; // RX5 used for puresignal feedback
                    nddc = 5;
                    if (!mox)
                    {
                        if (diversity2)
                        {
                            P1_DDCConfig = DDCEnable = DDC0;
                            SyncEnable = DDC1;
                            Rate[0] = rx1_rate;
                            Rate[1] = rx1_rate;
                            cntrl1 = rx_adc_ctrl1 & 0xff;
                            cntrl2 = rx_adc_ctrl2 & 0x3f;
                        }
                        else
                        {
                            P1_DDCConfig = 1;
                            DDCEnable = DDC2;
                            SyncEnable = 0;
                            // Rate[0] = rx1_rate;
                            // Rate[1] = rx2_rate;
                            Rate[2] = rx1_rate;
                            cntrl1 = rx_adc_ctrl1 & 0xff;
                            cntrl2 = rx_adc_ctrl2 & 0x3f;
                        }
                    }
                    else
                    {
                        if (!diversity2 && !psform.PSEnabled)
                        {
                            P1_DDCConfig = 1;
                            DDCEnable = DDC2;
                            SyncEnable = 0;
                            Rate[2] = rx1_rate;
                            cntrl1 = rx_adc_ctrl1 & 0xff;
                            cntrl2 = rx_adc_ctrl2 & 0x3f;
                        }
                        else if (!diversity2 && psform.PSEnabled)
                        {
                            P1_DDCConfig = 3;
                            DDCEnable = DDC0 + DDC2;
                            SyncEnable = DDC1;
                            Rate[0] = ps_rate;
                            Rate[1] = ps_rate;
                            Rate[2] = rx1_rate;
                            cntrl1 = (rx_adc_ctrl1 & 0xf3) | 0x08;
                            cntrl2 = rx_adc_ctrl2 & 0x3f;
                        }
                        else if (diversity2 && psform.PSEnabled)
                        {
                            P1_DDCConfig = 3;
                            DDCEnable = DDC0 + DDC2;
                            SyncEnable = DDC1;
                            Rate[0] = ps_rate;
                            Rate[1] = ps_rate;
                            Rate[2] = rx1_rate;
                            cntrl1 = (rx_adc_ctrl1 & 0xf3) | 0x08;
                            cntrl2 = rx_adc_ctrl2 & 0x3f;
                        }
                        else
                        { // diversity2 && !psform.PSEnabled
                            P1_DDCConfig = 2;
                            DDCEnable = DDC0;
                            SyncEnable = DDC1;
                            Rate[0] = rx1_rate;
                            Rate[1] = rx1_rate;
                            cntrl1 = rx_adc_ctrl1 & 0xff;
                            cntrl2 = rx_adc_ctrl2 & 0x3f;
                        }
                    }

                    if (rx2_enabled)
                    {
                        DDCEnable += DDC3;
                        Rate[3] = rx2_rate;
                    }
                    break;
                case HPSDRModel.HERMES:
                case HPSDRModel.ANAN10:
                case HPSDRModel.ANAN100:
                    P1_rxcount = 4; // RX4 used for puresignal feedback
                    nddc = 4;
                    if (!mox)
                    {
                        if (!diversity2)
                        {
                            P1_DDCConfig = 4;
                            DDCEnable = DDC0;
                            SyncEnable = 0;
                            Rate[0] = rx1_rate;
                            cntrl1 = 0;
                            cntrl2 = 0;

                            if (rx2_enabled)
                            {
                                DDCEnable += DDC1;
                                Rate[1] = rx2_rate;
                            }
                        }
                        else
                        {
                            P1_DDCConfig = 5;
                            DDCEnable = DDC0;
                            SyncEnable = DDC1;
                            Rate[0] = rx1_rate;
                            Rate[1] = rx1_rate;
                            cntrl1 = 0;
                            cntrl2 = 0;
                        }
                    }
                    else
                    {
                        if (!diversity2 && !psform.PSEnabled)
                        {
                            P1_DDCConfig = 4;
                            DDCEnable = DDC0;
                            SyncEnable = 0;
                            Rate[0] = rx1_rate;
                            cntrl1 = 0;
                            cntrl2 = 0;

                            if (rx2_enabled)
                            {
                                DDCEnable += DDC1;
                                Rate[1] = rx2_rate;
                            }
                        }
                        else if (diversity2 && !psform.PSEnabled)
                        {
                            P1_DDCConfig = 5;
                            DDCEnable = DDC0;
                            SyncEnable = DDC1;
                            Rate[0] = rx1_rate;
                            Rate[1] = rx1_rate;
                            cntrl1 = 0;
                            cntrl2 = 0;
                        }
                        else // transmitting and PS is ON
                        {
                            P1_DDCConfig = 6;
                            DDCEnable = DDC0;
                            SyncEnable = DDC1;
                            Rate[0] = ps_rate;
                            Rate[1] = ps_rate;
                            cntrl1 = 4;
                            cntrl2 = 0;
                        }
                    }
                    break;

                case HPSDRModel.ANAN10E:
                case HPSDRModel.ANAN100B:
                    P1_rxcount = 2; // RX2 used for puresignal feedback
                    nddc = 2;
                    if (!mox)
                    {
                        if (!diversity2)
                        {
                            P1_DDCConfig = 4;
                            DDCEnable = DDC0;
                            SyncEnable = 0;
                            Rate[0] = rx1_rate;
                            cntrl1 = 0;
                            cntrl2 = 0;

                            if (rx2_enabled)
                            {
                                DDCEnable += DDC1;
                                Rate[1] = rx2_rate;
                            }
                        }
                        else
                        {
                            P1_DDCConfig = 5;
                            DDCEnable = DDC0;
                            SyncEnable = DDC1;
                            Rate[0] = rx1_rate;
                            Rate[1] = rx1_rate;
                            cntrl1 = 0;
                            cntrl2 = 0;
                        }
                    }
                    else
                    {
                        if (!diversity2 && !psform.PSEnabled)
                        {
                            P1_DDCConfig = 4;
                            DDCEnable = DDC0;
                            SyncEnable = 0;
                            Rate[0] = rx1_rate;
                            cntrl1 = 0;
                            cntrl2 = 0;

                            if (rx2_enabled)
                            {
                                DDCEnable += DDC1;
                                Rate[1] = rx2_rate;
                            }
                        }
                        else if (diversity2 && !psform.PSEnabled)
                        {
                            P1_DDCConfig = 5;
                            DDCEnable = DDC0;
                            SyncEnable = DDC1;
                            Rate[0] = rx1_rate;
                            Rate[1] = rx1_rate;
                            cntrl1 = 0;
                            cntrl2 = 0;
                        }
                        else // transmitting and PS is ON
                        {
                            P1_DDCConfig = 5;
                            DDCEnable = DDC0;
                            SyncEnable = DDC1;
                            Rate[0] = ps_rate;
                            Rate[1] = ps_rate;
                            cntrl1 = 4;
                            cntrl2 = 0;
                        }
                    }
                    break;

                case HPSDRModel.HPSDR: break;
            }

            NetworkIO.EnableRxs(DDCEnable);
            NetworkIO.EnableRxSync(0, SyncEnable);
            for (i = 0; i < 4; i++) NetworkIO.SetDDCRate(i, Rate[i]);
            NetworkIO.SetADC_cntrl1(cntrl1);
            NetworkIO.SetADC_cntrl2(cntrl2);
            NetworkIO.CmdRx();
            NetworkIO.Protocol1DDCConfig(
                P1_DDCConfig, P1_diversity, P1_rxcount, nddc);
        }

        bool StartNetworkRadio(bool showMsg = false)
        {
            try
            {
                int rc = NetworkIO.initRadio(radio);

                if (rc != 0)
                {
                    if (rc == -101) // firmware version error; 
                    {
                        string fw_err = NetworkIO.getFWVersionErrorMsg();
                        if (fw_err == null)
                        {
                            fw_err = "Bad Firmware levels";
                        }
                        MessageBox.Show(fw_err, "Firmware Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("Error starting SDR hardware, is it connected and powered?", "Network Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    var d = NetworkIO.hpsdrd[0];
                    ucRadio1.ShowIPInfo(d.hostPortIPAddress.ToString() + " (Port " + d.localPort.ToString() + ")",
                        d.IPAddress.ToString() + " (Port " + d.remoteport.ToString() + ")", d.protocol, d.MACAddress, d.deviceType, d.codeVersion, d.MetisVersion);
                }
            }
            catch (Exception e)
            {
                // this.chkPower.Checked = false; <-- NOT here. If we are called again immed after
                // (say, if the protocol was wrong) then the audio will appear to fail as the power isn't on
                // any more!!
                Common.LogString("Unable to start the radio. Information:\n" + e.Message, showMsg);

                return false;
            }
            return true;
        }

        void DoPowerOn()
        {
            // radio.SyncDSP(); <-- takes ages. Do we need it?
            UpdateDDCs(CurrentRadio.Tuning.rx2_enabled);
            CurrentRadio.Tuning.fwc_dds_freq = 0.0f;
            CurrentRadio.Tuning.rx2_dds_freq = 0.0f;

            UpdateVFOASub();

            var existing_protocol = NetworkIO.RadioProtocolSaved;
            bool bad_radio = false;
            if (!StartNetworkRadio(existing_protocol == RadioProtocol.Auto))
            {
                bad_radio = true;
                if (existing_protocol != RadioProtocol.Auto)
                {
                    NetworkIO.RadioProtocolSaved = RadioProtocol.Auto;
                    if (!StartNetworkRadio(true))
                    {
                        bad_radio = true;
                        // because the protocol was auto here, a message will have shown.

                        //Debug.Assert(NetworkIO.LastError != null);
                        //MessageBox.Show("Cannot start the radio:\n" + NetworkIO.LastError.ToString(), App.Name);
                    }
                    else
                    {
                        bad_radio = false;
                    }
                }

            }
            else
            {
                bad_radio = false;
            }

            // switch this back to differentiate between what the user prefers, as saved,
            // rather than what we are using right now.
            NetworkIO.RadioProtocolSaved = existing_protocol;

            if (bad_radio)
            {
                chkPower.Checked = false;
                SelectMenuTab(TabPages.Radio);
                return;
            }
            if (!StartAudio())
            {
                chkPower.Checked = false;
            }
            else
            {
                NetworkIO.SetXVTREnable(0);
                NetworkIO.SetADC1StepAttenData(0);
                NetworkIO.SetAlexAtten(0);
                NetworkIO.SetAntBits(0, 1, 0, false);

                NetworkIO.SetOCBits(0);
                this.radio.Tuning.ChangeFrequency(VFO.FreqInMHz);
                NetworkIO.ATU_Tune(0);

                WDSPStart();

                Debug.Assert(DataFlowing = true);
                Debug.Assert(CheckSync() > 0);
                SetMicGain();


            }

            ShowPoweredUp();
        }

        void WDSPChangeState(int newState)
        {
            DataFlowing = newState == 1;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (newState == 1)
            {
                UpdateDDCs(rx2_enabled);
                UpdateAAudioMixerStates();
            }


            WDSP.SetChannelState(WDSP.id(0, 0), newState, 1);
            if (radio.GetDSPRX(0, 1).Active)
                WDSP.SetChannelState(WDSP.id(0, 1), newState, 1);
            if (radio.GetDSPRX(1, 0).Active)
                WDSP.SetChannelState(WDSP.id(2, 0), newState, 1);
            sw.Stop();
            var el = sw.ElapsedMilliseconds;

            if (newState != 1)
            {
                UpdateAAudioMixerStates();
            }

            Common.LogString("WDSPChangeState: Changing to state: " + newState + " took " + el.ToString());
        }

        public int NetworkOK { get { return NetworkIO.getHaveSync(); } }

        void WDSPStart()
        {
            WDSPChangeState(1);
        }

        int HaveSync { get; set; }
        int CheckSync()
        {
            //int ooo = 0;
            //ooo = NetworkIO.getOOO();
            HaveSync = NetworkIO.getHaveSync();
            return HaveSync;
        }

        void WDSPStop()
        {
            CheckSync();
            if (HaveSync == 1)
            {
                WDSPChangeState(0);
            }
        }

        private string[] lineinboost = new string[32];
        private bool lineinarrayfill = false;
        private void MakeLineInList()
        {
            int k = 0;
            for (double i = -34.5; i <= 12; i += 1.5)
            {
                string s = i.ToString();
                lineinboost[k] = s;
                ++k;
            }
            lineinarrayfill = true;
        }
        bool mic_boost = true;
        bool line_in = false;
        private double line_in_boost = 0.0;

        public int ClampFreq(int freqInHz)
        {
            double megs = (double)freqInHz / 1000000.0;
            megs = VFO.ClampFreq(megs);
            double d = megs * 1000000;
            return (int)d;
        }

        public void SetMicGain()
        {
            var v = mic_boost ? 1 : 0;
            NetworkIO.SetMicBoost(v);

            v = line_in ? 1 : 0;
            NetworkIO.SetLineIn(v);

            if (!lineinarrayfill) MakeLineInList();

            var lineboost
                = Array.IndexOf(lineinboost, line_in_boost.ToString());

            NetworkIO.SetLineBoost(lineboost);
        }

        private bool vac_enabled = false;
        public bool VACEnabled
        {
            get { return vac_enabled; }
            set
            {
                vac_enabled = value;
                Audio.EnableVAC1Exclusive(ucAudio1.Exclusive);
                Audio.VACEnabled = value;

            }
        }

        private Band rx1_band = Band.B160M;
        internal CustomUserControls.ucAudio UCAudio
        {
            get
            {
                return this.ucAudio1;
            }
        }

        internal CustomUserControls.ucBands UCBands
        {
            get { return this.ucBands1; }
        }

        internal bool StartAudio()
        {
            /*/
            if (PTTBitBangEnabled
                    && serialPTT
                        == null) // we are enabled but don't have port object
            {
                // Debug.WriteLine("Forcing property set on PTTBitBangEnabled");
                PTTBitBangEnabled = true; // force creation of serial ptt
            }
            /*/
            Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;


            bool vac_was_wanted = true;

            VACEnabled = true; // Don't trigger StopAudioIVAC if the VACs
                               // aren't needed now


            if (vac_was_wanted)
            {
                if (!Audio.Status[0].state)
                {
                    SelectMenuTab(TabPages.Audio);
                    return false;
                }
            }

            if (m_bAttontx)
                NetworkIO.SetTxAttenData(
                    this.radio.tx_step_attenuator_by_band[(int)rx1_band]);
            else
                NetworkIO.SetTxAttenData(0);

            // is this really needed? Yes, it will skip if no changes made
            ChangeRX1SampleRate(this.SampleRateRX1);
            if (!Audio.Start()) // starts JanusAudio running
            {
                chkPower.Checked = false;
                return false;
            }
            return true;
        }

        private void UpdateVFOASub()
        {
            CurrentRadio.Tuning.ChangeFrequency(VFO.FreqManager.FreqInMHz);
        }

        private void StopAudio()
        {
            NetworkIO.StopAudio();

            if (vac_enabled)
            {
                ivac.SetIVACrun(0, 0);
                ivac.StopAudioIVAC(0);
            }

            /*/
            if (vac2_enabled)
            {
                ivac.SetIVACrun(1, 0);
                ivac.StopAudioIVAC(1);
            }
            /*/
        }

        void DoPowerOff()
        {
            WDSPStop();
            ShowPoweredUp();
            StopAudio();
            ucRadio1.ShowIPInfo("", "", RadioProtocol.None, "", HPSDRHW.Unknown, 0, 0);
        }

        void ShowPoweredUp()
        {
            if (chkPower.Checked)
            {
                chkPower.Text = "Radio On ";
                coolVFO1.ForeColor = Color.Black;
                coolVFO1.BackColor = Color.DarkSeaGreen;
                coolVFO1.TextBackColor = Color.DarkSeaGreen;
            }
            else
            {
                chkPower.Text = "Radio Off";
                coolVFO1.ForeColor = Color.Black;
                coolVFO1.BackColor = Color.DarkGray;
                coolVFO1.TextBackColor = Color.DarkGray;
            }

            ucAudio1.Enabled = !chkPower.Checked;
            ucRadio1.Enabled = !chkPower.Checked;
            coolVFO1.Enabled = chkPower.Checked;
            sldPower.Enabled = chkPower.Checked;
            UpdateStatusLabel();
        }

        internal bool GotClosed { get; set; }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PowerOn)
            {
                chkPower.Checked = false;
            }
            // coz, MaterialSkin, this gets called TWICE
            if (!GotClosed)
            {
                GotClosed = true;
                AppSettings.MySettings.ActiveTab = tabMnu.SelectedIndex;
                MyPositioner.Save(this);
                Display.ShutdownDX2D();
                if (radio != null)
                {
                    radio.Dispose();
                }

            }
        }

        public volatile bool DoingWisdom = false;

        public void ShowWisdomMsg()
        {
            var w = new Form() { Size = new Size(0, 0) };
            Task.Delay(TimeSpan.FromSeconds(10))
                .ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            var message = "Please go grab a coffee. " +
                "A one-time command window will show whilst the FFT (Fourier Fast Transform) is optimised for your computer.\n"
                + "\n\nThis will take a while.\n" + "If you close the wisdom window prematurely, you will never get past this stage :-(\n\n\n"
                + "Click OK before you go!";
            MessageBox.Show(w, message, "CoolSDR One-Time FFT Calculation");

        }

        // private volatile bool m_waiting_for_radio = false;


        private Task PAInitTask = null;
        private Task RadioInitTask = null;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // This is the earliest we can do this, as it requires
            // that the window handle has been created.
            RadioDSP.CreateDSP(); // must be on main thread, and before any display thread starts. This is fine.
            Task tr = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "RadioCreationThread";


                specRX = new SpecRX();
                Display.SpecReady = true; // radio needs specrx
                CurrentRadio = new SDRRadio(AppDataPath, this);
            });

            RadioInitTask = tr;
        }


        public enum TabPages
        {
            Main = 0,
            Audio = 1,
            Radio = 2,
            LastTab
        }

        public void SelectMenuTab(TabPages which)
        {
            this.tabMnu.SelectedIndex = (int)which;
        }

        public void OnIPChanged(string newip)
        {
            radio.Settings.Save();
            // Debug.Print("You might want to restart the radio, eg if it is running, coz IP changed");
        }

        public void OnRadioChanged(HPSDRModel newModel)
        {
            // Debug.Print("You might want to restart the radio, eg if it is running, coz Model changed");
        }

        private void tabMnuSelectedIndexChanged(object sender, EventArgs e)
        {
            int curTab = tabMnu.SelectedIndex;
            if (curTab == (int)TabPages.Radio)
            {
                ucRadio1.Enabled = !PowerIsOn;
                ucRadio1.ForceShowAnyToolTips();

            }
        }
        private void tabMnuSelecting(object sender, TabControlCancelEventArgs e)
        {

        }

        private void tabMnuDeselecting(object sender, TabControlCancelEventArgs e)
        {
            int curTab = e.TabPageIndex;
            if (curTab == (int)TabPages.Radio)
            {
                ucRadio1.ForceHideAnyToolTips();
                ucRadio1.Enabled = false;
            }
            if (curTab == (int)TabPages.Audio)
            {
                if (ucAudio1.IsDirty)
                {
                    var ans = MessageBox.Show("There are unsaved (unapplied) changes on the audio tab.\n"
                         + ucAudio1.DirtyReason + " was changed.\n\nDo you want to go back to apply your changes?\n If not, you will lose the changes." +
                        "\n\n",
                        "Unsaved Audio Changes in CoolSDR", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ans == DialogResult.Yes)
                    {
                        e.Cancel = true;

                    }
                    else
                    {
                        e.Cancel = false;
                        ucAudio1.SyncToSettings();
                    }
                }
            }
        }


        private void mnuTabIndexChanged(object sender, EventArgs e)
        {

        }

        private void pnlTabMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ucRadio1_Load(object sender, EventArgs e)
        {

        }

        private async void picDisplayResize(object sender, System.EventArgs e)
        {
            DisplayHelper.SuspendDrawing(this);

            if (DisplayHelper == null) return;
            if (chkPower.Checked) DisplayHelper.Pause_DisplayThread = true;


            Display.Target = picDisplay;

            switch (current_display_engine)
            {
                case (DisplayEngine.GDI_PLUS):
                    {
                        await Task.Delay(1);
                        picDisplay.Invalidate();
                    }
                    break;
                case DisplayEngine.DIRECT_X:
                    {
                        Display.ResizeDX2D();
                        break;
                    }
            }
            DisplayHelper.Pause_DisplayThread = false;

            if (!initializing)
            {
                // UpdateRXSpectrumDisplayVars();
                // UpdateTXSpectrumDisplayVars();
            }
            DisplayHelper.ResumeDrawing(this);
        }

        private void kryptonCheckButton1_Click(object sender, EventArgs e)
        {
            Debug.Print(NetworkOK.ToString());
        }
    }

    public class MyPictureBox : PictureBox
    {
        public MyPictureBox()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // base.OnPaint(pe);   
        }

        public override Color BackColor
        {
            get => base.BackColor;
            set
            {

                base.BackColor = value;
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;
                baseParams.ExStyle |= 0x0200000;
                return baseParams;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }
        static int WM_ERASEBKGND = 0x0014;
        static int WM_WINDOWPOSCHANGING = 0x0046;
        //static uint SWP_NOSIZE = 0x0001;
        // static uint SWP_NOZORDER = 0x0004;
        // static uint SWP_NOREDRAW = 0x0008;
        //static uint SWP_NOACTIVATE = 0x0010;
        static uint SWP_NOCOPYBITS = 0x0100;
        // static uint SWP_NOOWNERZORDER = 0x0200;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_ERASEBKGND)
            {
                m.Result = (IntPtr)(1);
            }
            else if (m.Msg == WM_WINDOWPOSCHANGING)
            {
                unsafe
                {
                    WINDOWPOS* windowPos = (WINDOWPOS*)m.LParam;

                    // Windows has an optimization to copy pixels
                    // around to reduce the amount of repainting
                    // needed when moving or resizing a window.
                    // Unfortunately, this is not compatible with WPF
                    // in many cases due to our use of DirectX for
                    // rendering from our rendering thread.
                    // To be safe, we disable this optimization and
                    // pay the cost of repainting.
                    windowPos->flags |= SWP_NOCOPYBITS;
                    base.WndProc(ref m);
                }


                base.WndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Do nothing to disable background painting.
        }
    }

}
