using BiliAccount.Linq;
using System.Windows;

namespace BiliAccount.Geetest.Controls
{
    /// <summary>
    /// DataViewer.xaml 的交互逻辑
    /// </summary>
    public partial class GeetestBrowserWindow : Window
    {
        #region Private Fields

        private Account account;

        private string tmp_token;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// 初始化GeetestBrowser
        /// </summary>
        public GeetestBrowserWindow(ref Account account)
        {
            InitializeComponent();

            this.account = account;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// 显示验证窗体
        /// </summary>
        /// <param name="url">验证url</param>
        /// <param name="account">账号实例</param>
        /// <returns>运行结果</returns>
        public static bool? ShowValidateWindowDialog(string url, ref Account account)
        {
            GeetestBrowserWindow window = new GeetestBrowserWindow(ref account);

            window.browser.OnVaildate_Success += window.Browser_OnVaildate_Success;
            window.tmp_token = Device_Verify.Url2TmpToken(url);
            window.browser.StartVaildate(url);

            return window.ShowDialog();
        }

        #endregion Public Methods

        #region Private Methods

        private void Browser_OnVaildate_Success(string challenge, string key, string validate)
        {
            try
            {
                Device_Verify.Send_SMS(challenge, key, tmp_token, validate);

                Dispatcher.Invoke(() =>
                {
                    CodeInputMask.Visibility = Visibility.Visible;
                    browser.Visibility = Visibility.Hidden;
                });
            }
            catch (Exceptions.SMS_Send_Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"短信发送失败：{ex.Message}");
                Dispatcher.Invoke(() =>
                {
                    CodeInputMask.Visibility = Visibility.Hidden;
                    browser.Refresh();
                });
            }
        }

        private void Btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Device_Verify.GetAccount(Device_Verify.Verify(TB_Code.Text, tmp_token), ref account);

                DialogResult = true;
                Close();
            }
            catch (Exceptions.Verify_Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"验证码错误：{ex.Message}");
            }
            catch (Exceptions.GetAccount_Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"获取账号信息错误：{ex.Message}");
            }
        }

        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            browser.Refresh();
        }

        #endregion Private Methods
    }
}