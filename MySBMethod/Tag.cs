using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser
{
    #region タグクラス
    /// <summary>
    /// タグクラス
    /// </summary>
    public class Tag
    {
        #region 列挙体
        /// <summary>
        /// タイプ
        /// </summary>
        public enum enmTagType : int
        {
            /// <summary>
            /// タグ
            /// </summary>
            Tag = 0,
            /// <summary>
            /// テキスト
            /// </summary>
            Text,
            /// <summary>
            /// 不明
            /// </summary>
            UnKnown,
        }
        #endregion 列挙体

        #region メンバ変数
        /// <summary>
        /// LineNo
        /// </summary>
        private int _LineNo = 0;
        /// <summary>
        /// タグタイプ
        /// </summary>
        private Tag.enmTagType _TagType = enmTagType.UnKnown;
        /// <summary>
        /// タグ文字列
        /// </summary>
        private string _TagString = string.Empty;
        /// <summary>
        /// 絞り込みフラグ
        /// </summary>
        private bool _RefineFlag = false;
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// LineNo
        /// </summary>
        public int LineNo
        {
            set { this._LineNo = value; }
            get { return this._LineNo; }
        }
        /// <summary>
        /// タグ文字列
        /// </summary>
        public string TagString
        {
            set { this._TagString = value.Trim(); }
            get { return this._TagString; }
        }
        /// <summary>
        /// タグタイプ
        /// </summary>
        public Tag.enmTagType TagType
        {
            set { this._TagType = value; }
            get { return this._TagType; }
        }
        /// <summary>
        /// 絞り込みフラグ
        /// </summary>
        public bool RefineFlag
        {
            get { return this._RefineFlag; }
        }
        #endregion プロパティ

        #region メソッド

        #region Clear
        /// <summary>
        /// 内部変数を初期値に設定する
        /// </summary>
        public void Clear()
        {
            // 変数クリア
            this._TagType = enmTagType.UnKnown;
            this._TagString = string.Empty;
            this._RefineFlag = false;
        }
        #endregion Clear

        #region ClearRefineFlag
        /// <summary>
        /// 絞り込みフラグクリア
        /// </summary>
        public void ClearRefineFlag()
        {
            // クリア
            this._RefineFlag = false;
        }
        #endregion ClearRefineFlag

        #region SetRefineFlag
        /// <summary>
        /// 絞込フラグセット
        /// </summary>
        public void SetRefineFlag()
        {
            // セット
            this._RefineFlag = true;
        }
        #endregion SetRefineFlag

        #region Copy
        /// <summary>
        /// 原本の内容を複製する
        /// </summary>
        /// <param name="src">原本</param>
        public void Copy(
            Tag src
            )
        {
            // コピー
            this.TagType = src.TagType;
            this.TagString = src.TagString;
        }
        #endregion Copy

        #region Find
        /// <summary>
        /// 文字列内に指定文字列があるか調べて位置を返す
        /// Refineは、絞り込みを行う場合のみtrueとする
        /// 最初に見つかった位置を0以上の整数で返す
        /// 見つからなければ、-1を返す
        /// </summary>
        /// <param name="text">指定文字列</param>
        /// <param name="refine">絞り込みフラグ</param>
        /// <returns>位置</returns>
        public int Find(
            string text,
            bool refine = false
            )
        {
            int ret = -1;

            // 絞り込みチェック
            if (refine)
            {
                // 対象の場合、絞り込みを行う
                if (this._RefineFlag)
                {
                    // 文字を検索する
                    ret = this.FindText(text);
                }
            }
            else
            {
                // 文字を検索する
                ret = this.FindText(text);
            }

            return ret;
        }
        /// <summary>
        /// 文字列内に指定文字列があるか調べて位置を返す
        /// 最初に見つかった位置を0以上の整数で返す
        /// 見つからなければ、-1を返す
        /// 同時に絞り込みフラグ設定を行う
        /// </summary>
        /// <param name="text">検索文字</param>
        /// <returns>位置</returns>
        public int FindText(
            string text
            )
        {
            // 文字を検索する
            int ret = this._TagString.ToUpper().IndexOf(text.ToUpper());

            // 発見フラグ設定
            this._RefineFlag = ret >= 0;

            return ret;
        }
        #endregion Find

        #endregion メソッド
    }
    #endregion タグクラス
}
