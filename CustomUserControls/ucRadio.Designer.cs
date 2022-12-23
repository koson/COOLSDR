

namespace CoolSDR.CustomUserControls
{
    partial class ucRadio
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpRadio = new System.Windows.Forms.GroupBox();
            this.RadioTabControl = new MaterialSkin2DotNet.Controls.MaterialTabControl();
            this.tabRadioGeneral = new System.Windows.Forms.TabPage();
            this.pnlGen = new System.Windows.Forms.Panel();
            this.grpIPInfo = new System.Windows.Forms.GroupBox();
            this.lblHardware = new System.Windows.Forms.Label();
            this.materialLabel5 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.lblMac = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.materialLabel4 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.lblLocalNetwork = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.lblCurIP = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.lblCurProf = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.lblCurProfmeh = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpNetwork = new System.Windows.Forms.GroupBox();
            this.lblProt = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.lblDisabled = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.picWarnIP = new System.Windows.Forms.PictureBox();
            this.picWarnProt = new System.Windows.Forms.PictureBox();
            this.chkIP = new MaterialSkin2DotNet.Controls.MaterialCheckbox();
            this.pnlIP = new System.Windows.Forms.Panel();
            this.textBoxFakeIP = new System.Windows.Forms.TextBox();
            this.btnIP = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.txtIP = new CoolComponents.IPControl();
            this.lblIP = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpRadioSel = new System.Windows.Forms.GroupBox();
            this.pnlBot = new System.Windows.Forms.Panel();
            this.visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.toolTipRadio = new System.Windows.Forms.ToolTip(this.components);
            this.grpRadio.SuspendLayout();
            this.RadioTabControl.SuspendLayout();
            this.tabRadioGeneral.SuspendLayout();
            this.pnlGen.SuspendLayout();
            this.grpIPInfo.SuspendLayout();
            this.grpNetwork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWarnIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWarnProt)).BeginInit();
            this.pnlIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpRadio
            // 
            this.grpRadio.Controls.Add(this.RadioTabControl);
            this.grpRadio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRadio.Location = new System.Drawing.Point(0, 0);
            this.grpRadio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpRadio.Name = "grpRadio";
            this.grpRadio.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpRadio.Size = new System.Drawing.Size(862, 538);
            this.grpRadio.TabIndex = 0;
            this.grpRadio.TabStop = false;
            // 
            // RadioTabControl
            // 
            this.RadioTabControl.Controls.Add(this.tabRadioGeneral);
            this.RadioTabControl.Depth = 0;
            this.RadioTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RadioTabControl.Location = new System.Drawing.Point(4, 17);
            this.RadioTabControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RadioTabControl.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.RadioTabControl.Multiline = true;
            this.RadioTabControl.Name = "RadioTabControl";
            this.RadioTabControl.SelectedIndex = 0;
            this.RadioTabControl.Size = new System.Drawing.Size(854, 517);
            this.RadioTabControl.TabIndex = 1;
            // 
            // tabRadioGeneral
            // 
            this.tabRadioGeneral.BackColor = System.Drawing.Color.White;
            this.tabRadioGeneral.Controls.Add(this.pnlGen);
            this.tabRadioGeneral.Location = new System.Drawing.Point(4, 25);
            this.tabRadioGeneral.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabRadioGeneral.Name = "tabRadioGeneral";
            this.tabRadioGeneral.Padding = new System.Windows.Forms.Padding(15, 15, 15, 15);
            this.tabRadioGeneral.Size = new System.Drawing.Size(846, 488);
            this.tabRadioGeneral.TabIndex = 0;
            // 
            // pnlGen
            // 
            this.pnlGen.Controls.Add(this.grpIPInfo);
            this.pnlGen.Controls.Add(this.panel2);
            this.pnlGen.Controls.Add(this.grpNetwork);
            this.pnlGen.Controls.Add(this.panel1);
            this.pnlGen.Controls.Add(this.grpRadioSel);
            this.pnlGen.Controls.Add(this.pnlBot);
            this.pnlGen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGen.Location = new System.Drawing.Point(15, 15);
            this.pnlGen.Margin = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.pnlGen.Name = "pnlGen";
            this.pnlGen.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.pnlGen.Size = new System.Drawing.Size(816, 458);
            this.pnlGen.TabIndex = 0;
            // 
            // grpIPInfo
            // 
            this.grpIPInfo.Controls.Add(this.lblHardware);
            this.grpIPInfo.Controls.Add(this.materialLabel5);
            this.grpIPInfo.Controls.Add(this.lblMac);
            this.grpIPInfo.Controls.Add(this.materialLabel4);
            this.grpIPInfo.Controls.Add(this.lblLocalNetwork);
            this.grpIPInfo.Controls.Add(this.materialLabel3);
            this.grpIPInfo.Controls.Add(this.lblCurIP);
            this.grpIPInfo.Controls.Add(this.materialLabel2);
            this.grpIPInfo.Controls.Add(this.lblCurProf);
            this.grpIPInfo.Controls.Add(this.lblCurProfmeh);
            this.grpIPInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpIPInfo.Location = new System.Drawing.Point(512, 10);
            this.grpIPInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpIPInfo.Name = "grpIPInfo";
            this.grpIPInfo.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpIPInfo.Size = new System.Drawing.Size(267, 412);
            this.grpIPInfo.TabIndex = 32;
            this.grpIPInfo.TabStop = false;
            this.grpIPInfo.Text = "Connection Info";
            // 
            // lblHardware
            // 
            this.lblHardware.AutoSize = true;
            this.lblHardware.Location = new System.Drawing.Point(21, 290);
            this.lblHardware.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHardware.MinimumSize = new System.Drawing.Size(20, 20);
            this.lblHardware.Name = "lblHardware";
            this.lblHardware.Size = new System.Drawing.Size(20, 20);
            this.lblHardware.TabIndex = 34;
            // 
            // materialLabel5
            // 
            this.materialLabel5.AutoSize = true;
            this.materialLabel5.Depth = 0;
            this.materialLabel5.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel5.HighEmphasis = true;
            this.materialLabel5.Location = new System.Drawing.Point(21, 261);
            this.materialLabel5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.materialLabel5.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel5.Name = "materialLabel5";
            this.materialLabel5.Size = new System.Drawing.Size(73, 19);
            this.materialLabel5.TabIndex = 33;
            this.materialLabel5.Text = "Hardware:";
            this.materialLabel5.UseAccent = true;
            this.materialLabel5.UseMnemonic = false;
            // 
            // lblMac
            // 
            this.lblMac.AutoSize = true;
            this.lblMac.Depth = 0;
            this.lblMac.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMac.Location = new System.Drawing.Point(21, 230);
            this.lblMac.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMac.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblMac.Name = "lblMac";
            this.lblMac.Size = new System.Drawing.Size(67, 19);
            this.lblMac.TabIndex = 32;
            this.lblMac.Text = "Unknown";
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel4.HighEmphasis = true;
            this.materialLabel4.Location = new System.Drawing.Point(21, 207);
            this.materialLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.materialLabel4.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(145, 19);
            this.materialLabel4.TabIndex = 31;
            this.materialLabel4.Text = "Radio MAC Address:";
            this.materialLabel4.UseAccent = true;
            this.materialLabel4.UseMnemonic = false;
            // 
            // lblLocalNetwork
            // 
            this.lblLocalNetwork.AutoSize = true;
            this.lblLocalNetwork.Depth = 0;
            this.lblLocalNetwork.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLocalNetwork.Location = new System.Drawing.Point(21, 175);
            this.lblLocalNetwork.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocalNetwork.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblLocalNetwork.Name = "lblLocalNetwork";
            this.lblLocalNetwork.Size = new System.Drawing.Size(67, 19);
            this.lblLocalNetwork.TabIndex = 30;
            this.lblLocalNetwork.Text = "Unknown";
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel3.HighEmphasis = true;
            this.materialLabel3.Location = new System.Drawing.Point(21, 153);
            this.materialLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.materialLabel3.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(106, 19);
            this.materialLabel3.TabIndex = 29;
            this.materialLabel3.Text = "Local Network:";
            this.materialLabel3.UseAccent = true;
            this.materialLabel3.UseMnemonic = false;
            // 
            // lblCurIP
            // 
            this.lblCurIP.AutoSize = true;
            this.lblCurIP.Depth = 0;
            this.lblCurIP.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCurIP.Location = new System.Drawing.Point(21, 114);
            this.lblCurIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurIP.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblCurIP.Name = "lblCurIP";
            this.lblCurIP.Size = new System.Drawing.Size(67, 19);
            this.lblCurIP.TabIndex = 28;
            this.lblCurIP.Text = "Unknown";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.HighEmphasis = true;
            this.materialLabel2.Location = new System.Drawing.Point(21, 94);
            this.materialLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.materialLabel2.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(119, 19);
            this.materialLabel2.TabIndex = 27;
            this.materialLabel2.Text = "Current Radio IP:";
            this.materialLabel2.UseAccent = true;
            this.materialLabel2.UseMnemonic = false;
            // 
            // lblCurProf
            // 
            this.lblCurProf.AutoSize = true;
            this.lblCurProf.Depth = 0;
            this.lblCurProf.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCurProf.Location = new System.Drawing.Point(21, 59);
            this.lblCurProf.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurProf.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblCurProf.Name = "lblCurProf";
            this.lblCurProf.Size = new System.Drawing.Size(67, 19);
            this.lblCurProf.TabIndex = 26;
            this.lblCurProf.Text = "Unknown";
            // 
            // lblCurProfmeh
            // 
            this.lblCurProfmeh.AutoSize = true;
            this.lblCurProfmeh.Depth = 0;
            this.lblCurProfmeh.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCurProfmeh.HighEmphasis = true;
            this.lblCurProfmeh.Location = new System.Drawing.Point(21, 36);
            this.lblCurProfmeh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurProfmeh.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblCurProfmeh.Name = "lblCurProfmeh";
            this.lblCurProfmeh.Size = new System.Drawing.Size(119, 19);
            this.lblCurProfmeh.TabIndex = 25;
            this.lblCurProfmeh.Text = "Current Protocol:";
            this.lblCurProfmeh.UseAccent = true;
            this.lblCurProfmeh.UseMnemonic = false;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(483, 10);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(29, 412);
            this.panel2.TabIndex = 31;
            // 
            // grpNetwork
            // 
            this.grpNetwork.Controls.Add(this.lblProt);
            this.grpNetwork.Controls.Add(this.lblDisabled);
            this.grpNetwork.Controls.Add(this.picWarnIP);
            this.grpNetwork.Controls.Add(this.picWarnProt);
            this.grpNetwork.Controls.Add(this.chkIP);
            this.grpNetwork.Controls.Add(this.pnlIP);
            this.grpNetwork.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpNetwork.Location = new System.Drawing.Point(248, 10);
            this.grpNetwork.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpNetwork.Name = "grpNetwork";
            this.grpNetwork.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpNetwork.Size = new System.Drawing.Size(235, 412);
            this.grpNetwork.TabIndex = 30;
            this.grpNetwork.TabStop = false;
            this.grpNetwork.Text = "Network Settings";
            // 
            // lblProt
            // 
            this.lblProt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblProt.AutoSize = true;
            this.lblProt.Depth = 0;
            this.lblProt.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblProt.HighEmphasis = true;
            this.lblProt.Location = new System.Drawing.Point(13, 270);
            this.lblProt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProt.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblProt.Name = "lblProt";
            this.lblProt.Size = new System.Drawing.Size(132, 19);
            this.lblProt.TabIndex = 28;
            this.lblProt.Text = "Preferred Protocol:";
            this.lblProt.UseAccent = true;
            this.lblProt.UseMnemonic = false;
            // 
            // lblDisabled
            // 
            this.lblDisabled.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDisabled.Depth = 0;
            this.lblDisabled.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDisabled.HighEmphasis = true;
            this.lblDisabled.Location = new System.Drawing.Point(13, 33);
            this.lblDisabled.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDisabled.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblDisabled.Name = "lblDisabled";
            this.lblDisabled.Size = new System.Drawing.Size(203, 45);
            this.lblDisabled.TabIndex = 27;
            this.lblDisabled.Text = "Power down the radio to change disabled options.";
            this.lblDisabled.UseAccent = true;
            this.lblDisabled.UseMnemonic = false;
            // 
            // picWarnIP
            // 
            this.picWarnIP.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picWarnIP.Image = global::CoolSDR.Properties.Resources.warning4;
            this.picWarnIP.Location = new System.Drawing.Point(191, 95);
            this.picWarnIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.picWarnIP.Name = "picWarnIP";
            this.picWarnIP.Size = new System.Drawing.Size(25, 18);
            this.picWarnIP.TabIndex = 26;
            this.picWarnIP.TabStop = false;
            this.picWarnIP.Visible = false;
            // 
            // picWarnProt
            // 
            this.picWarnProt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picWarnProt.Image = global::CoolSDR.Properties.Resources.warning4;
            this.picWarnProt.Location = new System.Drawing.Point(203, 270);
            this.picWarnProt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.picWarnProt.Name = "picWarnProt";
            this.picWarnProt.Size = new System.Drawing.Size(25, 18);
            this.picWarnProt.TabIndex = 25;
            this.picWarnProt.TabStop = false;
            this.picWarnProt.Visible = false;
            // 
            // chkIP
            // 
            this.chkIP.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkIP.AutoSize = true;
            this.chkIP.Depth = 0;
            this.chkIP.Location = new System.Drawing.Point(13, 94);
            this.chkIP.Margin = new System.Windows.Forms.Padding(0);
            this.chkIP.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkIP.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.chkIP.Name = "chkIP";
            this.chkIP.ReadOnly = false;
            this.chkIP.Ripple = true;
            this.chkIP.Size = new System.Drawing.Size(132, 37);
            this.chkIP.TabIndex = 23;
            this.chkIP.Text = "Use Known IP";
            this.chkIP.UseVisualStyleBackColor = true;
            // 
            // pnlIP
            // 
            this.pnlIP.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlIP.Controls.Add(this.textBoxFakeIP);
            this.pnlIP.Controls.Add(this.btnIP);
            this.pnlIP.Controls.Add(this.txtIP);
            this.pnlIP.Controls.Add(this.lblIP);
            this.pnlIP.Enabled = false;
            this.pnlIP.Location = new System.Drawing.Point(4, 146);
            this.pnlIP.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.pnlIP.Name = "pnlIP";
            this.pnlIP.Size = new System.Drawing.Size(209, 111);
            this.pnlIP.TabIndex = 0;
            // 
            // textBoxFakeIP
            // 
            this.textBoxFakeIP.Location = new System.Drawing.Point(13, 27);
            this.textBoxFakeIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxFakeIP.Name = "textBoxFakeIP";
            this.textBoxFakeIP.ReadOnly = true;
            this.textBoxFakeIP.ShortcutsEnabled = false;
            this.textBoxFakeIP.Size = new System.Drawing.Size(172, 20);
            this.textBoxFakeIP.TabIndex = 25;
            this.textBoxFakeIP.Text = "192.168.1.5";
            this.textBoxFakeIP.Visible = false;
            // 
            // btnIP
            // 
            this.btnIP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnIP.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnIP.Depth = 0;
            this.btnIP.HighEmphasis = true;
            this.btnIP.Icon = null;
            this.btnIP.Location = new System.Drawing.Point(13, 64);
            this.btnIP.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnIP.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnIP.Name = "btnIP";
            this.btnIP.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnIP.Size = new System.Drawing.Size(84, 36);
            this.btnIP.TabIndex = 23;
            this.btnIP.Text = "Apply IP";
            this.btnIP.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnIP.UseAccentColor = false;
            this.btnIP.UseVisualStyleBackColor = true;
            // 
            // txtIP
            // 
            this.txtIP.Enabled = false;
            this.txtIP.Location = new System.Drawing.Point(13, 27);
            this.txtIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(172, 20);
            this.txtIP.TabIndex = 22;
            this.txtIP.Text = "0.0.0.0";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Depth = 0;
            this.lblIP.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblIP.Location = new System.Drawing.Point(13, 5);
            this.lblIP.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(125, 19);
            this.lblIP.TabIndex = 21;
            this.lblIP.Text = "Radio IP Address:";
            this.lblIP.UseAccent = true;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(219, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(29, 412);
            this.panel1.TabIndex = 28;
            // 
            // grpRadioSel
            // 
            this.grpRadioSel.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpRadioSel.Location = new System.Drawing.Point(11, 10);
            this.grpRadioSel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpRadioSel.Name = "grpRadioSel";
            this.grpRadioSel.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpRadioSel.Size = new System.Drawing.Size(208, 412);
            this.grpRadioSel.TabIndex = 27;
            this.grpRadioSel.TabStop = false;
            this.grpRadioSel.Text = "Radio Selection";
            // 
            // pnlBot
            // 
            this.pnlBot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBot.Location = new System.Drawing.Point(11, 422);
            this.pnlBot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBot.Name = "pnlBot";
            this.pnlBot.Size = new System.Drawing.Size(794, 26);
            this.pnlBot.TabIndex = 26;
            // 
            // visualStudioToolStripExtender1
            // 
            this.visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // toolTipRadio
            // 
            this.toolTipRadio.AutoPopDelay = 3000;
            this.toolTipRadio.InitialDelay = 500;
            this.toolTipRadio.IsBalloon = true;
            this.toolTipRadio.ReshowDelay = 100;
            this.toolTipRadio.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // ucRadio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpRadio);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ucRadio";
            this.Size = new System.Drawing.Size(862, 538);
            this.grpRadio.ResumeLayout(false);
            this.RadioTabControl.ResumeLayout(false);
            this.tabRadioGeneral.ResumeLayout(false);
            this.pnlGen.ResumeLayout(false);
            this.grpIPInfo.ResumeLayout(false);
            this.grpIPInfo.PerformLayout();
            this.grpNetwork.ResumeLayout(false);
            this.grpNetwork.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWarnIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWarnProt)).EndInit();
            this.pnlIP.ResumeLayout(false);
            this.pnlIP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpRadio;
        private MaterialSkin2DotNet.Controls.MaterialTabControl RadioTabControl;
        private System.Windows.Forms.TabPage tabRadioGeneral;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender visualStudioToolStripExtender1;
        private System.Windows.Forms.Panel pnlGen;
        private System.Windows.Forms.GroupBox grpRadioSel;
        private System.Windows.Forms.Panel pnlBot;
        private System.Windows.Forms.GroupBox grpIPInfo;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel5;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblMac;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel4;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblLocalNetwork;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel3;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblCurIP;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel2;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblCurProf;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblCurProfmeh;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox grpNetwork;
        private MaterialSkin2DotNet.Controls.MaterialCheckbox chkIP;
        private System.Windows.Forms.Panel pnlIP;
        private System.Windows.Forms.TextBox textBoxFakeIP;
        private MaterialSkin2DotNet.Controls.MaterialButton btnIP;
        private CoolComponents.IPControl txtIP;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblIP;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblHardware;
        private System.Windows.Forms.PictureBox picWarnProt;
        private System.Windows.Forms.ToolTip toolTipRadio;
        private System.Windows.Forms.PictureBox picWarnIP;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblProt;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblDisabled;
    }
}
