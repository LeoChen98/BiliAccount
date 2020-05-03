using BiliAccount.Linq;
using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BiliAccount.Core31TestProject
{
    class Program
    {
        static bool q = false;
        [STAThread]
        static void Main(string[] args)
        {
            Task t = Task.Run(() =>
            {
                ByQRCode.QrCodeStatus_Changed += ByQRCode_QrCodeStatus_Changed;
                ByQRCode.QrCodeRefresh += ByQRCode_QrCodeRefresh;
                ByQRCode.LoginByQrCode().Save("tmp.jpg");
                while (true)
                {

                }
            });
            t.Wait();
        }

        private static void ByQRCode_QrCodeRefresh(Bitmap newQrCode)
        {
            newQrCode.Save("tmp.jpg");
        }

        private static void ByQRCode_QrCodeStatus_Changed(ByQRCode.QrCodeStatus status, Account account = null)
        {
            Console.WriteLine(status);
            if (account != null) { Console.WriteLine(var_dump(account)); q = true; }
        }

        ///<summary>
        /// equiv of PHP's var dump for an object’s properties because i cbf writing all the properties out.
        ///</summary>
        ///<param name="info"></param>
        private static string var_dump(object info)
        {
            StringBuilder sb = new StringBuilder();

            Type t = info.GetType();
            FieldInfo[] props = t.GetFields();
            sb.AppendFormat("{0,-25} {1}", "Name", "Value");
            sb.AppendLine();

            foreach (FieldInfo prop in props)
            {
                try
                {
                    if (prop.GetValue(info) != null)
                    {
                        sb.AppendFormat("{0,-25} {1}", prop.Name, prop.GetValue(info).ToString());
                        sb.AppendLine();
                    }
                }
                catch { }
            }

            return sb.ToString();
        }
    }
}
