namespace CoolSDR.Forms
{
    partial class FrmBandEdit
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtName = new CoolComponents.CoolTextBox();
            this.lblName = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.lblFrom = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.txtLo = new CoolComponents.CoolTextBox();
            this.lblMegsTo = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.txtHi = new CoolComponents.CoolTextBox();
            this.lblMHz = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.chkTX = new MaterialSkin2DotNet.Controls.MaterialCheckbox();
            this.btnOK = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.btnCancel = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.label1 = new System.Windows.Forms.Label();
            this.chkChannels = new MaterialSkin2DotNet.Controls.MaterialCheckbox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnModes = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.AllowDecimal = false;
            this.txtName.AllowMultipleDecimals = false;
            this.txtName.AllowNegative = false;
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtName.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtName.BorderColorDarkenOnFocus = true;
            this.txtName.BorderFocusColor = System.Drawing.Color.Empty;
            this.txtName.BorderInactiveColor = System.Drawing.Color.Empty;
            this.txtName.BorderSize = 2;
            this.txtName.BorderThickenOnFocus = true;
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtName.Location = new System.Drawing.Point(26, 104);
            this.txtName.MaxTextLength = 32767;
            this.txtName.Multiline = false;
            this.txtName.Name = "txtName";
            this.txtName.NumericOnly = false;
            this.txtName.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txtName.PasswordChar = false;
            this.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtName.SelectionLength = 0;
            this.txtName.SelectionStart = 0;
            this.txtName.ShortcutsEnabled = true;
            this.txtName.Size = new System.Drawing.Size(332, 35);
            this.txtName.TabIndex = 0;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtName.TextValue = "Type the band name here ...";
            this.txtName.UnderlinedStyle = false;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Depth = 0;
            this.lblName.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblName.Location = new System.Drawing.Point(26, 82);
            this.lblName.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(88, 19);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Band Name:";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Depth = 0;
            this.lblFrom.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFrom.Location = new System.Drawing.Point(27, 168);
            this.lblFrom.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(42, 19);
            this.lblFrom.TabIndex = 6;
            this.lblFrom.Text = "From:";
            // 
            // txtLo
            // 
            this.txtLo.AllowDecimal = true;
            this.txtLo.AllowMultipleDecimals = false;
            this.txtLo.AllowNegative = false;
            this.txtLo.BackColor = System.Drawing.SystemColors.Window;
            this.txtLo.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtLo.BorderColorDarkenOnFocus = true;
            this.txtLo.BorderFocusColor = System.Drawing.Color.Empty;
            this.txtLo.BorderInactiveColor = System.Drawing.Color.Empty;
            this.txtLo.BorderSize = 2;
            this.txtLo.BorderThickenOnFocus = true;
            this.txtLo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLo.Location = new System.Drawing.Point(90, 156);
            this.txtLo.MaxTextLength = 32767;
            this.txtLo.Multiline = false;
            this.txtLo.Name = "txtLo";
            this.txtLo.NumericOnly = true;
            this.txtLo.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txtLo.PasswordChar = false;
            this.txtLo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtLo.SelectionLength = 0;
            this.txtLo.SelectionStart = 0;
            this.txtLo.ShortcutsEnabled = false;
            this.txtLo.Size = new System.Drawing.Size(83, 35);
            this.txtLo.TabIndex = 1;
            this.txtLo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLo.TextValue = "1.810";
            this.txtLo.UnderlinedStyle = true;
            // 
            // lblMegsTo
            // 
            this.lblMegsTo.AutoSize = true;
            this.lblMegsTo.Depth = 0;
            this.lblMegsTo.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMegsTo.Location = new System.Drawing.Point(189, 168);
            this.lblMegsTo.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblMegsTo.Name = "lblMegsTo";
            this.lblMegsTo.Size = new System.Drawing.Size(59, 19);
            this.lblMegsTo.TabIndex = 8;
            this.lblMegsTo.Text = "MHz, to:";
            // 
            // txtHi
            // 
            this.txtHi.AllowDecimal = true;
            this.txtHi.AllowMultipleDecimals = false;
            this.txtHi.AllowNegative = false;
            this.txtHi.BackColor = System.Drawing.SystemColors.Window;
            this.txtHi.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txtHi.BorderColorDarkenOnFocus = true;
            this.txtHi.BorderFocusColor = System.Drawing.Color.Empty;
            this.txtHi.BorderInactiveColor = System.Drawing.Color.Empty;
            this.txtHi.BorderSize = 2;
            this.txtHi.BorderThickenOnFocus = true;
            this.txtHi.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtHi.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtHi.Location = new System.Drawing.Point(274, 156);
            this.txtHi.MaxTextLength = 32767;
            this.txtHi.Multiline = false;
            this.txtHi.Name = "txtHi";
            this.txtHi.NumericOnly = true;
            this.txtHi.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txtHi.PasswordChar = false;
            this.txtHi.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtHi.SelectionLength = 0;
            this.txtHi.SelectionStart = 0;
            this.txtHi.ShortcutsEnabled = false;
            this.txtHi.Size = new System.Drawing.Size(83, 35);
            this.txtHi.TabIndex = 2;
            this.txtHi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHi.TextValue = "1.840";
            this.txtHi.UnderlinedStyle = true;
            // 
            // lblMHz
            // 
            this.lblMHz.AutoSize = true;
            this.lblMHz.Depth = 0;
            this.lblMHz.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMHz.Location = new System.Drawing.Point(375, 168);
            this.lblMHz.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.lblMHz.Name = "lblMHz";
            this.lblMHz.Size = new System.Drawing.Size(34, 19);
            this.lblMHz.TabIndex = 10;
            this.lblMHz.Text = "MHz";
            // 
            // chkTX
            // 
            this.chkTX.AutoSize = true;
            this.chkTX.Checked = true;
            this.chkTX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTX.Depth = 0;
            this.chkTX.Location = new System.Drawing.Point(26, 220);
            this.chkTX.Margin = new System.Windows.Forms.Padding(0);
            this.chkTX.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkTX.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.chkTX.Name = "chkTX";
            this.chkTX.ReadOnly = false;
            this.chkTX.Ripple = true;
            this.chkTX.Size = new System.Drawing.Size(243, 37);
            this.chkTX.TabIndex = 3;
            this.chkTX.Text = "Enable Transmit on this band";
            this.chkTX.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnOK.Depth = 0;
            this.btnOK.HighEmphasis = true;
            this.btnOK.Icon = null;
            this.btnOK.Location = new System.Drawing.Point(312, 294);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnOK.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnOK.Name = "btnOK";
            this.btnOK.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnOK.Size = new System.Drawing.Size(64, 36);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnOK.UseAccentColor = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnCancel.Depth = 0;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.HighEmphasis = true;
            this.btnCancel.Icon = null;
            this.btnCancel.Location = new System.Drawing.Point(386, 294);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnCancel.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnCancel.Size = new System.Drawing.Size(87, 36);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnCancel.UseAccentColor = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(30, 275);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(432, 2);
            this.label1.TabIndex = 16;
            // 
            // chkChannels
            // 
            this.chkChannels.AutoSize = true;
            this.chkChannels.Checked = true;
            this.chkChannels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChannels.Depth = 0;
            this.chkChannels.Location = new System.Drawing.Point(26, 297);
            this.chkChannels.Margin = new System.Windows.Forms.Padding(0);
            this.chkChannels.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkChannels.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.chkChannels.Name = "chkChannels";
            this.chkChannels.ReadOnly = false;
            this.chkChannels.Ripple = true;
            this.chkChannels.Size = new System.Drawing.Size(177, 37);
            this.chkChannels.TabIndex = 17;
            this.chkChannels.Text = "Band is channelised";
            this.chkChannels.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkChannels, "\r\nTx Strictly on channel entries only\r\n");
            this.chkChannels.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // btnModes
            // 
            this.btnModes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnModes.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnModes.Depth = 0;
            this.btnModes.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnModes.HighEmphasis = true;
            this.btnModes.Icon = null;
            this.btnModes.Location = new System.Drawing.Point(283, 219);
            this.btnModes.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnModes.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnModes.Name = "btnModes";
            this.btnModes.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnModes.Size = new System.Drawing.Size(93, 36);
            this.btnModes.TabIndex = 18;
            this.btnModes.Text = "&Modes...";
            this.btnModes.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnModes.UseAccentColor = false;
            this.btnModes.UseVisualStyleBackColor = true;
            this.btnModes.Click += new System.EventHandler(this.btnModes_Click);
            // 
            // FrmBandEdit
            // 
            this.AccentColor = MaterialSkin2DotNet.Accent.LightBlue700;
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(497, 347);
            this.Controls.Add(this.btnModes);
            this.Controls.Add(this.chkChannels);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkTX);
            this.Controls.Add(this.lblMHz);
            this.Controls.Add(this.txtHi);
            this.Controls.Add(this.lblMegsTo);
            this.Controls.Add(this.txtLo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.MaximizeBox = false;
            this.Name = "FrmBandEdit";
            this.Padding = new System.Windows.Forms.Padding(2, 52, 20, 20);
            this.PrimaryColor = MaterialSkin2DotNet.Primary.BlueGrey800;
            this.PrimaryDarkColor = MaterialSkin2DotNet.Primary.BlueGrey900;
            this.PrimaryLightColor = MaterialSkin2DotNet.Primary.BlueGrey200;
            this.ShowIcon = false;
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create New Band";
            this.Theme = MaterialSkin2DotNet.MaterialSkinManager.Themes.DARK;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmBandEdit_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CoolComponents.CoolTextBox txtName;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblName;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblFrom;
        private CoolComponents.CoolTextBox txtLo;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblMegsTo;
        private CoolComponents.CoolTextBox txtHi;
        private MaterialSkin2DotNet.Controls.MaterialLabel lblMHz;
        private MaterialSkin2DotNet.Controls.MaterialCheckbox chkTX;
        private MaterialSkin2DotNet.Controls.MaterialButton btnOK;
        private MaterialSkin2DotNet.Controls.MaterialButton btnCancel;
        private System.Windows.Forms.Label label1;
        private MaterialSkin2DotNet.Controls.MaterialCheckbox chkChannels;
        private System.Windows.Forms.ToolTip toolTip1;
        private MaterialSkin2DotNet.Controls.MaterialButton btnModes;
    }
}