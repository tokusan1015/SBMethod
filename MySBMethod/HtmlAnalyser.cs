using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser
{
    #region Html解析クラス
    /// <summary>
    /// Html解析クラス
    /// </summary>
    internal class HtmlAnalyser : TagCollection
    {
        #region 固定値
        /// <summary>
        /// タグ開始文字
        /// </summary>
        private const char STAR_TAG = '<';
        /// <summary>
        /// タグ終了文字
        /// </summary>
        private const char END_TAG = '>';
        /// <summary>
        /// 無視する文字
        /// </summary>
        private const string IGNORE_CHARACTER = "\t\n\r";
        #endregion 固定値

        #region 列挙型

        #endregion 列挙型

        #region メンバ変数
        /// <summary>
        /// HTML
        /// </summary>
        private string _HtmlText = string.Empty;
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// Html文字列
        /// </summary>
        internal string HtmlText
        {
            set { this._HtmlText = value; }
            get { return this._HtmlText; }
        }
        #endregion プロパティ

        #region メソッド

        #region AnalyseHtml
        /// <summary>
        /// HtmlTextを解析してタグコレクションを生成する
        /// 全角を半角に変換後解析する(changeDoubleByteSpace)
        /// </summary>
        /// <param name="changeDoubleByteSpace">全角半角変換</param>
        internal void AnalyseHtml(
            bool changeDoubleByteSpace
            )
        {
            // HtmlTextを取得する
            string thmltext = this._HtmlText;

            // 全角半角変換
            if (changeDoubleByteSpace)
                // HtmlTextの全角を半角に変換する
                thmltext = thmltext.Replace("　", " "); 

            // タグコレクションをクリアする
            this.Tags.Clear();

            // 文字長を取得する
            int length = thmltext.Length;

            // 解析状態をTextにする
            Tag.enmTagType eTTNow = Tag.enmTagType.Text;
            Tag.enmTagType eTTNext = Tag.enmTagType.Text;

            // 退避用String
            StringBuilder sb = new StringBuilder();

            // 直前の文字
            char before = ' ';

            // 1文字づつ解析してゆく
            for (int i = 0; i < length; i++)
            {
                // 1文字取り出す
                char c = thmltext[i];

                // 無視文字以外の場合
                if (!CheckIgnoreCharacter(
                    c: c,
                    before: before
                    ))
                {
                    // 文字チェック
                    switch (c)
                    {
                        case STAR_TAG:
                            eTTNext = Tag.enmTagType.Tag;
                            break;
                        case END_TAG:
                            eTTNext = Tag.enmTagType.Text;
                            break;
                        default:
                            break;
                    }

                    // 状態が変化した場合
                    if (eTTNow != eTTNext)
                    {
                        // Tagへ変化の場合
                        if (eTTNow == Tag.enmTagType.Tag)
                            // 文字列退避
                            sb.Append(c);

                        // 文字が存在する場合
                        if (sb.ToString().Trim().Length > 0)
                        {
                            // タグリストに追加する
                            this.Tags.Add(new Tag
                            {
                                LineNo = this.Tags.Count,
                                TagString = sb.ToString().Trim(),
                                TagType = eTTNow
                            });

                        }

                        // 文字退避クリア
                        sb.Clear();

                        // Textへ変化の場合
                        if (eTTNow == Tag.enmTagType.Text)
                            // 文字列退避
                            sb.Append(c);

                        // 状態を変更する
                        eTTNow = eTTNext;
                    }
                    else
                    {
                        // 文字列退避
                        sb.Append(c);
                    }
                }

                // 直前の文字設定
                before = c;
            }

            // sbに文字列が残っていた場合
            if (sb.ToString().Trim().Length > 0)
            {
                // 文字としてタグリストに追加する
                this.Tags.Add(new Tag
                {
                    LineNo = this.Tags.Count,
                    TagString = sb.ToString().Trim(),
                    TagType = Tag.enmTagType.Text

                });
            }
        }
        #endregion AnalyseHtml

        #region CheckIgnoreCharacter
        /// <summary>
        /// 対象文字が無視文字か調べる
        /// 連続空白も無視する
        /// 無視文字の場合true
        /// </summary>
        /// <param name="c">対象文字</param>
        /// <param name="before">直前文字</param>
        /// <returns>無視文字の場合true</returns>
        private bool CheckIgnoreCharacter(
            char c,
            char before
            )
        {
            // 連続空白の場合
            if (c == ' ' && c == before)
                // 無視する
                return true;

            // 無視文字一覧で回す
            foreach (char i in IGNORE_CHARACTER)
            {
                // 無視文字の場合、trueを返す
                if (c == i) return true;
            }

            // 存在しなかった為、falseを返す
            return false;
        }
        #endregion CheckIgnoreCharacter

        #endregion メソッド
    }
    #endregion Html解析クラス
}
