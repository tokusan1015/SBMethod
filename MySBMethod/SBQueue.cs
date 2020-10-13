using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用キュー
    /// <summary>
    /// SmallBasic用キュー
    /// </summary>
    [SmallBasicType]
    public static class SBQueue
    {
        #region メンバ変数
        /// <summary>
        /// キュー
        /// </summary>
        private static Queue<Primitive> _Queue = new Queue<Primitive>();
        #endregion メンバ変数

        #region 公開メソッド

        #region キューをクリアする
        /// <summary>
        /// キューをクリアします
        /// </summary>
        public static void Clear()
        {
            // キューをクリアする
            _Queue.Clear();
        }
        #endregion キューをクリアする

        #region キューに格納されているデータ件数を取得する
        /// <summary>
        /// キューに格納されているデータ件数を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive Counts()
        {
            // キューに格納されているデータ件数を取得する
            return (Primitive)_Queue.Count();
        }
        #endregion キューに格納されているデータ件数を取得する

        #region キューにデータを追加する
        /// <summary>
        /// キューにデータを追加します
        /// </summary>
        /// <param name="value">データ</param>
        public static void Enqueue(
            Primitive value
            )
        {
            // キューにデータを追加する
            _Queue.Enqueue(value);
        }
        #endregion キューにデータを追加する

        #region キューからデータを取得する
        /// <summary>
        /// キューからデータを取得します
        /// </summary>
        /// <returns>データ</returns>
        public static Primitive Dequeue()
        {

            // キューからデータを取得する
            return _Queue.Dequeue();
        }
        #endregion キューからデータを取得する

        #endregion 公開メソッド
    }
    #endregion SmallBasic用キュー
}
