using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thetis;


namespace CoolSDR.Class
{
    public class DLLCheck
    {
        private static void DebugMissingDLL(string dllName, bool only_if_not_exist = false, string extramsg = "")
        {
            string cwd = Environment.CurrentDirectory;
            if (!only_if_not_exist)
            {
                String msg = "Looking for " + dllName + "\n";
                Common.LogString(msg);
                MessageBox.Show(msg + "Current working dir is:\n" + cwd, App.Name);
            }

            string dllpath = cwd + "\\" + dllName;
            bool exists = File.Exists(dllpath);
            if (exists)
            {
                if (!only_if_not_exist)
                    MessageBox.Show(dllName + " DOES exist in cwd");
            }
            else
            {
                string tit = App.Name + (": Required DLL ") + dllName + " is not found.";
                string msg = dllName + " DOES NOT exist in:\n" + cwd + "\n\n" +
                    extramsg + "\n\n" + "You may have to re-install the app\n\n\n" + App.Name + " will now close.";
                Common.LogString(tit);
                MessageBox.Show(
                    msg,
                    tit, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
            }


        }
        public static bool CheckDLLVersions()
        {
            int nCMasterVersion;
            int nWDSPVersion;
            int nPAVersion;
            bool bRet;

            // PortAudio.dll FIRST (since ChannelMaster depends on it)
            try
            {

                nPAVersion = PortAudioForCoolSDR.PA_GetVersion();
                if (nPAVersion != Versions._PORTAUDIO_VERSION)
                {
                    DialogResult dr = MessageBox.Show(
                        "Incorrect version of portaudio.dll installed.",
                        "Version error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }

            }
            catch (Exception e)
            {
                DebugMissingDLL("PortAudioForCoolSDR.dll");
                MessageBox.Show("System error was : " + e.Message);
                MessageBox.Show("Could not find PA_GetVersion() " +
                    "in PortAudioForCoolSDR.dll .\nEnsure correct version installed."
                    , "Version function error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            DebugMissingDLL("libfftw3-3.dll", true, // ChannelMaster won't load if this isn't present.
                    " is required in the application folder");

            // channelmaster.dll
            try
            {
                nCMasterVersion = cmaster.GetCMVersion();
                if (nCMasterVersion != Versions._CMASTER_VERSION)
                {
                    MessageBox.Show(
                        "Incorrect version of ChannelMaster.dll installed.",
                        "Version error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("System error when checking ChannelMaster.dll error was : " + e.Message, "CoolSDR cannot start");
                DebugMissingDLL("ChannelMaster.dll");
                MessageBox.Show(
                    "Could not find GetCMVersion() in channelmaster.dll .\nEnsure correct version installed.",
                    "Version function error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }


            // wdsp.dll
            try
            {
                nWDSPVersion = WDSP.GetWDSPVersion();
                if (nWDSPVersion != Versions._WDSP_VERSION)
                {
                    MessageBox.Show(
                       "Incorrect version of wdsp.dll installed.", "Version error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Could not find GetWDSPVersion() in wdsp.dll .\nEnsure correct version installed.", "Version function error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DebugMissingDLL(
"wdsp.dll", true, "It is required in that application folder.");
                return false;
            }

            bRet = (nCMasterVersion == Versions._CMASTER_VERSION) &&
                //(nWDSPVersion == Versions._WDSP_VERSION) &&
                (nPAVersion == Versions._PORTAUDIO_VERSION);

            return bRet;
        }
    }
}
