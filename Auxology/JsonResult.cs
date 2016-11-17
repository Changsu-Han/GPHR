using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auxology
{
    class JsonResult
    {
        private bool bRes;
        private string sMsg;

        public bool res
        {
            get
            {
                return bRes;
            }
            set
            {
                bRes = value;
            }
        }

        public string msg
        {
            get
            {
                return sMsg;
            }
            set
            {
                Console.WriteLine(msg);
                sMsg = value;
            }
        }
    }
}
