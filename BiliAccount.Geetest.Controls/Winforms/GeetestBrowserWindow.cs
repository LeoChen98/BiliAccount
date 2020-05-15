using BiliAccount.Linq;
using System;
using System.Windows.Forms;

namespace BiliAccount.Geetest.Controls.Winforms
{
    /// <summary>
    /// DataViewer.xaml 的交互逻辑
    /// </summary>
    public partial class GeetestBrowserWindow : Form
    {
        #region Private Fields

        private Account account;
        private string tmp_token, challenge, key, validate;

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
        public static DialogResult ShowValidateWindowDialog(string url, ref Account account)
        {
            GeetestBrowserWindow window = new GeetestBrowserWindow(ref account);

            window.browser.OnVaildate_Success += window.Browser_OnVaildate_Success;
            window.tmp_token = Device_Verify.Url2TmpToken(url);
            window.browser.StartVaildate(url);

            return window.ShowDialog();
        }

        /// <summary>
        /// 显示验证窗体
        /// </summary>
        /// <param name="account">账号实例</param>
        /// <returns>运行结果</returns>
        public static DialogResult ShowValidateWindowDialog(ref Account account)
        {
            return ShowValidateWindowDialog(account.Url, ref account);
        }

        #endregion Public Methods

        #region Private Methods

        private void Browser_OnVaildate_Success(string challenge, string key, string validate)
        {
            try
            {
                Device_Verify.Send_SMS(challenge, key, tmp_token, validate);
                this.challenge = challenge;
                this.key = key;
                this.validate = validate;

                Invoke(new Action(() =>
                {
                    CodeInputMask.Visible = true;
                    browser.Visible = false;
                }));
            }
            catch (Exceptions.SMS_Send_Exception ex)
            {
                MessageBox.Show($"短信发送失败：{ex.Message}");

                Invoke(new Action(() =>
                {
                    CodeInputMask.Visible = false;
                    browser.Refresh();
                }));

                throw ex;
            }
        }

        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                Device_Verify.GetAccount(Device_Verify.Verify(TB_Code.Text, tmp_token), ref account);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exceptions.Verify_Exception ex)
            {
                MessageBox.Show($"验证码错误：{ex.Message}");
            }
            catch (Exceptions.GetAccount_Exception ex)
            {
                MessageBox.Show($"获取账号信息错误：{ex.Message}");
            }
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            browser.Refresh();
        }

        private void Btn_Resend_Click(object sender, EventArgs e)
        {
            Device_Verify.Send_SMS(challenge, key, tmp_token, validate);
        }


        #endregion Private Methods
    }
}