using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用トランプ
    /// <summary>
    /// SmallBasic用トランプ
    /// デッキ)
    ///  初期状態ではジョーカーは追加されていません
    ///  追加する場合は AddJoker メソッドを実行します
    /// カード番号)
    ///   ハート   A(1) - K(13) =  0 - 12
    ///   クラブ   A(1) - K(13) = 13 - 25
    ///   ダイヤ   A(1) - K(13) = 26 - 38
    ///   スペード A(1) - K(13) = 39 - 51
    ///   ジョーカー 52以降
    /// スート)
    ///   ジョーカー = 0, ハート = 1, クラブ = 2, ダイヤ = 3, スペード = 4 
    /// ランク)
    ///   ジョーカー = 0,
    ///   A = 1 or 14, 2 = 2, 3 = 3, 4 = 4, 5 = 5, 6 = 6,
    ///   7 = 7, 8 = 8, 9 = 9, 10 = 10, J = 11, Q = 12, K = 13
    /// </summary>
    [SmallBasicType]
    public static class SBTrump
    {
        #region メンバ変数
        /// <summary>
        /// トランプデッキ
        /// </summary>
        private static TrumpDeck _TrumpDeck = new TrumpDeck();
        #endregion メンバ変数

        /// <summary>
        /// ジョーカーを追加します
        /// </summary>
        /// <param name="count">枚数</param>
        public static void AddJoker(
            Primitive count
            )
        {
            _TrumpDeck.AddJoker((int)count);
        }

        /// <summary>
        /// デッキをリセットします
        /// 引いたカードを全てデッキに戻します
        /// </summary>
        public static void Reset()
        {
            _TrumpDeck.ResetDeck();
        }

        /// <summary>
        /// デッキ内のカード数を取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive CardsCountOfDeck()
        {
            // 念のため
            if (_TrumpDeck == null)
                throw new ArgumentNullException("TrumpDeckが生成されていません");

            return _TrumpDeck.ExistsCardsCount;
        }

        /// <summary>
        /// カードを1枚引きカード番号を取得します
        /// 事前にカードがあるか確認する必要があります
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive DrawNextCardNo()
        {
            // カードを引く
            return _TrumpDeck.DrawCard().CardNo;
        }

        /// <summary>
        /// カードを1枚引きカード情報を取得します
        /// 事前にカードがあるか確認する必要があります
        /// ret[1] = カード番号
        /// ret[2] = スート
        /// ret[3] = ランク
        /// </summary>
        /// <returns>数値配列</returns>
        public static Primitive DrawNextCard()
        {
            // カードを引く
            var c = _TrumpDeck.DrawCard();

            var result = new Primitive();

            // 戻り値設定
            result[1] = c.CardNo;
            result[2] = (int)c.Suit;
            result[3] = (int)c.Rank;

            return result;
        }
    }
    #endregion SmallBasic用トランプ

    #region トランプデッキクラス
    /// <summary>
    /// トランプデッキクラス
    /// ジョーカーはA(14)よりも強い設定
    /// </summary>
    internal class TrumpDeck : CardDeck<TrumpCard>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrumpDeck()
        {
            // カードリストを生成する
            var cardList = new List<TrumpCard>();

            // 全てのマークで繰り返す
            foreach (var s in typeof(TrumpCard.enmSuit).GetEnumValues())
            {
                // ジョーカーは無視する
                if ((TrumpCard.enmSuit)s != TrumpCard.enmSuit.Joker)
                {
                    // 全てのランクで繰り返す
                    foreach (var r in typeof(TrumpCard.enmRank).GetEnumValues())
                    {
                        // ジョーカーは無視する
                        if ((TrumpCard.enmRank)r != TrumpCard.enmRank.Joker)
                        {
                            // Aceを14とする場合
                            var otherRow = (int)r;
                            if ((TrumpCard.enmRank)r == TrumpCard.enmRank.Rank_Ace)
                                otherRow = 14;

                            // トランプを追加する
                            cardList.Add(
                                new TrumpCard
                                {
                                    CardNo = cardList.Count,
                                    InDeck = true,
                                    Suit = (TrumpCard.enmSuit)s,
                                    Rank = (TrumpCard.enmRank)r,
                                    Row = (int)r,
                                    OtherRow = otherRow
                                });
                        }
                    }
                }
            }

            // 初期化実行
            base.Init(
                cardList: cardList
                );
        }
        #endregion コンストラクタ

        #region ジョーカーを追加します
        /// <summary>
        /// ジョーカーを追加します
        /// </summary>
        /// <param name="joker">枚数</param>
        public void AddJoker(
            int joker
            )
        {
            // ジョーカーを追加する
            for (int i = 0; i < joker; i++)
            {
                // トランプを追加する
                base.Add(
                    new TrumpCard
                    {
                        CardNo = this.Count,
                        InDeck = true,
                        Suit = TrumpCard.enmSuit.Joker,
                        Rank = TrumpCard.enmRank.Joker,
                        Row = (int)15,
                        OtherRow = 15
                    });
            }

            // デッキをリセットする
            base.ResetDeck();
        }
        #endregion ジョーカーを追加します
    }
    #endregion トランプデッキクラス

    #region トランプカードクラス
    /// <summary>
    /// トランプカードインターフェース
    /// </summary>
    interface ITrump : ICard
    {
        /// <summary>
        /// 表面表示フラグ
        /// </summary>
        bool Surface { get; set; }
        /// <summary>
        /// スート
        /// </summary>
        TrumpCard.enmSuit Suit { get; set; }
        /// <summary>
        /// ランク
        /// </summary>
        TrumpCard.enmRank Rank { get; set; }
        /// <summary>
        /// 並び
        /// </summary>
        int Row { get; set; }
        /// <summary>
        /// 他の並び
        /// </summary>
        int OtherRow { get; set; }
    }
    /// <summary>
    /// トランプカードクラス
    /// </summary>
    internal class TrumpCard : ITrump
    {
        #region 列挙体
        /// <summary>
        /// マーク
        /// </summary>
        public enum enmSuit : int
        {
            /// <summary>
            /// ジョーカー
            /// </summary>
            Joker = 0,
            /// <summary>
            /// ハート
            /// </summary>
            Hart,
            /// <summary>
            /// クラブ
            /// </summary>
            Club,
            /// <summary>
            /// ダイヤ
            /// </summary>
            Diamond,
            /// <summary>
            /// スペード
            /// </summary>
            Spade,
        }
        /// <summary>
        /// ランク
        /// </summary>
        public enum enmRank : int
        {
            /// <summary>
            /// ジョーカー
            /// </summary>
            Joker = 0,
            /// <summary>
            /// エース
            /// </summary>
            Rank_Ace,
            /// <summary>
            /// ２
            /// </summary>
            Rank_2,
            /// <summary>
            /// ３
            /// </summary>
            Rank_3,
            /// <summary>
            /// ４
            /// </summary>
            Rank_4,
            /// <summary>
            /// ５
            /// </summary>
            Rank_5,
            /// <summary>
            /// ６
            /// </summary>
            Rank_6,
            /// <summary>
            /// ７
            /// </summary>
            Rank_7,
            /// <summary>
            /// ８
            /// </summary>
            Rank_8,
            /// <summary>
            /// ９
            /// </summary>
            Rank_9,
            /// <summary>
            /// １０
            /// </summary>
            Rank_10,
            /// <summary>
            /// ジャック
            /// </summary>
            Rank_Jack,
            /// <summary>
            /// クイーン
            /// </summary>
            Rank_Queen,
            /// <summary>
            /// キング
            /// </summary>
            Rank_King,
        }
        #endregion 列挙体

        #region プロパティ
        /// <summary>
        /// カード番号
        /// </summary>
        public int CardNo { get; set; } = -1;
        /// <summary>
        /// デッキ内存在フラグ
        /// </summary>
        public bool InDeck { get; set; } = true;
        /// <summary>
        /// 表面表示フラグ
        /// </summary>
        public bool Surface { get; set; } = true;
        /// <summary>
        /// スート
        /// </summary>
        public enmSuit Suit { get; set; } = enmSuit.Hart;
        /// <summary>
        /// ランク
        /// </summary>
        public enmRank Rank { get; set; } = enmRank.Rank_Ace;
        /// <summary>
        /// 並び
        /// </summary>
        public int Row { get; set; } = 0;
        /// <summary>
        /// 他の並び
        /// </summary>
        public int OtherRow { get; set; } = 0;
        /// <summary>
        /// 選択フラグ
        /// </summary>
        public bool Select { get; set; } = false;
        #endregion プロパティ
    }
    #endregion トランプカードクラス
}
