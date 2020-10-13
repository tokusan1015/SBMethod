using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SBMethod
{
    #region ユーティリティクラス
    /// <summary>
    /// ユーティリティクラス
    /// </summary>
    internal static class Utility
    {
        #region メンバ変数
        /// <summary>
        /// 乱数
        /// </summary>
        private static System.Random _Random = new Random(Environment.TickCount + 1);
        #endregion メンバ変数

        #region メソッド

        #region aとbを入れ替える
        /// <summary>
        /// aとbを入れ替える
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="a">データ</param>
        /// <param name="b">データ</param>
        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }
        #endregion aとbを入れ替える

        #region Arrayをオブジェクト配列に変換する
        /// <summary>
        /// Arrayをオブジェクト配列に変換する
        /// </summary>
        /// <param name="data">Array</param>
        /// <returns>結果</returns>
        public static object[] ConvertObjects<T>(
            this T[] data
            )
        {
            object[] result = new object[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                result[i] = data[i];
            }

            return result;
        }
        #endregion Arrayをオブジェクト配列に変換する

        #region 指定範囲の文字コードを文字に変換し文字列として取得する
        /// <summary>
        /// 指定範囲の文字コードを文字に変換し文字列として取得する
        /// </summary>
        /// <param name="start">開始文字コード</param>
        /// <param name="end">終了文字コード</param>
        /// <returns>結果</returns>
        public static string GetCharCodeString(
            int start,
            int end
            )
        {
            // 戻り値初期化
            var result = new StringBuilder();

            // 文字コードで回す
            for (int i = start; i <= end; i++)
            {
                // 文字を追加する
                result.Append(Convert.ToChar(i));
            }

            // 結果を返す
            return result.ToString();
        }
        #endregion 指定範囲の文字コードを文字に変換し文字列として取得する

        #region ループインデックスを取得する
        /// <summary>
        /// ループインデックスを取得する
        /// 0からサイズまでをループするインデックスを生成する
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="size">サイズ</param>
        /// <returns>結果</returns>
        public static int GetLoopIndex(
            this int index,
            int size
            )
        {
            // 余りを求める
            var result = index % size;

            // 結果を返す
            return result >= 0
                ? result
                : size + result;
        }
        #endregion ループインデックスを取得する

        #region 同一データの件数を取得する
        /// <summary>
        /// 同一データの件数を取得する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="ie">IEnumerable</param>
        /// <returns>結果</returns>
        public static int GetCountsOfSameData<T>(
            this IEnumerable<T> ie
            )
        {
            // 元データ件数を取得する
            var baseCounts = ie.Count();

            // 重複データを削除後のデータ件数を取得する
            var distinctCounts = ie.Distinct().Count();

            // 元データ件数から重複データ削除後件数を引く
            return baseCounts - distinctCounts;
        }
        #endregion 同一データの件数を取得する

        #region フラグ管理
        /// <summary>
        /// 管理フラグをセットする
        /// </summary>
        /// <param name="flags">管理フラグデータ</param>
        /// <param name="flag">対象フラグ</param>
        /// <returns>結果</returns>
        public static int SetManagementFlags(
            this Enum flags,
            Enum flag
            )
        {
            return flags.intParse() | flag.intParse();
        }
        /// <summary>
        /// 管理フラグをリセットする
        /// </summary>
        /// <param name="flags">管理フラグデータ</param>
        /// <param name="flag">対象フラグ</param>
        /// <returns>結果</returns>
        public static int ResetManagementFlags(
            this Enum flags,
            Enum flag
            )
        {
            return flags.intParse() & ~flag.intParse();
        }
        /// <summary>
        /// 管理フラグを検証する
        /// </summary>
        /// <param name="flags">管理フラグデータ</param>
        /// <param name="flag">対象フラグ</param>
        /// <returns>結果</returns>
        public static bool CheckManagementFlags(
            this Enum flags,
            Enum flag
            )
        {
            return flags.HasFlag(flag);
        }
        #endregion イベント条件フラグをセットする

        #endregion フラグ管理

        #region リストのファイル出力
        /// <summary>
        /// リストのファイル出力
        /// </summary>
        /// <param name="list">リスト</param>
        /// <param name="filePathName">ファイルパス名</param>
        public static void OutputList(
            this IEnumerable<string> list,
            string filePathName
            )
        {
            // リストにする
            var l = list.ToList();

            // ファイルをオープンする
            using (StreamWriter w = new StreamWriter(filePathName))
            {
                // 配列数繰り返す
                for (int i = 0; i < l.Count(); i++)
                {
                    // データ書き出し
                    w.WriteLine(l[i]);
                }
            }
        }
        #endregion 配列のファイル出力

        #region XMLファイルとして書き出す
        /// <summary>
        /// オブジェクトをXMLファイルとして保存する
        /// </summary>
        /// <param name="value">オブジェクト</param>
        /// <param name="filePathName">ファイルパス名</param>
        public static void WriteXml<T>(
            this T value,
            string filePathName
            )
        {
            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(value.GetType());
            
            //書き込むファイルを開く（UTF-8 BOM無し）
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(
                filePathName, false, new System.Text.UTF8Encoding(false)))
            {
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, value);

                //ファイルを閉じる
                sw.Close();
            }
        }
        #endregion XMLファイルとして書き出す

        #region XMLファイルを読み込む
        /// <summary>
        /// XMLファイルを読み込む
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="filePathName">ファイルパス名</param>
        /// <returns>結果</returns>
        public static T ReadXML<T>(
            this string filePathName
            )
        {
            //XmlSerializerオブジェクトを作成
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(T));

            // 戻り値生成
            T result;

            //読み込むファイルを開く
            using (System.IO.StreamReader sr = new System.IO.StreamReader(
                filePathName, new System.Text.UTF8Encoding(false)))
            {
                //XMLファイルから読み込み、逆シリアル化する
                result = (T)serializer.Deserialize(sr);
                //ファイルを閉じる
                sr.Close();
            }

            return result;
        }
        #endregion XMLファイルを読み込む

        #region nの階乗(n!)を計算する
        /// <summary>
        /// nの階乗(n!)を計算する
        /// n >= 0
        /// 負数の場合例外発生
        /// </summary>
        /// <param name="n">n</param>
        /// <returns>n!</returns>
        public static decimal CalcFactorial(
            this int n
            )
        {
            // 負数チェック
            if (n < 0)
                throw new ArgumentOutOfRangeException("n is less 0");

            // nの階乗を計算する
            return _CalcFactorial(n);
        }
        private static decimal _CalcFactorial(
            int n
            )
        {
            // オーバーフローをチェックする
            checked
            {
                return n == 0 ? 1L : n * _CalcFactorial(n - 1);
            }
        }
        #endregion 階乗(!)を計算する

        #region nPm(Permutation)を計算する
        /// <summary>
        /// 異なる n個のものから m個を選んで並べる順列の総数
        /// nPm = n! / (n - m)!
        /// nPm = n(n - 1)(n - 2) ... (n - m + 1)
        /// </summary>
        /// <param name="n">元の数</param>
        /// <param name="m">選ぶ数</param>
        /// <returns>並べ方数</returns>
        public static decimal CalcPermutation(
            int n,
            int m
            )
        {
            decimal result = 1;
            int f = n - m;

            while (n > f)
            {
                result *= n--;
            }

            return result;
        }
        #endregion nPm(Permutation)を計算する

        #region nCm(Combination)を計算する
        /// <summary>
        /// nCm(Combination)を計算する
        /// 異なるｎ個のもの(元の数)から異なるｍ個のもの(選ぶ数)を
        /// 並べる順番の違いを区別せずに並べたもの＝重複（ちょうふく）を
        /// 持たない組合せ
        /// nCm = n! / (m! * (n - m)!)
        /// nCm = nPm / m!
        /// nCm = n * (n - 1) * (n - 2) ... (n - m + 1) / (m * (m - 1) * (m - 2) ... * 1)
        /// </summary>
        /// <param name="n">元の数</param>
        /// <param name="m">選ぶ数</param>
        /// <returns>組み合わせ数</returns>
        public static decimal CalcCombination(
            int n,
            int m
            )
        {
            // nCm = n * (n - 1) * (n - 2) ... (n - m + 1) / (m * (m - 1) * (m - 2) ... * 1)
            decimal result = 1;
            int f = n - m;

            while (n > f)
            {
                result *= (n-- / (m <= 1 ? 1 : m--));
            }

            return result;
        }
        #endregion nCm(コンビネーション)を計算する

        #region 確率結果を取得する
        /// <summary>
        /// 確率結果を取得する
        /// nパーセントの確率でtrueが返る
        /// </summary>
        /// <param name="n">パーセント値(33)</param>
        /// <param name="max">パーセントの最大値(100)</param>
        /// <returns>Bool</returns>
        public static bool GetParcentage(
            int n = 33,
            int max = 100
            )
        {
            // 確率データ生成
            return _Random.Next(1, max) <= n;
        }
        #endregion 確率結果を取得する

        #region 多角形の内角の和を求める
        /// <summary>
        /// n角形の内角の和を求める
        /// </summary>
        /// <param name="n">n角形</param>
        /// <returns>内角の和(度)</returns>
        public static int GetSumOfInnerCorner(
            int n
            )
        {
            return 180 * (n - 2);
        }
        #endregion 多角形の内角の和を求める

    }
    #endregion ユーティリティクラス
}
