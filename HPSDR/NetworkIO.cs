/*
*
* Copyright (C) 2006 Bill Tracey, KD5TFD, bill@ewjt.com 
* Copyright (C) 2010-2020  Doug Wigley
* 
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

namespace Thetis
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Net.NetworkInformation;
    using CoolSDR.Forms;
    using System.Linq;
    using CoolSDR;
    using System.Runtime.InteropServices;
    using SharpDX;
    using System.Diagnostics;
    using CoolSDR.Class;
    using System.Runtime.CompilerServices;

    partial class NetworkIO
    {
        public NetworkIO()
        {
        }

        public static bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }
            bool b = int.TryParse(splitValues[0], out int value);
            if (value <= 0)
            {
                // first part of address must have a number, not zero or less
                return false;
            }


            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
        }


        public static bool isFirmwareLoaded = false;

        private static float swr_protect = 1.0f;
        public static float SWRProtect
        {
            get { return swr_protect; }
            set { swr_protect = value; }
        }

        public static void SetOutputPower(float f)
        {
            if (f < 0.0)
            {
                f = 0.0F;
            }
            if (f >= 1.0)
            {
                f = 1.0F;
            }

            int i = (int)(255 * f * swr_protect);
            //System.Console.WriteLine("output power i: " + i); 
            SetOutputPowerFactor(i);
        }

        // get the name of this PC and, using it, the IP address of the first adapter
        //static string strHostName = Dns.GetHostName();
        // public static IpAddress[] addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        // get a socket to send and receive on
        static Socket socket; // = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        // set an endpoint
        static IPEndPoint iep;
        static byte[] data = new byte[1444];
        const int DiscoveryPort = 1024;
        const int LocalPort = 0;
        public static bool enableStaticIP { get; set; } = false;
        public static uint static_host_network { get; set; } = 0;
        public static bool FastConnect { get; set; } = false;
        public static HPSDRHW BoardID { get; set; } = HPSDRHW.Hermes;
        public static byte FWCodeVersion { get; set; } = 0;
        public static string EthernetHostIPAddress { get; set; } = "";
        public static int EthernetHostPort { get; set; } = 0;
        public static string HpSdrHwIpAddress { get; set; } = "";
        public static string HpSdrHwMacAddress { get; set; } = "";
        public static byte NumRxs { get; set; } = 0;

        public static RadioProtocol RadioProtocolSaved
        {
            get
            {
                return Radio.Settings.Protocol;
            }

            set
            {
                Radio.Settings.Protocol = value;
            }
        }

        // a 'temporary' protocol we are currently using
        // this is actually set by the network code on successful Discovery()
        public static RadioProtocol CurrentRadioProtocol
        {
            get;
            set;
        } = RadioProtocol.Auto;

        // the protocol the user saved, if any
        public static RadioProtocol RadioProtocolSelected
        {
            get { return RadioProtocolSaved; }
        }


        public static RadioBase Radio { get; set; }

        public static string GetLastSockError()
        {
            string errorMessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
            return errorMessage;
        }

        public static ErrInfo LastError { get; set; }
        // set me in InitNetwork()
        public static FrmMain MainForm { get; private set; }
        // can throw
        public static void InitNetwork(FrmMain mainForm, RadioBase radio)
        {
            MainForm = mainForm;
            Radio = radio;
            InitNetwork(true, out IPAddress[] addr, out List<IPAddress> addrList);
        }



        private static List<IPAddress> m_addresses;
        private static IPAddress[] m_addrs;
        public static WinsockManager SockManager
        {
            get;
            private set;
        }
        // returns nothing, but populates m_addrs and m_addresses if it has none, or refresh is specified.
        // can throw exceptions.
        public static void InitNetwork(bool refresh, out IPAddress[] addr, out List<IPAddress> addrList)
        {

            LastError = null;

            if (SockManager == null)
            {
                SockManager = new WinsockManager();
            }


            if (!refresh)
            {
                refresh = m_addresses == null;
                if (!refresh)
                {
                    refresh = m_addrs.Length == 0;
                }
            }
            if (!refresh)
            {
                addr = m_addrs;
                addrList = m_addresses;
                return;
            }


            int adapterIndex = adapterSelected - 1;
            addr = null;
            try
            {
                addr = Dns.GetHostAddresses(Dns.GetHostName());
            }
            catch (SocketException e)
            {

                addr = Dns.GetHostAddresses(Dns.GetHostName());
                Common.LogException(e);

            }
            catch (Exception e)
            {
                LastError = ErrInfo.MakeError("Dns.GetHostAddresses failed", -1, e.Message);
                throw e;
            }

            var nInterfaces = GetNetworkInterfaces();
            if (nInterfaces <= 0)
            {
                LastError = ErrInfo.MakeError("Dns.GetHostAddresses failed", -1, "No suitable Network interfaces found.");
                throw new Exception("Cannot locate any useful network devices");
            }

            addrList = new List<IPAddress>();

            // make a list of all the adapters that we found in Dns.GetHostEntry(strHostName).AddressList
            foreach (IPAddress a in addr)
            {
                // make sure to get only IPV4 addresses!
                // test added because Erik Anderson noted an issue on Windows 7.  May have been in the socket
                // construction or binding below.
                if (a.AddressFamily == AddressFamily.InterNetwork)
                {
                    addrList.Add(a);
                }
            }

            m_addrs = addr;
            m_addresses = addrList;

            return;
        }

        // called when radio powered up. Populates public hpsdrd if successful (always picks the first one if there are many)
        // returns 0 on success, but also may throw
        public static List<HPSDRDevice> hpsdrd = null;
        public static int initRadio(RadioBase radio, bool refresh = false)
        {
            int rc = 0;
            InitNetwork(refresh, out IPAddress[] addr, out List<IPAddress> addrList);



            bool foundRadio = false;
            hpsdrd = new List<HPSDRDevice>();
            enableStaticIP = radio.UseStaticIp;

            if (enableStaticIP)
            {
                HpSdrHwIpAddress = radio.IpAddress;

                IPAddress remoteIp = IPAddress.Parse(HpSdrHwIpAddress);
                IPEndPoint remoteEndPoint = new IPEndPoint(remoteIp, 0);
                Socket sock = new Socket(
                                      AddressFamily.InterNetwork,
                                      SocketType.Dgram,
                                      ProtocolType.Udp);
                IPEndPoint localEndPoint = QueryRoutingInterface(sock, remoteEndPoint);
                EthernetHostIPAddress = IPAddress.Parse(localEndPoint.Address.ToString()).ToString();

                sock.Close();
                sock = null;

                // if success set foundRadio to true, and fill in ONE hpsdrd entry.
                IPAddress targetIP;
                IPAddress hostIP;
                if (IPAddress.TryParse(EthernetHostIPAddress, out hostIP) && IPAddress.TryParse(HpSdrHwIpAddress, out targetIP))
                {
                    System.Console.WriteLine(String.Format("Attempting connect to host adapter {0}, Static IP {1}", EthernetHostIPAddress, HpSdrHwIpAddress));

                    if (DiscoverRadioOnPort(ref hpsdrd, hostIP, targetIP))
                    {
                        foundRadio = true;

                        // make sure that there is only one entry in the list!
                        if (hpsdrd.Count > 0)
                        {
                            // remove the extra ones that don't match!
                            HPSDRDevice m2 = null;
                            foreach (var m in hpsdrd)
                            {
                                if (m.IPAddress.CompareTo(HpSdrHwIpAddress) == 0)
                                {
                                    m2 = m;
                                }
                            }

                            // clear the list and put our single element in it, if we found it.
                            hpsdrd.Clear();
                            if (m2 != null)
                            {
                                hpsdrd.Add(m2);
                                goto ActuallyConnect;
                            }
                            else
                            {
                                foundRadio = false;
                            }
                        }
                    }
                }
            }

            // if (FastConnect && (EthernetHostIPAddress.Length > 0) && (HpSdrHwIpAddress.Length > 0))
            // 'FastConnect in Thetis is the checkbox 'use last ip address', which should be the default if we have
            // a static ip saved, in any case. FFS.
            if ((EthernetHostIPAddress.Length > 0) && (HpSdrHwIpAddress.Length > 0))
            {
                // if success set foundRadio to true, and fill in ONE hpsdrd entry.
                IPAddress targetIP;
                IPAddress hostIP;
                if (IPAddress.TryParse(EthernetHostIPAddress, out hostIP) && IPAddress.TryParse(HpSdrHwIpAddress, out targetIP))
                {
                    string s = String.Format("Attempting fast re-connect to host adapter {0}, IP {1}", EthernetHostIPAddress, HpSdrHwIpAddress);
                    Common.LogString(s);
                    if (targetIP != null)
                    {
                        Common.LogString("Using target ip:" + targetIP.ToString());
                    }
                    else
                    {
                        Common.LogString("Where targetip == null");
                    }
                    if (DiscoverRadioOnPort(ref hpsdrd, hostIP, targetIP))
                    {
                        foundRadio = true;

                        // make sure that there is only one entry in the list!
                        if (hpsdrd.Count > 0)
                        {
                            // remove the extra ones that don't match!
                            HPSDRDevice m2 = null;
                            foreach (var m in hpsdrd)
                            {
                                if (m.IPAddress.CompareTo(HpSdrHwIpAddress) == 0)
                                {
                                    m2 = m;
                                }
                            }

                            // clear the list and put our single element in it, if we found it.
                            hpsdrd.Clear();
                            if (m2 != null)
                            {
                                hpsdrd.Add(m2);
                            }
                            else
                            {
                                foundRadio = false;
                            }
                        }
                    }
                }
            }

            LastError = null;
            if (!foundRadio)
            {
                foreach (IPAddress ipa in addrList)
                {
                    if (DiscoverRadioOnPort(ref hpsdrd, ipa, null))
                    {
                        foundRadio = true;
                    }
                }
            }

            ActuallyConnect:
            if (foundRadio) LastError = null;
            if (LastError != null)
            {
                LastError.chuck();
            }

            if (!foundRadio)
            {
                return -1;
            }

            int chosenDevice = 0;
            BoardID = hpsdrd[chosenDevice].deviceType;
            FWCodeVersion = hpsdrd[chosenDevice].codeVersion;
            HpSdrHwIpAddress = hpsdrd[chosenDevice].IPAddress;
            HpSdrHwMacAddress = hpsdrd[chosenDevice].MACAddress;
            EthernetHostIPAddress = hpsdrd[chosenDevice].hostPortIPAddress.ToString();
            EthernetHostPort = hpsdrd[chosenDevice].localPort;
            NumRxs = hpsdrd[chosenDevice].numRxs;

            if (BoardID == HPSDRHW.HermesII)
            {
                if (FWCodeVersion < 103)
                {
                    fwVersionMsg = "Invalid Firmware!\nRequires 10.3 or greater. ";
                    return -101;
                }
            }

            rc = nativeInitMetis(HpSdrHwIpAddress, EthernetHostIPAddress, EthernetHostPort, (int)CurrentRadioProtocol);
            return -rc;
        }



        public static bool fwVersionsChecked = false;
        private static string fwVersionMsg = null;

        public static string getFWVersionErrorMsg()
        {
            return fwVersionMsg;
        }

        public static bool forceFWGood = false;

        private static bool legacyDotDashPTT = false;

        // checks if the firmware versions are consistent - returns false if they are not 
        // and set fwVersionmsg to point to an appropriate message
        private static bool fwVersionsGood()
        {
            return true;
        }

        // returns -101 for firmware version error 
        unsafe public static int StartAudio(RadioBase radio)
        {
            if (initRadio(radio) != 0)
            {
                return 1;
            }

            int result = StartAudioNative();

            if (result == 0 && !fwVersionsChecked)
            {
                if (!fwVersionsGood())
                {
                    result = -101;
                }
                else
                {
                    fwVersionsChecked = true;
                }
            }

            return result;
        }

        unsafe public static int GetDotDashPTT()
        {
            int bits = nativeGetDotDashPTT();
            if (legacyDotDashPTT)  // old style dot and ptt overloaded on 0x1 bit, new style dot on 0x4, ptt on 0x1 
            {
                if ((bits & 0x1) != 0)
                {
                    bits |= 0x4;
                }
            }
            return bits;
        }

        private static double freq_correction_factor = 1.0;
        public static double FreqCorrectionFactor
        {
            get { return freq_correction_factor; }
            set
            {
                freq_correction_factor = value;
                freqCorrectionChanged();
            }
        }

        public static void freqCorrectionChanged()
        {
            if (!FrmMain.FreqCalibrationRunning)    // we can't be applying freq correction when cal is running 
            {
                VFOfreq(0, lastVFOfreq[0][0], 0);
                VFOfreq(1, lastVFOfreq[0][1], 0);
                VFOfreq(2, lastVFOfreq[0][2], 0);
                VFOfreq(3, lastVFOfreq[0][3], 0);
                VFOfreq(0, lastVFOfreq[1][0], 1);
            }
        }

        private static double[][] lastVFOfreq = new double[2][] { new double[] { 0.0, 0.0, 0.0, 0.0 }, new double[] { 0.0 } };
        unsafe public static void VFOfreq(int id, double f, int tx)
        {
            lastVFOfreq[tx][id] = f;
            int f_freq = (int)((f * 1e6) * freq_correction_factor);
            if (Common.Console != null)
            {
                f_freq = Common.Console.ClampFreq(f_freq);
            }
            if (f_freq >= 0)
                if (CurrentRadioProtocol == RadioProtocol.Protocol1)
                    SetVFOfreq(id, f_freq, tx);                  // sending freq Hz to firmware
                else SetVFOfreq(id, Freq2PW(f_freq), tx);   // sending phaseword to firmware
        }

        public static int Freq2PW(int freq)                     // freq to phaseword conversion
        {
            long pw = (long)Math.Pow(2, 32) * freq / 122880000;
            return (int)pw;
        }

        private static double low_freq_offset;
        public static double LowFreqOffset
        {
            get { return low_freq_offset; }
            set
            {
                low_freq_offset = value;
            }
        }

        private static double high_freq_offset;
        public static double HighFreqOffset
        {
            get { return high_freq_offset; }
            set
            {
                high_freq_offset = value;
            }
        }

        // Taken from: KISS Konsole
        public static List<NetworkInterface> foundNics = new List<NetworkInterface>();
        public static List<NicProperties> nicProperties = new List<NicProperties>();
        public static string numberOfIPAdapters;
        public static string Network_interfaces = null;  // holds a list with the description of each Network Adapter
        public static int adapterSelected = 1;           // from Setup form, the number of the Network Adapter to use

        // return the number of *usable* network adapters found on this machine
        // We can get these, then from foundNics
        public static int GetNetworkInterfaces()
        {
            // create a string that contains the name and speed of each Network adapter 
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foundNics.Clear();
            nicProperties.Clear();

            Network_interfaces = "";
            int adapterNumber = 1;

            foreach (var netInterface in nics)
            {
                if ((netInterface.OperationalStatus == OperationalStatus.Up ||
                     netInterface.OperationalStatus == OperationalStatus.Unknown) &&
                    (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                 netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            NicProperties np = new NicProperties();
                            np.ipv4Address = addrInfo.Address;
                            np.ipv4Mask = addrInfo.IPv4Mask;
                            nicProperties.Add(np);
                        }
                    }
                }

                // if the length of the network adapter name is > 31 characters then trim it, if shorter then pad to 31.
                // Need to use fixed width font - Courier New
                string speed = "  " + (netInterface.Speed / 1000000).ToString() + "T";
                if (netInterface.Description.Length > 31)
                {
                    Network_interfaces += adapterNumber++.ToString() + ". " + netInterface.Description.Remove(31) + speed + "\n";
                }
                else
                {
                    Network_interfaces += adapterNumber++.ToString() + ". " + netInterface.Description.PadRight(31, ' ') + speed + "\n";
                }

                foundNics.Add(netInterface);
            }


            System.Console.WriteLine(Network_interfaces);

            // display number of adapters on Setup form
            numberOfIPAdapters = (adapterNumber - 1).ToString();
            return foundNics.Count;
        }

        private static bool DiscoverRadioOnPort(ref List<HPSDRDevice> hpsdrdList, IPAddress HostIP, IPAddress targetIP)
        {
            bool result = false;
            if (SockManager == null)
            {
                SockManager = new WinsockManager();
            }

            try
            {
                // configure a new socket object for each Ethernet port we're scanning
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
            catch (Exception e) // is this message bogus?
            {
                if (e.Message.Contains("WSAStartup"))
                {
                    Debug.Assert(false); // This cannot _still_ be happening, right?
                    SockManager.Dispose();
                    SockManager = new WinsockManager();
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                }
            }

            // Listen to data on this PC's IP address. Allow the program to allocate a free port.
            iep = new IPEndPoint(HostIP, LocalPort);  // was iep = new IPEndPoint(ipa, 0);

            try
            {
                // bind to socket and Port
                socket.Bind(iep);
                //   socket.ReceiveBufferSize = 0xFFFFF;   // no lost frame counts at 192kHz with this setting
                socket.Blocking = true;

                IPEndPoint localEndPoint = (IPEndPoint)socket.LocalEndPoint;
                string s = String.Format("Looking for radio using host adapter IP {0}, port {1}", localEndPoint.Address, localEndPoint.Port);
                Common.LogString(s);

                if (Discovery(ref hpsdrdList, iep, targetIP))
                {
                    result = true;
                }
                else
                {
                    Common.LogString("Failed Discovery() for iep:" + iep.ToString());
                    if (targetIP != null)
                    {
                        Common.LogString("Discovery() failed for targetIP: " + targetIP.ToString());
                    }
                }

            }
            catch (System.Exception ex)
            {
                string s = String.Format("Caught an exception while binding a socket to endpoint {0}.  Exception was: {1} ", iep.ToString(), ex.ToString());
                Common.LogString(s);
                result = false;
            }
            finally
            {
                socket.Close();
                socket = null;
            }

            if (result == false)
            {
                // we do NOT chuck() because we may be called on more than one interface.

                if (LastError == null)
                {
                    if (!Radio.Settings.UseStaticIP)
                    {
                        LastError = ErrInfo.MakeError("Unable to Automatically discover an SDR Radio on your network: " + HostIP + "\n"
                            + "If you know its IP Address, you should enter it in the Network Setup.", -1, "No Radio Found Automatically");
                    }
                    else
                    {
                        LastError = ErrInfo.MakeError("Unable to discover an SDR Radio on your network, even though "
                            + "you are using a static ip: " + Radio.Settings.IPAddress + ", on interface " + HostIP + "\n"
                            + "\n\nCheck the radio is powered on and available on the network.", -1, "No Radio Found Automatically");
                    }
                }
                else
                {
                    LastError.Append("\n\nUnable to automatically discover any SDR Radio on your network: " + HostIP);
                }

            }

            return result;
        }


        private static bool Discovery(ref List<HPSDRDevice> hpsdrdList, IPEndPoint iep, IPAddress targetIP)
        {
            // set up HPSDR discovery packet
            string MAC;
            byte[] DiscoveryPacketP1 = new byte[63];
            //Array.Clear(DiscoveryPacketP1, 0, DiscoveryPacketP1.Length);
            DiscoveryPacketP1[0] = 0xef;
            DiscoveryPacketP1[1] = 0xfe;
            DiscoveryPacketP1[2] = 0x02;
            byte[] DiscoveryPacketP2 = new byte[60];
            //Array.Clear(DiscoveryPacketP2, 0, DiscoveryPacketP2.Length);
            DiscoveryPacketP2[4] = 0x02;

            bool radio_found = false;            // true when we find a radio
            bool static_ip_ok = true;
            int time_out = 0;
            const int TIME_OUT = 50;

            // set socket option so that broadcast is allowed.
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            // need this so we can Broadcast on the socket
            IPEndPoint broadcast;// = new IPEndPoint(IpAddress.Broadcast, DiscoveryPort);
            string receivedIP;   // the IP address Metis obtains; assigned, from DHCP or APIPA (169.254.x.y)

            IPAddress hostPortIPAddress = iep.Address;
            IPAddress hostPortMask = IPAddress.Broadcast;

            // find the subnet mask that goes with this host port
            foreach (NicProperties n in nicProperties)
            {
                if (hostPortIPAddress.Equals(n.ipv4Address))
                {
                    hostPortMask = n.ipv4Mask;
                    break;
                }
            }

            // send every second until we either find a radio or exceed the number of attempts
            // Note: we may find more than one radio! -- we have a collection: hpsdrdList
            int attempts = 0;
            while (!radio_found && time_out <= TIME_OUT) 
            {
                attempts++;
                // send a broadcast to port 1024
                // try target ip address 1 time if static
                if (enableStaticIP && static_ip_ok && targetIP != null)
                {
                    broadcast = new IPEndPoint(targetIP, DiscoveryPort);
                    static_ip_ok = false; // only try this once.
                }
                else
                {
                    // try directed broadcast address
                    broadcast = new IPEndPoint(IPAddressExtensions.GetBroadcastAddress(hostPortIPAddress, hostPortMask), DiscoveryPort);
                }

                if (RadioProtocolSelected == RadioProtocol.Auto || RadioProtocolSelected == RadioProtocol.Protocol1)
                    socket.SendTo(DiscoveryPacketP1, broadcast);
                if (RadioProtocolSelected == RadioProtocol.Auto || RadioProtocolSelected == RadioProtocol.Protocol2)
                    socket.SendTo(DiscoveryPacketP2, broadcast);

                // now listen on send port for any radio
                Common.LogString("Ready to receive UDP broadcast response ...");
                int recv = 0;
                byte[] data = new byte[100];

                bool data_available;
                const int HUNDRED_MS = 100000;

                // await possibly multiple replies, if there are multiple radios on this port,
                // which MIGHT be the 'any' port, 0.0.0.0
                do
                {
                    // Poll the port to see if data is available 
                    data_available = socket.Poll(HUNDRED_MS, SelectMode.SelectRead);
                    int remotePort = 0;
                    if (data_available)
                    {
                        EndPoint remoteEP = new IPEndPoint(IPAddress.None, 0);
                        recv = socket.ReceiveFrom(data, ref remoteEP);                 // recv has number of bytes we received
                                                                                       //string stringData = Encoding.ASCII.GetString(data, 0, recv); // use this to print the received data

                        Common.LogString("RAW Discovery data = " + BitConverter.ToString(data, 0, recv));
                        // see what port this came from at the remote end
                        // IPEndPoint remoteIpEndPoint = socket.RemoteEndPoint as IPEndPoint;
                        // System.Console.WriteLine(" Remote Port # = ", remoteIpEndPoint.Port);

                        string remoteInfo = Convert.ToString(remoteEP);  // see code in DataLoop
                        string[] words = remoteInfo.Split(':');
                        System.Console.Write(words[1]);
                        int.TryParse(words[1], out remotePort);

                        // get MAC address from the payload
                        byte[] mac = { 0, 0, 0, 0, 0, 0 };
                        Array.Copy(data, 5, mac, 0, 6);
                        MAC = BitConverter.ToString(mac);

                        // check for HPSDR frame ID and type 2 (not currently streaming data, which also means 'not yet in use')
                        // changed to filter a proper discovery packet from the radio, even if already in use!  This prevents the need to power-cycle the radio.
                        if (((data[0] == 0xef) && // Protocol-Protocol1 (P1) Busy 
                             (data[1] == 0xfe) &&
                             (data[2] == 0x3)) ||
                            ((data[0] == 0x0) &&  // Protocol-Protocol2 (P2) Busy
                             (data[1] == 0x0) &&
                             (data[2] == 0x0) &&
                             (data[3] == 0x0) &&
                             (data[4] == 0x3)))
                        {
                            LastError = ErrInfo.MakeError("In Discovery (having received some UDP data)", -1, "Radio Busy");
                            return false;
                        }

                        if (((data[0] == 0xef) && // Protocol-Protocol1 (P1)
                             (data[1] == 0xfe) &&
                             (data[2] == 0x2)) ||
                            ((data[0] == 0x0) &&  // Protocol-Protocol2 (P2)
                             (data[1] == 0x0) &&
                             (data[2] == 0x0) &&
                             (data[3] == 0x0) &&
                             (data[4] == 0x2)))
                        {
                            if (data[2] == 0x2) CurrentRadioProtocol = RadioProtocol.Protocol1;
                            else CurrentRadioProtocol = RadioProtocol.Protocol2;
                            freqCorrectionChanged();

                            Common.LogString("Found a radio on the network.  Checking whether it qualifies ...");

                            // get IP address from the IPEndPoint passed to ReceiveFrom.
                            IPEndPoint ripep = (IPEndPoint)remoteEP;
                            IPAddress receivedIPAddr = ripep.Address;
                            receivedIP = receivedIPAddr.ToString();
                            IPEndPoint localEndPoint = (IPEndPoint)socket.LocalEndPoint;
                            Common.LogString(String.Format("Looking for radio using host adapter IP {0}, port {1}", localEndPoint.Address, localEndPoint.Port));

                            Common.LogString("IP from IP Header = " + receivedIP);
                            Common.LogString("MAC address from payload = " + MAC);

                            if (!SameSubnet(receivedIPAddr, hostPortIPAddress, hostPortMask))
                            {
                                // device is NOT on the subnet that this port actually services.  Do NOT add to list!
                                Common.LogString(string.Format("Not on subnet of host adapter! Adapter IP {0}, Adapter mask {1}",
                                    hostPortIPAddress.ToString(), hostPortMask.ToString()));
                            }
                            else if (MAC.Equals("00-00-00-00-00-00"))
                            {
                                Common.LogString("Rejected: contains bogus MAC address of all-zeroes");

                            }
                            else
                            {
                                HPSDRDevice hpsdrd = new HPSDRDevice
                                {
                                    IPAddress = receivedIP,
                                    MACAddress = MAC,
                                    deviceType = CurrentRadioProtocol == RadioProtocol.Protocol1 ? (HPSDRHW)data[10] : (HPSDRHW)data[11],
                                    codeVersion = CurrentRadioProtocol == RadioProtocol.Protocol1 ? data[9] : data[13],
                                    hostPortIPAddress = hostPortIPAddress,
                                    localPort = localEndPoint.Port,
                                    MercuryVersion_0 = data[14],
                                    MercuryVersion_1 = data[15],
                                    MercuryVersion_2 = data[16],
                                    MercuryVersion_3 = data[17],
                                    PennyVersion = data[18],
                                    MetisVersion = data[19],
                                    numRxs = data[20],
                                    protocol = CurrentRadioProtocol,
                                    remoteport = remotePort
                                };

                                // Map P1 device types to P2
                                if (CurrentRadioProtocol == RadioProtocol.Protocol1)
                                {
                                    switch (data[10])
                                    {
                                        case 0:
                                            hpsdrd.deviceType = HPSDRHW.Atlas;
                                            break;
                                        case 1:
                                            hpsdrd.deviceType = HPSDRHW.Hermes;
                                            break;
                                        case 2:
                                            hpsdrd.deviceType = HPSDRHW.HermesII;
                                            break;
                                        case 4:
                                            hpsdrd.deviceType = HPSDRHW.Angelia;
                                            break;
                                        case 5:
                                            hpsdrd.deviceType = HPSDRHW.Orion;
                                            break;
                                        case 6:
                                            hpsdrd.deviceType = HPSDRHW.HermesLite;
                                            break;
                                        case 10:
                                            hpsdrd.deviceType = HPSDRHW.OrionMKII;
                                            break;
                                        default:
                                            hpsdrd.deviceType = HPSDRHW.Unknown;
                                            break;
                                    }
                                }

                                if (targetIP != null)
                                {
                                    if (hpsdrd.IPAddress.CompareTo(targetIP.ToString()) == 0)
                                    {
                                        radio_found = true;
                                        hpsdrdList.Add(hpsdrd);
                                        return true;
                                    }
                                }
                                else
                                {
                                    radio_found = true;
                                    hpsdrdList.Add(hpsdrd);
                                }
                            }
                        }
                    }
                    else
                    {
                        Common.LogString("No data got in UDP Discovery() on iteration: " + attempts.ToString());
                        if ((++time_out) > TIME_OUT)
                        {
                            Common.LogString("Discovery(): Timed out!");
                            attempts++;
                            break;
                        }

                    }
                } while (data_available);
            }

            Common.LogString("Discovery: radio_found = " + radio_found.ToString() + ", after " + attempts.ToString() + " attempts");
            return radio_found;
        }

        /// <summary>
        /// Determines whether the board and hostAdapter IPAddresses are on the same subnet,
        /// using subnetMask to make the determination.  All addresses are IPV4 addresses
        /// </summary>
        /// <param name="board">IP address of the remote device</param>
        /// <param name="hostAdapter">IP address of the ethernet adapter</param>
        /// <param name="subnetMask">subnet mask to use to determine if the above 2 IPAddresses are on the same subnet</param>
        /// <returns>true if same subnet, false otherwise</returns>
        public static bool SameSubnet(IPAddress board, IPAddress hostAdapter, IPAddress subnetMask)
        {
            byte[] boardBytes = board.GetAddressBytes();
            byte[] hostAdapterBytes = hostAdapter.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (boardBytes.Length != hostAdapterBytes.Length)
            {
                return false;
            }
            if (subnetMaskBytes.Length != hostAdapterBytes.Length)
            {
                return false;
            }

            for (int i = 0; i < boardBytes.Length; ++i)
            {
                byte boardByte = (byte)(boardBytes[i] & subnetMaskBytes[i]);
                byte hostAdapterByte = (byte)(hostAdapterBytes[i] & subnetMaskBytes[i]);
                if (boardByte != hostAdapterByte)
                {
                    return false;
                }
            }
            return true;
        }

        // Taken From: https://searchcode.com/codesearch/view/7464800/
        private static IPEndPoint QueryRoutingInterface(
                  Socket socket,
                  IPEndPoint remoteEndPoint)
        {
            SocketAddress address = remoteEndPoint.Serialize();

            byte[] remoteAddrBytes = new byte[address.Size];
            for (int i = 0; i < address.Size; i++)
            {
                remoteAddrBytes[i] = address[i];
            }

            byte[] outBytes = new byte[remoteAddrBytes.Length];
            socket.IOControl(
                        IOControlCode.RoutingInterfaceQuery,
                        remoteAddrBytes,
                        outBytes);
            for (int i = 0; i < address.Size; i++)
            {
                address[i] = outBytes[i];
            }

            EndPoint ep = remoteEndPoint.Create(address);
            return (IPEndPoint)ep;
        }

    }

    // Taken from: http://blogs.msdn.com/b/knom/archive/2008/12/31/ip-address-calculations-with-c-subnetmasks-networks.aspx
    public static class IPAddressExtensions
    {
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }
    }

    public class HPSDRDevice
    {
        public HPSDRHW deviceType;      // which type of device 
        public byte codeVersion;        // reported code version type
        public string IPAddress;        // IPV4 address
        public string MACAddress;       // physical (MAC) address
        public IPAddress hostPortIPAddress;
        public int localPort;
        public byte MercuryVersion_0;
        public byte MercuryVersion_1;
        public byte MercuryVersion_2;
        public byte MercuryVersion_3;
        public byte PennyVersion;
        public byte MetisVersion;
        public byte numRxs;
        public RadioProtocol protocol;
        public int remoteport;
    }

    public class NicProperties
    {
        public IPAddress ipv4Address;
        public IPAddress ipv4Mask;
    }


}
