using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMethod
{

    #region カードデッキクラス
    /// <summary>
    /// カードデッキインターフェース
    /// </summary>
    interface ICard
    {
        /// <summary>
        /// カード番号(カードの連番)
        /// </summary>
        int CardNo { get; set; }
        /// <summary>
        /// デッキ内存在フラグ
        /// </summary>
        bool InDeck { set; get; }
        /// <summary>
        /// 選択フラグ
        /// </summary>
        bool Select { set; get; }
    }
    /// <summary>
    /// カードデッキクラス
    /// カードの管理を行う
    /// カードを引く等の関数
    /// </summary>
    internal abstract class CardDeck<TCard> where TCard : ICard
    {
        #region メンバ変数
        /// <summary>
        /// 初期化完了フラグ
        /// </summary>
        private bool _Init = false;
        /// <summary>
        /// カードリスト
        /// </summary>
        private List<TCard> _CardList = null;
        /// <summary>
        /// ランダム
        /// </summary>
        private Random _Random = new Random(Seed: Environment.TickCount + 1);
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// デッキを取得する
        /// </summary>
        public IEnumerable<TCard> Deck
        {
            get
            {
                // 未初期化例外
                this.CheckInit();

                // カードリスト
                return this._CardList;
            }
        }

        /// <summary>
        /// カード枚数
        /// </summary>
        public int Count
        {
            get { return this._CardList.Count; }
        }

        /// <summary>
        /// 引いたカード一覧
        /// </summary>
        public IEnumerable<TCard> CastingCards
        {
            get
            {
                // 未初期化例外
                this.CheckInit();

                // 引いたカード一覧
                return this._CardList
                    .Where(x => !x.InDeck);
            }
        }

        /// <summary>
        /// 残りカード一覧
        /// </summary>
        public IEnumerable<TCard> RemainingCards
        {
            get
            {
                // 未初期化例外
                this.CheckInit();

                // 引いたカード一覧
                return this._CardList
                    .Where(x => x.InDeck);
            }
        }

        /// <summary>
        /// カードを取得する
        /// 存在しない場合nullを返す
        /// </summary>
        /// <param name="cardNo">カード番号</param>
        /// <returns>カード</returns>
        public TCard this[int cardNo]
        {
            get
            {
                // 未初期化例外
                this.CheckInit();

                // カードを取得する
                return this._CardList
                    .Where(x => x.CardNo == cardNo)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// 引いたカードの枚数
        /// </summary>
        public int DrawCount
        {
            get
            {
                // 未初期化例外
                this.CheckInit();

                // 引いたカードの枚数
                return this.CastingCards
                    .Count();
            }
        }

        /// <summary>
        /// 次に引くカードがある場合true
        /// </summary>
        public bool ExistsCards
        {
            get
            {
                // 次に引くカードがある場合true
                return this.ExistsCardsCount > 0;
            }
        }

        /// <summary>
        /// 残っているカード数
        /// </summary>
        public int ExistsCardsCount
        {
            get
            {
                // 未初期化例外
                this.CheckInit();

                // 残っているカード数
                return this.RemainingCards
                    .Count();
            }
        }
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CardDeck()
        {
            // 未初期化に設定
            this._Init = false;
        }
        #endregion コンストラクタ

        #region メソッド

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="cardList">カードリスト</param>
        protected void Init(
            IEnumerable<TCard> cardList
            )
        {
            // cardListチェック
            this.CheckList(cardList);

            // カードリストを設定
            this._CardList = cardList.ToList();

            // 初期化完了
            this._Init = true;

            // インデックスをクリアする
            this.ResetDeck();
        }
        /// <summary>
        /// 初期化チェック
        /// </summary>
        /// <returns>true</returns>
        private bool CheckInit()
        {
            // 未初期化例外
            if (!this._Init)
                throw new NotExecuteInitializeException("Not execute initialize");

            return true;
        }
        /// <summary>
        /// リストの件数チェック
        /// </summary>
        /// <param name="cards">IEnumerable</param>
        /// <returns>true</returns>
        private bool CheckList(
            IEnumerable<TCard> cards
            )
        {
            // nullチェック
            if (cards == null)
                throw new ArgumentNullException("Cards is null");

            // 件数チェック
            if (cards.Count() < 1)
                throw new ArgumentOutOfRangeException($"Cards count({cards.Count()}) is grater than 1");

            return true;
        }
        #endregion 初期化処理

        #region カードを追加する
        /// <summary>
        /// カードを追加する
        /// </summary>
        /// <param name="card">カード</param>
        public void Add(
            TCard card
            )
        {
            this._CardList.Add(card);
        }
        #endregion カードを追加する

        #region カードをデッキに戻す
        /// <summary>
        /// カードをデッキに戻す
        /// </summary>
        /// <param name="cardNos">カード番号一覧</param>
        public void ReturnCard(
            IEnumerable<int> cardNos
            )
        {
            foreach (var no in cardNos)
            {
                this.ReturnCard(no);
            }
        }
        /// <summary>
        /// カードをデッキに戻す
        /// </summary>
        /// <param name="cardNo">カード番号</param>
        public void ReturnCard(
            int cardNo
            )
        {
            this.ReturnCard(this[cardNo]);
        }
        /// <summary>
        /// カードをデッキに戻す
        /// </summary>
        /// <param name="cards">カード一覧</param>
        public void ReturnCard(
            IEnumerable<TCard> cards
            )
        {
            foreach (var c in cards)
            {
                this.ReturnCard(c);
            }
        }
        /// <summary>
        /// カードをデッキに戻す
        /// </summary>
        /// <param name="card">カード</param>
        public void ReturnCard(
            TCard card
            )
        {
            if (card != null)
                card.InDeck = true;
        }
        #endregion カードをデッキに戻す

        #region デッキをリセットする
        /// <summary>
        /// デッキをリセットする
        /// 全てのカードをデッキに戻す
        /// </summary>
        public void ResetDeck()
        {
            // 未初期化例外
            this.CheckInit();

            // 全てのカードをデッキに戻す
            this.ReturnCard(this._CardList);
        }
        #endregion デッキをリセットする

        #region カードを1枚引く
        /// <summary>
        /// デッキからカードを1枚引きカードを取得する
        /// カードが引けるかチェックすること
        /// </summary>
        /// <returns>カード</returns>
        public TCard DrawCard()
        {
            // 未初期化例外
            this.CheckInit();

            // 引けるカードがない場合は例外
            if (!this.ExistsCards)
                throw new ArgumentOutOfRangeException("Remaining cards is 0");

            // カード取得
            var cl = this.RemainingCards.ToList();
            var c = cl[this._Random.Next(minValue: 0, maxValue: cl.Count())];

            // カードを引いた状態にする
            c.InDeck = false;

            // カードを返す
            return c;
        }
        #endregion カードを1枚引く

        #region 次のカードを指定枚数引く
        /// <summary>
        /// カードを指定枚数引いてカードリストを返す
        /// 全てのカードを引いた場合は空のカードリストを返す
        /// </summary>
        /// <param name="count">枚数</param>
        /// <returns>カードリスト</returns>
        public IEnumerable<TCard> DrawCards(
            int count
            )
        {
            // 引数チェック
            if (count < 1
                || count > this.ExistsCardsCount
                )
                throw new ArgumentOutOfRangeException();

            // カードリストを生成
            var result = new List<TCard>();

            // count回繰り返す
            for (int i = 0; i < count; i++)
            {
                // カードを1枚引いてリストに追加する
                result.Add(
                    item: DrawCard()
                    );
            }

            return result;
        }
        #endregion 次のカードを複数枚引く

        #endregion メソッド
    }
    #endregion カードデッキクラス
}
