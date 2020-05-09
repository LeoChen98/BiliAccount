using CefSharp;
using CefSharp.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace BiliAccount.Geetest.Controls
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
        public void Refresh()
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

            settings.CefCommandLineArgs.Add("proxy-server", $"127.0.0.1:{Geetest.Port}");
            settings.CefCommandLineArgs.Add("enable-media-stream", "1");
            settings.CefCommandLineArgs.Add("disable-gpu", "1");

            Cef.Initialize(settings);

            //Cef.DoMessageLoopWork();

            browser = new ChromiumWebBrowser() { Width = content.Width, Height = content.Height };

            content.Children.Add(browser);

            browser.Margin = new Thickness(0, 0, 0, 0);
            browser.Address = url;
            browser.ExecuteScriptAsyncWhenPageLoaded("document.body.style.display='none';" +
                                    "setTimeout(document.getElementById('app').style.display = 'none',100);" +
                                    "setTimeout(function(){" +
                                    "document.getElementById('send-btn').click();" +
                                    "document.getElementsByClassName('geetest_panel_ghost')[0].remove();" +
                                    "document.body.style.display='block'" +
                                    $"}},{UIDelay}); ", false);
        }

        #endregion Public Methods

        #region Private Methods

        private void Geetest_OnValidate_Success(string challenge, string key, string validate)
        {
            try
            {
                content.Children.Remove(browser);
                browser.Visibility = Visibility.Hidden;
                browser.Dispose();
                Cef.Shutdown();
            }
            catch { }

            OnVaildate_Success(challenge, key, validate);
        }

        #endregion Private Methods
    }
}