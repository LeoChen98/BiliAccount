using System;

namespace BiliAccount.Exceptions
{
#pragma warning disable CS1591

    /// <summary>
    /// 发送验证码信息错误
    /// </summary>
    public class SMS_Send_Exception : Exception
    {
        #region Public Fields

        public int code;

        #endregion Public Fields

        #region Public Constructors

        public SMS_Send_Exception(int code, string message) : base(message)
        {
            this.code = code;
        }

        #endregion Public Constructors
    }
}