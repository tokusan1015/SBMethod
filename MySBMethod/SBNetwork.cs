using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlParser;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用独自Network
    /// <summary>
    /// SmallBasic用Network
    /// </summary>
    [SmallBasicType]
    public static class SBNetwork
    {
        #region メンバ変数
        /// <summary>
        /// HtmlAnalyser
        /// </summary>
        private static HtmlDocument _HtmlDocument = null;
        #endregion メンバ変数

        #region 公開メソッド

        #region URLが有効かチェックする
        /// <summary>
        /// URLが有効か検証し、
        /// 結果をBOOLで取得します
        /// 有効な場合、true
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>BOOL</returns>
        public static Primitive CheckURL(
            this Primitive url
            )
        {
            // 引数チェック(空文字チェック)
            if (url.ToString().Length < 1) return false;

            // URLが有効かチェックする
            Uri uri = null;
            return (Primitive)Uri.TryCreate(url.ToString(), UriKind.Absolute, out uri) && null != uri;
        }
        #endregion URLが有効かチェックする

        #region TextをURLとして標準ブラウザで開く
        /// <summary>
        /// TextをURLとして標準ブラウザで開き、
        /// 結果をBOOLで取得します
        /// urlがEmptyの場合、何もしません
        /// 正常終了の場合、true
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>正常終了の場合true</returns>
        public static Primitive OpenBrowser(
            this Primitive url
            )
        {
            // URLが有効かチェックする
            bool ret = (bool)url.CheckURL();

            // 結果チェック
            if (ret)
            {
                // 有効な場合、TextをURLとしてWebブラウザで開く
                System.Diagnostics.Process.Start(
                    fileName: url
                    );
            }

            return (Primitive)ret;
        }
        #endregion TextをURLとして標準ブラウザで開く

        #region HtmlParserLoad
        /// <summary>
        /// 解析用のurlをロードし、
        /// 解析を実行した上で
        /// 結果を取得します
        /// ロードに成功した場合trueが返ります
        /// </summary>
        /// <param name="url">文字列</param>
        /// <returns>BOOL</returns>
        public static Primitive HtmlParserLoad(
            Primitive url
            )
        {
            // インスタンスを生成する
            if (_HtmlDocument == null)
                _HtmlDocument = new HtmlDocument();

            return _HtmlDocument.Load(url.ToString());
        }
        #endregion HtmlParserLoad

        #region GetTagList
        /// <summary>
        /// HtmlのTag部分を文字列配列として取得します
        /// FindTagの結果を取得する場合trueにします
        /// 事前にHtmlParserLoadを実行する必要があります
        /// </summary>
        /// <param name="find">絞込結果</param>
        /// <returns>文字列配列</returns>
        public static Primitive GetTagList(
            Primitive find
            )
        {
            // _HtmlDocument nullチェック
            if (_HtmlDocument == null)
                throw new ArgumentNullException("HtmlParserLoad is not execute");

            var list = _HtmlDocument.GetList_Tag((bool)find);

            return list.ConvertDimStringToPrimitive();
        }
        #endregion GetTagList

        #region GetTextList
        /// <summary>
        /// HtmlのText部分を文字列配列として取得します
        /// FindTagの結果を取得する場合trueにします
        /// FindTagで絞り込まれたTag位置に存在する
        /// テキストを取得します
        /// 事前にHtmlParserLoadを実行する必要があります
        /// </summary>
        /// <param name="find">絞込結果</param>
        /// <returns>文字列配列</returns>
        public static Primitive GetTextList(
            Primitive find
            )
        {
            // _HtmlDocument nullチェック
            if (_HtmlDocument == null)
                throw new ArgumentNullException("HtmlParserLoad is not execute");

            var list = _HtmlDocument.GetList_Text((bool)find);

            return list.ConvertDimStringToPrimitive();
        }
        #endregion GetTextList

        #region Find
        /// <summary>
        /// 検索文字列で検索を行います
        /// 検索文字列は、'?'で区切り列挙します
        /// 全ての検索文字のAND条件で検索されます
        /// 例)
        /// text1, text2,..., textnのAND検索の場合、
        /// 検索文字列は、"text1?text2?..., textn"
        /// となります
        /// 事前にHtmlParserLoadを実行する必要があります
        /// </summary>
        /// <param name="findText">検索文字列</param>
        public static void Find(
            Primitive findText
            )
        {
            // _HtmlDocument nullチェック
            if (_HtmlDocument == null)
                throw new ArgumentNullException("HtmlParserLoad is not execute");

            _HtmlDocument.Find(args: findText.ToString().Split('?'));
        }
        #endregion Find

        #region FindRange
        /// <summary>
        /// 検索開始文字列から検索終了文字列までを対象とします
        /// </summary>
        /// <param name="start">検索開始文字列</param>
        /// <param name="end">検索終了文字列</param>
        public static void FindRange(
            Primitive start,
            Primitive end
            )
        {
            // _HtmlDocument nullチェック
            if (_HtmlDocument == null)
                throw new ArgumentNullException("HtmlParserLoad is not execute");

            _HtmlDocument.FindRange(
                start: start.ToString(),
                end: end.ToString()
                );
        }
        #endregion FindRange

        #endregion 公開メソッド
    }
    #endregion SmallBasic用Network
}
