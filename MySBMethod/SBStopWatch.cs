using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用ストップウォッチ
    /// <summary>
    /// SmallBasic用ストップウォッチ
    /// </summary>
    [SmallBasicType]
    public static class SBStopWatch 
    {
        #region メンバ変数
        /// <summary>
        /// ストップウォッチ
        /// </summary>
        private static System.Diagnostics.Stopwatch StopWatch = new System.Diagnostics.Stopwatch();
        #endregion メンバ変数

        #region 公開メソッド

        #region ストップウォッチ開始
        /// <summary>
        /// ストップウォッチを開始します
        /// </summary>
        public static void Start()
        {
            // ストップウォッチ開始
            StopWatch.Start();
        }
        #endregion ストップウォッチ開始

        #region ストップウォッチ停止
        /// <summary>
        /// ストップウォッチを停止します
        /// </summary>
        public static void Stop()
        {
            StopWatch.Stop();
        }
        #endregion ストップウォッチ停止

        #region ストップウォッチリセット
        /// <summary>
        /// ストップウォッチをリセットします
        /// </summary>
        public static void Reset()
        {
            StopWatch.Reset();
        }
        #endregion ストップウォッチリセット

        #region 経過時間
        /// <summary>
        /// 経過時間を文字列で取得します
        /// </summary>
        /// <returns>文字列</returns>
        public static Primitive ElapsedString()
        {
            // 経過時間
            Primitive ret = (decimal)StopWatch.ElapsedMilliseconds;
            return StopWatch.Elapsed.ToString();
        }
        /// <summary>
        /// 経過時間(ミリ秒)を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive ElapsedMilliseconds()
        {
            // 経過時間
            return (decimal)StopWatch.ElapsedMilliseconds;
        }
        /// <summary>
        /// 経過時間(Ticks)を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive ElapsedTicks()
        {
            // 経過時間
            return (decimal)StopWatch.ElapsedTicks;
        }
        #endregion 経過時間

        #endregion 公開メソッド
    }
    #endregion SmallBasic用ストップウォッチ
}
