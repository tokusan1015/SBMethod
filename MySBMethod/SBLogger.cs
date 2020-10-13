using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用ロガー
    /// <summary>
    /// SmallBasic用ロガー
    /// 出力ファイル名は ".\LastLog.txt"
    /// </summary>
    [SmallBasicType]
    public static class SBLogger
    {
        #region メンバ変数
        /// <summary>
        /// デバッグリスト
        /// </summary>
        private static List<string> _WriteList = new List<string>();
        /// <summary>
        /// ロガー開始フラグ
        /// </summary>
        private static bool _StartLogger { get; set; } = false;
        #endregion メンバ変数

        #region 公開メソッド
        /// <summary>
        /// デバッグ開始
        /// </summary>
        public static void Start()
        {
            // デバッグリストクリア
            _WriteList.Clear();
            // ロガー開始
            _StartLogger = true;
        }
        /// <summary>
        /// データ記録
        /// </summary>
        /// <param name="data">データ</param>
        public static void Write(
            Primitive data
            )
        {
            // ロガーが開始している場合
            if (_StartLogger == true)
            {
                // データを追加する
                _WriteList.Add(data.ToString());
            }
        }
        /// <summary>
        /// ログを文字列配列で取得します
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetWriteLog()
        {
            return _WriteList.ConvertDimStringToPrimitive();
        }
        /// <summary>
        /// ログをファイルに出力します
        /// デフォルトファイル名は".\LastLog.txt"
        /// </summary>
        public static void Output()
        {
            // ログをデフォルトファイル名で出力する
            _Output();
        }
        /// <summary>
        /// ログをファイルに出力します
        /// </summary>
        /// <param name="filePathName">ファイルパス名</param>
        public static void Output2(
            Primitive filePathName
            )
        {
            // ログを指定ファイルパス名で出力する
            _Output(filePathName.ToString());
        }
        #endregion 公開メソッド

        #region メソッド

        #region ロガー終了
        /// <summary>
        /// ロガー終了
        /// 最終ログを出力する
        /// デフォルト出力ファイル名は ".\LastLog.txt"
        /// </summary>
        private static void _Output(
            string filePathName = @".\LastLog.txt"
            )
        {
            // ロガーが開始している場合
            if (_StartLogger == true)
            {
                // ロガー停止
                _StartLogger = false;

                // ファイルを出力する
                _WriteList.OutputList(filePathName: filePathName);
            }
        }
        #endregion ロガー終了

        #endregion メソッド
    }
    #endregion SmallBasic用ロガー
}
