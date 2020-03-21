using System;

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
}