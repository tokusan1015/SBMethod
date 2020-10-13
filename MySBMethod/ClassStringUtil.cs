using System;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;

namespace SBMethod
{
    /// <summary>
    /// 文字ユーティリティ
    /// </summary>
    public static class ClassStringUtil
    {
        #region 固定値
        /// <summary>
        /// '0'文字
        /// </summary>
        private const string STRING_ZERO = @"0";
        /// <summary>
        /// '1'文字
        /// </summary>
        private const string STRING_ONE = @"1";
        /// <summary>
        /// タブ文字
        /// </summary>
        private const string STRING_TAB = "\t";
        /// <summary>
        /// 改行文字
        /// </summary>
        private const string STRING_ENTER = "\n";
        /// <summary>
        /// 改行タブ文字列
        /// </summary>
        private const string STRING_ENTER_TAB = STRING_ENTER + STRING_TAB;
        /// <summary>
        /// 文字開始位置
        /// </summary>
        private const string RTF_VIEWKIND = @"\viewkind";
        /// <summary>
        /// コマンド先頭文字
        /// </summary>
        private const char RTF_CHAR_COMMAND = '\\';
        /// <summary>
        /// 空白文字
        /// </summary>
        private const char RTF_CHAR_SPACE = ' ';
        /// <summary>
        /// リターン文字
        /// </summary>
        private const char RTF_CHAR_CR = '\r';
        /// <summary>
        /// ラインフィード文字
        /// </summary>
        private const char RTF_CHAR_LF = '\n';
        /// <summary>
        /// RTF改行コード
        /// </summary>
        private const string RTF_STRING_ENTER = @"\par";
        /// <summary>
        /// RTFタブコード
        /// </summary>
        private const string RTF_STRING_TAB = @"\tab";
        /// <summary>
        /// 16進コード先頭文字
        /// </summary>
        private const string RTF_STRING_HEX_HED = @"\'";
        /// <summary>
        /// 開始括弧
        /// </summary>
        private const char RTF_CHAR_START_BRACKETS = '{';
        /// <summary>
        /// 終了括弧
        /// </summary>
        private const char RTF_CHAR_END_BRACKETS = '}';
        /// <summary>
        /// 文字開始
        /// </summary>
        private const string RTF_STRING_START = " ";
        /// <summary>
        /// カナ文字
        /// </summary>
        private const string HAN_KANA = "ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜｦﾝﾞﾟｰｧｨｩｪｫｬｭｮｯ";
        /// <summary>
        /// nullエラー
        /// </summary>
        public const string ERROR_NULL = "nullエラー：オブジェクトがnull";
        #endregion 固定値

        #region 列挙型
        /// <summary>
        /// Rtf
        /// </summary>
        private enum enmRtfControlCode : int
        {
            /// <summary>
            /// 0文字
            /// </summary>
            Empty = 0,
            /// <summary>
            /// その他のコントロールコード
            /// </summary>
            Control,
            /// <summary>
            /// タブ("\tab")
            /// </summary>
            Tab,
            /// <summary>
            /// テキスト
            /// </summary>
            Text,
            /// <summary>
            /// 改行("/par")
            /// </summary>
            Par,
            /// <summary>
            /// リターン('\r')
            /// </summary>
            Return,
            /// <summary>
            /// ラインフィード('\n')
            /// </summary>
            Linefeed,
            /// <summary>
            /// 改行テキスト(先頭に'\n'で2文字以上)
            /// </summary>
            LinefeedText,
            /// <summary>
            /// 開始文字('{')
            /// </summary>
            StartBrackets,
            /// <summary>
            /// 終了文字('}')
            /// </summary>
            EndBrackets,
        }
        #endregion 列挙型

        #region メソッド

        #region 指定文字を指定回数連結した文字列を返す
        /// <summary>
        /// 指定文字を指定回数連結した文字列を返す
        /// </summary>
        /// <param name="c">文字</param>
        /// <param name="count">回数</param>
        /// <returns>連結文字列</returns>
        public static string StrDup(
            char c,
            int count
            )
        {
            // 指定文字を指定回数連結して返す
            return new string(c: c, count: count);
        }
        #endregion 指定文字列を指定回数連結した文字列を返す

        #region 文字列をHexStringに変換する
        /// <summary>
        /// 文字列をHexStringに変換する
        /// 00-00-00-00-...
        /// </summary>
        /// <param name="sData">文字列</param>
        /// <returns>16進文字列</returns>
        public static string StringToHexString(
            this string sData
            )
        {
            try
            {
                // 文字列をHexStringに変換する
                return sData.StringToBytes().BytesToHexString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列をHexStringに変換する

        #region HexStringを文字列に変換する
        /// <summary>
        /// HexStringを文字列に変換する
        /// 00-00-00-00-...
        /// </summary>
        /// <param name="sHexString">16進文字列</param>
        /// <returns>文字列</returns>
        public static string HexStringToString(
            this string sHexString
            )
        {
            try
            {
                // HexStringを文字列に変換する
                return sHexString.HexStringToByte().BytesToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion HexStringを文字列に変換する

        #region Byte配列をHexStringに変換する
        /// <summary>
        /// Byte配列をHexStringに変換する(分割文字固定'-')
        /// 00-00-00-00-...
        /// </summary>
        /// <param name="bData">byte配列</param>
        /// <returns>文字列</returns>
        public static string BytesToHexString(
            this byte[] bData
            )
        {
            try
            {
                // Byte配列をHexStringに変換する
                return BitConverter.ToString(
                    value: bData
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Byte配列をHexStringに変換する

        #region HexStringをByte配列に変換する
        /// <summary>
        /// HexStringをByte配列に変換する
        /// 00-00-00-00-...
        /// </summary>
        /// <param name="sData">Hex文字列</param>
        /// <param name="separator">分割文字(基本固定)</param>
        /// <param name="fromBase">数値基数(基本固定)</param>
        /// <returns>Byte配列</returns>
        public static byte[] HexStringToByte(
            this string sData,
            char separator = '-',
            int fromBase = 16
            )
        {
            try
            {
                // Hex文字列を分割('-')する
                string[] sDim = sData.Split(separator: separator);
                
                // Byte配列を生成する
                byte[] bData = new byte[sDim.Length];

                // 全データを回す
                for (int i = 0; i < sDim.Length; i++)
                {
                    // 対象データをByteに変換する
                    bData[i] = Convert.ToByte(
                        value: sDim[i], 
                        fromBase: fromBase
                        );
                }

                return bData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion HexStringをByte配列に変換する

        #region 文字列をByte配列に変換する
        /// <summary>
        /// 文字列をByte配列に変換する
        /// </summary>
        /// <param name="sData">文字列</param>
        /// <param name="sName">コードページ名</param>
        /// <returns>byte配列</returns>
        public static byte[] StringToBytes(
            this string sData,
            string sName = "Shift_JIS"
            )
        {
            try
            {
                return Encoding.GetEncoding(
                    name: sName
                    ).GetBytes(s: sData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列をByte配列に変換する

        #region Byte配列を文字列に変換する
        /// <summary>
        /// Byte配列を文字列に変換する
        /// </summary>
        /// <param name="bData">byte配列</param>
        /// <param name="sName">コードページ名</param>
        /// <returns>文字列</returns>
        public static string BytesToString(
            this byte[] bData,
            string sName = "Shift_JIS"
            )
        {
            try
            {
                return Encoding.GetEncoding(
                    name: sName
                    ).GetString(bytes: bData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Byte配列を文字列に変換する

        #region 文字列を数値(int,double,float,long)に変換する

        #region 文字列が対象の型に変換できるか確認する
        /// <summary>
        /// 文字列が対象の型に変換できるか確認する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="sData">チェックデータ</param>
        /// <returns>変換できる場合はtrue</returns>
        public static bool IsNumber<T>(
            this string sData
            )
        {
            try
            {
                TypeDescriptor.GetConverter(type: typeof(T)).ConvertFromString(text: sData);
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion 対象の型に変換できるか確認する

        #region Boolを数値(int)に変換する
        /// <summary>
        /// Boolを数値(int)に変換する
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        public static int intParse(
            this bool bData
            )
        {
            try
            {
                // Boolをintに変換する
                return bData ? 1 : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Boolを数値(int)に変換する

        #region 文字列を数値(int)に変換する
        /// <summary>
        /// 文字列を数値(int)に変換する
        /// </summary>
        /// <param name="sData">数値文字</param>
        /// <returns>数値</returns>
        public static int intParse(
            this string sData
            )
        {
            try
            {
                // 文字列をintに変換する
                return (int)doubleParse(sData: sData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列を数値(int)に変換する

        #region 文字列を数値(double)に変換する
        /// <summary>
        /// 文字列を数値(double)に変換する
        /// </summary>
        /// <param name="sData">数値文字</param>
        /// <returns>数値</returns>
        public static double doubleParse(
            this string sData
            )
        {
            try
            {
                // 文字列をdoubleに変換する
                double ret = 0;
                bool result = double.TryParse(s: sData, result: out ret);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列を数値(double)に変換する

        #region 文字列を数値(float)に変換する
        /// <summary>
        /// 文字列を数値(float)に変換する
        /// </summary>
        /// <param name="sData">数値文字</param>
        /// <returns>数値</returns>
        public static float floatParse(
            this string sData
            )
        {
            try
            {
                // 文字列をfloatに変換する
                float ret = 0;
                float.TryParse(s: sData, result: out ret);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列を数値(float)に変換する

        #region 文字列を数値(long)に変換する
        /// <summary>
        /// 文字列を数値(long)に変換する
        /// </summary>
        /// <param name="sData">数値文字</param>
        /// <returns>数値</returns>
        public static long longParse(
            this string sData
            )
        {
            try
            {
                // 文字列をdoubleに変換する
                long ret = 0;
                long.TryParse(s: sData, result: out ret);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列を数値(long)に変換する

        #endregion 文字列を数値(int,double,float,long)に変換する

        #region オブジェクトを(int,double,float,long)に変換する

        #region オブジェクトを(int)に変換する
        /// <summary>
        /// オブジェクトを(int)に変換する
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <returns>int</returns>
        public static int intParse(
            this object obj
            )
        {
            try
            {
                // 文字列をdoubleに変換する
                return obj.ToString().intParse();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion オブジェクトを(int)に変換する

        #region オブジェクトを(double)に変換する
        /// <summary>
        /// オブジェクトを(double)に変換する
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <returns>double</returns>
        public static double doubleParse(
            this object obj
            )
        {
            try
            {
                // 文字列をdoubleに変換する
                return obj.ToString().doubleParse();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion オブジェクトを(double)に変換する

        #region オブジェクトを(float)に変換する
        /// <summary>
        /// オブジェクトを(double)に変換する
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <returns>float</returns>
        public static float floatParse(
            this object obj
            )
        {
            try
            {
                // 文字列をfloatに変換する
                return obj.ToString().floatParse();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion オブジェクトを(float)に変換する

        #region オブジェクトを(long)に変換する
        /// <summary>
        /// オブジェクトを(long)に変換する
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <returns>float</returns>
        public static long longParse(
            this object obj
            )
        {
            try
            {
                // 文字列をlongに変換する
                return obj.ToString().longParse();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion オブジェクトを(long)に変換する

        #endregion オブジェクトを(int,double,float,long)に変換する

        #region オブジェクト文字列からBool値を取得する(大文字変換後)
        /// <summary>
        /// オブジェクト文字列からBool値を取得する(大文字変換後)
        /// true  : CConstants.STRING_ONE,"TRUE","真"
        /// false : CConstants.STRING_ZERO,"FALSE","偽"
        /// </summary>
        /// <param name="objString">Bool文字列</param>
        /// <returns>Bool値</returns>
        public static bool GetBool(
            this object objString
            )
        {
            try
            {
                // 文字列からBool値を取得する
                return objString.ToString().GetStringToBool();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion オブジェクト文字列からBool値を取得する(大文字変換後)

        #region Bool値から文字列を取得する
        /// <summary>
        /// Bool値から文字列を取得する
        /// </summary>
        /// <param name="b">Bool値</param>
        /// <param name="sTrueString">True文字列</param>
        /// <param name="sFalseString">False文字列</param>
        /// <returns></returns>
        public static string GetBoolToString(
            this bool b,
            string sTrueString = STRING_ONE,
            string sFalseString = STRING_ZERO
            )
        {
            try
            {
                // 文字列を返す
                return b
                    ? sTrueString
                    : sFalseString;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Bool値から文字列を取得する

        #region 文字列からBool値を取得する
        /// <summary>
        /// 文字列からBool値を取得する(大文字変換後)
        /// true  : CConstants.STRING_ONE,"TRUE","真"
        /// false : CConstants.STRING_ZERO,"FALSE","偽"
        /// </summary>
        /// <param name="bString">Bool文字列</param>
        /// <returns>Bool値</returns>
        public static bool GetStringToBool(
            this string bString
            )
        {
            try
            {
                // 文字チェック
                switch (bString.ToUpper())
                {
                    // true
                    case STRING_ONE:
                    case "TRUE":
                    case "ON":
                    case "真":
                    case "成":
                    case "有":
                    case "勝":
                    case "正":
                    case "有効":
                    case "成功":
                        return true;

                    // false
                    case STRING_ZERO:
                    case "FALSE":
                    case "OFF":
                    case "偽":
                    case "否":
                    case "無":
                    case "敗":
                    case "負":
                    case "無効":
                    case "失敗":
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Bool文字列からBool値を取得する

        #region 指定文字以降を削除する
        /// <summary>
        /// 指定文字以降を削除する
        /// 発見できなかった場合はdataを返す
        /// </summary>
        /// <param name="sData">対象文字列</param>
        /// <param name="c">指定文字</param>
        /// <returns>削除した文字列</returns>
        public static string RemoveAfterChar(
            this string sData,
            char c = '_'
            )
        {
            try
            {
                // dataがnullの場合は例外
                if (sData == null)
                    throw new Exception(message: ERROR_NULL);

                // dataが空の場合は何もしない
                if (sData.Length < 1)
                    return sData;

                // 指定文字を検索する
                int p = sData.IndexOf(value: c);

                // 指定文字が未発見の場合は全文字とする
                if (p < 0)
                    p = sData.Length;

                // 文字を切り出す
                return sData.Substring(startIndex: 0, length: p);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 指定文字以降を削除する

        #region 指定文字以前を削除する
        /// <summary>
        /// 指定文字以前を削除する
        /// 発見位置が先頭の場合はdataを返す
        /// 発見できなかった場合
        ///   bNotFindEmpty=trueはEmptyを返す
        ///   bNotFindEmpty=falseはdataを返す
        /// </summary>
        /// <param name="sData">対象文字列</param>
        /// <param name="c">指定文字</param>
        /// <param name="bNotFindEmpty">trueの場合、未発見時Emptyにする</param>
        /// <returns>削除した文字列</returns>
        public static string RemoveBeforeChar(
            this string sData,
            char c = '_',
            bool bNotFindEmpty = true
            )
        {
            try
            {
                // dataがnullの場合は例外
                if (sData == null)
                    throw new Exception(message: ERROR_NULL);

                // dataが空の場合は何もしない
                if (sData.Length < 1) return sData;

                // 指定文字を検索する
                int p = sData.IndexOf(value: c);

                // 指定文字が2文字目以降の場合のみ処理する
                if (p + 1 > 0)
                {
                    // 発見位置が最後の場合は全文字とする
                    if (p + 1 >= sData.Length)
                        p = 0;
                    // 文字を切り出す
                    return sData.Substring(
                        startIndex: p, 
                        length: sData.Length - p
                        );
                }
                else
                {
                    // 発見できなかった場合の処理
                    if (bNotFindEmpty)
                        return string.Empty;
                    else
                        return sData;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 指定文字以降を削除する

        #region 指定文字列で囲まれた文字を取得する
        /// <summary>
        /// 指定文字で囲まれた文字を取得する
        /// </summary>
        /// <param name="sText">対象文字列</param>
        /// <param name="sSeparatorStart">開始文字列</param>
        /// <param name="sSeparatorEnd">終了文字列</param>
        /// <returns>指定文字で囲まれた文字列</returns>
        public static string GetSandwichedCharacters(
            this string sText,
            string sSeparatorStart,
            string sSeparatorEnd
            )
        {
            try
            {
                // パターン生成
                string pattern = string.Format(
                    format: @"(\{0})(?<target>.+?)(\{1})",
                    args: new object[]
                    {
                        sSeparatorStart,
                        sSeparatorEnd
                    });
                // 文字検索
                return Regex.Match(
                    input: sText, 
                    pattern: pattern
                    ).Groups["target"].Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 指定文字列で囲まれた文字を取得する
        /// </summary>
        /// <param name="sText">対象文字列</param>
        /// <param name="sSeparator">指定文字</param>
        /// <returns>指定文字で囲まれた文字列</returns>
        public static string GetSandwichedCharacters(
            this string sText,
            string sSeparator
            )
        {
            try
            {
                // 文字検索
                return GetSandwichedCharacters(
                    sText: sText, 
                    sSeparatorStart: sSeparator, 
                    sSeparatorEnd: sSeparator
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 指定文字で囲まれた文字を取得する

        #region 指定文字列から半角カナ文字以外を削除する
        /// <summary>
        /// 指定文字列から半角カナ文字以外を削除する
        /// </summary>
        /// <param name="sData">入力データ</param>
        /// <returns>削除された文字列</returns>
        public static string GetHanKana(
            this string sData
            )
        {
            try
            {
                var sb = new StringBuilder();
                foreach (char ch in sData)
                    // 不恰好だが確実なのは折り紙付き。
                    if (HAN_KANA.IndexOf(ch) >= 0)
                        sb.Append(value: ch);
                return sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 半角カナ文字以外は削除する

        #region 文字列切り出し

        #region 文字列の指定した位置から指定した長さを取得する
        /// <summary>
        /// 文字列の指定した位置から指定した長さを取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="start">開始位置</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Mid(
            this string str, 
            int start, 
            int len
            )
        {
            try
            {
                // 開始位置が１未満かチェックする
                if (start < 1)
                    throw new ArgumentException(message: "引数'start'は1以上でなければなりません。");
                
                // 長さが0以上かをチェックする
                if (len < 0)
                    throw new ArgumentException(message: "引数'len'は0以上でなければなりません。");
                
                // 文字列がnullかチェックする
                if (str == null)
                    return string.Empty;
                
                // 文字列の長さが開始位置より大きいことをチェックする
                if (str.Length < start)
                    return string.Empty;
                
                // 文字列長が開始位置＋長さより小さいことをチェックする
                if (str.Length < (start + len))
                    return str.Substring(startIndex: start - 1);

                // 文字を切り出す
                return str.Substring(startIndex: start - 1, length: len);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列の指定した位置から指定した長さを取得する

        #region 文字列の指定した位置から末尾までを取得する
        /// <summary>
        /// 文字列の指定した位置から末尾までを取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="start">開始位置</param>
        /// <returns>取得した文字列</returns>
        public static string Mid(
            this string str, 
            int start
            )
        {
            try
            {
                // 文字を切り出す
                return Mid(
                    str: str, 
                    start: start, 
                    len: str.Length
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列の指定した位置から末尾までを取得する

        #region 文字列の先頭から指定した長さの文字列を取得する
        /// <summary>
        /// 文字列の先頭から指定した長さの文字列を取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Left(
            this string str, 
            int len
            )
        {
            try
            {
                // 長さが0以上かをチェックする
                if (len < 0)
                    throw new ArgumentException(message: "引数'len'は0以上でなければなりません。");

                // 文字列がnullかチェックする
                if (str == null)
                    return string.Empty;

                // 文字列の長さが長さ以上であることをチェックする
                if (str.Length <= len)
                    return str;

                // 文字を切り出す
                return str.Substring(startIndex: 0, length: len);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列の先頭から指定した長さの文字列を取得する

        #region 文字列の末尾から指定した長さの文字列を取得する
        /// <summary>
        /// 文字列の末尾から指定した長さの文字列を取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Right(
            this string str, 
            int len
            )
        {
            try
            {
                // 長さが0以上かをチェックする
                if (len < 0)
                    throw new ArgumentException(message: "引数'len'は0以上でなければなりません。");

                // 文字列がnullかチェックする
                if (str == null)
                    return string.Empty;

                // 文字列の長さが長さ以上であることをチェックする
                if (str.Length <= len)
                    return str;

                // 文字を切り出す
                return str.Substring(startIndex: str.Length - len, length: len);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列の末尾から指定した長さの文字列を取得する

        #endregion 文字列切り出し

        #region 連続した空白をタブ変換に変換する
        /// <summary>
        /// 連続した空白をタブ変換に変換する
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <param name="spaceCounts"></param>
        /// <returns></returns>
        public static string SpaceToTab(
            this string str,
            int spaceCounts = 4
            )
        {
            try
            {
                // 文字列がnullかチェックする
                if (str == null)
                    return string.Empty;

                // 空白数をチェック
                if (spaceCounts < 1)
                    return str;

                // 文字を切り出す
                return str.Replace(new string(' ', spaceCounts), "\t");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 連続した空白をタブ変換に変換する

        #region 検索文字列位置リストを取得する
        /// <summary>
        /// 検索文字列位置リストを取得する
        /// 発見できなかった場合、0件となる
        /// </summary>
        /// <param name="text">対象文字列</param>
        /// <param name="findText">検索文字列</param>
        /// <returns>位置リスト</returns>
        public static List<int> GetIndexList_FindText(
            this string text,
            string findText
            )
        {
            try
            {
                // 戻り値初期化
                var ret = new List<int>();

                // 引数チェック
                if (text.Length < 1 
                    || findText.Length < 1)
                    return ret;

                // 最初の位置を取得する
                int foundIndex = text.IndexOf(findText);

                // 発見位置が0以上の場合、繰り返す
                while (foundIndex >= 0)
                {
                    // 位置を追加する
                    ret.Add(foundIndex);

                    // 次の開始位置を取得
                    int nextIndex = foundIndex + findText.Length;

                    // 最後まで検索しているかチェック
                    if (nextIndex < text.Length)
                        // 最後でない場合、次を検索
                        foundIndex = text.IndexOf(findText, nextIndex);
                    else
                        // 最後まで検索したので終了
                        break;
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 検索文字列位置リストを取得する

        #region 行頭位置リストを取得する
        /// <summary>
        /// 行頭位置リストを取得する
        /// 未発見の場合は、0件となる
        /// </summary>
        /// <param name="text">対象文字列</param>
        /// <returns>行頭位置リスト</returns>
        public static List<int> GetIndexList_BeginningOfLine(
            this string text
            )
        {
            try
            {
                // 戻り値初期化
                var ret = new List<int>();

                // 改行文字位置リスト取得
                List<int> list = text.GetIndexList_FindText(
                    findText: STRING_ENTER
                    );

                // 改行の次の位置にIndexを移動する
                foreach(int index in list)
                {
                    // 改行の次の位置取得
                    int newIndex = index + 1;
                    // 改行の次の位置が対象文字列に含まれていることを確認する
                    if (newIndex < text.Length)
                        // 文字列内ならば、リストに追加
                        ret.Add(newIndex);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 行頭位置リストを取得する

        #region 前方一致
        /// <summary>
        /// 前方一致検索
        /// 含まれている場合true
        /// </summary>
        /// <param name="targetText">対象文字列</param>
        /// <param name="findText">検索文字列</param>
        /// <returns>含まれている場合true</returns>
        public static bool FowardMatch(
            this string targetText,
            string findText 
            )
        {
            try
            {
                // 対象文字列数が検索文字列数よりも小さい場合false
                if (targetText.Length < findText.Length)
                    return false;

                // 先頭から検索文字数分切り出し比較する
                return targetText.Left(findText.Length) == findText;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 前方一致

        #region 後方一致
        /// <summary>
        /// 後方一致
        /// 含まれている場合true
        /// </summary>
        /// <param name="targetText">対象文字列</param>
        /// <param name="findText">検索文字列</param>
        /// <returns>含まれている場合true</returns>
        public static bool BackwardMatch(
            this string targetText,
            string findText
            )
        {
            try
            {
                // 対象文字列数が検索文字列数よりも小さい場合false
                if (targetText.Length < findText.Length)
                    return false;

                // 先頭から検索文字数分切り出し比較する
                return targetText.Right(findText.Length) == findText;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 後方一致

        #region MemoryStream

        #region 文字列からMemoryStreamに変換する
        /// <summary>
        /// 文字列からMemoryStreamに変換する
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>MemoryStream</returns>
        public static MemoryStream ConvertMemoryStream(
            this string text,
            Encoding encoding
            )
        {
            try
            {
                // 文字列からMemoryStreamに変換する
                return new MemoryStream(encoding.GetBytes(text));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 文字列からMemoryStreamに変換する

        #region MemoryStreamから文字列に変換する
        /// <summary>
        /// MemoryStreamから文字列に変換する
        /// </summary>
        /// <param name="stream">MemoryStream</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>文字列</returns>
        public static string ConvertString(
            this MemoryStream stream,
            Encoding encoding
            )
        {
            try
            {
                // 文字列からMemoryStreamに変換する
                return encoding.GetString(stream.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion MemoryStreamから文字列に変換する

        #endregion MemoryStream

        #region リッチテキスト

        #region Viewkind位置を取得する
        /// <summary>
        /// Viewkind位置を取得する
        /// 発見できなかった場合は、-1
        /// </summary>
        /// <param name="rtfText">対象RTF</param>
        /// <returns>Viewkind位置</returns>
        public static int GetIndexOfViewkind(
            this string rtfText
            )
        {
            try
            {
                // Viewkind位置を取得する
                return rtfText.IndexOf(RTF_VIEWKIND);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Viewkind位置を取得する

        #region RTFの設定を解析する
        /// <summary>
        /// RTFの設定を解析する
        /// 結果は、enmRtfで戻る
        /// </summary>
        /// <param name="data">設定</param>
        /// <returns>結果</returns>
        private static enmRtfControlCode RtfAnalysis(
            string data
            )
        {
            try
            {
                // 0文字の場合は、不明とする
                if (data.Length < 1)
                    return enmRtfControlCode.Empty;

                // "/par"の場合は改行
                if (data == RTF_STRING_ENTER)
                    return enmRtfControlCode.Par;

                // "\tab"の場合はタブ
                if (data == RTF_STRING_TAB)
                    return enmRtfControlCode.Tab;

                // 先頭が"\'"の場合はテキスト
                if (data.Length > 2)
                {
                    if (data.Left(2) == RTF_STRING_HEX_HED)
                        return enmRtfControlCode.Text;
                }

                // 先頭が'\'の場合はその他の設定
                if (data[0] == RTF_CHAR_COMMAND)
                    return enmRtfControlCode.Control;

                // '\r'のみの場合はリターン
                if (data[0] == RTF_CHAR_CR)
                    return enmRtfControlCode.Return;

                // '\n'のみの場合はラインフィード
                if (data[0] == RTF_CHAR_LF && data.Length == 1)
                    return enmRtfControlCode.Linefeed;

                // 先頭が'\n'で2文字以上の場合は改行テキスト
                if (data[0] == RTF_CHAR_LF && data.Length > 1)
                    return enmRtfControlCode.LinefeedText;

                // 先頭が'{'の場合は、終了
                if (data[0] == RTF_CHAR_START_BRACKETS)
                    return enmRtfControlCode.StartBrackets;

                // 先頭が'}'の場合は、終了
                if (data[0] == RTF_CHAR_END_BRACKETS)
                    return enmRtfControlCode.EndBrackets;

                // その他の場合、テキスト
                return enmRtfControlCode.Text;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion RTFの設定かチェックする

        #region リッチテキストの行頭にTabを付加する
        /// <summary>
        /// リッチテキストの行頭にTabを付加する(英語のみ)
        /// 但し、最終文字は処理しない
        /// </summary>
        /// <param name="rtfText">対象RTF</param>
        /// <returns>行頭にTabを追加した文字列</returns>
        public static string AddRtfTabBeginningOfLine(
            this string rtfText
            )
        {
            try
            {
                // コマンド用SBのキャパシティ
                const int sbCommand_length = 128;

                // 戻り値初期化
                var ret = new StringBuilder(rtfText.Length + rtfText.Length / 10);

                // \viewkind位置を取得する
                int startIndex = rtfText.GetIndexOfViewkind();

                // \viewkindが発見できない場合はエラーとする
                if (startIndex < 0)
                    throw new ArgumentException(@"\viewkind is nothing");

                // 開始位置以前の文字列を戻り値に追加する
                ret.Append(rtfText.Left(startIndex));

                // コマンド初期化
                var command = new StringBuilder(sbCommand_length);
                bool addTabForLine = false;
                bool endBrackets = false;

                // 文字列に対して1文字づつ処理する(最後の'}'を無視)
                for (int i = startIndex; i < rtfText.Length; i++)
                {
                    // 1文字取得
                    char d = rtfText[i];

                    // 終了の場合は何もしない
                    if (!endBrackets)
                    {
                        // コマンド確定フラグ('\\', ' ', '\r', '\n', '{', '}')
                        bool commandComplete =
                            d == RTF_CHAR_COMMAND
                            || d == RTF_CHAR_SPACE
                            || d == RTF_CHAR_CR
                            || d == RTF_CHAR_LF
                            || d == RTF_CHAR_START_BRACKETS
                            || d == RTF_CHAR_END_BRACKETS
                            ;

                        // コマンドが確定している場合
                        if (commandComplete)
                        {
                            // コマンド解析
                            switch (RtfAnalysis(
                                data: command.ToString()
                                ))
                            {
                                case enmRtfControlCode.Text:
                                    // Tabを追加していない場合
                                    if (!addTabForLine)
                                    {
                                        // "\tab"をコマンドの先頭に追加する
                                        command.Insert(
                                            index: 0,
                                            value: RTF_STRING_TAB
                                            );
                                        addTabForLine = true;
                                    }
                                    break;
                                case enmRtfControlCode.LinefeedText:
                                    // Tabを追加していない場合
                                    if (!addTabForLine)
                                    {
                                        // "\tab"をコマンドの'\n'の次に追加する
                                        command.Insert(
                                            index: 1,
                                            value: RTF_STRING_TAB + RTF_CHAR_SPACE
                                            );
                                        addTabForLine = true;
                                    }
                                    break;
                                case enmRtfControlCode.Par:
                                    addTabForLine = false;
                                    break;
                                case enmRtfControlCode.EndBrackets:
                                    endBrackets = true;
                                    break;
                                case enmRtfControlCode.Empty:
                                case enmRtfControlCode.Control:
                                case enmRtfControlCode.Tab:
                                case enmRtfControlCode.Return:
                                case enmRtfControlCode.Linefeed:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            // コマンドを戻り値に設定
                            ret.Append(command.ToString());

                            // コマンド初期化
                            command.Clear();
                        }
                    }

                    // コマンドに文字追加
                    command.Append(d);
                }

                // commandに文字が残っている場合はそれを出力
                if (command.Length > 0)
                    ret.Append(command.ToString());
#if DEBUG
                string tmp = ret.ToString();
#endif
                return ret.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion リッチテキストの行頭にTabを付加する

        #region リッチテキストの行頭からTabを削除する
        /// <summary>
        /// リッチテキストの行頭からTabを削除する(英語のみ)
        /// 存在しない場合、何もしない
        /// </summary>
        /// <param name="rtfText">対象RTF</param>
        /// <returns>行頭からTabを削除した文字列</returns>
        public static string DelRtfTabBeginningOfLine(
            this string rtfText
            )
        {
            try
            {
                // コマンド用SBのキャパシティ
                const int sbCommand_length = 128;

                // 戻り値初期化
                var ret = new StringBuilder(rtfText.Length + rtfText.Length / 10);

                // \viewkind位置を取得する
                int startIndex = rtfText.GetIndexOfViewkind();

                // \viewkindが発見できない場合はエラーとする
                if (startIndex < 0)
                    throw new ArgumentException(@"\viewkind is nothing");

                // 開始位置以前の文字列を戻り値に追加する
                ret.Append(rtfText.Left(startIndex));

                // コマンド初期化
                var command = new StringBuilder(sbCommand_length);
                bool delTabForLine = false;
                bool endBrackets = false;
                bool firstText = true;

                // 文字列に対して1文字づつ処理する(最後の'}'を無視)
                for (int i = startIndex; i < rtfText.Length; i++)
                {
                    // 1文字取得
                    char d = rtfText[i];

                    // 終了の場合は何もしない
                    if (!endBrackets)
                    {
                        // コマンド確定フラグ('\\', ' ', '\r', '\n', '{', '}')
                        bool commandComplete =
                            d == RTF_CHAR_COMMAND
                            || d == RTF_CHAR_SPACE
                            || d == RTF_CHAR_CR
                            || d == RTF_CHAR_LF
                            || d == RTF_CHAR_START_BRACKETS
                            || d == RTF_CHAR_END_BRACKETS
                            ;

                        // コマンドが確定している場合
                        if (commandComplete)
                        {
                            // コマンド解析
                            switch (RtfAnalysis(
                                data: command.ToString()
                                ))
                            {
                                case enmRtfControlCode.Tab:
                                    // テキスト前のタブの場合
                                    if (firstText && !delTabForLine)
                                    {
                                        // タブをクリアする
                                        command.Clear();
                                        delTabForLine = true;
                                    }
                                    break;
                                case enmRtfControlCode.Text:
                                case enmRtfControlCode.LinefeedText:
                                    firstText = false;
                                    break;
                                case enmRtfControlCode.Par:
                                    delTabForLine = false;
                                    firstText = true;
                                    break;
                                case enmRtfControlCode.EndBrackets:
                                    endBrackets = true;
                                    break;
                                case enmRtfControlCode.Empty:
                                case enmRtfControlCode.Control:
                                case enmRtfControlCode.Return:
                                case enmRtfControlCode.Linefeed:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            // コマンドを戻り値に設定
                            ret.Append(command.ToString());

                            // コマンド初期化
                            command.Clear();
                        }
                    }

                    // コマンドに文字追加
                    command.Append(d);
                }

                // commandに文字が残っている場合はそれを出力
                if (command.Length > 0)
                    ret.Append(command.ToString());
#if DEBUG
                string tmp = ret.ToString();
#endif
                return ret.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion リッチテキストの行頭からTabを削除する

        #endregion リッチテキスト

        #region URLとしてブラウザで開く
        /// <summary>
        /// TextをURLとしてブラウザで開く
        /// urlもTextもEmptyの場合、何もしない
        /// 正常終了の場合true
        /// </summary>
        /// <param name="urlText">URL</param>
        /// <returns>正常終了の場合true</returns>
        public static bool UrlProcessStart(
            this string urlText
            )
        {
            try
            {
                // URLが有効かチェックする
                bool ret = urlText.CheckURL();

                // 結果チェック
                if (ret)
                {
                    // 有効な場合、TextをURLとしてWebブラウザで開く
                    System.Diagnostics.Process.Start(
                        fileName: urlText
                        );
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion URLとしてブラウザで開く

        #region URLが有効かチェックする
        /// <summary>
        /// URLが有効かチェックする
        /// 有効な場合、true
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>有効な場合True</returns>
        public static bool CheckURL(
            this string url
            )
        {
            try
            {
                // 引数チェック(空文字チェック)
                if (url.Length < 1) return false;

                // URLが有効かチェックする
                Uri uri = null;
                return Uri.TryCreate(url, UriKind.Absolute, out uri) && null != uri;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion URLが有効かチェックする

        #region フルパスファイル名からディレクトリを取得する
        /// <summary>
        /// フルパスファイル名からディレクトリを取得する
        /// </summary>
        /// <param name="fullPathFileName">フルパスファイル名</param>
        /// <returns>結果</returns>
        public static string GetDirectoryName(
            this string fullPathFileName
            )
        {
            return System.IO.Path.GetDirectoryName(fullPathFileName);
        }
        #endregion フルパスファイル名からディレクトリを取得する

        #region フルパスファイル名から拡張子を取得する
        /// <summary>
        /// フルパスファイル名から拡張子を取得する
        /// </summary>
        /// <param name="fullPathFileName">フルパスファイル名</param>
        /// <returns>結果</returns>
        public static string GetExtension(
            this string fullPathFileName
            )
        {
            return System.IO.Path.GetExtension(fullPathFileName);
        }
        #endregion フルパスファイル名から拡張子を取得する

        #region フルパスファイル名からファイル名を取得する
        /// <summary>
        /// フルパスファイル名からファイル名を取得する
        /// </summary>
        /// <param name="fullPathFileName">フルパスファイル名</param>
        /// <returns>結果</returns>
        public static string GetFileName(
            this string fullPathFileName
            )
        {
            return System.IO.Path.GetFileName(fullPathFileName);
        }
        #endregion フルパスファイル名からファイル名を取得する

        #region フルパスファイル名から拡張子抜きファイル名を取得する
        /// <summary>
        /// フルパスファイル名からファイル名を取得する
        /// </summary>
        /// <param name="fullPathFileName">フルパスファイル名</param>
        /// <returns>結果</returns>
        public static string GetFileNameWithoutExtension(
            this string fullPathFileName
            )
        {
            return System.IO.Path.GetFileNameWithoutExtension(fullPathFileName);
        }
        #endregion フルパスファイル名から拡張子抜きファイル名を取得する

        #region フルパスファイル名からルートディレクトリを取得する
        /// <summary>
        /// フルパスファイル名からルートディレクトリを取得する
        /// </summary>
        /// <param name="fullPathFileName">フルパスファイル名</param>
        /// <returns>結果</returns>
        public static string GetPathRoot(
            this string fullPathFileName
            )
        {
            return System.IO.Path.GetPathRoot(fullPathFileName);
        }
        #endregion フルパスファイル名からルートディレクトリを取得する

        #region フルパスファイル名からディレクトリの一覧を取得します
        /// <summary>
        /// フルパスファイル名からディレクトリの一覧を取得します
        /// 例)
        /// var result = GetDirectoryNameList("c:\aaa\bbb\ccc\ddd.jpg")
        /// result[0] = "c:"
        /// result[1] = "aaa"
        /// result[2] = "bbb"
        /// result[3] = "ddd.jpg"
        /// </summary>
        /// <param name="fullPathFileName">フルパスファイル名</param>
        /// <returns>結果</returns>
        public static string[] GetDirectoryNameList(
            this string fullPathFileName
            )
        {
            return fullPathFileName.Split('\\');
        }
        #endregion フルパスファイル名からディレクトリの一覧を取得します

        #endregion メソッド
    }
}
