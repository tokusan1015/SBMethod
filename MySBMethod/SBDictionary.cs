using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用辞書テーブル
    /// <summary>
    /// SmallBasic用辞書テーブル
    /// </summary>
    [SmallBasicType]
    public static class SBDictionary
    {
        #region メンバ変数
        /// <summary>
        /// ハッシュテーブル
        /// </summary>
        private static Dictionary<Primitive, Primitive> _Dictionary = new Dictionary<Primitive, Primitive>();
        #endregion メンバ変数

        #region 公開メソッド

        #region ハッシュテーブルをクリアする
        /// <summary>
        /// ハッシュテーブルをクリアする
        /// </summary>
        public static void Clear()
        {
            // ハッシュテーブルをクリアする
            _Dictionary.Clear();
        }
        #endregion ハッシュテーブルをクリアする

        #region ハッシュテーブルに格納されている件数を取得する
        /// <summary>
        /// ハッシュテーブルに格納されている件数を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive Counts()
        {
            // ハッシュテーブルに格納されている件数を取得する
            return (Primitive)_Dictionary.Count;
        }
        #endregion ハッシュテーブルに格納されている件数を取得する

        #region ハッシュテーブルにデータを追加する
        /// <summary>
        /// ハッシュテーブルにデータを追加します
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        public static void Add(
            Primitive key,
            Primitive value
            )
        {
            // ハッシュテーブルにデータを追加する
            _Dictionary.Add(key, value);
        }
        #endregion ハッシュテーブルにデータを追加する

        #region ハッシュテーブルからデータを削除する
        /// <summary>
        /// ハッシュテーブルからデータを削除します
        /// </summary>
        /// <param name="key">キー</param>
        public static void Remove(
            Primitive key
            )
        {
            // ハッシュテーブルからデータを削除する
            _Dictionary.Remove(key);
        }
        #endregion ハッシュテーブルからデータを削除する

        #region ハッシュテーブルにキーが存在するか確認する
        /// <summary>
        /// ハッシュテーブルにキーが存在するか検証します
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>BOOL</returns>
        public static Primitive FindKey(
            Primitive key
            )
        {
            Primitive ret = null;
            return (Primitive)_Dictionary.TryGetValue(key, out ret);
        }
        #endregion ハッシュテーブルにキーが存在するか確認する

        #region ハッシュテーブルからデータを取得する
        /// <summary>
        /// ハッシュテーブルからデータを取得する
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>データ</returns>
        public static Primitive GetValue(
            Primitive key
            )
        {
            // 戻り値初期化
            Primitive ret = null;

            // データを取得する
            _Dictionary.TryGetValue(key, out ret);

            return ret;
        }
        #endregion ハッシュテーブルからデータを取得する

        #endregion 公開メソッド
    }
    #endregion SmallBasic用独自ハッシュテーブル
}
