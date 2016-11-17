using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auxology
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {/*
            string OriStr = "Hello World";
            string EncStr = Encrypt(OriStr);
            string DecStr = Decrypt(EncStr);

            Console.WriteLine("원본 데이터==========================");
            Console.WriteLine(OriStr);

            Console.WriteLine("암호화 데이터=========================");
            Console.WriteLine(EncStr);

            Console.WriteLine("복호화 데이터=========================");
            Console.WriteLine(DecStr);*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            
        }

        //암호화 키.  8글자로 이루어짐.
        
    }
}
