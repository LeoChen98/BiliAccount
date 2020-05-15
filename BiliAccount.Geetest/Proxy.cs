using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace BiliAccount.Geetest
{
    /// <summary>
    /// 用于截获数据的代理
    /// </summary>
    internal class Proxy
    {
        #region Private Fields

        private static Proxy _proxy;
        private string challenge, key;
        private ProxyServer proxyServer;

        private Regex reg_challenge = new Regex("&challenge=(.+?)&");
        private Regex reg_key = new Regex("\"key\":\"(.+?)\"");
        private Regex reg_validate = new Regex("\"validate\": \"(.+?)\"");

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        /// 初始化代理
        /// </summary>
        private Proxy()
        {
            proxyServer = new ProxyServer(false);

            proxyServer.BeforeRequest += OnRequest;
            proxyServer.BeforeResponse += OnResponse;

            Port = GetFirstAvailablePort();
            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, Port, true);

            proxyServer.AddEndPoint(explicitEndPoint);
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// 代理单例实例
        /// </summary>
        public static Proxy Instance
        {
            get
            {
                if (_proxy == null) _proxy = new Proxy();
                return _proxy;
            }
        }

        /// <summary>
        /// 指示代理是否正在运行
        /// </summary>
        public bool IsRuning { get; private set; } = false;

        /// <summary>
        /// 代理端口
        /// </summary>
        public int Port { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// 启动代理
        /// </summary>
        public void Run()
        {
            if (IsRuning)
            {
                proxyServer.Start();
                IsRuning = true;
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 获取第一个可用的端口号
        /// </summary>
        /// <returns></returns>
        private static int GetFirstAvailablePort(int BEGIN_PORT = 8000, int MAX_PORT = 10000)
        {
            for (int i = BEGIN_PORT; i < MAX_PORT; i++)
            {
                if (PortIsAvailable(i)) return i;
            }

            return -1;
        }

        /// <summary>
        /// 检查指定端口是否已用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static bool PortIsAvailable(int port)
        {
            bool isAvailable = true;

            IList portUsed = PortIsUsed();

            foreach (int p in portUsed)
            {
                if (p == port)
                {
                    isAvailable = false; break;
                }
            }

            return isAvailable;
        }

        /// <summary>
        /// 获取操作系统已用的端口号
        /// </summary>
        /// <returns></returns>
        private static IList PortIsUsed()
        {
            //获取本地计算机的网络连接和通信统计数据的信息
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //返回本地计算机上的所有Tcp监听程序
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();

            //返回本地计算机上的所有UDP监听程序
            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();

            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            IList allPorts = new ArrayList();
            foreach (IPEndPoint ep in ipsTCP) allPorts.Add(ep.Port);
            foreach (IPEndPoint ep in ipsUDP) allPorts.Add(ep.Port);
            foreach (TcpConnectionInformation conn in tcpConnInfoArray) allPorts.Add(conn.LocalEndPoint.Port);

            return allPorts;
        }

        private Task OnRequest(object sender, SessionEventArgs e)
        {
            return Task.Run(() =>
            {
                if (e.HttpClient.Request.Url.Contains("/x/safecenter/sms/send"))
                {
                    Thread.Sleep(2000);
                    e.Ok("");
                }

                challenge = reg_challenge.IsMatch(e.HttpClient.Request.Url) ? reg_challenge.Match(e.HttpClient.Request.Url).Groups[1].Value : challenge;
            });
        }

        private async Task OnResponse(object sender, SessionEventArgs e)
        {
            if (e.HttpClient.Response.StatusCode == 200)
            {
                string body = await e.GetResponseBodyAsString();
                if (e.HttpClient.Request.Url.Contains("/web/captcha/combine"))
                {
                    key = reg_key.IsMatch(body) ? reg_key.Match(body).Groups[1].Value : null;
                }

                if (e.HttpClient.Request.Url.Contains("/ajax.php"))
                {
                    string validate = reg_validate.IsMatch(body) ? reg_validate.Match(body).Groups[1].Value : null;

                    if (!string.IsNullOrEmpty(validate))
                    {
                        Geetest.Call_Validate_Success(challenge, key, validate);
                        proxyServer.Stop();
                        IsRuning = false;
                    }
                }
            }
        }

        #endregion Private Methods
    }
}