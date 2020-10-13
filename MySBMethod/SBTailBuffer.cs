using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用テイルバッファクラス
    /// <summary>
    /// SmallBasic用テイルバッファクラス
    /// </summary>
    [SmallBasicType]
    public static class SBTailBuffer
    {
        #region メンバ変数
        /// <summary>
        /// テイルバッファ
        /// </summary>
        private static TailBuffer<Primitive> _TailBuffer = null;
        #endregion メンバ変数

        #region 公開プロパティ
        /// <summary>
        /// バッファサイズ
        /// </summary>
        public static Primitive Size
        {
            get { return _TailBuffer.Capacity; }
        } 

        /// <summary>
        /// 個数
        /// </summary>
        public static Primitive Count
        {
            get { return _TailBuffer.Count; }
        }

        /// <summary>
        /// 追加されている場合true
        /// </summary>
        public static Primitive Exists
        {
            get { return _TailBuffer.Exists; }
        }
        #endregion 公開プロパティ

        #region 公開メソッド
        /// <summary>
        /// バッファを生成します
        /// </summary>
        /// <param name="bufferSize">バッファサイズ</param>
        public static void Create(
            Primitive bufferSize
            )
        {
            // _TailBufferがnullでない場合は生成できない
            if (_TailBuffer != null)
                throw new ArithmeticException("Buffer is already created");

            // バッファを生成する
            _TailBuffer = new TailBuffer<Primitive>(bufferSize);
        }

        /// <summary>
        /// バッファをクリアします
        /// </summary>
        public static void Clear()
        {
            // _TailBufferがnullの場合は例外
            if (_TailBuffer == null)
                throw new ArithmeticException("Buffer is not created");

            // バッファクリア
            _TailBuffer.Clear();
        }

        /// <summary>
        /// データを追加します
        /// </summary>
        /// <param name="value">値</param>
        public static void Add(
            Primitive value
            )
        {
            // _TailBufferがnullの場合は例外
            if (_TailBuffer == null)
                throw new ArithmeticException("Buffer is not created");

            // 値を追加する
            _TailBuffer.Add(value);
        }
        /// <summary>
        /// インデックスのデータを取得します
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>データ</returns>
        public static Primitive GetValue(
            Primitive index
            )
        {
            var idx = (int)index;

            // 引数チェック
            if (idx < 1 || idx > _TailBuffer.Count)
                throw new ArgumentOutOfRangeException($"Index range is 1 - {_TailBuffer.Count}");

            return _TailBuffer[idx - 1];
        }
        /// <summary>
        /// 追加したデータを文字列配列で取得します
        /// 配列は1から生成されます
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetValuesToText()
        {
            // _TailBufferがnullの場合は例外
            if (_TailBuffer == null)
                throw new ArithmeticException("Buffer is not created");

            // 一覧を取得する
            var list = _TailBuffer.GetValues();

            // 戻り値初期化
            var result = new Primitive();
            int i = 1;

            // バッファからPrimitive配列生成
            foreach (var p in list)
            {
                result[i++] = p.ToString();
            }

            // 結果を返す
            return result;
        }
        /// <summary>
        /// 追加したデータを数値配列で取得します
        /// 配列は1から生成されます
        /// </summary>
        /// <returns>数値配列</returns>
        public static Primitive GetValuesToNumber()
        {
            // _TailBufferがnullの場合は例外
            if (_TailBuffer == null)
                throw new ArithmeticException("Buffer is not created");

            // 一覧を取得する
            var list = _TailBuffer.GetValues();

            // 戻り値初期化
            var result = new Primitive();
            int i = 1;

            // バッファからPrimitive配列生成
            foreach (var p in list)
            {
                result[i++] = p.doubleParse();
            }

            // 結果を返す
            return result;
        }
        #endregion 公開メソッド
    }
    #endregion テイルバッファクラス

    #region テイルバッファ
    /// <summary>
    /// テイルバッファ(簡易型)
    /// </summary>
    /// <typeparam name="T">要素</typeparam>
    internal class TailBuffer<T> : IEnumerable<T>
    {
        #region メンバ変数
        /// <summary>
        /// 容量
        /// </summary>
        private int _Capacity;
        /// <summary>
        /// バッファ
        /// </summary>
        private T[] _Buffer;
        /// <summary>
        /// 書き換えインデックス
        /// </summary>
        private int _WriteIndex = -1;
        /// <summary>
        /// フルフラグ
        /// </summary>
        private bool _IsFull = false;
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// インデックスの要素を取得する
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>要素</returns>
        public T this[int index]
        {
            get { return this._Buffer[index]; }
        }
        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity
        {
            get { return this._Capacity; }
        }
        /// <summary>
        /// 要素数
        /// </summary>
        public int Count
        {
            get { return this._IsFull ? this._Capacity : this._WriteIndex + 1; }
        }
        /// <summary>
        /// 要素の有無
        /// </summary>
        public bool Exists
        {
            get { return this.Count > 0; }
        }
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="capacity">容量</param>
        public TailBuffer(
            int capacity
            )
        {
            // 容量設定
            this._Capacity = capacity;

            // バッファ生成
            this._Buffer = new T[this.Capacity];

            // 書き換えインデックス設定
            this._WriteIndex = -1;
        }
        #endregion コンストラクタ

        #region メソッド

        #region 次のインデックスを取得する
        /// <summary>
        /// 次のインデックスを取得する
        /// </summary>
        /// <remarks>
        /// %を使用しているため遅い
        /// 高速化するためにはmask andする方法ととること
        /// </remarks>
        /// <param name="index">現在のインデックス</param>
        /// <returns>次のインデックス</returns>
        private int GetNextIndex(
            int index
            )
        {
            // 次のインデックスを取得する
            return ++index % this._Capacity;
        }
        #endregion 次のインデックスを取得する

        #region 先頭インデックスを取得する
        /// <summary>
        /// 先頭インデックスを取得する
        /// </summary>
        /// <returns>先頭インデックス</returns>
        private int GetStartIndex()
        {
            return this._IsFull ? GetNextIndex(this._WriteIndex) : 0;
        }
        #endregion 先頭インデックスを取得する

        #region クリア
        /// <summary>
        /// クリアする
        /// </summary>
        public void Clear(
            )
        {
            this._WriteIndex = -1;
            this._IsFull = false;
        }
        #endregion クリア

        #region 要素を追加する
        /// <summary>
        /// 要素を追加する
        /// </summary>
        /// <param name="item">要素</param>
        public void Add(
            T item
            )
        {
            // 次のインデックスを取得する
            this._WriteIndex = this.GetNextIndex(this._WriteIndex);

            // インデックスが容量に達していたらIsFullをtrueにする
            if (this._WriteIndex == this.Capacity - 1)
                this._IsFull = true;

            // バッファに要素を設定する
            this._Buffer[this._WriteIndex] = item;
        }
        #endregion データを追加する

        #region データリストを取得する
        /// <summary>
        /// データリストを取得する
        /// </summary>
        /// <returns>結果</returns>
        public IEnumerable<T> GetValues()
        {
            var result = new List<T>();
            var index = this.GetStartIndex();
            for (int i = 0; i < this.Count; i++)
            {
                result.Add(
                    this._Buffer[index]
                    );
                index = this.GetNextIndex(index);
            }
            return result;
        }
        #endregion データリストを取得する

        #endregion メソッド

        #region インターフェース実装
        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns>結果</returns>
        public IEnumerator<T> GetEnumerator()
        {
            // 先頭インデックスを取得
            var index = this.GetStartIndex();

            // 要素数繰り返す
            for (int i = 0; i < this.Count; i++)
            {
                // 先頭を返す
                yield return this._Buffer[index];

                // 次のインデックス取得
                index = this.GetNextIndex(index);
            }
        }
        /// <summary>
        /// IEnumerable.GetEnumerator
        /// </summary>
        /// <returns>結果</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion インターフェース実装
    }
    #endregion SmallBasic用テイルバッファ    
}
