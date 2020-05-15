
using System;

namespace BiliAccount.Geetest.Controls.Winforms
{

    partial class GeetestBrowserWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeetestBrowserWindow));
            this.browser = new BiliAccount.Geetest.Controls.Winforms.GeetestBrowser();
            this.CodeInputMask = new System.Windows.Forms.Panel();
            this.Btn_Confirm = new System.Windows.Forms.Button();
            this.Btn_Refresh = new System.Windows.Forms.Button();
            this.Btn_Resend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_Code = new System.Windows.Forms.TextBox();
            this.CodeInputMask.SuspendLayout();
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(283, 310);
            this.browser.TabIndex = 0;
            this.browser.UIDelay = 3000;
            // 
            // CodeInputMask
            // 
            this.CodeInputMask.Controls.Add(this.TB_Code);
            this.CodeInputMask.Controls.Add(this.label1);
            this.CodeInputMask.Controls.Add(this.Btn_Resend);
            this.CodeInputMask.Controls.Add(this.Btn_Confirm);
            this.CodeInputMask.Location = new System.Drawing.Point(0, 0);
            this.CodeInputMask.Name = "CodeInputMask";
            this.CodeInputMask.Size = new System.Drawing.Size(283, 310);
            this.CodeInputMask.TabIndex = 1;
            this.CodeInputMask.Visible = false;
            // 
            // Btn_Confirm
            // 
            this.Btn_Confirm.Location = new System.Drawing.Point(60, 161);
            this.Btn_Confirm.Name = "Btn_Confirm";
            this.Btn_Confirm.Size = new System.Drawing.Size(75, 23);
            this.Btn_Confirm.TabIndex = 1;
            this.Btn_Confirm.Text = "确定";
            this.Btn_Confirm.UseVisualStyleBackColor = true;
            this.Btn_Confirm.Click += new System.EventHandler(this.Btn_Confirm_Click);
            // 
            // Btn_Refresh
            // 
            this.Btn_Refresh.Location = new System.Drawing.Point(205, 284);
            this.Btn_Refresh.Name = "Btn_Refresh";
            this.Btn_Refresh.Size = new System.Drawing.Size(75, 23);
            this.Btn_Refresh.TabIndex = 0;
            this.Btn_Refresh.Text = "刷新";
            this.Btn_Refresh.UseVisualStyleBackColor = true;
            this.Btn_Refresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // Btn_Resend
            // 
            this.Btn_Resend.Location = new System.Drawing.Point(141, 161);
            this.Btn_Resend.Name = "Btn_Resend";
            this.Btn_Resend.Size = new System.Drawing.Size(75, 23);
            this.Btn_Resend.TabIndex = 1;
            this.Btn_Resend.Text = "重新发送";
            this.Btn_Resend.UseVisualStyleBackColor = true;
            this.Btn_Resend.Click += new System.EventHandler(this.Btn_Resend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请输入验证码";
            // 
            // TB_Code
            // 
            this.TB_Code.Location = new System.Drawing.Point(60, 110);
            this.TB_Code.Name = "TB_Code";
            this.TB_Code.Size = new System.Drawing.Size(156, 21);
            this.TB_Code.TabIndex = 3;
            this.TB_Code.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GeetestBrowserWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 311);
            this.Controls.Add(this.CodeInputMask);
            this.Controls.Add(this.browser);
            this.Controls.Add(this.Btn_Refresh);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeetestBrowserWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "极验验证";
            this.CodeInputMask.ResumeLayout(false);
            this.CodeInputMask.PerformLayout();
            this.ResumeLayout(false);

        }

        private GeetestBrowser browser;
        private System.Windows.Forms.Panel CodeInputMask;
        private System.Windows.Forms.Button Btn_Refresh;
        private System.Windows.Forms.Button Btn_Confirm;
        private System.Windows.Forms.Button Btn_Resend;
        private System.Windows.Forms.TextBox TB_Code;
        private System.Windows.Forms.Label label1;
    }


}
