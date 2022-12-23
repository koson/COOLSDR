using System;
using System.Windows;

namespace CoolSDR.Forms
{
    partial class FrmMain
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
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch (Exception) { }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            CoolComponents.FrequencyManager frequencyManager1 = new CoolComponents.FrequencyManager();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.coolVFO1 = new CoolComponents.VFOControl();
            this.Status = new Krypton.Toolkit.KryptonStatusStrip();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.errorProvFreq = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlSep = new System.Windows.Forms.Panel();
            this.pnlTop = new Krypton.Toolkit.KryptonPanel();
            this.kryptonCheckButton1 = new Krypton.Toolkit.KryptonCheckButton();
            this.UDRX1StepAttData = new System.Windows.Forms.NumericUpDownTS();
            this.ComboMeterTXMode = new System.Windows.Forms.ComboBoxTS();
            this.ComboMeterRXMode = new System.Windows.Forms.ComboBoxTS();
            this.ComboPreamp = new System.Windows.Forms.ComboBoxTS();
            this.sldPower = new MaterialSkin2DotNet.Controls.MaterialSlider();
            this.chkPower = new MaterialSkin2DotNet.Controls.MaterialSwitch();
            this.pnlView = new Krypton.Toolkit.KryptonPanel();
            this.tabMnu = new MaterialSkin2DotNet.Controls.MaterialTabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.pnlLoadProgress = new System.Windows.Forms.Panel();
            this.pbLoading = new CoolComponents.CoolProgressBar();
            this.lblLoadProgress = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.picDisplay = new CoolSDR.Forms.MyPictureBox();
            this.tabAudio = new System.Windows.Forms.TabPage();
            this.ucAudio1 = new CoolSDR.CustomUserControls.ucAudio();
            this.tabRadio = new System.Windows.Forms.TabPage();
            this.ucRadio1 = new CoolSDR.CustomUserControls.ucRadio();
            this.tabRegion = new System.Windows.Forms.TabPage();
            this.ucBands1 = new CoolSDR.CustomUserControls.ucBands();
            this.tabLooknFeel = new System.Windows.Forms.TabPage();
            this.imageListMnu = new System.Windows.Forms.ImageList(this.components);
            this.Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTop)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UDRX1StepAttData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlView)).BeginInit();
            this.pnlView.SuspendLayout();
            this.tabMnu.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.pnlLoadProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.tabAudio.SuspendLayout();
            this.tabRadio.SuspendLayout();
            this.tabRegion.SuspendLayout();
            this.SuspendLayout();
            // 
            // coolVFO1
            // 
            this.coolVFO1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.coolVFO1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.coolVFO1.Digit = -1;
            this.coolVFO1.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coolVFO1.FontOverride = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coolVFO1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.coolVFO1.FreqInMHz = 27.555D;
            frequencyManager1.Buddy = this.coolVFO1;
            frequencyManager1.FreqInHz = ((long)(27555000));
            frequencyManager1.FreqInMHz = 27.555D;
            frequencyManager1.Max = ((long)(50000000));
            frequencyManager1.Min = ((long)(50000));
            this.coolVFO1.FreqManager = frequencyManager1;
            this.coolVFO1.FrequencyDefaultMHz = 27.555D;
            this.coolVFO1.HoverUnderline = true;
            this.coolVFO1.IVFO = null;
            this.coolVFO1.Location = new System.Drawing.Point(704, 7);
            this.coolVFO1.Name = "coolVFO1";
            this.coolVFO1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.coolVFO1.Size = new System.Drawing.Size(275, 57);
            this.coolVFO1.TabIndex = 143;
            this.coolVFO1.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.coolVFO1.UnderlineColor = System.Drawing.SystemColors.WindowText;
            this.coolVFO1.UnderlineDecimals = false;
            this.coolVFO1.UnderlineThickness = 3;
            this.coolVFO1.Value = ((long)(27555000));
            // 
            // Status
            // 
            this.Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Status.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Status.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1});
            this.Status.Location = new System.Drawing.Point(3, 591);
            this.Status.Name = "Status";
            this.Status.ProgressBars = null;
            this.Status.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.Status.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Status.Size = new System.Drawing.Size(1160, 22);
            this.Status.TabIndex = 1;
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(1145, 17);
            this.StatusLabel1.Spring = true;
            this.StatusLabel1.Text = "StatusLabel1";
            this.StatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // errorProvFreq
            // 
            this.errorProvFreq.ContainerControl = this;
            // 
            // pnlSep
            // 
            this.pnlSep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlSep.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSep.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pnlSep.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlSep.Location = new System.Drawing.Point(3, 550);
            this.pnlSep.Name = "pnlSep";
            this.pnlSep.Size = new System.Drawing.Size(1160, 41);
            this.pnlSep.TabIndex = 3;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlTop.Controls.Add(this.kryptonCheckButton1);
            this.pnlTop.Controls.Add(this.coolVFO1);
            this.pnlTop.Controls.Add(this.UDRX1StepAttData);
            this.pnlTop.Controls.Add(this.ComboMeterTXMode);
            this.pnlTop.Controls.Add(this.ComboMeterRXMode);
            this.pnlTop.Controls.Add(this.ComboPreamp);
            this.pnlTop.Controls.Add(this.sldPower);
            this.pnlTop.Controls.Add(this.chkPower);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Enabled = false;
            this.pnlTop.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pnlTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlTop.Location = new System.Drawing.Point(3, 64);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(0, 10, 15, 10);
            this.pnlTop.Size = new System.Drawing.Size(1160, 70);
            this.pnlTop.StateCommon.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlTop.StateCommon.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlTop.TabIndex = 4;
            // 
            // kryptonCheckButton1
            // 
            this.kryptonCheckButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonCheckButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.kryptonCheckButton1.CornerRoundingRadius = 2F;
            this.kryptonCheckButton1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.kryptonCheckButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.kryptonCheckButton1.Location = new System.Drawing.Point(305, 18);
            this.kryptonCheckButton1.Name = "kryptonCheckButton1";
            this.kryptonCheckButton1.Size = new System.Drawing.Size(90, 25);
            this.kryptonCheckButton1.StateCommon.Back.Color1 = System.Drawing.Color.Gray;
            this.kryptonCheckButton1.StateCommon.Back.Color2 = System.Drawing.Color.DarkGray;
            this.kryptonCheckButton1.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonCheckButton1.StateCommon.Border.Rounding = 2F;
            this.kryptonCheckButton1.TabIndex = 144;
            this.kryptonCheckButton1.Values.Text = "MOX";
            this.kryptonCheckButton1.Click += new System.EventHandler(this.kryptonCheckButton1_Click);
            // 
            // UDRX1StepAttData
            // 
            this.UDRX1StepAttData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.UDRX1StepAttData.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.UDRX1StepAttData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.UDRX1StepAttData.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UDRX1StepAttData.Location = new System.Drawing.Point(225, 19);
            this.UDRX1StepAttData.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.UDRX1StepAttData.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.UDRX1StepAttData.Name = "UDRX1StepAttData";
            this.UDRX1StepAttData.Size = new System.Drawing.Size(55, 24);
            this.UDRX1StepAttData.TabIndex = 142;
            this.UDRX1StepAttData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UDRX1StepAttData.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.UDRX1StepAttData.Visible = false;
            // 
            // ComboMeterTXMode
            // 
            this.ComboMeterTXMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ComboMeterTXMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboMeterTXMode.DropDownWidth = 72;
            this.ComboMeterTXMode.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboMeterTXMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ComboMeterTXMode.ItemHeight = 17;
            this.ComboMeterTXMode.Location = new System.Drawing.Point(147, 18);
            this.ComboMeterTXMode.MaxDropDownItems = 12;
            this.ComboMeterTXMode.Name = "ComboMeterTXMode";
            this.ComboMeterTXMode.Size = new System.Drawing.Size(72, 25);
            this.ComboMeterTXMode.TabIndex = 64;
            this.ComboMeterTXMode.Visible = false;
            // 
            // ComboMeterRXMode
            // 
            this.ComboMeterRXMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ComboMeterRXMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboMeterRXMode.DropDownWidth = 72;
            this.ComboMeterRXMode.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboMeterRXMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ComboMeterRXMode.ItemHeight = 17;
            this.ComboMeterRXMode.Location = new System.Drawing.Point(71, 18);
            this.ComboMeterRXMode.Name = "ComboMeterRXMode";
            this.ComboMeterRXMode.Size = new System.Drawing.Size(72, 25);
            this.ComboMeterRXMode.TabIndex = 63;
            this.ComboMeterRXMode.Visible = false;
            // 
            // ComboPreamp
            // 
            this.ComboPreamp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ComboPreamp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboPreamp.DropDownWidth = 48;
            this.ComboPreamp.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboPreamp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ComboPreamp.ItemHeight = 17;
            this.ComboPreamp.Location = new System.Drawing.Point(10, 18);
            this.ComboPreamp.Name = "ComboPreamp";
            this.ComboPreamp.Size = new System.Drawing.Size(55, 25);
            this.ComboPreamp.TabIndex = 40;
            this.ComboPreamp.Visible = false;
            // 
            // sldPower
            // 
            this.sldPower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sldPower.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.sldPower.Depth = 0;
            this.sldPower.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sldPower.Location = new System.Drawing.Point(428, 9);
            this.sldPower.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.sldPower.Name = "sldPower";
            this.sldPower.Size = new System.Drawing.Size(250, 40);
            this.sldPower.TabIndex = 6;
            this.sldPower.Text = "Power";
            this.sldPower.UseAccentColor = true;
            this.sldPower.ValueMax = 100;
            this.sldPower.ValueSuffix = "%";
            // 
            // chkPower
            // 
            this.chkPower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPower.AutoSize = true;
            this.chkPower.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.chkPower.Depth = 0;
            this.chkPower.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chkPower.Location = new System.Drawing.Point(998, 12);
            this.chkPower.Margin = new System.Windows.Forms.Padding(0);
            this.chkPower.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkPower.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.chkPower.Name = "chkPower";
            this.chkPower.Ripple = true;
            this.chkPower.Size = new System.Drawing.Size(147, 37);
            this.chkPower.TabIndex = 3;
            this.chkPower.Text = "Radio Power";
            this.chkPower.UseVisualStyleBackColor = false;
            // 
            // pnlView
            // 
            this.pnlView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlView.Controls.Add(this.tabMnu);
            this.pnlView.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.pnlView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlView.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pnlView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlView.Location = new System.Drawing.Point(3, 134);
            this.pnlView.Name = "pnlView";
            this.pnlView.Size = new System.Drawing.Size(1160, 416);
            this.pnlView.StateNormal.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlView.TabIndex = 5;
            // 
            // tabMnu
            // 
            this.tabMnu.Controls.Add(this.tabPageMain);
            this.tabMnu.Controls.Add(this.tabAudio);
            this.tabMnu.Controls.Add(this.tabRadio);
            this.tabMnu.Controls.Add(this.tabRegion);
            this.tabMnu.Controls.Add(this.tabLooknFeel);
            this.tabMnu.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tabMnu.Depth = 0;
            this.tabMnu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMnu.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tabMnu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabMnu.HotTrack = true;
            this.tabMnu.ImageList = this.imageListMnu;
            this.tabMnu.Location = new System.Drawing.Point(0, 0);
            this.tabMnu.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.tabMnu.Multiline = true;
            this.tabMnu.Name = "tabMnu";
            this.tabMnu.SelectedIndex = 0;
            this.tabMnu.Size = new System.Drawing.Size(1160, 416);
            this.tabMnu.TabIndex = 5;
            // 
            // tabPageMain
            // 
            this.tabPageMain.AutoScroll = true;
            this.tabPageMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabPageMain.Controls.Add(this.pnlLoadProgress);
            this.tabPageMain.Controls.Add(this.picDisplay);
            this.tabPageMain.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageMain.ImageKey = "house.png";
            this.tabPageMain.Location = new System.Drawing.Point(4, 26);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(1152, 386);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "Main";
            this.tabPageMain.ToolTipText = "Main App Page";
            // 
            // pnlLoadProgress
            // 
            this.pnlLoadProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlLoadProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlLoadProgress.Controls.Add(this.pbLoading);
            this.pnlLoadProgress.Controls.Add(this.lblLoadProgress);
            this.pnlLoadProgress.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pnlLoadProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlLoadProgress.Location = new System.Drawing.Point(301, 83);
            this.pnlLoadProgress.Name = "pnlLoadProgress";
            this.pnlLoadProgress.Size = new System.Drawing.Size(566, 227);
            this.pnlLoadProgress.TabIndex = 6;
            // 
            // pbLoading
            // 
            this.pbLoading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pbLoading.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pbLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pbLoading.Location = new System.Drawing.Point(22, 126);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(315, 18);
            this.pbLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbLoading.TabIndex = 14;
            // 
            // lblLoadProgress
            // 
            this.lblLoadProgress.AutoSize = true;
            this.lblLoadProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblLoadProgress.Depth = 0;
            this.lblLoadProgress.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLoadProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblLoadProgress.Location = new System.Drawing.Point(29, 90);
            this.lblLoadProgress.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblLoadProgress.Name = "lblLoadProgress";
            this.lblLoadProgress.Size = new System.Drawing.Size(75, 19);
            this.lblLoadProgress.TabIndex = 8;
            this.lblLoadProgress.Text = "Loading ...";
            // 
            // picDisplay
            // 
            this.picDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.picDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picDisplay.Enabled = false;
            this.picDisplay.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.picDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.picDisplay.Location = new System.Drawing.Point(3, 3);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(1146, 380);
            this.picDisplay.TabIndex = 4;
            this.picDisplay.TabStop = false;
            // 
            // tabAudio
            // 
            this.tabAudio.AutoScroll = true;
            this.tabAudio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabAudio.Controls.Add(this.ucAudio1);
            this.tabAudio.ImageKey = "headphones.png";
            this.tabAudio.Location = new System.Drawing.Point(4, 26);
            this.tabAudio.Name = "tabAudio";
            this.tabAudio.Padding = new System.Windows.Forms.Padding(3);
            this.tabAudio.Size = new System.Drawing.Size(1152, 386);
            this.tabAudio.TabIndex = 1;
            this.tabAudio.Text = "Audio Routing";
            this.tabAudio.ToolTipText = "Audio Routing ...";
            // 
            // ucAudio1
            // 
            this.ucAudio1.AutoScroll = true;
            this.ucAudio1.AutoScrollMinSize = new System.Drawing.Size(920, 405);
            this.ucAudio1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ucAudio1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucAudio1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucAudio1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucAudio1.Location = new System.Drawing.Point(3, 3);
            this.ucAudio1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ucAudio1.Name = "ucAudio1";
            this.ucAudio1.Settings = null;
            this.ucAudio1.Size = new System.Drawing.Size(1146, 380);
            this.ucAudio1.TabIndex = 0;
            // 
            // tabRadio
            // 
            this.tabRadio.AutoScroll = true;
            this.tabRadio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabRadio.Controls.Add(this.ucRadio1);
            this.tabRadio.ImageKey = "network-wired.png";
            this.tabRadio.Location = new System.Drawing.Point(4, 26);
            this.tabRadio.Name = "tabRadio";
            this.tabRadio.Padding = new System.Windows.Forms.Padding(3);
            this.tabRadio.Size = new System.Drawing.Size(1152, 386);
            this.tabRadio.TabIndex = 2;
            this.tabRadio.Text = "Network Radio";
            this.tabRadio.ToolTipText = "Set up Radio";
            // 
            // ucRadio1
            // 
            this.ucRadio1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ucRadio1.CurrentHPSDRModel = Thetis.HPSDRModel.HPSDR;
            this.ucRadio1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRadio1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucRadio1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucRadio1.Location = new System.Drawing.Point(3, 3);
            this.ucRadio1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ucRadio1.Name = "ucRadio1";
            this.ucRadio1.Size = new System.Drawing.Size(1146, 380);
            this.ucRadio1.TabIndex = 0;
            this.ucRadio1.Load += new System.EventHandler(this.ucRadio1_Load);
            // 
            // tabRegion
            // 
            this.tabRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabRegion.Controls.Add(this.ucBands1);
            this.tabRegion.ImageKey = "address-card.png";
            this.tabRegion.Location = new System.Drawing.Point(4, 26);
            this.tabRegion.Name = "tabRegion";
            this.tabRegion.Padding = new System.Windows.Forms.Padding(20);
            this.tabRegion.Size = new System.Drawing.Size(1152, 386);
            this.tabRegion.TabIndex = 3;
            this.tabRegion.Text = "Region & Bands";
            this.tabRegion.ToolTipText = "Region/Bands ...";
            // 
            // ucBands1
            // 
            this.ucBands1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ucBands1.Bands = null;
            this.ucBands1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBands1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBands1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucBands1.Location = new System.Drawing.Point(20, 20);
            this.ucBands1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucBands1.Name = "ucBands1";
            this.ucBands1.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
            this.ucBands1.Size = new System.Drawing.Size(1112, 346);
            this.ucBands1.TabIndex = 0;
            // 
            // tabLooknFeel
            // 
            this.tabLooknFeel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabLooknFeel.ImageKey = "eye.png";
            this.tabLooknFeel.Location = new System.Drawing.Point(4, 26);
            this.tabLooknFeel.Name = "tabLooknFeel";
            this.tabLooknFeel.Padding = new System.Windows.Forms.Padding(3);
            this.tabLooknFeel.Size = new System.Drawing.Size(1152, 386);
            this.tabLooknFeel.TabIndex = 4;
            this.tabLooknFeel.Text = "Look n Feel";
            this.tabLooknFeel.ToolTipText = "Adjust Appearance ...";
            // 
            // imageListMnu
            // 
            this.imageListMnu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMnu.ImageStream")));
            this.imageListMnu.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMnu.Images.SetKeyName(0, "house.png");
            this.imageListMnu.Images.SetKeyName(1, "headphones.png");
            this.imageListMnu.Images.SetKeyName(2, "network-wired.png");
            this.imageListMnu.Images.SetKeyName(3, "address-card.png");
            this.imageListMnu.Images.SetKeyName(4, "eye.png");
            // 
            // FrmMain
            // 
            this.AccentColor = MaterialSkin2DotNet.Accent.LightBlue700;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 616);
            this.Controls.Add(this.pnlView);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlSep);
            this.Controls.Add(this.Status);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.DrawerBackgroundWithAccent = true;
            this.DrawerIndicatorWidth = 1;
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.tabMnu;
            this.DrawerUseColors = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.FormToManage = this;
            this.MinimumSize = new System.Drawing.Size(677, 456);
            this.Name = "FrmMain";
            this.Opacity = 0D;
            this.PrimaryColor = MaterialSkin2DotNet.Primary.BlueGrey800;
            this.PrimaryDarkColor = MaterialSkin2DotNet.Primary.BlueGrey900;
            this.PrimaryLightColor = MaterialSkin2DotNet.Primary.BlueGrey200;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CoolSDR";
            this.Theme = MaterialSkin2DotNet.MaterialSkinManager.Themes.DARK;
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTop)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UDRX1StepAttData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlView)).EndInit();
            this.pnlView.ResumeLayout(false);
            this.tabMnu.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.pnlLoadProgress.ResumeLayout(false);
            this.pnlLoadProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.tabAudio.ResumeLayout(false);
            this.tabRadio.ResumeLayout(false);
            this.tabRegion.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Krypton.Toolkit.KryptonStatusStrip Status;
        private System.Windows.Forms.ErrorProvider errorProvFreq;
        private System.Windows.Forms.Panel pnlSep;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
        private Krypton.Toolkit.KryptonPanel pnlTop;
        private System.Windows.Forms.NumericUpDownTS UDRX1StepAttData;
        private System.Windows.Forms.ComboBoxTS ComboMeterTXMode;
        private System.Windows.Forms.ComboBoxTS ComboMeterRXMode;
        private System.Windows.Forms.ComboBoxTS ComboPreamp;
        private MaterialSkin2DotNet.Controls.MaterialSlider sldPower;
        private MaterialSkin2DotNet.Controls.MaterialSwitch chkPower;
        private Krypton.Toolkit.KryptonPanel pnlView;
        private MaterialSkin2DotNet.Controls.MaterialTabControl tabMnu;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabPage tabAudio;
        private CustomUserControls.ucAudio ucAudio1;
        private System.Windows.Forms.TabPage tabRadio;
        private CustomUserControls.ucRadio ucRadio1;
        private MyPictureBox picDisplay;
        private CoolComponents.VFOControl coolVFO1;
        private System.Windows.Forms.Panel pnlLoadProgress;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblLoadProgress;
        private CoolComponents.CoolProgressBar pbLoading;
        private System.Windows.Forms.TabPage tabRegion;
        private CustomUserControls.ucBands ucBands1;
        private Krypton.Toolkit.KryptonCheckButton kryptonCheckButton1;
        private System.Windows.Forms.ImageList imageListMnu;
        private System.Windows.Forms.TabPage tabLooknFeel;
    }
}