﻿using MSScriptControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace BiliAccount
{
    internal class Core
    {
        #region Internal Classes

        /// <summary>
        /// 通过密码登录
        /// </summary>
        internal class ByPassword
        {
            #region Public Properties

            /// <summary>
            /// Appkey
            /// </summary>
            public static string Appkey { get; private set; }

            /// <summary>
            /// AppSecret
            /// </summary>
            public static string Appsecret { get; private set; }

            /// <summary>
            /// Build
            /// </summary>
            public static string Build { get; private set; }

            /// <summary>
            /// UA
            /// </summary>
            public static string User_Agent { get; private set; }

            #endregion Public Properties

            #region Private Properties

            /// <summary>
            /// Unix时间戳
            /// </summary>
            private static long TimeStamp
            {
                get
                {
                    return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                }
            }

            #endregion Private Properties

            #region Public Methods

            /// <summary>
            /// 登录
            /// </summary>
            /// <param name="username">用户名（邮箱/手机号）</param>
            /// <param name="password">加密后密码</param>
            /// <returns>账号信息实例</returns>
            public static void DoLogin(string username, string password, ref Account account)
            {
                string parm = "appkey=" + Appkey + "&build=" + Build + "&mobi_app=android&password=" + password + "&platform=android&ts=" + TimeStamp + "&username=" + username;
                parm += "&sign=" + GetSign(parm);
                string str = Http.PostBody("http://passport.bilibili.com/api/v2/oauth2/login", parm);
                if (!string.IsNullOrEmpty(str))
                {
                    DoLogin_DataTemplete obj = (new JavaScriptSerializer()).Deserialize<DoLogin_DataTemplete>(str);

                    if (obj.code == 0)
                    {
                        account.Uid = obj.data.token_info.mid;
                        account.AccessToken = obj.data.token_info.access_token;
                        account.RefreshToken = obj.data.token_info.refresh_token;
                        account.Expires_AccessToken = DateTime.Now.AddSeconds(obj.data.token_info.expires_in);

                        account.Cookies = new CookieCollection();
                        foreach (DoLogin_DataTemplete.Data_Templete.Cookie_Info_Templete.Cookie_Templete i in obj.data.cookie_info.cookies)
                        {
                            account.strCookies += i.name + "=" + i.value + "; ";
                            account.Cookies.Add(new Cookie(i.name, i.value));
                            account.Expires_Cookies = DateTime.Parse("1970-01-01 08:00:00").AddSeconds(i.expires);

                            if (i.name == "bili_jct")
                                account.CsrfToken = i.value;
                        }
                        account.strCookies = account.strCookies.Substring(0, account.strCookies.Length - 2);
                        account.LoginStatus = Account.LoginStatusEnum.ByPassword;
                    }
                }
            }

            /// <summary>
            /// 密码加密
            /// </summary>
            /// <param name="password">未加密密码</param>
            /// <param name="key">key</param>
            /// <param name="hash">hash</param>
            /// <returns>加密后密码</returns>
            public static string EncryptPwd(string password, string key, string hash)
            {
                return UrlEncode(ExecuteScript("getpwd(\"" + key.Replace("\n", "").Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "") + "\",\"" + hash + "\",\"" + password + "\")", Properties.Resources.js_pwd));
            }

            /// <summary>
            /// 获取key
            /// </summary>
            /// <param name="hash">输出hash</param>
            /// <param name="key">输出key</param>
            public static void GetKey(out string hash, out string key)
            {
                string parm = "appkey=" + Appkey;
                parm += "&sign=" + GetSign(parm);
                string str = Http.PostBody("http://passport.bilibili.com/api/oauth2/getKey", parm);
                if (!string.IsNullOrEmpty(str))
                {
                    GetKey_DataTemplete obj = (new JavaScriptSerializer()).Deserialize<GetKey_DataTemplete>(str);
                    if (obj.code == 0)
                    {
                        hash = obj.data.hash;
                        key = obj.data.key;
                        return;
                    }
                }

                //获取失败
                hash = "";
                key = "";
            }

            /// <summary>
            /// 初始化登录模块
            /// </summary>
            public static void Init()
            {
                string str = Http.GetBody("http://ctrl.zhangbudademao.com/118/Init.json");

                if (!string.IsNullOrEmpty(str))
                {
                    Init_DataTemplete obj = (new JavaScriptSerializer()).Deserialize<Init_DataTemplete>(str);
                    Appkey = obj.appkey;
                    Appsecret = obj.appsecret;
                    Build = obj.build;
                    User_Agent = obj.user_agent;
                }
                else
                {
                    throw new Exception("Login module initialization failure.");
                }
            }

            /// <summary>
            /// 检查token可用性
            /// </summary>
            /// <param name="access_token">token</param>
            /// <returns>是否可用</returns>
            public static bool IsTokenAvailable(string access_token)
            {
                string parm = "access_token=" + access_token + "&appkey=" + Appkey + "&ts=" + TimeStamp;
                parm += "&sign=" + GetSign(parm);
                string str = Http.GetBody("https://passport.bilibili.com/api/oauth2/info?" + parm);

                if (!string.IsNullOrEmpty(str))
                {
                    IsTokenAvailable_DataTemplete obj = (new JavaScriptSerializer()).Deserialize<IsTokenAvailable_DataTemplete>(str);

                    if (obj.code == 0 && obj.data.expiress_in > 0)
                    {
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// token续期
            /// </summary>
            /// <param name="access_token"></param>
            /// <param name="refresh_token"></param>
            /// <returns>到期时间</returns>
            public static DateTime? RefreshToken(string access_token, string refresh_token)
            {
                string parm = "access_token=" + access_token + "&appkey=" + Appkey + "&refresh_token=" + refresh_token + "&ts=" + TimeStamp;
                parm += "&sign=" + GetSign(parm);
                string str = Http.PostBody("https://passport.bilibili.com/api/oauth2/refreshToken", parm);

                if (!string.IsNullOrEmpty(str))
                {
                    RefreshToken_DataTemplete obj = (new JavaScriptSerializer()).Deserialize<RefreshToken_DataTemplete>(str);

                    if (obj.code == 0)
                    {
                        return DateTime.Parse("1970-01-01 08:00:00").AddSeconds(obj.ts + obj.data.expiress_in);
                    }
                }
                return null;
            }

            /// <summary>
            /// SSO
            /// </summary>
            /// <param name="access_token"></param>
            /// <returns>[0]=>(string)strCookies,[1]=>(string)csrf_token,[2]=>(DateTime)Expiress,[3]=>(CookieCollection)Cookies</returns>
            public static object[] SSO(string access_token)
            {
                string parm = "access_key=" + access_token + "&appkey=" + Appkey + "&build=5470400&gourl=" + UrlEncode("https://www.bilibili.com/") + "&mobi_app=android&platform=android&ts=" + TimeStamp;
                parm += "&sign=" + GetSign(parm);

                HttpWebRequest req = null;
                HttpWebResponse rep = null;
                string cookies = "", csrf_token = "";
                CookieCollection cookiesC = new CookieCollection();
                DateTime expires = new DateTime();
                try
                {
                    req = (HttpWebRequest)WebRequest.Create("https://passport.bilibili.com/api/login/sso?" + parm);
                    req.AllowAutoRedirect = false;
                    req.UserAgent = "Mozilla/5.0 BiliDroid/5.46.0 (bbcallen@gmail.com) os/android model/MI 9 mobi_app/android build/5460400 channel/master innerVer/5460400 osVer/10 network/2";
                    rep = (HttpWebResponse)req.GetResponse();

                    foreach (string i in rep.Headers.GetValues("Set-Cookie"))
                    {
                        string[] tmp = i.Split(';');
                        string[] tmp2 = tmp[0].Split('=');

                        cookies += tmp[0] + "; ";
                        cookiesC.Add(new Cookie(tmp2[0], tmp2[1]));
                        expires = DateTime.Parse(tmp[2].Split('=')[1]);

                        if (tmp2[0] == "bili_jct")
                            csrf_token = tmp2[1];
                    }
                    cookies = cookies.Substring(0, cookies.Length - 2);
                }
                finally
                {
                    if (rep != null) rep.Close();
                    if (req != null) req.Abort();
                }
                return new object[] { cookies, csrf_token, expires, cookiesC };
            }

            public static string UrlEncode(string str)
            {
                StringBuilder builder = new StringBuilder();
                foreach (char c in str)
                {
                    string tmp = HttpUtility.UrlEncode(c.ToString());
                    if (tmp.Length > 1)
                    {
                        builder.Append(tmp.ToUpper());
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
                return builder.ToString();
            }

            #endregion Public Methods

            #region Private Methods

            /// <summary>
            /// 运行js
            /// </summary>
            /// <param name="sExperssion"></param>
            /// <param name="sCode"></param>
            /// <returns></returns>
            private static string ExecuteScript(string sExperssion, string sCode)
            {
                ScriptControl scriptControl = new ScriptControl
                {
                    UseSafeSubset = true,
                    Language = "JScript"
                };
                scriptControl.AddCode(sCode);
                return scriptControl.Eval(sExperssion);
            }

            /// <summary>
            /// 获取文件md5
            /// </summary>
            /// <param name="fileName">文件路径</param>
            /// <returns>md5</returns>
            private static string GetMD5(string str)
            {
                try
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] retVal = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
                catch
                {
                    return "";
                }
            }

            /// <summary>
            /// 获取参数签名
            /// </summary>
            /// <param name="strReq">请求参数</param>
            /// <returns>签名</returns>
            private static string GetSign(string strReq)
            {
                return GetMD5(strReq + Appsecret);
            }

            #endregion Private Methods

            #region Private Classes

            /// <summary>
            /// 登录数据模板
            /// </summary>
            private class DoLogin_DataTemplete
            {
                #region Public Fields

                public int code;
                public Data_Templete data;

                #endregion Public Fields

                #region Public Classes

                public class Data_Templete
                {
                    #region Public Fields

                    public Cookie_Info_Templete cookie_info;
                    public Token_Info_Templete token_info;

                    #endregion Public Fields

                    #region Public Classes

                    public class Cookie_Info_Templete
                    {
                        #region Public Fields

                        public Cookie_Templete[] cookies;

                        #endregion Public Fields

                        #region Public Classes

                        public class Cookie_Templete
                        {
                            #region Public Fields

                            public long expires;
                            public string name;
                            public string value;

                            #endregion Public Fields
                        }

                        #endregion Public Classes
                    }

                    public class Token_Info_Templete
                    {
                        #region Public Fields

                        public string access_token;
                        public long expires_in;
                        public string mid;
                        public string refresh_token;

                        #endregion Public Fields
                    }

                    #endregion Public Classes
                }

                #endregion Public Classes
            }

            /// <summary>
            /// GetKey返回值的数据模板
            /// </summary>
            private class GetKey_DataTemplete
            {
                #region Public Fields

                public int code;
                public Data_Templete data;

                #endregion Public Fields

                #region Public Classes

                public class Data_Templete
                {
                    #region Public Fields

                    public string hash;
                    public string key;

                    #endregion Public Fields
                }

                #endregion Public Classes
            }

            /// <summary>
            /// 初始化数据模板
            /// </summary>
            private class Init_DataTemplete
            {
                #region Public Fields

                public string appkey;
                public string appsecret;
                public string build;
                public string user_agent;

                #endregion Public Fields
            }

            /// <summary>
            /// 检查token可用性数据模板
            /// </summary>
            private class IsTokenAvailable_DataTemplete
            {
                #region Public Fields

                public int code;
                public Data_Templete data;
                public long ts;

                #endregion Public Fields

                #region Public Classes

                public class Data_Templete
                {
                    #region Public Fields

                    public long expiress_in;

                    #endregion Public Fields
                }

                #endregion Public Classes
            }

            /// <summary>
            /// token续期数据模板
            /// </summary>
            private class RefreshToken_DataTemplete
            {
                #region Public Fields

                public int code;
                public Data_Templete data;
                public long ts;

                #endregion Public Fields

                #region Public Classes

                public class Data_Templete
                {
                    #region Public Fields

                    public long expiress_in;

                    #endregion Public Fields
                }

                #endregion Public Classes
            }

            #endregion Private Classes
        }

        /// <summary>
        /// 通过二维码登录
        /// </summary>
        internal class ByQrCode
        {
            #region Private Fields

            /// <summary>
            /// 状态监视器
            /// </summary>
            private static Timer Monitor;

            /// <summary>
            /// 刷新监视器
            /// </summary>
            private static Timer Refresher;

            #endregion Private Fields

            #region Public Methods

            /// <summary>
            /// 获取登陆二维码并显示
            /// </summary>
            public static Bitmap GetQrcode()
            {
                Bitmap qrCodeImage = null;
            re:
                //获取二维码要包含的url
                string str = Http.GetBody("https://passport.bilibili.com/qrcode/getLoginUrl", null, "https://passport.bilibili.com/login");
                if (!string.IsNullOrEmpty(str))
                {
                    GetQrcode_DataTemplete obj = (new JavaScriptSerializer()).Deserialize<GetQrcode_DataTemplete>(str);

                    if (obj.code == 0)
                    {
                        // 生成二维码的内容
                        string strCode = obj.data.url;
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(strCode, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrcode = new QRCode(qrCodeData);

                        //生成二维码位图
                        qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 0, 6, false);

                        Monitor = new Timer(MonitorCallback, obj.data.oauthKey, 1000, 1000);
                        Refresher = new Timer(RefresherCallback, null, 180000, Timeout.Infinite);
                    }
                }
                else goto re;

                return qrCodeImage;
            }

            #endregion Public Methods

            #region Private Methods

            /// <summary>
            /// 状态监视器回调
            /// </summary>
            /// <param name="o">oauthKey</param>
            private static void MonitorCallback(object o)
            {
                string oauthKey = o.ToString();

                string str = Http.PostBody("https://passport.bilibili.com/qrcode/getLoginInfo", "oauthKey=" + oauthKey + "&gourl=https%3A%2F%2Fwww.bilibili.com%2F", null, "application/x-www-form-urlencoded; charset=UTF-8", "https://passport.bilibili.com/login");
                if (!string.IsNullOrEmpty(str))
                {
                    MonitorCallBack_Templete obj = (new JavaScriptSerializer()).Deserialize<MonitorCallBack_Templete>(str);

                    if (obj.status)
                    {
                        //关闭监视器
                        Monitor.Change(Timeout.Infinite, Timeout.Infinite);
                        Refresher.Change(Timeout.Infinite, Timeout.Infinite);

                        Account account = new Account();

                        string Querystring = Regex.Split((obj.data as Dictionary<string, object>)["url"].ToString(), "\\?")[1];
                        string[] KeyValuePair = Regex.Split(Querystring, "&");
                        account.Cookies = new CookieCollection();
                        for (int i = 0; i < KeyValuePair.Length - 1; i++)
                        {
                            string[] tmp = Regex.Split(KeyValuePair[i], "=");
                            switch (tmp[0])
                            {
                                case "bili_jct":
                                    account.CsrfToken = tmp[1];
                                    account.strCookies += KeyValuePair[i] + "; ";
                                    account.Cookies.Add(new Cookie(tmp[0], tmp[1]));
                                    break;

                                case "DedeUserID":
                                    account.Uid = tmp[1];
                                    account.strCookies += KeyValuePair[i] + "; ";
                                    account.Cookies.Add(new Cookie(tmp[0], tmp[1]));
                                    break;

                                case "Expires":
                                    account.Expires_Cookies = DateTime.Now.AddSeconds(double.Parse(tmp[1]));
                                    break;

                                case "gourl":

                                    break;

                                default:
                                    account.strCookies += KeyValuePair[i] + "; ";
                                    account.Cookies.Add(new Cookie(tmp[0], tmp[1]));
                                    break;
                            }
                        }
                        account.strCookies = account.strCookies.Substring(0, account.strCookies.Length - 2);
                        account.LoginStatus = Account.LoginStatusEnum.ByQrCode;
                        Linq.ByQRCode.RaiseQrCodeStatus_Changed(Linq.ByQRCode.QrCodeStatus.Success, account);
                    }
                    else
                    {
                        switch ((int)obj.data)
                        {
                            case -4://未扫描
                                Linq.ByQRCode.RaiseQrCodeStatus_Changed(Linq.ByQRCode.QrCodeStatus.Wating);
                                break;

                            case -5://已扫描
                                Linq.ByQRCode.RaiseQrCodeStatus_Changed(Linq.ByQRCode.QrCodeStatus.Scaned);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            /// <summary>
            /// 刷新监视器回调
            /// </summary>
            /// <param name="state"></param>
            private static void RefresherCallback(object state)
            {
                Linq.ByQRCode.RaiseQrCodeRefresh(GetQrcode());
            }

            #endregion Private Methods

            #region Private Classes

            /// <summary>
            /// 获取二维码的数据模板
            /// </summary>
            private class GetQrcode_DataTemplete
            {
                #region Public Fields

                public int code;
                public Data_Templete data;

                #endregion Public Fields

                #region Public Classes

                public class Data_Templete
                {
                    #region Public Fields

                    public string oauthKey;
                    public string url;

                    #endregion Public Fields
                }

                #endregion Public Classes
            }

            /// <summary>
            /// 状态监视器回调数据模板（子）
            /// </summary>
            private class MonitorCallBack_DataTemplete
            {
                #region Public Fields

                public string url;

                #endregion Public Fields
            }

            /// <summary>
            /// 状态监视器回调数据模板（父）
            /// </summary>
            private class MonitorCallBack_Templete
            {
                #region Public Fields

                public object data;
                public bool status;

                #endregion Public Fields
            }

            #endregion Private Classes
        }

        #endregion Internal Classes
    }
}