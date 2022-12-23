using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thetis;

namespace CoolSDR.Class
{
    internal class WinsockManager : IDisposable
    {
        private static bool winsock_initted = false;
        private const int IP_SUCCESS = 0;
        private const short VERSION = 2;
        public ErrInfo LastError
        {
            get;
            private set;
        }
        public WinsockManager()
        {
            if (winsock_initted) return;
            int rc = Win32.WSAStartup(VERSION, out Win32.WSAData data);
            Debug.Assert(rc == 0); // There is really no reason why this should fail unless the system is unstable.
            if (rc != IP_SUCCESS)
            {
                System.Console.WriteLine(data.description);
                LastError = ErrInfo.MakeError("WSA Startup failed", rc, Thetis.NetworkIO.GetLastSockError());
                LastError.chuck();

            }
            winsock_initted = rc == 0;

            return;
        }

        ~WinsockManager()
        {
            CloseWinsock();
        }

        public static int CloseWinsock()
        {
            if (winsock_initted)
            {
                int retval = Win32.PublicWSACleanup();
                Debug.Assert(retval == 0);
                winsock_initted = false;
                return retval;
            }
            return 0;
        }

        public void Dispose()
        {
            CloseWinsock();
        }
    }
}
