using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolComponents
{
    using System.Security.Permissions;
    using System.Windows.Forms;
    // using System.Windows.Media;

    public class IPControl : TextBox
    {
        public IPControl() : base()
        {

        }

        protected override CreateParams CreateParams
        {
            get
            {
                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
                CreateParams cp = base.CreateParams;
                cp.ClassName = "SysIPAddress32";
                return cp;
            }
        }


    }
}
