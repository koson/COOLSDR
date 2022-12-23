namespace CoolSDR.CustomUserControls
{
    partial class ucBands
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucBands));
            this.grpRegion = new System.Windows.Forms.GroupBox();
            this.btnDefaults = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.chkBBTX = new System.Windows.Forms.CheckBox();
            this.radRegionPacific = new System.Windows.Forms.RadioButton();
            this.radRegionAmerica = new System.Windows.Forms.RadioButton();
            this.radRegionEU = new System.Windows.Forms.RadioButton();
            this.lblCheckStandard = new System.Windows.Forms.Label();
            this.btnAll = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.btnNone = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.btnDel = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.btnAdd = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.tv = new System.Windows.Forms.TreeView();
            this.btnEdit = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.cboWhich = new System.Windows.Forms.ComboBox();
            this.btnAddSub = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ImgTV = new System.Windows.Forms.ImageList(this.components);
            this.grpRegion.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpRegion
            // 
            this.grpRegion.Controls.Add(this.btnDefaults);
            this.grpRegion.Controls.Add(this.chkBBTX);
            this.grpRegion.Controls.Add(this.radRegionPacific);
            this.grpRegion.Controls.Add(this.radRegionAmerica);
            this.grpRegion.Controls.Add(this.radRegionEU);
            this.grpRegion.Location = new System.Drawing.Point(10, 12);
            this.grpRegion.Margin = new System.Windows.Forms.Padding(2);
            this.grpRegion.Name = "grpRegion";
            this.grpRegion.Padding = new System.Windows.Forms.Padding(15, 16, 15, 16);
            this.grpRegion.Size = new System.Drawing.Size(237, 277);
            this.grpRegion.TabIndex = 0;
            this.grpRegion.TabStop = false;
            this.grpRegion.Text = "Select Region";
            // 
            // btnDefaults
            // 
            this.btnDefaults.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDefaults.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnDefaults.Depth = 0;
            this.btnDefaults.HighEmphasis = true;
            this.btnDefaults.Icon = null;
            this.btnDefaults.Location = new System.Drawing.Point(17, 220);
            this.btnDefaults.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnDefaults.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnDefaults.Size = new System.Drawing.Size(140, 36);
            this.btnDefaults.TabIndex = 7;
            this.btnDefaults.Text = "Reset Defaults";
            this.btnDefaults.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnDefaults.UseAccentColor = false;
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // chkBBTX
            // 
            this.chkBBTX.AutoSize = true;
            this.chkBBTX.Location = new System.Drawing.Point(17, 141);
            this.chkBBTX.Margin = new System.Windows.Forms.Padding(2);
            this.chkBBTX.Name = "chkBBTX";
            this.chkBBTX.Size = new System.Drawing.Size(95, 17);
            this.chkBBTX.TabIndex = 3;
            this.chkBBTX.Text = "Broadband TX";
            this.chkBBTX.UseVisualStyleBackColor = true;
            // 
            // radRegionPacific
            // 
            this.radRegionPacific.AutoSize = true;
            this.radRegionPacific.Location = new System.Drawing.Point(17, 84);
            this.radRegionPacific.Margin = new System.Windows.Forms.Padding(2);
            this.radRegionPacific.Name = "radRegionPacific";
            this.radRegionPacific.Size = new System.Drawing.Size(153, 17);
            this.radRegionPacific.TabIndex = 2;
            this.radRegionPacific.TabStop = true;
            this.radRegionPacific.Text = "Region 3 (Asia and Pacific)";
            this.radRegionPacific.UseVisualStyleBackColor = true;
            // 
            // radRegionAmerica
            // 
            this.radRegionAmerica.AutoSize = true;
            this.radRegionAmerica.Location = new System.Drawing.Point(17, 56);
            this.radRegionAmerica.Margin = new System.Windows.Forms.Padding(2);
            this.radRegionAmerica.Name = "radRegionAmerica";
            this.radRegionAmerica.Size = new System.Drawing.Size(142, 17);
            this.radRegionAmerica.TabIndex = 1;
            this.radRegionAmerica.TabStop = true;
            this.radRegionAmerica.Text = "Region 2 (The Americas)";
            this.radRegionAmerica.UseVisualStyleBackColor = true;
            // 
            // radRegionEU
            // 
            this.radRegionEU.AutoSize = true;
            this.radRegionEU.Location = new System.Drawing.Point(17, 29);
            this.radRegionEU.Margin = new System.Windows.Forms.Padding(2);
            this.radRegionEU.Name = "radRegionEU";
            this.radRegionEU.Size = new System.Drawing.Size(205, 17);
            this.radRegionEU.TabIndex = 0;
            this.radRegionEU.TabStop = true;
            this.radRegionEU.Text = "Region 1 (Europe, Africa, Middle East)";
            this.radRegionEU.UseVisualStyleBackColor = true;
            // 
            // lblCheckStandard
            // 
            this.lblCheckStandard.AutoSize = true;
            this.lblCheckStandard.Location = new System.Drawing.Point(275, 45);
            this.lblCheckStandard.Name = "lblCheckStandard";
            this.lblCheckStandard.Size = new System.Drawing.Size(21, 13);
            this.lblCheckStandard.TabIndex = 6;
            this.lblCheckStandard.Text = "TX";
            // 
            // btnAll
            // 
            this.btnAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAll.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnAll.Depth = 0;
            this.btnAll.HighEmphasis = true;
            this.btnAll.Icon = null;
            this.btnAll.Location = new System.Drawing.Point(262, 232);
            this.btnAll.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAll.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnAll.Name = "btnAll";
            this.btnAll.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnAll.Size = new System.Drawing.Size(64, 36);
            this.btnAll.TabIndex = 5;
            this.btnAll.Text = "All";
            this.btnAll.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAll.UseAccentColor = false;
            this.btnAll.UseVisualStyleBackColor = true;
            // 
            // btnNone
            // 
            this.btnNone.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNone.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnNone.Depth = 0;
            this.btnNone.HighEmphasis = true;
            this.btnNone.Icon = null;
            this.btnNone.Location = new System.Drawing.Point(335, 232);
            this.btnNone.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnNone.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnNone.Name = "btnNone";
            this.btnNone.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnNone.Size = new System.Drawing.Size(64, 36);
            this.btnNone.TabIndex = 6;
            this.btnNone.Text = "None";
            this.btnNone.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnNone.UseAccentColor = false;
            this.btnNone.UseVisualStyleBackColor = true;
            // 
            // btnDel
            // 
            this.btnDel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDel.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnDel.Depth = 0;
            this.btnDel.HighEmphasis = true;
            this.btnDel.Icon = null;
            this.btnDel.Location = new System.Drawing.Point(572, 232);
            this.btnDel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnDel.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnDel.Name = "btnDel";
            this.btnDel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnDel.Size = new System.Drawing.Size(64, 36);
            this.btnDel.TabIndex = 10;
            this.btnDel.Text = "Del";
            this.btnDel.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnDel.UseAccentColor = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnAdd.Depth = 0;
            this.btnAdd.HighEmphasis = true;
            this.btnAdd.Icon = null;
            this.btnAdd.Location = new System.Drawing.Point(408, 232);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnAdd.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnAdd.Size = new System.Drawing.Size(64, 36);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add";
            this.btnAdd.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAdd.UseAccentColor = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tv
            // 
            this.tv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tv.HideSelection = false;
            this.tv.ImageIndex = 0;
            this.tv.ImageList = this.ImgTV;
            this.tv.Location = new System.Drawing.Point(263, 61);
            this.tv.Name = "tv";
            this.tv.SelectedImageIndex = 0;
            this.tv.Size = new System.Drawing.Size(447, 162);
            this.tv.TabIndex = 4;
            this.tv.DoubleClick += new System.EventHandler(this.tv_DoubleClick);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnEdit.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnEdit.Depth = 0;
            this.btnEdit.HighEmphasis = true;
            this.btnEdit.Icon = null;
            this.btnEdit.Location = new System.Drawing.Point(645, 232);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnEdit.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnEdit.Size = new System.Drawing.Size(64, 36);
            this.btnEdit.TabIndex = 11;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnEdit.UseAccentColor = false;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // cboWhich
            // 
            this.cboWhich.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.cboWhich.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWhich.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboWhich.FormattingEnabled = true;
            this.cboWhich.Items.AddRange(new object[] {
            "View Standard Bands",
            "View User-Defined Bands"});
            this.cboWhich.Location = new System.Drawing.Point(262, 18);
            this.cboWhich.Name = "cboWhich";
            this.cboWhich.Size = new System.Drawing.Size(448, 21);
            this.cboWhich.TabIndex = 12;
            // 
            // btnAddSub
            // 
            this.btnAddSub.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddSub.Density = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnAddSub.Depth = 0;
            this.btnAddSub.HighEmphasis = true;
            this.btnAddSub.Icon = null;
            this.btnAddSub.Location = new System.Drawing.Point(481, 232);
            this.btnAddSub.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnAddSub.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.btnAddSub.Name = "btnAddSub";
            this.btnAddSub.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnAddSub.Size = new System.Drawing.Size(82, 36);
            this.btnAddSub.TabIndex = 13;
            this.btnAddSub.Text = "Add Sub";
            this.btnAddSub.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAddSub.UseAccentColor = false;
            this.btnAddSub.UseVisualStyleBackColor = true;
            this.btnAddSub.Click += new System.EventHandler(this.btnAddSub_Click);
            // 
            // ImgTV
            // 
            this.ImgTV.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImgTV.ImageStream")));
            this.ImgTV.TransparentColor = System.Drawing.Color.Transparent;
            this.ImgTV.Images.SetKeyName(0, "zap.png");
            // 
            // ucBands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.btnAddSub);
            this.Controls.Add(this.cboWhich);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.tv);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.lblCheckStandard);
            this.Controls.Add(this.grpRegion);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucBands";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(721, 289);
            this.grpRegion.ResumeLayout(false);
            this.grpRegion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpRegion;
        private System.Windows.Forms.RadioButton radRegionPacific;
        private System.Windows.Forms.RadioButton radRegionAmerica;
        private System.Windows.Forms.RadioButton radRegionEU;
        private System.Windows.Forms.CheckBox chkBBTX;
        private System.Windows.Forms.Label lblCheckStandard;
        private MaterialSkin2DotNet.Controls.MaterialButton btnAll;
        private MaterialSkin2DotNet.Controls.MaterialButton btnNone;
        private MaterialSkin2DotNet.Controls.MaterialButton btnDel;
        private MaterialSkin2DotNet.Controls.MaterialButton btnAdd;
        private System.Windows.Forms.TreeView tv;
        private MaterialSkin2DotNet.Controls.MaterialButton btnEdit;
        private System.Windows.Forms.ComboBox cboWhich;
        private MaterialSkin2DotNet.Controls.MaterialButton btnAddSub;
        private MaterialSkin2DotNet.Controls.MaterialButton btnDefaults;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ImageList ImgTV;
    }
}
