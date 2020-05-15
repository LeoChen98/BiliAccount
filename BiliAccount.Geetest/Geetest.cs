
namespace BiliAccount.Geetest
{
    /// <summary>
    /// 极验模块入口
    /// </summary>
    public class Geetest
    {
        #region Public Delegates

        /// <summary>
        /// 验证成功委托
        /// </summary>
        /// <param name="challenge">challenge</param>
        /// <param name="key">key</param>
        /// <param name="validate">validate值</param>
        public delegate void Validate_Success_Handler(string challenge, string key, string validate);

        #endregion Public Delegates

        #region Public Events

        /// <summary>
        /// 验证成功事件
        /// </summary>
        public static event Validate_Success_Handler OnValidate_Success;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// 代理端口
        /// </summary>
        public static int Port { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// 开始验证
        /// </summary>
        public static void StartValidate()
        {
            Proxy.Instance.Run();

            Port = Proxy.Instance.Port;
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// 调起验证成功事件
        /// </summary>
        /// <param name="challenge">challenge</param>
        /// <param name="key">key</param>
        /// <param name="validate">validate值</param>
        internal static void Call_Validate_Success(string challenge, string key, string validate)
        {
            OnValidate_Success(challenge, key, validate);
        }

        #endregion Internal Methods
    }
}