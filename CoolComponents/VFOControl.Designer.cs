namespace CoolComponents
{
    partial class VFOControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt = new CoolComponents.CoolTextBox();
            this.lblAux1 = new System.Windows.Forms.Label();
            this.lblAux2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt
            // 
            this.txt.AllowDecimal = true;
            this.txt.AllowMultipleDecimals = true;
            this.txt.AllowNegative = false;
            this.txt.BackColor = System.Drawing.SystemColors.Window;
            this.txt.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.txt.BorderColorDarkenOnFocus = true;
            this.txt.BorderFocusColor = System.Drawing.Color.Empty;
            this.txt.BorderInactiveColor = System.Drawing.Color.Empty;
            this.txt.BorderSize = 2;
            this.txt.BorderThickenOnFocus = true;
            this.txt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt.Location = new System.Drawing.Point(2, 2);
            this.txt.MaxTextLength = 32767;
            this.txt.Multiline = false;
            this.txt.Name = "txt";
            this.txt.NumericOnly = true;
            this.txt.Padding = new System.Windows.Forms.Padding(7);
            this.txt.PasswordChar = false;
            this.txt.SelectionLength = 0;
            this.txt.SelectionStart = 0;
            this.txt.ShortcutsEnabled = true;
            this.txt.Size = new System.Drawing.Size(271, 52);
            this.txt.TabIndex = 0;
            this.txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt.TextValue = "27.555.000";
            this.txt.UnderlinedStyle = false;
            // 
            // lblAux1
            // 
            this.lblAux1.AutoSize = true;
            this.lblAux1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAux1.Location = new System.Drawing.Point(5, 6);
            this.lblAux1.Name = "lblAux1";
            this.lblAux1.Size = new System.Drawing.Size(33, 14);
            this.lblAux1.TabIndex = 7;
            this.lblAux1.Text = "Aux1";
            // 
            // lblAux2
            // 
            this.lblAux2.AutoSize = true;
            this.lblAux2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAux2.Location = new System.Drawing.Point(5, 38);
            this.lblAux2.Name = "lblAux2";
            this.lblAux2.Size = new System.Drawing.Size(33, 14);
            this.lblAux2.TabIndex = 8;
            this.lblAux2.Text = "Aux2";
            // 
            // VFOControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblAux2);
            this.Controls.Add(this.lblAux1);
            this.Controls.Add(this.txt);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VFOControl";
            this.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.Size = new System.Drawing.Size(275, 59);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CoolTextBox txt;
        private System.Windows.Forms.Label lblAux1;
        private System.Windows.Forms.Label lblAux2;
    }
}
