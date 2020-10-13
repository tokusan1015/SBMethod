using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用メッセージボックス
    /// <summary>
    /// SmallBasic用メッセージボックス
    /// </summary>
    [SmallBasicType]
    public static class SBMessageBox 
    {
        #region 公開メソッド

        #region 入力ボックスを表示して入力内容を返す
        /// <summary>
        /// 入力ボックスを表示して入力内容を文字列で取得します
        /// </summary>
        /// <param name="text">表示するメッセージ</param>
        /// <param name="title">表示するタイトル</param>
        /// <returns>文字列</returns>
        public static Primitive ShowInputBox(
            Primitive text,
            Primitive title
            )
        {
            return (Primitive)Interaction.InputBox(
                Prompt: text.ToString(),
                Title: title.ToString()
                );
        }
        #endregion 入力ボックスを表示して入力内容を返す

        #region メッセージボックス(Ok)を表示して結果(ボタン文字)を返す
        /// <summary>
        /// メッセージボックス(Ok)を表示し、
        /// 結果(ボタン文字)を文字列で取得します
        /// </summary>
        /// <param name="text">表示するメッセージ</param>
        /// <param name="title">表示するタイトル</param>
        /// <returns>文字列</returns>
        public static Primitive ShowOk(
            Primitive text,
            Primitive title
            )
        {
            // メッセージボックスを表示して結果を返す
            return (Primitive)MessageBox.Show(
                text: text.ToString(),
                caption: title.ToString(),
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.None,
                defaultButton: MessageBoxDefaultButton.Button1,
                options: MessageBoxOptions.DefaultDesktopOnly
                ).ToString();
        }
        #endregion メッセージボックス(Ok)を表示して結果(ボタン文字)を返す

        #region メッセージボックス(Ok,Cancel)を表示して結果(ボタン文字)を返す
        /// <summary>
        /// メッセージボックス(Ok,Cancel)を表示し、
        /// 結果(ボタン文字)を文字列で取得します
        /// </summary>
        /// <param name="text">表示するメッセージ</param>
        /// <param name="title">表示するタイトル</param>
        /// <returns>文字列</returns>
        public static Primitive ShowOkCancel(
            Primitive text,
            Primitive title
            )
        {
            // メッセージボックスを表示して結果を返す
            return (Primitive)MessageBox.Show(
                text: text.ToString(), 
                caption: title.ToString(),
                buttons: MessageBoxButtons.OKCancel,
                icon: MessageBoxIcon.None,
                defaultButton: MessageBoxDefaultButton.Button2,
                options: MessageBoxOptions.DefaultDesktopOnly
                ).ToString();
        }
        #endregion メッセージボックス(Ok,Cancel)を表示して結果(ボタン文字)を返す

        #region メッセージボックス(Yes,No)を表示して結果(ボタン文字)を返す
        /// <summary>
        /// メッセージボックス(Yes,No)を表示し、
        /// 結果(ボタン文字)を文字列で取得します
        /// </summary>
        /// <param name="text">表示するメッセージ</param>
        /// <param name="title">表示するタイトル</param>
        /// <returns>文字列</returns>
        public static Primitive ShowYesNo(
            Primitive text,
            Primitive title
            )
        {
            // メッセージボックスを表示して結果を返す
            return (Primitive)MessageBox.Show(
                text: text.ToString(),
                caption: title.ToString(),
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.None,
                defaultButton: MessageBoxDefaultButton.Button2,
                options: MessageBoxOptions.DefaultDesktopOnly
                ).ToString();
        }
        #endregion メッセージボックス(Yes,No)を表示して結果(ボタン文字)を返す

        #region メッセージボックス(Yes,NO,Cancel)を表示して結果(ボタン文字)を返す
        /// <summary>
        /// メッセージボックス(Yes,NO,Cancel)を表示し、
        /// 結果(ボタン文字)を文字列で取得します
        /// </summary>
        /// <param name="text">表示するメッセージ</param>
        /// <param name="title">表示するタイトル</param>
        /// <returns>文字列</returns>
        public static Primitive ShowYesNoCancel(
            Primitive text,
            Primitive title
            )
        {
            // メッセージボックスを表示して結果を返す
            return (Primitive)MessageBox.Show(
                text: text.ToString(),
                caption: title.ToString(),
                buttons: MessageBoxButtons.YesNoCancel,
                icon: MessageBoxIcon.None,
                defaultButton: MessageBoxDefaultButton.Button3,
                options: MessageBoxOptions.DefaultDesktopOnly
                ).ToString();
        }
        #endregion メッセージボックス(Yes,NO,Cancel)を表示して結果(ボタン文字)を返す

        #endregion 公開メソッド
    }
    #endregion SmallBasic用メッセージボックス
}
