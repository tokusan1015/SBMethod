using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用多次元インデックス⇔一次元配列インデックス変換
    /// <summary>
    /// SmallBasic用多次元インデックス⇔一次元配列インデックス変換
    /// </summary>
    [SmallBasicType]
    public static class SBDimension
    {
        #region メンバ変数
        /// <summary>
        /// 
        /// </summary>
        private static ConvertArrayIndex _ConvertArrayIndex = null;
        #endregion メンバ変数

        #region 公開メソッド
        /// <summary>
        /// 次元構造を設定します
        /// 2次元以上の構造のみ有効となります
        /// 次元の並び順は固定しなければなりません。
        /// 例)
        /// 座標(x, y, z)の要素数が(2, 3, 4)場合
        /// d[1] = 2
        /// d[2] = 3
        /// d[3] = 4
        /// SBDimension.SetDimention(d)
        /// </summary>
        /// <param name="dimensions">次元構造</param>
        public static void SetDimention(
            this Primitive dimensions
            )
        {
            // 入力チェック
            if (dimensions.GetItemCount() < 2)
                throw new ArgumentOutOfRangeException("次元数は2以上にしてください");

            // ConvertArrayIndexクラス生成
            _ConvertArrayIndex = new ConvertArrayIndex(
                elements: dimensions.ConvertPrimitiveToDimInt().ToArray()
                );
        }
        /// <summary>
        /// 次元数を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetDimentionCount()
        {
            // 初期化完了チェック
            CheckInit();

            return _ConvertArrayIndex.Dimensions;
        }
        /// <summary>
        /// 総要素数を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetTotalElementCount()
        {
            // 初期化完了チェック
            CheckInit();

            return _ConvertArrayIndex.TotalElementCounts;
        }
        /// <summary>
        /// 各次元の要素数を数値で取得します
        /// 次元番号は1から始まる数字になります
        /// 存在しない次元を指定した場合-1が返ります
        /// 例)
        /// 配列(x, y, z, ... , n)の場合、
        /// xの要素数 : SBDimension.GetLength(1)
        /// yの要素数 : SBDimension.GetLength(2)
        /// zの要素数 : SBDimension.GetLength(3)
        /// nの要素数 : SBDimension.GetLength(n)
        /// とします
        /// </summary>
        /// <param name="dimentionNo">次元番号</param>
        /// <returns>数値</returns>
        public static Primitive GetLength(
            Primitive dimentionNo
            )
        {
            // 初期化完了チェック
            CheckInit();

            // 次元番号取得
            var dn = (int)dimentionNo - 1;

            // 次元数が正しいかチェック
            if (dn < 0 || dn >= _ConvertArrayIndex.Dimensions)
                return -1;

            // 次元数を返す
            return _ConvertArrayIndex.GetLength(dn);
        }
        /// <summary>
        /// 多次元インデックスを一次元インデックスに変換し数値配列で取得します
        /// 次元の並び順は固定しなければなりません。
        /// 配列は1から始まる数字になります
        /// 例)
        /// 配列(x, y, z, ... n)の場合、
        /// d[1] = x
        /// d[2] = y
        /// d[3] = z
        ///   :
        /// d[n] = n
        /// SBDimension.ToIndex(d)
        /// とします
        /// </summary>
        /// <param name="dimentions">多次元インデックス配列</param>
        /// <returns>数値配列</returns>
        public static Primitive ConvertIndex(
            this Primitive dimentions
            )
        {
            // 初期化完了チェック
            CheckInit();

            // 次元数チェック
            if (dimentions.GetItemCount() != _ConvertArrayIndex.Dimensions)
                throw new ArgumentOutOfRangeException(
                    $"{_ConvertArrayIndex.Dimensions}次元で指定してください"
                    );

            // 結果を返す
            return _ConvertArrayIndex.ArrayIndexToIndex(
                index: dimentions.ConvertPrimitiveToDimInt().ToArray()
                );
        }
        /// <summary>
        /// 一次元インデックスを多次元インデックスに変換し数値配列で取得します
        /// 結果は配列で戻されます
        /// 配列は1から始まる数字になります
        /// 例)
        /// 配列(x, y, z, ... , n)の場合、
        /// d = SBDimension.ToArrayIndex(index)
        /// d[1] にxのインデックス
        /// d[2] にyのインデックス
        /// d[3] にzのインデックス
        ///   :
        /// d[n] にnのインデックス
        /// </summary>
        /// <param name="index">一元インデックス</param>
        /// <returns>数値配列</returns>
        public static Primitive ConvertArrayIndex(
            Primitive index
            )
        {
            // 変換する
            var d = _ConvertArrayIndex.IndexToArrayIndex(
                index: (int)index
                );

            // 結果を返す
            return ComvertDimIntToPrimitive(d);
        }
        /// <summary>
        /// 配列のXML出力
        /// </summary>
        /// <param name="dimension">文字列配列</param>
        /// <param name="filePathName">ファイルパス名</param>
        public static void WriteXML(
            Primitive dimension,
            Primitive filePathName
            )
        {
            dimension.ConvertPrimitiveToDimString().WriteXml(filePathName: filePathName.ToString());
        }
        /// <summary>
        /// XMLの配列読込
        /// </summary>
        /// <param name="filePathName">ファイルパス名</param>
        /// <returns>文字列配列</returns>
        public static Primitive ReadXML(
            Primitive filePathName
            )
        {
            return filePathName.ToString().ReadXML<List<string>>().ConvertDimStringToPrimitive();
        }
        #endregion 公開メソッド

        #region メソッド
        /// <summary>
        /// 初期化完了チェック
        /// </summary>
        private static void CheckInit()
        {
            // 初期化完了チェック
            if (_ConvertArrayIndex == null)
                throw new ArgumentNullException("SetDimention()が実行されていません");
        }
        /// <summary>
        /// int[]をPrimitiveに変換する
        /// </summary>
        /// <param name="datas">int配列</param>
        /// <returns>結果</returns>
        private static Primitive ComvertDimIntToPrimitive(
            IEnumerable<int> datas
            )
        {
            var p = new Primitive();
            var d = datas.ToList();

            for (int i = 0; i < datas.Count(); i++)
            {
                p[i + 1] = d[i];
            }

            return p;
        }
        #endregion メソッド
    }
    #endregion SmallBasic用多次元インデックス⇔一次元配列インデックス変換

    #region 多次元配列インデックス⇔一次元配列インデックス変換クラス
    /// <summary>
    /// 多次元配列インデックス⇔一次元配列インデックス変換クラス
    /// 1)多次元配列, ジャグ配列アクセス用の一次元インデックスを生成する
    /// 2)1次元配列から多次元配列用インデックスを生成する
    /// 　参考まで
    /// 　ジャグ配列の場合(これにより配列が1次元分展開される)
    /// 　三次元ジャグ配列の例
    /// 　int[][][] array3; //ジャグ配列に対して
    ///   int[][] array2 = array3.SelectMany(x => x).ToArray();
    ///   int[] array1 = array2.SelectMany(x => x).ToArray();
    ///   で一次元配列アクセスが可能
    /// </summary>
    internal class ConvertArrayIndex
    {
        #region メンバ変数
        /// <summary>
        /// 配列の要素数
        /// </summary>
        private int[] _Length = null;
        /// <summary>
        /// 全要素数
        /// </summary>
        private int _TotalElementCounts;
        /// <summary>
        /// 倍率
        /// </summary>
        private int[] _Mag = null;
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// 次元数
        /// </summary>
        public int Dimensions
        {
            get { return this._Length.Length; }
        }
        /// <summary>
        /// 全要素数
        /// </summary>
        public int TotalElementCounts
        {
            get
            {
                return this._TotalElementCounts;
            }
        }
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// 使用する配列構造を渡す
        /// 例)
        /// var array = new int[3, 4, 5];
        /// var ai = new ArrayIndex(array);
        /// 等と記載する
        /// </summary>
        /// <param name="data">多次元配列</param>
        public ConvertArrayIndex(
            System.Array data
            )
        {
            // 配列の個々の要素数を求める
            this._Length = new int[data.Rank];
            for (int i = 0; i < this._Length.Length; i++)
            {
                this._Length[i] = data.GetLength(i);
            }

            // 配列の全要素数を求める
            this._TotalElementCounts =
                this._Length.Aggregate((now, next) => now * next);

            // 倍率配列を求める
            this._Mag = this.GetMagDimIndex(this._Length).ToArray();
        }
        /// <summary>
        /// コンストラクタ
        /// 使用する配列構造を渡す
        /// 例)
        /// var array = new int[3, 4, 5];
        /// var ai = new ArrayIndex(3, 4, 5);
        /// 等と記載する
        /// </summary>
        /// <param name="elements">次元の要素数列挙</param>
        public ConvertArrayIndex(
            params int[] elements
            )
        {
            // 配列の個々の要素数を求める
            this._Length = new int[elements.Length];
            for (int i = 0; i < this._Length.Length; i++)
            {
                this._Length[i] = elements[i];
            }

            // 配列の全要素数を求める
            this._TotalElementCounts =
                this._Length.Aggregate((now, next) => now * next);

            // 倍率配列を求める
            this._Mag = this.GetMagDimIndex(elements).ToArray();
        }
        #endregion コンストラクタ

        #region 公開メソッド

        #region 次元番号の要素数を取得する
        /// <summary>
        /// 次元番号の要素数を取得する
        /// 配列[0, 1, 2,..., n]のnが次元番号
        /// </summary>
        /// <param name="dimensionNo">次元番号</param>
        /// <returns>結果</returns>
        public int GetLength(
            int dimensionNo
            )
        {
            return this._Length[dimensionNo];
        }
        #endregion 指定次元番号の要素数を取得する

        #region n次元配列インデックスを1次元配列インデックスにする
        /// <summary>
        /// n次元配列インデックスを1次元配列インデックスにする
        /// インデックス配列はdata配列の次元数の配列
        /// 例)
        /// int index = ArrayIndexToIndex(x, y, z);
        /// </summary>
        /// <param name="index">インデックス配列</param>
        /// <returns>結果</returns>
        public int ArrayIndexToIndex(
            params int[] index
            )
        {
            // 配列数が異なる場合は例外
            if (index.Length != this._Length.Length)
                throw new ArgumentOutOfRangeException(
                    $"index length is not {this._Length.Length} : {index.Length}"
                    );

            // indexを求める
            var result = 0;
            int len = index.Length;
            for (int i = 0; i < len; i++)
            {
                result += index[i] * this._Mag[i];
            }

            // 結果を返す
            return result;
        }
        #endregion n次元配列インデックスを1次元配列インデックスにする

        #region 一次元配列インデックスをn次元配列インデックスにする
        /// <summary>
        /// 一次元配列インデックスをn次元配列インデックスにする
        /// 結果はdata配列に次元数の配列
        /// 例)
        /// int[] xyz = IndexToArrayIndex(index);
        /// 等と記述する
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>結果</returns>
        public IEnumerable<int> IndexToArrayIndex(
            int index
            )
        {
            // 戻り値生成
            var len = this.Dimensions;
            int[] result = new int[len];

            // 個々のインデックスを設定する
            int mag;
            for (int i = 0; i < len; i++)
            {
                mag = this._Mag[i];
                result[i] = index / mag;
                index = index % mag;
            }

            // 結果を返す
            return result;
        }
        #endregion 一次元配列インデックスをn次元配列インデックスにする

        #endregion 公開メソッド

        #region メソッド

        #region 各次元の要素数から次元毎の倍率を求める
        /// <summary>
        /// 各次元の要素数から次元毎の倍率を求める
        /// dnは必ず1となる
        /// dn-1はdnの要素数
        /// dn-2はdnからdn-1までの要素数の積
        ///      :
        /// d0はdnからd1までの要素数の積
        /// 一次元前の配列数*それ以前の配列数が設定される
        /// </summary>
        /// <param name="dimensionElementCounts">次元要素数配列</param>
        /// <returns>結果</returns>
        private IEnumerable<int> GetMagDimIndex(
            IEnumerable<int> dimensionElementCounts
            )
        {
            // 戻り値初期化
            var dec = dimensionElementCounts.ToList();
            int len = dec.Count();
            int[] result = new int[len];

            // 最大値を求める
            var size = 1;
            for (int i = len - 1; i >= 0; i--)
            {
                result[i] = size;
                size *= dec[i];
            }

            // 結果を返す
            return result;
        }
        #endregion 各次元の要素数から次元毎の倍率を求める

        #endregion メソッド
    }
    #endregion 多次元配列インデックス⇔一次元配列インデックス変換クラス
}
