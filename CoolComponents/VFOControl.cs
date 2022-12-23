// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com


using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static CoolComponents.VFOControl;

namespace CoolComponents
{


    [DefaultEvent("ValueChanged")]
    public partial class VFOControl : UserControl, IVfo
    {
        [Category("Cool")]
        public new event EventHandler MouseMove;
        private readonly Panel panelUnderliner;
        [Category("Cool")]
        public event EventHandler ValueChanged;
        [Category("Cool")]
        public event EventHandler<HoverIndexChangedArgs> HoverIndexChanged;
        public new event EventHandler MouseEnter;
        public new event EventHandler MouseLeave;



        [Category("Cool")]
        public class HoverIndexChangedArgs : EventArgs
        {
            internal HoverIndexChangedArgs(int old, int new_)
            {
                old_index = old; new_index = new_;
            }
            public readonly int old_index;
            public readonly int new_index;
        }

        public class MouseEventArgsEx : EventArgs
        {
            public MouseEventArgs EventArgs { get; set; }
            public bool DoDefault { get; set; } = true;
            public MouseEventArgsEx(MouseEventArgs e)
            {
                EventArgs = e;
            }
        }

        [Category("Cool")]
        public event EventHandler<MouseEventArgsEx> MouseMoved;
        public new event EventHandler<EventArgs> TextChanged;


        [Description("The current Frequency Manager"), Category("Cool")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public FrequencyManager FreqManager { get; set; } = new FrequencyManager();
        public VFOControl()
        {
            InitializeComponent();
            m_DigitRep = new DigitRep(this.TextBoxControl);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            panelUnderliner = new Panel
            {
                Visible = false,
                Enabled = false // dont want him generating clicks
            };

            this.BackColor = Color.DarkSeaGreen;
            this.TextBackColor = BackColor;
            this.txt.BorderThickenOnFocus = true;
            panelUnderliner.BringToFront();
            panelUnderliner.BackColor = txt.ForeColor; // sane default. And yes, I do _mean_ backColor!
            this.Controls.Add(panelUnderliner);
            txt.MouseMove += LocalMouseMove;
            txt.MouseEnter += LocalMouseEnter;
            txt.MouseLeave += LocalMouseLeave;
            txt.MouseWheel += LocalMouseWheel;
            txt.LostFocus += LocalLostFocus;
            txt.TextChanged += LocalTextChanged;
            SetControlZOrder(panelUnderliner, 0); // make sure he is on top of Zorder so he can be seen
            SetControlZOrder(lblAux1, 1);
            SetControlZOrder(lblAux2, 1);
            FreqManager.Buddy = this;

            FreqManager.FreqInHz = (long)(FrequencyDefaultMHz * 1_000_000);
            lblAux1.Visible = false;
            lblAux2.Visible = false;
            txt.ShortcutsEnabled = false;
            txt.TextBoxControl.ReadOnly = true;
            txt.MouseMoveOverText += LocalMouseMoveOverText;
            txt.OnBeforeKeySubmit += LocalOnBeforeKeySubmit;
            txt.OnBeforeKeyDown += LocalOnBeforeKeyDown;
            txt.Click += LocalClick;
            txt.BorderSize = 1;
            txt.BorderColor = Color.FromArgb(50, 0, 0, 0);
            txt.BorderFocusColor = Color.FromArgb(80, 0, 0, 0);
            txt.BorderColorDarkenOnFocus = false;
        }

        public double ClampFreq(double f)
        {
            if (f <= this.FreqManager.MinFreqMHz)
            {
                f = this.FreqManager.MinFreqMHz;
            }
            if (f >= this.FreqManager.MaxFreqMHz)
            {
                f = this.FreqManager.MaxFreqMHz;
            }
            return f;
        }

        public double FreqInMHz
        {
            get => this.FreqManager.FreqInMHz;
            set => this.FreqManager.FreqInMHz = value;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            MouseEnter?.Invoke(this, e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseLeave?.Invoke(this, e);
        }


        private void LocalClick(object sender, EventArgs e)
        {
            if (Digit >= 0)
            {
                DrawHoverUnderline();
                txt.SelectionStart = Digit;
                txt.SelectionLength = 1;
            }
        }

        private void LocalOnBeforeKeyDown(object sender, KeyEventArgs e)
        {
            bool changed = false;
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right
                || e.KeyCode == Keys.Back)
            {
                this.m_DigitRep.IncrementDigit(e.KeyCode == Keys.Left || e.KeyCode == Keys.Back);

                changed = true;
            }
            if (e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            {
                Digit = e.KeyCode == Keys.Home ? 0 : txt.Text.Length - 1;
                changed = true;
            }

            if (changed)
            {
                timeWhenManualEntry = Now();
            }

            if (Digit >= 0 && Digit < TextBoxControl.Text.Length && changed)
            {
                DrawHoverUnderline();
            }
        }

        private void LocalOnBeforeKeySubmit(object sender, KeyPressEventArgs e)
        {
            var array = txt.Text.ToCharArray();
            int pos = txt.SelectionStart;
            int index;

            if ((Keys)e.KeyChar == Keys.Tab) { }
            if (pos >= 0 && pos <= array.Length)
            {
                index = pos; // prefer to take the index from where the caret is
            }
            else
            {
                index = Digit;
            }
            if (index >= 0 && index < array.Length)
            {
                array[index] = e.KeyChar;
            }
            string s = new string(array);
            // check the freq is ok with the manager
            string shz = s.Replace(".", "");
            bool ok = long.TryParse(shz, out long hz2);
            if (!ok)
            {
                e.KeyChar = (char)0;
                return;
            }
            if (hz2 >= FreqManager.Min && hz2 <= FreqManager.Max)
            {
                txt.Text = s;

                if (index + 1 < array.Length)
                {
                    index++;
                    if (array[index] == '.') index++;
                }
                else
                {
                    index = 0;
                }

                Digit = index;

                txt.SelectionStart = index;
                txt.SelectionLength = 1;


            }
            else
            {
                // illegal frequency
            }

            timeWhenManualEntry = Now();
            DrawHoverUnderline();

            e.KeyChar = (char)0; //to cancel
        }



        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

        }


        #region AuxLabels
        public enum AuxLabels { Aux1, Aux2 }

        [Category("Cool")]
        public Color AuxLabelBackColor(AuxLabels which, Color newColor)
        {
            Label lbl = which == AuxLabels.Aux1 ? lblAux1 : lblAux2;
            Color oldColor = lbl.BackColor;
            if (newColor != oldColor)
            {
                lbl.BackColor = newColor;
            }
            return oldColor;
        }

        [Category("Cool")]
        public Color AuxLabelForeColor(AuxLabels which, Color newColor)
        {
            Label lbl = which == AuxLabels.Aux1 ? lblAux1 : lblAux2;
            Color oldColor = lbl.ForeColor;
            if (newColor != oldColor)
            {
                lbl.ForeColor = newColor;
            }
            return oldColor;
        }


        [Category("Cool")]
        public string AuxLabelText(AuxLabels which)
        {
            Label lbl = which == AuxLabels.Aux1 ? lblAux1 : lblAux2;
            return lbl.Text;
        }

        [Category("Cool")]
        public bool AuxLabelVisibleSet(AuxLabels which, bool vis)
        {
            Label lbl = which == AuxLabels.Aux1 ? lblAux1 : lblAux2;
            bool old_vis = lbl.Visible;
            if (old_vis != lbl.Visible)
                lbl.Visible = vis;

            if (lbl.Visible)
                lbl.BringToFront();

            return old_vis;
        }


        [Category("Cool")]
        public Label AuxLabel(AuxLabels which)
        {
            return which == AuxLabels.Aux1 ? lblAux1 : lblAux2;
        }
        #endregion
        private void LocalTextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private const double DEFAULT_FREQUENCY = 27.555;
        public double FrequencyDefaultMHz
        {
            get;
            set;
        } = DEFAULT_FREQUENCY;

        internal class DigitRep
        {
            private int value = -1;
            internal int prev_value = -1;
            internal int explicit_value = -1;
            internal TextBox m_tb;

            internal int Value
            {
                get => value;
                set
                {
                    // Debug.Print("Setting value to: " + value.ToString() + ", from: " + this.prev_value.ToString());
                    prev_value = value;
                    this.value = value;
                }
            }

            internal DigitRep(TextBox tb)
            {
                Debug.Assert(tb != null);
                m_tb = tb;
            }
            internal int SetDigitValue(int val, bool skip_decimals = true,
                                        bool is_explicit = false, int step = int.MinValue)
            {
                explicit_value = is_explicit ? val : -1;
                int max = m_tb.Text.Length;
                //if (val == prev_value) return val;
                if (val >= max) val = 0;
                if (val < 0) val = -1;
                Value = val;
                if (step != int.MinValue)
                    step = prev_value > Value ? -1 : 1;

                if (val < 0)
                {
                    prev_value = val;
                    value = val;
                    return val;
                }

                Value = GetDigitAvoidingDecimalPoints(step, skip_decimals, Value, m_tb.Text.Length, m_tb.Text);
                prev_value = val;
                UpdateTextSelection(val);
                return Value;
            }

            internal int IncrementDigit(bool minus, bool skipDecimals = true)
            {
                int ret;
                Debug.Assert(this.m_tb != null);
                int txtLen = m_tb.Text.Length;
                string t = m_tb.Text;
                int step = minus ? -1 : 1;
                ret = Value + step;
                if (ret >= txtLen) ret = 0;
                if (ret < 0) ret = 0;
                ret = GetDigitAvoidingDecimalPoints(minus ? -1 : 1, skipDecimals, ret, txtLen, t);


                this.Value = ret;
                this.prev_value = ret;

                UpdateTextSelection(ret);
                return ret;
            }

            private void UpdateTextSelection(int val)
            {
                Debug.Assert(m_tb != null);
                if (val >= 0 && val < m_tb.Text.Length && m_tb.SelectionStart != val
                    && m_tb.SelectionLength != 1)
                {
                    m_tb.SelectionStart = val;
                    m_tb.SelectionLength = 1;
                }
            }

            internal static int GetDigitAvoidingDecimalPoints(int step,
                                        bool skipDecimals, int val, int txtLen, string t)
            {
                if (!skipDecimals) return val;

                while (t.Substring(val, 1) == CoolTextBox.DecimalSeparator)
                {
                    if (step < 0)
                    {
                        val--;
                        if (val < 0) val = txtLen - 1; // wrap
                    }
                    else
                    {
                        val++;
                        if (val >= txtLen) val = 0;
                    }

                };
                return val;
            }
        }

        private readonly DigitRep m_DigitRep;
        public int Digit
        {
            get
            {

                return m_DigitRep.Value;
            }
            set
            {
                var wanted = value;
                m_DigitRep.SetDigitValue(wanted);
            }
        }



        private void LocalLostFocus(object sender, System.EventArgs e)
        {
            // Digit = -1;
        }

        private int GetStep(int charPosition, string text)
        {
#pragma warning disable IDE0059 // unnec' assignment. Yeah, yeah, ffs.
            int step = 0;
            int len = text.Length;
            Debug.Assert(len == 10);
            if (len != 10) return 0;
            if (charPosition < 0) return 0;


            // this assumes text is of the form: xx.xxx.xxx (no spaces in between, see FrequencyManager.DisplayMHz,
            // .DisplaykHz, .DisplayHz)
            // x x . x x x . xx

            switch (charPosition)
            {
                case 9: step = 1; break;
                case 8: step = 10; break;
                case 7: step = 100; break;

                case 6:
                case 2:
                    step = 0; break; // decimal point

                case 5: step = 1_000; break;
                case 4: step = 10_000; break;
                case 3: step = 100_000; break;

                case 1: step = 1_000_000; break;
                case 0: step = 10_000_000; break;
                default: Debug.Assert(false); step = 0; break;
            }
            return step;
        }

        private void LocalMouseWheel(object sender, MouseEventArgs e)
        {

            int position = Digit; // trust LocalMouseMove is doing its thing
            int step = GetStep(position, this.Text);
            if (e.Delta < 0)
            {
                // Debug.Print("Scrolling down?");
                step = -step;

            }
            else
            {
                //Debug.Print("Scrolling up?");
            }
            //Debug.Print("Step is: " + step.ToString());

            this.FreqManager.FreqInHz += (long)step;
        }


        private void LocalMouseEnter(object sender, EventArgs e)
        {

            if (FocusOnMouseEnter)
            {
                txt.Focus(); // do NOT call Select(): it makes VS2022 not respond to input if you do!
            }
        }

        private void LocalMouseLeave(object sender, EventArgs e)
        {
            lastMouseEventArgs = null;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                gTmp?.Dispose();
            }
            base.Dispose(disposing);
        }

        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }

            set
            {
                panelUnderliner.Visible = value;
                base.Enabled = value;

            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (base.Enabled)
            {
                txt.Focus();
                panelUnderliner.Visible = HoverUnderline;
                DrawHoverUnderline();
            }
            else
            {
                panelUnderliner.Visible = false;
            }
            base.OnEnabledChanged(e);


        }


        [Description("The background colour for the internal textbox"), Category("Cool")]
        public Color TextBackColor
        {
            get => txt.BackColor;
            set => txt.BackColor = value;
        }

        [Description("The background colour for the part of the control that is not the textbox kinda like the 'surround' colour."), Category("Cool")]
        public new Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        [Category("Cool")]
        new string Text
        {
            get
            {
                if (txt.TextValue == null)
                {
                    txt.TextValue = "";
                }
                return txt.TextValue;
            }
            set { txt.TextValue = value; }
        }

        [Category("Cool")]
        bool SelectionOnHover { get; set; } = true;

        [Category("Cool")]
        private void TextBox1TextChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        [Category("Cool")]
        public System.Windows.Forms.TextBox TextBoxControl { get => txt.TextBoxControl; }


        Color underlineColor = Color.Empty;

        [Category("Cool")]
        public bool HoverUnderline { get; set; } = true;
        [Category("Cool")]
        public Color UnderlineColor
        {
            get { if (underlineColor == Color.Empty) return txt.ForeColor; else return underlineColor; }
            set { underlineColor = value; this.panelUnderliner.BackColor = value; gTmp = null; }
        }

        [Category("Cool")]
        public new Color ForeColor
        {
            get => txt.ForeColor;
            set
            {
                txt.ForeColor = value;
                this.panelUnderliner.BackColor = value; // yes, I do mean BackColor
                gTmp = null;
            }
        }

        private Font m_fontOverride;
        [Category("Cool")]
        public Font FontOverride
        {
            get => m_fontOverride;
            set { m_fontOverride = value; Font = m_fontOverride; txt.FontOverride = value; }

        }
        public override Font Font
        {
            get { if (m_fontOverride == null) return base.Font; else return m_fontOverride; }
            set
            {
                if (m_fontOverride != null)
                    value = m_fontOverride;

                base.Font = value;
                this.txt.Font = value;
                gTmp = null;
            }
        }

        private int underlineThickness = 3;

        [Category("Cool")]
        public long Value
        {
            get
            {
                return FreqManager.FreqInHz;
            }

            set
            {
                FreqManager.FreqInHz = value;
            }
        }

        [Category("Cool")]
        public double ValueInMhz
        {
            get
            {
                return this.FreqManager.FreqInMHz;
            }
        }

        [Category("Cool")]
        public bool UnderlineDecimals { get; set; } = false;

        [Category("Cool")]
        public int UnderlineThickness { get => underlineThickness; set { underlineThickness = value; this.panelUnderliner.Height = value; Invalidate(); } }

        [Category("Cool")]
        public bool FocusOnMouseEnter { get; private set; } = true;

        [Description("Pad the position of the underline"), Category("Cool")]
        public int UnderlinePadding { get; private set; }
        public CoolComponents.IVfo IVFO { get; set; }

        private DateTimeOffset timeWhenManualEntry;
        private void LocalMouseMoveOverText(object sender, MouseEventArgs e)
        {
            // Weird: we are getting a mouseMove when the mouse DOESN'T move after
            // setting the current digit whilst typing!
            var timeSinceManualEntry = Now() - timeWhenManualEntry;
            if (timeSinceManualEntry.TotalMilliseconds < 50)
            {
                return;
            }
            MouseMove?.Invoke(sender, e);
            var ev = new MouseEventArgsEx(e);
            MouseMoved?.Invoke(sender, ev); // customisation point for optional cancellation.
            if (!ev.DoDefault) return;
            var a = this.txt.TextBoxControl.GetCharIndexFromPosition(e.Location);
            // Debug.Print("(VFOControl.cs: Mouse is over character position: " + a.ToString());
            CalcDigit(e);
        }

        private void LocalMouseMove(object sender, MouseEventArgs e)
        {
            MouseMove?.Invoke(sender, e);
            var ev = new MouseEventArgsEx(e);
            MouseMoved?.Invoke(sender, ev); // customisation point for optional cancellation.
            if (!ev.DoDefault) return;
        }

        private MouseEventArgs lastMouseEventArgs = null;
        void CalcDigit(MouseEventArgs e)
        {
            if (txt.ContainsFocus && e != null)
            {

                // this was tripping over to the next Digit when halfway across the previous one.
                // I didn't like it.
                const int FUDGE = 6;
                var pt = e.Location;
                var mypt = new Point(pt.X - FUDGE, pt.Y);
                int old_digit = Digit;
                int diff = 0;

                int d = this.txt.TextBoxControl.GetCharIndexFromPosition(mypt);
                if (d == CoolTextBox.DecimalSeparator[0])
                {
                    if (lastMouseEventArgs != null)
                    {
                        diff = e.X - lastMouseEventArgs.X;
                        if (diff == 0)
                        {
                            return;
                        }
                        diff = diff < 0 ? -1 : 1;
                    }
                    m_DigitRep.SetDigitValue(d, true, false, diff);
                }
                else
                {
                    Digit = d;
                }
                lastMouseEventArgs = e;

                if (Digit != old_digit)
                {
                    // Debug.Print("Location is " + e.Location + ", Digit = " + Digit);
                    this.Invalidate();
                    HoverIndexChanged?.Invoke(this, new HoverIndexChangedArgs(old_digit, Digit));
                }
                lastMouseEventArgs = e;
            }
        }

        private bool CheckDecimalBehaviour(char c)
        {
            bool ret = true;
            if (c == CoolTextBox.DecimalSeparator[0])
            {
                if (!UnderlineDecimals)
                {

                    return false;
                }
            }
            return ret;
        }

        public int ShowUnderlineAtIndex(int index)
        {
            int ret = -1;
            if (!HoverUnderline) HoverUnderline = true;
            if (index < 0) return index;
            Digit = index;
            this.Invalidate();
            return ret;
        }



        private void DrawHoverUnderline()
        {
            if (!HoverUnderline || !Enabled) return;
            var t = txt.TextBoxControl;
            var tv = txt.TextValue;
            if (string.IsNullOrEmpty(tv)) return;
            var ta = t.TextAlign;
            // I have only coded this for right-aligned text.
            // Exercise for the reader: Implement this for left-aligned text,
            // and, if you are feeling super brave, implement this for centre-aligned text ;-)
            Debug.Assert(ta == HorizontalAlignment.Right);
            if (ta != HorizontalAlignment.Right) return;

            // Debug.Print("Drawing HoverUnderline, for Digit: " + Digit.ToString());

            if (Digit >= 0 && Digit < tv.Length)
            {
                char val = tv.ElementAt(Digit);
                panelUnderliner.Visible = CheckDecimalBehaviour(val);
                if (!panelUnderliner.Visible) return;

                HandleUnderline(t, val);

                if (SelectionOnHover)
                {
                    t.SelectionStart = Digit;
                    t.SelectionLength = 1;
                }

            }
            else
            {
                panelUnderliner.Visible = false;
            }

        }

        public static void SetControlZOrder(Control ctrl, int z)
        {
            ctrl.Parent.Controls.SetChildIndex(ctrl, z);
        }

        private bool DrawUnderline(Panel p, Rectangle myRect)
        {
            if (p.Left == myRect.Left && p.Width == myRect.Width && p.Top == myRect.Top &&
                p.Height == myRect.Height && p.BackColor == this.underlineColor && p.Visible)
            {

                return false;
            }
            if (p.Left != myRect.Left)
                p.Left = myRect.Left;

            if (p.Top != myRect.Top)
                p.Top = myRect.Top;

            if (p.Width != myRect.Width)
                p.Width = myRect.Width;

            if (p.Height != UnderlineThickness)
                p.Height = UnderlineThickness;

            if (p.BackColor != underlineColor)
            {
                p.BackColor = underlineColor;
                p.BringToFront();
            }

            if (!p.Visible)
            {
                p.Visible = true;
                p.BringToFront();
            }


            return true;

        }

        private void HandleUnderline(System.Windows.Forms.TextBox t, char val)
        {
            var mySize = GetCharSize(val);
            int myWidth = (int)mySize.Width;
            int myTop = (int)mySize.Height + txt.Padding.Top + txt.Padding.Bottom + UnderlinePadding;
            int pos = t.GetPositionFromCharIndex(Digit).X;
            int myLeft = pos - txt.Padding.Left;
            myLeft += panelUnderliner.Width;
            int FUDGE = -2;
            myLeft += FUDGE;
            var p = panelUnderliner;
            Rectangle myRect = new Rectangle(myLeft, myTop, myWidth, UnderlineThickness);

            if (DrawUnderline(p, myRect))
            {

                Invalidate();
            }


        }

        Graphics gTmp = null;
        private SizeF GetCharSize(string s)
        {
            if (gTmp == null)
            {
                gTmp = txt.CreateGraphics();
            }
            SizeF size = gTmp.MeasureString(
                s, txt.Font, 1000, StringFormat.GenericTypographic);

            return size;
        }
        private SizeF GetCharSize(char c)
        {
            return GetCharSize(c.ToString());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawHoverUnderline();
        }


        public void FrequencyChanged(double newFreqMHz)
        {
            int oldSelStart = txt.SelectionStart;
            int oldSelLen = txt.SelectionLength;

            FrequencyManager f = FreqManager;
            var s = string.Join(".", f.DisplayMHz, f.DisplaykHz, f.DisplayHz);
            this.txt.Text = s;
            txt.SelectionStart = oldSelStart;
            txt.SelectionLength = oldSelLen;
            if (this.IVFO != null)
            {
                this.IVFO.FrequencyChanged(newFreqMHz);
            }
            Debug.Assert(s.Length == 10);

        }

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        static extern void GetSystemTimePreciseAsFileTime(out long filetime);

        public DateTimeOffset Now()
        {
            GetSystemTimePreciseAsFileTime(out long fileTime);
            return DateTimeOffset.FromFileTime(fileTime);
        }
    }


}
