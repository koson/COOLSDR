
using CoolSDR.Properties;
using Krypton.Toolkit;
using MaterialSkin2DotNet.Controls;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using Thetis;
using MessageBox = System.Windows.Forms.MessageBox;

namespace CoolSDR.CustomUserControls
{
    public partial class ucRadio : UserControl
    {
        Thetis.RadioBase m_radio;
        public ucRadio()
        {
            InitializeComponent();

        }
        private void IPInfoEnable(bool b)
        {
            grpIPInfo.Enabled = true;
            if (b) grpIPInfo.Text = "Current Connection Info"; else grpIPInfo.Text = "Radio not running";
            foreach (var o in grpIPInfo.Controls)
            {
                Control c = (Control)o;
                c.Enabled = b;
            }
        }

        private RadioProtocol m_last_protocol_seen = RadioProtocol.None;
        private string m_last_ip_seen = "";
        private string m_sProtWarn = "";
        private string m_sIPWarn = "";

        public void ShowIPInfo(string localIPAndPort, string remoteIPAndPort, RadioProtocol protocol, string macAddress,
            HPSDRHW hw, byte codeVersion, byte metisInfo)
        {
            if (String.IsNullOrEmpty(localIPAndPort) || string.IsNullOrEmpty(remoteIPAndPort))
            {
                grpIPInfo.Enabled = true; // looks shit when disabled (dark text on dark background)
                grpIPInfo.Text = "Radio not running";
                IPInfoEnable(false);
            }
            else
            {
                grpIPInfo.Text = "Current Connection Info";
                // m_radio.Settings.Protocol = protocol;
                // m_radio.Settings.Save(); // since this clearly works, we will save it
                // ^^ Don't do this -- there is a difference between what you would like, and what you will get
                m_radio.Settings.LastSuccessfulProtocol = protocol;
                grpIPInfo.Enabled = true;
                IPInfoEnable(true);
            }

            this.lblCurIP.Text = remoteIPAndPort;
            lblCurProf.Text = protocol.ToString();
            lblLocalNetwork.Text = localIPAndPort;
            lblMac.Text = macAddress.Replace("-", ":");
            lblHardware.Text = EnumStrings.HPSDRHWToString(hw)
                + Environment.NewLine + "Firmware Version: " + codeVersion.ToString()
                + Environment.NewLine + "Metis:" + metisInfo.ToString();

            if (protocol != RadioProtocol.None)
            {
                m_last_protocol_seen = protocol;
            }

            bool show_prot_warning = true;
            if (m_radio.Settings.Protocol == RadioProtocol.Auto)
            {
                show_prot_warning = false;
            }
            else
            {
                if (protocol == RadioProtocol.None)
                {
                    // powering down, nothing to compare against, except ...
                    if (m_last_protocol_seen != m_radio.Settings.Protocol)
                    {
                        show_prot_warning = true;
                    }
                    else
                    {
                        show_prot_warning = false;
                    }
                }
                else
                {
                    if (protocol == m_radio.Settings.Protocol)
                    {
                        show_prot_warning = false;
                    }
                }
            }

            if (show_prot_warning)
            {
                RadioProtocol this_prot = protocol == RadioProtocol.None ? m_last_protocol_seen : protocol;
                this.picWarnProt.Visible = true;
                m_sProtWarn = "The radio will start more quickly if you \nset the protocol to the correct value: \n"
                    + this_prot.ToString() + "\n\n" + "Note: The radio must be stopped to make any changes here!";
                MySetToolTip(m_bEnabled && Visible, m_sProtWarn, picWarnProt);
            }
            else
            {
                m_sProtWarn = "";
                MySetToolTip(m_bEnabled && Visible, m_sProtWarn, picWarnProt);
                this.picWarnProt.Visible = false;

            }


            if (!String.IsNullOrEmpty(remoteIPAndPort))
            {
                string[] splut = remoteIPAndPort.Split(' ');
                m_last_ip_seen = splut[0];
                if ((splut[0] != m_radio.Settings.IPAddress) || !m_radio.UseStaticIp)
                {
                    m_sIPWarn = "The radio will start more quickly if you set the correct \nstatic IP address for the radio, which is:\n"
                        + splut[0] + "\n\n" + "Note: The radio must be stopped in order to make changes!";

                    textBoxFakeIP.Text = splut[0];
                    picWarnIP.Visible = true;
                    MySetToolTip(Visible && m_bEnabled, m_sIPWarn, picWarnIP);

                }
                else
                {
                    picWarnIP.Visible = false;
                }
            }
        }

        bool m_bEnabled = true;
        public new bool Enabled
        {
            get => m_bEnabled;
            set
            {
                // we DO NOT call the base else our groupboxes look like SHIT
                // cannot disable group radio buttons, because they lose their 'set' state.
                // And I want to show this for informational purposes!
                m_bEnabled = value;
                ShowStaticIP();
            }
        }





        bool m_bInitting = false;
        Forms.FrmMain m_frmMain;
        public void Init(Forms.FrmMain mainForm, RadioBase radio)
        {
            try
            {
                // grpIPInfo.Enabled = false;
                IPInfoEnable(false);
                m_bInitting = true;
                m_frmMain = (Forms.FrmMain)mainForm;
                m_radio = radio;
                CreateRadioSelButtons();
                CreateProtocolSelButtons();
                PopSettings(radio);
                this.chkIP.CheckedChanged += new System.EventHandler(chkIP_CheckedChanged);
                this.btnIP.Click += new System.EventHandler(this.btnIP_Click);
                toolTipRadio.ShowAlways = false; // otherwise they can show on top of other windows, including the IDE

            }
            catch (Exception e)
            {
                Common.LogException(e, true, "A fatal error occurred whilst loading the radio");
                throw e;
            }
            finally { m_bInitting = false; }
        }



        bool popped = false;
        void PopSettings(Thetis.RadioBase radio)
        {
            Debug.Assert(!popped);
            if (!popped)
            {
                var val = (int)radio.CurrentRadio;
                this.m_radiosels[val].Checked = true;
                this.txtIP.Text = radio.IpAddress;
                this.chkIP.Checked = radio.UseStaticIp;
                ShowStaticIP();
                this.CurrentHPSDRModel = m_radio.Settings.RadioModel;
                Debug.Assert(radio.CurrentRadio == CurrentHPSDRModel);
                Debug.Assert(Common.console.CurrentHPSDRModel == CurrentHPSDRModel);
                Debug.Assert(radio.Tuning.CurrentHPSDRModel == CurrentHPSDRModel);

            }

        }


        bool mox
        {
            get
            {
                if (m_frmMain == null) return false;
                return m_frmMain.mox;
            }
        }







        public bool initializing { get => m_bInitting; }




        void SelectRadio(HPSDRModel m)
        {
            int idx = (int)m;
            if (idx < 0 || idx >= m_radiosels.Length)
            {
                idx = (int)HPSDRModel.HERMES;
            }
            m_radiosels[idx].Checked = true;
        }





        private System.Windows.Forms.RadioButton[] m_radiosels;
        private System.Windows.Forms.RadioButton[] m_protocolsels;
        void CreateProtocolSelButtons()
        {
            if (m_protocolsels == null)
            {
                m_protocolsels = new System.Windows.Forms.RadioButton[(int)RadioProtocol.LAST - 1];
                int mysep = 5; int mytop = lblProt.Bottom + 10; int myheight = 20; int mywid = 150;
                for (int i = 0; i < (int)RadioProtocol.LAST - 1; i++)
                {
                    var btn = new System.Windows.Forms.RadioButton();
                    btn.Name = "ProtocolButton" + i.ToString();
                    m_protocolsels[i] = btn;
                    RadioProtocol m = (RadioProtocol)i;
                    btn.Text = m.ToString();


                    this.grpNetwork.Controls.Add(btn);
                    btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    btn.Left = this.chkIP.Left + 5;
                    btn.Top = mytop;
                    btn.Height = myheight;
                    btn.Width = mywid;
                    mytop += btn.Height + mysep;
                    btn.Tag = i;
                    btn.Checked = (m == m_radio.Settings.Protocol);
                    btn.CheckedChanged += ProtocolSelChanged;
                    btn.Click += radioButtonClick;
                }

                // make sure at least one is selected
                int x = 0; int checkedIndex = -1;
                foreach (var b in m_protocolsels)
                {
                    if (b.Checked)
                    {
                        checkedIndex = x;
                        break;
                    }
                    x++;
                }
                if (checkedIndex < 0)
                {
                    m_protocolsels[m_protocolsels.Length - 1].Checked = true;
                }
            }
        }
        void CreateRadioSelButtons()
        {
            if (m_radiosels == null)
            {
                m_radiosels = new System.Windows.Forms.RadioButton[(int)HPSDRModel.LAST];
                int mysep = 5; int mytop = 30; int myheight = 25; int mywid = 150;
                int savedRadio = (int)m_radio.Settings.RadioModel;

                for (int i = 0; i < (int)HPSDRModel.LAST; i++)
                {
                    var btn = new System.Windows.Forms.RadioButton();
                    btn.Name = "radButton" + i;
                    btn.Width = mywid;
                    btn.Height = myheight;
                    HPSDRModel m = (HPSDRModel)i;
                    btn.Text = m.ToString();
                    btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    btn.Left = 10;
                    btn.Top = mytop;
                    btn.Tag = i;
                    this.grpRadioSel.Controls.Add(btn);
                    mytop += btn.Height + mysep;
                    grpRadioSel.Height = myheight;
                    btn.CheckedChanged += RadSelChanged;
                    m_radiosels[i] = btn;
                    btn.Click += radioButtonClick;
                }

                { // just so I can use 'i' again. WTF, CSharp?!
                    int i = 0;
                    foreach (var btn in m_radiosels)
                    {
                        if (savedRadio == i)
                        {
                            btn.Checked = true;
                            m_radio.CurrentRadio = (HPSDRModel)(i);
                        }
                        else
                        {
                            btn.Checked = false;
                        }
                        ++i;
                    }
                }
            }
        }


        // because the model is changed immediately when the button is clicked,
        // we do not need to differentiate between 'Current' and 'Saved' for the model.
        private HPSDRModel m_Model = HPSDRModel.FIRST;
        private HPSDRModel m_PrevModel = HPSDRModel.FIRST;
        public HPSDRModel CurrentHPSDRModel
        {
            get
            {
                if (m_radio == null)
                    return m_Model;
                if (!m_bEnabled || m_protoSelBusy)
                {
                    return m_radio.Settings.RadioModel;
                }
                return m_Model;
            }

            set
            {
                m_PrevModel = m_Model;
                m_Model = value;
                if (m_radio != null)
                {
                    int which = (int)value;
                    var what = m_radiosels[which];
                    Debug.Assert(what != null);
                    what.Checked = true;
                    m_radio.CurrentRadio = value;
                }

            }
        }



        private int whichRadioModelButtonSelected()
        {
            if (m_radiosels == null || m_radiosels.Length == 0 || this.DesignMode)
                return 0; // do not throw in the damn designer!

            Debug.Assert(m_radiosels != null && m_radiosels.Length > 0);
            int i = 0;
            foreach (var v in m_radiosels)
            {
                RadioButton btn = (RadioButton)v;
                if (btn.Checked) return i;
                ++i;
            }
            return 0;
        }

        private void radioButtonClick(object sender, EventArgs e)
        {

            var radio = (RadioButton)sender;
            if (radio.Parent == this.grpRadioSel)
            {
                if (!m_bEnabled)
                {
                    radio.Checked = !radio.Checked;
                    m_protoSelBusy = true;
                    var mdl = this.CurrentHPSDRModel;
                    m_protoSelBusy = false;
                    int m = (int)mdl;
                    m_bEnabled = true; // fake it
                    m_radiosels[m].Checked = true;
                    m_bEnabled = false;
                    MessageBox.Show("You must first power down the radio before changing this setting",
                        Class.App.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (radio.Parent == this.grpNetwork)
            {
                if (!m_bEnabled)
                {
                    m_protoSelBusy = false;
                    radio.Checked = !radio.Checked;
                    var p = m_radio.Settings.Protocol;
                    int i = (int)p;
                    m_bEnabled = true;
                    m_protocolsels[i].Checked = true;
                    m_bEnabled = false;
                    //MessageBox.Show("You must first power down the radio before changing this setting",
                    //    Class.App.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    MaterialMessageBox.Show("Radio power needs to be off in order to select a different protocol."
                        , Class.App.Name, MessageBoxButtons.OK, FlexibleMaterialForm.ButtonsPosition.Right);
                    //KryptonMessageBox.Show("Radio power has to be off before you can select a different protocol.", Class.App.Name);
                }

            }

        }

        private void RadSelChanged(object sender, EventArgs e)
        {

            if (m_protoSelBusy)
            {
                return;
            }
            this.picWarnProt.Visible = false;
            m_protoSelBusy = true;
            RadioButton btn = (RadioButton)sender;
            if (!btn.Checked)
            {
                m_PrevModel = CurrentHPSDRModel;
            }
            if (btn.Checked && m_bEnabled)
            {
                HPSDRModel newModel = (HPSDRModel)btn.Tag;
                if (newModel != m_PrevModel && m_PrevModel != HPSDRModel.FIRST)
                {
                    CurrentHPSDRModel = newModel;
                    m_radio.CurrentRadio = newModel;
                    m_radio.Settings.Save();
                    m_frmMain.OnRadioChanged(newModel);
                }
            }
            m_protoSelBusy = false;
        }

        private const int TOOLTIPS_TIMEOUT = 3000;
        private void MySetToolTip(bool showNow, string what, Control ctl, int timeout = TOOLTIPS_TIMEOUT)
        {
            toolTipRadio.SetToolTip(ctl, what);
            toolTipRadio.Hide(ctl); // don't want to see a tooltip in the wrong place briefly flash up on screen.
            if (showNow)
            {
                toolTipRadio.Show(what, ctl, 0, 0, timeout);
            }
        }

        private bool m_protoSelBusy = false;
        private void ProtocolSelChanged(object sender, EventArgs e)
        {
            if (m_protoSelBusy) return;

            m_protoSelBusy = true;

            RadioButton btn = (RadioButton)sender;
            if (btn.Checked && m_bEnabled)
            {
                RadioProtocol prot = (RadioProtocol)btn.Tag;
                m_radio.Settings.Protocol = prot;
                m_radio.Settings.Save();
                if (prot == m_last_protocol_seen)
                {
                    picWarnProt.Visible = false;
                    m_sProtWarn = "";
                    MySetToolTip(false, m_sProtWarn, picWarnProt, 0);

                }
                else
                {
                    if ((m_last_protocol_seen != RadioProtocol.None) && (m_last_protocol_seen != m_radio.Settings.Protocol))
                    {
                        if (m_radio.Settings.Protocol != RadioProtocol.Auto) // Auto is ok, it starts just as fast as with the correct protocol.
                        {
                            picWarnProt.Visible = true;
                            m_sProtWarn = "Explicitly using an incorrect protocol version will mean the radio starts less quickly.";
                            MySetToolTip(m_bEnabled && Visible && !Common.Console.PowerIsOn, m_sProtWarn, picWarnProt);

                        }
                        else
                        {
                            picWarnProt.Visible = false;
                            m_sProtWarn = "";
                            MySetToolTip(false, m_sProtWarn, picWarnProt);
                        }

                    }
                    else
                    {
                        m_sProtWarn = "";
                        MySetToolTip(false, m_sProtWarn, picWarnProt);
                        picWarnProt.Visible = false;
                    }
                }

            }
            m_protoSelBusy = false;

        }

        private void btnIP_Click(object sender, EventArgs e)
        {
            ApplyIP();
        }

        void ApplyIP()
        {
            string txt = txtIP.Text;
            var splut = txt.Split('.');
            bool quit = false;
            if (splut.Length != 4)
            {
                MessageBox.Show("IP address incomplete", Class.App.Name);
                quit = true;
            }
            else
            {
                int i = 0;

                foreach (var s in splut)
                {
                    if (quit) break;
                    int ret = 0;
                    bool ok = int.TryParse(s, out ret);
                    if (!ok)
                    {
                        MessageBox.Show("Bad or incomplete IP Address", Class.App.Name);
                        quit = true;
                        break;
                    }
                    if (i == 0)
                    {
                        if (ret <= 0)
                        {
                            MessageBox.Show("IP Address may not start with a '0'", Class.App.Name);
                            quit = true;
                            break;
                        }
                    }
                    else
                    {

                    }

                }
            }
            if (!quit)
            {
                if (txtIP.Text != m_radio.IpAddress)
                {
                    m_radio.UseStaticIp = true;
                    m_radio.IpAddress = txtIP.Text;
                    txtIP.Text = m_radio.IpAddress;
                    m_frmMain.OnIPChanged(txtIP.Text);

                }
            }
            else
            {
                m_radio.UseStaticIp = false;
                chkIP.Checked = false;
            }

            if (chkIP.Checked)
            {
                m_radio.UseStaticIp = NetworkIO.ValidateIPv4(this.txtIP.Text);
            }
            else
            {
                m_radio.UseStaticIp = false;
            }
            m_radio.Settings.Save();

            if (m_radio.UseStaticIp && m_radio.IpAddress == txtIP.Text)
            {
                picWarnIP.Visible = false;
                m_sIPWarn = "";
            }
        }
        #region "Faking Shown Event"

        public event EventHandler Shown;
        private bool wasShown = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!wasShown)
            {
                wasShown = true;
                if (Shown != null)
                {
                    Shown(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        private void chkIP_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bInitting) return;

            if (chkIP.Checked == false)
            {
                // do this here because controls get disabled,
                // in other cases, it is done by ApplyIP
                m_sIPWarn = "";
                this.toolTipRadio.SetToolTip(picWarnIP, "");
                picWarnIP.Visible = false;
                m_radio.Settings.UseStaticIP = false;
                m_radio.Settings.Save();
                if (m_radio.Settings.IPAddress != m_last_ip_seen)
                {
                    // NOT checked here, so has anything changed, or not? I think not.
                    //picWarnIP.Visible = true;
                    //m_sIPWarn = "IP settings are not saved until they are applied.\n\nPlease use the Apply button." +
                    //    "\n\n WARNING: Not using a static ip address means the radio will take longer to start.";
                    //MySetToolTip(m_bEnabled && Visible, m_sIPWarn, picWarnIP);
                }
            }
            else
            {
                picWarnIP.Visible = true;
                m_sIPWarn = "IP settings are not saved until they are applied.\nPlease use the Apply button"
                    + "\n\nNOTE: If you have not connected to a radio before in this app,\n"
                    + "try NOT using a static ip to enable CoolSDR to try to discover the radio on your network";

                MySetToolTip(m_bEnabled && Visible, m_sIPWarn, picWarnIP);
                if (!String.IsNullOrEmpty(m_last_ip_seen))
                {
                    txtIP.Text = m_last_ip_seen;
                    textBoxFakeIP.Text = m_last_ip_seen;
                }


            }
            ShowStaticIP();

            if (chkIP.Checked)
            {
                this.txtIP.Focus();
                txtIP.Select();
                txtIP.SelectAll();
            }
        }

        public void ForceShowAnyToolTips()
        {

            toolTipRadio.RemoveAll();

            if (!string.IsNullOrEmpty(m_sProtWarn))
            {
                // we are guaranteed to be about to become visible, so true is ok here
                MySetToolTip(true, m_sProtWarn, picWarnProt);

            }
            if (!string.IsNullOrEmpty(m_sIPWarn))
            {
                // we are guaranteed to be about to become visible, so true is ok here
                MySetToolTip(true, m_sIPWarn, picWarnIP);
            }
        }

        public void ForceHideAnyToolTips()
        {
            // timer disables itself after timeout tmrToolTips.Enabled = false;
            toolTipRadio.Hide(picWarnIP);
            toolTipRadio.Hide(picWarnProt);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            ForceHideAnyToolTips();
            base.OnLostFocus(e);

        }

        protected override void OnGotFocus(EventArgs e)
        {
            ForceShowAnyToolTips();
            base.OnGotFocus(e);
        }





        private void ShowStaticIP()
        {
            if (m_bEnabled)
            {
                pnlIP.Enabled = chkIP.Checked;
                txtIP.Enabled = chkIP.Checked;
                btnIP.Enabled = chkIP.Checked;
                if (string.IsNullOrEmpty(m_last_ip_seen))
                    textBoxFakeIP.Text = txtIP.Text;
                else
                    textBoxFakeIP.Text = m_last_ip_seen;

                if (btnIP.Enabled)
                    textBoxFakeIP.Visible = false;
                else
                    textBoxFakeIP.Visible = true; // fake it looking disabled as it does not like to
            }
            else
            {
                textBoxFakeIP.Visible = true;

            }

            btnIP.Enabled = m_bEnabled && !m_frmMain.PowerIsOn;
            textBoxFakeIP.Enabled = m_bEnabled;
            chkIP.Enabled = m_bEnabled;
            lblDisabled.Visible = !m_bEnabled;
            if (chkIP.Checked != m_radio.Settings.UseStaticIP
                || txtIP.Text != m_radio.Settings.IPAddress)
            {
                // dirty!
                picWarnIP.Visible = true;
                m_sIPWarn = "Unsaved changes in IP settings.\n" +
                    "They are not saved until you click the Apply button,\n" +
                    "and you can only Apply IP Settings when the radio is powered OFF";
                MySetToolTip(m_bEnabled && Visible, m_sIPWarn, picWarnIP);

            }
            else
            {
                // we may not be dirty, but not using fixed ip slows things down, so:
                picWarnIP.Visible = !m_radio.Settings.UseStaticIP;
                if (!m_radio.Settings.UseStaticIP)
                {
                    if (m_sIPWarn.IndexOf("static") == -1)
                    {
                        m_sIPWarn = "Using a static IP for the radio, if you know it,\n" +
                                    "leads to a faster powerup time\n" +
                                    "and you can only Apply IP Settings when the radio is powered OFF\n\n"
                                    + "If you don't know the static IP, and have not run this app yet,\n"
                                    + "you should start the radio one-time and this field should then be\n"
                                    + "populated with the correct static IP.";
                    }
                    MySetToolTip(m_bEnabled && Visible, m_sIPWarn, picWarnIP, 10000);

                }
            }

        }



    } // class definition ends.
}
