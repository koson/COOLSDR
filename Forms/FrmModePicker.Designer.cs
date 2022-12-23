namespace CoolSDR.Forms
{
    partial class FrmModePicker
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
            this.grp = new System.Windows.Forms.GroupBox();
            this.chkCW = new System.Windows.Forms.CheckBox();
            this.chkDigi = new System.Windows.Forms.CheckBox();
            this.chkFM = new System.Windows.Forms.CheckBox();
            this.chkAM = new System.Windows.Forms.CheckBox();
            this.chkUSB = new System.Windows.Forms.CheckBox();
            this.chkLSB = new System.Windows.Forms.CheckBox();
            this.btnCancel = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.btnOK = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.btnAll = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.btnNone = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.grp.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp
            // 
            this.grp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.grp.Controls.Add(this.chkCW);
            this.grp.Controls.Add(this.chkDigi);
            this.grp.Controls.Add(this.chkFM);
            this.grp.Controls.Add(this.chkAM);
            this.grp.Controls.Add(this.chkUSB);
            this.grp.Controls.Add(this.chkLSB);
            this.grp.ForeColor = System.Drawing.Color.Silver;
            this.grp.Location = new System.Drawing.Point(35, 77);
            this.grp.Name = "grp";
            this.grp.Size = new System.Drawing.Size(160, 190);
            this.grp.TabIndex = 0;
            this.grp.TabStop = false;
            // 
            // chkCW
            // 
            this.chkCW.AutoSize = true;
            this.chkCW.ForeColor = System.Drawing.Color.Silver;
            this.chkCW.Location = new System.Drawing.Point(20, 154);
            this.chkCW.Name = "chkCW";
            this.chkCW.Size = new System.Drawing.Size(44, 17);
            this.chkCW.TabIndex = 6;
            this.chkCW.Text = "CW";
            this.chkCW.UseVisualStyleBackColor = true;
            // 
            // chkDigi
            // 
            this.chkDigi.AutoSize = true;
            this.chkDigi.ForeColor = System.Drawing.Color.Silver;
            this.chkDigi.Location = new System.Drawing.Point(20, 127);
            this.chkDigi.Name = "chkDigi";
            this.chkDigi.Size = new System.Drawing.Size(90, 17);
            this.chkDigi.TabIndex = 5;
            this.chkDigi.Text = "Digital Modes";
            this.chkDigi.UseVisualStyleBackColor = true;
            // 
            // chkFM
            // 
            this.chkFM.AutoSize = true;
            this.chkFM.ForeColor = System.Drawing.Color.Silver;
            this.chkFM.Location = new System.Drawing.Point(20, 100);
            this.chkFM.Name = "chkFM";
            this.chkFM.Size = new System.Drawing.Size(41, 17);
            this.chkFM.TabIndex = 4;
            this.chkFM.Text = "FM";
            this.chkFM.UseVisualStyleBackColor = true;
            // 
            // chkAM
            // 
            this.chkAM.AutoSize = true;
            this.chkAM.ForeColor = System.Drawing.Color.Silver;
            this.chkAM.Location = new System.Drawing.Point(20, 73);
            this.chkAM.Name = "chkAM";
            this.chkAM.Size = new System.Drawing.Size(42, 17);
            this.chkAM.TabIndex = 3;
            this.chkAM.Text = "AM";
            this.chkAM.UseVisualStyleBackColor = true;
            // 
            // chkUSB
            // 
            this.chkUSB.AutoSize = true;
            this.chkUSB.ForeColor = System.Drawing.Color.Silver;
            this.chkUSB.Location = new System.Drawing.Point(20, 46);
            this.chkUSB.Name = "chkUSB";
            this.chkUSB.Size = new System.Drawing.Size(48, 17);
            this.chkUSB.TabIndex = 2;
            this.chkUSB.Text = "USB";
            this.chkUSB.UseVisualStyleBackColor = true;
            // 
            // chkLSB
            // 
            this.chkLSB.AutoSize = true;
            this.chkLSB.ForeColor = System.Drawing.Color.Silver;
            this.chkLSB.Location = new System.Drawing.Point(20, 19);
            this.chkLSB.Name = "chkLSB";
            this.chkLSB.Size = new System.Drawing.Size(46, 17);
            this.chkLSB.TabIndex = 1;
            this.chkLSB.Text = "LSB";
            this.chkLSB.UseVisualStyleBackColor = true;
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
            this.btnCancel.Location = new System.Drawing.Point(376, 287);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnCancel.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnCancel.Size = new System.Drawing.Size(87, 36);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnCancel.UseAccentColor = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnOK.Depth = 0;
            this.btnOK.HighEmphasis = true;
            this.btnOK.Icon = null;
            this.btnOK.Location = new System.Drawing.Point(286, 287);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnOK.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnOK.Name = "btnOK";
            this.btnOK.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnOK.Size = new System.Drawing.Size(64, 36);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&Ok";
            this.btnOK.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnOK.UseAccentColor = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAll
            // 
            this.btnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAll.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnAll.Depth = 0;
            this.btnAll.HighEmphasis = true;
            this.btnAll.Icon = null;
            this.btnAll.Location = new System.Drawing.Point(34, 287);
            this.btnAll.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAll.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnAll.Name = "btnAll";
            this.btnAll.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnAll.Size = new System.Drawing.Size(64, 36);
            this.btnAll.TabIndex = 3;
            this.btnAll.Text = "&All";
            this.btnAll.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAll.UseAccentColor = false;
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnNone
            // 
            this.btnNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNone.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNone.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnNone.Depth = 0;
            this.btnNone.HighEmphasis = true;
            this.btnNone.Icon = null;
            this.btnNone.Location = new System.Drawing.Point(124, 287);
            this.btnNone.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnNone.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnNone.Name = "btnNone";
            this.btnNone.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnNone.Size = new System.Drawing.Size(70, 36);
            this.btnNone.TabIndex = 4;
            this.btnNone.Text = "&None";
            this.btnNone.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnNone.UseAccentColor = false;
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // FrmModePicker
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(492, 341);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grp);
            this.MaximizeBox = false;
            this.Name = "FrmModePicker";
            this.Text = "Choose Modes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmModePicker_FormClosing);
            this.grp.ResumeLayout(false);
            this.grp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grp;
        private System.Windows.Forms.CheckBox chkDigi;
        private System.Windows.Forms.CheckBox chkFM;
        private System.Windows.Forms.CheckBox chkAM;
        private System.Windows.Forms.CheckBox chkUSB;
        private System.Windows.Forms.CheckBox chkLSB;
        private System.Windows.Forms.CheckBox chkCW;
        private MaterialSkin2DotNet.Controls.MaterialButton btnCancel;
        private MaterialSkin2DotNet.Controls.MaterialButton btnOK;
        private MaterialSkin2DotNet.Controls.MaterialButton btnAll;
        private MaterialSkin2DotNet.Controls.MaterialButton btnNone;
    }
}