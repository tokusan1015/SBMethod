using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser
{
    #region タグコレクションクラス
    /// <summary>
    /// タグコレクションクラス
    /// </summary>
    internal class TagCollection
    {
        #region メンバ変数
        /// <summary>
        /// タグリスト
        /// </summary>
        private List<Tag> _Tags = new List<Tag>();
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// 対象インデックスのタグを取得する
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>Tag</returns>
        protected Tag this[int index]
        {
            get { return this._Tags[index]; }
        }
        /// <summary>
        /// タグリスト
        /// </summary>
        protected List<Tag> Tags
        {
            get { return this._Tags; }
        }
        #endregion プロパティ

        #region メソッド

        #region Clear
        /// <summary>
        /// タグリストをクリアする
        /// </summary>
        public void Clear()
        {
            // クリア
            this._Tags.Clear();
        }
        #endregion Clear

        #region ClearRefineFlag
        /// <summary>
        /// タグリストの絞り込みフラグをクリアする
        /// </summary>
        protected void ClearRefineFlag()
        {
            // タグリスト数繰り返す
            foreach(var t in this._Tags)
            {
                // 絞り込みフラグをクリアする
                t.ClearRefineFlag();
            }
        }
        #endregion ClearRefineFlag

        #region Find
        /// <summary>
        /// 文字列内に指定文字列があるか調べる
        /// Refineは、絞り込みを行う場合のみtrueとする
        /// </summary>
        /// <param name="text">指定文字列</param>
        /// <param name="refine">絞り込みフラグ</param>
        public void Find(
            string text,
            bool refine = false
            )
        {
            // 絞り込みでない場合
            if (!refine)
                // 絞り込みフラグをクリアする
                this.ClearRefineFlag();

            // タグリスト数繰り返す
            foreach (var t in this._Tags)
            {
                // 絞り込み実行
                t.Find(
                    text: text, 
                    refine: refine
                    );
            }
        }
        #endregion Find

        #region FindRange
        /// <summary>
        /// 文字列内に開始文字列から
        /// 終了文字列の間を範囲とする
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void FindRange(
            string start,
            string end
            )
        {
            // 絞り込みフラグをクリアする
            this.ClearRefineFlag();

            // タグリスト数繰り返す
            bool hit = false;
            foreach (var t in this._Tags)
            {
                if (!hit)
                {
                    // 範囲絞込開始
                    if (t.Find(
                        text: start,
                        refine: false
                        ) >= 0)
                    {
                        hit = true;
                    }
                }
                else
                {
                    // 範囲絞込終了
                    if (t.Find(
                        text: end,
                        refine: false
                        ) < 0)
                    {
                        t.SetRefineFlag();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        #endregion FindRange

        #region タグリストを取得する
        /// <summary>
        /// タグタイプと同じタグのタグリストを取得する
        /// Tag.enmTagType TagType = Tag.enmTagType.UnKnownの場合、全てリスト化する
        /// </summary>
        /// <param name="tagType">タグタイプ</param>
        /// <param name="refine">絞り込み結果の場合true</param>
        /// <returns>リスト</returns>
        internal List<string> GetTagList(
            Tag.enmTagType tagType = Tag.enmTagType.UnKnown,
            bool refine = true
            )
        {
            // 戻り値生成
            // tagType == UnKnown  全て
            // tagType != UnKnown  x.TagType == tagType
            // refin   == true     x.RefineFlag == true
            return this.TagToList(
                this._Tags.Where(
                    x => x.TagType == tagType && tagType != Tag.enmTagType.UnKnown
                    && (!refine || x.RefineFlag)
                    || tagType == Tag.enmTagType.UnKnown 
                    ).ToList());
        }
        #endregion タグリストを取得する

        #region タグリストを指定したタグタイプの文字列リストに変換する
        /// <summary>
        /// タグリストを指定したタグタイプの文字列リストに変換する
        /// </summary>
        /// <param name="tag">タグリスト</param>
        /// <returns>文字列リスト</returns>
        internal List<string> TagToList(
            List<Tag> tag
            )
        {
            // 戻り値初期化
            var ret = new List<string>();

            // 要素数繰り返す
            for (int i = 0; i < tag.Count; i++)
            {
                // 要素を追加する
                ret.Add(tag[i].TagString);
            }

            return ret;
        }
        #endregion タグリストを指定したタグタイプの文字列リストに変換する

        #endregion メソッド
    }
    #endregion タグコレクションクラス
}
