namespace CoolSDR.CustomUserControls
{
    partial class ucAudio
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
            this.grpVAC = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblDisabled = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmdTest = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.btnApplyAudio = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.btnAudioReset = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.cboBufferSize = new MaterialSkin2DotNet.Controls.MaterialComboBox();
            this.materialLabel2 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.cboSampleRate = new MaterialSkin2DotNet.Controls.MaterialComboBox();
            this.materialLabel1 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.cboAudioOut = new MaterialSkin2DotNet.Controls.MaterialComboBox();
            this.cboAudioIn = new MaterialSkin2DotNet.Controls.MaterialComboBox();
            this.cboAPI = new MaterialSkin2DotNet.Controls.MaterialComboBox();
            this.lblAudioOut = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.lblAudioIn = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.lblAPI = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkExclusive = new MaterialSkin2DotNet.Controls.MaterialCheckbox();
            this.grpVAC.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpVAC
            // 
            this.grpVAC.Controls.Add(this.panel3);
            this.grpVAC.Controls.Add(this.lblDisabled);
            this.grpVAC.Controls.Add(this.panel2);
            this.grpVAC.Controls.Add(this.panel1);
            this.grpVAC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpVAC.Location = new System.Drawing.Point(0, 0);
            this.grpVAC.Name = "grpVAC";
            this.grpVAC.Size = new System.Drawing.Size(633, 393);
            this.grpVAC.TabIndex = 1;
            this.grpVAC.TabStop = false;
            this.grpVAC.Text = "Virtual Audio Cable (VAC) Settings";
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(610, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(20, 374);
            this.panel3.TabIndex = 5;
            // 
            // lblDisabled
            // 
            this.lblDisabled.AutoSize = true;
            this.lblDisabled.Depth = 0;
            this.lblDisabled.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDisabled.Location = new System.Drawing.Point(5, 0);
            this.lblDisabled.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDisabled.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblDisabled.Name = "lblDisabled";
            this.lblDisabled.Size = new System.Drawing.Size(418, 19);
            this.lblDisabled.TabIndex = 27;
            this.lblDisabled.Text = "These settings are enabled when you power down the radio";
            this.lblDisabled.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkExclusive);
            this.panel2.Controls.Add(this.cmdTest);
            this.panel2.Controls.Add(this.btnApplyAudio);
            this.panel2.Controls.Add(this.btnAudioReset);
            this.panel2.Controls.Add(this.cboBufferSize);
            this.panel2.Controls.Add(this.materialLabel2);
            this.panel2.Controls.Add(this.cboSampleRate);
            this.panel2.Controls.Add(this.materialLabel1);
            this.panel2.Controls.Add(this.cboAudioOut);
            this.panel2.Controls.Add(this.cboAudioIn);
            this.panel2.Controls.Add(this.cboAPI);
            this.panel2.Controls.Add(this.lblAudioOut);
            this.panel2.Controls.Add(this.lblAudioIn);
            this.panel2.Controls.Add(this.lblAPI);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(23, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(607, 374);
            this.panel2.TabIndex = 4;
            // 
            // cmdTest
            // 
            this.cmdTest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cmdTest.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.cmdTest.Depth = 0;
            this.cmdTest.HighEmphasis = true;
            this.cmdTest.Icon = null;
            this.cmdTest.Location = new System.Drawing.Point(215, 316);
            this.cmdTest.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.cmdTest.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.cmdTest.Name = "cmdTest";
            this.cmdTest.NoAccentTextColor = System.Drawing.Color.Empty;
            this.cmdTest.Size = new System.Drawing.Size(187, 36);
            this.cmdTest.TabIndex = 28;
            this.cmdTest.Text = "&Test These Settings";
            this.cmdTest.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.cmdTest.UseAccentColor = false;
            this.cmdTest.UseVisualStyleBackColor = true;
            this.cmdTest.Click += new System.EventHandler(this.cmdTest_Click);
            // 
            // btnApplyAudio
            // 
            this.btnApplyAudio.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApplyAudio.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnApplyAudio.Depth = 0;
            this.btnApplyAudio.HighEmphasis = true;
            this.btnApplyAudio.Icon = null;
            this.btnApplyAudio.Location = new System.Drawing.Point(416, 316);
            this.btnApplyAudio.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnApplyAudio.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnApplyAudio.Name = "btnApplyAudio";
            this.btnApplyAudio.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnApplyAudio.Size = new System.Drawing.Size(147, 36);
            this.btnApplyAudio.TabIndex = 25;
            this.btnApplyAudio.Text = "&Apply Changes";
            this.btnApplyAudio.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnApplyAudio.UseAccentColor = false;
            this.btnApplyAudio.UseVisualStyleBackColor = true;
            this.btnApplyAudio.Click += new System.EventHandler(this.btnApplyAudio_Click);
            // 
            // btnAudioReset
            // 
            this.btnAudioReset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAudioReset.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnAudioReset.Depth = 0;
            this.btnAudioReset.HighEmphasis = true;
            this.btnAudioReset.Icon = null;
            this.btnAudioReset.Location = new System.Drawing.Point(23, 316);
            this.btnAudioReset.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAudioReset.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnAudioReset.Name = "btnAudioReset";
            this.btnAudioReset.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnAudioReset.Size = new System.Drawing.Size(175, 36);
            this.btnAudioReset.TabIndex = 24;
            this.btnAudioReset.Text = "Fail safe &defaults";
            this.btnAudioReset.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAudioReset.UseAccentColor = false;
            this.btnAudioReset.UseVisualStyleBackColor = true;
            this.btnAudioReset.Click += new System.EventHandler(this.btnAudioReset_Click);
            // 
            // cboBufferSize
            // 
            this.cboBufferSize.AutoResize = false;
            this.cboBufferSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cboBufferSize.Depth = 0;
            this.cboBufferSize.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboBufferSize.DropDownHeight = 174;
            this.cboBufferSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBufferSize.DropDownWidth = 121;
            this.cboBufferSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cboBufferSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboBufferSize.FormattingEnabled = true;
            this.cboBufferSize.IntegralHeight = false;
            this.cboBufferSize.ItemHeight = 43;
            this.cboBufferSize.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cboBufferSize.Location = new System.Drawing.Point(435, 53);
            this.cboBufferSize.MaxDropDownItems = 4;
            this.cboBufferSize.MouseState = MaterialSkin2DotNet.MouseState.OUT;
            this.cboBufferSize.Name = "cboBufferSize";
            this.cboBufferSize.Size = new System.Drawing.Size(128, 49);
            this.cboBufferSize.StartIndex = 0;
            this.cboBufferSize.TabIndex = 23;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(435, 22);
            this.materialLabel2.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(79, 19);
            this.materialLabel2.TabIndex = 22;
            this.materialLabel2.Text = "Buffer Size";
            // 
            // cboSampleRate
            // 
            this.cboSampleRate.AutoResize = false;
            this.cboSampleRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cboSampleRate.Depth = 0;
            this.cboSampleRate.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboSampleRate.DropDownHeight = 174;
            this.cboSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSampleRate.DropDownWidth = 121;
            this.cboSampleRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cboSampleRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboSampleRate.FormattingEnabled = true;
            this.cboSampleRate.IntegralHeight = false;
            this.cboSampleRate.ItemHeight = 43;
            this.cboSampleRate.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cboSampleRate.Location = new System.Drawing.Point(294, 53);
            this.cboSampleRate.MaxDropDownItems = 4;
            this.cboSampleRate.MouseState = MaterialSkin2DotNet.MouseState.OUT;
            this.cboSampleRate.Name = "cboSampleRate";
            this.cboSampleRate.Size = new System.Drawing.Size(108, 49);
            this.cboSampleRate.StartIndex = 0;
            this.cboSampleRate.TabIndex = 21;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(294, 22);
            this.materialLabel1.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(87, 19);
            this.materialLabel1.TabIndex = 20;
            this.materialLabel1.Text = "SampleRate";
            // 
            // cboAudioOut
            // 
            this.cboAudioOut.AutoResize = false;
            this.cboAudioOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cboAudioOut.Depth = 0;
            this.cboAudioOut.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboAudioOut.DropDownHeight = 174;
            this.cboAudioOut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAudioOut.DropDownWidth = 121;
            this.cboAudioOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cboAudioOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboAudioOut.FormattingEnabled = true;
            this.cboAudioOut.IntegralHeight = false;
            this.cboAudioOut.ItemHeight = 43;
            this.cboAudioOut.Items.AddRange(new object[] {
            "Default"});
            this.cboAudioOut.Location = new System.Drawing.Point(23, 246);
            this.cboAudioOut.MaxDropDownItems = 4;
            this.cboAudioOut.MouseState = MaterialSkin2DotNet.MouseState.OUT;
            this.cboAudioOut.Name = "cboAudioOut";
            this.cboAudioOut.Size = new System.Drawing.Size(540, 49);
            this.cboAudioOut.StartIndex = 0;
            this.cboAudioOut.TabIndex = 18;
            // 
            // cboAudioIn
            // 
            this.cboAudioIn.AutoResize = false;
            this.cboAudioIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cboAudioIn.Depth = 0;
            this.cboAudioIn.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboAudioIn.DropDownHeight = 174;
            this.cboAudioIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAudioIn.DropDownWidth = 121;
            this.cboAudioIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cboAudioIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboAudioIn.FormattingEnabled = true;
            this.cboAudioIn.IntegralHeight = false;
            this.cboAudioIn.ItemHeight = 43;
            this.cboAudioIn.Items.AddRange(new object[] {
            "Default"});
            this.cboAudioIn.Location = new System.Drawing.Point(23, 159);
            this.cboAudioIn.MaxDropDownItems = 4;
            this.cboAudioIn.MouseState = MaterialSkin2DotNet.MouseState.OUT;
            this.cboAudioIn.Name = "cboAudioIn";
            this.cboAudioIn.Size = new System.Drawing.Size(540, 49);
            this.cboAudioIn.StartIndex = 0;
            this.cboAudioIn.TabIndex = 17;
            // 
            // cboAPI
            // 
            this.cboAPI.AutoResize = false;
            this.cboAPI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cboAPI.Depth = 0;
            this.cboAPI.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboAPI.DropDownHeight = 174;
            this.cboAPI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAPI.DropDownWidth = 121;
            this.cboAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cboAPI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboAPI.FormattingEnabled = true;
            this.cboAPI.IntegralHeight = false;
            this.cboAPI.ItemHeight = 43;
            this.cboAPI.Items.AddRange(new object[] {
            "Default"});
            this.cboAPI.Location = new System.Drawing.Point(23, 53);
            this.cboAPI.MaxDropDownItems = 4;
            this.cboAPI.MouseState = MaterialSkin2DotNet.MouseState.OUT;
            this.cboAPI.Name = "cboAPI";
            this.cboAPI.Size = new System.Drawing.Size(240, 49);
            this.cboAPI.StartIndex = 0;
            this.cboAPI.TabIndex = 16;
            // 
            // lblAudioOut
            // 
            this.lblAudioOut.AutoSize = true;
            this.lblAudioOut.Depth = 0;
            this.lblAudioOut.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAudioOut.Location = new System.Drawing.Point(23, 224);
            this.lblAudioOut.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblAudioOut.Name = "lblAudioOut";
            this.lblAudioOut.Size = new System.Drawing.Size(94, 19);
            this.lblAudioOut.TabIndex = 15;
            this.lblAudioOut.Text = "Audio Output";
            // 
            // lblAudioIn
            // 
            this.lblAudioIn.AutoSize = true;
            this.lblAudioIn.Depth = 0;
            this.lblAudioIn.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAudioIn.Location = new System.Drawing.Point(23, 128);
            this.lblAudioIn.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblAudioIn.Name = "lblAudioIn";
            this.lblAudioIn.Size = new System.Drawing.Size(82, 19);
            this.lblAudioIn.TabIndex = 14;
            this.lblAudioIn.Text = "Audio Input";
            // 
            // lblAPI
            // 
            this.lblAPI.AutoSize = true;
            this.lblAPI.Depth = 0;
            this.lblAPI.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAPI.Location = new System.Drawing.Point(23, 22);
            this.lblAPI.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblAPI.Name = "lblAPI";
            this.lblAPI.Size = new System.Drawing.Size(70, 19);
            this.lblAPI.TabIndex = 13;
            this.lblAPI.Text = "Audio API";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(20, 374);
            this.panel1.TabIndex = 2;
            // 
            // chkExclusive
            // 
            this.chkExclusive.AutoSize = true;
            this.chkExclusive.Depth = 0;
            this.chkExclusive.Location = new System.Drawing.Point(294, 110);
            this.chkExclusive.Margin = new System.Windows.Forms.Padding(0);
            this.chkExclusive.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkExclusive.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.chkExclusive.Name = "chkExclusive";
            this.chkExclusive.ReadOnly = false;
            this.chkExclusive.Ripple = true;
            this.chkExclusive.Size = new System.Drawing.Size(202, 37);
            this.chkExclusive.TabIndex = 29;
            this.chkExclusive.Text = "Use Devices Exclusively";
            this.chkExclusive.UseVisualStyleBackColor = true;
            // 
            // ucAudio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.grpVAC);
            this.Name = "ucAudio";
            this.Size = new System.Drawing.Size(633, 393);
            this.EnabledChanged += new System.EventHandler(this.ucAudio_EnabledChanged);
            this.grpVAC.ResumeLayout(false);
            this.grpVAC.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpVAC;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private MaterialSkin2DotNet.Controls.MaterialButton btnApplyAudio;
        private MaterialSkin2DotNet.Controls.MaterialButton btnAudioReset;
        private MaterialSkin2DotNet.Controls.MaterialComboBox cboBufferSize;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel2;
        private MaterialSkin2DotNet.Controls.MaterialComboBox cboSampleRate;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel1;
        private MaterialSkin2DotNet.Controls.MaterialComboBox cboAudioOut;
        private MaterialSkin2DotNet.Controls.MaterialComboBox cboAudioIn;
        private MaterialSkin2DotNet.Controls.MaterialComboBox cboAPI;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblAudioOut;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblAudioIn;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblAPI;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblDisabled;
        private MaterialSkin2DotNet.Controls.MaterialButton cmdTest;
        private MaterialSkin2DotNet.Controls.MaterialCheckbox chkExclusive;
    }
}
