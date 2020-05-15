using CefSharp;
using CefSharp.Internals;
using CefSharp.WinForms;
using System.Windows.Forms;

namespace BiliAccount.Geetest.Controls.Winforms
{
    /// <summary>
    /// DataViewer.xaml 的交互逻辑
    /// </summary>
    public partial class GeetestBrowser : UserControl
    {
        #region Private Fields

        private ChromiumWebBrowser browser;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// 初始化GeetestBrowser
        /// </summary>
        public GeetestBrowser()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Delegates

        /// <summary>
        /// 验证成功委托
        /// </summary>
        /// <param name="challenge">challenge</param>
        /// <param name="key">key</param>
        /// <param name="validate">validate值</param>
        public delegate void Vaildate_Success_Handler(string challenge, string key, string validate);

        #endregion Public Delegates

        #region Public Events

        /// <summary>
        /// 验证成功事件
        /// </summary>
        public event Vaildate_Success_Handler OnVaildate_Success;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// 指示加载完毕后多少毫秒显示验证（建议不低于1500）,在验证启动前修改有效，否则不会应用新的值。
        /// </summary>
        public int UIDelay { get; set; } = 3000;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// 刷新验证页
        /// </summary>
        public new void Refresh()
        {
            browser.Reload();
        }

        /// <summary>
        /// 启动验证
        /// </summary>
        /// <param name="url">验证地址</param>
        public void StartVaildate(string url)
        {
            Geetest.OnValidate_Success += Geetest_OnValidate_Success;

            Geetest.StartValidate();

            var settings = new CefSettings();

            settings.CefCommandLineArgs.Add("--proxy-server", $"127.0.0.1:{Geetest.Port}");
            settings.CefCommandLineArgs.Add("enable-media-stream", "1");
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("--ignore-urlfetcher-cert-requests", "1");
            settings.CefCommandLineArgs.Add("--ignore-certificate-errors", "1");
            settings.CefCommandLineArgs.Add("--disable-web-security", "1"); 
            Cef.Initialize(settings);

            browser = new ChromiumWebBrowser(url) { Width = Width, Height = Height };

            Controls.Add(browser);

            browser.Left = 0;
            browser.Top = 0;
            browser.FrameLoadEnd += Browser_FrameLoadEnd;
        }


        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            browser.GetMainFrame().ExecuteJavaScriptAsync("document.body.style.display='none';" +
                                    "setTimeout(document.getElementById('app').style.display = 'none',100);" +
                                    "setTimeout(function(){" +
                                    "document.getElementById('send-btn').click();" +
                                    "document.getElementsByClassName('geetest_panel_ghost')[0].remove();" +
                                    "document.body.style.display='block'" +
                                    $"}},{UIDelay}); ");
        }

        #endregion Public Methods

        #region Private Methods

        private void Geetest_OnValidate_Success(string challenge, string key, string validate)
        {
            try
            {
                Controls.Remove(browser);
                browser.Visible = false;
                browser.Dispose();
                Cef.Shutdown();
            }
            catch { }

            OnVaildate_Success(challenge, key, validate);
        }

        #endregion Private Methods


    }
}