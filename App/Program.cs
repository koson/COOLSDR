using CoolSDR.Class;
using CoolSDR.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thetis;
using static CoolSDR.Forms.FrmMain;

namespace CoolSDR
{
    internal static class Program
    {
        public static FrmMain console;
        public static Task GlobalLogTask;
        internal static void ThrowAndExit(string why, int returnCode = -1000)
        {
            var e = new Exception(why);
            ShowBadExitMessage(e);
            try
            {
                Display.ShutdownDX2D();

            }
            catch (Exception) { }
            finally
            {
                Application.Exit();
                Environment.Exit(returnCode);
            }
        }
        static void ShowBadExitMessage(Exception e)
        {
            try
            {
                Common.LogException(e);
                Display.ShutdownDX2D();
            }
            catch (Exception) { }
            finally
            {
                if (Common.console != null)
                {
                    Common.console.Hide();
                }
                MessageBox.Show("The program was closed due to a fatal error:\n\n" + e.Message + "\n\n"
                    + e.StackTrace, App.Name);
            }

        }
        static void WriteThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                Display.ShutdownDX2D();
            }
            catch (Exception) { }
            Thetis.Common.LogException(e.Exception);
            ShowBadExitMessage(e.Exception);
            Application.Exit();
        }

        static void WriteUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Thetis.Common.LogException((Exception)e.ExceptionObject);
            try
            {
                Display.ShutdownDX2D();
            }
            catch (Exception) { }
            ShowBadExitMessage((Exception)e.ExceptionObject);
            Application.Exit();
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException +=
                new ThreadExceptionEventHandler(WriteThreadException);

            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(WriteUnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            Common.SetLogPath(App.GetDataFolder());
            Task t = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "LoggerThread";
                Common.RunLogger();
            });
            GlobalLogTask = t;

            Common.LogString("App Started");
            var fo = new DummyException("Welcome to the logfile!");
            Common.LogException(fo);

            if (!DLLCheck.CheckDLLVersions())
            {
                Application.Exit();
                return;
            }

            var f = new FrmMain();
            f.MinimumSize = new System.Drawing.Size(1024, 712);
            try
            {
                Application.Run(f);
            }
            catch (Exception e)
            {
                // likely user closed form when we were trying to start up
                Debug.Print(e.Message);
                Thetis.Common.LogException(e);
            }
            finally
            {
                Common.StopLogging();
            }
        }
    }
}
