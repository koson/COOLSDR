using CoolSDR.Class;
using Newtonsoft.Json.Linq;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Thetis;
using static Thetis.Audio;
using static Thetis.PortAudioForCoolSDR;

namespace CoolSDR.CustomUserControls
{
    public partial class ucAudio : UserControl
    {

        public PortAudioSettings Settings { get; set; }
        public PortAudioHelper PAHelper { get; private set; }
        public bool IsDirty
        {
            get
            {
                if (DirtyControl == null)
                    return false;
                return true;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (PAHelper != null)
            {
                PAHelper.Dispose();
            }
        }


        public Control DirtyControl { get; private set; }

        public ucAudio()
        {
            InitializeComponent();
        }

        private void GetSettings()
        {
            try
            {
                if (Settings == null)
                {
                    Settings = new PortAudioSettings(App.GetDataFolder());
                }
            }
            catch (Exception e)
            {
                Common.LogString("Audio: failed to load settings" + e.Message);
                MessageBox.Show("Audio cannot load settings. Error encountered:\n" + e.Message + "\n\n" +
                    "The previous audio settings were backed up with a .failed extension\n\n" +
                    "I will attempt to load some default audio settings.", App.Name);
                try
                {
                    Settings = new PortAudioSettings(App.GetDataFolder());
                }
                catch (Exception ex)
                {
                    Common.LogString("Audio: failed to load the most basic settings" + ex.Message);
                    MessageBox.Show("Audio cannot even load the most basic of settings. This is fatal.", "Cool SDR Fatal Error");
                    Application.Exit();
                }
            }
        }

        private void LoadDefaults()
        {
            Debug.Assert(PAHelper != null);
            if (Common.console != null)
            {
                if (Common.console.GotClosed) return;
            }
            PAHelper.SetDefaults(m_Combos);
            CollectAudioSettings(true);
            AddEventHandlers();
            if (cboAPI.Text == "Windows WASAPI") chkExclusive.Visible = true; else chkExclusive.Visible = false;
        }

        public bool Exclusive
        {
            get => chkExclusive.Checked;
        }


        private bool m_bSetEventHandlers = false;
        private void AddEventHandlers()
        {
            if (!m_bSetEventHandlers)
            {
                this.cboAPI.SelectedIndexChanged += new System.EventHandler(this.cboAPI_SelectedIndexChanged);
                this.cboAPI.SelectedIndexChanged += new System.EventHandler(this.OnControlValueChanged);
                cboBufferSize.SelectedIndexChanged += new System.EventHandler(this.OnControlValueChanged);
                cboBufferSize.SelectedIndexChanged += new System.EventHandler(this.OnControlValueChanged);
                cboSampleRate.SelectedIndexChanged += new System.EventHandler(this.OnControlValueChanged);
                chkExclusive.CheckedChanged += new System.EventHandler(this.OnControlValueChanged);
                cboAudioIn.SelectedIndexChanged += new System.EventHandler(this.OnControlValueChanged);
                cboAudioOut.SelectedIndexChanged += new System.EventHandler(this.OnControlValueChanged);
            }
            m_bSetEventHandlers = true;
        }

        private PortAudioHelper.ComboControls m_Combos;
        void PopCombos()
        {
            if (m_Combos.API == null)
            {
                m_Combos.API = cboAPI;
                m_Combos.Exclusive = chkExclusive;
                m_Combos.AudioOut = cboAudioOut;
                m_Combos.AudioIn = cboAudioIn;
                m_Combos.BufferSize = cboBufferSize;
                m_Combos.SampleRate = cboSampleRate;
                PortAudioHelper.CheckComboControls(m_Combos);
            }
        }
        bool initted = false;
        public void Init()
        {
            if (initted) return;
            PopCombos();
            try
            {
                GetSettings();

                if (PAHelper == null)
                    PAHelper = new PortAudioHelper();

                if (!Settings.HadSettingsToLoad)
                {
                    Common.LogString("No audio settings detected, going ahead and setting defaults here ...");
                    LoadDefaults();
                    return;
                }


                PAHelper.GetHosts();
                PAHelper.PopulateCombos(cboAPI, cboAudioIn, cboAudioOut, Settings.API);
                var ret = PAHelper.LoadSettings(Settings, m_Combos);
                
                if (ret.Retval != PortAudioHelper.LoadSettingsRetval.Good)
                {
                    // it can 'partially' succeed, in which case:
                    this.CollectAudioSettings(true);
                }
                if (PortAudioHelper.DevicesOutAll.Count == 0)

                {
                    Program.ThrowAndExit("No audio output devices detected on system");
                    return;
                }
                else if (PortAudioHelper.DevicesInAll.Count == 0)
                {
                    Program.ThrowAndExit("No audio input devices detected on system");
                    return;
                }

                if (string.IsNullOrEmpty(Settings.API)
                    || string.IsNullOrEmpty(Settings.InputDevice)
                    || string.IsNullOrEmpty(Settings.OutputDevice))

                {

                    Common.LogString("No settings.api, or no input device, or no output device causes us to try to load default audio settings.", Settings.HadSettingsToLoad);
                    LoadDefaults();
                }
            }
            catch (Exception e)
            {
                Thetis.Common.LogString("Exception when Init() audio\n" + e.Message + "\n\nI will try to load some sane defaults now.\t" + Environment.NewLine + e.StackTrace, Settings.HadSettingsToLoad);
                Thetis.Common.LogString("This causes us to try to load fail-safe settings");
                LoadDefaults();

            }

            AddEventHandlers();
            if (Common.console != null)
            {
                if (Common.console.GotClosed) return;
            }
            TDD();
            initted = true;

        }

        private void TDD()
        {
# if DEBUG
            int was_sel_input_index = cboAudioIn.SelectedIndex;
            int was_sel_output_index = cboAudioOut.SelectedIndex;
            Debug.Assert(!initted);
            Debug.Assert(InputDeviceCurrent == InputDevice);
            Debug.Assert(OutputDeviceCurrent == OutputDevice);
            Debug.Assert(APIIndexCurrent == APIIndex);
            Debug.Assert(HostAPI == HostAPICurrent);

            var was_sel_index = cboAPI.SelectedIndex;

            if (cboAPI.Items.Count > 0)
            {
                if (cboAPI.SelectedIndex == 0)
                {
                    cboAPI.SelectedIndex = 1;
                }
                else
                {
                    cboAPI.SelectedIndex = 0;
                }

                Debug.Assert(APIIndexCurrent != APIIndex);
                Debug.Assert(InputDeviceCurrent != InputDevice);
                Debug.Assert(OutputDeviceCurrent != OutputDevice);
                Debug.Assert(HostAPI != HostAPICurrent);


                cboAPI.SelectedIndex = was_sel_index;
                cboAudioIn.SelectedIndex = was_sel_input_index;
                cboAudioOut.SelectedIndex = was_sel_output_index;

                Debug.Assert(InputDeviceCurrent == InputDevice);
                Debug.Assert(OutputDeviceCurrent == OutputDevice);
                Debug.Assert(APIIndexCurrent == APIIndex);
                Debug.Assert(HostAPI == HostAPICurrent);

            }

#endif
        }

        private void SetDefaults()
        {
            var defapiName = PAHelper.DefaultAPIName;

            PAHelper.SetDefaults(m_Combos);
            this.CollectAudioSettings();
            cboBufferSize.Refresh();
            cboSampleRate.Refresh();
            cboAPI.Refresh();
            CollectAudioSettings(); // save this now
        }

        private void btnAudioReset_Click(object sender, EventArgs e)
        {
            var ans = MessageBox.Show("Really load failsafe defaults? You will lose any settings you have saved if you do.",
                App.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (ans == DialogResult.Yes)
            {
                SetDefaults();
            }

        }

        private bool DecideDirty(Control c)
        {
            bool ret = false;
            if (c.Focused) ret = true;
            if (ret)
                DirtyControl = c;
            if (DirtyControl == (Control)cboAPI)
            {
                if (cboAPI.SelectedItem.ToString() == Settings.API)
                {
                    DirtyControl = null;
                    return false;
                }
            }
            if (DirtyControl == (Control)cboAudioIn)
            {
                if (cboAudioIn.SelectedItem.ToString() == Settings.InputDevice)
                {
                    DirtyControl = null;
                    return false;
                }
            }
            if (DirtyControl == (Control)cboAudioOut)
            {
                if (cboAudioOut.SelectedItem.ToString() == Settings.OutputDevice)
                {
                    DirtyControl = null;
                    return false;
                }
            }
            if (DirtyControl == (Control)cboBufferSize)
            {
                if (cboBufferSize.SelectedItem.ToString() == Settings.BufferSize.ToString())
                {
                    DirtyControl = null;
                    return false;
                }
            };
            if (DirtyControl == (Control)cboSampleRate)
            {
                if (cboSampleRate.SelectedItem.ToString() == Settings.SampleRate.ToString())
                {
                    DirtyControl = null;
                    return false;
                }
            };
            if (DirtyControl == (Control)chkExclusive)
            {
                if (chkExclusive.Checked != Settings.AudioExclusive)
                {
                    DirtyControl = null;
                    return false;
                }
            };

            return ret;
        }
        public string DirtyReason
        {
            get
            {
                if (DirtyControl != null)
                {
                    if (DirtyControl == (Control)cboAPI) return "API";
                    if (DirtyControl == (Control)cboAudioIn) return "Input Device";
                    if (DirtyControl == (Control)cboAudioOut) return "Output Device";
                    if (DirtyControl == (Control)cboBufferSize) return "Buffer Size";
                    if (DirtyControl == (Control)cboSampleRate) return "Samplerate";
                    if (DirtyControl == (Control)chkExclusive) return "Exclusivity";
                }

                return "Nothing changed";
            }
        }

        public void SyncToSettings()
        {
            // sync the UI with the settings
            this.DirtyControl = null;
            GetSettings();
            PAHelper.PopulateCombos(cboAPI, cboAudioIn, cboAudioOut, Settings.API);
        }

        private void OnControlValueChanged(object sender, EventArgs e)
        {
            DecideDirty((Control)sender);
        }
        private void cboAPI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAPI.Text == "Windows WASAPI") chkExclusive.Visible = true; else chkExclusive.Visible = false;
            PortAudioHelper.ChangeAPI(cboAPI.SelectedItem.ToString(), cboAudioIn, cboAudioOut);
            cboAPI.Refresh();
            DecideDirty(cboAPI);
        }

        // returns the one selected in the UI, not the saved one
        public PaDeviceInfoEx InputDeviceCurrent
        {
            get
            {
                
                int index = cboAudioIn.SelectedIndex;
                if (index < 0) index = 0;
                PaDeviceInfoEx dev = (PaDeviceInfoEx)cboAudioIn.Items[cboAudioIn.SelectedIndex];
                return dev;
            }
        }

        // returns the saved one, not the one selected in the UI
        public PaDeviceInfoEx InputDevice
        {
            get
            {
                string name = this.Settings.InputDevice;
                var devs = PortAudioHelper.DevicesIn(Settings.API);
                var dev = devs.Find(x => x.Name == name);
                Debug.Assert(dev != null);
                return dev;
            }
        }

        // returns the saved one, not the one selected in the UI
        public PaDeviceInfoEx OutputDevice
        {
            get
            {
                string name = this.Settings.OutputDevice;
                var devs = PortAudioHelper.DevicesOut(this.HostAPI.Name);
                var dev = devs.Find(x => x.Name == name);
                Debug.Assert(dev != null);
                return dev;
            }
        }


        // returns the one selected in the UI, not the saved one.
        public PaDeviceInfoEx OutputDeviceCurrent
        {
            get
            {
                int index = cboAudioOut.SelectedIndex;
                if (index < 0) index = 0;
                string api_name = cboAPI.SelectedItem.ToString();
                return (PaDeviceInfoEx)cboAudioOut.SelectedItem;
            }
        }



        // returns the saved one, not the one selected in the UI
        public int APIIndex
        {
            get
            {
                if (Settings == null) return 0; // for design mode
                return Settings.APIIndex;
            }
        }

        // returns the one selected in the UI, not the saved one.
        public int APIIndexCurrent
        {
            get
            {
                Debug.Assert(cboAPI.SelectedItem != null);
                PaHostAPIInfoEx info = (PaHostAPIInfoEx)cboAPI.SelectedItem;
                return info.Index;
            }
        }

        // returns the one selected in the UI, not the saved one.
        public string APICurrent
        {
            get
            {
                return cboAPI.SelectedItem.ToString();
            }
        }

        // returns the saved one, not the one selected in the UI
        public PaHostAPIInfoEx HostAPI
        {
            get
            {
                return PortAudioHelper.APIFromName(Settings.API);
            }
        }

        // returns the one selected in the UI, not the saved one.
        public PaHostAPIInfoEx HostAPICurrent
        {
            get
            {
                PaHostAPIInfoEx ret;
                string s = APICurrent;
                ret = PortAudioHelper.APIFromName(s);
                return ret;
            }
        }

        void CollectAudioSettings(bool save = true)
        {
            Settings.AudioExclusive = chkExclusive.Checked;
            Settings.InputDevice = cboAudioIn.SelectedItem.ToString();
            Settings.OutputDevice = cboAudioOut.SelectedItem.ToString();
            Settings.BufferSize = int.Parse(cboBufferSize.SelectedItem.ToString());
            Settings.SampleRate = int.Parse(cboSampleRate.SelectedItem.ToString());
            Settings.API = cboAPI.SelectedItem.ToString();
            Settings.APIIndex = this.HostAPICurrent.Index;
            if (save)
                Settings.Save();

            DirtyControl = null;
        }

        private void btnApplyAudio_Click(object sender, EventArgs e)
        {
            CollectAudioSettings();
        }

        private void ucAudio_EnabledChanged(object sender, EventArgs e)
        {
            this.lblDisabled.Visible = !this.Enabled;
        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            TestCurrentSettings();
        }

        public int SampleRateCurrent
        {
            get
            {
                try
                {
                    var s = cboSampleRate.SelectedItem.ToString();
                    return int.Parse(s);
                }
                catch (Exception e)
                {
                    Common.LogException(e);
                    return PortAudioHelper.DEFAULT_SAMPLERATE;
                }

            }
        }

        public int SampleRate
        {
            get
            {
                return (int)Settings.SampleRate;
            }
        }

        public int BufferSizeCurrent
        {
            get
            {
                try
                {
                    var s = cboBufferSize.SelectedItem.ToString();
                    return int.Parse(s);
                }
                catch (Exception e)
                {
                    Common.LogException(e);
                    return PortAudioHelper.DEFAULT_SAMPLERATE;
                }

            }
        }

        public int BufferSize
        {
            get
            {
                return (int)Settings.BufferSize;
            }
        }

        private void TestCurrentSettings()
        {
            var indev = this.InputDeviceCurrent;
            var outdev = this.OutputDeviceCurrent;
            var api = this.APIIndexCurrent;
            bool ok = true;
            Debug.Assert(Common.console != null);
            Debug.Assert(indev.Info.hostApiIndex == outdev.Info.hostApiIndex);
            bool was_running = Common.console.PowerOn;
            bool was_vac = console.VACEnabled;

            if (was_running) Common.console.PowerOn = false;
            try
            {
                console.VACEnabled = true; // takes care of exclusive or not
                EnableVAC1(true, true);

            }
            catch (Exception e)
            {
                console.VACEnabled = false;
                ok = false;
                MessageBox.Show("Testing devices failed. Please review the information carefully \n\n"
                    + e.ToString(), App.Name);
                ivac.StopAudioIVAC(0);
            }
            finally
            {
                ok = Audio.Status[0].state; 
                if (ok)
                {
                    
                    var r = MessageBox.Show("Test was successful.\n\nSave and apply these settings?",
                        App.Name + " Audio Test", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r == DialogResult.Yes)
                    {
                        this.CollectAudioSettings();
                    }

                    console.VACEnabled = was_running;
                    Common.Console.PowerOn = was_running;
                    
                }


            }
        }
    }
}
