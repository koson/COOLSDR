
//using Band = Thetis.Band;
using Timer = System.Windows.Forms.Timer;
using ProgressBar = System.Windows.Forms.ProgressBar;
using Control = System.Windows.Forms.Control;
using System.Windows.Forms;
using System.Drawing;

namespace CoolComponents
{

    public class InstanceTimer : Timer
    {
        public object instance;
        public InstanceTimer(object instance)
        {
            this.instance = instance;
        }
    }
    public class CoolProgressBar : ProgressBar
    {
        private InstanceTimer timer;
        private delegate void RefreshControlDelegate(Control control);  // defines a delegate type

        public void RefreshControl(Control control)
        {
            try
            {
                if (control.InvokeRequired)
                {
                    // Invoke makes the calling thread wait, and I think this is what we want.
                    control.Invoke(new RefreshControlDelegate(RefreshControl), new object[] { control });  // invoking itself
                }
                else
                {
                    Value = (Value + 1) % Maximum;
                    if (this.Visible)
                    {
                        Invalidate();
                    }
                }
            }
            catch (System.Exception) { }
        }
        private void OnTimedEvent(object sender, System.EventArgs e)
        {
            InstanceTimer tmr = (InstanceTimer)sender;
            var me = (CoolProgressBar)tmr.instance;
            Control control = (Control)me;
            RefreshControl(control);
        }
        public CoolProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


        }

        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
        }


        public new int Value
        {
            get => base.Value;
            set
            {
                this.Invalidate();
                base.Value = value;
            }
        }

        public new ProgressBarStyle Style
        {
            get => base.Style;
            set
            {
                if (value == ProgressBarStyle.Marquee)
                {
                    timer = new InstanceTimer(this);
                    timer.Tick += OnTimedEvent;
                    timer.Interval = 10;
                }
                base.Style = value;
            }
        }

        public new Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
            }
        }

        protected override void OnStyleChanged(System.EventArgs e)
        {
            base.OnStyleChanged(e);
            HandleTimer();
        }

        private void HandleTimer()
        {
            if (DesignMode) { return; }
            if (Style == ProgressBarStyle.Marquee && timer != null)
            {
                timer.Enabled = Style == ProgressBarStyle.Marquee;
            }
        }
        private SolidBrush brush;
        protected override void OnPaint(PaintEventArgs e)
        {


            HandleTimer();
            if (brush == null || brush.Color != ForeColor)
                brush = new SolidBrush(this.ForeColor);

            Rectangle rec = e.ClipRectangle;
            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (timer != null && timer.Enabled == false)

                if (ProgressBarRenderer.IsSupported)
                {

                    ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
                }
            rec.Height = rec.Height - 4;
            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
        }
    }
}
