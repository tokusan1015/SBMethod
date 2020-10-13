using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用サイコロ
    /// <summary>
    /// SmallBasic用サイコロ
    /// </summary>
    [SmallBasicType]
    public static class SBDice
    {
        #region メンバ変数
        /// <summary>
        /// 乱数
        /// </summary>
        private static System.Random _Random = new Random(Environment.TickCount + 1);
        #endregion メンバ変数

        #region 公開メソッド

        #region サイコロを振る
        /// <summary>
        /// 6面サイコロを1個振り、結果を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive Shake6()
        {
            // 固定値
            const int count = 1;
            const int sideCount = 6;

            // 結果を返す
            return ShakeS_nTimes(
                nTimes: count,
                sideCount: sideCount
                );
        }
        /// <summary>
        /// 6面サイコロをn回振り、結果を数値で取得します
        /// </summary>
        /// <param name="nTimes">回数</param>
        /// <returns>数値</returns>
        public static Primitive Shake6_nTimes(
            Primitive nTimes
            )
        {
            // 固定値
            const int sideCount = 6;

            // 結果を返す
            return ShakeS_nTimes(
                nTimes: nTimes,
                sideCount: sideCount
                );
        }
        /// <summary>
        /// sideCount面サイコロをnTimes個振り、結果を数値で取得します
        /// </summary>
        /// <param name="nTimes">個数</param>
        /// <param name="sideCount">面数</param>
        /// <returns>数値</returns>
        public static Primitive ShakeS_nTimes(
            Primitive nTimes,
            Primitive sideCount
            )
        {
            // 固定値
            const int DICE_MIN_VALUE = 1;

            // 戻り値初期化
            int ret = 0;
            int maxValue = sideCount + 1;

            // count個振る
            for (int i = 0; i < nTimes; i++)
            {
                // ダイスを振る
                ret += _Random.Next(
                    minValue: DICE_MIN_VALUE,
                    maxValue: maxValue
                    );
            }

            // 結果を返す
            return ret;
        }
        /// <summary>
        /// 数値配列に指定された面ダイスをnTimes回振り、
        /// 結果を数値で返します
        /// 例)
        /// 6面, 12面, 100面ダイスをnTimes回振る場合
        /// d[1] = 6
        /// d[2] = 12
        /// d[3] = 100
        /// </summary>
        /// <param name="nTimes">振る回数</param>
        /// <param name="sideCounts">数値配列</param>
        /// <returns>数値</returns>
        public static Primitive ShakeManyDice(
            Primitive nTimes,
            Primitive sideCounts
            )
        {
            // 戻り値初期化
            int result = 0;

            // 面ダイスを1回づつ振る
            for (int i = 1; i <= sideCounts.GetItemCount(); i++)
            {
                result += ShakeS_nTimes(nTimes: (int)nTimes, sideCount: (int)sideCounts[i]);
            }

            return result;
        }

        /// <summary>
        /// n面ダイスを振った結果を重み配列の値(割合)の配列番号として取得します
        /// 失敗した場合は-1を取得します
        /// 重み配列)
        ///   重み配列の値には割合を総和がnになるように設定します
        /// 例1)
        /// 　100面ダイスの割合として取得する場合
        /// 　sideCounts = 100
        ///   weights[1] = 1  :  1/100 =  1%
        ///   weights[2] = 2  :  2/100 =  2%
        ///   weights[3] = 11 : 11/100 = 11%
        ///   weights[4] = 41 : 41/100 = 41%
        ///   weights[5] = 45 : 45/100 = 45%
        ///   実行結果は1-5の値として取得します
        /// 例2)
        /// 　1000面ダイスの割合として取得する場合
        /// 　sideCounts = 100
        ///   weights[1] = 15  :  15/1000 =  1.5%
        ///   weights[2] = 25  :  25/1000 =  2.5%
        ///   weights[3] = 100 : 100/1000 = 10.0%
        ///   weights[4] = 417 : 417/1000 = 41.7%
        ///   weights[5] = 443 : 443/1000 = 44.3%
        ///   実行結果は1-5の値として取得します
        /// </summary>
        /// <param name="sideCounts">面数</param>
        /// <param name="weights">重み配列</param>
        /// <returns>数値</returns>
        public static Primitive ShakeDiceWeight(
            Primitive sideCounts,
            Primitive weights
            )
        {
            // 配列化
            var list = weights.ConvertPrimitiveToDimInt().ToList();

            // %値の総和がnにならない場合は例外とする
            if (list.Sum() != (int)sideCounts)
                throw new ArgumentOutOfRangeException($"weights({list.Sum()}) sum is not {sideCounts})");

            // n面ダイスを振る
            var rnd = _Random.Next(minValue: 1, maxValue: sideCounts + 1);

            // 重み値と比較して結果を返す
            var sum = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                sum += list[i];
                if (rnd <= sum)
                    return i + 1;
            }

            return -1;
        }
        /// <summary>
        /// n面ダイスを振った結果を重み配列の各値と比較し配列番号を取得します
        /// 重み配列)
        ///   重み配列の値には範囲を指定します
        ///   weights[n] ≦ 乱数 ＜ weights[n + 1]の範囲の場合nを取得します
        ///   配列数は2以上に設定します
        /// 例)
        ///   sideCounts = 100
        ///   weights[1] = 1
        ///   weights[2] = 2
        ///   weights[3] = 11
        ///   weights[4] = 41 の場合
        ///   1  ≦ 乱数 ＜ 2   : 1を取得します
        ///   2  ≦ 乱数 ＜ 11  : 2を取得します
        ///   11 ≦ 乱数 ＜ 41  : 3を取得します
        ///   41 ≦ 乱数        : 4を取得します
        /// </summary>
        /// <param name="sideCounts">面数</param>
        /// <param name="weights">重み配列</param>
        /// <returns>数値</returns>
        public static Primitive ShakeDiceWeight2(
            Primitive sideCounts,
            Primitive weights
            )
        {
            // 引数チェック
            if (sideCounts < 2)
                throw new ArgumentOutOfRangeException($"sideCounts({sideCounts}) is grater than 2");
            if (weights.GetItemCount() < 2)
                throw new ArgumentOutOfRangeException($"weights({weights.GetItemCount()}) array count is grater than 2");

            // n面ダイスを振る
            var rnd = _Random.Next(minValue: 1, maxValue: sideCounts + 1);

            // 重み配列と比較する
            for (int i = 1; i < weights.GetItemCount(); i++)
            {
                // weights[n] ≦ 乱数 ＜ weights[n + 1]
                if ((int)weights[i] <=  rnd
                    && rnd < (int)weights[i]
                    )
                    return i;
            }

            return weights.GetItemCount();
        }

        #endregion サイコロを振る

        #region 正規分布乱数取得
        /// <summary>
        /// 正規分布乱数を取得する
        /// -6 ～ +6 の範囲の乱数を取得する
        /// 但し偏りは標準偏差1、平均値0の正規分布に従う
        /// 実働では-4～4の範囲内にほぼ収まる
        /// </summary>
        /// <returns>結果</returns>
        public static Primitive NormalDistribution()
        {
            double avg = 0;

            // 指定回数繰り返す
            for (int i = 0; i < 12; i++)
            {
                // 0 ≦ x ＜ 1の乱数を加算する
                avg += _Random.NextDouble();
            }

            // 加算数で割る
            return (int)(avg - 6);
        }
        #endregion 正規分布乱数取得

        #endregion 公開メソッド
    }
    #endregion SmallBasic用独自サイコロ
}
