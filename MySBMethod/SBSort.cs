using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用ソート
    /// <summary>
    /// SmallBasic用ソート
    /// </summary>
    [SmallBasicType]
    public static class SBSort
    {
        #region 公開メソッド

        #region 配列を文字列として昇順ソートする
        /// <summary>
        /// 配列を文字列として昇順ソートし、
        /// 結果を文字列配列で取得します
        /// </summary>
        /// <param name="datas">配列</param>
        /// <returns>文字列配列</returns>
        public static Primitive SortString(
            Primitive datas
            )
        {
            // Listに変換する
            List<string> list = PrimitiveUtility.ConvertPrimitiveToDimString(datas).ToList();

            // 昇順ソート実行
            List<string> sorted = list.OrderBy(x => x.ToString()).ToList();

            // Primitiveに変換する
            return sorted.ConvertDimStringToPrimitive();
        }
        #endregion 配列を文字列として昇順ソートする

        #region 配列を文字列として降順ソートする
        /// <summary>
        /// 配列を文字列として降順ソートし、
        /// 結果を文字列配列で取得します
        /// </summary>
        /// <param name="datas">配列</param>
        /// <returns>文字列配列</returns>
        public static Primitive SortStringDescending(
            Primitive datas
            )
        {
            // Listに変換する
            List<string> list = PrimitiveUtility.ConvertPrimitiveToDimString(datas).ToList();

            // 降順ソート実行
            List<string> sorted = list.OrderByDescending(x => x.ToString()).ToList();

            // Primitiveに変換する
            return sorted.ConvertDimStringToPrimitive();
        }
        #endregion 配列を文字列として降順ソートする

        #endregion 公開メソッド
    }
    #endregion SmallBasic用ソート
}
