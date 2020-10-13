using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用絞込
    /// <summary>
    /// SmallBasic用絞込
    /// </summary>
    [SmallBasicType]
    public static class SBWhere
    {
        #region 列挙型
        /// <summary>
        /// 比較番号
        /// </summary>
        private enum Compare : int
        {
            /// <summary>
            /// ≧
            /// </summary>
            GreaterThan = 0,
            /// <summary>
            /// ＞
            /// </summary>
            Greater,
            /// <summary>
            /// ＝
            /// </summary>
            Equal,
            /// <summary>
            /// ＜＞
            /// </summary>
            Noterual,
            /// <summary>
            /// ＜
            /// </summary>
            Less,
            /// <summary>
            /// ≦
            /// </summary>
            LessThan,
        }
        #endregion 列挙型

        #region 公開メソッド
        /// <summary>
        /// 比較名の文字列配列を取得します
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetCompareNameList()
        {
            return typeof(Compare).GetPrimitiveEnumNameList();
        }
        /// <summary>
        /// 比較名が存在するか検証し結果を取得します
        /// 存在する場合trueが返ります
        /// </summary>
        /// <param name="compareName">比較名</param>
        /// <returns>BOOL</returns>
        public static Primitive ExistCompareName(
            Primitive compareName
            )
        {
            return typeof(Compare).ExistPrimitiveTextInEnumType(text: compareName);
        }
        /// <summary>
        /// 比較名の比較番号を取得します
        /// 存在しない場合は-1が返ります
        /// </summary>
        /// <param name="compareName">比較名</param>
        /// <returns>数値</returns>
        public static Primitive GetCompareNo(
            Primitive compareName
            )
        {
            // 比較名が存在するか検証する
            if (ExistCompareName(compareName: compareName))
                // 比較名から比較値を取得する
                return typeof(Compare).GetEnumValueOfPrimitiveText(text: compareName);
            else
                // 比較名が存在しない為-1を返す
                return -1;
        }
        /// <summary>
        /// 一次元数値配列を対象数値と比較番号で比較し、
        /// 比較が正しい数値のみの配列番号の配列を取得します
        /// 比較番号
        /// 0 : ≧  (Greater Than)
        /// 1 : ＞  (Greater)
        /// 2 : ＝  (Equal)
        /// 3 : ＜＞(Not Equal)
        /// 4 : ＜  (Less)
        /// 5 : ≦  (Less Than)
        /// </summary>
        /// <param name="numbers">一次元数値配列</param>
        /// <param name="target">対象数値</param>
        /// <param name="compareNo">比較番号</param>
        /// <returns>数値配列</returns>
        public static Primitive CompareNumbers(
            Primitive numbers,
            Primitive target,
            Primitive compareNo
            )
        {
            var tar = (double)target;

            var list = numbers.ConvertPrimitiveDoubleToKeyValuePair().ToList();

            // 比較番号の比較を行う
            switch ((Compare)(int)compareNo)
            {
                case Compare.GreaterThan:
                    return list.Where(x => x.Value >= tar)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.Greater:
                    return list.Where(x => x.Value > tar)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.Equal:
                    return list.Where(x => x.Value == tar)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.Noterual:
                    return list.Where(x => x.Value != tar)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.Less:
                    return list.Where(x => x.Value < tar)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.LessThan:
                    return list.Where(x => x.Value <= tar)
                        .ToList().ConvertKeyValuePairToPrimitive();
                default:
                    throw new ArgumentOutOfRangeException("比較番号は 0-5 の範囲でなければなりません");
            }
        }
        /// <summary>
        /// 一次元文字列配列を対象文字列と比較番号で比較し、
        /// 比較が正しい文字列のみの配列番号の配列を取得します
        /// 比較番号
        /// 0 : ≧  (Greater Than)
        /// 1 : ＞  (Greater)
        /// 2 : ＝  (Equal)
        /// 3 : ＜＞(Not Equal)
        /// 4 : ＜  (Less)
        /// 5 : ≦  (Less Than)
        /// </summary>
        /// <param name="texts">一次元文字列配列</param>
        /// <param name="target">対象文字列</param>
        /// <param name="compareNo">比較番号</param>
        /// <returns>数値配列</returns>
        public static Primitive CompareTexts(
            Primitive texts,
            Primitive target,
            Primitive compareNo
            )
        {
            var tar = target.ToString();

            var list = texts.ConvertPrimitiveStringToKeyValuePair().ToList();

            // 比較番号の比較を行う
            switch ((Compare)(int)compareNo)
            {
                case Compare.GreaterThan:
                    return list.Where(x => x.Value.CompareTo(tar) >= 0)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.Greater:
                    return list.Where(x => x.Value.CompareTo(tar) > 0)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.Equal:
                    return list.Where(x => x.Value.CompareTo(tar) == 0)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.Noterual:
                    return list.Where(x => x.Value.CompareTo(tar) != 0)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.Less:
                    return list.Where(x => x.Value.CompareTo(tar) < 0)
                        .ToList().ConvertKeyValuePairToPrimitive();
                case Compare.LessThan:
                    return list.Where(x => x.Value.CompareTo(tar) <= 0)
                        .ToList().ConvertKeyValuePairToPrimitive();
                default:
                    throw new ArgumentOutOfRangeException("比較番号は 0-5 の範囲でなければなりません");
            }
        }
        /// <summary>
        /// 配列番号の配列に一致する文字列配列を取得します
        /// </summary>
        /// <param name="texts">文字列配列</param>
        /// <param name="dimNos">配列番号配列</param>
        /// <returns>文字列配列</returns>
        public static Primitive GetDimNoTexts(
            this Primitive texts,
            Primitive dimNos
            )
        {
            var result = new Primitive();

            for (int i = 1; i < dimNos.GetItemCount(); i++)
            {
                result[i] = texts[(int)dimNos[i]].ToString();
            }

            return result;
        }
        /// <summary>
        /// 配列番号の配列に一致する数値配列を取得します
        /// </summary>
        /// <param name="nombers">数値配列</param>
        /// <param name="dimNos">配列番号配列</param>
        /// <returns>数値配列</returns>
        public static Primitive GetDimNoNumbers(
            this Primitive nombers,
            Primitive dimNos
            )
        {
            var result = new Primitive();

            for (int i = 1; i < dimNos.GetItemCount(); i++)
            {
                result[i] = (double)nombers[(int)dimNos[i]];
            }

            return result;
        }
        #endregion 公開メソッド
    }
    #endregion SmallBasic用絞込
}
