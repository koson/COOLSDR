// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
namespace CoolComponents
{
    partial class CoolTextBox
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
            this.TextBoxCtrl = new MyTextBox();
            this.SuspendLayout();
            // 
            // TextBoxCtrl
            // 
            this.TextBoxCtrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxCtrl.Location = new System.Drawing.Point(7, 7);
            this.TextBoxCtrl.Name = "TextBoxCtrl";
            this.TextBoxCtrl.Size = new System.Drawing.Size(236, 13);
            this.TextBoxCtrl.TabIndex = 0;
            // 
            // CoolTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.TextBoxCtrl);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Name = "CoolTextBox";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Size = new System.Drawing.Size(250, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MyTextBox TextBoxCtrl;
    }
}
