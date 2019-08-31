using System;
using System.Net;

namespace BiliAccount
{
    /// <summary>
    /// 账号信息模板
    /// </summary>
    public class Account
    {
        #region Public Fields

        /// <summary>
        /// Access_Token（使用二维码登录时此项为空）
        /// </summary>
        public string AccessToken;

        /// <summary>
        /// Cookies集合实例
        /// </summary>
        public CookieCollection Cookies;

        /// <summary>
        /// csrf_token
        /// </summary>
        public string CsrfToken;

        /// <summary>
        /// Access_Token有效期（使用二维码登录时此项为空）
        /// </summary>
        public DateTime Expires_AccessToken;

        /// <summary>
        /// Cookies有效期
        /// </summary>
        public DateTime Expires_Cookies;

        /// <summary>
        /// 指示是否登录成功
        /// </summary>
        public LoginStatusEnum LoginStatus = LoginStatusEnum.None;

        /// <summary>
        /// 密码（使用二维码登录时此项为空）
        /// </summary>
        public string Password;

        /// <summary>
        /// Refresh_Token（使用二维码登录时此项为空）
        /// </summary>
        public string RefreshToken;

        /// <summary>
        /// Cookies字符串
        /// </summary>
        public string strCookies;

        /// <summary>
        /// 用户数字id
        /// </summary>
        public string Uid;

        /// <summary>
        /// 用户名（使用二维码登录时此项为空）
        /// </summary>
        public string UserName;

        #endregion Public Fields

        #region Public Enums

        /// <summary>
        /// 登录状态枚举
        /// </summary>
        public enum LoginStatusEnum
        {
            /// <summary>
            /// 未登录
            /// </summary>
            None,

            /// <summary>
            /// 二维码登录
            /// </summary>
            ByQrCode,

            /// <summary>
            /// 密码登录
            /// </summary>
            ByPassword
        }

        #endregion Public Enums
    }
}