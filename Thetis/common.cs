//=================================================================
// common.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2012  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: gpl@flexradio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    4616 W. Howard Lane  Suite 1-150
//    Austin, TX 78728
//    USA
//=================================================================

using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.IO.Ports;
using System.IO;
using System.Reflection;
using SharpDX.Direct3D11;
using CoolSDR.Forms;
using System.Globalization;
using CoolSDR.Class;
using System.Runtime.InteropServices;
using CoolSDR;
using System.Collections.Concurrent;
using System.Threading;

namespace Thetis
{
    using ConsoleType = FrmMain;
    public class Common
    {
        public static void ExitApp(string why, int exitCode)
        {

            Program.ThrowAndExit(why, exitCode);
        }

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        static extern void GetSystemTimePreciseAsFileTime(out long filetime);
        public static DateTimeOffset Now()
        {
            GetSystemTimePreciseAsFileTime(out long fileTime);
            return DateTimeOffset.FromFileTime(fileTime);
        }

        private static string m_separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        public static string separator { get => m_separator; }
        private static int txachannel = WDSP.id(1, 0);
        public int TXAchannel
        {
            get { return txachannel; }
            set { txachannel = value; }
        }

        private static ConsoleType theConsole;
        public static ConsoleType console
        {
            get { return theConsole; }
            set { theConsole = value; }
        }



        private static string m_sPeakPath = "";
        public static string PSPeakValueFilePath()
        {
            //Debug.Assert(Skin.GetAppDataPath().Length > 0);
            //return Skin.GetAppDataPath() + "PSPeak.dat";
            if (String.IsNullOrEmpty(m_sPeakPath))
            {
                m_sPeakPath = System.IO.Path.Combine(App.DataFolderPath + "PSPeak.dat");
            }

            return m_sPeakPath;
        }
        public static void SavePeakPSValue(string value)

        {
            string filepath = PSPeakValueFilePath();
            try
            {
                if (Double.TryParse(value, out double dbl))
                    File.WriteAllText(filepath, value);
            }
            catch (Exception e)
            {
                Debug.Assert(false);
                Common.LogException(e);
            }

        }


        public static string GetSavedPSPeakValue()
        {
            string filepath = PSPeakValueFilePath();
            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    string sval = File.ReadAllText(filepath);
                    double val = Double.Parse(sval);
                    return sval;
                }
            }
            catch (Exception e)
            {
                Debug.Assert(false);
                Common.LogException(e);
                return "";
            }
            return "";
        }

        public static void ControlList(Control c, ref ArrayList a)
        {
            if (c.Controls.Count > 0)
            {
                foreach (Control c2 in c.Controls)
                {
                    ControlList(c2, ref a);
                }
            }
            Debug.Assert(false);
            /*/
            
            if (c.GetType() == typeof(CheckBoxTS) || c.GetType() == typeof(CheckBoxTS) ||
                c.GetType() == typeof(ComboBoxTS) || c.GetType() == typeof(ComboBox) ||
                c.GetType() == typeof(NumericUpDownTS) || c.GetType() == typeof(NumericUpDown) ||
                c.GetType() == typeof(RadioButtonTS) || c.GetType() == typeof(RadioButton) ||
                c.GetType() == typeof(TextBoxTS) || c.GetType() == typeof(TextBox) ||
                c.GetType() == typeof(TrackBarTS) || c.GetType() == typeof(TrackBar) ||
                c.GetType() == typeof(ColorButton))
                a.Add(c);
            /*/

        }

        public static bool IsVisibleOnAnyScreen(Rectangle rect)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(rect))
                {
                    return true;
                }
            }

            return false;
        }


        public static bool ShiftKeyDown
        {
            get
            {
                return Keyboard.IsKeyDown(Keys.LShiftKey) || Keyboard.IsKeyDown(Keys.RShiftKey);
            }
        }

        public static void SaveForm(Form form, string tablename)
        {
            Debug.Assert(false);
            /*/
            ArrayList a = new ArrayList();
            ArrayList temp = new ArrayList();

            ControlList(form, ref temp);

            foreach (Control c in temp)             // For each control
            {
                if (c.GetType() == typeof(CheckBoxTS))
                    a.Add(c.Name + "/" + ((CheckBoxTS)c).Checked.ToString());
                else if (c.GetType() == typeof(ComboBoxTS))
                {
                    //if(((ComboBox)c).SelectedIndex >= 0)
                    a.Add(c.Name + "/" + ((ComboBoxTS)c).Text);
                }
                else if (c.GetType() == typeof(NumericUpDownTS))
                    a.Add(c.Name + "/" + ((NumericUpDownTS)c).Value.ToString());
                else if (c.GetType() == typeof(RadioButtonTS))
                    a.Add(c.Name + "/" + ((RadioButtonTS)c).Checked.ToString());
                else if (c.GetType() == typeof(TextBoxTS))
                    a.Add(c.Name + "/" + ((TextBoxTS)c).Text);
                else if (c.GetType() == typeof(TrackBarTS))
                    a.Add(c.Name + "/" + ((TrackBarTS)c).Value.ToString());
                else if (c.GetType() == typeof(ColorButton))
                {
                    Color clr = ((ColorButton)c).Color;
                    a.Add(c.Name + "/" + clr.R + "." + clr.G + "." + clr.B + "." + clr.A);
                }
#if (DEBUG)
                else if (c.GetType() == typeof(GroupBox) ||
                    c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(ComboBox) ||
                    c.GetType() == typeof(NumericUpDown) ||
                    c.GetType() == typeof(RadioButton) ||
                    c.GetType() == typeof(TextBox) ||
                    c.GetType() == typeof(TrackBar))
                    Debug.WriteLine(form.Name + " -> " + c.Name + " needs to be converted to a Thread Safe control.");
#endif
            }
            a.Add("Top/" + form.Top);
            a.Add("Left/" + form.Left);
            a.Add("Width/" + form.Width);
            a.Add("Height/" + form.Height);
            // a.Add("WindowState/" + (int)form.WindowState);

            DB.SaveVars(tablename, ref a);      // save the values to the DB
            /*/
        }

        public static void RestoreForm(Form form, string tablename, bool restore_size)
        {
            Debug.Assert(false);
        }

        public static void ForceFormOnScreen(Form f)
        {
            Screen[] screens = Screen.AllScreens;
            bool on_screen = false;

            int left = 0, right = 0, top = 0, bottom = 0;

            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i].Bounds.Left < left)
                    left = screens[i].Bounds.Left;

                if (screens[i].Bounds.Top < top)
                    top = screens[i].Bounds.Top;

                if (screens[i].Bounds.Bottom > bottom)
                    bottom = screens[i].Bounds.Bottom;

                if (screens[i].Bounds.Right > right)
                    right = screens[i].Bounds.Right;
            }

            if (f.Left > left &&
                f.Top > top &&
                f.Right < right &&
                f.Bottom < bottom)
                on_screen = true;

            if (!on_screen)
            {
                //f.Location = new Point(0, 0);

                if (f.Left < left)
                    f.Left = left;

                if (f.Top < top)
                    f.Top = top;

                if (f.Bottom > bottom)
                {
                    if ((f.Top - (f.Bottom - bottom)) >= top)
                        f.Top -= (f.Bottom - bottom);
                    else f.Top = 0;
                }

                if (f.Right > right)
                {
                    if ((f.Left - (f.Right - right)) >= left)
                        f.Left -= (f.Right - right);
                    else f.Left = 0;
                }
            }
        }

        public static void TabControlInsert(TabControl tc, TabPage tp, int index)
        {
            tc.SuspendLayout();
            // temp storage to rearrange tabs
            TabPage[] temp = new TabPage[tc.TabPages.Count + 1];

            // copy pages in order and insert new page when needed
            for (int i = 0; i < tc.TabPages.Count + 1; i++)
            {
                if (i < index) temp[i] = tc.TabPages[i];
                else if (i == index) temp[i] = tp;
                else if (i > index) temp[i] = tc.TabPages[i - 1];
            }

            // erase all tab pages
            while (tc.TabPages.Count > 0)
                tc.TabPages.RemoveAt(0);

            // add them back with new page inserted
            for (int i = 0; i < temp.Length; i++)
                tc.TabPages.Add(temp[i]);

            tc.ResumeLayout();
        }

        public static string[] SortedComPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            Array.Sort<string>(ports, delegate (string strA, string strB)
            {
                try
                {
                    int idA = int.Parse(strA.Substring(3));
                    int idB = int.Parse(strB.Substring(3));

                    return idA.CompareTo(idB);
                }
                catch (Exception)
                {
                    return strA.CompareTo(strB);
                }
            });
            return ports;
        }

        public static string RevToString(uint rev)
        {
            return ((byte)(rev >> 24)).ToString() + "." +
                ((byte)(rev >> 16)).ToString() + "." +
                ((byte)(rev >> 8)).ToString() + "." +
                ((byte)(rev >> 0)).ToString();
        }

        private static string m_sLogPath = "";
        public static void SetLogPath(string sPath)
        {
            m_sLogPath = sPath;
            LogFileFullPath = Path.Combine(m_sLogPath, AppName() + ".log");
        }

        private static string m_sFullFilePath;
        public static string LOG_NAME { get => "CoolSDR.log"; }
        public static string LogFileFullPath { get => m_sFullFilePath; private set => m_sFullFilePath = value; }

        private struct logstruct
        {
            internal string entry;
            internal bool showMsg;
            internal string xtra;
            internal Exception e;
        }

        static volatile bool inLogTask = false;
        static volatile bool shouldRunLogger = true;
        internal static void StopLogging()
        {
            shouldRunLogger = false;
            int slept = 0;
            while (inLogTask)
            {
                Thread.Sleep(10);
                slept += 10;
                if (slept > 2000)
                {
                    break;
                }
            }

            Debug.Assert(slept < 1000);
        }

        static ConcurrentQueue<logstruct> LogQ = new ConcurrentQueue<logstruct> { };

        static internal void RunLogger()
        {
            inLogTask = true;
            logstruct log;
            while (LogQ.Count > 0 || shouldRunLogger)
            {
                bool ok = LogQ.TryDequeue(out log);
                if (ok)
                {
                    if (log.e == null)
                    {
                        LogStringp(log);
                    }
                    else
                    {
                        LogExceptionp(log);
                    }
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
            inLogTask = false;
        }

        public static void LogString(string what, bool showMsg = false)
        {
            logstruct ls = new logstruct { };
            ls.showMsg = showMsg;
            ls.entry = what;
            LogQ.Enqueue(ls);
        }

        public static void LogException(Exception e, bool showMsg = false, string context = "")
        {
            logstruct ls = new logstruct { };
            ls.showMsg = showMsg;
            ls.e = e;
            ls.xtra = context;
            LogQ.Enqueue(ls);
        }

        private static void LogStringp(logstruct ls)
        {
            //lock (locker)
            {
                // MW0LGE very simple logger
                if (string.IsNullOrEmpty(m_sLogPath))
                {
                    Debug.Assert(false); // uh oh! SetLogPath was not called
                    return;
                }
                if (string.IsNullOrEmpty(m_sLogPath)) return;
                if (string.IsNullOrEmpty(ls.entry)) return;
                var fp = LogFileFullPath;

                try
                {
                    using (StreamWriter w = File.AppendText(fp))
                    {
                        //using block will auto close stream
                        w.Write("\r\nEntry : ");
                        w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                        w.WriteLine(ls.entry);
                        w.WriteLine("-------------------------------");
                        Debug.WriteLine(ls.entry);
                        if (ls.showMsg)
                        {
                            var foundTab = ls.entry.IndexOf('\t');
                            if (foundTab >= 0)
                            {
                                ls.entry = ls.entry.Substring(0, foundTab);
                            }
                            MessageBox.Show(ls.entry, App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch
                {

                }
            }
        }

        // private static object locker = new object();
        private static void LogExceptionp(logstruct l)
        {
            //lock (locker)
            {
                // MW0LGE very simple logger
                if (string.IsNullOrEmpty(m_sLogPath))
                {
                    Debug.Assert(false); // uh oh! SetLogPath was not called
                    return;
                }
                if (l.e == null) return;

                var fp = LogFileFullPath;
                if (l.showMsg)
                {
                    var mymsg = l.e.Message;
                    var foundTab = mymsg.IndexOf('\t');
                    if (foundTab >= 0)
                    {
                        mymsg = mymsg.Substring(0, foundTab);
                    }
                    MessageBox.Show(mymsg, App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(l.xtra + "\nAn unexpected error occurred: \n" + mymsg, "CoolSDR Error Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                try
                {
                    using (StreamWriter w = File.AppendText(fp))
                    {
                        //using block will auto close stream
                        w.Write("\r\nEntry : ");
                        w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                        if (string.IsNullOrEmpty(l.e.Message))
                            w.WriteLine("This exception contains no message.");
                        w.WriteLine(l.e.Message);
                        if (!String.IsNullOrEmpty(l.e.StackTrace))
                        {

                            StackTrace st = new StackTrace(l.e, true);
                            StackFrame sf = st.GetFrames().Last();
                            w.WriteLine("File : " + sf.GetFileName() + " ... line : " + sf.GetFileLineNumber().ToString());

                            w.WriteLine("---------stacktrace------------");
                            w.WriteLine(l.e.StackTrace);
                        }
                        w.WriteLine("-------------------------------");
                    }
                }
                catch
                {

                }
            }
        }

        public static DateTime GetLinkerTime(Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        public static DateTime AppBuildDate()
        {
            return GetLinkerTime(Assembly.GetExecutingAssembly());
        }

        // returns something like "Thetis"
        public static string AppName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;

        }

        public static int AppMajor()
        {
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            var maj = ver.Major;
            return maj;
        }

        public static int AppMinor()
        {
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            var minor = ver.Minor;
            return minor;
        }

        public static int AppRevision()
        {
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            var rev = ver.Revision;
            return rev;
        }

        public static int AppBuild
        {
            get
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;
                var rev = ver.Build;

                return rev;
            }
        }

        public static HPSDRModel RadioModel
        {
            get
            {
#if (DEBUG)
                Debug.Assert(radioModel == HPSDRModel.HERMES);
#endif
                return radioModel;
            }
            set => radioModel = value;
        }

        public static FrmMain Console { get => console; set => console = value; }
        public static string Seperator { get => separator; }

        private static HPSDRModel radioModel;


        // returns something like: "Thetis v2.8.12"
        public static string VersionName()
        {
            var thisAssemName = Assembly.GetExecutingAssembly().GetName().Name;
            string versionName = thisAssemName + " v" + AppMajor() + "." + AppMinor() + "." + AppBuild;
            return versionName;
        }



        // returns the Thetis version number in "a.b.c" format
        // MW0LGE moved here from titlebar.cs, and used by console.cs and others
        private static string m_sVersionNumber = "";
        private static string m_sFileVersion = "";
        public static string GetVerNum()
        {
            if (m_sVersionNumber != "") return m_sVersionNumber;

            SetupVersions();

            return m_sVersionNumber;
        }
        public static string GetFileVersion()
        {
            if (m_sFileVersion != "") return m_sFileVersion;

            SetupVersions();

            return m_sFileVersion;
        }

        private static void SetupVersions()
        {
            //MW0LGE build version number string once and return that
            // if called again. Issue reported by NJ2US where assembly.Location
            // passed into GetVersionInfo failed. Perhaps because norton or something
            // moved the file after it was accessed. The version isn't going to
            // change anyway while running, so obtaining it once is fine.
            if (m_sVersionNumber != "" && m_sFileVersion != "") return; // already setup

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            m_sVersionNumber = fvi.FileVersion.Substring(0, fvi.FileVersion.LastIndexOf("."));
            m_sFileVersion = fvi.FileVersion;
        }
    }
}