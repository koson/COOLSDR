using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.ExceptionServices;
using System.Reflection.Metadata.Ecma335;

// If you had an ideal way to do SDR, how would you write it?

namespace TheAppYouWantToWrite
{

    enum RadioModel
    {
        First = 0,
        None = First,
        HermesLite,
        Anan100
    }
    internal interface ISdrRadio
    {
        IConnection ConnectionObject();
        RadioModel Model { get; }
        abstract List<IVFO> Vfos { get; }
        abstract IVFO Vfo(VFOID which);
        abstract IAudioDevice AudioIn { get; set; }
        abstract IAudioDevice AudioOut { get; set; }
        abstract void Start();
        abstract void Stop();
       abstract AudioState AudioRunState { get; set; }
    }

    public enum VFOID
    {
        A, B
    }
    public interface IVFO
    {
        public  VFOID ID { get; set; }
        public  double Freq { get; set; }
        public  string ToString();
        public bool Active { get; set; }

    }

    public class VFO : IVFO
    {
        public VFO(VFOID id)
        {
            ID = id;
        }
        public double Freq { get; set; } = 1.933;
        public VFOID ID { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            var ret = "VFO " + ID.ToString() + " has freq:\t" + Freq;
            ret += Active ? " and is active" : " and is not active";
            return ret;
        }
    }

    internal interface IConnection
    {
        void Connect(string ip);
        string LocalIPAddress { get; protected set; }
        string RemoteIPAddress { get; protected set; }
        int LocalPort { get; protected set; }
        int RemotePort { get; protected set; }
        string ToString();
    }

    class Connection : IConnection
    {
        public override string ToString()
        {
            // "Hello, {0}! Today is {1}, it's {2:HH:mm} now.", name, date.DayOfWeek, date
            string ret = string.Format("\nLocal IP:\t {0}:{1}", LocalIPAddress, LocalPort);
            ret += "\n";
            ret += string.Format("Remote IP:\t {0}:{1}", RemoteIPAddress, RemotePort);
            return ret;
        }

        public void Connect(string ip)
        {
            Console.WriteLine("SdrBase connecting to: {0} ... ", ip);
            Thread.Sleep(500);
            Console.WriteLine("SdrBase succesfully connected to: {0} ", ip);

            LocalIPAddress = ip;
            RemoteIPAddress = "192.168.1.99";
            LocalPort = 178097;
            RemotePort = 576998;
        }

        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public string LocalIPAddress { get; set; }
        public string RemoteIPAddress { get; set; }
    }

    public interface IAudioDevice
    {
        string Name { get; set; }
    }

    public class BaseAudioDevice : IAudioDevice
    {
        protected BaseAudioDevice(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
    public class AudioInputDevice : BaseAudioDevice
    {
        public AudioInputDevice(string name) : base(name) { }

    }

    public class AudioOutputDevice : BaseAudioDevice
    {
        public AudioOutputDevice(string name) : base(name) { }

    }

    public enum AudioState
    {
        Stopped, Running
    }
    internal abstract class SdrBase : ISdrRadio
    {
        private Connection connection;
        public readonly List<IVFO> VFOs = new();
        public void Connect(string ip)
        {
            var VfoA = new VFO(VFOID.A);
            var VfoB = new VFO(VFOID.B);
            VFOs.Add(VfoA);
            VFOs.Add(VfoB);
            VfoA.Freq = 1.933;
            VfoB.Freq = 3.615;
            VfoA.Active = true; VfoB.Active = false;
            connection = new Connection();
            connection.Connect(ip);
        }

        public IConnection ConnectionObject() { return connection; }

        protected RadioModel model;
        public RadioModel Model { get => RadioModelID(); set { model = value; } }

        public List<IVFO> Vfos => VFOs;

        public IAudioDevice AudioIn { get; set; }
        public IAudioDevice AudioOut { get; set; }

        public abstract RadioModel RadioModelID();


        IVFO ISdrRadio.Vfo(VFOID which)
        {
            if (which == VFOID.A) return VFOs[0];
            return VFOs[1];
        }

        public AudioState AudioRunState { get;set; }
        public void Start()
        {
            AudioRunState = AudioState.Running;
        }

        public void Stop()
        {
            AudioRunState = AudioState.Stopped;
        }
    }


    internal class HermesLiteRadio : SdrBase, ISdrRadio
    {
        public HermesLiteRadio(string IP)
        {
            Connect(IP);
        }

        public override RadioModel RadioModelID()
        {
            return RadioModel.HermesLite;
        }
    }

    internal class Program
    {
        static void Main()
        {
            ISdrRadio radio = new HermesLiteRadio("192.168.1.10");
            IConnection conn = radio.ConnectionObject();
            Console.WriteLine("\nI have a radio, connected to: {0}", conn.ToString());
            Console.WriteLine("Radio model is:\t {0}", radio.Model.ToString());
            Console.WriteLine("\nVFOs available: ");
            foreach (var vfo in radio.Vfos)
            {
                Console.WriteLine(vfo.ToString());
            }

            radio.AudioIn = new AudioInputDevice("A posh microphone");
            radio.AudioOut = new AudioOutputDevice("Some nifty speakers");

            Console.WriteLine("Audio Input:\t{0}", radio.AudioIn.Name);
            Console.WriteLine("Audio output:\t{0}", radio.AudioOut.Name);
            Console.WriteLine ("Audio state is:\t{0}", radio.AudioRunState.ToString());
            radio.Start();

            Console.WriteLine("Audio state is:\t{0}", radio.AudioRunState.ToString());




            //Console.WriteLine("\n\nApp finished. Hit enter to close the window.");
           // Console.ReadLine();
        }
    }
}
