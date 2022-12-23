// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com

using CoolComponents;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;



namespace CoolComponentsTest
{
    public partial class VFOTester : Form
    {

        readonly TextBox TB;

        public bool StopStress { get; private set; }
        public bool Stressing { get; private set; }

        public VFOTester()
        {
            InitializeComponent();
            TB = VFO.TextBoxControl;
            TB.MouseMove += this.NewMouseMove;
            TB.TextChanged += this.TB_TextChanged;
            VFO.HoverIndexChanged += this.TBHoverIndexChanged;
            lbl.Text = VFO.FreqManager.FreqInHz.ToString() + " Hz";
            this.ShowTextInAuxLabel(this.TxtAux1.TextBoxControl, VFOControl.AuxLabels.Aux1);
            this.ShowTextInAuxLabel(this.TxtAux2.TextBoxControl, VFOControl.AuxLabels.Aux2);
        }

        private void TBHoverIndexChanged(object sender, VFOControl.HoverIndexChangedArgs e)
        {
            lblHover.Text = "HoverIndex : " + e.new_index.ToString();
        }

        // private CoolComponents.CoolTextBox TB;

        private void NewMouseMove(object sender, MouseEventArgs e)
        {

            // var c = TB.GetCharFromPosition(e.Location);
            var a = TB.GetCharIndexFromPosition(e.Location);
            this.Text = "Mouse is over character position: " + a;

        }


        private void TB_TextChanged(object sender, EventArgs e)
        {
            lbl.Text = VFO.FreqManager.FreqInHz.ToString() + " Hz";
        }

        private void BtnStress_Click(object sender, EventArgs e)
        {
            if (Stressing) return;

            Stressing = true;
            BtnEndStress.Visible = true;
            int s = 0;
            VFO.TextBoxControl.Select();
            while (!StopStress)
            {
                this.VFO.FreqManager.FreqInHz++;
                this.VFO.TextBoxControl.SelectionLength = 1;
                this.VFO.TextBoxControl.SelectionStart = s;
                s += 1;
                s %= VFO.TextBoxControl.Text.Length;
                this.VFO.ShowUnderlineAtIndex(s);
                Thread.Sleep(1);
                Application.DoEvents();

            }

            Stressing = false;
            BtnEndStress.Visible = false;
            VFO.TextBoxControl.Select();
            StopStress = false;
        }

        private void BtnEndStress_Click(object sender, EventArgs e)
        {
            StopStress = true;
        }

        private void VFOTester_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopStress = true;
        }

        private void ShowTextInAuxLabel(TextBox t, VFOControl.AuxLabels which)
        {
            var lbl = VFO.AuxLabel(which);
            if (which == VFOControl.AuxLabels.Aux1)
                Debug.Assert(lbl.Name == "lblAux1");
            else
                Debug.Assert(lbl.Name == "lblAux2");

            lbl.Visible = (!string.IsNullOrEmpty(t.Text));
            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
            lbl.Text = t.Text;
        }

        private void TxtAux1_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            ShowTextInAuxLabel(t, VFOControl.AuxLabels.Aux1);
        }

        private void TxtAux2_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            ShowTextInAuxLabel(t, VFOControl.AuxLabels.Aux2);

        }
    }
}
