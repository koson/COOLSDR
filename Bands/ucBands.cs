using CoolSDR.BandsTypes;
using CoolSDR.BandsTypes.NamedBands;
using CoolSDR.Class;
using CoolSDR.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;
using MessageBox = System.Windows.Forms.MessageBox;

namespace CoolSDR.CustomUserControls
{
    public partial class ucBands : System.Windows.Forms.UserControl
    {
        public ucBands()
        {
            InitializeComponent();
        }

        private void BBTXChanged(object sender, EventArgs e)
        {
            bands.BroadbandTX = chkBBTX.Checked;
        }

        private IARURegion CurrentRegion
        {
            get { return bands.CurrentRegion; }
            set
            {

                bands.CurrentRegion = value;

                ShowBands();
            }

        }


        // private bool m_RegionBusy = false;
        private RadioButton m_LastSelRadioButton = null;
        private void MyRegionChanged(object Sender, EventArgs args)
        {
            RadioButton cb = (RadioButton)Sender;
            if (cb == m_LastSelRadioButton && cb.Checked) return;

            var v = MessageBox.Show("Changing region resets the bands to the defaults. Are you sure you want to continue?",
                        App.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (v != DialogResult.Yes)
            {
                m_LastSelRadioButton.Checked = true;

                return;
            }

            if (Sender == radRegionEU && cb.Checked)
            {
                CurrentRegion = IARURegion.Europe;
                m_LastSelRadioButton = cb;

                return;
            }
            if (Sender == radRegionAmerica && cb.Checked)
            {
                CurrentRegion = IARURegion.Americas;
                m_LastSelRadioButton = cb;

                return;
            }
            if (Sender == radRegionPacific && cb.Checked)
            {
                CurrentRegion = IARURegion.AsiaAndPacific;
                m_LastSelRadioButton = cb;

                return;
            }



            Debug.Assert(false); // how'd we ever get here?

        }

        public void Init()
        {
            m_Initting = true;
            Bands = new CoolSDR.BandsTypes.BandsCollection(App.GetDataFolder(), "BandsCollection");
            var r = Bands.CurrentRegion;
            switch (r)
            {
                case IARURegion.Europe: m_LastSelRadioButton = radRegionEU; break;
                case IARURegion.Americas: m_LastSelRadioButton = radRegionAmerica; break;
                case IARURegion.AsiaAndPacific: m_LastSelRadioButton = radRegionPacific; break;
                default: m_LastSelRadioButton = null; break;
            }

            cboWhich.SelectedIndex = 0;
            cboWhich.SelectedIndexChanged += CboWhich_SelectedIndexChanged;

            m_Initting = false;


        }

        private void CboWhich_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWhich.SelectedIndex == 0)
            {
                tv.Tag = bands.HamBands;

            }
            else
            {
                tv.Tag = bands.UserBands;
            }

            ShowBands();
        }

        private BandsTypes.BandsCollection bands;

        public BandsTypes.BandsCollection Bands
        {
            get { return bands; }
            set
            {
                BandsTypes.BandsCollection old_value = bands;
                if (value == null) return; // designer ffs
                bands = value;
                Debug.Assert(value.CurrentRegion != IARURegion.None);
                if (value.CurrentRegion == IARURegion.Europe)
                {
                    this.radRegionEU.Checked = true;
                }
                else if (value.CurrentRegion == IARURegion.Americas)
                {
                    radRegionAmerica.Checked = true;
                }
                else
                {
                    radRegionPacific.Checked = true;
                }

                chkBBTX.Checked = value.BroadbandTX;
                if (old_value == null) // this to avoid events during initial load
                {
                    tv.Tag = Bands.HamBands;
                    tv.CheckBoxes = true;
                    ShowBands();
                    tv.AfterCheck += Tv_AfterCheck;

                    radRegionAmerica.Click += this.MyRegionChanged;
                    this.radRegionEU.Click += this.MyRegionChanged;
                    radRegionPacific.Click += this.MyRegionChanged;
                    chkBBTX.Click += this.BBTXChanged;
                    btnNone.Click += this.BtnSelAll;
                    btnAll.Click += this.BtnSelAll;
                    btnNone.Click += this.BtnSelAll;
                }

            }
        }

        private void ShowBands()
        {
            BandsBase b = (BandsBase)tv.Tag;
            PopBands(tv, b);

        }

        private bool PoppingBands { get; set; }
        private void PopBands(TreeView tv, BandsBase bb)
        {
            try
            {
                PoppingBands = true;
                tv.Nodes.Clear();
                foreach (BandLimits b in bb.Bands)
                {
                    var nd = tv.Nodes.Add(b.ToString());
                    nd.Checked = b.TXAllowed;
                    foreach (var sub in b.SubBands)
                    {
                        var node = nd.Nodes.Add(sub.ToString());
                        node.Checked = sub.TXAllowed;
                    }
                }
            }
            catch (Exception e)
            {
                Thetis.Common.LogException(e, true, "PopBands");
            }
            finally
            {
                PoppingBands = false;
            }
        }

        private bool m_bShownTVCheckError = false;
        private void Tv_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (m_Initting || PoppingBands) return;
                TreeView tv = (TreeView)sender;
                BandWithSub band = GetSelectedBand(e.Node);
                if (e.Node.Parent == null)
                {
                    band.ParentBand.TXAllowed = e.Node.Checked;
                }
                else
                {
                    band.SubBand.TXAllowed = e.Node.Checked;
                }
                band.Collection.Save();
            }
            catch (Exception ex)
            {
                Thetis.Common.LogException(ex, m_bShownTVCheckError == false, "Error on Tv_AfterCheck");
                m_bShownTVCheckError = true;
            }

        }




        private bool m_Initting;



        private void BtnSelAll(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                Button b = (Button)sender;
                TreeView tv = null;
                bool value = false;
                if (b == this.btnAll)
                {
                    tv = this.tv;
                    value = true;
                }
                else if (b == this.btnNone)
                {
                    tv = this.tv;
                    value = false;
                }
                SetAllChecked(tv, value);
            }
            catch (Exception ex)
            {
                Thetis.Common.LogException(ex, true, "Error in SelAll");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void SetAllChecked(TreeView tv, bool val)
        {
            for (int i = 0; i < tv.Nodes.Count; i++)
            {
                tv.Nodes[i].Checked = val;
                foreach (var v in tv.Nodes[i].Nodes)
                {
                    TreeNode tn = (TreeNode)v;
                    tn.Checked = val;
                }
            }

            // this.Bands.Save(); <-- not needed. Tv_AfterCheck does the saving
        }

        private void ListBoxItemCheck(object sender, ItemCheckEventArgs e)
        {
            /*/
            CheckedListBox l = (CheckedListBox)sender;

            var bs = Bands.HamBands.Bands;
            if (l == lb)
            {

            }
            else
            {
                bs = Bands.UserBands.Bands;
            }


            var b = bs[e.Index];
            b.TXAllowed = e.NewValue == CheckState.Checked;
            if (l == lb)
            {
                Bands.HamBands.Save();
            }
            else
            {
                Bands.UserBands.Save();
            }
            /*/

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (m_frmEdit == null)
            {
                m_frmEdit = new FrmBandEdit(Thetis.Common.Console);
            }
            var b = GetSelectedBand(tv.SelectedNode);
            b.State = EditBandState.Create_New_MainBand;
            b.ParentBand = new BandsTypes.NamedBands.CustomBand(FrmBandEdit.DefaultText, 1.8, 2.0, Modes.ModeKind.All, true);
            m_frmEdit.ShowForm(b);
            var be = m_frmEdit;
            if (m_frmEdit.OK)
            {
                var nb = new BandsTypes.NamedBands.CustomBand(be.BandName, be.BandLo, be.BandHi, Modes.ModeKind.All, be.BandTx);

                if (cboWhich.SelectedIndex >= 1)
                {
                    this.Bands.UserBands.Bands.Insert(0, nb);
                    this.Bands.UserBands.Save();
                }
                else
                {
                    this.Bands.HamBands.Bands.Insert(0, nb);
                    this.Bands.HamBands.Save();
                }
                this.PopBands(this.tv, (BandsBase)tv.Tag);
                tv.SelectedNode = tv.Nodes[0];

            }

        }

        private void btnAddSub_Click(object sender, EventArgs e)
        {
            try
            {
                AddSubBand();
            }
            catch (Exception ex)
            {
                Thetis.Common.LogException(ex, true, "Error when Add Sub-Band Button Pressed");
            }
        }

        private void AddSubBand()
        {
            if (tv.SelectedNode == null)
            {
                if (tv.Nodes.Count > 0)
                {
                    tv.SelectedNode = tv.Nodes[0];
                }
            }
            if (tv.SelectedNode != null)
            {
                var b = GetSelectedBand(tv.SelectedNode);

                if (b != null)
                {

                    var sb = new CustomBand(FrmBandEdit.DefaultText, b.ParentBand.MinFreq, b.ParentBand.MaxFreq,
                        Modes.ModeKind.All, b.ParentBand.TXAllowed);
                    BandWithSub bws = new BandWithSub(b.ParentBand, sb, b.Collection, true);
                    bws.State = EditBandState.Create_new_SubBand;
                    if (m_frmEdit == null)
                    {
                        m_frmEdit = new FrmBandEdit(Thetis.Common.console);
                    }

                    var a = m_frmEdit.ShowForm(bws);
                    if (a == DialogResult.OK)
                    {
                        UpdateBandFromFrmEdit(bws.SubBand, m_frmEdit);
                        b.ParentBand.SubBands.Insert(0, bws.SubBand);
                        b.Collection.Save();
                        ReloadAll();
                    }

                }



            }


        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var b = GetSelectedBand(tv.SelectedNode);
                if (b.ParentSelected)
                {
                    b.State = EditBandState.Edit_MainBand;
                }
                EditBand(b);
            }
            catch (Exception ex)
            {
                Thetis.Common.LogException(ex, true, "Error when Edit Button Pressed");
            }
        }

        FrmBandEdit m_frmEdit;

        private void EditBand(BandWithSub b)
        {
            if (m_frmEdit == null)
            {
                m_frmEdit = new FrmBandEdit(Thetis.Common.console);
            }
            var state = b.State;
            bool editParent = state == EditBandState.Create_New_MainBand || state == EditBandState.Edit_MainBand;
            if (!editParent)
            {
                if (b.SubBand == null)
                {
                    b.SubBand = new CustomBand(FrmBandEdit.DefaultText, 1.800, 1.840, Modes.ModeKind.All, true);
                }
            }

            DialogResult ret = m_frmEdit.ShowForm(b);

            if (ret == DialogResult.OK)
            {
                if (editParent)
                {
                    if (b.NewSubBands.Count > 0)
                    {
                        foreach (var bl in b.NewSubBands)
                        {
                            b.ParentBand.AddSubBand(bl);
                        }
                        b.NewSubBands = new List<BandLimits>();

                    }
                    UpdateBandFromFrmEdit(b.ParentBand, m_frmEdit);
                    b.Collection.Save();
                    ReloadAll();
                }
                else
                {
                    UpdateBandFromFrmEdit(b.SubBand, m_frmEdit);
                    b.Collection.Save();
                    ReloadAll();
                }
            }
        }

        void UpdateBandFromFrmEdit(BandLimits band, FrmBandEdit f)
        {
            BandLimits b = band;
            b.UniqueName = f.BandName;
            b.TXAllowed = f.BandTx;
            b.MaxFreq = f.BandHi;
            b.MinFreq = f.BandLo;
            b.Channelised = f.BandChannelised;
            b.AllowedModes = f.BandModes;

        }

        public enum EditBandState
        {
            None = 0,
            Create_New_MainBand = 1,
            Create_new_SubBand = 2,
            Edit_MainBand = 3,
            Edit_SubBand = 4
        }

        public class BandWithSub
        {
            public BandLimits ParentBand;
            public BandLimits SubBand;
            public BandsBase Collection;
            public List<BandLimits> NewSubBands = new List<BandLimits>();
            public bool ParentSelected { get; private set; }
            public EditBandState State { get; set; }
            public BandWithSub(BandLimits blParent, BandLimits blSub, BandsBase collection, bool parentSelected)
            {
                ParentSelected = parentSelected;
                ParentBand = blParent;
                SubBand = blSub;
                Debug.Assert(blParent != null);
                Collection = collection;
            }
            public BandWithSub() { }
        }
        private BandWithSub GetSelectedBand(TreeNode node)
        {
            var nd = node;
            if (tv.Nodes.Count == 0) return new BandWithSub();
            int selIndex = -1;
            if (nd != null)
            {
                selIndex = nd.Index;
            }

            Debug.Assert(tv.Tag != null);
            BandsBase bb = (BandsBase)tv.Tag;
            if (selIndex < 0)
            {
                tv.SelectedNode = tv.Nodes[0];
                selIndex = tv.SelectedNode.Index;
            }
            Debug.Assert(selIndex >= 0);
            if (node == null)
                node = tv.SelectedNode;


            if (node.Parent == null)
            {

                BandLimits editBand = bb.Bands[selIndex];
                return new BandWithSub(editBand, null, bb, true);
            }
            else
            {
                var nde = node;

                nde = GetParentNode(nde);
                BandLimits parentBand = bb.Bands[nde.Index];
                BandLimits subBand = parentBand.SubBands[node.Index];
                return new BandWithSub(parentBand, subBand, bb, false);

            }
        }

        private TreeNode GetParentNode(TreeNode n)
        {
            while (n.Parent != null)
            {
                n = n.Parent;
            }
            return n;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveSelected();
            }
            catch (Exception ex)
            {
                Thetis.Common.LogException(ex, true, "Error when trying to delete band or sub-band");
            }
        }
        void RemoveSelected()
        {
            bool removed = false;
            var b = GetSelectedBand(tv.SelectedNode);
            if (b.ParentSelected)
            {
                var a = MessageBox.Show("Really delete band " + b.ParentBand.UniqueName + " and all the sub-bands it contains?\n\nAre you sure? You cannot undo this action!",
                    App.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (a == DialogResult.Yes)
                {
                    removed = true;
                    bool ret = b.Collection.RemoveBand(b.ParentBand);
                    if (!ret)
                    {
                        Thetis.Common.LogString("Failed to remove band " + b.ParentBand.UniqueName, true);
                    }

                }
            }
            else
            {
                var a = MessageBox.Show("Really delete band " + b.SubBand.UniqueName + " in the band " + b.ParentBand.UniqueName
                    + "?\n\nAre you sure? You cannot undo this action!",
                        App.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (a == DialogResult.Yes)
                {
                    bool ret = b.ParentBand.SubBands.Remove(b.SubBand);
                    removed = true;
                    if (!ret)
                    {
                        Thetis.Common.LogString("Failed to remove band " + b.ParentBand.UniqueName, true);
                    }

                }

            }

            if (removed)
            {

                ReloadAll();
            }
        }

        private void ReloadAll()
        {
            var nd = GetParentNode(tv.SelectedNode);
            var was_sel_node = tv.SelectedNode;
            int was_sel_node_index = was_sel_node.Index;
            int idx = nd.Index;
            ShowBands();
            if (nd != null)
                tv.SelectedNode = tv.Nodes[idx];
            var bb = GetSelectedBand(tv.SelectedNode);
            if (bb.ParentBand.SubBands.Count > 0)
            {
                tv.SelectedNode.Expand();
                if (was_sel_node_index <= tv.SelectedNode.Nodes.Count - 1)
                {
                    tv.SelectedNode = tv.SelectedNode.Nodes[was_sel_node_index];
                }
                else
                {
                    tv.SelectedNode = tv.SelectedNode.Nodes[tv.SelectedNode.Nodes.Count - 1];
                }
            }
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            var a = MessageBox.Show("Really reset all to defaults?\n\nYou will lose all and any bands or sub-bands you have saved.",
               App.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (a == DialogResult.Yes)
            {
                var old_reg = CurrentRegion;
                switch (old_reg)
                {
                    case IARURegion.Americas: CurrentRegion = IARURegion.Europe; break;
                    case IARURegion.Europe: CurrentRegion = IARURegion.Americas; break;
                    default:
                        CurrentRegion = IARURegion.Europe; break;
                }
                CurrentRegion = old_reg;
            }
        }

        private void tv_DoubleClick(object sender, EventArgs e)
        {
            var b = this.GetSelectedBand(tv.SelectedNode);
            if (b.SubBand == null)
                b.State = EditBandState.Edit_MainBand;
            else
                b.State = EditBandState.Edit_SubBand;

            EditBand(b);
        }
    }
}
