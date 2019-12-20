using System;
using System.Drawing;

#if NETFRAMEWORK
using System.Windows;
#endif

#if !NETSTANDARD2_0 && !NETCORE3_0
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

namespace BiliAccount.Linq
{
    /// <summary>
    /// 账号密码登录模式
    /// </summary>
    public class ByPassword
    {
        #region Public Methods

        /// <summary>
        /// 检查token可用性
        /// </summary>
        /// <param name="access_token">token</param>
        /// <returns>是否可用</returns>
        public static bool IsTokenAvailable(string access_token)
        {
            return Core.ByPassword.IsTokenAvailable(access_token);
        }

        /// <summary>
        /// 用账号密码登录
        /// </summary>
        /// <param name="username">用户名（邮箱/手机）</param>
        /// <param name="password">密码（明文）</param>
        /// <returns>账号信息实例</returns>
        public static Account LoginByPassword(string username, string password)
        {
            Core.ByPassword.Init();
            Account account = new Account()
            {
                UserName = username,
                Password = password
            };
            Core.ByPassword.GetKey(out string hash, out string key, out account.Cookies);
            account.EncryptedPassword = Core.ByPassword.EncryptPwd(password, key, hash);
            Core.ByPassword.DoLogin(ref account);
            return account;
        }

        /// <summary>
        /// 带验证码的账号密码登录
        /// </summary>
        /// <param name="Captcha">验证码</param>
        /// <param name="account">账号信息实例</param>
        public static void LoginByPasswordWithCaptcha(string Captcha, ref Account account)
        {
            Core.ByPassword.DoLoginWithCatpcha(Captcha, ref account);
        }

        /// <summary>
        /// token续期
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="refresh_token"></param>
        /// <returns>到期时间</returns>
        public static DateTime? RefreshToken(string access_token, string refresh_token)
        {
            return Core.ByPassword.RefreshToken(access_token, refresh_token);
        }

        /// <summary>
        /// SSO
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns>[0]=>(string)strCookies,[1]=>(string)csrf_token,[2]=>(DateTime)Expiress,[3]=>(CookieCollection)Cookies</returns>
        public static object[] SSO(string access_token)
        {
            return Core.ByPassword.SSO(access_token);
        }

        #endregion Public Methods
    }

    /// <summary>
    /// 二维码登录
    /// </summary>
    public class ByQRCode
    {
        #region Public Delegates

        /// <summary>
        /// 二维码刷新处理程序
        /// </summary>
        /// <param name="newQrCode">新二维码</param>
        public delegate void QrCodeRefresh_Handle(Bitmap newQrCode);

        /// <summary>
        /// 二维码登录状态变更处理程序
        /// </summary>
        /// <param name="status">二维码状态</param>
        /// <param name="account">登录成功时有值，账号信息实例</param>
        public delegate void QrCodeStatus_Changed_Handle(QrCodeStatus status, Account account = null);

        #endregion Public Delegates

        #region Public Events

        /// <summary>
        /// 二维码刷新事件
        /// </summary>
        public static event QrCodeRefresh_Handle QrCodeRefresh;

        /// <summary>
        /// 二维码登录状态变更事件
        /// </summary>
        public static event QrCodeStatus_Changed_Handle QrCodeStatus_Changed;

        #endregion Public Events

        #region Public Enums

        /// <summary>
        /// 二维码登录状态枚举
        /// </summary>
        public enum QrCodeStatus
        {
            #region Public Fields
            /// <summary>
            /// 等待扫描
            /// </summary>
            Wating,
            /// <summary>
            /// 等待确认
            /// </summary>
            Scaned,
            /// <summary>
            /// 登录成功
            /// </summary>
            Success

            #endregion Public Fields
        }

        #endregion Public Enums

        #region Public Methods

        /// <summary>
        /// 取消登录
        /// </summary>
        public static void CancelLogin()
        {
            Core.ByQrCode.CancelLogin();
        }

#if !NETSTANDARD2_0 && !NETCORE3_0
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="hObject">对象指针</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 获取WPF显示用的ImageSource
        /// </summary>
        /// <param name="qrCodeImage">二维码图片Bitmap</param>
        /// <returns>ImageSource</returns>
        public static ImageSource GetQrCodeImageSource(Bitmap qrCodeImage)
        {
            IntPtr myImagePtr = qrCodeImage.GetHbitmap();     //创建GDI对象，返回指针

            BitmapSource imgsource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(myImagePtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());  //创建imgSource

            DeleteObject(myImagePtr);

            return imgsource;
        }
#endif



        /// <summary>
        /// 用二维码登录
        /// </summary>
        /// <returns>二维码图片实例</returns>
        public static Bitmap LoginByQrCode()
        {
            return Core.ByQrCode.GetQrcode();
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// 调起二维码刷新
        /// </summary>
        /// <param name="newQrCode">新二维码</param>
        internal static void RaiseQrCodeRefresh(Bitmap newQrCode)
        {
            QrCodeRefresh?.Invoke(newQrCode);
        }

        /// <summary>
        /// 调起二维码登录状态变更
        /// </summary>
        /// <param name="status">二维码状态</param>
        /// <param name="account">登录成功时有值，账号信息实例</param>
        internal static void RaiseQrCodeStatus_Changed(QrCodeStatus status, Account account = null)
        {
            QrCodeStatus_Changed?.Invoke(status, account);
        }

        #endregion Internal Methods
    }
}