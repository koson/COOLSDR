using CoolSDR.BandsTypes;
using CoolSDR.BandsTypes.NamedBands;
using CoolSDR.Class;
using Krypton.Toolkit;
using MaterialSkin2DotNet;
using MaterialSkin2DotNet.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Navigation;
using static CoolSDR.CustomUserControls.ucBands;
using static System.Windows.Forms.AxHost;

namespace CoolSDR.Forms
{
    public partial class FrmBandEdit : MaterialForm
    {

        private FrmMain m_frmMain;
        public FrmBandEdit(FrmMain mainForm)
        {
            m_frmMain = mainForm;
            InitializeComponent();
            MaterialSkinManager.Instance.AddFormToManage(this);
            this.Text = "Edit Band";
            this.ClientSize = new Size(557, 355);
            this.txtName.BorderFocusColor
                = mainForm.SkinManager.ColorScheme.AccentColor;
            this.txtHi.BorderFocusColor
                = mainForm.SkinManager.ColorScheme.AccentColor;
            this.txtLo.BorderFocusColor
                = mainForm.SkinManager.ColorScheme.AccentColor;
        }

        public string BandNameText { get => this.txtName.Text; }
        internal BandWithSub Band { get; private set; }
        public bool BeenShown { get; private set; }

        internal DialogResult ShowForm(BandWithSub b)
        {
            var state = b.State;

            this.SubBand = b.SubBand;
            this.Band = b;
            bool isParentBand = state == EditBandState.Edit_MainBand
                || state == EditBandState.Create_New_MainBand;
            if (SubBand == null)
            {
                Debug.Assert(isParentBand);
                isParentBand = true;
            }

            if (b.ParentBand == null)
            {
                this.Text = "Create New Band";
                Band.ParentBand = new CustomBand(FrmBandEdit.DefaultText, 1.800,
                    1.900, Modes.ModeKind.All, true);
                this.BandModes = Modes.ModeKind.All;
            }

            this.BeenShown = false;
            this.ShowDialog();
            DialogResult result;
            if (!this.OK)
                result = DialogResult.Cancel;
            else
                result = DialogResult.OK;
            return result;
        }

        public FrmBandEdit ParentEditForm { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!BeenShown)
            {
                BeenShown = true;
                DoShown();
            }
        }

        public BandLimits SubBand { get; private set; }

        void DoShown() { popBand(this.Band); }
        void popBand(BandWithSub b, bool useNewSubBands = false)
        {
            Debug.Assert(b.ParentBand != null);
            BandLimits bnd = b.ParentBand;
            BandLimits SubBandEdit = null;
            if (SubBand != null)
            {
                SubBandEdit = SubBand;
            }

            var state = b.State;
            bool editParent = state == EditBandState.Create_New_MainBand
                || state == EditBandState.Edit_MainBand;

            if (!editParent)
            {
                Debug.Assert(this.SubBand != null);
                bnd = this.SubBand;
                this.txtLo.Text = b.ParentBand.MinFreq.ToString();
                this.txtHi.Text = b.ParentBand.MaxFreq.ToString();
                if (SubBandEdit == null)
                {
                    this.Text
                        = "Creating Sub-band within " + b.ParentBand.UniqueName;
                }
                else
                {
                    this.Text
                        = "Editing Sub-band within " + b.ParentBand.UniqueName;
                    txtName.Text = SubBandEdit.UniqueName;
                }
                chkChannels.Visible = false; // only for parent
            }
            else
            {
                chkChannels.Visible = true;
                chkChannels.Checked = bnd.Channelised;
                txtLo.Text = bnd.MinFreq.ToString();
                txtHi.Text = bnd.MaxFreq.ToString();
            }

            if (SubBandEdit == null)
            {
                txtName.Text = bnd.UniqueName;
            }
            else
            {
                txtName.Text = SubBandEdit.UniqueName;
            }

            chkTX.Checked = bnd.TXAllowed;
            this.BandModes = bnd.AllowedModes;

            this.txtName.TextBoxControl.SelectionLength = txtName.Text.Length;
            txtName_TextChanged(txtName, EventArgs.Empty); // update the form title
            txtName.Focus();
        }

        public bool BandChannelised
        {
            get { return chkChannels.Checked; }
        }
        public string BandName
        {
            get { return txtName.Text; }
        }
        public bool BandTx
        {
            get { return chkTX.Checked; }
        }
        public double BandLo { get => double.Parse(txtLo.Text); }
        public double BandHi { get => double.Parse(txtHi.Text); }
        public bool OK { get; private set; }

        public string ValidateData()
        {
            if (string.IsNullOrEmpty(BandName))
                return "Band Name must not be empty.";
            try
            {

                if (BandLo < m_frmMain.VFO.FreqManager.MinFreqMHz)
                {
                    txtLo.Focus();
                    txtLo.Text = m_frmMain.VFO.FreqManager.MinFreqMHz.ToString();
                    txtLo.SelectionLength = txtLo.Text.Length;

                    return "Lowest band frequency is lower than the lowest radio frequency";
                }

                if (BandHi > m_frmMain.VFO.FreqManager.MaxFreqMHz)
                {
                    txtHi.Focus();
                    txtHi.Text = m_frmMain.VFO.FreqManager.MaxFreqMHz.ToString();
                    txtHi.SelectionLength = txtHi.Text.Length;

                    return "Highest band frequency is lower than the highest radio frequency";
                }
                if (BandLo > BandHi)
                {
                    txtLo.Focus();

                    txtLo.SelectionLength = txtLo.Text.Length;
                    return "Low frequency limit must be lower than the high frequency limit of the band";
                }

                if (BandName.StartsWith("Type the"))
                {
                    txtName.Focus();
                    txtName.SelectionLength = txtName.Text.Length;
                    return "You must type a band name.";
                }

                var state = this.Band.State;
                bool editParent = state == EditBandState.Edit_MainBand;
                if (editParent)
                {
                    if (BandLo < Band.ParentBand.MinFreq)
                        return "Lowest sub-band frequency must not be lower than "
                            + Band.ParentBand.UniqueName + "'s lowest frequency ("
                            + Band.ParentBand.MinFreq.ToString() + " MHz)";
                    if (BandHi > Band.ParentBand.MaxFreq)
                        return "Highest sub-band frequency must not be higher than "
                            + Band.ParentBand.UniqueName + "'s highest frequency ("
                            + Band.ParentBand.MaxFreq.ToString() + " MHz)";
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

            return "";
        }

        static readonly public string DefaultText = "Type the band name here ...";
        private int m_ModeShowCount
            = 0; // hack because closing mode form seems to close US, too!
        private void FrmBandEdit_FormClosing(
            object sender, FormClosingEventArgs e)
        {
            if (this.OK)
            {
                var s = ValidateData();
                if (!string.IsNullOrEmpty(s))
                {
                    var a = MessageBox.Show(
                        "Bad Input:\n" + s + "\n\nDo you want to correct it?",
                        App.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (a == DialogResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        this.OK = false;
                    }
                }
            }
            if (e.CloseReason == CloseReason.None)
            {
                if (m_ModeShowCount <= 0)
                {
                    this.Hide();
                    e.Cancel = true;
                }

                m_ModeShowCount--;

            }
            else
            {
                this.Hide();
            }
            e.Cancel = true;
        }
        protected override void OnResize(EventArgs e) { base.OnResize(e); }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.OK = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.OK = false;
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            var state = this.Band.State;
            bool editParent = state == EditBandState.Create_New_MainBand
                || state == EditBandState.Edit_MainBand;
            string name = txtName.Text;
            if (name.StartsWith("Type the "))
            {
                name = "";
            }
            if (editParent)
            {
                if (state == EditBandState.Create_New_MainBand)
                    this.Text = "Creating New band: " + name;
                else
                    this.Text = "Editing Band " + name;
            }
            else
            {

                if (state == EditBandState.Create_new_SubBand)
                    this.Text = "Creating Sub-band " + name
                        + " in band: " + this.Band.ParentBand.UniqueName;
                else
                    this.Text = "Editing Sub-band " + name
                        + " in band: " + this.Band.ParentBand.UniqueName;
            }
        }

        public Modes.ModeKind BandModes { get; private set; }
        private void btnModes_Click(object sender, EventArgs e)
        {
            FrmModePicker f = new FrmModePicker();
            if (f.ShowForm(this) == DialogResult.OK)
            {
                this.BandModes = f.SelectedModes;
            }
            m_ModeShowCount++;
        }
    }
}
