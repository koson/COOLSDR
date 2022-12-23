// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com

using CoolComponentsTest;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using TextBox = System.Windows.Forms.TextBox;

[assembly: CLSCompliant(true)]
namespace CoolComponents
{

    /*/
    internal class CoolTextDesigner : CoolTextBox
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CoolTextDesigner
            // 
            this.Name = "CoolTextDesigner";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
    /*/


    [DefaultEvent("TextChanged")]
    public partial class CoolTextBox : UserControl
    {

        public new event EventHandler TextChanged;
        public event EventHandler OnBeforeKeyPressHandled;
        public event EventHandler<KeyPressEventArgs> OnBeforeKeySubmit;
        public new MouseEventHandler MouseMove;

        public event EventHandler<MouseEventArgs> MouseMoveOverText;
        public event EventHandler<KeyEventArgs> OnBeforeKeyDown;


        private Color borderColor = Color.MediumSlateBlue;
        private bool borderColorDarkenOnFocus = true;
        private bool borderThickenOnFocus = true;
        private Color borderFocusColor = Color.Empty;
        private Color borderInactiveColor = Color.Empty;
        private int borderSize = 1;
        private bool underlinedStyle = false;

        public CoolTextBox()
        {
            InitializeComponent();
            TextBoxCtrl.Leave += this.TextBox1Leave;
            TextBoxCtrl.Enter += this.TextBox1Enter;
            TextBoxCtrl.Click += this.TextBox1Click;
            TextBoxCtrl.MouseEnter += this.TextBox1MouseEnter;
            TextBoxCtrl.MouseLeave += this.TextBox1Leave;
            TextBoxCtrl.KeyPress += this.TextBox1KeyPress;
            TextBoxCtrl.MouseMove += this.TextBoxMouseMove;
            base.MouseMove += this.LocalMouseMove;
            TextBoxCtrl.MouseMove += this.LocalMouseMoveOverText;
            TextBoxCtrl.TextChanged += TextBox1TextChanged;
            TextBoxCtrl.MouseLeave += this.TextBox1MouseLeave;
            TextBoxCtrl.KeyDown += this.TextBox1KeyDown;
        }

        private void TextBox1KeyDown(object sender, KeyEventArgs e)
        {
            OnBeforeKeyDown?.Invoke(sender, e);

        }

        public int SelectionStart { get => TextBoxCtrl.SelectionStart; set => TextBoxCtrl.SelectionStart = value; }
        public int SelectionLength { get => TextBoxCtrl.SelectionLength; set => TextBoxCtrl.SelectionLength = value; }
        private void LocalMouseMove(object sender, MouseEventArgs e)
        {
            MouseMove?.Invoke(sender, e);
        }

        private void TextBoxMouseMove(object sender, MouseEventArgs e)
        {
            var a = this.TextBoxCtrl.GetCharIndexFromPosition(e.Location);
            //Debug.Print("(CoolTextBox.cs: Mouse is over character position: " + a.ToString());
            LocalMouseMoveOverText(sender, e);
        }

        private void LocalMouseMoveOverText(object sender, MouseEventArgs e)
        {
            var a = this.TextBoxCtrl.GetCharIndexFromPosition(e.Location);
            // Debug.Print("(@CoolTextBox.cs: Mouse is over character position: " + a.ToString());
            MouseMoveOverText?.Invoke(sender, e);
        }

        [Description("The rect inside which the text is drawn *inside* the textBox. " +
                "NOT the same as the client rectangle of the textbox. In screen coordinates"),
                Category("Cool")]
        public Rectangle GetFormattingRect()
        {
            Rectangle r = TextBoxExtensions.GetFormattingRect(this.TextBoxCtrl);
            Rectangle s = TextBoxCtrl.RectangleToScreen(r);
            return s;
        }

        public void SetFormattingRect(Rectangle textRect)
        {
            TextBoxExtensions.SetFormattingRect(TextBoxCtrl, textRect);
            return;
        }



        private void TextBox1TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        #region LocalEvents
        private void TextBox1Enter(object sender, EventArgs e)
        {

            this.Invalidate();
        }
        private void TextBox1Leave(object sender, EventArgs e)
        {

            this.Invalidate();
        }

        private void TextBox1Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
        private void TextBox1MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }
        private void TextBox1MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        static readonly string decSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        public static string DecimalSeparator { get => decSep; }
        private void TextBox1KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnBeforeKeyPressHandled?.Invoke(this, e);
            TextBox textBox = (TextBox)sender;
            if (char.IsControl(e.KeyChar))
            {

            }
            else
            {
                if (NumericOnly)
                {

                    var dec_sep = DecimalSeparator;
                    if (AllowDecimal || AllowMultipleDecimals)
                    {
                        // only allow one decimal point
                        string s = e.KeyChar.ToString();
                        if (s == dec_sep)
                        {
                            if (!AllowMultipleDecimals)
                            {
                                if (textBox.Text.IndexOf(dec_sep, StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    e.Handled = true;
                                    return;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                // return here to prevent numeric checks bypass
                                // for a dec_sep
                                // dont allow decimals at the beginning, ever
                                if (string.IsNullOrEmpty(TextValue))
                                {
                                    e.Handled = true;
                                    return;
                                }
                                else
                                {
                                    // don't allow consecutive decimals
                                    int found;
                                    int last_found = -1;
                                    string newval = TextValue + dec_sep;

                                    while (true)
                                    {
                                        found = newval.IndexOf(dec_sep, last_found + 1, StringComparison.OrdinalIgnoreCase);
                                        if (found == -1) break;
                                        if (found == last_found + 1)
                                        {
                                            e.Handled = true;
                                            return;
                                        }
                                        last_found = found;
                                    }
                                }
                                return;
                            }
                        }
                        else
                        {
                            // not a dec sep, fall through
                        }
                    }

                    if (AllowNegative)
                    {
                        if (string.IsNullOrEmpty(TextValue) && e.KeyChar == '-')
                        {
                            if (textBox.Text.IndexOf('-') > -1)
                            {
                                e.Handled = true;
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }

                    if (!char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                        return;
                    }


                }

            }


            OnBeforeKeySubmit?.Invoke(this, e);

            // base.OnKeyPress(e);
        }

        #endregion

        public HorizontalAlignment TextAlign
        {
            get => TextBoxCtrl.TextAlign;
            set => TextBoxCtrl.TextAlign = value;
        }

        //Properties
        [Category("Cool")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }
        [Category("Cool")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {

                if (value <= 0) value = 0;
                borderSize = value;
                this.Invalidate();
            }
        }
        [Category("Cool")]
        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }

        [Category("Cool")]
        public bool BorderColorDarkenOnFocus
        {
            get => borderColorDarkenOnFocus;
            set
            {
                borderColorDarkenOnFocus = value;
                Invalidate();
            }
        }

        [Category("Cool")]
        public bool AllowNegative { get; set; }
        [Category("Cool")]
        public bool AllowDecimal { get; set; }
        [Category("Cool")]
        public bool PasswordChar
        {
            get { return TextBoxCtrl.UseSystemPasswordChar; }
            set { TextBoxCtrl.UseSystemPasswordChar = value; }
        }

        [Category("Cool")]
        public int MaxTextLength
        {
            get { return TextBoxCtrl.MaxLength; }
            set { TextBoxCtrl.MaxLength = value; }
        }

        [Category("Cool")]
        public bool NumericOnly
        {
            get;
            set;
        }


        [Category("Cool")]
        public bool Multiline
        {
            get { return TextBoxCtrl.Multiline; }
            set { TextBoxCtrl.Multiline = value; }
        }
        [Category("Cool")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                TextBoxCtrl.BackColor = value;
            }
        }
        [Category("Cool")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                TextBoxCtrl.ForeColor = value;
            }
        }
        [Category("Cool")]
        public Font m_fontOverride = null;

        public Font FontOverride
        {
            get { return m_fontOverride; }
            set { m_fontOverride = value; TextBoxCtrl.FontOverride = value; }
        }
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (m_fontOverride != null)
                {
                    value = m_fontOverride;
                }
                base.Font = value;
                TextBoxCtrl.Font = value;
                if (this.DesignMode)
                    UpdateControlHeight();
            }
        }
        [Category("Cool")]
        public string TextValue
        {
            get { return TextBoxCtrl.Text; }
            set { TextBoxCtrl.Text = value; }
        }

        public bool ShortcutsEnabled
        {
            get => TextBoxCtrl.ShortcutsEnabled;
            set => TextBoxCtrl.ShortcutsEnabled = value;
        }


        public new string Text { get => TextValue; set => TextValue = value; }


        [Category("Cool")]
        public Color BorderFocusColor
        {
            get { return borderFocusColor; }
            set { borderFocusColor = value; }
        }
        [Category("Cool")]
        public Color BorderInactiveColor
        {
            get { return borderInactiveColor; }
            set { borderInactiveColor = value; Invalidate(); }
        }

        [Category("Cool")]
        public bool BorderThickenOnFocus
        {
            get
            {
                return borderThickenOnFocus;
            }
            set
            {
                borderThickenOnFocus = value;
                Invalidate();
            }
        }
        [Category("Cool")]
        public bool AllowMultipleDecimals
        {
            get; set;
        }

        public TextBox TextBoxControl
        {
            get => TextBoxCtrl;
        }

        private Color CalcBorderColor()
        {
            if (ContainsFocus)
            {
                if (borderFocusColor != Color.Empty)
                {
                    return borderFocusColor;
                }
                else
                {

                    if (borderColorDarkenOnFocus)
                    {
                        return borderColor;
                    }
                    else
                    {
                        return Color.FromArgb(200, borderColor);
                    }

                }
            }
            else
            {
                if (BorderInactiveColor != Color.Empty)
                {
                    return BorderInactiveColor;
                }
                else
                {
                    if (!borderColorDarkenOnFocus)
                    {
                        return borderColor;
                    }
                    else
                    {
                        return Color.FromArgb(200, borderColor);
                    }
                }
            }

        }

        private int InternalBorderSize()
        {
            if (this.borderThickenOnFocus)
            {
                if (this.ContainsFocus)
                    return borderSize + 1;
                else
                    return borderSize;
            }
            else
            {
                return borderSize;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;
            //Draw border
            using (Pen penBorder = new Pen(borderColor, InternalBorderSize()))
            {
                penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                penBorder.Color = CalcBorderColor();

                if (underlinedStyle)
                {

                    graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                }
                else
                {
                    graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }

            }
        }

        public Rectangle TextRect
        {
            get
            {
                return GetTextRect(this.TextBoxCtrl);
            }
        }

        public bool TextRectSet(Rectangle newTextRect)
        {
            return TextBoxExtensions.SetFormattingRect(this.TextBoxCtrl, newTextRect) == 0;
        }

        public static Rectangle GetTextRect(TextBoxBase textBox)
        {
            return TextBoxExtensions.GetFormattingRect(textBox);
        }

        public static void SetTextRect(TextBoxBase textBox, Rectangle rect)
        {
            TextBoxExtensions.SetFormattingRect(textBox, rect);
            return;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.DesignMode)
                UpdateControlHeight();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }

        private void UpdateControlHeight()
        {
            if (TextBoxCtrl.Multiline == false)
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                TextBoxCtrl.Multiline = true;
                TextBoxCtrl.MinimumSize = new Size(0, txtHeight);
                TextBoxCtrl.Multiline = false;
                this.Height = TextBoxCtrl.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }



    }

    internal class MyTextBox : System.Windows.Forms.TextBox
    {

        public Font m_fontOverride;

        public Font FontOverride
        {
            get { return m_fontOverride; }
            set { m_fontOverride = value; }
        }
        public override Font Font
        {
            get
            {
                if (m_fontOverride != null)
                {
                    return m_fontOverride;
                }
                return base.Font;
            }
            set
            {
                if (m_fontOverride != null)
                {
                    value = m_fontOverride;
                }
                base.Font = value;
            }
        }
    }
}
