using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用正規表現クラス
    /// <summary>
    /// SmallBasic用正規表現検証クラス
    /// </summary>
    [SmallBasicType]
    public static class SBRegex
    {
        #region 文字列を正規表現パターンで検証します
        /// <summary>
        /// 対象文字列を正規表現パターンで検証し、
        /// 結果をBOOLで取得します
        /// 検証結果が正しい場合はtrueが返ります
        /// </summary>
        /// <param name="input">対象文字列</param>
        /// <param name="pattern">正規表現パターン</param>
        /// <returns>BOOL</returns>
        public static Primitive IsMatch(
            this Primitive input,
            Primitive pattern
            )
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                input: input.ToString(),
                pattern: pattern.ToString()
                );
        }
        #endregion 文字列を正規表現パターンで検証します

        #region 文字列を正規表現パターンを使用して置換します
        /// <summary>
        /// 文字列を正規表現パターンを使用して置換し、
        /// 結果を文字列で取得します
        /// </summary>
        /// <param name="input">置換対象文字列</param>
        /// <param name="pattern">正規表現パターン</param>
        /// <param name="replacement">置換パターン</param>
        /// <returns>文字列</returns>
        public static Primitive Replace(
            this Primitive input,
            Primitive pattern,
            Primitive replacement
            )
        {
            return System.Text.RegularExpressions.Regex.Replace(
                input: input.ToString(),
                pattern: pattern.ToString(),
                replacement: replacement.ToString()
                );
        }
        #endregion 文字列を正規表現パターンで置換します

        #region 文字列を正規表現パターンで抽出します
        /// <summary>
        /// 対象文字列を正規表現パターンで抽出し、
        /// 結果を文字列配列で取得します
        /// </summary>
        /// <param name="input">対象文字列</param>
        /// <param name="pattern">正規表現パターン</param>
        /// <returns>文字列配列</returns>
        public static Primitive Match(
            this Primitive input,
            Primitive pattern
            )
        {
            // パターンマッチ実行
            var m = System.Text.RegularExpressions.Regex.Match(
                input: input.ToString(),
                pattern: pattern.ToString()
                );

            // 戻り値初期化
            var result = new Primitive();

            // 結果処理
            var index = 1;
            while (m.Success)
            {
                // 一致した対象が見つかったときキャプチャした部分文字列を表示
                result[index++] = m.Value;
                // 次に一致する対象を検索
                m = m.NextMatch();
            }

            return result;
        }
        #endregion 文字列を正規表現パターンで抽出します

        #region 文字列を正規表現パターンで分割します
        /// <summary>
        /// 文字列を正規表現パターンで分割し、
        /// 結果を文字列配列で取得します
        /// </summary>
        /// <param name="input">対象文字列</param>
        /// <param name="pattern">正規表現パターン</param>
        /// <returns>文字列配列</returns>
        public static Primitive Split(
            this Primitive input,
            Primitive pattern
            )
        {
            var sp = System.Text.RegularExpressions.Regex.Split(
                input: input.ToString(), 
                pattern: pattern.ToString()
                );

            // 戻り値初期化
            var result = new Primitive();

            // 結果処理
            var index = 1;
            foreach (var s in sp)
            {
                // 結果保存
                result[index++] = s;
            }

            return result;
        }
        #endregion 文字列を正規表現パターンで分割します
    }
    #endregion SmallBasic用正規表現クラス
}
