using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Message
    {
        public string frmUser;
        public string toTheuser;
        public string msg;
        public string imgDATA; //BASE64 String encoded Bitmap
        public string[] DATAtxtfile;

        public Message()
        {
            frmUser = null;
            toTheuser = null;
            msg = null;
            imgDATA = null;
            DATAtxtfile = null;
        }
    }
}
