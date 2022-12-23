using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thetis;

namespace CoolSDR
{
    public class ErrInfo
    {
        public string where;
        public int what;
        public string why;
        public Exception exception;

        public ErrInfo(string where, int what, string why, Exception ex = null)
        {
            this.where = where;
            this.what = what;
            this.why = why;
        }
        public static ErrInfo MakeError(string where, int what, string why, Exception ex = null)
        {
            var e = new ErrInfo(where, what, why, ex);
            Common.LogString(e.ToString());
            return e;
        }

        public void Append(string s)
        {
            why += s;
        }

        public void chuck()
        {
            throw new Exception(this.ToString());
        }

        public void Throw()
        {
            throw new Exception(this.ToString());
        }

        public new string ToString()
        {
            string s = "Error Info: what: " + what.ToString() + "\n\nwhere: " + where + "\n\nwhy: " + why.ToString();
            if (exception != null)
            {
                s += "exception information:\n" + exception.Message;
                s += "exception call stack:\n" + exception.StackTrace;
            }
            return s;
        }
    }

}
