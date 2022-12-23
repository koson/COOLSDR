using CoolSDR.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Navigation;
using Thetis;
using Cursor = System.Windows.Forms.Cursor;

namespace CoolSDR.Class
{
    using static DisplayHelper;
    using ConsoleType = FrmMain;
    public class DisplayHelper
    {

        private static readonly MemoryStream msgrab
            = new MemoryStream(Properties.Resources.grab);
        private static readonly MemoryStream msgrabbing
            = new MemoryStream(Properties.Resources.grabbing);
        private Cursor grab = new Cursor(msgrab);
        private Cursor grabbing = new Cursor(msgrabbing);

        private DisplayEngine current_display_engine = DisplayEngine.DIRECT_X;
        private volatile bool pause_DisplayThread;

        private const int WM_SETREDRAW = 11;
        [DllImport("user32.dll")]
        private static extern int SendMessage(
            IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        public static void SuspendDrawing(Control parent)
        {
            if (parent.Visible) return;
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }
        public void ResumeDrawing(Control parent, bool refresh = true)
        {
            if (parent.Visible) return;

            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);

            if (refresh)
            {
                parent.Refresh();
            }
        }

        public bool Pause_DisplayThread { get => pause_DisplayThread; set => pause_DisplayThread = value; }

        public DisplayEngine CurrentDisplayEngine
        {
            get { return current_display_engine; }
            set
            {
                var c = FrmMain.getConsole();
                bool power = c.PowerOn;

                current_display_engine = value;

                if (power)
                {
                    Pause_DisplayThread = true;
                    c.PowerOn = false; // wtf?
                    Thread.Sleep(100);
                }

                Display.CurrentDisplayEngine
                    = value; // moved so that display engine knows what to do in
                             // Init() //MW0LGE

                switch (value)
                {
                    case DisplayEngine.GDI_PLUS:
                        {
                            Display.ShutdownDX2D();
                        }
                        break;
                    case DisplayEngine.DIRECT_X:
                        {
                            Display.InitDX2D();
                            // Display.Init();
                        }
                        break;
                }

                Display.Target = c.PicDisplay; // MW0LGE reset this, causes an update
                                               // to the arrays, and rebuilds bitmaps

                Pause_DisplayThread = false;

                if (power) c.PowerOn = true;
            }
        }

        public void UpdateDisplay()
        {
            var specTX = FrmMain.getConsole().specRX;
            var c = FrmMain.getConsole();
            switch (current_display_engine)
            {
                case DisplayEngine.GDI_PLUS:
                    c.specRX.GetSpecRX(0).Pixels = c.PicDisplay.Width;
                    c.specRX.GetSpecRX(1).Pixels = c.PicDisplay.Width;
                    c.specRX.GetSpecRX(cmaster.inid(1, 0)).Pixels = c.PicDisplay.Width;

                    c.PicDisplay.Invalidate();
                    break;
            }
        }
        //
        private static bool isBitSet(int n, int pos)
        {
            return (n & (1 << pos)) != 0;
        }
        //

        // private bool old_psautocal = false;
        private int HaveSync = 1;
        //  private int change_overload_color_count = 0;
        private int oload_select
            = 0; // selection of which overload to display this time
        private const int num_oloads = 2; // number of possible overload displays

        private async void UpdatePeakText()
        {
            var c = FrmMain.getConsole();
            int ooo = 0;
            ooo = NetworkIO.getOOO();
            HaveSync = NetworkIO.getHaveSync();
            if (HaveSync == 0) c.PowerOn = false;

            int adc_oload_num = NetworkIO.getAndResetADC_Overload();
            bool adc_oload = adc_oload_num > 0;
            bool amp_oload
                = c.AmpProtect && cmaster.GetAndResetAmpProtect(0) == 1;
            if (amp_oload)
            {
                c.ptbPWR.Value -= 2;
                await Task.Delay(100);
                cmaster.GetAndResetAmpProtect(0);
            }
            bool overload = adc_oload || amp_oload;
            if (adc_oload && amp_oload)
                oload_select = ++oload_select % num_oloads;
            else if (adc_oload)
                oload_select = 0;
            else if (amp_oload)
                oload_select = 1;

            throw new NotImplementedException();
            /*/ fixme  
            if (overload)
            {
                //visible = true;
                switch (oload_select)
                {
                    case 0:
                        switch (adc_oload_num)
                        {
                            case 1: infoBar.Warning("ADC1 Overload!"); break;
                            case 2: infoBar.Warning("ADC2 Overload!"); break;
                            case 4: infoBar.Warning("ADC3 Overload!"); break;
                            default: infoBar.Warning("ADC Overload!"); break;
                        }
                        break;
                    case 1: infoBar.Warning("AMP OVERLOAD!"); break;
                }
                change_overload_color_count = ++change_overload_color_count % 2;

            }
            else
            {
                // if (!tx_inhibit)
                // txtOverload.Text = "";
                change_overload_color_count = 0;

                if (ooo > 0)
                {

                    string s = "";
                    if (isBitSet(ooo, 0)) s += "CC ";
                    if (isBitSet(ooo, 1)) s += "DDC0 ";
                    if (isBitSet(ooo, 2)) s += "DDC1 ";
                    if (isBitSet(ooo, 3)) s += "DDC2 ";
                    if (isBitSet(ooo, 4)) s += "DDC3 ";
                    if (isBitSet(ooo, 5)) s += "DDC4 ";
                    if (isBitSet(ooo, 6)) s += "DDC5 ";
                    if (isBitSet(ooo, 7)) s += "DDC6 ";
                    if (isBitSet(ooo, 8)) s += "Mic ";

                    int[] nSeqLogData = new int[40]; // MAX_IN_SEQ_LOG
                    StringBuilder sDateTimeStamp = new StringBuilder(
                        24); // same size as dateTimeStamp in network.h

                    bool bNegative = false;
                    bool bDCCSeqErrors = false;
                    for (int nDCC = 0; nDCC < 7; nDCC++)
                    {
                        if (isBitSet(ooo, nDCC + 1))
                        {
                            bDCCSeqErrors = true;
                            string ss = "DCC" + nDCC.ToString()
                                + System.Environment.NewLine;

                            int n = 0;
                            bool bInit = true;
                            uint rec_seq; //= 0;
                            uint last_seq; // = 0;
                            while (NetworkIO.getSeqInDelta(bInit, nDCC, nSeqLogData,
                                sDateTimeStamp, out rec_seq, out last_seq))
                            {
                                bInit = false;
                                ss += "s" + n.ToString() + "=";
                                for (int ff = 0; ff < nSeqLogData.Length; ff++)
                                {
                                    ss += nSeqLogData[ff].ToString() + " ";
                                    if (nSeqLogData[ff] < 0)
                                        bNegative = true; // there have been negative
                                                          // packets, these are out
                                                          // of order, important !
                                }
                                ss += " r:" + rec_seq.ToString()
                                    + " l:" + last_seq.ToString() + " "
                                    + sDateTimeStamp.ToString()
                                    + System.Environment.NewLine;
                                n++;
                            }
                            m_frmSeqLog.LogString(ss);
                        }
                    }

                    if (bDCCSeqErrors)
                    {
                        bool bShow = true;
                        if (bNegative)
                        {
                            toolStripStatusLabel_SeqWarning.BackColor = Color.Red;
                            DumpCap.StopDumpcap();
                        }
                        else
                        {
                            toolStripStatusLabel_SeqWarning.BackColor
                                = Color.Transparent;
                            if (!DumpCap.KillOnNegativeSeqOnly)
                                DumpCap.StopDumpcap();
                            bShow = !m_frmSeqLog.StatusBarWarningOnNegativeOnly;
                        }

                        DumpCap.StartDumpcap(2000);

                        toolStripStatusLabel_SeqWarning.Visible = bShow;
                    }

                    // txtOverload.ForeColor = Color.Red;
                    lblOverload
                   = "Seq=> " + ooo.ToString() + " (" + s.Trim() + ")";
                }
                else if (tx_inhibit)
                {
                    //lblOverload = "TX Inhibit";
                    infoBar.Warning("TX Inhibit");
                }
                else
                {
                    infoBar.Warning("");
                }
            }
            switch (change_overload_color_count)
            {
                 case 0: infoBar.WarningLabel.ForeColor = Color.Red; break;
                case 1: infoBar.WarningLabel.ForeColor = Color.Yellow; break;
            }

            if (txtVFOAFreq.Text == "" || txtVFOAFreq.Text == "."
                || txtVFOAFreq.Text == ",")
                return;

            // update peak value
            float x = PixelToHz(Display.MaxX);
            float y = PixelToDb(Display.MaxY);
            // y = Display.MaxY;

            double freq
                = double.Parse(txtVFOAFreq.Text) + (double)x * 0.0000010;

            if (rx1_dsp_mode == DSPMode.CWL)
                freq += (double)cw_pitch * 0.0000010;
            else if (rx1_dsp_mode == DSPMode.CWU)
                freq -= (double)cw_pitch * 0.0000010;


            if (old_psautocal != chkFWCATUBypass.Checked)
            {
                old_psautocal = chkFWCATUBypass.Checked;
                if (chkFWCATUBypass.Checked)
                {
                    if (this.mox)
                    {
                        lblDisplayPeakOffset = "PureSignal 2";
                        lblDisplayPeakPower = "Feedback";
                        lblDisplayPeakPower = "Correcting";
                    }
                    else
                    {

                    }
                }
                else
                {
                }
            }

            if (!chkFWCATUBypass.Checked || !mox)
            {
                //lblDisplayPeakOffset.BackColor = peak_background_color;
                // lblDisplayPeakPower.BackColor = peak_background_color;
                // lblDisplayPeakFreq.BackColor = peak_background_color;
                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.HISTOGRAM:
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.SPECTRUM:
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANASCOPE:
                    case DisplayMode.SPECTRASCOPE:
                        //lblDisplayPeakOffset.ForeColor = peak_text_color;
                        //lblDisplayPeakPower.ForeColor = peak_text_color;
                        //lblDisplayPeakPower.ForeColor = peak_text_color;
                        lblDisplayPeakOffset = x.ToString("f1") + "Hz";
                        lblDisplayPeakPower = y.ToString("f1") + "dBm";
                        // txtDisplayPeakPower.Text = "Fuck off";
                        double Freq = double.Parse(txtVFOAFreq.Text);
                        string temp_text;
                        if (click_tune_display
                            && !mox) // Correct Right hand peak
                                     // frequency when CTUN on -G3OQD
                            temp_text
                                = (freq + (center_frequency - Freq)).ToString("f6")
                                + " MHz"; // Disply Right hand peak frequency under
                                          // Spectrum - G3OQD
                        else
                            temp_text = freq.ToString("f6")
                                + " MHz"; // Right hand - Peak frequency readout

                        int jper = temp_text.IndexOf(separator) + 4;
                        lblDisplayPeakFreq = String.Copy(temp_text.Insert(jper, " "));
                        break;
                    default:
                        lblDisplayPeakOffset = "";
                        lblDisplayPeakFreq = "";
                        lblDisplayPeakFreq = "";
                        break;
                }
                //lblOverload.Visible = visible;
            
            }
            /*/
        }

        public void UpdatePAVoltsAmpsDisplay()
        {

        }

        private Color txtcenterBackColor = Color.Black;
        public Color TxtCenterBackColor
        {
            set
            {
                txtcenterBackColor = value;
                //lblDisplayPeakFreq.BackColor = value;
            }
        }

        private Color txtrightBackColor = Color.Black;
        public Color TxtRightBackColor
        {
            set
            {
                txtrightBackColor = value;
                //lblDisplayPeakFreq.BackColor = value;
            }
        }

        private Color txtleftForeColor = Color.Red;
        public Color TxtLeftForeColor
        {
            set
            {
                txtleftForeColor = value;
                //lblDisplayPeakOffset.ForeColor = value;
                // ucInfoBar.lblFB.
            }
        }




        private int HzInNPixels(int nPixelCount, int rx)
        {
            var c = FrmMain.getConsole();
            int low, high;
            if (!c.MOX)
            {
                if (rx == 1)
                {
                    if (Display.CurrentDisplayMode == DisplayMode.SPECTRUM
                        || Display.CurrentDisplayMode == DisplayMode.HISTOGRAM)
                    // Display.CurrentDisplayMode != DisplayMode.SPECTRASCOPE)
                    {
                        low = Display.RXSpectrumDisplayLow;
                        high = Display.RXSpectrumDisplayHigh;
                    }
                    else
                    {
                        low = Display.RXDisplayLow;
                        high = Display.RXDisplayHigh;
                    }
                }
                else
                {
                    if (Display.CurrentDisplayMode == DisplayMode.SPECTRUM
                        || Display.CurrentDisplayMode == DisplayMode.HISTOGRAM)
                    // Display.CurrentDisplayMode != DisplayMode.SPECTRASCOPE)
                    {
                        low = Display.RX2SpectrumDisplayLow;
                        high = Display.RX2SpectrumDisplayHigh;
                    }
                    else
                    {

                        low = Display.RX2DisplayLow;
                        high = Display.RX2DisplayHigh;
                    }
                }
            }
            else
            {
                if (Display.CurrentDisplayMode == DisplayMode.SPECTRUM
                    || Display.CurrentDisplayMode == DisplayMode.HISTOGRAM)
                {
                    if (c.display_duplex)
                    {
                        low = Display.RXSpectrumDisplayLow;
                        high = Display.RXSpectrumDisplayHigh;
                    }
                    else
                    {
                        low = Display.TXSpectrumDisplayLow;
                        high = Display.TXSpectrumDisplayHigh;
                    }
                }
                else
                {
                    if (display_duplex)
                    {
                        low = Display.RXDisplayLow;
                        high = Display.RXDisplayHigh;
                    }
                    else
                    {
                        low = Display.TXDisplayLow;
                        high = Display.TXDisplayHigh;
                    }
                }
            }

            int width = high - low;
            return (int)((double)nPixelCount / (double)c.PicDisplay.Width
                * (double)width);
        }

        private float PixelToHz(float x)
        {
            var c = FrmMain.getConsole();
            int low, high;
            if (!c.MOX)
            {
                if (Display.CurrentDisplayMode == DisplayMode.SPECTRUM
                    || Display.CurrentDisplayMode == DisplayMode.HISTOGRAM)
                // Display.CurrentDisplayMode != DisplayMode.SPECTRASCOPE)
                {
                    low = Display.RXSpectrumDisplayLow;
                    high = Display.RXSpectrumDisplayHigh;
                }
                else
                {
                    low = Display.RXDisplayLow;
                    high = Display.RXDisplayHigh;
                }
            }
            else
            {
                if (Display.CurrentDisplayMode == DisplayMode.SPECTRUM
                    || Display.CurrentDisplayMode == DisplayMode.HISTOGRAM)
                // Display.CurrentDisplayMode != DisplayMode.SPECTRASCOPE)
                {
                    low = Display.TXSpectrumDisplayLow;
                    high = Display.TXSpectrumDisplayHigh;
                }
                else
                {

                    low = Display.TXDisplayLow;
                    high = Display.TXDisplayHigh;
                }
            }

            if (!c.MOX)
            {
                if (c.RIT)
                {
                    int offset = (int)c.RITValue;
                    low += offset;
                    high += offset;
                }
            }
            else
            {
                if (c.XIT)
                {
                    int offset = (int)c.XITValue;
                    low += offset;
                    high += offset;
                }
            }

            int width = high - low;
            return (float)(low
                + (double)x / (double)c.PicDisplay.Width * (double)width);
        }

        private float PixelToHz(float x, int rx)
        {
            int low, high;
            var c = FrmMain.getConsole();
            if (!c.MOX)
            {
                if (rx == 1)
                {
                    if (Display.CurrentDisplayMode == DisplayMode.SPECTRUM
                        || Display.CurrentDisplayMode == DisplayMode.HISTOGRAM)
                    // Display.CurrentDisplayMode != DisplayMode.SPECTRASCOPE)
                    {
                        low = Display.RXSpectrumDisplayLow;
                        high = Display.RXSpectrumDisplayHigh;
                    }
                    else
                    {
                        low = Display.RXDisplayLow;
                        high = Display.RXDisplayHigh;
                    }
                }
                else
                {
                    if (Display.CurrentDisplayMode == DisplayMode.SPECTRUM
                        || Display.CurrentDisplayMode == DisplayMode.HISTOGRAM)
                    // Display.CurrentDisplayMode != DisplayMode.SPECTRASCOPE)
                    {
                        low = Display.RX2SpectrumDisplayLow;
                        high = Display.RX2SpectrumDisplayHigh;
                    }
                    else
                    {

                        low = Display.RX2DisplayLow;
                        high = Display.RX2DisplayHigh;
                    }
                }
            }
            else
            {
                if (Display.CurrentDisplayMode == DisplayMode.SPECTRUM
                    || Display.CurrentDisplayMode == DisplayMode.HISTOGRAM)
                // Display.CurrentDisplayMode != DisplayMode.SPECTRASCOPE)
                {
                    if (c.display_duplex)
                    {
                        low = Display.RXSpectrumDisplayLow;
                        high = Display.RXSpectrumDisplayHigh;
                    }
                    else
                    {
                        low = Display.TXSpectrumDisplayLow;
                        high = Display.TXSpectrumDisplayHigh;
                    }
                }
                else
                {
                    if (c.display_duplex)
                    {
                        low = Display.RXDisplayLow;
                        high = Display.RXDisplayHigh;
                    }
                    else
                    {
                        low = Display.TXDisplayLow;
                        high = Display.TXDisplayHigh;
                    }
                }
            }

            if (!c.MOX)
            {
                if (c.RIT)
                {
                    int offset = (int)c.RITValue;
                    low += offset;
                    high += offset;
                }
            }
            else
            {
                if (c.XIT)
                {
                    int offset = (int)c.XITValue;
                    low += offset;
                    high += offset;
                }
            }

            int width = high - low;
            return (float)(low
                + (double)x / (double)c.PicDisplay.Width * (double)width);
        }

        private int HzToPixel(float freq)
        {
            var c = FrmMain.getConsole();
            int low, high;
            if (!c.MOX)
            {
                low = Display.RXDisplayLow;
                high = Display.RXDisplayHigh;
            }
            else
            {
                low = Display.TXDisplayLow;
                high = Display.TXDisplayHigh;
            }

            int width = high - low;
            return (int)((double)(freq - low) / (double)width
                * (double)c.PicDisplay.Width);
            // return
            // picDisplay.Width/2+(int)(freq/(high-low)*picDisplay.Width);
        }

        private int HzToPixel(float freq, int rx)
        {
            var c = FrmMain.getConsole();
            int low, high;
            if (!c.MOX)
            {
                if (rx == 1)
                {
                    low = Display.RXDisplayLow;
                    high = Display.RXDisplayHigh;
                }
                else
                {
                    low = Display.RX2DisplayLow;
                    high = Display.RX2DisplayHigh;
                }
            }
            else
            {
                low = Display.TXDisplayLow;
                high = Display.TXDisplayHigh;
            }

            int width = high - low;
            return (int)((double)(freq - low) / (double)width
                * (double)c.PicDisplay.Width);
        }

        private float PixelToDb(float y)
        {
            return (float)(Display.SpectrumGridMax
                - y
                    * (double)(Display.SpectrumGridMax
                        - Display.SpectrumGridMin)
                    / Display.RX1DisplayHeight);
        }

        private float PixelToRx2Db(float y)
        {

            if (Display.CurrentDisplayMode == DisplayMode.PANAFALL)
            {
                // if rx1 is a panafall then we need to offset double rx1 height
                // (height returned is just the panadapter section)
                y -= Display.RX1DisplayHeight * 2;
            }
            else
            {
                y -= Display.RX1DisplayHeight;
            }

            return (float)(Display.RX2SpectrumGridMax
                - y
                    * (double)(Display.RX2SpectrumGridMax
                        - Display.RX2SpectrumGridMin)
                    / Display.RX2DisplayHeight);
        }

        private float WaterfallPixelToTime(float y)
        {
            var c = FrmMain.getConsole();
            int h = Display.RX1DisplayHeight;
            if (c.RX2Enabled
                && Display.CurrentDisplayMode == DisplayMode.PANAFALL)
                h *= 2;

            if (c.RX2Enabled && y > h)
            {
                y -= h;
            }
            else if (Display.CurrentDisplayMode == DisplayMode.PANAFALL)
            {
                y -= h;
            }
            float fRet = (y - 16)
                * (Display.WaterfallUpdatePeriod * (1000f / display_fps));
            if (fRet < 0) fRet = 0;
            return fRet;
        }

        private Point drag_notch_start_point;
        private double center_frequency = 0;
        private bool m_bDraggingPanafallSplit;
        private bool rx1_grid_adjust;
        private Point grid_minmax_drag_start_point;
        private bool gridminmaxadjust;
        private bool tx2_grid_adjust;
        private bool tx1_grid_adjust;
        private int grid_minmax_max_y;
        private int grid_minmax_min_y;
        private bool gridmaxadjust;
        private ClickTuneMode current_click_tune_mode = 0;
        private bool tx_high_filter_drag;
        private bool tx_low_filter_drag;
        private bool rx2_low_filter_drag;
        private bool rx1_low_filter_drag;
        private bool rx2_high_filter_drag;
        private bool rx1_high_filter_drag;
        private int whole_filter_start_x;
        private bool tx_whole_filter_drag;
        private bool rx2_whole_filter_drag;
        private bool rx1_whole_filter_drag;
        private int whole_filter_start_low;
        private int whole_filter_start_high;
        private int sub_drag_last_x;
        private double sub_drag_start_freq;
        private bool rx1_sub_drag;
        private int spectrum_drag_last_x;
        private bool rx2_spectrum_drag;
        private bool rx1_spectrum_drag;
        private bool rx2_grid_adjust;
        private bool click_tune_rx2_display = false;
        private bool rx2_spectrum_tune_drag;
        private bool rx2_click_tune_drag;
        private bool click_tune_display = false;
        private bool rx1_spectrum_tune_drag;
        private bool rx1_click_tune_drag;
        private bool m_bShiftKeyDown = false;
        private bool mouse_tune_step = false;
        private bool click_tune_drag = false;
        private bool display_duplex = false;
        private float display_cursor_y = 0;
        //  private int ptbNoiseGateValue;
        //  private bool m_bDraggingNotch;
        // private bool m_bDraggingNotchBW;
        //  private double drag_notch_start_data;
        //private bool m_bDragginNotchBWRightSide;
        //private double max_filter_width;
        private int RX1display_grid_x = 0;
        private int RX1display_grid_w = 0;
        private int RX2display_grid_x = 0;
        private int RX2display_grid_w = 0;
        private bool m_bWaterfallUseRX1SpectrumMinMax = false;
        private bool m_bWaterfallUseRX2SpectrumMinMax = false;
        private int tx_filter_low = 0;
        private int tx_filter_high = 0;
        private bool m_bUseAccurateFrameTiming = true;
        private bool calibration_running = false;
        private Mutex calibration_mutex = new Mutex();
        private bool displaydidit = false; // display did WHAT?? KLJ

        private string txtVFOAFreqText { get => console.txtVFOAFreq; set => console.txtVFOAFreq = value; }

        private string lblDisplayCursorPower { get => console.lblDisplayCursorPower; set => console.lblDisplayCursorPower = value; }
        public object SelectedNotch { get; private set; }
        public float display_fps { get; private set; }
        public bool show_agc { get; private set; }
        public bool agc_knee_drag { get; private set; }
        public bool agc_hang_drag { get; private set; }
        public DSPMode Rx1_display_mode { get => Common.console.RX1DSPMode; set => Common.console.RX1DSPMode = value; }

        public void picDisplayMouseDown(object sender, MouseEventArgs e)
        {
            var c = FrmMain.getConsole();

            switch (e.Button)
            {
                case MouseButtons.Left:

                    bool bOverRX1 = overRX(e.X, e.Y, 1, false); // MW0LGE
                    bool bOverRX2 = overRX(e.X, e.Y, 2, false);

                    // NOTCH MW0LGE
                    if (SelectedNotch != null)
                    {
                        // this will be the notch we have mouse over

                        int nRX = 0;
                        if (bOverRX1
                            && (Display.CurrentDisplayMode == DisplayMode.PANADAPTER
                                || Display.CurrentDisplayMode
                                    == DisplayMode.PANAFALL))
                        {
                            nRX = 1;
                        }
                        else if (bOverRX2
                            && (Display.CurrentDisplayModeBottom
                                    == DisplayMode.PANADAPTER
                                || Display.CurrentDisplayModeBottom
                                    == DisplayMode.PANAFALL))
                        {
                            nRX = 2;
                        }
                        if (nRX != 0)
                        {
                            // the inital click point, delta is worked in mouse_move
                            drag_notch_start_point = new Point(e.X, e.Y);

                            double dMouseVFO = 0;
                            double dCentreFreq = 0;
                            double dCWoffset = 0;

                            if (nRX == 1)
                            {
                                dCentreFreq = center_frequency * 1e6;
                                dMouseVFO = dCentreFreq + PixelToHz(e.X, 1);
                                if (c.RX1DSPMode == DSPMode.CWL)
                                    dCWoffset = (double)c.cw_pitch;
                                else if (c.RX1DSPMode == DSPMode.CWU)
                                    dCWoffset = -(double)c.cw_pitch;
                            }
                            else
                            {
                                dCentreFreq = c.center_rx2_frequency * 1e6;
                                dMouseVFO = dCentreFreq + PixelToHz(e.X, 2);
                                if (c.RX2DSPMode == DSPMode.CWL)
                                    dCWoffset = (double)c.cw_pitch;
                                else if (c.RX2DSPMode == DSPMode.CWU)
                                    dCWoffset = -(double)c.cw_pitch;
                            }
                            dMouseVFO += dCWoffset;

                            // upper and lower sides of the notch
                            /*/
                            double dL
                                = SelectedNotch.FCenter - (SelectedNotch.FWidth / 2);
                            double dH
                                = SelectedNotch.FCenter + (SelectedNotch.FWidth / 2);
                            

                            // convert the upper and lower sides into pixels from
                            // left edge of picDispay
                            int nLpx = HzToPixel(
                                (float)(dL - dCentreFreq - dCWoffset), nRX);
                            int nHpx = HzToPixel(
                                (float)(dH - dCentreFreq - dCWoffset), nRX);

                            
                            bool bNearEdge = false;

                            // default this based on which side of middle the mouse
                            // is so that we get inuative feeling when using shift
                            // modifier to resize ie we are not draggin an edge
                            m_BDragginNotchBWRightSide
                                = (dMouseVFO >= SelectedNotch.FCenter);

                            if (nHpx - nLpx > 8)
                            {
                                // Debug.Print("x={0} lpx={1} hpx={2}", e.X, nLpx,
                                // nHpx); ok, the edges are far enough appart in
                                // pixels to actually check to see if we are over
                                // low or high side
                                if (Math.Abs(e.X - nLpx) < 4)
                                {
                                    m_BDragginNotchBWRightSide = false;
                                    bNearEdge = true;
                                }
                                else if (Math.Abs(e.X - nHpx) < 4)
                                {
                                    m_BDragginNotchBWRightSide = true;
                                    bNearEdge = true;
                                }
                            }

                            if (bNearEdge
                                || m_bShiftKeyDown) // can also hold shift drag to
                                                    // resize the notch
                            {
                                // near edge of notch, let us drag the width
                                drag_notch_start_data = SelectedNotch.FWidth;
                                m_bDraggingNotchBW = true;
                            }
                            else
                            {
                                // drag whole notch, as we are not near the edge
                                drag_notch_start_data = SelectedNotch.FCenter;
                                m_bDraggingNotch = true;
                            }
                            /*/
                            return;
                        }
                    }
                    // END NOTCH

                    // MIDDLE OF PANAFALL MOVEUPDOWN MW0LGE
                    if (!c.RX2Enabled
                        && Display.CurrentDisplayMode == DisplayMode.PANAFALL)
                    {
                        m_bDraggingPanafallSplit
                            = (e.Y >= Display.PanafallSplitBarPos
                                && e.Y < Display.PanafallSplitBarPos + 20);
                        if (m_bDraggingPanafallSplit) return;
                    }
                    // END SPLITTER DRAG


                    switch (Display.CurrentDisplayMode)
                    {
                        case DisplayMode.PANADAPTER:
                        case DisplayMode.PANAFALL:
                        case DisplayMode.HISTOGRAM:
                        case DisplayMode.SPECTRUM:
                        case DisplayMode.PANASCOPE:
                        case DisplayMode.SPECTRASCOPE:
                            if (!c.MOX)
                            {
                                if (rx1_grid_adjust)
                                {
                                    grid_minmax_drag_start_point
                                        = new Point(e.X, e.Y);
                                    gridminmaxadjust = true;
                                    tx1_grid_adjust = false;
                                    grid_minmax_max_y = Display.SpectrumGridMax;
                                    grid_minmax_min_y = Display.SpectrumGridMin;
                                    c.Cursor = grabbing;
                                }

                                if (rx2_grid_adjust)
                                {
                                    grid_minmax_drag_start_point
                                        = new Point(e.X, e.Y);
                                    gridminmaxadjust = true;
                                    tx1_grid_adjust = false;
                                    grid_minmax_max_y = Display.RX2SpectrumGridMax;
                                    grid_minmax_min_y = Display.RX2SpectrumGridMin;
                                    c.Cursor = grabbing;
                                }
                            }
                            else
                            {
                                if ((rx1_grid_adjust && !Display.TXOnVFOB)
                                    || (rx1_grid_adjust && Display.TXOnVFOB
                                        && !c.RX2Enabled)) // &&
                                                           // Display.CurrentDisplayMode
                                                           // != DisplayMode.PANAFALL)
                                {
                                    grid_minmax_drag_start_point
                                        = new Point(e.X, e.Y);
                                    gridminmaxadjust = true;
                                    tx1_grid_adjust = true;
                                    grid_minmax_max_y = Display.TXSpectrumGridMax;
                                    grid_minmax_min_y = Display.TXSpectrumGridMin;
                                    c.Cursor = grabbing;
                                }
                                else if (rx1_grid_adjust && Display.TXOnVFOB)
                                {
                                    grid_minmax_drag_start_point
                                        = new Point(e.X, e.Y);
                                    gridminmaxadjust = true;
                                    tx1_grid_adjust = false;
                                    grid_minmax_max_y = Display.SpectrumGridMax;
                                    grid_minmax_min_y = Display.SpectrumGridMin;
                                    c.Cursor = grabbing;
                                }

                                if (rx2_grid_adjust && Display.TXOnVFOB)
                                {
                                    grid_minmax_drag_start_point
                                        = new Point(e.X, e.Y);
                                    gridminmaxadjust = true;
                                    tx2_grid_adjust = true;
                                    grid_minmax_max_y = Display.TXSpectrumGridMax;
                                    grid_minmax_min_y = Display.TXSpectrumGridMin;
                                    c.Cursor = grabbing;
                                }
                                else if (rx2_grid_adjust && !Display.TXOnVFOB)
                                {
                                    grid_minmax_drag_start_point
                                        = new Point(e.X, e.Y);
                                    gridminmaxadjust = true;
                                    tx2_grid_adjust = false;
                                    grid_minmax_max_y = Display.RX2SpectrumGridMax;
                                    grid_minmax_min_y = Display.RX2SpectrumGridMin;
                                    c.Cursor = grabbing;
                                }
                            }
                            break;
                        case DisplayMode.WATERFALL: break;
                    }
                    // }

                    if (!c.MOX)
                    {
                        switch (Display.CurrentDisplayMode)
                        {
                            case DisplayMode.PANAFALL:
                            case DisplayMode.PANASCOPE:
                            case DisplayMode.PANADAPTER:
                                if (c.RX2Enabled && e.Y > c.PicDisplay.Height / 2)
                                {
                                    if (Display.AGCRX2Knee.Contains(e.X, e.Y)
                                        && show_agc)
                                    {
                                        agc_knee_drag = true;
                                        c.Cursor = grabbing;

                                    }
                                    else if (Display.AGCRX2Hang.Contains(e.X, e.Y)
                                        && show_agc)
                                    {
                                        agc_hang_drag = true;
                                        c.Cursor = grabbing; // Cursors.HSplit;
                                                             // Debug.WriteLine("AGCKnee
                                                             // Y:" + Display.AGCKnee.Y);

                                    }
                                    else
                                    {
                                        agc_knee_drag = false;
                                        agc_hang_drag = false;
                                        // Cursor = Cursors.Cross;
                                    }
                                }
                                else
                                {
                                    if (Display.AGCKnee.Contains(e.X, e.Y)
                                        && show_agc)
                                    {
                                        agc_knee_drag = true;
                                        c.Cursor
                                            = grabbing; // Cursors.HSplit;
                                                        // Debug.WriteLine("AGCKnee
                                                        // Y:" + Display.AGCKnee.Y);

                                    }
                                    else if (Display.AGCHang.Contains(e.X, e.Y)
                                        && show_agc)
                                    {
                                        agc_hang_drag = true;
                                        c.Cursor
                                            = grabbing; // Cursors.HSplit;
                                                        // Debug.WriteLine("AGCKnee
                                                        // Y:" + Display.AGCKnee.Y);

                                    }
                                    else
                                    {
                                        agc_knee_drag = false;
                                        agc_hang_drag = false;
                                        // Cursor = Cursors.Cross;
                                    }
                                }
                                break;
                        }
                    }

                    if (/*!near_notch &&*/
                        !agc_knee_drag && !agc_hang_drag && !gridminmaxadjust
                        && !gridmaxadjust
                        && (current_click_tune_mode != ClickTuneMode.Off
                            || (click_tune_display && bOverRX1)
                            || (click_tune_rx2_display && bOverRX2)))
                    {
                        switch (Display.CurrentDisplayMode)
                        {
                            case DisplayMode.SPECTRUM:
                            case DisplayMode.WATERFALL:
                            case DisplayMode.HISTOGRAM:
                            case DisplayMode.PANADAPTER:
                            case DisplayMode.PANAFALL:
                            case DisplayMode.PANASCOPE:
                                float x = PixelToHz(e.X);
                                double freq;
                                if (c.RX2Enabled
                                    && e.Y > c.PicDisplay.Height / 2) // RX2
                                {
                                    x = PixelToHz(e.X, 2);
                                    // if (!click_tune_rx2_display)
                                    //    freq = double.Parse(txtVFOBFreq.Text) +
                                    //    (double)x * 0.0000010; // click tune
                                    //    w/x-hairs
                                    // else if (click_tune_drag) freq =
                                    // center_rx2_frequency + (double)x * 0.0000010;
                                    // // click tune & drag vfo else freq =
                                    // double.Parse(txtVFOBFreq.Text); // click &
                                    // drag vfo
                                    if (click_tune_rx2_display
                                        && current_click_tune_mode
                                            != ClickTuneMode.Off)
                                        freq = c.center_rx2_frequency
                                            + (double)x * 0.0000010;
                                    else if (current_click_tune_mode
                                        != ClickTuneMode.Off)
                                        freq = double.Parse(c.txtVFOBFreq)
                                            + (double)x
                                                * 0.0000010; // click tune w/x-hairs
                                    else if (click_tune_drag)
                                        freq = c.center_rx2_frequency
                                            + (double)x * 0.0000010; // click tune &
                                                                     // drag vfo
                                    else
                                        freq = double.Parse(
                                            c.txtVFOBFreq); // click & drag vfo

                                    switch (c.RX2DSPMode)
                                    {
                                        case DSPMode.CWL:
                                            freq += (float)c.cw_pitch * 0.0000010;
                                            break;
                                        case DSPMode.CWU:
                                            freq -= (float)c.cw_pitch * 0.0000010;
                                            break;
                                        case DSPMode.DIGL:
                                            freq += (float)c.digl_click_tune_offset
                                                * 0.0000010;
                                            break;
                                        case DSPMode.DIGU:
                                            freq -= (float)c.digu_click_tune_offset
                                                * 0.0000010;
                                            break;
                                    }

                                    if (c.snap_to_click_tuning
                                        && (current_click_tune_mode
                                                != ClickTuneMode.Off
                                            || click_tune_drag)
                                        && c.RX2DSPMode != DSPMode.CWL
                                        && c.RX2DSPMode != DSPMode.CWU
                                        && c.RX2DSPMode != DSPMode.DIGL
                                        && c.RX2DSPMode != DSPMode.DIGU
                                        && Audio.WavePlayback == false)
                                    {
                                        // round freq to the nearest tuning step
                                        long f = (long)(freq * 1000000.0);
                                        int mult = c.CurrentTuneStepHz;
                                        if (f % mult > mult / 2)
                                            f += (mult - f % mult);
                                        else
                                            f -= f % mult;
                                        freq = (double)f * 0.0000010;
                                    }
                                }
                                else
                                {
                                    if (click_tune_display
                                        && current_click_tune_mode
                                            != ClickTuneMode.Off)
                                        freq = center_frequency
                                            + (double)x * 0.0000010;
                                    else if (current_click_tune_mode
                                        != ClickTuneMode.Off)
                                        freq = double.Parse(c.txtVFOAFreq)
                                            + (double)x
                                                * 0.0000010; // click tune w/x-hairs
                                    else if (click_tune_drag)
                                        freq = center_frequency
                                            + (double)x * 0.0000010; // click tune &
                                                                     // drag vfo
                                    else
                                        freq = double.Parse(
                                            c.txtVFOAFreq); // click & drag vfo

                                    switch (c.RX1DSPMode)
                                    {
                                        case DSPMode.CWL:
                                            freq += (float)c.cw_pitch * 0.0000010;
                                            break;
                                        case DSPMode.CWU:
                                            freq -= (float)c.cw_pitch * 0.0000010;
                                            break;
                                        case DSPMode.DIGL:
                                            if (!c.ClickTuneFilter)
                                                freq += (float)c.digl_click_tune_offset
                                                    * 0.0000010;
                                            break;
                                        case DSPMode.DIGU:
                                            if (!c.ClickTuneFilter)
                                                freq -= (float)c.digu_click_tune_offset
                                                    * 0.0000010;
                                            break;
                                    }

                                    if (c.snap_to_click_tuning
                                        && (current_click_tune_mode
                                                != ClickTuneMode.Off
                                            || click_tune_drag)
                                        && c.RX1DSPMode != DSPMode.CWL
                                        && c.RX1DSPMode != DSPMode.CWU
                                        && c.RX1DSPMode != DSPMode.DIGL
                                        && c.RX1DSPMode != DSPMode.DIGU
                                        && Audio.WavePlayback == false)
                                    {
                                        // round freq to the nearest tuning step
                                        long f = (long)(freq * 1000000.0);
                                        int mult
                                            = c.CurrentTuneStepHz; //(int)(wheel_tune_list[wheel_tune_index]
                                                                   //* 1000000.0);
                                        if (f % mult > mult / 2)
                                            f += (mult - f % mult);
                                        else
                                            f -= f % mult;
                                        freq = (double)f * 0.0000010;
                                    }
                                }

                                //  if (click_tune_rx2_display ||
                                //  click_tune_display)
                                // {
                                // spectrum_drag_last_x = e.X;

                                // MW0LGE block below handles dragging top frequency
                                // bars
                                if (current_click_tune_mode == ClickTuneMode.Off)
                                {
                                    if (c.RX2Enabled && e.Y > c.PicDisplay.Height / 2)
                                    {
                                        spectrum_drag_last_x = e.X;
                                        if (click_tune_rx2_display)
                                        {
                                            if (e.Y
                                                < ((c.PicDisplay.Height / 2) + 15))
                                            {
                                                rx2_spectrum_tune_drag = true;
                                                c.Cursor = Cursors.SizeWE;
                                            }
                                            else
                                            {
                                                rx2_click_tune_drag = true;
                                                c.Cursor = grabbing;
                                            }
                                        }
                                        else
                                            rx2_spectrum_drag = true;
                                    }
                                    else
                                    {
                                        spectrum_drag_last_x = e.X;
                                        if (click_tune_display)
                                        {
                                            if (e.Y < 15)
                                            {
                                                rx1_spectrum_tune_drag = true;
                                                c.Cursor = Cursors.SizeWE;
                                            }
                                            else
                                            {
                                                rx1_click_tune_drag = true;
                                                c.Cursor = grabbing;
                                            }
                                        }
                                        else
                                            rx1_spectrum_drag = true;
                                    }
                                }
                                // }

                                if (!rx1_spectrum_drag && !rx2_spectrum_drag)
                                {
                                    if (!c.RX2Enabled)
                                    {
                                        if (!rx1_spectrum_tune_drag)
                                        {
                                            if (current_click_tune_mode
                                                    == ClickTuneMode.VFOA
                                                || (click_tune_display
                                                    && current_click_tune_mode
                                                        != ClickTuneMode.VFOB))
                                            {
                                                c.VFOAFreq = Math.Round(freq, 6);
                                            }
                                            else
                                                c.VFOBFreq = Math.Round(freq, 6);
                                        }
                                    }
                                    else
                                    {

                                        if (current_click_tune_mode
                                                == ClickTuneMode.VFOB
                                            && // red cross hairs
                                            (c.VFOSplit
                                                || c.EnableMultiRX))
                                        {
                                            c.VFOASubFreq = Math.Round(freq, 6);
                                        }
                                        else
                                        {
                                            if (e.Y <= c.PicDisplay.Height / 2)
                                            {
                                                if (!rx1_spectrum_tune_drag)
                                                    c.VFOAFreq = Math.Round(freq, 6);
                                            }

                                            else
                                            {
                                                if (!rx2_spectrum_tune_drag)
                                                    c.VFOBFreq = Math.Round(freq, 6);
                                            }
                                        }
                                    }
                                }

                                if (c.MOX && c.XIT
                                    && current_click_tune_mode == ClickTuneMode.VFOB)
                                {
                                    //  c.udXIT.Value = 0;
                                }
                                break;
                            default: break;
                        }
                    }
                    else if (/*!near_notch &&*/
                        !agc_knee_drag && !agc_hang_drag && !gridminmaxadjust
                        && !gridmaxadjust) // current_click_tune_mode ==
                                           // ClickTuneMode.Off)
                    {
                        switch (Display.CurrentDisplayMode)
                        {
                            case DisplayMode.PANADAPTER:
                            case DisplayMode.WATERFALL:
                            case DisplayMode.PANAFALL:
                            case DisplayMode.PANASCOPE:
                                int low_x = 0, high_x = 0;
                                int vfoa_sub_x = 0;
                                int vfoa_sub_low_x = 0;
                                int vfoa_sub_high_x = 0;
                                if (c.RX2Enabled
                                    && e.Y > c.PicDisplay.Height / 2) // rx2
                                {
                                    if (c.MOX) // && chkVFOBTX.Checked)
                                    {
                                        low_x = HzToPixel(
                                            c.radio.GetDSPTX(0).TXFilterLow);
                                        high_x = HzToPixel(
                                            c.radio.GetDSPTX(0).TXFilterHigh);
                                    }
                                    else if (c.RX2DSPMode != DSPMode.DRM)
                                    {
                                        low_x = HzToPixel(
                                            c.radio.GetDSPRX(1, 0).RXFilterLow, 2);
                                        high_x = HzToPixel(
                                            c.radio.GetDSPRX(1, 0).RXFilterHigh, 2);
                                    }
                                }
                                else
                                {
                                    if (c.MOX) // && chkVFOATX.Checked)
                                    {
                                        low_x = HzToPixel(
                                            c.radio.GetDSPTX(0).TXFilterLow);
                                        high_x = HzToPixel(
                                            c.radio.GetDSPTX(0).TXFilterHigh);
                                    }
                                    else if (c.RX1DSPMode != DSPMode.DRM)
                                    {
                                        low_x = HzToPixel(
                                            c.radio.GetDSPRX(0, 0).RXFilterLow);
                                        high_x = HzToPixel(
                                            c.radio.GetDSPRX(0, 0).RXFilterHigh);
                                    }

                                    if (c.EnableMultiRX && !c.MOX)
                                    {
                                        if (!c.RX2Enabled)
                                        {
                                            vfoa_sub_x = HzToPixel(
                                                (float)((c.VFOBFreq - c.VFOAFreq)
                                                    * 1000000.0));
                                            vfoa_sub_low_x = vfoa_sub_x
                                                + (HzToPixel((int)c.FilterLowValue)
                                                    - HzToPixel(0.0f));
                                            vfoa_sub_high_x = vfoa_sub_x
                                                + (HzToPixel((int)c.FilterHighValue)
                                                    - HzToPixel(0.0f));
                                        }
                                        else
                                        {
                                            vfoa_sub_x = HzToPixel(
                                                (float)((c.VFOASubFreq - c.VFOAFreq)
                                                    * 1000000.0));
                                            vfoa_sub_low_x = vfoa_sub_x
                                                + (HzToPixel((int)c.FilterLowValue)
                                                    - HzToPixel(0.0f));
                                            vfoa_sub_high_x = vfoa_sub_x
                                                + (HzToPixel((int)c.FilterHighValue)
                                                    - HzToPixel(0.0f));
                                        }
                                    }
                                }

                                if (Math.Abs(e.X - low_x) < 3 && e.X < high_x)
                                {
                                    if (c.RX2Enabled
                                        && e.Y > c.PicDisplay.Height
                                                / 2) // rx2_low_filter_drag = true;
                                    {
                                        if (c.MOX && c.VFOBTX)
                                        {
                                            switch (
                                                c.radio.GetDSPTX(0).CurrentDSPMode)
                                            {
                                                case DSPMode.LSB:
                                                case DSPMode.CWL:
                                                case DSPMode.DIGL:
                                                case DSPMode.AM:
                                                case DSPMode.SAM:
                                                case DSPMode.FM:
                                                case DSPMode.DSB:
                                                    tx_high_filter_drag = true;
                                                    break;
                                                default:
                                                    tx_low_filter_drag = true;
                                                    break;
                                            }
                                        }
                                        else
                                            rx2_low_filter_drag = true;
                                    }
                                    else
                                    {
                                        if (c.MOX
                                            && (!c.SplitDisplay
                                                || c.VFOATX))
                                        {
                                            switch (
                                                c.radio.GetDSPTX(0).CurrentDSPMode)
                                            {
                                                case DSPMode.LSB:
                                                case DSPMode.CWL:
                                                case DSPMode.DIGL:
                                                case DSPMode.AM:
                                                case DSPMode.SAM:
                                                case DSPMode.FM:
                                                case DSPMode.DSB:
                                                    tx_high_filter_drag = true;
                                                    break;
                                                default:
                                                    tx_low_filter_drag = true;
                                                    break;
                                            }
                                        }
                                        else
                                            rx1_low_filter_drag = true;
                                    }
                                }
                                else if (Math.Abs(e.X - high_x) < 3)
                                {
                                    if (c.RX2Enabled
                                        && e.Y > c.PicDisplay.Height
                                                / 2) // rx2_high_filter_drag = true;
                                    {
                                        if (c.MOX && c.VFOBTX)
                                        {
                                            switch (
                                                c.radio.GetDSPTX(0).CurrentDSPMode)
                                            {
                                                case DSPMode.LSB:
                                                case DSPMode.CWL:
                                                case DSPMode.DIGL:
                                                    tx_low_filter_drag = true;
                                                    break;
                                                default:
                                                    tx_high_filter_drag = true;
                                                    break;
                                            }
                                        }
                                        else
                                            rx2_high_filter_drag = true;
                                        // Cursor = Cursors.SizeWE;
                                    }
                                    else if (c.MOX
                                        && (!c.SplitDisplay
                                            || (c.SplitDisplay
                                                && c.VFOATX)))
                                    {
                                        switch (c.radio.GetDSPTX(0).CurrentDSPMode)
                                        {
                                            case DSPMode.LSB:
                                            case DSPMode.CWL:
                                            case DSPMode.DIGL:
                                                tx_low_filter_drag = true;
                                                break;
                                            default:
                                                tx_high_filter_drag = true;
                                                break;
                                        }
                                    }
                                    else
                                        rx1_high_filter_drag = true;
                                }
                                else if (e.X > low_x && e.X < high_x)
                                {
                                    whole_filter_start_x = e.X;
                                    if (c.RX2Enabled && e.Y > c.PicDisplay.Height / 2)
                                    {
                                        if (c.MOX && c.VFOBTX)
                                        {
                                            tx_whole_filter_drag = true;
                                            whole_filter_start_low
                                                = c.TXFilterLow;
                                            whole_filter_start_high
                                                = c.TXFilterHigh;
                                        }
                                        else
                                        {
                                            rx2_whole_filter_drag = true;
                                            whole_filter_start_low
                                                = c.radio.GetDSPRX(1, 0).RXFilterLow;
                                            whole_filter_start_high
                                                = c.radio.GetDSPRX(1, 0).RXFilterHigh;
                                        }
                                    }
                                    else
                                    {

                                        if (!c.MOX)
                                        {
                                            rx1_whole_filter_drag = true;
                                            whole_filter_start_low
                                                = c.radio.GetDSPRX(0, 0).RXFilterLow;
                                            whole_filter_start_high
                                                = c.radio.GetDSPRX(0, 0).RXFilterHigh;
                                        }
                                        else
                                        {
                                            tx_whole_filter_drag = true;
                                            whole_filter_start_low
                                                = c.TXFilterLow;
                                            whole_filter_start_high
                                                = c.TXFilterHigh;
                                        }
                                    }
                                }
                                else if (c.EnableMultiRX && !c.MOX
                                    && (e.X > vfoa_sub_low_x - 3
                                        && e.X < vfoa_sub_high_x + 3))
                                {
                                    sub_drag_last_x = e.X;
                                    if (c.RX2Enabled)
                                        sub_drag_start_freq = c.VFOASubFreq;
                                    else
                                        sub_drag_start_freq = c.VFOBFreq;
                                    rx1_sub_drag = true;
                                }
                                else
                                {
                                    spectrum_drag_last_x = e.X;
                                    if (c.RX2Enabled && e.Y > c.PicDisplay.Height / 2)
                                        rx2_spectrum_drag = true;
                                    else
                                        rx1_spectrum_drag = true;
                                }

                                break;
                        }
                    }
                    break;
                case MouseButtons.Right:
                    // if we have a notch highlighted, then all other right click is
                    // ignored
                    if (SelectedNotch != null) return;

                    // right click in the middle splitter bar will recentre it
                    if (!c.RX2Enabled
                        && Display.CurrentDisplayMode == DisplayMode.PANAFALL)
                    {
                        if (e.Y >= Display.PanafallSplitBarPos
                            && e.Y < Display.PanafallSplitBarPos + 20)
                        {
                            Display.PanafallSplitBarPerc = 0.5f;
                            Display.ResetWaterfallBmp();
                            return;
                        }
                    }

                    if (!c.MOX && (rx1_grid_adjust || rx2_grid_adjust))
                    {
                        if (rx1_grid_adjust)
                        {
                            grid_minmax_drag_start_point = new Point(e.X, e.Y);
                            gridmaxadjust = true;
                            tx1_grid_adjust = false;
                            grid_minmax_max_y = Display.SpectrumGridMax;
                            c.Cursor = grabbing;
                        }

                        if (rx2_grid_adjust)
                        {
                            grid_minmax_drag_start_point = new Point(e.X, e.Y);
                            gridmaxadjust = true;
                            tx1_grid_adjust = false;
                            grid_minmax_max_y = Display.RX2SpectrumGridMax;
                            c.Cursor = grabbing;
                        }
                    }
                    else if (c.MOX && (rx1_grid_adjust || rx2_grid_adjust))
                    {
                        if ((rx1_grid_adjust && !Display.TXOnVFOB)
                            || (rx1_grid_adjust && Display.TXOnVFOB
                                && !c.RX2Enabled))
                        {
                            grid_minmax_drag_start_point = new Point(e.X, e.Y);
                            gridmaxadjust = true;
                            tx1_grid_adjust = true;
                            grid_minmax_max_y = Display.TXSpectrumGridMax;
                            c.Cursor = grabbing;
                        }
                        else if (rx1_grid_adjust && Display.TXOnVFOB)
                        {
                            grid_minmax_drag_start_point = new Point(e.X, e.Y);
                            gridmaxadjust = true;
                            tx1_grid_adjust = false;
                            grid_minmax_max_y = Display.SpectrumGridMax;
                            c.Cursor = grabbing;
                        }

                        if (rx2_grid_adjust && Display.TXOnVFOB)
                        {
                            grid_minmax_drag_start_point = new Point(e.X, e.Y);
                            gridmaxadjust = true;
                            tx2_grid_adjust = true;
                            grid_minmax_max_y = Display.TXSpectrumGridMax;
                            c.Cursor = grabbing;
                        }
                        else if (rx2_grid_adjust && !Display.TXOnVFOB)
                        {
                            grid_minmax_drag_start_point = new Point(e.X, e.Y);
                            gridmaxadjust = true;
                            tx2_grid_adjust = false;
                            grid_minmax_max_y = Display.RX2SpectrumGridMax;
                            c.Cursor = grabbing;
                        }
                    }
                    else
                    {
                        switch (current_click_tune_mode)
                        {
                            case ClickTuneMode.Off:
                                c.CurrentClickTuneMode = ClickTuneMode.VFOA;
                                break;
                            case ClickTuneMode.VFOA:
                                if (c.VFOSplit || c.EnableMultiRX)
                                    c.CurrentClickTuneMode = ClickTuneMode.VFOB;
                                else
                                    c.CurrentClickTuneMode = ClickTuneMode.Off;
                                break;
                            case ClickTuneMode.VFOB:
                                c.CurrentClickTuneMode = ClickTuneMode.Off;
                                break;
                        }
                    }
                    break;
                case MouseButtons.Middle:

                    if (SelectedNotch != null)
                    {
                        // move or toggle notch
                        /*/
                        if (m_bShiftKeyDown)
                        {
                            if (removeNotch(SelectedNotch))
                                SelectedNotch = null; // remove the notch, and if ok
                                                      // clear selected MW0LGE
                        }
                        else
                        {
                            toggleNotchActive(SelectedNotch);
                        }
                        /*/
                    }

                    else if (c.CurrentClickTuneMode != ClickTuneMode.Off)
                    {
                        /*/
                        double dFreq;
                        // add notck from cross hair mode with middle mouse
                        if (c.RX2Enabled && e.Y > c.PicDisplay.Height / 2)
                        {
                            dFreq = getFrequencyAtPixel(e.X, 2);
                        }
                        else
                        {
                            dFreq = getFrequencyAtPixel(e.X, 1);
                        }

                        Debug.Print("Middle Mouse notch add @ {0} Hz", dFreq);
                        addNotch(dFreq);
                        /*/
                    }
                    // carry onto the tune step, but give notch priority
                    else if (mouse_tune_step)
                    {
                        if (m_bShiftKeyDown)
                            c.ChangeTuneStepDown(); // MW0LGE
                        else
                            c.ChangeTuneStepUp();
                    }
                    break;
            }
        }

        public void picDisplayMouseLeave(object sender, System.EventArgs e)
        {
            //if (!m_frmNotchPopup.Visible)
            //    SelectedNotch = null; // clear the selected notch (if there was one)
            m_bDraggingPanafallSplit = false;
            var c = FrmMain.getConsole();
            Display.HighlightNumberScaleRX1 = false;
            Display.HighlightNumberScaleRX2 = false;

            c.lblDisplayCursorOffset = "";
            c.lblDisplayCursorPower = "";
            c.lblDisplayCursorFreq = "";
            DisplayCursorX = -1;
            DisplayCursorY = -1;
            Cursor.Current = Cursors.Default;
        }



        private bool overRX(
                int x, int y, int rx, bool bIgnorePanafallWaterfall = true)
        {
            var c = FrmMain.getConsole();
            int nMinHeightRX1 = 0;
            int nMaxHeightRX1 = c.PicDisplay.Height;
            int nMinHeightRX2 = c.PicDisplay.Height / 2;
            int nMaxHeightRX2 = c.PicDisplay.Height;

            if (c.RX2Enabled)
            {
                // top half only
                nMaxHeightRX1 = c.PicDisplay.Height / 2;

                if (Display.CurrentDisplayModeBottom == DisplayMode.PANAFALL
                    && bIgnorePanafallWaterfall)
                {
                    // top half, of bottom half is available only
                    nMaxHeightRX2 = (c.PicDisplay.Height / 4) * 3;
                }
            }

            if (Display.CurrentDisplayMode == DisplayMode.PANAFALL
                && bIgnorePanafallWaterfall)
            {
                if (!c.RX2Enabled)
                {
                    // top half is available only
                    nMaxHeightRX1
                        = Display.PanafallSplitBarPos; // picDisplay.Height / 2;
                }
                else
                {
                    // top half, of top half is available only
                    nMaxHeightRX1 = c.PicDisplay.Height / 4;
                }
            }

            if (rx == 1)
            {
                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.SPECTRUM:
                    case DisplayMode.HISTOGRAM:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANASCOPE:
                    case DisplayMode.SPECTRASCOPE:
                        // check if we are anywhere over area that filters etc can
                        // be adjusted
                        if ((x >= 0 && x < c.PicDisplay.Width)
                            && (y < nMaxHeightRX1 && y >= nMinHeightRX1)) // + 10))
                        {
                            return true;
                        }

                        break;
                }
            }
            else if (c.RX2Enabled && rx == 2)
            {
                switch (Display.CurrentDisplayModeBottom)
                {
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.SPECTRUM:
                    case DisplayMode.HISTOGRAM:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANASCOPE:
                    case DisplayMode.SPECTRASCOPE:
                        // check if we are anywhere over area that filters etc can
                        // be adjusted
                        if ((x >= 0 && x < c.PicDisplay.Width)
                            && (y < nMaxHeightRX2 && y >= nMinHeightRX2)) // + 10))
                        {
                            return true;
                        }
                        break;
                }
            }

            return false;
        }

        private double getFrequencyAtPixel(int x, int nRX)
        {
            var c = FrmMain.getConsole();
            double dFreq = 0;

            if (nRX == 2)
            {
                if (click_tune_rx2_display
                    && current_click_tune_mode != ClickTuneMode.Off)
                {
                    dFreq = (double)PixelToHz(x, 2) + (c.center_rx2_frequency * 1e6);
                }
                else
                {
                    dFreq = (double)PixelToHz(x, 2) + (c.VFOBFreq * 1e6);
                }

                switch (c.RX2DSPMode)
                {
                    case DSPMode.CWU: dFreq -= c.cw_pitch; break;
                    case DSPMode.CWL: dFreq += c.cw_pitch; break;
                }
            }
            else
            {
                if (click_tune_display
                    && current_click_tune_mode != ClickTuneMode.Off)
                {
                    dFreq = (double)PixelToHz(x, 1) + (center_frequency * 1e6);
                }
                else
                {
                    dFreq = (double)PixelToHz(x, 1) + (c.VFOAFreq * 1e6);
                }
                switch (c.RX1DSPMode)
                {
                    case DSPMode.CWU: dFreq -= c.cw_pitch; break;
                    case DSPMode.CWL: dFreq += c.cw_pitch; break;
                }
            }

            return dFreq;
        }

        DSPMode rx1_dsp_mode
        {
            get { return Common.Console.RX1DSPMode; }
            set { Common.Console.RX1DSPMode = value; }
        }

        public int DisplayCursorX { get; private set; }
        public int DisplayCursorY { get; private set; }
        public bool rx2_enabled { get { return Common.Console.RX2Enabled; } private set { Common.console.RX2Enabled = value; } }

        public PictureBox picDisplay { get { return (PictureBox)Display.Target; } private set { Display.Target = value; } }
        static public ConsoleType console { get { return Common.console; } }

        public bool mox { get { return console.MOX; } private set { console.MOX = value; } }

        public RadioBase radio { get { return console.radio; } }

        public bool chkEnableMultiRX { get { return console.EnableMultiRX; } private set { console.EnableMultiRX = value; } }

        public double VFOBFreq { get => console.VFOBFreq; private set => console.VFOBFreq = value; }
        public double VFOAFreq { get => console.VFOAFreq; private set => console.VFOBFreq = value; }
        public double VFOASubFreq { get => console.VFOASubFreq; private set => console.VFOASubFreq = value; }
        public double cw_pitch { get => console.cw_pitch; private set => console.cw_pitch = value; }
        public double center_rx2_frequency { get => console.center_rx2_frequency; private set => console.center_rx2_frequency = value; }
        public DSPMode rx2_dsp_mode { get => console.RX2DSPMode; private set => console.RX2DSPMode = value; }
        public float DisplayGridMax { get; private set; }
        public float DisplayGridMin { get; private set; }
        public float TXGridMax { get; private set; }
        public float TXGridMin { get; private set; }
        public float RX2DisplayGridMax { get; private set; }
        public float RX2DisplayGridMin { get; private set; }
        public AGCMode RX2AGCMode { get => console.RX2AGCMode; private set => console.RX2AGCMode = value; }
        public int AGCRX2FixedGain { get => console.AGCRX2FixedGain; private set => console.AGCRX2FixedGain = value; }
        public AGCMode RX1AGCMode { get => console.RX1AGCMode; private set => console.RX1AGCMode = value; }
        public int AGCFixedGain { get => console.AGCFixedGain; private set => console.AGCFixedGain = value; }
        public int AGCMaxGain { get => console.AGCMaxGain; private set => console.AGCMaxGain = value; }
        public int SetAGCRX2HangThreshold { get => console.SetAGCRX2HangThreshold; private set => console.SetAGCRX2HangThreshold = value; }
        public int AGCHangThreshold { get => console.AGCHangThreshold; private set => console.AGCHangThreshold = value; }
        public int sample_rate_rx1 { get => console.SampleRateRX1; private set => console.SampleRateRX1 = value; }
        public int AGCRX2MaxGain { get => console.AGCRX2MaxGain; private set => console.AGCRX2MaxGain = value; }
        public string txtVFOAFreq { get => console.txtVFOAFreq; private set => console.txtVFOAFreq = value; }
        public string lblDisplayCursorOffset { get => console.lblDisplayCursorOffset; private set => console.lblDisplayCursorOffset = value; }
        public string separator { get => Common.separator; }
        public string lblDisplayCursorFreq { get => console.lblDisplayCursorFreq; private set => console.lblDisplayCursorFreq = value; }
        public int TXFilterLow { get => console.TXFilterLow; private set => console.TXFilterLow = value; }
        public int TXFilterHigh { get => console.TXFilterHigh; private set => console.TXFilterHigh = value; }

        public static double DisplayFPS
        {
            get;
            private set;
        } = 51.0;
        public double DisplayDelay
        {
            get;
            private set;
        } = 1000.0 / DisplayFPS;

        public bool RX2Enabled { get => console.RX2Enabled; private set => console.RX2Enabled = value; }
        public bool chkRX2 { get => console.RX2Enabled; private set => console.RX2Enabled = value; }
        public bool VFOATX { get => console.VFOATX; private set => console.VFOATX = value; }
        public bool chkVFOATX { get => console.VFOATX; private set => console.VFOATX = value; }
        public bool chkVFOBTX { get => console.VFOBTX; private set => console.VFOBTX = value; }
        public bool chkSplitDisplay { get => console.SplitDisplay; private set => console.SplitDisplay = value; }
        public int scope_time { get; private set; }
        public bool VFOBTX { get => console.VFOBTX; }

        internal void picDisplayDoubleClick(object sender, EventArgs e)
        {
            FrmMain c = FrmMain.getConsole();
            int new_val = (int)PixelToDb(display_cursor_y);
            if (!(rx1_grid_adjust || gridmaxadjust))
            {
                if (!c.MOX) // RX1
                {
                    if (rx1_dsp_mode == DSPMode.FM) return;

                    if (new_val > c.ptbSquelchMaximum) new_val = c.ptbSquelchMaximum;
                    if (new_val < c.ptbSquelchMinimum) new_val = c.ptbSquelchMinimum;
                    c.ptbSquelchValue = new_val;
                    // ptbSquelch_Scroll(this, EventArgs.Empty);
                }
                else // TX
                {
                    new_val += 24;
                    if (new_val > c.ptbNoiseGateMaximum)
                        new_val = c.ptbNoiseGateMaximum;
                    if (new_val < c.ptbNoiseGateMinimum)
                        new_val = c.ptbNoiseGateMinimum;
                    c.ptbNoiseGateValue = new_val;
                    //ptbNoiseGate_Scroll(this, EventArgs.Empty);
                }
            }
        }

        internal void picDisplayMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.SPECTRUM:
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANASCOPE:
                    case DisplayMode.HISTOGRAM:
                    case DisplayMode.SPECTRASCOPE:
                        rx1_low_filter_drag = false;
                        rx1_high_filter_drag = false;
                        rx1_whole_filter_drag = false;
                        rx2_low_filter_drag = false;
                        rx2_high_filter_drag = false;
                        rx2_whole_filter_drag = false;
                        tx_low_filter_drag = false;
                        tx_high_filter_drag = false;
                        tx_whole_filter_drag = false;
                        rx1_click_tune_drag = false;
                        rx2_click_tune_drag = false;
                        rx1_spectrum_tune_drag = false;
                        rx2_spectrum_tune_drag = false;

                        agc_knee_drag = false;
                        agc_hang_drag = false;
                        // agc_knee_drag_max_delta_x = 0;
                        // agc_knee_drag_max_delta_y = 0;
                        gridminmaxadjust = false;
                        rx1_grid_adjust = false;
                        rx2_grid_adjust = false;
                        tx1_grid_adjust = false;
                        tx2_grid_adjust = false;

                        // grid_minmax_drag_max_delta_y = 0;

                        // notch_drag = false;
                        // notch_drag_max_delta_x = 0;
                        // notch_drag_max_delta_y = 0;
                        // timerNotchZoom.Enabled = false;
                        // notch_zoom = false;
                        // if (Display.TNFZoom)
                        //{
                        //    Display.TNFZoom = false;
                        //}
                        // stop showing details for this notch in the panadapter
                        // if (notch_drag_active != null)
                        //{
                        //    notch_drag_active.Details = false;
                        //    notch_drag_active = null;
                        //}
                        // rx2_sub_drag = false;
                        break;
                }

                if (rx1_sub_drag)
                {
                    rx1_sub_drag = false;
                    //if (rx2_enabled)
                    //    txtVFOABand_LostFocus(this, EventArgs.Empty);
                    //else
                    //   txtVFOBFreq_LostFocus(this, EventArgs.Empty);
                }

                if (rx1_spectrum_drag)
                {
                    rx1_spectrum_drag = false;
                    //txtVFOAFreq_LostFocus(this, EventArgs.Empty);
                }
                rx2_spectrum_drag = false;
                // Cursor = Cursors.Default;

                /*/
                if (m_bDraggingNotch)
                {
                    // finished dragging a notch, let use change its frequency
                    // MW0LGE
                    m_bDraggingNotch = false;
                    double tmp = SelectedNotch.FCenter;
                    changeNotchCentreFrequency(SelectedNotch, tmp);
                }
                else if (m_bDraggingNotchBW) // can only do one or the other
                {
                    // finished dragging notch BW, lets us change it
                    m_bDraggingNotchBW = false;
                    double tmp = SelectedNotch.FWidth;
                    changeNotchBW(SelectedNotch, tmp);
                }
                /*/

                if (m_bDraggingPanafallSplit)
                {
                    m_bDraggingPanafallSplit = false;
                    Display.ResetWaterfallBmp();
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.HISTOGRAM:
                    case DisplayMode.SPECTRUM:
                    case DisplayMode.PANASCOPE:
                    case DisplayMode.SPECTRASCOPE:
                        gridminmaxadjust = false;
                        gridmaxadjust = false;
                        rx1_grid_adjust = false;
                        rx2_grid_adjust = false;
                        tx1_grid_adjust = false;
                        tx2_grid_adjust = false;
                        break;
                }

                /*/
                if (SelectedNotch != null)
                {
                    Point p = new Point(e.X, e.Y);
                    m_frmNotchPopup.Left = picDisplay.PointToScreen(p).X - 16;
                    m_frmNotchPopup.Top = picDisplay.PointToScreen(p).Y - 16;
                    if (!m_frmNotchPopup.Visible)
                        m_frmNotchPopup.Show(
                            SelectedNotch, 0, 1000 );
            }
              /*/

            }
        }

        unsafe internal void picDisplayMouseMove(object sender, MouseEventArgs e)
        {
            Cursor next_cursor = null;
            var c = FrmMain.getConsole();
            try
            {
                // get filter location information
                int filt_low_x = 0;
                int filt_high_x = 0;
                if (rx2_enabled
                    && e.Y > picDisplay.Height
                            / 2) // if RX2 is enabled and the cursor is
                                 // in the lower half of the display
                {
                    if (mox) // && chkVFOBTX.Checked)
                    {
                        filt_low_x = HzToPixel(radio.GetDSPTX(0).TXFilterLow, 2);
                        filt_high_x = HzToPixel(radio.GetDSPTX(0).TXFilterHigh, 2);
                    }
                    else
                    {
                        filt_low_x = HzToPixel(radio.GetDSPRX(1, 0).RXFilterLow, 2);
                        filt_high_x
                            = HzToPixel(radio.GetDSPRX(1, 0).RXFilterHigh, 2);
                    }
                }
                else
                {
                    if (mox) // && chkVFOATX.Checked)
                    {
                        filt_low_x = HzToPixel(radio.GetDSPTX(0).TXFilterLow);
                        filt_high_x = HzToPixel(radio.GetDSPTX(0).TXFilterHigh);
                    }
                    else
                    {
                        filt_low_x = HzToPixel(radio.GetDSPRX(0, 0).RXFilterLow);
                        filt_high_x = HzToPixel(radio.GetDSPRX(0, 0).RXFilterHigh);
                    }
                }

                // get VFO A Sub + Filter location information
                int vfoa_sub_x = 0;
                int vfoa_sub_low_x = 0;
                int vfoa_sub_high_x = 0;
                if (chkEnableMultiRX && !mox)
                {
                    if (!rx2_enabled)
                    {
                        vfoa_sub_x = HzToPixel((float)((VFOBFreq - VFOAFreq) * 1e6));
                        vfoa_sub_low_x = vfoa_sub_x
                            + (HzToPixel(radio.GetDSPRX(0, 0).RXFilterLow)
                                - HzToPixel(0.0f));
                        vfoa_sub_high_x = vfoa_sub_x
                            + (HzToPixel(radio.GetDSPRX(0, 0).RXFilterHigh)
                                - HzToPixel(0.0f));
                    }
                    else
                    {
                        vfoa_sub_x
                            = HzToPixel((float)((VFOASubFreq - VFOAFreq) * 1e6));
                        vfoa_sub_low_x = vfoa_sub_x
                            + (HzToPixel(radio.GetDSPRX(0, 1).RXFilterLow)
                                - HzToPixel(0.0f));
                        vfoa_sub_high_x = vfoa_sub_x
                            + (HzToPixel(radio.GetDSPRX(0, 1).RXFilterHigh)
                                - HzToPixel(0.0f));
                    }
                }

                // get VFO B filter location information
                int vfob_x = 0;
                int vfob_low_x = 0;
                int vfob_high_x = 0;
                if (rx2_enabled && rx2_spectrum_drag)
                {
                    vfob_x = HzToPixel((float)((VFOBFreq - VFOAFreq) * 1e6));
                    vfob_low_x = vfob_x
                        + (HzToPixel(radio.GetDSPRX(1, 0).RXFilterLow)
                            - HzToPixel(0.0f));
                    vfob_high_x = vfob_x
                        + (HzToPixel(radio.GetDSPRX(1, 0).RXFilterHigh)
                            - HzToPixel(0.0f));
                }

                rx1_grid_adjust = false;
                rx2_grid_adjust = false;

                bool bOverRX1 = overRX(e.X, e.Y, 1, true);
                bool bOverRX2 = overRX(e.X, e.Y, 2, true);

                bool bHighlightNumberScaleRX1 = false;
                bool bHighlightNumberScaleRX2 = false;
                if (bOverRX1)
                {
                    switch (Display.CurrentDisplayMode)
                    {
                        case DisplayMode.PANADAPTER:
                        case DisplayMode.SPECTRUM:
                        case DisplayMode.HISTOGRAM:
                        case DisplayMode.PANAFALL:
                        case DisplayMode.PANASCOPE:
                        case DisplayMode.SPECTRASCOPE:
                            // check if we are over scale on left
                            if (e.X > RX1display_grid_x && e.X < RX1display_grid_w)
                            {
                                if (gridminmaxadjust || gridmaxadjust)
                                    Cursor.Current = grabbing;
                                else
                                    Cursor.Current = grab;
                                rx1_grid_adjust = true;
                                bHighlightNumberScaleRX1 = true;
                            }
                            break;
                    }
                }

                if (rx2_enabled && bOverRX2)
                {
                    switch (Display.CurrentDisplayModeBottom)
                    {
                        case DisplayMode.PANADAPTER:
                        case DisplayMode.SPECTRUM:
                        case DisplayMode.HISTOGRAM:
                        case DisplayMode.PANAFALL:
                        case DisplayMode.PANASCOPE:
                        case DisplayMode.SPECTRASCOPE:
                            // check if we are over scale on left
                            if (e.X > RX2display_grid_x && e.X < RX2display_grid_w)
                            {
                                if (gridminmaxadjust || gridmaxadjust)
                                    Cursor.Current = grabbing;
                                else
                                    Cursor.Current = grab;
                                rx2_grid_adjust = true;
                                bHighlightNumberScaleRX2 = true;
                            }
                            break;
                    }
                }
                Display.HighlightNumberScaleRX1 = bHighlightNumberScaleRX1; // MW0LGE
                Display.HighlightNumberScaleRX2 = bHighlightNumberScaleRX2;

                // MIDDLE OF PANAFALL MOVEUPDOWN MW0LGE
                if (!rx2_enabled
                    && Display.CurrentDisplayMode == DisplayMode.PANAFALL)
                {
                    if (e.Y >= Display.PanafallSplitBarPos
                        && e.Y < Display.PanafallSplitBarPos + 20)
                        Cursor.Current = Cursors.SizeNS;

                    if (m_bDraggingPanafallSplit)
                    {
                        float f = (float)e.Y / (float)picDisplay.Height;
                        f = Math.Max(0.1f, f);
                        f = Math.Min(0.9f, f);
                        Display.PanafallSplitBarPerc = f;
                    }
                }
                // END SPLITTER DRAG

                if (rx1_grid_adjust || rx2_grid_adjust)
                {
                    if (rx1_grid_adjust)
                    {
                        if (gridminmaxadjust)
                        {
                            int delta_y = e.Y - grid_minmax_drag_start_point.Y;
                            double delta_db = (delta_y / 10) * 5;
                            float val = grid_minmax_max_y;
                            val += (float)delta_db;
                            float min_val = grid_minmax_min_y;
                            min_val += (float)delta_db;

                            if (min_val < -200)
                            {
                                min_val = -200;
                                if (val - min_val < 24) val = min_val + 24;
                            }

                            if (val > 200)
                            {
                                val = 200;
                                if (val - min_val < 24) min_val = val - 24;
                            }

                            if (!tx1_grid_adjust)
                            {
                                DisplayGridMax = val;
                                DisplayGridMin = min_val;

                                // MW0LGE
                                if (m_bWaterfallUseRX1SpectrumMinMax)
                                {
                                    // use display directly so we dont change any
                                    // band based thresholds in setupform
                                    Display.WaterfallHighThreshold = val;
                                    Display.WaterfallLowThreshold = min_val;
                                }
                            }
                            else
                            {
                                TXGridMax = val;
                                TXGridMin = min_val;
                            }
                        }

                        if (gridmaxadjust)
                        {
                            int delta_y = e.Y - grid_minmax_drag_start_point.Y;
                            double delta_db = (delta_y / 10) * 5;
                            float val = grid_minmax_max_y;
                            val += (float)delta_db;

                            if (!tx1_grid_adjust)
                            {
                                if (val - DisplayGridMin < 24)
                                    val = DisplayGridMin + 24;

                                DisplayGridMax = val;

                                // MW0LGE
                                if (m_bWaterfallUseRX1SpectrumMinMax)
                                {
                                    // use display directly so we dont change any
                                    // band based thresholds in setupform
                                    Display.WaterfallHighThreshold = val;
                                }
                            }
                            else
                            {
                                if (val - TXGridMin < 24)
                                    val = TXGridMin + 24;

                                TXGridMax = val;
                            }
                        }
                    }
                    else if (rx2_grid_adjust)
                    {
                        if (gridminmaxadjust)
                        {
                            int delta_y = e.Y - grid_minmax_drag_start_point.Y;
                            double delta_db = (delta_y / 10) * 5;
                            float val = grid_minmax_max_y;
                            val += (float)delta_db;
                            float min_val = grid_minmax_min_y;
                            min_val += (float)delta_db;

                            if (min_val < -200)
                            {
                                min_val = -200;
                                if (val - min_val < 24) val = min_val + 24;
                            }

                            if (val > 200)
                            {
                                val = 200;
                                if (val - min_val < 24) min_val = val - 24;
                            }

                            if (!tx2_grid_adjust)
                            {
                                RX2DisplayGridMax = val;
                                RX2DisplayGridMin = min_val;

                                // MW0LGE
                                if (m_bWaterfallUseRX2SpectrumMinMax)
                                {
                                    // use display directly so we dont change any
                                    // band based thresholds in setupform
                                    Display.RX2WaterfallHighThreshold = val;
                                    Display.RX2WaterfallLowThreshold = min_val;
                                }
                            }
                            else
                            {
                                TXGridMax = val;
                                TXGridMin = min_val;
                            }
                        }
                        if (gridmaxadjust)
                        {
                            int delta_y = e.Y - grid_minmax_drag_start_point.Y;
                            double delta_db = (delta_y / 10) * 5;
                            float val = grid_minmax_max_y;
                            val += (float)delta_db;

                            if (!tx2_grid_adjust)
                            {
                                if (val - RX2DisplayGridMin < 24)
                                    val = RX2DisplayGridMin + 24;

                                RX2DisplayGridMax = val;

                                // MW0LGE
                                if (m_bWaterfallUseRX2SpectrumMinMax)
                                {
                                    // use display directly so we dont change any
                                    // band based thresholds in setupform
                                    Display.RX2WaterfallHighThreshold = val;
                                }
                            }
                            else
                            {
                                if (val - TXGridMin < 24)
                                    val = TXGridMin + 24;

                                TXGridMax = val;
                            }
                        }
                    }
                }

                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.HISTOGRAM:
                    case DisplayMode.SPECTRUM:
                        DisplayCursorX = e.X;
                        DisplayCursorY = e.Y;
                        float x = PixelToHz(e.X);
                        float y = PixelToDb(e.Y);
                        double rf_freq;
                        if (rx2_enabled && e.Y > picDisplay.Height / 2)
                            rf_freq = VFOBFreq + (double)x * 0.0000010;
                        else
                            rf_freq = VFOAFreq + (double)x * 0.0000010;

                        if (rx1_dsp_mode == DSPMode.CWL)
                            rf_freq += (double)cw_pitch * 0.0000010;
                        else if (rx1_dsp_mode == DSPMode.CWU)
                            rf_freq -= (double)cw_pitch * 0.0000010;
                        console.lblDisplayCursorOffset = x.ToString("f1") + "Hz";
                        console.lblDisplayCursorPower = y.ToString("f1") + "dBm";

                        string temp_text = rf_freq.ToString("f6") + " MHz";
                        int jper = temp_text.IndexOf(Common.separator) + 4;
                        console.lblDisplayCursorFreq
                            = String.Copy(temp_text.Insert(jper, " "));
                        break;
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANASCOPE:
                        DisplayCursorX = e.X; // update display cursor position
                        DisplayCursorY = e.Y;
                        x = PixelToHz(e.X);
                        switch (Display.CurrentDisplayMode)
                        {
                            case DisplayMode.PANAFALL:
                            case DisplayMode.PANASCOPE:
                            case DisplayMode.PANADAPTER:
                                y = PixelToDb(e.Y);
                                lblDisplayCursorPower
                                    = y.ToString("f1") + "dBm";

                                float cal_offset = 0.0f;
                                if (rx2_enabled && e.Y > picDisplay.Height / 2)
                                {
                                    switch (RX2AGCMode)
                                    {
                                        case AGCMode.FIXD: cal_offset = 0.0f; break;
                                        default:
                                            cal_offset = 2.0f
                                                + (Display.RX1DisplayCalOffset
                                                    + (Display.RX1PreampOffset
                                                        - Display.AlexPreampOffset)
                                                    - Display.RX2FFTSizeOffset);
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (RX1AGCMode)
                                    {
                                        case AGCMode.FIXD: cal_offset = 0.0f; break;
                                        default:
                                            cal_offset = 2.0f
                                                + (Display.RX1DisplayCalOffset
                                                    + (Display.RX1PreampOffset
                                                        - Display.AlexPreampOffset)
                                                    - Display.RX1FFTSizeOffset);
                                            break;
                                    }
                                }

                                if (!mox)
                                {
                                    if (show_agc)
                                    {
                                        if (rx2_enabled
                                            && e.Y > picDisplay.Height / 2)
                                        {
                                            if (Display.AGCRX2Knee.Contains(
                                                    e.X, e.Y))
                                            {
                                                if (agc_knee_drag)
                                                    Cursor.Current = grabbing;
                                                else
                                                    Cursor.Current = grab;
                                            }
                                            if (Display.AGCRX2Hang.Contains(
                                                    e.X, e.Y))
                                            {
                                                if (agc_hang_drag)
                                                    Cursor.Current = grabbing;
                                                else
                                                    Cursor.Current = grab;
                                            }
                                        }
                                        else
                                        {
                                            if (Display.AGCKnee.Contains(e.X, e.Y))
                                            {
                                                if (agc_knee_drag)
                                                    Cursor.Current = grabbing;
                                                else
                                                    Cursor.Current = grab;
                                            }
                                            if (Display.AGCHang.Contains(e.X, e.Y))
                                            {
                                                if (agc_hang_drag)
                                                    Cursor.Current = grabbing;
                                                else
                                                    Cursor.Current = grab;
                                            }
                                        }
                                    }
                                }

                                if (agc_knee_drag && show_agc)
                                {
                                    if (rx2_enabled && e.Y > picDisplay.Height / 2)
                                    {
                                        double agc_rx2_thresh_point
                                            = (double)PixelToRx2Db(e.Y + 4);
                                        agc_rx2_thresh_point -= (double)cal_offset;
                                        if (agc_rx2_thresh_point > 2)
                                            agc_rx2_thresh_point = 2;
                                        if (agc_rx2_thresh_point < -143.0)
                                            agc_rx2_thresh_point = -143.0;
                                        // Debug.WriteLine("agc_db_point2: " +
                                        // agc_db_point);

                                        double agc_rx2_top = 0.0;
                                        // DttSP.SetRXAGCThresh(2, 0,
                                        // agc_rx2_thresh_point);
                                        WDSP.SetRXAAGCThresh(WDSP.id(2, 0),
                                            agc_rx2_thresh_point, 4096.0,
                                            sample_rate_rx1);
                                        // DttSP.GetRXAGCMaxGain(2, 0,
                                        // &agc_rx2_top);
                                        WDSP.GetRXAAGCTop(
                                            WDSP.id(2, 0), &agc_rx2_top);

                                        agc_rx2_top = Math.Round(agc_rx2_top);

                                        // DttSP.SetRXAGCThresh(1, 0,
                                        // agc_rx2_thresh_point);
                                        // DttSP.SetRXAGCThresh(0, 0,
                                        // agc_rx2_thresh_point); txtOverload.Text =
                                        // agc_top.ToString("f3") + " " +
                                        // agc_thresh_point.ToString("f3");
                                        switch (RX2AGCMode)
                                        {
                                            case AGCMode.FIXD:
                                                if (agc_rx2_top > 120)
                                                    agc_rx2_top = 120;
                                                if (agc_rx2_top < -20.0)
                                                    agc_rx2_top = -20.0;

                                                if (true)
                                                    AGCRX2FixedGain = (int)
                                                        agc_rx2_top; // agc_top;
                                                                     // Debug.WriteLine("agc_db_point3:
                                                                     // "
                                                                     // +
                                                                     // agc_db_point);
                                                break;
                                            default:
                                                if (agc_rx2_top > 120)
                                                    agc_rx2_top = 120;
                                                if (agc_rx2_top < -20.0)
                                                    agc_rx2_top = -20.0;

                                                if (true)
                                                    AGCRX2MaxGain
                                                        = (int)agc_rx2_top;
                                                // DttSP.SetRXAGCMaxGain(1, 0,
                                                // agc_rx2_top);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        double agc_thresh_point
                                            = (double)PixelToDb(e.Y + 4);
                                        agc_thresh_point -= (double)
                                            cal_offset; // (double)Display.RX1PreampOffset;
                                        if (agc_thresh_point > 2)
                                            agc_thresh_point = 2;
                                        if (agc_thresh_point < -143.0)
                                            agc_thresh_point = -143.0;
                                        // Debug.WriteLine("agc_db_point2: " +
                                        // agc_db_point);

                                        double agc_top = 0.0;
                                        // DttSP.SetRXAGCThresh(0, 0,
                                        // agc_thresh_point);
                                        WDSP.SetRXAAGCThresh(WDSP.id(0, 0),
                                            agc_thresh_point, 4096.0,
                                            sample_rate_rx1);
                                        // DttSP.GetRXAGCMaxGain(0, 0, &agc_top);
                                        WDSP.GetRXAAGCTop(WDSP.id(0, 0), &agc_top);
                                        agc_top = Math.Round(agc_top);

                                        // DttSP.SetRXAGCThresh(0, 0,
                                        // agc_thresh_point); txtOverload.Text =
                                        // agc_top.ToString("f3") + " " +
                                        // agc_thresh_point.ToString("f3");
                                        switch (RX1AGCMode)
                                        {
                                            case AGCMode.FIXD:
                                                if (agc_top > 120) agc_top = 120;
                                                if (agc_top < -20.0) agc_top = -20.0;

                                                if (true)
                                                    AGCFixedGain = (int)
                                                        agc_top; // agc_top;
                                                                 // Debug.WriteLine("agc_db_point3:
                                                                 // "
                                                                 // + agc_db_point);
                                                break;
                                            default:
                                                if (agc_top > 120) agc_top = 120;
                                                if (agc_top < -20.0) agc_top = -20.0;

                                                if (true)
                                                    AGCMaxGain
                                                        = (int)agc_top;
                                                break;
                                        }
                                    }
                                }

                                if (agc_hang_drag && show_agc)
                                {
                                    if (rx2_enabled && e.Y > picDisplay.Height / 2)
                                    {
                                        double agc_hang_point
                                            = (double)PixelToRx2Db(e.Y + 4);
                                        agc_hang_point -= (double)
                                            cal_offset; // (double)Display.RX1PreampOffset;
                                                        // agc_hang_point += 120;
                                                        // Debug.WriteLine("agc_db_point1:
                                                        // " + agc_db_point);
                                        if (agc_hang_point > 4.0)
                                            agc_hang_point = 4.0;
                                        if (agc_hang_point < -121.0)
                                            agc_hang_point = -121.0;
                                        // Debug.WriteLine("agc_db_point2: " +
                                        // agc_db_point);

                                        // SetupForm.AGCMaxGain = agc_db_point -
                                        // agc_slope;
                                        int hang_threshold = 0;
                                        // DttSP.SetRXAGCHangLevel(2, 0,
                                        // agc_hang_point);
                                        WDSP.SetRXAAGCHangLevel(
                                            WDSP.id(2, 0), agc_hang_point);
                                        // DttSP.GetRXAGCHangThreshold(2, 0,
                                        // &hang_threshold);
                                        WDSP.GetRXAAGCHangThreshold(
                                            WDSP.id(2, 0), &hang_threshold);
                                        if (hang_threshold > 100)
                                        {
                                            hang_threshold = 100;
                                            // agc_hang_point = 0.0;
                                        }
                                        if (hang_threshold < 0) hang_threshold = 0;

                                        // if (!IsSetupNull)
                                        // SetupForm.AGCHangThreshold =
                                        // agc_hang_point;
                                        // DttSP.SetRXAGCHangLevel(1, 0,
                                        // agc_hang_point);
                                        // Debug.WriteLine("agc_hang_point: " +
                                        // agc_hang_point);
                                        // Debug.WriteLine("hang_threshold: " +
                                        // hang_threshold);
                                        if (true)
                                            SetAGCRX2HangThreshold
                                                = hang_threshold;
                                    }
                                    else
                                    {
                                        // int agc_hang_point = (int)PixelToDb(e.Y);
                                        double agc_hang_point
                                            = (double)PixelToDb(e.Y + 4);
                                        agc_hang_point -= (double)
                                            cal_offset; // (double)Display.RX1PreampOffset;
                                                        // agc_hang_point += 120;
                                                        // int agc_slope =
                                                        // radio.GetDSPRX(0,
                                                        // 0).RXAGCSlope / 10;
                                                        // Debug.WriteLine("agc_db_point1:
                                                        // " + agc_db_point);
                                        if (agc_hang_point > 4.0)
                                            agc_hang_point = 4.0;
                                        if (agc_hang_point < -121.0)
                                            agc_hang_point = -121.0;
                                        // Debug.WriteLine("agc_db_point2: " +
                                        // agc_db_point); SetupForm.AGCMaxGain =
                                        // agc_db_point - agc_slope;
                                        int hang_threshold = 0;
                                        // DttSP.SetRXAGCHangLevel(0, 0,
                                        // agc_hang_point);
                                        WDSP.SetRXAAGCHangLevel(
                                            WDSP.id(0, 0), agc_hang_point);
                                        // DttSP.GetRXAGCHangThreshold(0, 0,
                                        // &hang_threshold);
                                        WDSP.GetRXAAGCHangThreshold(
                                            WDSP.id(0, 0), &hang_threshold);
                                        if (hang_threshold > 100)
                                        {
                                            hang_threshold = 100;
                                            // agc_hang_point = 0.0;
                                        }
                                        if (hang_threshold < 0) hang_threshold = 0;

                                        // if (!IsSetupNull)
                                        // SetupForm.AGCHangThreshold =
                                        // agc_hang_point;
                                        // DttSP.SetRXAGCHangLevel(0, 0,
                                        // agc_hang_point);

                                        if (true)
                                            AGCHangThreshold
                                                = hang_threshold;
                                    }
                                }
                                break;
                            case DisplayMode.WATERFALL:
                                y = WaterfallPixelToTime(e.Y);
                                lblDisplayCursorPower
                                    = (y / 1000.0f).ToString("f1") + "sec";
                                break;
                        }
                        switch (Display.CurrentDisplayMode)
                        {
                            case DisplayMode.PANAFALL:
                                if (e.Y < Display.PanafallSplitBarPos /*picDisplay.Height / 2*/)
                                {
                                    y = PixelToDb(e.Y);
                                    lblDisplayCursorPower
                                        = y.ToString("f1") + "dBm";
                                }
                                else
                                {
                                    y = WaterfallPixelToTime(e.Y);
                                    lblDisplayCursorPower
                                        = (y / 1000.0f).ToString("f1") + "sec";
                                }
                                break;
                            case DisplayMode.PANASCOPE:
                                if (e.Y < picDisplay.Height / 2)
                                {
                                    y = PixelToDb(e.Y);
                                    lblDisplayCursorPower
                                        = y.ToString("f1") + "dBm";
                                }
                                else
                                {
                                    lblDisplayCursorPower = "";
                                }
                                break;
                        }

                        if (rx2_enabled
                            && e.Y > picDisplay.Height
                                    / 2) // if RX2 is enabled and the cursor is in
                                         // the lower half of the display
                        {

                            x = PixelToHz(e.X, 2);
                            rf_freq = VFOBFreq + (double)x * 0.0000010;
                            if (rx2_dsp_mode == DSPMode.CWL)
                                rf_freq += (double)cw_pitch * 0.0000010;
                            else if (rx2_dsp_mode == DSPMode.CWU)
                                rf_freq -= (double)cw_pitch * 0.0000010;
                            Display.FREQ = x;
                        }
                        else
                        {
                            x = PixelToHz(e.X, 1);
                            rf_freq = VFOAFreq + (double)x * 0.0000010;
                            if (rx1_dsp_mode == DSPMode.CWL)
                                rf_freq += (double)cw_pitch * 0.0000010;
                            else if (rx1_dsp_mode == DSPMode.CWU)
                                rf_freq -= (double)cw_pitch * 0.0000010;
                            Display.FREQ
                                = x; // PixelToHz(e.X); //for cross hair filter
                        }
                        double freq = double.Parse(txtVFOAFreqText);

                        lblDisplayCursorOffset = x.ToString("f1") + "Hz";

                        if (click_tune_display
                            && !mox) // Correct cursor frequency when CTUN on -G3OQD
                            temp_text = (rf_freq + (center_frequency - freq))
                                            .ToString("f6")
                                + " MHz"; // Disply cursor frequency under Spectrum
                                          // - G3OQD
                        else
                            temp_text = rf_freq.ToString("f6")
                                + " MHz"; // Disply cursor frequency under Spectrum

                        jper = temp_text.IndexOf(separator) + 4;
                        lblDisplayCursorFreq
                            = String.Copy(temp_text.Insert(jper, " "));

                        bool bDragRX1Filter = bOverRX1
                            && /*rx1_enabled &&*/ (rx1_dsp_mode != DSPMode.DRM)
                            && !click_tune_display; // MW0LGE CHECK
                        bool bDragRX2Filter = bOverRX2 && rx2_enabled
                            && (rx2_dsp_mode != DSPMode.DRM)
                            && !click_tune_rx2_display;

                        if (bDragRX1Filter || bDragRX2Filter)
                        {
                            if (/*!click_tune_display && !click_tune_rx2_display
                  &&*/
                                current_click_tune_mode == ClickTuneMode.Off
                                && picDisplay.Cursor != Cursors.Hand
                                && next_cursor != Cursors.SizeNS
                                && next_cursor != Cursors.VSplit)
                            {
                                if (Math.Abs(e.X - filt_low_x) < 3
                                    || // RX low filter edge
                                    Math.Abs(e.X - filt_high_x) < 3
                                    || // RX high filter edge

                                    rx1_high_filter_drag || rx1_low_filter_drag
                                    || // already dragging a filter edge

                                    (chkEnableMultiRX && // RX1 Sub
                                        ((rx2_enabled && e.Y < picDisplay.Height / 2)
                                            || !rx2_enabled)
                                        && (e.X > vfoa_sub_low_x - 3
                                            && e.X < vfoa_sub_high_x + 3))
                                    ||

                                    (rx2_enabled && e.Y > picDisplay.Height / 2
                                        && (Math.Abs(e.X - vfob_low_x) < 3))
                                    || // RX2 low filter edge
                                    (rx2_enabled && e.Y > picDisplay.Height / 2
                                        && (Math.Abs(e.X - vfob_high_x) < 3))
                                    ||

                                    rx2_high_filter_drag
                                    || rx2_low_filter_drag) // already dragging a
                                                            // filter edge
                                {
                                    next_cursor = Cursors.SizeWE;
                                }
                                else if (e.X > filt_low_x && e.X < filt_high_x)
                                {
                                    next_cursor = Cursors.NoMoveHoriz;
                                }
                                else
                                {
                                    next_cursor = Cursors.Cross;
                                }
                            }

                            if (rx1_high_filter_drag)
                            {
                                SelectRX1VarFilter();
                                int new_high = (int)Math.Max(PixelToHz(e.X),
                                    radio.GetDSPRX(0, 0).RXFilterLow + 10);
                                UpdateRX1Filters(
                                    radio.GetDSPRX(0, 0).RXFilterLow, new_high);
                            }
                            else if (rx1_low_filter_drag)
                            {
                                SelectRX1VarFilter();
                                int new_low = (int)Math.Min(PixelToHz(e.X),
                                    radio.GetDSPRX(0, 0).RXFilterHigh - 10);
                                UpdateRX1Filters(
                                    new_low, radio.GetDSPRX(0, 0).RXFilterHigh);
                            }
                            else if (rx1_whole_filter_drag)
                            {
                                SelectRX1VarFilter();
                                int diff = (int)(PixelToHz(e.X)
                                    - PixelToHz(whole_filter_start_x));
                                UpdateRX1Filters(whole_filter_start_low + diff,
                                    whole_filter_start_high + diff);
                            }
                            else if (rx1_sub_drag)
                            {
                                int diff = (int)(PixelToHz(e.X)
                                    - PixelToHz(sub_drag_last_x));
                                if (rx2_enabled)
                                    VFOASubFreq = sub_drag_start_freq + diff * 1e-6;
                                else
                                    VFOBFreq = sub_drag_start_freq + diff * 1e-6;
                            }
                            else if (rx2_high_filter_drag)
                            {
                                c.SelectRX2VarFilter();
                                int new_high = (int)Math.Max(PixelToHz(e.X, 2),
                                    radio.GetDSPRX(1, 0).RXFilterLow + 10);
                                c.UpdateRX2Filters(
                                     radio.GetDSPRX(1, 0).RXFilterLow, new_high);
                            }
                            else if (rx2_low_filter_drag)
                            {
                                c.SelectRX2VarFilter();
                                int new_low = (int)Math.Min(PixelToHz(e.X, 2),
                                    radio.GetDSPRX(1, 0).RXFilterHigh - 10);
                                c.UpdateRX2Filters(
                                    new_low, radio.GetDSPRX(1, 0).RXFilterHigh);
                            }
                            else if (rx2_whole_filter_drag)
                            {
                                c.SelectRX2VarFilter();
                                int diff = (int)(PixelToHz(e.X, 2)
                                    - PixelToHz(whole_filter_start_x, 2));
                                c.UpdateRX2Filters(whole_filter_start_low + diff,
                                    whole_filter_start_high + diff);
                            }
                            else if (tx_high_filter_drag)
                            {
                                int new_high = (int)Math.Max(
                                    Math.Abs(PixelToHz(e.X)), tx_filter_low + 10);
                                TXFilterHigh = new_high;
                            }
                            else if (tx_low_filter_drag)
                            {
                                int new_low = (int)(Math.Min(
                                    Math.Abs(PixelToHz(e.X)), tx_filter_high - 10));
                                TXFilterLow = new_low;
                            }
                            else if (tx_whole_filter_drag)
                            {
                                int diff = (int)(PixelToHz(e.X)
                                    - PixelToHz(whole_filter_start_x));
                                switch (rx1_dsp_mode)
                                {
                                    case DSPMode.LSB:
                                    case DSPMode.DIGL:
                                        TXFilterLow
                                            = whole_filter_start_low - diff;
                                        TXFilterHigh
                                            = whole_filter_start_high - diff;
                                        break;
                                    case DSPMode.USB:
                                    case DSPMode.DIGU:
                                        TXFilterLow
                                            = whole_filter_start_low + diff;
                                        TXFilterHigh
                                            = whole_filter_start_high + diff;
                                        break;
                                    case DSPMode.AM:
                                    case DSPMode.SAM:
                                    case DSPMode.FM:
                                    case DSPMode.DSB:
                                        TXFilterHigh
                                            = whole_filter_start_high + diff;
                                        break;
                                }
                            }
                        }
                        break;
                    default:
                        lblDisplayCursorOffset = "";
                        lblDisplayCursorPower = "";
                        lblDisplayCursorFreq = "";
                        break;
                }

                if (rx1_spectrum_tune_drag)
                {
                    if (!mox || (rx2_enabled && c.VFOBTXChecked))
                    {
                        float start_freq = PixelToHz(spectrum_drag_last_x);
                        float end_freq = PixelToHz(e.X);
                        spectrum_drag_last_x = e.X;
                        float delta = end_freq - start_freq;
                        c.CenterFrequency -= delta * 0.0000010;
                        //txtVFOAFreq_LostFocus(this, EventArgs.Empty);
                    }
                }

                if (rx2_spectrum_tune_drag)
                {
                    if (rx2_enabled && (!mox || c.VFOATX))
                    {
                        float start_freq = PixelToHz(spectrum_drag_last_x, 2);
                        float end_freq = PixelToHz(e.X, 2);
                        spectrum_drag_last_x = e.X;
                        float delta = end_freq - start_freq;
                        c.CenterRX2Frequency -= delta * 0.0000010;
                        //txtVFOBFreq_LostFocus(this, EventArgs.Empty);
                    }
                }

                if (rx1_spectrum_drag)
                {
                    if (!mox || (rx2_enabled && c.VFOBTX))
                    {
                        float start_freq = PixelToHz(spectrum_drag_last_x);
                        float end_freq = PixelToHz(e.X);
                        spectrum_drag_last_x = e.X;
                        float delta = end_freq - start_freq;
                        VFOAFreq -= delta * 0.0000010;
                    }
                }

                if (rx2_spectrum_drag)
                {
                    if (rx2_enabled && (!mox || c.VFOATX))
                    {
                        float start_freq = PixelToHz(spectrum_drag_last_x, 2);
                        float end_freq = PixelToHz(e.X, 2);
                        spectrum_drag_last_x = e.X;
                        float delta = end_freq - start_freq;
                        VFOBFreq -= delta * 0.0000010;
                    }
                }

                if (rx1_click_tune_drag)
                {
                    if (!mox || (rx2_enabled && c.VFOBTX))
                    {
                        float start_freq = PixelToHz(spectrum_drag_last_x);
                        float end_freq = PixelToHz(e.X);
                        spectrum_drag_last_x = e.X;
                        float delta = start_freq - end_freq;
                        VFOAFreq -= delta * 0.0000010;
                    }
                }

                if (rx2_click_tune_drag)
                {
                    if (rx2_enabled && (!mox || c.VFOATX))
                    {
                        float start_freq = PixelToHz(spectrum_drag_last_x, 2);
                        float end_freq = PixelToHz(e.X, 2);
                        spectrum_drag_last_x = e.X;
                        float delta = start_freq - end_freq;
                        VFOBFreq -= delta * 0.0000010;
                    }
                }

            }
            catch (Exception)
            {
            }

            if (next_cursor != null && picDisplay.Cursor != next_cursor)
                picDisplay.Cursor = next_cursor;
        }

        unsafe internal void RunDisplay()
        {

            Display.dx_running = true;
            try
            {
                Stopwatch objStopWatch = new Stopwatch();
                objStopWatch.Start();
                double fFractionOfMs = 0;
                double fThreadSleepLate = 0;


                while (!Display.shut_dx2) //(chkPower.Checked)
                {
                    objStopWatch.Reset();
                    objStopWatch.Start();

                    uint top_thread = 0;
                    uint bottom_thread = 2;
                    int flag;
                    bool bLocalMox = mox;

                    if (bLocalMox)
                    {
                        if (VFOATX || !RX2Enabled)
                            top_thread = 1;
                        else if (chkVFOBTX && chkRX2)
                            bottom_thread = 1;
                    }

                    if ((!Display.DataReady || !Display.WaterfallDataReady)
                        || (chkSplitDisplay
                            && (!Display.DataReadyBottom
                                || !Display.WaterfallDataReadyBottom)))
                    {

                        if (calibration_running)
                        {
                            calibration_mutex.WaitOne();
                            displaydidit = true;
                        }

                        if (!pause_DisplayThread
                            && (!Display.DataReady || !Display.WaterfallDataReady)
                            && Display.CurrentDisplayMode != DisplayMode.OFF)
                        {
                            flag = 0;
                            switch (Display.CurrentDisplayMode)
                            {
                                case DisplayMode.WATERFALL:
                                case DisplayMode.PANAFALL:
                                    if (bLocalMox && !display_duplex)
                                    {
                                        if (chkVFOATX || !chkRX2)
                                        {
                                            fixed (float* ptr
                                                = &Display.new_display_data[0])
                                                SpecHPSDRDLL.GetPixels(
                                                    cmaster.inid(1, 0), 0, ptr,
                                                    ref flag);
                                            Display.DataReady = (flag == 1);
                                            fixed (float* ptr
                                                = &Display.new_waterfall_data[0])
                                                SpecHPSDRDLL.GetPixels(
                                                    cmaster.inid(1, 0), 1, ptr,
                                                    ref flag);
                                            Display.WaterfallDataReady = (flag == 1);
                                        }
                                        else
                                        {
                                            fixed (float* ptr
                                                = &Display.new_display_data[0])
                                                // SpecHPSDRDLL.GetPixels(cmaster.inid(1,
                                                // 0), 0, ptr, ref flag);
                                                SpecHPSDRDLL.GetPixels(
                                                    0, 0, ptr, ref flag);
                                            Display.DataReady = (flag == 1);
                                            fixed (float* ptr
                                                = &Display.new_waterfall_data[0])
                                                // SpecHPSDRDLL.GetPixels(cmaster.inid(1,
                                                // 0), 1, ptr, ref flag);
                                                SpecHPSDRDLL.GetPixels(
                                                    0, 1, ptr, ref flag);
                                            Display.WaterfallDataReady = (flag == 1);
                                        }
                                    }
                                    else // rx
                                    {
                                        fixed (float* ptr
                                            = &Display.new_display_data[0])
                                            SpecHPSDRDLL.GetPixels(
                                                0, 0, ptr, ref flag);
                                        Display.DataReady = (flag == 1);
                                        fixed (float* ptr
                                            = &Display.new_waterfall_data[0])
                                            SpecHPSDRDLL.GetPixels(
                                                0, 1, ptr, ref flag);
                                        Debug.Assert(flag == 0); // we have some data, finallly
                                        Display.WaterfallDataReady = (flag == 1);
                                    }
                                    break;
                                case DisplayMode.SPECTRUM:
                                case DisplayMode.HISTOGRAM:
                                case DisplayMode.SPECTRASCOPE:
                                case DisplayMode.PANADAPTER:
                                case DisplayMode.PANASCOPE:
                                    if (bLocalMox && !display_duplex)
                                    {
                                        if (chkVFOATX || !chkRX2)
                                        {
                                            fixed (float* ptr
                                                = &Display.new_display_data[0])
                                                SpecHPSDRDLL.GetPixels(
                                                    cmaster.inid(1, 0), 0, ptr,
                                                    ref flag);
                                            Display.DataReady = (flag == 1);
                                        }
                                        else
                                        {
                                            fixed (float* ptr
                                                = &Display.new_display_data[0])
                                                SpecHPSDRDLL.GetPixels(
                                                    0, 0, ptr, ref flag);
                                            Display.DataReady = (flag == 1);
                                        }
                                    }
                                    else
                                    {
                                        fixed (float* ptr
                                            = &Display.new_display_data[0])
                                            SpecHPSDRDLL.GetPixels(
                                                0, 0, ptr, ref flag);
                                        Display.DataReady = (flag == 1);
                                    }
                                    break;
                                case DisplayMode.SCOPE:
                                case DisplayMode.SCOPE2:
                                    fixed (float* ptr = &Display.new_display_data[0])
                                    // DttSP.GetScope(top_thread, ptr,
                                    // (int)(scope_time * 48));
                                    {
                                        if (top_thread != 1)
                                            WDSP.RXAGetaSipF(WDSP.id(top_thread, 0),
                                                ptr, (int)(scope_time * 48));
                                        else
                                            WDSP.TXAGetaSipF(WDSP.id(top_thread, 0),
                                                ptr, (int)(scope_time * 48));
                                    }
                                    Display.DataReady = true;
                                    break;
                                case DisplayMode.PHASE:
                                    fixed (float* ptr = &Display.new_display_data[0])
                                    // DttSP.GetPhase(top_thread, ptr,
                                    // Display.PhaseNumPts);
                                    {
                                        if (top_thread != 1)
                                            WDSP.RXAGetaSipF1(WDSP.id(top_thread, 0),
                                                ptr, Display.PhaseNumPts);
                                        else
                                            WDSP.TXAGetaSipF1(WDSP.id(top_thread, 0),
                                                ptr, Display.PhaseNumPts);
                                    }
                                    Display.DataReady = true;
                                    break;
                                case DisplayMode.PHASE2:
                                    if (Audio.phase_buf_l != null
                                        && Audio.phase_buf_r
                                            != null) // MW0LGE would be null if
                                                     // audio not running (ie not
                                                     // connected?)
                                    {
                                        // Audio.phase_mutex.WaitOne();
                                        for (int i = 0; i < Display.PhaseNumPts;
                                             i++)
                                        {
                                            Display.new_display_data[i * 2]
                                                = Audio.phase_buf_l[i];
                                            Display.new_display_data[i * 2 + 1]
                                                = Audio.phase_buf_r[i];
                                        }
                                        Display.DataReady = true;
                                        // Audio.phase_mutex.ReleaseMutex();
                                    }
                                    break;
                            }
                        }

                        if (!pause_DisplayThread && chkSplitDisplay
                            && (!Display.DataReadyBottom
                                || !Display.WaterfallDataReadyBottom)
                            && Display.CurrentDisplayModeBottom != DisplayMode.OFF)
                        {
                            flag = 0;
                            switch (Display.CurrentDisplayModeBottom)
                            {
                                case DisplayMode.SPECTRUM:
                                case DisplayMode.HISTOGRAM: break;
                                case DisplayMode.WATERFALL:
                                    if (bLocalMox && VFOBTX)
                                    {
                                        fixed (float* ptr
                                            = &Display.new_waterfall_data_bottom[0])
                                            SpecHPSDRDLL.GetPixels(
                                                cmaster.inid(1, 0), 1, ptr,
                                                ref flag);
                                    }
                                    else
                                    {
                                        fixed (float* ptr
                                            = &Display.new_waterfall_data_bottom[0])
                                            SpecHPSDRDLL.GetPixels(
                                                1, 1, ptr, ref flag);
                                    }
                                    Display.WaterfallDataReadyBottom = (flag == 1);
                                    break;
                                case DisplayMode.PANADAPTER:
                                    if (bLocalMox && VFOBTX)
                                    {

                                        fixed (float* ptr
                                            = &Display.new_display_data_bottom[0])
                                            SpecHPSDRDLL.GetPixels(
                                                cmaster.inid(1, 0), 0, ptr,
                                                ref flag);
                                    }
                                    else
                                    {
                                        fixed (float* ptr
                                            = &Display.new_display_data_bottom[0])
                                            SpecHPSDRDLL.GetPixels(
                                                1, 0, ptr, ref flag);
                                    }
                                    Display.DataReadyBottom = (flag == 1);
                                    break;
                                case DisplayMode.PANAFALL: // MW0LGE
                                    if (bLocalMox && VFOBTX)
                                    {
                                        fixed (float* ptr
                                            = &Display.new_display_data_bottom[0])
                                            SpecHPSDRDLL.GetPixels(
                                                cmaster.inid(1, 0), 0, ptr,
                                                ref flag);
                                        Display.DataReadyBottom = (flag == 1);
                                        fixed (float* ptr
                                            = &Display.new_waterfall_data_bottom[0])
                                            SpecHPSDRDLL.GetPixels(
                                                cmaster.inid(1, 0), 1, ptr,
                                                ref flag);
                                        Display.WaterfallDataReadyBottom
                                            = (flag == 1);
                                    }
                                    else
                                    {
                                        fixed (float* ptr
                                            = &Display.new_display_data_bottom[0])
                                            SpecHPSDRDLL.GetPixels(
                                                1, 0, ptr, ref flag);
                                        Display.DataReadyBottom = (flag == 1);
                                        fixed (float* ptr
                                            = &Display.new_waterfall_data_bottom[0])
                                            SpecHPSDRDLL.GetPixels(
                                                1, 1, ptr, ref flag);
                                        Display.WaterfallDataReadyBottom
                                            = (flag == 1);
                                    }
                                    break;
                                case DisplayMode.SCOPE:
                                case DisplayMode.SCOPE2:
                                    fixed (float* ptr
                                        = &Display.new_display_data_bottom[0])
                                    // DttSP.GetScope(bottom_thread, ptr,
                                    // (int)(scope_time * 48));
                                    {
                                        if (bottom_thread != 1)
                                            WDSP.RXAGetaSipF(
                                                WDSP.id(bottom_thread, 0), ptr,
                                                (int)(scope_time * 48));
                                        else
                                            WDSP.TXAGetaSipF(
                                                WDSP.id(bottom_thread, 0), ptr,
                                                (int)(scope_time * 48));
                                    }
                                    Display.DataReadyBottom = true;
                                    break;
                                case DisplayMode.PHASE:
                                    fixed (float* ptr
                                        = &Display.new_display_data_bottom[0])
                                    // DttSP.GetPhase(bottom_thread, ptr,
                                    // Display.PhaseNumPts);
                                    {
                                        if (bottom_thread != 1)
                                            WDSP.RXAGetaSipF1(
                                                WDSP.id(bottom_thread, 0), ptr,
                                                Display.PhaseNumPts);
                                        else
                                            WDSP.TXAGetaSipF1(
                                                WDSP.id(bottom_thread, 0), ptr,
                                                Display.PhaseNumPts);
                                    }
                                    Display.DataReadyBottom = true;
                                    break;
                                case DisplayMode.PHASE2:
                                    if (Audio.phase_buf_l != null
                                        && Audio.phase_buf_r
                                            != null) // MW0LGE would be null if
                                                     // audio not running (ie not
                                                     // connected?)
                                    {
                                        // Audio.phase_mutex.WaitOne();
                                        for (int i = 0; i < Display.PhaseNumPts;
                                             i++)
                                        {
                                            Display.new_display_data_bottom[i * 2]
                                                = Audio.phase_buf_l[i];
                                            Display
                                                .new_display_data_bottom[i * 2 + 1]
                                                = Audio.phase_buf_r[i];
                                        }
                                        // Audio.phase_mutex.ReleaseMutex();
                                        Display.DataReadyBottom = true;
                                    }
                                    break;
                            }
                        }
                        if (displaydidit)
                        {
                            displaydidit = false;
                            calibration_mutex.ReleaseMutex();
                        }
                    }

                    // MW0LGE - note, if both displays are off, then the refresh to
                    // hide the final one will not happen. This is now forced in
                    // combo display mode to force a refresh
                    if (!pause_DisplayThread
                        && ((Display.CurrentDisplayMode != DisplayMode.OFF)
                            || (RX2Enabled
                                && Display.CurrentDisplayModeBottom
                                    != DisplayMode.OFF)))
                    {
                        switch (current_display_engine)
                        {
                            case DisplayEngine.GDI_PLUS:
                                {
                                    if (picDisplay.InvokeRequired)
                                    {
                                        RefreshControl(picDisplay);
                                    }
                                    else
                                    {
                                        picDisplay.Refresh();
                                    }
                                }
                                break;
                            case DisplayEngine.DIRECT_X: Display.RenderDX2D(); break;
                        }
                    }

                    MakeFrameRateAccurate(ref objStopWatch, ref fFractionOfMs, ref fThreadSleepLate);
                } // (while !Display.shut_dx2)
            }
            catch (Exception e)
            {
                Display.dx_running = false;
                // MessageBox.Show("Error in RunDisplay.\n" + e.Message);
                Common.LogException(e, false, "Error in RunDisplay should be fatal.");
            }

            Display.dx_running = false;
        }


        unsafe void MakeFrameRateAccurate(ref Stopwatch objStopWatch, ref double fFractionOfMs, ref double fThreadSleepLate)
        {

            double dly = DisplayDelay - objStopWatch.ElapsedMilliseconds
                         - fThreadSleepLate;

            if (dly < 0)
            {
                dly = 1;
            }

            if (m_bUseAccurateFrameTiming)
            {
                // wait for the calculated delay
                objStopWatch.Reset();
                objStopWatch.Start();
                int slept = 0;
                while (objStopWatch.ElapsedMilliseconds < dly)
                {
                    Thread.Sleep(1);  // hmmm
                    slept++;
                }
                fThreadSleepLate = objStopWatch.ElapsedMilliseconds - dly;
            }
            else
            {
                // accumulate the fractional delay
                fFractionOfMs += dly - (int)dly;
                int nIntegerPart = (int)fFractionOfMs;
                fFractionOfMs -= nIntegerPart;

                int nWantToWait = (int)dly + nIntegerPart;

                // time how long we actually sleep for, and use this
                // difference to lower dly time next time around
                objStopWatch.Reset();
                objStopWatch.Start();
                Thread.Sleep(
                    nWantToWait); // not guaranteed to be the delay we want
                fThreadSleepLate = objStopWatch.ElapsedMilliseconds - nWantToWait;
            }
        }



        public delegate void RefreshControlDelegate(Control control);  // defines a delegate type

        public void RefreshControl(Control control)
        {
            if (control.InvokeRequired)
            {
                // Invoke makes the calling thread wait, and I think this is what we want.
                control.Invoke(new RefreshControlDelegate(RefreshControl), new object[] { control });  // invoking itself
            }
            else
            {
                control.Refresh();      // the "functional part", executing only on the main thread
            }
        }

        private void UpdateRX1Filters(int rXFilterLow, int new_high)
        {
            throw new NotImplementedException();
        }

        private void SelectRX1VarFilter()
        {
            throw new NotImplementedException();
        }
    }
}
