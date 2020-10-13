using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace HtmlParser
{
    #region インターフェース
    /// <summary>
    /// HtmlDocumentインターフェース
    /// </summary>
    public interface IHtmlDocument
    {
        #region プロパティ
        /// <summary>
        /// Html文字数
        /// </summary>
        int HtmlLength
        {
            get;
        }
        #endregion プロパティ

        #region メソッド
        /// <summary>
        /// Html文字列を設定する
        /// </summary>
        /// <param name="text">Html文字列</param>
        void SetHtmlText(
            string text
            );
        /// <summary>
        /// Html文字列をクリアする
        /// </summary>
        void HtmlTextClear();
        /// <summary>
        /// 複数検索文字列で絞り込みを繰り返す
        /// And条件となる
        /// </summary>
        /// <param name="args">検索文字列配列</param>
        void Find(
            string[] args
            );
        /// <summary>
        /// 初回検索
        /// タグ内に指定文字が存在するか検索する
        /// </summary>
        /// <param name="text">検索文字列</param>
        void FindFirst(
            string text
            );
        /// <summary>
        /// 絞り込み検索
        /// 前回の検索結果に対して実行する
        /// タグ内に指定文字が存在するか検索する
        /// And条件となる
        /// </summary>
        /// <param name="text">検索文字列</param>
        void FindRefine(
            string text
            );
        /// <summary>
        /// タグとテキストのリストを取得する
        /// </summary>
        /// <param name="Refine">検索結果の場合true</param>
        /// <returns>リスト</returns>
        List<string> GetList_All(
            bool Refine = true
            );
        /// <summary>
        /// タグのリストを取得する
        /// </summary>
        /// <param name="Refine">検索結果の場合true</param>
        /// <returns>リスト</returns>
        List<string> GetList_Tag(
            bool Refine = true
            );
        /// <summary>
        /// テキストのリストを取得する
        /// </summary>
        /// <param name="Refine">検索結果の場合true</param>
        /// <returns>リスト</returns>
        List<string> GetList_Text(
            bool Refine = true
            );
        #endregion メソッド
    }
    #endregion インターフェース

    #region HtmlDocument
    /// <summary>
    /// HtmlDocument
    /// </summary>
    public class HtmlDocument : IHtmlDocument
    {
        #region メンバ変数
        /// <summary>
        /// Html解析クラス
        /// </summary>
        private HtmlAnalyser _Analyse = new HtmlAnalyser();
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// Html文字数
        /// </summary>
        public int HtmlLength
        {
            get { return this._Analyse.HtmlText.Length; }
        }
        /// <summary>
        /// 全角半角変換フラグ
        /// </summary>
        public bool ChangeDoubleByteSpace
        {
            set;
            get;
        } = true;
        #endregion プロパティ

        #region メソッド

        #region SetHtmlText
        /// <summary>
        /// Html文字列を設定する
        /// </summary>
        /// <param name="text">Html文字列</param>
        public void SetHtmlText(
            string text
            )
        {
            // HtmlText設定
            this._Analyse.HtmlText = text;

            // 解析実行
            this._Analyse.AnalyseHtml(this.ChangeDoubleByteSpace);
        }
        #endregion SetHtmlText

        #region Load
        /// <summary>
        /// urlに接続しHtmlを取得する
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>成功した場合true</returns>
        public bool Load(
            string url
            )
        {
            // urlが有効かチェックする
            if (!this.CheckURL(url))
                // 無効な場合何もしない
                return false;

            // Htmlを取得してHtmlTextに設定する
            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            this.SetHtmlText(wc.DownloadString(url));

            return this._Analyse.HtmlText.Length > 0;
        }
        #endregion Load

        #region HtmlTextClear
        /// <summary>
        /// Html文字列をクリアする
        /// </summary>
        public void HtmlTextClear()
        {
            // HtmlTextクリア
            this._Analyse.HtmlText = string.Empty;
        }
        #endregion HtmlTextClear

        #region Find
        /// <summary>
        /// 複数検索文字列で絞り込みを繰り返す
        /// And条件となる
        /// </summary>
        /// <param name="args">検索文字列配列</param>
        public void Find(
            string[] args
            )
        {
            // 引数チェック
            if (args == null)
                throw new ArgumentNullException("args");
            // 1件未満の場合は、何もしない
            if (args.Length < 1) return;

            // 初回検索を行う
            this.FindFirst(args[0]);

            // argsの件数回絞り込みを行う
            for (int i = 1; i < args.Length; i++)
            {
                // 絞り込み検索を行う
                this.FindRefine(args[i]);
            }
        }
        #endregion Find

        #region FindFirst
        /// <summary>
        /// 初回検索
        /// タグ内に指定文字が存在するか検索する
        /// </summary>
        /// <param name="text">検索文字列</param>
        public void FindFirst(
            string text
            )
        {
            // 初回検索
            this._Analyse.Find(
                text: text, 
                refine: false
                );
        }
        #endregion FindFirst

        #region FindRefine
        /// <summary>
        /// 絞り込み検索
        /// 前回の検索結果に対して実行する
        /// タグ内に指定文字が存在するか検索する
        /// </summary>
        /// <param name="text">検索文字列</param>
        public void FindRefine(
            string text
            )
        {
            // 初回検索
            this._Analyse.Find(
                text: text,
                refine: true
                );
        }
        #endregion FindRefine

        #region FindRange
        /// <summary>
        /// 絞込開始文字列から
        /// 絞込終了文字列までを結果とする
        /// </summary>
        /// <param name="start">開始文字列</param>
        /// <param name="end">終了文字列</param>
        public void FindRange(
            string start,
            string end
            )
        {
            this._Analyse.FindRange(
                start: start,
                end: end
                );
        }
        #endregion FindRange

        #region GetList_All
        /// <summary>
        /// タグとテキストのリストを取得する
        /// </summary>
        /// <param name="Refine">検索結果の場合true</param>
        /// <returns>リスト</returns>
        public List<string> GetList_All(
            bool Refine = true
            )
        {
            // リストを取得する
            return this._Analyse.GetTagList(
                tagType: Tag.enmTagType.UnKnown,
                refine: Refine
                );
        }
        #endregion GetList_All

        #region GetList_Tag
        /// <summary>
        /// タグのリストを取得する
        /// </summary>
        /// <param name="Refine">検索結果の場合true</param>
        /// <returns>リスト</returns>
        public List<string> GetList_Tag(
            bool Refine = true
            )
        {
            // タグリストを取得する
            return this._Analyse.GetTagList(
                tagType: Tag.enmTagType.Tag,
                refine: Refine
                );
        }
        #endregion GetList_Tag

        #region GetList_Text
        /// <summary>
        /// テキストのリストを取得する
        /// </summary>
        /// <param name="Refine">検索結果の場合true</param>
        /// <returns>リスト</returns>
        public List<string> GetList_Text(
            bool Refine = true
            )
        {
            // テキストリストを取得する
            return this._Analyse.GetTagList(
                tagType: Tag.enmTagType.Text,
                refine: Refine
                );
        }
        #endregion GetList_Text

        #region CheckURL
        /// <summary>
        /// URLが有効かチェックする
        /// 有効な場合、true
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>有効な場合True</returns>
        private bool CheckURL(
            string url
            )
        {
            // URLが空文字の場合何もしない
            if (url.Length < 1) return false;

            // URLが有効かチェックする
            Uri uri = null;

            // 結果を返す
            return Uri.TryCreate(
                uriString: url,
                uriKind: UriKind.Absolute,
                result: out uri
                ) && null != uri;
        }
        #endregion CheckURL

        #endregion メソッド
    }
    #endregion HtmlDocument
}
