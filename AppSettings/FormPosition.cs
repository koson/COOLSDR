
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoolSDR.Class
{
    public class FormPosition
    {
        private bool IsVisibleOnAnyScreen(Rectangle rect)
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


        public FormPosition(Form f)
        {
            Restore(f);
        }
        public void Restore(Form f)
        {
            try
            {
                f.WindowState = FormWindowState.Normal;
                f.StartPosition = FormStartPosition.WindowsDefaultBounds;
                var settings = AppSettings.MySettings;
                var bag = settings.DynamicProperties;
                var ws = settings.Get<FormWindowState>(f.Name + "_WindowState");
                var wp = settings.Get<Point>(f.Name + "_WindowLocation");
                var rb = settings.Get<Rectangle>(f.Name + "_WindowBounds");

                if ((wp != Point.Empty ||
                    IsVisibleOnAnyScreen(rb)) || !rb.IsEmpty)
                {

                    f.StartPosition = FormStartPosition.Manual;
                    f.DesktopBounds = rb;
                    f.WindowState = ws;

                }
                else
                {
                    // this resets the upper left corner of the window to windows standards
                    f.StartPosition = FormStartPosition.WindowsDefaultLocation;
                }
            }
            catch (Exception e)
            {
                Thetis.Common.LogException(e);
            }
        }

        // call me from _Closing
        public void Save(Form f)
        {
            var settings = AppSettings.MySettings;
            var bag = settings.DynamicProperties;

            switch (f.WindowState)
            {
                case FormWindowState.Normal:
                case FormWindowState.Maximized:
                    settings.Set<FormWindowState>(f.WindowState, f.Name + "_WindowState");
                    break;

                default:
                    settings.Set<FormWindowState>(FormWindowState.Normal, f.Name + "_WindowState");
                    break;
            }

            if (f.WindowState == FormWindowState.Maximized)
            {
                settings.Set<Rectangle>(f.RestoreBounds, f.Name + "_WindowBounds");
            }
            else
            {
                settings.Set<Rectangle>(f.DesktopBounds, f.Name + "_WindowBounds");
            }
            settings.Set<Point>(f.Location, f.Name + "_WindowLocation");
            settings.Save();
        }
    }
}
