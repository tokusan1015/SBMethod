using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBacic用簡単な暗号化クラス
    /// <summary>
    /// SmallBacic用簡単な暗号化クラス
    /// </summary>
    [SmallBasicType]
    public static class SBEasyCipher
    {
        #region 固定値
        /// <summary>
        /// ひらがな
        /// </summary>
        private static readonly string _HIRAGANA = 
            "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよわをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽぁぃぅぇぉっゃゅょゎ"; 
        /// <summary>
        /// カタカナ
        /// </summary>
        private static readonly string _KATAKANA =
            "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨワヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポァィゥェォッャュョヮ";
        /// <summary>
        /// 記号
        /// </summary>
        private static readonly string _KIGOU =
            "ー、。…！”’＃＄％＆？｛｝（）＜＞＝｜￥＾＋－＊／．＿　";
        /// <summary>
        /// 仮名
        /// </summary>
        private static readonly string _KANA = _HIRAGANA + _KATAKANA;
        /// <summary>
        /// 仮名記号
        /// </summary>
        private static readonly string _KANAKIGOU = _KANA + _KIGOU;
        /// <summary>
        /// 文字コードの開始値
        /// </summary>
        private const int _CHARCODE_START = 0x3041;
        /// <summary>
        /// 文字コードの終了値
        /// </summary>
        private const int _CHARCODE_END = 0x3093;
        #endregion 固定値

        #region 公開プロパティ
        /// <summary>
        /// 平仮名辞書文字列
        /// </summary>
        public static Primitive HIRAGANA
        {
            get { return _HIRAGANA; }
        }
        /// <summary>
        /// 片仮名辞書文字列
        /// </summary>
        public static Primitive KATAKANA
        {
            get { return _KATAKANA; }
        }
        /// <summary>
        /// 仮名辞書文字列
        /// </summary>
        public static Primitive KANA
        {
            get { return _KANA; }
        }
        /// <summary>
        /// 記号辞書文字列
        /// </summary>
        public static Primitive KIGOU
        {
            get { return _KIGOU; }
        }
        /// <summary>
        /// 仮名記号辞書文字列
        /// </summary>
        public static Primitive KANAKIGOU
        {
            get { return _KANAKIGOU; }
        }
        /// <summary>
        /// 文字コード開始値
        /// </summary>
        public static Primitive CHARCODE_START
        {
            get { return _CHARCODE_START; }
        }
        /// <summary>
        /// 文字コード終了値
        /// </summary>
        public static Primitive CHARCODE_END
        {
            get { return _CHARCODE_END; }
        }
        #endregion 公開プロパティ

        #region 公開メソッド
        /// <summary>
        /// 文字列を辞書で1文字づつシフト数ずらし、結果を文字列で取得します
        /// 辞書は仮名記号辞書文字列となります
        /// 辞書に存在しない文字列はそのまま返します
        /// シフトを+nした結果を-nすると元に戻ります
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="shift">シフト</param>
        /// <returns>文字列</returns>
        public static Primitive KanaKigouShift(
            Primitive text,
            Primitive shift
            )
        {
            // 文字シフトを行った文字列を返す
            return _DictionaryShift(
                text: text.ToString(),
                shift: (int)shift,
                dictionary: _KANAKIGOU
                );
        }

        /// <summary>
        /// 文字列を辞書で1文字づつシフト数ずらし、結果を文字列で取得します
        /// 辞書は文字コードとなります
        /// 辞書に存在しない文字列はそのまま返します
        /// シフトを+nした結果を-nすると元に戻ります
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="shift">シフト値</param>
        /// <returns>文字列</returns>
        public static Primitive KanaCodeShift(
            Primitive text,
            Primitive shift
            )
        {
            // 文字シフトを行った文字列を返す
            return _CodeShift(
                text: text.ToString(),
                shift: (int)shift,
                startUnicode: _CHARCODE_START,
                endUnicode: _CHARCODE_END
                );
        }

        /// <summary>
        /// 文字列を辞書で1文字づつシフト数ずらし、結果を文字列で取得します
        /// 辞書は外部指定文字列となります
        /// 辞書に存在しない文字列はそのまま返します
        /// シフトを+nした結果を-nすると元に戻ります
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="shift">シフト</param>
        /// <param name="dictionary">辞書文字列</param>
        /// <returns>文字列</returns>
        public static Primitive DictionaryShift(
            Primitive text,
            Primitive shift,
            Primitive dictionary
            )
        {
            // 文字シフトを行った文字列を返す
            return _DictionaryShift(
                text: text.ToString(),
                shift: (int)shift,
                dictionary: dictionary.ToString()
                );
        }

        /// <summary>
        /// 文字列を辞書で1文字づつシフト数ずらし、結果を文字列で取得します
        /// 辞書は文字コード表(開始値, 終了値)となります
        /// 辞書に存在しない文字列はそのまま返します
        /// シフトを+nした結果を-nすると元に戻ります
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="shift">シフト値</param>
        /// <param name="startUnicode">Unicode表開始値</param>
        /// <param name="endUnicode">Unicode表終了値</param>
        /// <returns>文字列</returns>
        public static Primitive CodeShift(
            Primitive text,
            Primitive shift,
            Primitive startUnicode,
            Primitive endUnicode
            )
        {
            // 文字シフトを行った文字列を返す
            return _CodeShift(
                text: text.ToString(),
                shift: (int)shift,
                startUnicode: startUnicode,
                endUnicode: endUnicode
                );
        }
        #endregion 公開メソッド

        #region メソッド
        /// <summary>
        /// 文字列を辞書で1文字づつシフト数ずらしした文字列を取得します
        /// 辞書は文字コード表(開始値, 終了値)となります
        /// 辞書に存在しない文字列はそのまま返します
        /// シフトを+nした結果を-nすると元に戻ります
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="shift">シフト値</param>
        /// <param name="startUnicode">Unicode表開始値</param>
        /// <param name="endUnicode">Unicode表終了値</param>
        /// <returns>結果</returns>
        private static string _CodeShift(
            string text,
            int shift,
            int startUnicode,
            int endUnicode
            )
        {
            // 範囲をチェックする
            if (startUnicode >= endUnicode)
                throw new ArgumentOutOfRangeException();

            // 文字シフトを行った文字列を返す
            return _DictionaryShift(
                text: text.ToString(),
                shift: shift,
                dictionary: Utility.GetCharCodeString(
                    start: startUnicode,
                    end: endUnicode
                    ));
        }
        /// <summary>
        /// 文字列を辞書で1文字づつシフト数ずらしした文字列を取得します
        /// 辞書は外部指定文字列となります
        /// 辞書に存在しない文字列はそのまま返します
        /// シフトを+nした結果を-nすると元に戻ります
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="shift">シフト</param>
        /// <param name="dictionary">辞書文字列</param>
        /// <returns>結果</returns>
        private static string _DictionaryShift(
            this string text,
            int shift,
            string dictionary
            )
        {
            // 重複チェック
            if (dictionary.ToList().GetCountsOfSameData() > 0)
                throw new ArgumentException("辞書に重複文字が存在します");

            // 戻り値初期化
            var sb = new StringBuilder();

            // 文字列に変換
            string s = text.ToString();
            string dic = dictionary;

            // サイズを取得
            int size = dic.Length;

            // 文字列で回す
            foreach (var c in s)
            {
                // 辞書での位置を取得する
                var index = dic.IndexOf(c);

                // 辞書に文字が存在している場合
                if (index >= 0)
                {
                    // 文字をシフトして追加する
                        index = (index + shift).GetLoopIndex(
                            size: size
                            );
                    sb.Append(dic[index]);
                }
                else
                {
                    // そのまま追加する
                    sb.Append(c);
                }
            }

            // 結果を返す
            return sb.ToString();
        }
        #endregion メソッド
    }
    #endregion SmallBacic用簡単な暗号化クラス
}
