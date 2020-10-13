using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用くじ
    /// <summary>
    /// SmallBasic用くじ
    /// 箱の中にくじを入れ、それを引いてゆく方式
    /// BINGOなどに利用する場合は、全ての番号を
    /// ランク指定してください
    /// 引いたくじを箱に戻すことも可能
    /// </summary>
    [SmallBasicType]
    public static class SBLots
    {
        #region メンバ変数
        /// <summary>
        /// 乱数
        /// </summary>
        private static System.Random rnd = new Random(Environment.TickCount + 1);
        /// <summary>
        /// くじセット
        /// </summary>
        private static List<Lots> _Lottery = new List<Lots>();
        /// <summary>
        /// 最後に引いたくじ
        /// </summary>
        private static Lots _LastCast = null;
        #endregion メンバ変数

        #region 公開プロパティ
        /// <summary>
        /// 最小ランク
        /// </summary>
        public static Primitive MinRank
        {
            get { return _Lottery.Min(x => x.Rank); }
        }
        /// <summary>
        /// 最大ランク
        /// </summary>
        public static Primitive MaxRank
        {
            get { return _Lottery.Max(x => x.Rank); }
        }
        /// <summary>
        /// 箱の外のくじ数
        /// 
        /// </summary>
        public static Primitive CastCount
        {
            get { return _Lottery.Where(x => x.Cast).Count(); }
        }
        /// <summary>
        /// 箱の中のくじ数
        /// </summary>
        public static Primitive RemainingCount
        {
            get { return _Lottery.Where(x => !x.Cast).Count(); }
        }
        /// <summary>
        /// 最後に引いたくじの番号
        /// 
        /// </summary>
        public static Primitive LastCastNo
        {
            get { return _LastCast.No; }
        }
        /// <summary>
        /// 最後に引いたくじのランク
        /// </summary>
        public static Primitive LastCastRank
        {
            get { return _LastCast.Rank; }
        }
        /// <summary>
        /// 最後に引いたくじの状態
        /// </summary>
        public static Primitive LastCastFlag
        {
            get { return _LastCast.Cast; }
        }
        #endregion 公開プロパティ

        #region 公開メソッド

        #region くじを生成する
        /// <summary>
        /// くじを生成する
        /// ランク配列は、ランクと枚数を指定します
        /// rank[1] = 1  : 一等(値は本数)
        /// rank[2] = 2  : 二等(値は本数)
        ///    :
        /// </summary>
        /// <param name="rankCounts">ランク配列</param>
        public static void CreateLots(
            Primitive rankCounts
            )
        {
            // くじをクリアする
            _Lottery.Clear();
            _LastCast = null;

            // くじを生成する
            var no = 1;
            for (int rank = 1; rank <= rankCounts.GetItemCount(); rank++)
            {
                for (int i = 0; i < (int)rankCounts[rank]; i++)
                {
                    _Lottery.Add(new Lots
                    {
                        No = no++,
                        Rank = rank,
                        Cast = false
                    });
                }
            }
        }
        #endregion くじを生成する

        #region 引いたくじのランク一覧を取得する
        /// <summary>
        /// 引いたくじのランク一覧を取得します
        /// </summary>
        /// <returns>数値配列</returns>
        public static Primitive GetLotsRankOfCast()
        {
            var result = new Primitive();

            var min = MinRank;
            var max = MaxRank;

            for (int i = min; i <= max; i++)
            {
                result[i] = _Lottery
                    .Where(x => x.Rank == i && x.Cast)
                    .Count();
            }

            return result;
        }
        #endregion 引いたくじのランク一覧を取得する

        #region 残りくじのランク一覧を取得する
        /// <summary>
        /// 残りくじのランク一覧を取得する
        /// </summary>
        /// <returns>数値配列</returns>
        public static Primitive GetLotsRankOfRemaining()
        {
            var result = new Primitive();

            var min = MinRank;
            var max = MaxRank;

            for (int i = min; i <= max; i++)
            {
                result[i] = _Lottery
                    .Where(x => x.Rank == i && !x.Cast)
                    .Count();
            }

            return result;
        }
        #endregion 残りくじのランク一覧を取得する

        #region くじを引き、結果としてランクを取得します
        /// <summary>
        /// くじを引き、結果としてランクを取得します
        /// </summary>
        /// <returns>ランク</returns>
        public static Primitive CastLots()
        {
            var lots = _Lottery.Where(x => !x.Cast).ToList();

            if (lots.Count > 0)
            {
                _LastCast = lots[rnd.Next(0, lots.Count - 1)];

                _LastCast.Cast = true;

                return _LastCast.Rank;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// くじを引き、結果としてランクを取得します
        /// reloadRank以上のランクの場合、引いたくじを戻します
        /// 全てのくじが１回のみの場合、0を指定します
        /// くじが存在しない場合は-1を返します
        /// 例)
        /// 一等１本のみ当たりでその他が外れの場合で
        /// reloadRank = 2と指定した場合
        /// 一等以外の本数は変わらない
        /// </summary>
        /// <param name="reloadRank">指定ランク以上の場合くじを戻す</param>
        /// <returns>ランク</returns>
        public static Primitive CastLots2(
            Primitive reloadRank
            )
        {
            var lots = _Lottery.Where(x => !x.Cast).ToList();

            if (lots.Count > 0)
            {
                _LastCast = lots[rnd.Next(0, lots.Count - 1)];

                if (_LastCast.Rank < reloadRank)
                    _LastCast.Cast = true;

                return _LastCast.Rank;
            }
            else
            {
                return -1;
            }
        }
        #endregion くじを引き、結果としてランクを取得します

        #region くじをリセットします
        /// <summary>
        /// くじをリセットします。
        /// </summary>
        public static void Reset()
        {
            foreach (var l in _Lottery)
                l.Cast = false;
        }
        #endregion くじをリセットします

        #endregion 公開メソッド
    }
    #endregion SmallBasic用くじ

    #region くじクラス
    /// <summary>
    /// くじクラス
    /// </summary>
    internal class Lots
    {
        /// <summary>
        /// 通し番号
        /// </summary>
        public int No { get; set; } = 0;
        /// <summary>
        /// ランク
        /// </summary>
        public int Rank { get; set; } = 0;
        /// <summary>
        /// 引くフラグ
        /// </summary>
        public bool Cast { get; set; } = false;
    }
    #endregion くじクラス
}
