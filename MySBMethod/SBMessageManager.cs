using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用メッセージ管理クラス
    /// <summary>
    /// SmallBasic用メッセージ管理クラス
    /// </summary>
    [SmallBasicType]
    public static class SBMessageManager
    {
        #region メンバ変数
        /// <summary>
        /// メッセージ辞書
        /// </summary>
        private static Dictionary<int, string> _Message = new Dictionary<int, string>();
        #endregion メンバ変数

        #region 公開メソッド
        /// <summary>
        /// メッセージをid番号で登録します
        /// </summary>
        /// <param name="id">id番号</param>
        /// <param name="message">メッセージ</param>
        public static void Add(
            Primitive id,
            Primitive message
            )
        {
            _Message.Add(
                key: (int)id,
                value: message.ToString()
                );
        }
        /// <summary>
        /// id番号のメッセージを削除し、
        /// 結果をBOOLで取得します
        /// 成功した場合trueが返ります
        /// </summary>
        /// <param name="id">id番号</param>
        /// <returns>BOOL</returns>
        public static Primitive Remove(
            Primitive id
            )
        {
            return _Message.Remove(
                key: (int)id
                );
        }
        /// <summary>
        /// idが登録されているか検証し、
        /// 結果をBOOLで取得します
        /// 登録されている場合trueが返ります
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BOOL</returns>
        public static Primitive Exist(
            Primitive id
            )
        {
            return _Exist((int)id);
        }
        /// <summary>
        /// id番号で登録されているメッセージを文字列で取得します
        /// 取得に失敗した場合は空の文字列が返ります
        /// </summary>
        /// <param name="id">id番号</param>
        /// <returns>文字列</returns>
        public static Primitive GetMessage(
            Primitive id
            )
        {
            string result = string.Empty;
            _Message.TryGetValue(
                key: (int)id,
                value: out result
                );

            return result; 
        }
        /// <summary>
        /// メッセージをXMLファイルに保存します
        /// </summary>
        /// <param name="fileName">ファイルパス名</param>
        public static void Save(
            Primitive fileName
            )
        {
            //DictionaryをXMLファイルに保存する
            _XmlSerialize(fileName.ToString(), _Message);
        }
        /// <summary>
        /// メッセージをXMLファイルから復元します
        /// </summary>
        /// <param name="fileName">ファイルパス名</param>
        public static void Load(
            Primitive fileName
            )
        {
            // ファイルの存在チェック
            if (System.IO.File.Exists(fileName))
                throw new ArgumentException($"ファイル({fileName})が存在しませんでした");

            //DictionaryをXMLファイルから復元する
            _Message = _XmlDeserialize<int, string>(fileName.ToString());
        }
        #endregion 公開メソッド

        #region メソッド

        #region idが登録されているかチェックします
        /// <summary>
        /// idが登録されているか検証し、
        /// 結果をBOOLで取得します
        /// 登録されている場合trueが返ります
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BOOL</returns>
        private static bool _Exist(
            int id
            )
        {
            string result = string.Empty;
            return (_Message.TryGetValue(
                key: (int)id,
                value: out result
                ));
        }
        #endregion idが登録されているかチェックします

        #region DictionaryをKeyAndValueのListに変換する
        /// <summary>
        /// DictionaryをKeyAndValueのListに変換します
        /// </summary>
        /// <typeparam name="TKey">Dictionaryのキーの型</typeparam>
        /// <typeparam name="TValue">Dictionaryの値の型</typeparam>
        /// <param name="dic">変換するDictionary</param>
        /// <returns>変換されたKeyAndValueのList</returns>
        private static List<KeyAndValue<TKey, TValue>>
            _ConvertDictionaryToList<TKey, TValue>(Dictionary<TKey, TValue> dic)
        {
            List<KeyAndValue<TKey, TValue>> lst =
                new List<KeyAndValue<TKey, TValue>>();
            foreach (KeyValuePair<TKey, TValue> pair in dic)
            {
                lst.Add(new KeyAndValue<TKey, TValue>(pair));
            }
            return lst;
        }
        #endregion DictionaryをKeyAndValueのListに変換する

        #region KeyAndValueのListをDictionaryに変換する
        /// <summary>
        /// KeyAndValueのListをDictionaryに変換する
        /// </summary>
        /// <typeparam name="TKey">KeyAndValueのKeyの型</typeparam>
        /// <typeparam name="TValue">KeyAndValueのValueの型</typeparam>
        /// <param name="lst">変換するKeyAndValueのList</param>
        /// <returns>変換されたDictionary</returns>
        private static Dictionary<TKey, TValue>
            _ConvertListToDictionary<TKey, TValue>(List<KeyAndValue<TKey, TValue>> lst)
        {
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
            foreach (KeyAndValue<TKey, TValue> pair in lst)
            {
                dic.Add(pair.Key, pair.Value);
            }
            return dic;
        }
        #endregion KeyAndValueのListをDictionaryに変換する

        #region DictionaryをXMLファイルに保存する
        /// <summary>
        /// DictionaryをXMLファイルに保存する
        /// </summary>
        /// <typeparam name="TKey">Dictionaryのキーの型</typeparam>
        /// <typeparam name="TValue">Dictionaryの値の型</typeparam>
        /// <param name="fileName">保存先のXMLファイル名</param>
        /// <param name="dic">保存するDictionary</param>
        private static void _XmlSerialize<TKey, TValue>(
            string fileName, Dictionary<TKey, TValue> dic)
        {
            //シリアル化できる型に変換
            List<KeyAndValue<TKey, TValue>> obj = _ConvertDictionaryToList(dic);

            //XMLファイルに保存
            XmlSerializer serializer =
                new XmlSerializer(typeof(List<KeyAndValue<TKey, TValue>>));

            using (StreamWriter sw =
                new StreamWriter(fileName, false, new UTF8Encoding(false)))
            {
                serializer.Serialize(sw, obj);
            }
        }
        #endregion DictionaryをXMLファイルに保存する

        #region シリアル化されたXMLファイルをDictionaryに復元する
        /// <summary>
        /// シリアル化されたXMLファイルをDictionaryに復元する
        /// </summary>
        /// <typeparam name="TKey">Dictionaryのキーの型</typeparam>
        /// <typeparam name="TValue">Dictionaryの値の型</typeparam>
        /// <param name="fileName">復元するXMLファイル名</param>
        /// <returns>復元されたDictionary</returns>
        private static Dictionary<TKey, TValue> _XmlDeserialize<TKey, TValue>(
            string fileName)
        {
            //XMLファイルから復元
            XmlSerializer serializer =
                new XmlSerializer(typeof(List<KeyAndValue<TKey, TValue>>));

            List<KeyAndValue<TKey, TValue>> obj = null;
            using (StreamReader sr = new StreamReader(fileName, new UTF8Encoding(false)))
            {
                 obj = (List<KeyAndValue<TKey, TValue>>)serializer.Deserialize(sr);
            }

            //Dictionaryに戻す
            Dictionary<TKey, TValue> dic = _ConvertListToDictionary(obj);

            return dic;
        }
        #endregion シリアル化されたXMLファイルをDictionaryに復元する

        #endregion メソッド

        #region シリアル化できる、KeyValuePairに代わる構造体
        /// <summary>
        /// シリアル化できる、KeyValuePairに代わる構造体
        /// </summary>
        /// <typeparam name="TKey">Keyの型</typeparam>
        /// <typeparam name="TValue">Valueの型</typeparam>
        [Serializable]
        internal struct KeyAndValue<TKey, TValue>
        {
            #region メンバ変数
            /// <summary>
            /// キー
            /// </summary>
            public TKey Key;
            /// <summary>
            /// 値
            /// </summary>
            public TValue Value;
            #endregion メンバ変数

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="pair">ペア</param>
            public KeyAndValue(
                KeyValuePair<TKey, TValue> pair
                )
            {
                Key = pair.Key;
                Value = pair.Value;
            }
            #endregion コンストラクタ
        }
        #endregion シリアル化できる、KeyValuePairに代わる構造体
    }
    #endregion SmallBasic用メッセージ管理
}
