using CoolSDR.Forms;
using MaterialSkin2DotNet.Controls;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Thetis;
using WeifenLuo.WinFormsUI.Docking;
using static Thetis.Audio;
using static Thetis.PortAudioForCoolSDR;

namespace CoolSDR.Class
{
    using ComboType = MaterialComboBox;
    public class PortAudioHelper : IDisposable
    {
        public volatile static bool m_waiting_for_portaudio;

        public PortAudioHelper() { }
        ~PortAudioHelper() { }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PA_Terminate();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // err = Pa_IsFormatSupported( inputParameters, outputParameters, desiredSampleRate );
        // if (err == paFormatIsSupported ) (...)
        public static int FindDevice(List<PaDeviceInfoEx> list, string name)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var device = list[i];
                if (device.Name == name)
                    return i;
            }
            return -1;
        }


        public static void InitPortAudio()
        {
            m_waiting_for_portaudio = true;
#pragma warning disable CS0618 // Type or member is obsolete
            new Thread(() =>
            {

                PortAudioForCoolSDR.PA_Initialize();
                m_waiting_for_portaudio = false;
            })
            {
                IsBackground = true,
                ApartmentState = ApartmentState.STA, // no ASIO devices without Apartment state
                Name = "PortAudioStarter"
            }.Start();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public static int SampleRate { get; internal set; }
        public static int BufferSize { get; internal set; }
        public static string API { get; internal set; }
        public static string InputDevice { get; internal set; }
        public static string OutputDevice { get; internal set; }
        public static bool AudioExclusive { get; internal set; }
        public static bool LoadingSettings { get; private set; }




        private static readonly Dictionary<string, List<PaDeviceInfoEx>> m_devicesIn = new Dictionary<string, List<PaDeviceInfoEx>>();
        private static readonly Dictionary<string, List<PaDeviceInfoEx>> m_devicesOut = new Dictionary<string, List<PaDeviceInfoEx>>();
        private static readonly List<PaHostAPIInfoEx> m_apis = new List<PaHostAPIInfoEx>();

        public static List<PaHostAPIInfoEx> HostAPIs
        {
            get
            {
                return m_apis;
            }
        }

        public static PaHostAPIInfoEx APIFromIndex(int index)
        {
            return m_apis[index];
        }

        public static PaHostAPIInfoEx APIFromName(string name)
        {
            var ret = m_apis.Find(n => n.Name == name);
            Debug.Assert(ret.Name == name);
            return ret;
        }


        public string DefaultAPIName
        {
            get
            {
                if (m_devicesIn.Count == 0 || m_devicesOut.Count == 0 || m_apis.Count == 0)
                {
                    GetHosts(true);
                }
                int i = PA_GetDefaultHostApi();
                //fixme : return an api only if it has devices (in and out)
                string pref_api = null;
                if (m_devicesIn.Count > 0 && m_devicesOut.Count > 0)
                {
                    foreach (var key in m_devicesIn.Keys)
                    {
                        var devs = m_devicesIn[key];
                        if (devs.Count > 0)
                        {
                            var outdevs = m_devicesOut[key];
                            if (outdevs.Count > 0)
                            {
                                pref_api = key;
                                break;
                            }

                        }
                    }
                }
                if (!string.IsNullOrEmpty(pref_api))
                    return pref_api;

                return PA_GetHostApiInfo(i).name;
            } // get
        }
        public static List<PaDeviceInfoEx> DevicesIn(string APIName)
        {
            return m_devicesIn[APIName];
        }
        public static List<PaDeviceInfoEx> DevicesOut(string APIName)
        {
            return m_devicesOut[APIName];
        }

        public static Dictionary<string, List<PaDeviceInfoEx>> DevicesOutAll { get => m_devicesOut; }
        public static Dictionary<string, List<PaDeviceInfoEx>> DevicesInAll { get => m_devicesIn; }


        public static PaDeviceInfoEx GetDefaultDevice(DeviceKinds kind, string APIName)
        {
            List<PaDeviceInfoEx> myList;
            if (kind == DeviceKinds.input)
                m_devicesIn.TryGetValue(APIName, out myList);
            else
                m_devicesOut.TryGetValue(APIName, out myList);

            return myList[0];
        }

        public void GetHosts(bool refresh = false)
        {
            int host_index = 0;
            var hosts = Audio.GetPAHosts(refresh);

            bool useCachedValues = false;

            if (m_devicesIn.Count > 0 || m_devicesOut.Count > 0 || m_apis.Count > 0)
            {
                useCachedValues = !refresh;
            }

            if (!useCachedValues)
            {

                m_apis.Clear();
                int ctr = 0;
                foreach (var a in hosts)
                {
                    PaHostApiInfo info = (PaHostApiInfo)a;
                    m_apis.Add(new PaHostAPIInfoEx(info, ctr++));
                }


                foreach (var a in hosts)
                {
                    var hostName = a.name;
                    var indevs = Audio.GetPAInputDevices(host_index);
                    var outdevs = Audio.GetPAOutputDevices(host_index);
                    if (indevs.Count > 0 || outdevs.Count > 0)
                    {

                        {
                            foreach (PaDeviceInfoEx dev in indevs)
                            {
                                if (!m_devicesIn.ContainsKey(hostName))
                                {
                                    m_devicesIn.Add(hostName, new List<PaDeviceInfoEx>());
                                }

                                var devs = m_devicesIn[hostName];
                                dev.Kind = DeviceKinds.input;
                                devs.Add(dev);
                            }
                            foreach (PaDeviceInfoEx dev in outdevs)
                            {
                                if (!m_devicesOut.ContainsKey(hostName))
                                {
                                    m_devicesOut.Add(hostName, new List<PaDeviceInfoEx>());
                                }
                                var devs = m_devicesOut[hostName];
                                devs.Add(dev);
                                dev.Kind = DeviceKinds.output;
                            }
                        }
                    }


                    host_index++; // Increment host index
                }
            }

            // sanity
#if DEBUG
            foreach (var apiName in m_devicesIn.Keys)
            {
                var devs = m_devicesIn[(apiName)];
                foreach (var d in devs)
                {
                    Debug.Assert(d.Kind == DeviceKinds.input);
                }
            }

            foreach (var apiName in m_devicesOut.Keys)
            {
                var devs = m_devicesOut[(apiName)];
                foreach (var d in devs)
                {
                    Debug.Assert(d.Kind == DeviceKinds.output);
                }
            }
#endif

            if (m_devicesIn.Count == 0)

            {

                Common.ExitApp("No input devices found", -1);
                throw new Exception("No input devices found");
            }

            if (m_devicesOut.Count == 0)
            {

                Common.ExitApp("No output devices found", -2);

                throw new Exception("No output devices found");
            }


        }


        private static int SelectComboItem(ref ComboType cbo, string what, int default_index = 0)
        {
            int index = 0;
            foreach (string s in cbo.Items)
            {
                if (s == what)
                {
                    cbo.SelectedIndex = index;
                    cbo.Refresh();
                    return index;
                }
                index++;
            }

            cbo.SelectedIndex = default_index;
            cbo.Refresh();
            return -1;
        }

        private static int PopCombo(ref ComboType cbo, ref List<PaDeviceInfoEx> devs)
        {
            cbo.Items.Clear();
            foreach (PaDeviceInfoEx device in devs)
            {
                cbo.Items.Add(device.Name);
            }
            return cbo.Items.Count;
        }

        public class NoAudioDevicesException : Exception
        {
            public NoAudioDevicesException(string what) : base(what) { }
        }
        internal void SetDefaults(ComboControls c)
        {
            CheckComboControls(c);
            var cboAPI = c.API;
            var cboAudioIn = c.AudioIn;
            var cboBuffer = c.BufferSize;
            var cboAudioOut = c.AudioOut;
            var sapi = DefaultAPIName;
            var cboSampleRate = c.SampleRate;

            PopulateCombos(cboAPI, cboAudioIn, cboAudioOut, sapi);
            PopulateBufferSizesAndSampleRates(cboSampleRate, cboBuffer);
            var idx = SelectComboItem(ref cboAPI, sapi);
            if (idx >= 0)
            {
                if (string.IsNullOrEmpty(API))
                {
                    API = cboAPI.SelectedItem.ToString();
                }
                List<PaDeviceInfoEx> inDevs = new List<PaDeviceInfoEx>();
                if (m_devicesIn != null && m_devicesIn.Count > 0)
                {
                    inDevs = m_devicesIn[API];
                }
                var outDevs = m_devicesOut[sapi];

                var where = SelectComboItem(ref cboAudioIn, inDevs[0].Name);

                if (where < 0)
                {
                    throw new NoAudioDevicesException("Failed to select default input device: " + inDevs[0].Name);
                }

                cboBuffer.SelectedIndex = cboBuffer.Items.Count - 1;



                where = SelectComboItem(ref cboAudioOut, outDevs[0].Name);
                if (where < 0)

                {
                    throw new NoAudioDevicesException("Failed to select default output device: " + outDevs[0].Name);
                }

                where = SelectComboItem(ref cboSampleRate, outDevs[0].Info.defaultSampleRate.ToString());
                if (where < 0)
                {
                    Common.LogString("NOTE: cannot find SampleRate: " + outDevs[0].Info.defaultSampleRate.ToString());
                }

            }
            else
            {
                throw new NoAudioDevicesException("failed to select default api: " + sapi);
            }

        }

        public static bool ChangeAPI(string newAPI, MaterialComboBox cbIn, MaterialComboBox cbOut)
        {
            if (!m_devicesIn.ContainsKey(newAPI)) return false;
            var inDevs = m_devicesIn[newAPI];
            var outDevs = m_devicesOut[newAPI];
            PopCombo(ref cbIn, ref inDevs);
            PopCombo(ref cbOut, ref outDevs);
            SelectComboItem(ref cbIn, "", 0);
            SelectComboItem(ref cbOut, "", 0);
            return true;

        }

        public static bool ChangeAPI(string newAPI, MaterialComboBox cbIn, MaterialComboBox cbOut, MaterialComboBox cbAPI)
        {

            if (!m_devicesIn.ContainsKey(newAPI)) return false;
            SelectComboItem(ref cbAPI, newAPI);
            var inDevs = m_devicesIn[newAPI];
            var outDevs = m_devicesOut[newAPI];
            PopCombo(ref cbIn, ref inDevs);
            PopCombo(ref cbOut, ref outDevs);
            SelectComboItem(ref cbIn, "", 0);
            SelectComboItem(ref cbOut, "", 0);
            return true;

        }

        public static readonly int[] AudioSampleRates =
            new int[] { 8000, 11025, 16000, 16000, 22050, 44100, 48000, 88200,
                96000, 192000 };

        public static readonly int[] AudioBufferSizes =
            new int[] { 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };

        public static int MIN_SAMPLERATE = AudioSampleRates.Min();
        public static int MAX_SAMPLERATE = AudioSampleRates.Max();
        public static int DEFAULT_SAMPLERATE = 44100;


        public static int MAX_BUFFERSIZE = AudioBufferSizes.Max();
        public static int MIN_BUFFERSIZE = AudioBufferSizes.Min();



        int SelectComboItem(MaterialComboBox cb, string item, int default_index = 0)
        {
            var sel = cb.FindStringExact(item);
            if (sel == -1)
            {
                if (cb.Items.Count > 0)
                {
                    cb.SelectedIndex = default_index;
                    return -1;
                }
            }
            else
            {
                cb.SelectedIndex = sel;
                return sel;
            }
            return -1;
        }

        private void FillCombo(MaterialComboBox c, int[] array, bool force = false)
        {
            Debug.Assert(c != null);
            if (c.Items.Count == array.Max() && !force) return;
            c.Items.Clear();
            foreach (int i in array)
            {
                c.Items.Add(i.ToString());
            }
        }

        private void PopulateBufferSizesAndSampleRates(MaterialComboBox SampleCombo, MaterialComboBox BufferCombo, bool force = false)
        {
            Debug.Assert(SampleCombo != null);
            Debug.Assert(BufferCombo != null);
            if (BufferCombo != null && BufferCombo.Items.Count != AudioBufferSizes.Max())
            {
                FillCombo(BufferCombo, AudioBufferSizes, force);

            }
            if (SampleCombo != null && SampleCombo.Items.Count != AudioSampleRates.Max())
            {
                FillCombo(SampleCombo, AudioSampleRates, force);
            }
        }


        public void PopulateCombos(MaterialComboBox cboAPI, MaterialComboBox cboIn, MaterialComboBox cboOut,
                                    string apiName)
        {
            Debug.Assert(cboAPI != null);
            Debug.Assert(cboIn != null);
            Debug.Assert(cboOut != null);
            Debug.Assert(apiName != null);
            Debug.Assert(m_devicesIn.Count > 0 || m_devicesOut.Count > 0);

            cboAPI.Items.Clear();
            cboIn.Items.Clear();
            cboOut.Items.Clear();
            int found = -1;
            int idx = 0;

            // using m_DevicesOut here because we may not have any input devices,
            // eg remote desktop.
            if (m_devicesOut == null || m_devicesOut.Count == 0)
            {
                Common.LogString("No output devices detected on machine. Application Exiting", true);
            }
            foreach (var key in m_devicesOut.Keys)
            {
                cboAPI.Items.Add(key);
                if (!string.IsNullOrEmpty(apiName) && key == apiName)
                {
                    found = idx;
                }
                idx++;

            }

            if (found < 0)
            {
                throw new Exception("PopCombos [PortAudioHelper]: cannot find apiName: " + apiName + " in api collection");
            }

            found = this.SelectComboItem(cboAPI, apiName);
            if (found < 0)
            {
                throw new Exception("PopCombos [PortAudioHelper]: cannot find apiName: " + apiName + " in UI control");
            }
            List<PaDeviceInfoEx> indevs = new List<PaDeviceInfoEx>();
            if (m_devicesIn.ContainsKey(apiName))
            {
                indevs = m_devicesIn[apiName];
            }

            var outdevs = m_devicesOut[apiName];

            foreach (var d in indevs)
            {
                cboIn.Items.Add(d.Name); // note: You can add the actual devices here if u want. I chose not to.
            }
            foreach (var d in outdevs)
            {
                cboOut.Items.Add(d.Name);
            }
        }


        public struct ComboControls
        {
            public ComboType API;
            public ComboType AudioIn;
            public ComboType AudioOut;
            public ComboType SampleRate;
            public ComboType BufferSize;
            public MaterialCheckbox Exclusive;
        }

        public static void CheckComboControls(ComboControls c)
        {
            Debug.Assert(c.API != null);
            Debug.Assert(c.AudioIn != null);
            Debug.Assert(c.AudioOut != null);
            Debug.Assert(c.SampleRate != null);
            Debug.Assert(c.BufferSize != null);
            Debug.Assert(c.Exclusive != null);

        }


        public struct LoadSettingsResult
        {
            public const int ErrorBase = -1;
            public LoadSettingsRetval Retval;
        }
        public enum LoadSettingsRetval
        {

            Good,
            BadAPI = LoadSettingsResult.ErrorBase - 1,
            BadInputDevice = LoadSettingsResult.ErrorBase - 2,
            BadOutputDevice = LoadSettingsResult.ErrorBase - 3,
            BadSampleRate = LoadSettingsResult.ErrorBase - 4,
            BadBufferSize = LoadSettingsResult.ErrorBase - 5

        }


        public LoadSettingsResult LoadSettings(PortAudioSettings s, ComboControls c, bool showErrorMsgs = true)
        {
            LoadSettingsResult ret;
            ret.Retval = LoadSettingsRetval.Good;
            LoadingSettings = true;
            CheckComboControls(c);
            var cboSampleRate = c.SampleRate;
            var cboAPI = c.API;
            var cboInput = c.AudioIn;
            var cboOutput = c.AudioOut;
            var ExclusiveCheckBox = c.Exclusive;
            var cboBuffer = c.BufferSize;

            try
            {
                PopulateBufferSizesAndSampleRates(cboSampleRate, cboBuffer);

                API = s.API;
                if (string.IsNullOrEmpty(API) || !m_devicesIn.ContainsKey(API) || !m_devicesOut.ContainsKey(API))
                {
                    string oldapi = s.API;
                    if (oldapi == null) oldapi = "API had no value set";

                    if (!s.HadSettingsToLoad)
                        API = DefaultAPIName;

                    Common.LogString("PortAudioSettings: Unable to find API, with saved name: " + oldapi, showErrorMsgs);


                }
                if (cboAPI != null)
                {
                    var sel = SelectComboItem(cboAPI, API);
                    Debug.Assert(sel >= 0);
                }


                if (ExclusiveCheckBox != null)
                    ExclusiveCheckBox.Visible = API == "Windows WASAPI";


                SampleRate = s.SampleRate;
                if (SampleRate < MIN_SAMPLERATE || SampleRate > MAX_SAMPLERATE)
                    SampleRate = DEFAULT_SAMPLERATE;

                if (cboSampleRate != null)
                {
                    var sel = SelectComboItem(cboSampleRate, SampleRate.ToString());
                    Debug.Assert(sel >= 0);
                }

                BufferSize = s.BufferSize;
                if (BufferSize < MIN_BUFFERSIZE || BufferSize > MAX_BUFFERSIZE)
                    BufferSize = MAX_BUFFERSIZE;

                if (cboBuffer != null)
                {
                    var sel = SelectComboItem(cboBuffer, BufferSize.ToString(), cboBuffer.Items.Count - 1);
                    Debug.Assert(sel >= 0);
                }

                List<PaDeviceInfoEx> indevs = new List<PaDeviceInfoEx>();
                if (m_devicesIn != null && m_devicesIn.Count > 0)
                {
                    indevs = m_devicesIn[API];
                }

                var incbo = cboInput;
                PopCombo(ref incbo, ref indevs);

                InputDevice = s.InputDevice;
                int found = FindDevice(indevs, InputDevice);
                if (found == -1)
                {
                    Debug.Assert(string.IsNullOrEmpty(InputDevice));
                    Common.LogString("InputDevice, with name: " + InputDevice + " was not found.");
                    ret.Retval = LoadSettingsRetval.BadInputDevice;
                    if (showErrorMsgs && indevs.Count > 0 && s.HadSettingsToLoad)
                    {
                        MessageBox.Show("Unable to find input device: " + InputDevice + "\n\nSo using default: " + indevs[0].Name, "CoolSDR Input Device Error");
                        ret.Retval = LoadSettingsRetval.BadInputDevice;
                    }
                    if (indevs.Count > 0)
                    {
                        InputDevice = indevs[0].Name;
                        cboInput.SelectedIndex = 0;
                    }

                }
                else
                {
                    if (cboInput != null)
                    {
                        cboInput.SelectedIndex = found;
                    }
                }

                OutputDevice = s.OutputDevice;
                var outdevs = m_devicesOut[API];
                var outcbo = cboOutput;
                PopCombo(ref outcbo, ref outdevs);
                found = FindDevice(outdevs, OutputDevice);
                if (found == -1)
                {
                    Debug.Assert(string.IsNullOrEmpty(OutputDevice));
                    Common.LogString("OutputDevice, with name: " + OutputDevice + " was not found.");
                    if (showErrorMsgs && outdevs.Count > 0 && s.HadSettingsToLoad)
                    {
                        MessageBox.Show("Unable to find output device: " + OutputDevice + "\n\nSo using default: " + outdevs[0].Name, "CoolSDR Output Device Error");
                        ret.Retval = LoadSettingsRetval.BadOutputDevice;
                    }
                    if (outdevs.Count > 0)
                    {
                        OutputDevice = outdevs[0].Name;
                        cboOutput.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (cboOutput != null)
                        cboOutput.SelectedIndex = found;
                }

                AudioExclusive = s.AudioExclusive;
                if (ExclusiveCheckBox != null)
                {
                    ExclusiveCheckBox.Checked = AudioExclusive;
                }
            }
            catch (Exception e)
            {
                LoadingSettings = false;
                Common.LogException(e);
                throw e;
            }
            finally
            {
                LoadingSettings = false;

            }
            return ret;

        }



        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PortAudioHelper()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
