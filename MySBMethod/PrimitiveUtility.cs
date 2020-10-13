using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region Primitiveユーティリティクラス
    /// <summary>
    /// Primitiveユーティリティクラス
    /// </summary>
    [SmallBasicType]
    internal static class PrimitiveUtility
    {
        #region メソッド

        #region 列挙型の名前一覧をPrimitive配列で取得する
        /// <summary>
        /// カルチャータイプ一覧を文字列配列として取得する
        /// </summary>
        /// <returns>結果</returns>
        public static Primitive GetPrimitiveEnumNameList(
            this Type t
            )
        {
            // 戻り値初期化
            var result = new Primitive();

            // 列挙型で回す
            int index = 1;
            foreach (var s in t.GetEnumNames())
            {
                // 要素数は除く
                if (s != "Unknown")
                    result[index++] = s;
            }

            // 結果を返す
            return result;
        }
        #endregion 列挙型の名前一覧をPrimitive配列で取得する

        #region 文字列が列挙型に存在しているかチェックする
        /// <summary>
        /// 文字列が列挙型に存在しているかチェックする
        /// </summary>
        /// <param name="enumType">列挙型のタイプ</param>
        /// <param name="text">文字列</param>
        /// <returns>結果</returns>
        public static bool ExistPrimitiveTextInEnumType(
            this Type enumType,
            Primitive text
            )
        {
            // 存在チェック
            return enumType.GetEnumNames()
                .Any(x => x == text);
        }
        #endregion 文字列が列挙型の名前と一致するかチェックする

        #region 文字列から列挙型値を取得する
        /// <summary>
        /// 文字列から列挙型値を取得する
        /// 事前に文字列が列挙型に存在しているかチェックすること
        /// </summary>
        /// <param name="enumType">列挙型のタイプ</param>
        /// <param name="text">文字列</param>
        /// <returns>結果</returns>
        public static int GetEnumValueOfPrimitiveText(
            this Type enumType,
            Primitive text
            )
        {
            var s = text.ToString();
            return enumType.GetEnumNames()
                .Where(x => x == text)
                .Select((x, index) => index)
                .FirstOrDefault();
        }
        #endregion 文字列が列挙型の名前と一致するかチェックする

        #region Primitive配列をdoubleリストに変換する
        /// <summary>
        /// Primitive配列をdoubleリストに変換する
        /// </summary>
        /// <param name="datas">配列</param>
        /// <returns>結果</returns>
        public static IEnumerable<double> ConvertPrimitiveToDimDouble(
            this Primitive datas
            )
        {
            // 配列数取得
            int itemCounts = datas.GetItemCount();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentOutOfRangeException();

            // 配列生成
            var ret = new List<double>();

            // 配列数繰り返す
            for (int i = 1; i <= itemCounts; i++)
            {
                // 値設定
                ret.Add((double)datas[i]);
            }

            return ret;
        }
        #endregion Primitive配列をdoubleリストに変換する

        #region Primitive配列をintリストに変換する
        /// <summary>
        /// Primitive配列をintリストに変換する
        /// </summary>
        /// <param name="datas">配列</param>
        /// <returns>結果</returns>
        public static IEnumerable<int> ConvertPrimitiveToDimInt(
            this Primitive datas
            )
        {
            // 配列数取得
            int itemCounts = datas.GetItemCount();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentOutOfRangeException();

            // 配列生成
            var ret = new List<int>();

            // 配列数繰り返す
            for (int i = 1; i <= itemCounts; i++)
            {
                // 値設定
                ret.Add((int)datas[i]);
            }

            return ret;
        }
        #endregion Primitive配列をintリストに変換する

        #region Primitive配列をstringリストに変換する
        /// <summary>
        /// Primitive配列をstringリストに変換する
        /// </summary>
        /// <param name="datas">配列</param>
        /// <returns>結果</returns>
        public static IEnumerable<string> ConvertPrimitiveToDimString(
            this Primitive datas
            )
        {
            // 配列数取得
            int itemCounts = datas.GetItemCount();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentOutOfRangeException();

            // 配列生成
            var ret = new List<string>();

            // 配列数繰り返す
            for (int i = 1; i <= itemCounts; i++)
            {
                // 値設定
                ret.Add((string)datas[i]);
            }

            return ret;
        }
        #endregion Primitive配列をstringリストに変換する

        #region Primitive数値配列をKeyValuePairリストに変換する
        /// <summary>
        /// Primitive数値配列をKeyValuePairリストに変換する
        /// </summary>
        /// <param name="datas">配列</param>
        /// <returns>結果</returns>
        public static IEnumerable<KeyValuePair<int, double>> ConvertPrimitiveDoubleToKeyValuePair(
            this Primitive datas
            )
        {
            // 配列数取得
            int itemCounts = datas.GetItemCount();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentOutOfRangeException();

            // 配列生成
            var ret = new List<KeyValuePair<int, double>>();

            // 配列数繰り返す
            for (int i = 1; i <= itemCounts; i++)
            {
                // 値設定
                ret.Add(new KeyValuePair<int, double>(
                    key: i,
                    value: (double)datas[i]
                    ));
            }

            return ret;
        }
        #endregion Primitive文字列配列をKeyValuePairリストに変換する

        #region Primitive文字列配列をKeyValuePairリストに変換する
        /// <summary>
        /// Primitive文字列配列をKeyValuePairリストに変換する
        /// </summary>
        /// <param name="datas">配列</param>
        /// <returns>結果</returns>
        public static IEnumerable<KeyValuePair<int, string>> ConvertPrimitiveStringToKeyValuePair(
            this Primitive datas
            )
        {
            // 配列数取得
            int itemCounts = datas.GetItemCount();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentOutOfRangeException();

            // 配列生成
            var ret = new List<KeyValuePair<int, string>>();

            // 配列数繰り返す
            for (int i = 1; i <= itemCounts; i++)
            {
                // 値設定
                ret.Add(new KeyValuePair<int, string>(
                    key: i, 
                    value: datas[i].ToString()
                    ));
            }

            return ret;
        }
        #endregion Primitive文字列配列をKeyValuePairリストに変換する

        #region datasのkeyを配列番号として配列番号配列を生成し結果を取得する
        /// <summary>
        /// datasのkeyを配列番号として配列番号配列を生成し結果を取得する
        /// </summary>
        /// <param name="datas">キーペアリスト</param>
        /// <returns>結果</returns>
        public static Primitive ConvertKeyValuePairToPrimitive<T>(
            this IEnumerable<KeyValuePair<int, T>> datas
            )
        {
            // 引数を配列に変換
            var d = datas.ToList();

            // 配列数取得
            int itemCounts = d.Count();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentOutOfRangeException();

            // 配列生成
            var ret = new Primitive();

            // 配列数繰り返す
            for (int i = 0; i < itemCounts; i++)
            {
                // 値設定
                ret[i + 1] = d[i].Key;
            }

            return ret;
        }
        #endregion datasのkeyを配列番号として配列番号配列を生成し結果を取得する

        #region double配列をPrimitive配列に変換する
        /// <summary>
        /// double配列をPrimitive配列に変換する
        /// Primitive配列は1から始まる
        /// </summary>
        /// <param name="datas">double配列</param>
        /// <returns>結果</returns>
        public static Primitive ConvertDimDoubleToPrimitive(
            this IEnumerable<double> datas
            )
        {
            // 引数を配列に変換
            var d = datas.ToList();

            // 配列数取得
            int itemCounts = d.Count();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentOutOfRangeException();

            // 配列生成
            var ret = new Primitive();

            // 配列数繰り返す
            for (int i = 0; i < itemCounts; i++)
            {
                // 値設定
                ret[i + 1] = d[i];
            }

            return ret;
        }
        #endregion double配列をPrimitive配列に変換する

        #region int配列をPrimitive配列に変換する
        /// <summary>
        /// int配列をPrimitive配列に変換する
        /// Primitive配列は1から始まる
        /// </summary>
        /// <param name="datas">double配列</param>
        /// <returns>結果</returns>
        public static Primitive ConvertDimIntToPrimitive(
            this IEnumerable<int> datas
            )
        {
            // 引数を配列に変換
            var d = datas.ToList();

            // 配列数取得
            int itemCounts = d.Count();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentOutOfRangeException();

            // 配列生成
            var ret = new Primitive();

            // 配列数繰り返す
            for (int i = 0; i < itemCounts; i++)
            {
                // 値設定
                ret[i + 1] = d[i];
            }

            return ret;
        }
        #endregion int配列をPrimitive配列に変換する

        #region string配列をPrimitive配列に変換する
        /// <summary>
        /// string配列をPrimitive配列に変換する
        /// Primitive配列は1から始まる
        /// </summary>
        /// <param name="datas">string配列</param>
        /// <returns>結果</returns>
        public static Primitive ConvertDimStringToPrimitive(
            this IEnumerable<string> datas
            )
        {
            // 引数を配列に変換
            var d = datas.ToList();

            // 配列数取得
            int itemCounts = d.Count();

            // 引数チェック
            if (itemCounts < 1)
                throw new ArgumentOutOfRangeException();

            // 配列生成
            var ret = new Primitive();

            // 配列数繰り返す
            for (int i = 0; i < itemCounts; i++)
            {
                // 値設定
                ret[i + 1] = d[i];
            }

            return ret;
        }
        #endregion 文字列リストをPrimitive配列に変換する

        #endregion メソッド
    }
    #endregion Primitiveユーティリティクラス
}
