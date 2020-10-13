using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMethod
{
    class SBPoker
    {
    }

    #region ポーカークラス
    /// <summary>
    /// ポーカークラス
    /// HightCardsは最大値のみチェック
    /// </summary>
    internal class Poker : TrumpDeck
    {
        #region 固定値
        /// <summary>
        /// ゲーム名
        /// </summary>
        private const string GAME_NAME = "ポーカー";
        #endregion 固定値

        #region メンバ変数
        /// <summary>
        /// プレイヤーリスト
        /// </summary>
        private Players<TrumpCard> _Players { get; } = new Players<TrumpCard>();
        /// <summary>
        /// 総ベット
        /// </summary>
        private int _TotalBet { get; set; } = 0;
        /// <summary>
        /// ゲーム中フラグ
        /// </summary>
        private bool _NowGame = false;
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// プレイヤー情報を取得する
        /// 存在しない場合nullを返す
        /// </summary>
        /// <param name="name">プレイヤー名</param>
        /// <returns>PlayerInfo</returns>
        public PlayerInfo<TrumpCard> this[string name]
        {
            get
            {
                return this._Players
                    .PlayerList
                    .Where(x => x.Name == name)
                    .FirstOrDefault();
            }
        }
        /// <summary>
        /// ゲーム名
        /// </summary>
        public string GameName
        {
            get { return GAME_NAME; }
        }
        #endregion プロパティ

        #region メソッド

        #region プレイヤー

        #region プレイヤークリア
        /// <summary>
        /// プレイヤークリア
        /// </summary>
        public void ClearPleyer()
        {
            this._Players
                .ClearPlayer();
        }
        #endregion プレイヤークリア

        #region プレイヤー追加
        /// <summary>
        /// プレイヤー追加
        /// 順番を指定しない場合は追加順(開始: 0)になる
        /// 順番を指定する場合は全プレイヤーの順番を指定すること
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="coin">持ち金</param>
        /// <param name="order">順番</param>
        public void AddPlayer(
            string name,
            int coin = 500,
            int order = -1
            )
        {
            // オーダーチェック
            if (order < 0)
                order = this._Players.PlayerList.Count();

            this.AddPlayer(new PlayerInfo<TrumpCard>
            {
                Order = order,
                Name = name,
                Coins = 500,
                HandPoint = 0
            });
        }
        /// <summary>
        /// プレイヤー追加
        /// </summary>
        /// <param name="pi">PlayerInfo</param>
        private void AddPlayer(
            PlayerInfo<TrumpCard> pi
            )
        {
            this._Players
                .AddPlayer(pi);
        }
        #endregion プレイヤー追加

        #region プレイヤーを削除する
        /// <summary>
        /// プレイヤーを削除する
        /// </summary>
        /// <param name="name">プレイヤー名</param>
        public void RemovePlayer(
            string name
            )
        {
            this._Players
                .RemovePlayer(playerName: name);
        }
        #endregion プレイヤーを削除する

        #region 全プレイヤーの手持ちカードをクリアする
        /// <summary>
        /// 全プレイヤーの手持ちカードをクリアする
        /// </summary>
        private void ClearPlayerCards()
        {
            foreach (var p in this._Players.PlayerList)
                p.ClearCards();
        }
        #endregion 全プレイヤーの手持ちカードをクリアする

        #region ゲームを開始する
        /// <summary>
        /// ゲームを開始する
        /// </summary>
        /// <param name="bet">掛け金</param>
        public void StartGame(
            int bet
            )
        {
            // ゲーム中かチェック
            if (this._NowGame)
                throw new ArgumentOutOfRangeException("Playing game now");

            // 初期処理
            this._TotalBet = 0;
            this._NowGame = true;

            // 全プレイヤーの手持ちカードをクリアする
            this.ClearPlayerCards();

            // デッキをリセットする
            this.ResetDeck();

            // 全プレイヤーで回す
            foreach (var p in this._Players.PlayerList)
            {
                // 掛け金を設定
                if (p.BetCoin(bet))
                {
                    // カードを5枚引いて手持ちに加える
                    p.AddCard(
                        this.DrawCards(5));

                    // 総ベット追加
                    this._TotalBet += bet;
                }
            }
        }
        #endregion ゲームを開始する

        #region カードを交換する
        /// <summary>
        /// カードを交換する
        /// 同時に追加の掛け金を設定する
        /// 事前に選択フラグで交換するカードを選んでおくこと
        /// </summary>
        /// <param name="playerName">プレイヤー名</param>
        /// <param name="bet">掛け金</param>
        public void ChangeCard(
            string playerName,
            int bet
            )
        {
            // ゲーム中かチェック
            if (!this._NowGame)
                throw new ArgumentOutOfRangeException("Not playing game");

            // プレイヤー情報取得
            var p = this._Players[playerName];

            // nullの場合何もしない
            if (p == null)
                return;

            // 掛け金を設定
            if (!p.BetCoin(bet))
                return;

            // 総ベット追加
            this._TotalBet += bet;

            // 選択カードを削除する
            p.RemoveCard();

            // 補充カード数取得
            var c = 5 - p.GetCards();

            // カードを補充する
            p.AddCard(this.DrawCards(c));
        }
        #endregion カードを交換する

        #region ゲームを終了し配当のあったプレイヤー名一覧を取得する
        /// <summary>
        /// ゲームを終了し配当のあったプレイヤー名一覧を取得する
        /// 配当も同時に行う
        /// </summary>
        /// <returns>プレイヤー名一覧</returns>
        public IEnumerable<string> EndGame()
        {
            // ゲーム中かチェック
            if (!this._NowGame)
                throw new ArgumentOutOfRangeException("Not playing game");

            // 勝敗判定
            return this.VictoryOrDefeat(this._TotalBet);
        }
        #endregion ゲームを終了し配当のあったプレイヤー名一覧を取得する

        #region プレイヤーカード一覧取得
        /// <summary>
        /// プレイヤーカード一覧取得
        /// </summary>
        /// <param name="playerName">プレイヤー名</param>
        /// <returns>カード一覧</returns>
        private IEnumerable<TrumpCard> PleyerCards(
            string playerName
            )
        {
            // カード一覧取得
            return this._Players[playerName]
                ?.CardList;
        }
        #endregion プレイヤーカード一覧取得

        #endregion プレイヤー

        #region 勝敗処理
        /// <summary>
        /// 勝敗処理を行い勝利者名リストを取得する
        /// </summary>
        /// <returns>勝利者名</returns>
        private IEnumerable<string> VictoryOrDefeat(
            int totalBet
            )
        {
            // 勝者処理を実行する
            var pl = this._Players.PlayerList;
            foreach (var pi in pl)
            {
                // 役ポイント取得
                pi.HandPoint = this.GetHandPoint(pi.CardList);
            }

            // 役ポイント最大値取得
            var maxHP = pl.Max(x => x.HandPoint);

            // 最大役ポイントが0の場合は勝者無(掛け金没収)
            if (maxHP < 1)
                return new List<string>();

            // 最大役ポイントのプレイヤー情報取得
            var pio = pl.Where(x => x.HandPoint == maxHP);

            // 掛け金を振り分ける
            foreach (var pi in pio)
            {
                // 掛け金配当
                pi.Coins += totalBet / pio.Count();
            }

            // 名前一覧を返す
            return pio.Select(x => x.Name);
        }
        #endregion 勝敗処理

        #endregion メソッド

        #region 役処理

        #region 列挙型
        /// <summary>
        /// 役
        /// </summary>
        public enum enmHand : int
        {
            /// <summary>
            /// ゲームから降りる
            /// </summary>
            GetDown = 0,
            /// <summary>
            /// ノーペア
            /// 最大の数字(Aが最大)
            /// 1302540 / 2598960 = 50.11773%
            /// </summary>
            HighCards,
            /// <summary>
            /// ワンペア
            /// 2枚の同じ数字
            /// 1098240 / 2598960 = 42.25690%
            /// </summary>
            OnePair,
            /// <summary>
            /// ツーペア
            /// 2枚の同じ数値 + 2枚の同じ数字
            /// 123552 / 2598960  =  4.75390%
            /// </summary>
            TwoPair,
            /// <summary>
            /// スリーカード
            /// 3枚の同じ数字
            /// 54912 / 2598960   =  2.11284%
            /// </summary>
            ThreeCards,
            /// <summary>
            /// ストレート
            /// 1(A), 2, 3, 4, 5 等の続き番号
            /// Aは14にもなる
            /// 10, 11(J), 12(Q), 13(K), 14(A) も可能
            /// 10200 / 2598960   =  0.39246%
            /// </summary>
            Straidht,
            /// <summary>
            /// フラッシュ
            /// 5枚の同じスート
            /// 5108 / 2598960    =  0.19654%
            /// </summary>
            Flush,
            /// <summary>
            /// フルハウス
            /// 3枚の同じ数字 + 2枚の同じ数字
            /// 3744 / 2598960    =  0.14405%
            /// </summary>
            FullHouse,
            /// <summary>
            /// フォーカード
            /// 4枚の同じ数字
            /// 624 / 2598960     =  0.02400%
            /// </summary>
            FourCards,
            /// <summary>
            /// ストレートフラッシュ
            /// ストレート　かつ　同一スート
            /// 36 / 2598960      =  0.00138%
            /// </summary>
            StraightFlash,
            /// <summary>
            /// ロイヤルストレートフラッシュ
            /// 10,J,Q,K,A かつ 同一スート
            /// 4 / 2598960       =  0.00015%
            /// </summary>
            RoyalStraightFlash,
        }
        #endregion 列挙型

        #region 役ポイントを取得する
        /// <summary>
        /// 役ポイントを取得する
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private int GetHandPoint(
            IEnumerable<TrumpCard> cards
            )
        {
            // 役を取得する
            var ch = this.CheckHand(cards: cards);

            // 点数計算を行い結果を返す
            return ((int)ch.Item1 * 100) + ch.Item2;
        }
        #endregion 役ポイントを取得する

        #region 役チェック
        /// <summary>
        /// 役チェック
        /// 戻り値 : 役,最大カードポイント
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>役, point</returns>
        private Tuple<enmHand, int> CheckHand(
            IEnumerable<TrumpCard> cards
            )
        {
            // 枚数が異なる場合は0点で返す
            if (cards.Count() != 5)
                return new Tuple<enmHand, int>(enmHand.GetDown, 0);

            // 役を調べる
            var royal = this.Royal(cards);
            var straidht = this.Straidht(cards);
            var flash = this.Flash(cards);
            var fourCard = this.FourCard(cards);
            var fullHouse = this.FullHouse(cards);
            var threeCard = this.ThreeCard(cards);
            var twoPair = this.TwoPair(cards);
            var onePair = this.OnePair(cards);

            // カードポイント取得(カードの最大値)
            var maxCardPoint = GetMaxCardPoint(cards: cards);

            // ロイヤルストレートフラッシュ
            if (royal && straidht && flash)
                return new Tuple<enmHand, int>(
                    enmHand.RoyalStraightFlash,
                    maxCardPoint
                    );
            // ストレートフラッシュ
            if (!royal && straidht && flash)
                return new Tuple<enmHand, int>(
                    enmHand.StraightFlash,
                    maxCardPoint
                    );
            // フォーカード
            if (fourCard)
                return new Tuple<enmHand, int>(
                    enmHand.FourCards,
                    maxCardPoint
                    );
            // フルハウス
            if (fullHouse)
                return new Tuple<enmHand, int>(
                    enmHand.FullHouse,
                    maxCardPoint
                    );
            // フラッシュ
            if (flash)
                return new Tuple<enmHand, int>(
                    enmHand.Flush,
                    maxCardPoint
                    );
            // ストレート
            if (straidht)
                return new Tuple<enmHand, int>(
                    enmHand.Straidht,
                    maxCardPoint
                    );
            // スリーカード
            if (threeCard)
                return new Tuple<enmHand, int>(
                    enmHand.ThreeCards,
                    maxCardPoint
                    );
            // ツーペア
            if (twoPair)
                return new Tuple<enmHand, int>(
                    enmHand.TwoPair,
                    maxCardPoint
                    );
            // ワンペア
            if (onePair)
                return new Tuple<enmHand, int>(
                    enmHand.OnePair,
                    maxCardPoint
                    );
            // ハイカード
            return new Tuple<enmHand, int>(
                enmHand.HighCards,
                    maxCardPoint
                );
        }
        #endregion 役チェック

        #region 最大カードポイントを取得する
        /// <summary>
        /// 最大カードポイントを取得する
        /// Aを14とする
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private int GetMaxCardPoint(
            IEnumerable<TrumpCard> cards
            )
        {
            // 最大カードポイントを取得する
            return cards.Max(x => x.OtherRow);
        }
        #endregion 最大カードポイントを取得する

        #region ロイヤルかどうかチェックする
        /// <summary>
        /// ロイヤルかどうかチェックする
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private bool Royal(
            IEnumerable<TrumpCard> cards
            )
        {
            // 最大のカードがAce(14)かチェックする
            return GetMaxCardPoint(cards: cards) == 14;
        }
        #endregion ロイヤルかどうかチェックする

        #region フラッシュかどうかチェックする
        /// <summary>
        /// フラッシュかどうかチェックする
        /// 全て同じスートの場合true
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private bool Flash(
            IEnumerable<TrumpCard> cards
            )
        {
            // 全てのスートが先頭カードと同じ場合
            var suit = cards.First().Suit;
            return cards.Where(x => x.Suit == suit).Count() == 5;
        }
        #endregion フラッシュかどうかチェックする

        #region プレイヤーカードをソートする
        /// <summary>
        /// プレイヤーカードをソートする
        /// Rowでソートする場合true
        /// OtherRowでソートする場合false
        /// </summary>
        /// <param name="cards">プレイヤーカード</param>
        /// <param name="row">対象</param>
        /// <returns></returns>
        private IEnumerable<TrumpCard> SortCard(
            IEnumerable<TrumpCard> cards,
            bool row = true
            )
        {
            // カードが5枚かチェックする
            if (cards.Count() != 5)
                throw new ArgumentOutOfRangeException($"Cards counts({cards.Count()}) is not 5");

            // ソート対象チェック
            if (row)
                // Rowでソートする
                return cards.OrderBy(x => x.Row);
            else
                // OtherRowでソートする
                return cards.OrderBy(x => x.OtherRow);
        }
        #endregion カードをソートする

        #region ストレートかどうかチェックする
        /// <summary>
        /// ストレートかどうかチェックする
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private bool Straidht(
            IEnumerable<TrumpCard> cards
            )
        {
            // Rowでチェック
            var r = this.SortCard(cards: cards, row: true);
            var br = this.CheckStraidht(r);

            // OtherRowでチェック
            var or = this.SortCard(cards: cards, row: false);
            var bor = this.CheckStraidht(or);

            // どちらかがストレートならばtrue
            return br || bor;
        }
        /// <summary>
        /// ストレートかどうかチェックする
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private bool CheckStraidht(
            IEnumerable<TrumpCard> cards
            )
        {
            // List化
            var ob = cards.ToList();

            // ストレート
            // カード値+1が次のカード
            return ob[0].Row == ob[1].Row + 1
                && ob[1].Row == ob[2].Row + 1
                && ob[2].Row == ob[3].Row + 1
                && ob[3].Row == ob[4].Row + 1;
        }
        #endregion ストレートかどうかチェックする

        #region フォーカードかどうかチェックする
        /// <summary>
        /// フォーカードかどうかチェックする
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private bool FourCard(
            IEnumerable<TrumpCard> cards
            )
        {
            // Row順に並べる
            var ob = this.SortCard(cards: cards, row: true).ToList();

            // フォーカード
            // 01234
            // AAAA*
            // *AAAA
            return
                // 01234
                // AAAA*
                ob[0].Row == ob[1].Row
                && ob[1].Row == ob[2].Row
                && ob[2].Row == ob[3].Row
                && ob[3].Row != ob[4].Row
                // 01234
                // *AAAA
                || ob[0].Row != ob[1].Row
                && ob[1].Row == ob[2].Row
                && ob[2].Row == ob[3].Row
                && ob[3].Row == ob[4].Row;
        }
        #endregion フォーカードかどうかチェックする

        #region フルハウスかどうかチェックする
        /// <summary>
        /// フルハウスかどうかチェックする
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private bool FullHouse(
            IEnumerable<TrumpCard> cards
            )
        {
            // Row順に並べる
            var ob = this.SortCard(cards: cards, row: true).ToList();

            // フルハウス
            // 01234
            // AAABB
            // AABBB
            return
                // 01234
                // AAABB
                ob[0].Row == ob[1].Row
                && ob[1].Row == ob[2].Row
                && ob[2].Row != ob[3].Row
                && ob[3].Row == ob[4].Row
                // 01234
                // AABBB
                || ob[0].Row == ob[1].Row
                && ob[1].Row != ob[2].Row
                && ob[2].Row == ob[3].Row
                && ob[3].Row == ob[4].Row;
        }
        #endregion フルハウスかどうかチェックする

        #region スリーカードかどうかチェックする
        /// <summary>
        /// スリーカードかどうかチェックする
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private bool ThreeCard(
            IEnumerable<TrumpCard> cards
            )
        {
            // Row順に並べる
            var ob = this.SortCard(cards: cards, row: true).ToList();

            // スリーカード
            // 01234
            // AAA**
            // *AAA*
            // **AAA
            return
                // 01234
                // AAA**
                ob[0].Row == ob[1].Row
                && ob[1].Row == ob[2].Row
                && ob[2].Row != ob[3].Row
                && ob[3].Row != ob[4].Row
                // 01234
                // *AAA*
                || ob[0].Row != ob[1].Row
                && ob[1].Row == ob[2].Row
                && ob[2].Row == ob[3].Row
                && ob[3].Row != ob[4].Row
                // 01234
                // **AAA
                || ob[0].Row != ob[1].Row
                && ob[1].Row != ob[2].Row
                && ob[2].Row == ob[3].Row
                && ob[3].Row == ob[4].Row;
        }
        #endregion スリーカードかどうかチェックする

        #region ツーペアかどうかチェックする
        /// <summary>
        /// ツーペアかどうかチェックする
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private bool TwoPair(
            IEnumerable<TrumpCard> cards
            )
        {
            // Row順に並べる
            var ob = this.SortCard(cards: cards, row: true).ToList();

            // ツーペア
            // 01234
            // AABB*
            // AA*BB
            // *AABB
            return
                // 01234
                // AABB*
                ob[0].Row == ob[1].Row
                && ob[1].Row != ob[2].Row
                && ob[2].Row == ob[3].Row
                && ob[3].Row != ob[4].Row
                // 01234
                // AA*BB
                || ob[0].Row == ob[1].Row
                && ob[1].Row != ob[2].Row
                && ob[2].Row != ob[3].Row
                && ob[3].Row == ob[4].Row
                // 01234
                // *AABB
                || ob[0].Row != ob[1].Row
                && ob[1].Row == ob[2].Row
                && ob[2].Row != ob[3].Row
                && ob[3].Row == ob[4].Row;
        }
        #endregion ツーペアかどうかチェックする

        #region ワンペアかどうかチェックする
        /// <summary>
        /// ワンペアかどうかチェックする
        /// </summary>
        /// <param name="cards">手札</param>
        /// <returns>結果</returns>
        private bool OnePair(
            IEnumerable<TrumpCard> cards
            )
        {
            // Row順に並べる
            var ob = this.SortCard(cards: cards, row: true).ToList();

            // ワンペア
            // 01234
            // AA***
            // *AA**
            // **AA*
            // ***AA
            return
                // 01234
                // AA***
                ob[0].Row == ob[1].Row
                && ob[1].Row != ob[2].Row
                && ob[2].Row != ob[3].Row
                && ob[3].Row != ob[4].Row
                // 01234
                // *AA**
                || ob[0].Row != ob[1].Row
                && ob[1].Row == ob[2].Row
                && ob[2].Row != ob[3].Row
                && ob[3].Row != ob[4].Row
                // 01234
                // **AA*
                || ob[0].Row != ob[1].Row
                && ob[1].Row != ob[2].Row
                && ob[2].Row == ob[3].Row
                && ob[3].Row != ob[4].Row
                // 01234
                // ***AA
                || ob[0].Row != ob[1].Row
                && ob[1].Row != ob[2].Row
                && ob[2].Row != ob[3].Row
                && ob[3].Row == ob[4].Row;
        }
        #endregion ワンペアかどうかチェックする

        #endregion 役処理
    }
    #endregion ポーカークラス

    #region プレイヤークラス
    /// <summary>
    /// プレイヤークラス
    /// </summary>
    internal class Players<T> where T : ICard
    {
        #region メンバ変数
        /// <summary>
        /// プレイヤーリスト
        /// </summary>
        private List<PlayerInfo<T>> _Players = new List<PlayerInfo<T>>();
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// プレイヤー情報取得
        /// </summary>
        /// <param name="name">プレイヤー名</param>
        /// <returns>PlayerInfo</returns>
        public PlayerInfo<T> this[string name]
        {
            get
            {
                return this._Players
                    .Where(x => x.Name == name)
                    .FirstOrDefault();
            }
        }
        /// <summary>
        /// プレイヤー情報一覧取得
        /// </summary>
        public IEnumerable<PlayerInfo<T>> PlayerList
        {
            get
            {
                return this._Players;
            }
        }
        /// <summary>
        /// プレイヤー名一覧取得
        /// プレイヤーのOrder順に並んでいる
        /// </summary>
        public IEnumerable<string> PlayerNames
        {
            get
            {
                return this._Players
                    .OrderBy(x => x.Order)
                    .Select(x => x.Name);
            }
        }
        #endregion プロパティ

        #region メソッド

        #region プレイヤーをクリアする
        /// <summary>
        /// プレイヤーをクリアする
        /// </summary>
        public void ClearPlayer()
        {
            this._Players
                .Clear();
        }
        #endregion プレイヤーをクリアする

        #region プレイヤーを追加する
        /// <summary>
        /// プレイヤーを追加する
        /// </summary>
        /// <param name="pi">プレイヤー情報</param>
        public void AddPlayer(
            PlayerInfo<T> pi
            )
        {
            this._Players
                .Add(pi);
        }
        #endregion プレイヤーを追加する

        #region プレイヤーを削除する
        /// <summary>
        /// プレイヤーを削除する
        /// </summary>
        /// <param name="playerName"></param>
        public void RemovePlayer(
            string playerName
            )
        {
            this.RemovePlayer(pi: this[playerName]);
        }
        /// <summary>
        /// プレイヤーを削除する
        /// </summary>
        /// <param name="pi">プレイヤー情報</param>
        private void RemovePlayer(
            PlayerInfo<T> pi
            )
        {
            if (pi != null)
                this._Players
                    .Remove(item: pi);
        }
        #endregion プレイヤーを削除する

        #endregion メソッド
    }
    /// <summary>
    /// プレイヤー情報
    /// </summary>
    internal class PlayerInfo<T> where T : ICard
    {
        #region メンバ変数
        /// <summary>
        /// 手札
        /// </summary>
        private List<T> _CardList = new List<T>();
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// 順番
        /// </summary>
        public int Order { get; set; } = -1;
        /// <summary>
        /// プレイヤー名
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 役ポイント
        /// </summary>
        public int HandPoint { get; set; } = 0;
        /// <summary>
        /// 手持ちコイン
        /// </summary>
        public int Coins { get; set; } = 500;
        /// <summary>
        /// カード一覧
        /// </summary>
        public IEnumerable<T> CardList
        {
            get { return this._CardList; }
        }
        #endregion プロパティ

        #region メソッド

        #region 手持ちカードをクリアする
        /// <summary>
        /// 手持ちカードをクリアする
        /// </summary>
        public void ClearCards()
        {
            this._CardList
                .Clear();
        }
        #endregion 手持ちカードをクリアする

        #region 手札カードを追加する
        /// <summary>
        /// 複数枚のカードを手持ちに追加する
        /// </summary>
        /// <param name="cards"></param>
        public void AddCard(
            IEnumerable<T> cards
            )
        {
            foreach (var c in cards)
            {
                this.AddCard(c);
            }
        }
        /// <summary>
        /// 手札カードを追加する
        /// </summary>
        /// <param name="card">カードリスト</param>
        public void AddCard(
            T card
            )
        {
            this._CardList
                .Add(card);
        }
        #endregion 手札カードを追加する

        #region 手持ちカード数を取得する
        /// <summary>
        /// 手持ちカード数を取得する
        /// </summary>
        /// <returns>カード数</returns>
        public int GetCards()
        {
            return this._CardList.Count();
        }
        #endregion 手持ちカード数を取得する

        #region カード存在チェック
        /// <summary>
        /// カード存在チェック
        /// 存在する場合true
        /// </summary>
        /// <param name="cardNo">カード番号</param>
        /// <returns>結果</returns>
        public bool ExistCard(
            int cardNo
            )
        {
            return this._CardList
                .Where(x => x.CardNo == cardNo)
                .Count() > 0;
        }
        #endregion カード存在チェック

        #region 手持ちカードを削除する
        /// <summary>
        /// 手持ちカードを削除する
        /// 事前に選択しておく必要がある
        /// </summary>
        public void RemoveCard()
        {
            var rc = this._CardList
                ?.Where(x => x.Select)
                .Select(x => x.CardNo);

            if (rc != null)
                this.RemoveCard(rc);
        }
        /// <summary>
        /// 複数枚のカードを削除する
        /// </summary>
        /// <param name="cardNos">カード番号一覧</param>
        public void RemoveCard(
            IEnumerable<int> cardNos
            )
        {
            foreach (var no in cardNos)
            {
                this.RemoveCard(no);
            }
        }
        /// <summary>
        /// 手持ちカードを削除する
        /// </summary>
        /// <param name="cardNo">カード番号</param>
        public void RemoveCard(
            int cardNo
            )
        {
            // cardNoのTrumpCard
            var c = this._CardList
                .Where(x => x.CardNo == cardNo)
                .FirstOrDefault();

            // カードを削除する
            this.RemoveCard(c);
        }
        /// <summary>
        /// 複数枚のカードを削除する
        /// </summary>
        /// <param name="cards">カード一覧</param>
        public void RemoveCard(
            IEnumerable<T> cards
            )
        {
            foreach (var c in cards)
            {
                this.RemoveCard(c);
            }
        }
        /// <summary>
        /// 手持ちカードを削除する
        /// </summary>
        /// <param name="card">カード</param>
        public void RemoveCard(
            T card
            )
        {
            // nullチェック
            if (card != null)
                // 削除
                this._CardList
                    .Remove(card);
        }
        #endregion 手持ちカードを削除する

        #region ベットする
        /// <summary>
        /// ベットする
        /// ベットできない場合はfalseを返す
        /// </summary>
        /// <param name="bet">掛け金</param>
        /// <returns>結果</returns>
        public bool BetCoin(
            int bet
            )
        {
            // 掛け金を支払えるかチェック
            if (this.Coins - bet >= 0)
            {
                this.Coins -= bet;
                return true;
            }

            return false;
        }
        #endregion ベットする

        #region 清算する
        /// <summary>
        /// 清算する
        /// </summary>
        /// <param name="profit">儲け</param>
        public void Liquidation(
            int profit
            )
        {
            this.Coins += profit;
        }
        #endregion 清算する

        #endregion メソッド
    }
    #endregion プレイヤー情報クラス
}
