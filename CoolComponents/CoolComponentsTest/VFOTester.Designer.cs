using System;

namespace CoolComponentsTest
{
    partial class VFOTester
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
            CoolComponents.FrequencyManager frequencyManager1 = new CoolComponents.FrequencyManager();
            this.Btn = new System.Windows.Forms.Button();
            this.BtnStress = new System.Windows.Forms.Button();
            this.BtnEndStress = new System.Windows.Forms.Button();
            this.lbl = new System.Windows.Forms.Label();
            this.lblHover = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtAux2 = new CoolComponents.CoolTextBox();
            this.TxtAux1 = new CoolComponents.CoolTextBox();
            this.VFO = new CoolComponents.VFOControl();
            this.SuspendLayout();
            // 
            // Btn
            // 
            this.Btn.Location = new System.Drawing.Point(15, 112);
            this.Btn.Name = "Btn";
            this.Btn.Size = new System.Drawing.Size(75, 23);
            this.Btn.TabIndex = 1;
            this.Btn.Text = "Set new text";
            this.Btn.UseVisualStyleBackColor = true;
            this.Btn.Click += new System.EventHandler(this.Btn_Click);
            // 
            // BtnStress
            // 
            this.BtnStress.Location = new System.Drawing.Point(108, 112);
            this.BtnStress.Name = "BtnStress";
            this.BtnStress.Size = new System.Drawing.Size(75, 23);
            this.BtnStress.TabIndex = 3;
            this.BtnStress.Text = "&Stress";
            this.BtnStress.UseVisualStyleBackColor = true;
            this.BtnStress.Click += new System.EventHandler(this.BtnStress_Click);
            // 
            // BtnEndStress
            // 
            this.BtnEndStress.Location = new System.Drawing.Point(199, 112);
            this.BtnEndStress.Name = "BtnEndStress";
            this.BtnEndStress.Size = new System.Drawing.Size(75, 23);
            this.BtnEndStress.TabIndex = 4;
            this.BtnEndStress.Text = "&EndStress";
            this.BtnEndStress.UseVisualStyleBackColor = true;
            this.BtnEndStress.Visible = false;
            this.BtnEndStress.Click += new System.EventHandler(this.BtnEndStress_Click);
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Location = new System.Drawing.Point(12, 9);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(35, 13);
            this.lbl.TabIndex = 5;
            this.lbl.Text = "label1";
            // 
            // lblHover
            // 
            this.lblHover.AutoSize = true;
            this.lblHover.Location = new System.Drawing.Point(196, 9);
            this.lblHover.Name = "lblHover";
            this.lblHover.Size = new System.Drawing.Size(77, 13);
            this.lblHover.TabIndex = 6;
            this.lblHover.Text = "HoverIndex: -1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Aux1 Text:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 221);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Aux2 Text:";
            // 
            // TxtAux2
            // 
            this.TxtAux2.AllowDecimal = false;
            this.TxtAux2.AllowMultipleDecimals = false;
            this.TxtAux2.AllowNegative = false;
            this.TxtAux2.BackColor = System.Drawing.SystemColors.Window;
            this.TxtAux2.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.TxtAux2.BorderColorDarkenOnFocus = true;
            this.TxtAux2.BorderFocusColor = System.Drawing.Color.Empty;
            this.TxtAux2.BorderInactiveColor = System.Drawing.Color.Empty;
            this.TxtAux2.BorderSize = 1;
            this.TxtAux2.BorderThickenOnFocus = true;
            this.TxtAux2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TxtAux2.Location = new System.Drawing.Point(18, 247);
            this.TxtAux2.MaxTextLength = 32767;
            this.TxtAux2.Multiline = false;
            this.TxtAux2.Name = "TxtAux2";
            this.TxtAux2.NumericOnly = false;
            this.TxtAux2.Padding = new System.Windows.Forms.Padding(7);
            this.TxtAux2.PasswordChar = false;
            this.TxtAux2.SelectionLength = 0;
            this.TxtAux2.SelectionStart = 0;
            this.TxtAux2.ShortcutsEnabled = true;
            this.TxtAux2.Size = new System.Drawing.Size(250, 28);
            this.TxtAux2.TabIndex = 9;
            this.TxtAux2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtAux2.TextValue = "11m";
            this.TxtAux2.UnderlinedStyle = false;
            this.TxtAux2.TextChanged += new System.EventHandler(this.TxtAux2_TextChanged);
            // 
            // TxtAux1
            // 
            this.TxtAux1.AllowDecimal = false;
            this.TxtAux1.AllowMultipleDecimals = false;
            this.TxtAux1.AllowNegative = false;
            this.TxtAux1.BackColor = System.Drawing.SystemColors.Window;
            this.TxtAux1.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.TxtAux1.BorderColorDarkenOnFocus = true;
            this.TxtAux1.BorderFocusColor = System.Drawing.Color.Empty;
            this.TxtAux1.BorderInactiveColor = System.Drawing.Color.Empty;
            this.TxtAux1.BorderSize = 1;
            this.TxtAux1.BorderThickenOnFocus = true;
            this.TxtAux1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TxtAux1.Location = new System.Drawing.Point(18, 182);
            this.TxtAux1.MaxTextLength = 32767;
            this.TxtAux1.Multiline = false;
            this.TxtAux1.Name = "TxtAux1";
            this.TxtAux1.NumericOnly = false;
            this.TxtAux1.Padding = new System.Windows.Forms.Padding(7);
            this.TxtAux1.PasswordChar = false;
            this.TxtAux1.SelectionLength = 0;
            this.TxtAux1.SelectionStart = 0;
            this.TxtAux1.ShortcutsEnabled = true;
            this.TxtAux1.Size = new System.Drawing.Size(250, 28);
            this.TxtAux1.TabIndex = 7;
            this.TxtAux1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtAux1.TextValue = "USB";
            this.TxtAux1.UnderlinedStyle = false;
            this.TxtAux1.TextChanged += new System.EventHandler(this.TxtAux1_TextChanged);
            // 
            // VFO
            // 
            this.VFO.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.VFO.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            frequencyManager1.Buddy = this.VFO;
            frequencyManager1.FreqInHz = ((long)(27555000));
            frequencyManager1.Max = ((long)(55000000));
            frequencyManager1.Min = ((long)(50000));
            this.VFO.FreqManager = frequencyManager1;
            this.VFO.FrequencyDefaultMHz = 27.555D;
            this.VFO.HoverUnderline = true;
            this.VFO.Location = new System.Drawing.Point(15, 41);
            this.VFO.Name = "VFO";
            this.VFO.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.VFO.Size = new System.Drawing.Size(259, 65);
            this.VFO.TabIndex = 2;
            this.VFO.TextBackColor = System.Drawing.Color.DarkSeaGreen;
            this.VFO.UnderlineColor = System.Drawing.SystemColors.WindowText;
            this.VFO.UnderlineDecimals = false;
            this.VFO.UnderlineThickness = 3;
            // 
            // VFOTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 289);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtAux2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtAux1);
            this.Controls.Add(this.lblHover);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.BtnEndStress);
            this.Controls.Add(this.BtnStress);
            this.Controls.Add(this.VFO);
            this.Controls.Add(this.Btn);
            this.Name = "VFOTester";
            this.Text = "Testing VFO Component";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VFOTester_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Btn_Click(object sender, EventArgs e)
        {
            VFO.FreqManager.FreqInHz += 1_000;
        }

        #endregion
        private System.Windows.Forms.Button Btn;
        private CoolComponents.VFOControl VFO;
        private System.Windows.Forms.Button BtnStress;
        private System.Windows.Forms.Button BtnEndStress;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Label lblHover;
        private CoolComponents.CoolTextBox TxtAux1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private CoolComponents.CoolTextBox TxtAux2;
    }
}

