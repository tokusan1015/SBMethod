using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用スタック
    /// <summary>
    /// SmallBasic用スタック
    /// </summary>
    [SmallBasicType]
    public static class SBStack
    {
        #region メンバ変数
        /// <summary>
        /// スタック
        /// </summary>
        private static Stack<Primitive> _Stack = new Stack<Primitive>();
        #endregion メンバ変数

        #region 公開メソッド

        #region スタックをクリアする
        /// <summary>
        /// スタックをクリアする
        /// </summary>
        public static void Clear()
        {
            // スタックをクリアする
            _Stack.Clear();
        }
        #endregion スタックをクリアする

        #region スタックに格納されているデータ件数を取得する
        /// <summary>
        /// スタックに格納されているデータ件数を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive Counts()
        {
            // スタックに格納されているデータ件数を取得する
            return (Primitive)_Stack.Count;
        }
        #endregion スタックに格納されているデータ件数を取得する

        #region データを追加する
        /// <summary>
        /// データを追加します
        /// </summary>
        /// <param name="value">データ</param>
        public static void Push(
            Primitive value
            )
        {
            // Pushする
            _Stack.Push(value);
        }
        #endregion データを追加する

        #region データを取得する
        /// <summary>
        /// データを取得します
        /// </summary>
        /// <returns>データ</returns>
        public static Primitive Pop()
        {
            // Popする
            return _Stack.Pop();
        }
        #endregion データを取得する

        #endregion 公開メソッド
    }
    #endregion SmallBasic用スタック
}
