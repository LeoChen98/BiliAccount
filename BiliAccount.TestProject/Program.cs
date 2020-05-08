using BiliAccount.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

#pragma warning disable CS0649

namespace BiliAccount.TestProject
{
    internal class Program
    {
        #region Private Methods

        private static void ByQRCode_QrCodeRefresh(Bitmap newQrCode)
        {
            newQrCode.Save("tmp.jpg");
        }

        private static void ByQRCode_QrCodeStatus_Changed(ByQRCode.QrCodeStatus status, Account account = null)
        {
            Console.WriteLine(status);
            if (account != null) Console.WriteLine(var_dump(account));
        }

        private static void Main(string[] args)
        {
            //Console.WriteLine($"测试版本：{AssemblyName.GetAssemblyName("BiliAccount.dll").Version.ToString()}");
            //Console.WriteLine("账号");
            //string username = Console.ReadLine();
            //Console.WriteLine("密码");
            //string pwd = Console.ReadLine();
            //Account account = ByPassword.LoginByPassword(username, pwd);
            //Console.WriteLine(var_dump(account));
            //if (account.LoginStatus == Account.LoginStatusEnum.NeedSafeVerify) Process.Start(account.Url);

            ByQRCode.QrCodeStatus_Changed += ByQRCode_QrCodeStatus_Changed;
            ByQRCode.QrCodeRefresh += ByQRCode_QrCodeRefresh;
            ByQRCode.LoginByQrCode("#FF66CCFF", "#00000000", true).Save("tmp.png");

            //string token = Console.ReadLine();
            //Console.WriteLine(BiliAccount.Linq.ByPassword.IsTokenAvailable(token));

            //Console.WriteLine("手机号");
            //string tel = Console.ReadLine();
            //BySMS.SendSMS(tel);
            //Console.WriteLine("验证码");
            //string code = Console.ReadLine();
            //Account account = BySMS.Login(code, tel);
            //Console.WriteLine(var_dump(account));

            //Console.WriteLine(var_dump(ByPassword.SSO(account.AccessToken)));

            //account.Expires_AccessToken = (DateTime)ByPassword.RefreshToken("76b3e1cb9b0d35a80f8c444dcdcb1a21", "bcb656a377362db92487929a7f258d21");
            //Console.WriteLine(var_dump(account));

            //ByPassword.Revoke(ref account);

            Application.Run();
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

        #endregion Private Methods

        #region Private Classes

        /// <summary>
        /// 初始化数据模板
        /// </summary>
        private class Init_DataTemplete
        {
            #region Public Fields

            public string appkey;
            public string appsecret;
            public string build;
            public string message;
            public string version;

            #endregion Public Fields
        }

        #endregion Private Classes
    }
}