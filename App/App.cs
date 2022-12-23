using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace CoolSDR.Class
{
    internal class App
    {

        static readonly public string DecimalSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static string m_d;

        public static string DataFolderPath
        {
            get => GetDataFolder();
        }
        public static string GetDataFolder()
        {
            if (!string.IsNullOrEmpty(m_d))
            {
                return m_d;
            }
            string appData = Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData);

            string theFolder = Path.Combine(appData, Application.ProductName);

            if (Environment.Is64BitProcess)
            {
                theFolder = Path.Combine(theFolder, "64-bit");
            }

#if (DEBUG)
            theFolder = Path.Combine(theFolder, "Debug");
#endif
            if (!Directory.Exists(theFolder))
            {
                Directory.CreateDirectory(theFolder);
            }

            if (!theFolder.EndsWith("\\"))
                theFolder += "\\"; // compatibility for earlier versions who always expect '\' on the end.

            m_d = theFolder;
            return m_d;
        }

        public static string Name
        {
            get { return Assembly.GetExecutingAssembly().GetName().Name; }
        }
    }
}
