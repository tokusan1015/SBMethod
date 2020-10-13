using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SBMethod
{
    #region ジェネリックユーティリティ
    /// <summary>
    /// ジェネリックユーティリティ
    /// </summary>
    public static class GenericUtil
    {
        #region 固定値
        /// <summary>
        /// シャッフル倍率
        /// </summary>
        public const int DEFAULT_MAGNIFICATION = 10;
        #endregion 固定値

        #region 値を入れ替える
        /// <summary>
        /// 値を入れ替える
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="a">値a</param>
        /// <param name="b">値b</param>
        public static void Swap<T>(
            ref T a,
            ref T b
            )
        {
            T t = a;
            a = b;
            b = t;
        }
        #endregion 値を入れ替える

        #region 要素リストのインデックスA,Bの値を入れ替える
        /// <summary>
        /// 要素リストのインデックスA,Bの値を入れ替える
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="list">要素リスト</param>
        /// <param name="indexA">インデックスA</param>
        /// <param name="indexB">インデックスB</param>
        public static void SwapList<T>(
            this List<T> list,
            int indexA,
            int indexB
            )
        {
            // インデックスA,Bが範囲外の場合例外とする
            if (indexA < 0 || indexA >= list.Count
                || indexB < 0 || indexB >= list.Count
                )
                throw new ArgumentOutOfRangeException();

            // 要素を入れ替える
            var tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp; 
        }
        #endregion 要素リストのインデックスA,Bの値を入れ替える

        #region 要素リストをシャッフルする
        /// <summary>
        /// 要素リストをシャッフルする
        /// </summary>
        /// <remarks>
        /// 開始インデックス以降の要素を
        /// (要素数 - 開始インデックス) * シャッフル倍率
        /// 回数シャッフルする
        /// </remarks>
        /// <typeparam name="T">型</typeparam>
        /// <param name="list">要素一覧</param>
        /// <param name="magnification">シャッフル倍率</param>
        /// <param name="startIndex">開始インデックス</param>
        /// <returns>結果</returns>
        public static void Shuffle<T>(
            this List<T> list,
            int magnification = DEFAULT_MAGNIFICATION,
            int startIndex = 0
            )
        {
            // ランダムを生成
            var rnd = new Random();

            // 要素数*倍率回シャッフルする
            var shuffleCount = (list.Count - startIndex) * magnification;
            for (int i = 0; i < shuffleCount; i++)
            {
                // インデックスA,Bを生成
                var indexA = rnd.Next(minValue: startIndex, maxValue: list.Count);
                var indexB = rnd.Next(minValue: startIndex, maxValue: list.Count);

                // 入れ替える
                SwapList(
                    list: list,
                    indexA: indexA,
                    indexB: indexB
                    );
            }
        }
        #endregion 要素一覧をシャッフルする

        #region 全ての要素を文字列フォーマット変換する
        /// <summary>
        /// 全ての要素を文字列フォーマット変換する
        /// </summary>
        /// <param name="items">配列</param>
        /// <param name="format">フォーマット</param>
        /// <param name="provider">IFormatProvider</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<string> ToFormat<T>(
            this IEnumerable<T> items,
            string format,
            IFormatProvider provider = null
            ) where T : IFormattable
        {
            // 要素
            return items.Select(x => x.ToString(format, provider));
        }
        #endregion 全ての要素を文字列フォーマット変換する


        #region 重複データ件数を取得する
        /// <summary>
        /// 重複データ件数を取得する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="list">リスト</param>
        /// <returns>結果</returns>
        public static int CountOfSameData<T>(
            List<T> list
            )
        {
            // 元データ件数を取得する
            var baseCounts = list.Count;

            // 重複データを削除後のデータ件数を取得する
            var distinctCounts = list.Distinct().Count();

            // 元データ件数から重複データ削除後件数を引く
            return baseCounts - distinctCounts;
        }
        #endregion 重複データ件数を取得する
    }
    #endregion ジェネリックユーティリティ
}
