using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMethod
{
    #region 未初期化例外クラス
    /// <summary>
    /// 未初期化例外
    /// </summary>
    public class NotExecuteInitializeException : System.Exception
    {
        #region 固定値
        /// <summary>
        /// 標準メッセージ
        /// </summary>
        private const string DEFAULT_MESSAGE = "Not execute Initialize";
        #endregion 固定値

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NotExecuteInitializeException()
            : base(DEFAULT_MESSAGE)
        {
            // 標準メッセージを渡すコンストラクタ
        }
        /// <summary>
        /// NotExecuteInitializeException
        /// </summary>
        /// <param name="message"></param>
        public NotExecuteInitializeException(
            string message
            ) : base (
                message: message
                )
        {
            // メッセージ文字列を渡すコンストラクタ
        }
        /// <summary>
        /// メッセージ文字列と発生済み例外オブジェクトを渡すコンストラクタ
        /// </summary>
        /// <param name="message">メッセージ文字列</param>
        /// <param name="innerException">発生済み例外オブジェクト</param>
        public NotExecuteInitializeException(
            string message, 
            Exception innerException
            )
            : base(
                  message: message,
                  innerException: innerException
                  )
        {
            // メッセージ文字列と発生済み例外オブジェクトを渡すコンストラクタ
        }
        #endregion コンストラクタ
    }
    #endregion 未初期化例外クラス

    #region 不明Case例外クラス
    /// <summary>
    /// 不明Case例外クラス
    /// </summary>
    public class UnknownSwitchCaseException : System.Exception
    {
        #region 固定値
        /// <summary>
        /// 標準メッセージ
        /// </summary>
        private const string DEFAULT_MESSAGE = "Unknown switch case";
        #endregion 固定値

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnknownSwitchCaseException()
            : base(DEFAULT_MESSAGE)
        {
            // 標準メッセージを渡すコンストラクタ
        }
        /// <summary>
        /// UnknownSwitchCaseException
        /// </summary>
        /// <param name="message"></param>
        public UnknownSwitchCaseException(
            string message
            ) : base(
                message: message
                )
        {
            // メッセージ文字列を渡すコンストラクタ
        }
        /// <summary>
        /// メッセージ文字列と発生済み例外オブジェクトを渡すコンストラクタ
        /// </summary>
        /// <param name="message">メッセージ文字列</param>
        /// <param name="innerException">発生済み例外オブジェクト</param>
        public UnknownSwitchCaseException(
            string message,
            Exception innerException
            )
            : base(
                  message: message,
                  innerException: innerException
                  )
        {
            // メッセージ文字列と発生済み例外オブジェクトを渡すコンストラクタ
        }
        #endregion コンストラクタ
    }
    #endregion 未初期化例外クラス
}
