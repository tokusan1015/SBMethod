using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMethod
{
    #region 重み付けアイテム解析クラス
    /// <summary>
    /// 重み付けアイテム解析クラス
    /// </summary>
    internal abstract class AnalyseWeightItem
    {
        #region メンバ変数
        /// <summary>
        /// 重み付けアイテムリスト
        /// </summary>
        private List<WeightItem> _ItemList = new List<WeightItem>();
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// 重み付けアイテム
        /// 存在しないアイテム番号を指定した場合nullを返す
        /// </summary>
        /// <param name="itemNo">アイテム番号</param>
        /// <returns>結果</returns>
        protected WeightItem this[
            int itemNo
            ]
        {
            get
            {
                return this._ItemList
                    .Where(x => x.ItemNo == itemNo)
                    .FirstOrDefault();
            }
        }
        /// <summary>
        /// 重み付け値
        /// </summary>
        public int this[
            int itemNo,
            int weightNo
            ]
        {
            get
            {
                // 重み付けアイテムを取得する
                var wi = this[itemNo];

                // 結果がnullの場合は例外
                if (wi == null)
                    throw new ArgumentNullException($"存在しないアイテム番号({itemNo})が指定されました");

                // 結果を返す
                return wi.GetWeight(weightNo: weightNo);
            }
        }
        /// <summary>
        /// アイテムリスト数
        /// </summary>
        public int ItemListCounts
        {
            get { return this._ItemList.Count; }
        }
        #endregion プロパティ

        #region 公開メソッド

        #region アイテムリストをクリアする
        /// <summary>
        /// アイテムリストをクリアする
        /// </summary>
        public void Clear()
        {
            this._ItemList.Clear();
        }
        #endregion アイテムリストをクリアする

        #region アイテムリストに重み付けアイテムを追加する
        /// <summary>
        /// アイテムリストに重み付けアイテムを追加する
        /// 重み付け番号は0固定
        /// </summary>
        /// <param name="itemNo">アイテム番号</param>
        /// <param name="weightNoCount">重み付け番号件数</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <param name="select">選択フラグ</param>
        public void AddItemList(
            int itemNo,
            int weightNoCount,
            int min,
            int max ,
            bool select = false
            )
        {
            // 重み付けアイテムを生成する
            var wi = new WeightItem();

            // 重み付け番号を追加する
            wi.AddWeightList(
                count: weightNoCount,
                weight: min,
                min: min,
                max: max
                );

            // 重み付けアイテムを追加する
            this.AddItemList(wi);
        }
        /// <summary>
        /// アイテムリストに重み付けアイテムを追加する
        /// </summary>
        /// <param name="wi">重み付けアイテム</param>
        private void AddItemList(
            WeightItem wi
            )
        {
            this._ItemList.Add(wi);
        }
        #endregion アイテムリストに重み付けアイテムを追加する

        #region 選択アイテムを取得する
        /// <summary>
        /// 選択アイテムを取得する
        /// Select==trueのアイテムを取得する
        /// </summary>
        /// <param name="wil">アイテムリスト</param>
        /// <returns>結果</returns>
        public IEnumerable<WeightItem> GetSelectItems(
            IEnumerable<WeightItem> wil
            )
        {
            return wil.Where(x => x.Select);
        }
        #endregion 選択アイテムを取得する

        #region 指定したアイテム番号を選択にする
        /// <summary>
        /// 指定したアイテム番号を選択にする
        /// </summary>
        /// <param name="wil">アイテムリスト</param>
        /// <param name="itemNos">アイテム番号列挙</param>
        public void SetSelect(
            IEnumerable<WeightItem> wil,
            params int[] itemNos
            )
        {
            // 対象のSelectをfalseにする            
            foreach (var itemNo in itemNos)
            {
                var wi = this.GetItems(
                    wil: wil,
                    itemNo: itemNo
                    ).FirstOrDefault();
                if (wi != null)
                    wi.Select = true;
            }
        }
        #endregion 指定したアイテム番号を選択にする

        #region 指定したアイテム番号を非選択にする
        /// <summary>
        /// 指定したアイテム番号を非選択にする
        /// </summary>
        /// <param name="wil">アイテムリスト</param>
        /// <param name="itemNos">アイテム番号列挙</param>
        public void ResetSelect(
            IEnumerable<WeightItem> wil,
            params int[] itemNos
            )
        {
            // 対象のSelectをfalseにする            
            foreach (var itemNo in itemNos)
            {
                var wi = this.GetItems(
                    wil: wil,
                    itemNo: itemNo
                    ).FirstOrDefault();
                if (wi != null)
                    wi.Select = false;
            }
        }
        #endregion 指定したアイテム番号を非選択にする

        #endregion 公開メソッド

        #region メソッド

        #region 指定したアイテム番号のアイテムリストを取得する
        /// <summary>
        /// 指定したアイテム番号のアイテムリストを取得する
        /// </summary>
        /// <param name="wil">アイテムリスト</param>
        /// <param name="itemNo">アイテム番号</param>
        /// <returns></returns>
        protected IEnumerable<WeightItem> GetItems(
            IEnumerable<WeightItem> wil,
            int itemNo
            )
        {
            // ItemNoで絞り込み結果を返す
            return wil.Where(x => x.ItemNo == itemNo);
        }
        #endregion 指定したアイテム番号のアイテムリストを取得する

        #region 重み付け番号に一致する重み付け値のリストを取得する
        /// <summary>
        /// 重み付け番号に一致する重み付け値のリストを取得する
        /// </summary>
        /// <param name="wil">アイテムリスト</param>
        /// <param name="weightNo">重み付け番号</param>
        /// <returns>結果</returns>
        protected IEnumerable<int> GetWeightList(
            IEnumerable<WeightItem> wil,
            int weightNo
            )
        {
            return wil.Select(x => x.GetWeight(weightNo));
        }
        #endregion 重み付け値のリストを取得する

        #region 重み付け値が連番になっているかチェックする
        /// <summary>
        /// 重み付け値が連番になっているかチェックする
        /// 1.選択されているデータのみにする
        ///   結果件数を保存しておく
        /// 2.重み付け値のみのデータにしたのちに、重み付け値が異なる値のみにする
        /// 3.結果件数を1の結果件数と比較し、異なる場合はfalseを返す
        /// 4.結果データの重み付け値の最小値, 最大値を取得する
        /// 5.最大値, 最小値から等差級数和を求める
        /// 6.結果データの重み付け値の合計を取得し、等差級数和と比較する
        /// 　一致した場合はtrue, 一致しない場合はfalseを返す
        /// </summary>
        /// <returns></returns>
        protected bool CheckStraightWeight(
            IEnumerable<WeightItem> wil,
            int weightNo
            )
        {
            // 1.選択されているデータのみにする
            //   結果件数を保存しておく
            var d1 = this.GetSelectItems(wil);
            if (d1.Count() < 1) return false;

            // 2.重み付け値のみのデータにしたのちに、重み付け値が異なる値のみにする
            var d2 = this.GetWeightList(wil: wil, weightNo: weightNo)
                .Distinct();

            // 3.結果件数を1の結果件数と比較し、異なる場合はfalseを返す
            if (d1.Count() != d2.Count()) return false;

            // 4.結果データの重み付け値の最小値, 最大値を取得する
            var d4 = d2.OrderBy(x => x);
            var min = d4.First();
            var max = d4.Last();

            // 5.最大値, 最小値から等差級数和を求める
            var sas = this.SumArithmeticSeries(min: min, max: max);

            // 6.結果データの重み付け値の合計を取得し、等差級数和と比較する
            // 　一致した場合はtrue, 一致しない場合はfalseを返す
            var d6 = d4.Sum();
            if (d6 == sas) return true;

            // 一致しなかったのでfalseを返す
            return false;
        }
        #endregion 重み付け値が連番になっているかチェックする

        #region 最小値から最大値までの等差級数和を求める
        /// <summary>
        /// 最小値から最大値までの等差級数和を求める
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>結果</returns>
        protected int SumArithmeticSeries(
            int min,
            int max
            )
        {
            return (max * (max + 1) - min * (min - 1)) / 2;
        }
        #endregion 最小値から最大値までの等差級数和を求める

        #endregion メソッド
    }
    #endregion 重み付けアイテム解析クラス

    #region 重み付けアイテム
    /// <summary>
    /// 重み付けアイテム
    /// 重みは必ず連番で無ければならない
    /// ループの場合はループ最小値ループ最大値の値であり
    /// </summary>
    internal class WeightItem
    {
        #region 固定値
        /// <summary>
        /// 重み付け番号エラー
        /// </summary>
        private const string ERROR_WEIGHT_NO = "存在しない重み付け番号({0})が指定されました";
        #endregion 固定値

        #region メンバ変数
        /// <summary>
        /// アイテム番号
        /// </summary>
        private int _ItemNo = 0;
        /// <summary>
        /// 重み付け
        /// </summary>
        private List<Weighting> _WeightList = new List<Weighting>();
        /// <summary>
        /// 選択フラグ
        /// </summary>
        private bool _Select = false;
        /// <summary>
        /// 重み付け値チェック完了フラグ
        /// </summary>
        private bool _CheckWeight = false;
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// アイテム番号
        /// </summary>
        public int ItemNo
        {
            get { return this._ItemNo; }
            set { this._ItemNo = value; }
        }
        /// <summary>
        /// 重み付け番号数
        /// </summary>
        public int WeightListCounts
        {
            get { return this._WeightList.Count; }
        }
        /// <summary>
        /// 選択フラグ
        /// </summary>
        public bool Select
        {
            get { return this._Select; }
            set { this._Select = value; }
        }
        /// <summary>
        /// ステータス値
        /// </summary>
        public int Status
        {
            get;
            set;
        } = 0;
        /// <summary>
        /// 重み付け値チェックフラグ
        /// </summary>
        public bool CheckWeight
        {
            get { return this._CheckWeight; }
        }
        #endregion プロパティ

        #region メソッド

        #region 重み付けリストをクリアする
        /// <summary>
        /// 重み付けリストをクリアする
        /// </summary>
        public void ClearWeightList()
        {
            this._WeightList.Clear();
            this._CheckWeight = false;
        }
        #endregion 重み付けリストをクリアする

        #region 重み付けリストに重み付け番号を追加する
        /// <summary>
        /// 重み付けリストに重み付け番号を追加する
        /// </summary>
        /// <param name="count">重み付け数</param>
        /// <param name="weight">重み付け値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public void AddWeightList(
            int count,
            int weight,
            int min,
            int max
            )
        {
            // 最大値になるまで追加する
            while (this._WeightList.Count < count)
                this._WeightList.Add(item: new Weighting(
                    weight: weight,
                    min: min,
                    max: max
                    ));
        }
        #endregion 重み付けリストに重み値を追加する

        #region 重み付け番号の重み値を取得する
        /// <summary>
        /// 重み付け番号の重み値を取得する
        /// 存在しない場合は-1を返す
        /// </summary>
        /// <param name="weightNo">重み付け番号</param>
        /// <returns>結果</returns>
        public int GetWeight(
            int weightNo
            )
        {
            // 重み付け番号が存在するかチェックする
            if (this.CheckWeightNo(weightNo: weightNo, exception: false))
                // 存在する場合は重み値を返す
                return this._WeightList[weightNo].Weight;

            // 存在しない場合は-1を返す
            return -1;
        }
        #endregion 重み付け番号の重み値を取得する

        #region 重み付け番号の重み値を変更する
        /// <summary>
        /// 重み付け番号の重み値を変更する
        /// 存在しない重み付け番号が指定された場合は何もしない
        /// </summary>
        /// <param name="weightNo">重み付け番号</param>
        /// <param name="weight">重み値</param>
        public void ChangeWeight(
            int weightNo,
            int weight
            )
        {
            // 重み付け番号が存在するかチェックする
            if (this.CheckWeightNo(weightNo: weightNo))
                // 存在する場合は重み値を変更する
                this._WeightList[weightNo].Weight = weight;
        }
        #endregion 重み付け番号の重み値を変更する

        #region 重み付け番号が存在するかチェックする
        /// <summary>
        /// 重み付け番号が存在するかチェックする
        /// 存在しない場合は
        ///   exception == true  : 例外発生
        ///   exception == false : false
        /// となる
        /// </summary>
        /// <param name="weightNo">重み付け番号</param>
        /// <param name="exception">例外発生フラグ</param>
        /// <returns>結果</returns>
        private bool CheckWeightNo(
            int weightNo,
            bool exception = true
            )
        {
            // 重み付け番号が存在するかチェックする
            if (weightNo < 0 || weightNo >= this.WeightListCounts)
            {
                // 例外発生フラグ
                if (exception)
                    // 存在しない場合は例外とする
                    throw new ArgumentOutOfRangeException(
                        string.Format(
                            ERROR_WEIGHT_NO,
                            weightNo
                            ));
                else
                    // 存在しない場合はfalse
                    return false;
            }

            // 存在する場合はtrue
            return true;
        }
        #endregion 重み付け番号が存在するかチェックする

        #endregion メソッド
    }
    #endregion 重み付けアイテム

    #region 重み付けクラス
    /// <summary>
    /// 重み付けクラス
    /// </summary>
    internal class Weighting
    {
        #region 固定値
        /// <summary>
        /// 最小値エラー
        /// </summary>
        private const string ERROR_MIN = "最小値({0})が異常(負値, 最大値と同じか大きい)です";
        /// <summary>
        /// 最大値エラー
        /// </summary>
        private const string ERROR_MAX = "最大値({0})が(負値, 最小値と同じか小さい)です";
        /// <summary>
        /// 重み付け値エラー
        /// </summary>
        private const string ERROR_WEIGHT = "重み付け値{0}が異常です";
        /// <summary>
        /// 重み付け値範囲エラー
        /// </summary>
        private const string ERROR_RANGE = "重み付け値({0})がMin({1})Max({2})の範囲外に存在します";
        /// <summary>
        /// 重み付け値負数エラー
        /// </summary>
        private const string ERROR_MINUS = "重み付け値に負数は設定できません";
        #endregion 固定値

        #region メンバ変数
        /// <summary>
        /// 重み付け値
        /// </summary>
        private int _Weight = 0;
        /// <summary>
        /// 最小値
        /// </summary>
        private int _Min = -1;
        /// <summary>
        /// 最大値
        /// </summary>
        private int _Max = -1;
        /// <summary>
        /// 重み付け値の範囲チェック完了フラグ
        /// </summary>
        private bool _CheckWeight = false;
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// 重み
        /// </summary>
        public int Weight
        {
            get { return this._Weight; }
            set
            {
                // 範囲チェック
                this._CheckWeight = this.CheckWeightAndRange(
                    weight: this.Weight,
                    min: this._Min,
                    max: value
                    );

                // 値を設定
                this._Weight = value;
            }
        }
        /// <summary>
        /// 重み付け値の最小値
        /// </summary>
        public int Min
        {
            get { return this._Min; }
            set
            {
                // 範囲チェック
                this._CheckWeight = this.CheckWeightAndRange(
                    weight: this.Weight,
                    min: this._Min,
                    max: value
                    );

                // 値を設定
                this._Min = value;
            }
        }
        /// <summary>
        /// 重み付け値の最大値
        /// </summary>
        public int Max
        {
            get { return this._Max; }
            set
            {
                // 範囲チェック
                this._CheckWeight = this.CheckWeightAndRange(
                    weight: this.Weight,
                    min: this._Min,
                    max: value
                    );

                // 値を設定
                this._Max = value;
            }
        }
        /// <summary>
        /// ループフラグ
        /// </summary>
        public bool Loop
        {
            get;
            set;
        } = false;
        /// <summary>
        /// 重み付け値範囲チェック完了フラグ
        /// </summary>
        public bool CheckWeight
        {
            get { return _CheckWeight; }
        }
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="weight">重み付け値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public Weighting(
            int weight,
            int min,
            int max
            )
        {
            this._Weight = weight;
            this._Min = min;
            this._Max = max;
        }
        #endregion コンストラクタ

        #region 重み付け値および範囲が正しいかチェックする
        /// <summary>
        /// 重み付け値および範囲が正しいかチェックする
        /// 条件 : MinMaxの範囲内
        /// 正常な場合はtrueが返る
        /// </summary>
        /// <returns>結果</returns>
        private bool CheckWeightAndRange(
            int weight,
            int min,
            int max
            )
        {
            // 最小値チェック
            if (min < 0)
                throw new ArgumentOutOfRangeException(
                    string.Format(
                        ERROR_MIN,
                        min
                        ));

            // 最大値チェック
            if (max < 0)
                throw new ArgumentOutOfRangeException(
                    string.Format(
                        ERROR_MAX,
                        max
                        ));

            // 最小値, 最大値範囲チェック
            if (min < max)
                throw new ArgumentOutOfRangeException(
                    string.Format(
                        ERROR_MIN,
                        min
                        ));

            // 重み付け値チェック
            if (weight < min
                && weight > max
                )
                throw new ArgumentOutOfRangeException(
                    string.Format(
                        ERROR_RANGE,
                        this.Weight,
                        min,
                        max
                        ));


            // 結果を返す
            return true;
        }
        #endregion 重み付け値が正しいかチェックする

        #region 重み値を取得する
        /// <summary>
        /// 重み値を取得する
        /// </summary>
        /// <returns></returns>
        public int GetWeight(
            )
        {
            // 範囲チェック
            if (!this._CheckWeight)
                throw new IndexOutOfRangeException(ERROR_RANGE);

            // 結果を返す
            return this.Weight;
        }
        #endregion 重み値を取得する

        #region 前の重み付け値を取得する
        /// <summary>
        /// 前の重み付け値を取得する
        /// 存在しない場合は-1を返す
        /// </summary>
        /// <returns>結果</returns>
        public int GetPrevWeight()
        {
            // 範囲チェック
            if (!this._CheckWeight)
                throw new IndexOutOfRangeException(ERROR_RANGE);

            // ループが有効かチェックする
            if (this.Loop)
            {
                // ループが有効な場合
                if (this.Weight == this.Min)
                    // Minの前はMax
                    return this.Max;
                else if (this.Weight > this.Min)
                    // Weight - 1
                    return this.Weight - 1;
                else
                    // 範囲外なので例外
                    throw new IndexOutOfRangeException(
                        string.Format(
                            ERROR_WEIGHT,
                            this.Weight
                            ));
            }
            else
            {
                // ループが無効の場合
                if (this.Weight == this.Min)
                    // Minの前は-1
                    return -1;
                else if (this.Weight > this.Min)
                    // Weight - 1
                    return this.Weight - 1;
                else
                    // 範囲外なので例外
                    throw new IndexOutOfRangeException(
                        string.Format(
                            ERROR_WEIGHT,
                            this.Weight
                            ));
            }
        }
        #endregion 前の重み付け値を取得する

        #region 次の重み付け値を取得する
        /// <summary>
        /// 次の重み付け値を取得する
        /// 存在しない場合は-1を返す
        /// </summary>
        /// <returns>結果</returns>
        public int GetNextWeight()
        {
            // 範囲チェック
            if (!this._CheckWeight)
                throw new IndexOutOfRangeException("重み付け値がMinMaxの範囲外に存在します");

            // ループが有効かチェックする
            if (this.Loop)
            {
                // ループが有効な場合
                if (this.Weight == this.Max)
                    // Maxの次はMin
                    return this.Min;
                else if (this.Weight > this.Min)
                    // Weight + 1
                    return this.Weight + 1;
                else
                    // 範囲外なので例外
                    throw new IndexOutOfRangeException(
                        string.Format(
                            ERROR_WEIGHT,
                            this.Weight
                            ));
            }
            else
            {
                // ループが無効の場合
                if (this.Weight == this.Max)
                    // Maxの次は-1
                    return -1;
                else if (this.Weight < this.Max)
                    // Weight + 1
                    return this.Weight + 1;
                else
                    // 範囲外なので例外
                    throw new IndexOutOfRangeException(
                        string.Format(
                            ERROR_WEIGHT,
                            this.Weight
                            ));
            }
        }
        #endregion 次の重み付け値を取得する
    }
    #endregion 重み付けクラス
}
