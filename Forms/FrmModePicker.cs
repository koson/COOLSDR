using MaterialSkin2DotNet;
using MaterialSkin2DotNet.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoolSDR.CustomUserControls;
using CoolSDR.Modes;
using CoolSDR.BandsTypes;
using Newtonsoft.Json.Linq;

namespace CoolSDR.Forms
{
    public partial class FrmModePicker : MaterialForm
    {
        public FrmModePicker()
        {
            InitializeComponent();
            MaterialSkinManager.Instance.AddFormToManage(this);

        }

        DialogResult m_Result = DialogResult.Cancel;
        public DialogResult ShowForm(FrmBandEdit f)
        {
            m_Result = DialogResult.Cancel;
            var state = f.Band.State;
            BandLimits band = null;
            if (f.Band.State == ucBands.EditBandState.Create_New_MainBand ||
                state == ucBands.EditBandState.Edit_MainBand)
            {
                this.Text = "Choose modes allowed for " + f.BandNameText;
                band = f.Band.ParentBand;
                
            }
            else
            {
                this.Text = "Choose modes allowed for " + f.BandNameText;
                band = f.Band.SubBand;
            }

            this.SelectedModes = band.AllowedModes;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog(f);
            
           return m_Result;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_Result = DialogResult.OK;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_Result = DialogResult.Cancel;
            this.Hide();
        }

        private void FrmModePicker_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            return;
        }

        public ModeKind SelectedModes
        {
            get
            {
                ModeKind ret = ModeKind.None;
                if (chkAM.Checked) ret |= ModeKind.AM;
                if (chkFM.Checked) ret |= ModeKind.FM;
                if (chkCW.Checked) ret |= ModeKind.CW;
                if (chkDigi.Checked) ret |= ModeKind.DIGI;
                if (chkLSB.Checked) ret |= ModeKind.LSB;
                if (chkUSB.Checked) ret |= ModeKind.USB;
                return ret;
            }
            private set
            {
                int i = (int)(value & ModeKind.AM);
                chkAM.Checked= i != 0;

                i = (int)(value & ModeKind.FM);
                chkFM.Checked= i != 0;

                i = (int)(value & ModeKind.USB);
                chkUSB.Checked= i != 0;

                i = (int)(value & ModeKind.LSB);
                chkLSB.Checked= i != 0;

                i = (int)(value & ModeKind.DIGI);
                chkDigi.Checked= i != 0;

                i = (int)(value & ModeKind.CW);
                chkCW.Checked= i != 0;

            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            SelectAll(true);
        }

        void SelectAll(bool value)
        {
            foreach (var v in grp.Controls)
            {
                if (v.GetType() == typeof(CheckBox))
                {
                    CheckBox c = (CheckBox)v;
                    c.Checked = value;
                }
            }
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            SelectAll(false);
        }
    }
}
