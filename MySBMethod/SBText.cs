using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用文字列
    /// <summary>
    /// SmallBasic用文字列
    /// </summary>
    [SmallBasicType]
    public static class SBText
    {
        #region 固定値
        /// <summary>
        /// n進数変換文字列
        /// </summary>
        private static string _CONVERT_TABLE = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        #endregion 固定値

        #region 公開プロパティ
        /// <summary>
        /// n進数変換文字列
        /// </summary>
        public static Primitive CONVERT_STRING
        {
            get { return _CONVERT_TABLE; }
            set
            {
                // 文字配列に変換する
                var v = value.ToString().ToList();

                // 文字のかぶりチェック
                if (value.ToString().ToList().GetCountsOfSameData() > 0)
                    throw new ArgumentException("同一文字が存在します");

                // 文字かぶりがないので保存する
                _CONVERT_TABLE = value;
            }
        }
        #endregion 公開プロパティ

        #region 公開メソッド

        #region 文字列の先頭と末尾の空白を削除する
        /// <summary>
        /// 文字列の先頭と末尾の空白を削除します
        /// </summary>
        /// <param name="text">文字列</param>
        /// <returns>文字列</returns>
        public static Primitive Trim(
            this Primitive text
            )
        {
            // 文字列の先頭と末尾の空白を削除する
            return (Primitive)text.ToString().Trim();
        }
        #endregion 文字列の先頭と末尾の空白を削除する

        #region 文字列の先頭の空白を削除する
        /// <summary>
        /// 文字列の先頭の空白を削除します
        /// </summary>
        /// <param name="text">文字列</param>
        /// <returns>文字列</returns>
        public static Primitive TrimStart(
            this Primitive text
            )
        {
            // 文字列の先頭の空白を削除する
            return (Primitive)text.ToString().TrimStart();
        }
        #endregion 文字列の先頭の空白を削除する

        #region 文字列の後方の空白を削除する
        /// <summary>
        /// 文字列の末尾の空白を削除します
        /// </summary>
        /// <param name="text">文字列</param>
        /// <returns>文字列</returns>
        public static Primitive TrimEnd(
            this Primitive text
            )
        {
            // 文字列の末尾の空白を削除する
            return (Primitive)text.ToString().TrimEnd();
        }
        #endregion 文字列の先頭の空白を削除する

        #region 文字列を分割文字列で分割して文字列配列で返す
        /// <summary>
        /// 文字列を分割文字列で分割して、
        /// 結果を文字列配列で取得します
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="splitter">分割文字列</param>
        /// <returns>文字列配列</returns>
        public static Primitive Split(
            this Primitive text,
            Primitive splitter
            )
        {
            // 戻り値初期化
            var ret = new Primitive();

            // 削除文字列
            string[] del = { splitter.ToString() };

            // 分割する
            string[] retString = text.ToString().Split(
                separator: del,
                options: StringSplitOptions.None
                );

            // 分割数繰り返す
            for (int i = 0; i < retString.Length; i++)
            {
                // 戻り値に設定する
                ret[i] = retString[i];
            }

            return ret;
        }
        #endregion 文字列を分割文字列で分割して文字列配列で返す

        #region 配列を分割文字列で結合して結合後の文字列を返す
        /// <summary>
        /// 文字列配列を分割文字列で結合し、
        /// 結果を文字列で取得します
        /// </summary>
        /// <param name="source">配列</param>
        /// <param name="separator">分割文字列</param>
        /// <returns>文字列</returns>
        public static Primitive Join(
            this Primitive source,
            Primitive separator
            )
        {
            // 配列数を取得する
            int itemCounts = source.GetItemCount();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentException("sourceは、配列ではありません。");

            // 結合文字列初期化
            StringBuilder sb = new StringBuilder();

            // 先頭配列を追加
            sb.Append(source[0].ToString());

            // 配列数回す
            for (int i = 1; i <= itemCounts; i++)
            {
                // 文字列をセパレータで結合する
                sb.Append(
                    value: string.Format(
                        format: "{0}{1}",
                        arg0: separator,
                        arg1: source[i]
                        ));
            }

            // 結果を返す
            return (Primitive)sb.ToString();
        }
        #endregion 配列を分割文字列で結合して結合後の文字列を返す

        #region 数値配列を書式に応じて加工する
        /// <summary>
        /// 数値配列を書式文字列に応じて加工し、
        /// 結果を文字列で取得します
        /// </summary>
        /// <param name="format">書式</param>
        /// <param name="values">数値配列</param>
        /// <returns>文字列</returns>
        public static Primitive FormatDimDouble(
            this Primitive format,
            Primitive values
            )
        {
            // Primitiveを文字列に変換する
            var f = format.ToString();

            // Primitiveをdouble[]に変換する
            var a = values.ConvertPrimitiveToDimDouble().ToArray();

            if (a.Length < 1)
                throw new ArgumentOutOfRangeException("配列が0件です");

            // 文字列加工して戻す
            return string.Format(
                format: f,
                args: a.ConvertObjects()
                );
        }
        #endregion 文字列を書式に応じて加工する

        #region 文字列中の旧文字列を新文字列に置換する
        /// <summary>
        /// 文字列中の旧文字列を新文字列に置換し、
        /// 結果を文字列で取得します
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="oldValue">旧文字列</param>
        /// <param name="newValue">新文字列</param>
        /// <returns>文字列</returns>
        public static Primitive Replace(
            this Primitive text,
            Primitive oldValue,
            Primitive newValue
            )
        {
            // 文字列中の旧文字列を新文字列に置換する
            return text.ToString().Replace(
                oldValue: oldValue,
                newValue: newValue
                );
        }
        #endregion 文字列中の旧文字列を新文字列に置換する

        #region 文字列を反転して返す
        /// <summary>
        /// 文字列の先頭から末尾を反転して文字列として取得します
        /// 例) "ABC" ⇒ "CBA"
        /// </summary>
        /// <param name="text">文字列</param>
        /// <returns>文字列</returns>
        public static Primitive Reverse(
            this Primitive text
            )
        {
            // 文字列を反転して返す
            return string.Join("", text.ToString().Reverse());
        }
        #endregion 文字列を反転して返す

        #region 10進数数値をn進数文字列に変換して取得します
        /// <summary>
        /// 10進数数値をn進数文字列に変換し、
        /// 結果を文字列で取得します
        /// n進数 : 2進数から最大進数
        /// 変換文字列に登録されている文字数が変換可能な最大進数となります
        /// </summary>
        /// <param name="value">数値</param>
        /// <param name="toDecimal">n進数値</param>
        /// <returns>文字列</returns>
        public static Primitive ConvertToString(
            this Primitive value,
            Primitive toDecimal
            )
        {
            // 結果を返す
            return _ConvertToString(value: (int)value, toDecimal: (int)toDecimal);
        }
        #endregion 10進数をn進数に変換する

        #region n進数文字列を10進数数値に変換して取得します
        /// <summary>
        /// n進数文字列を10進数値に変換し、
        /// 結果を数値で取得します
        /// n進数 : 2進数から最大進数
        /// 変換文字列に登録されている文字数が変換可能な最大進数となります
        /// </summary>
        /// <param name="value">文字列</param>
        /// <param name="baseDecimal">n進数値</param>
        /// <returns>数値</returns>
        public static Primitive ConvertToInt(
            this Primitive value,
            Primitive baseDecimal
            )
        {
            // 結果を返す
            return _ConvertToInt(value: value.ToString(), baseDecimal: (int)baseDecimal);
        }
        #endregion n進数文字列を10進数数値に変換して取得します

        #endregion 公開メソッド

        #region メソッド

        #region 10進数をn進数に変換する
        /// <summary>
        /// 10進数値をn進数文字列に変換し、
        /// 結果を文字列で取得します
        /// 変換するために変換文字列を使用します
        /// </summary>
        /// <param name="value">10進数値</param>
        /// <param name="toDecimal">n進数値</param>
        /// <returns>文字列</returns>
        public static string _ConvertToString(
            int value,
            int toDecimal
            )
        {
            // 進数が範囲内かチェックする
            if (toDecimal < 2 || toDecimal > _CONVERT_TABLE.Length)
                throw new ArgumentOutOfRangeException();

            // 戻り値初期化
            string result = string.Empty;

            // 商が0になるまでループ
            while (value > 0)
            {
                // 余りを求め文字で追加
                result += _CONVERT_TABLE[value % toDecimal];

                // 整数部を求める
                value = value / toDecimal;
            }

            // 文字列を反転して返す
            return string.Join("", result.Reverse());
        }
        #endregion 10進数をn進数に変換する

        #region n進数を10進数に変換する
        /// <summary>
        /// n進数を10進数に変換し、
        /// 結果を数値で取得します
        /// 変換するために変換文字列を使用する
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <param name="baseDecimal">n進数</param>
        /// <returns>数値</returns>
        public static int _ConvertToInt(
            string value,
            int baseDecimal
            )
        {
            // 進数が範囲内かチェックする
            if (baseDecimal < 2 || baseDecimal > _CONVERT_TABLE.Length)
                throw new ArgumentOutOfRangeException();

            // 戻り値初期化
            var result = 0;

            // 文字列の先頭から取り出す
            foreach (var c in value)
            {
                // 結果をbaseDecimal倍する
                result *= baseDecimal;

                // 文字を検索する
                var d = _CONVERT_TABLE.IndexOf(c);

                // 文字を発見できなかった場合例外とする
                if (d < 0)
                    throw new ArgumentException($"定義されていない文字があります : {c}");

                // 戻り値に加える
                result += d;
            }

            // 結果を返す
            return result;
        }
        #endregion n進数を10進数に変換する

        #endregion メソッド
    }
    #endregion SmallBasic用文字列
}
