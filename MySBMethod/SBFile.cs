using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用ファイル
    /// <summary>
    /// SmallBasic用ファイル
    /// </summary>
    [SmallBasicType]
    public static class SBFile
    {
        #region 公開メソッド

        #region ファイルの完全パス名から拡張子付きのファイル名を文字列で取得します
        /// <summary>
        /// ファイルの完全パス名から拡張子付きのファイル名を文字列で取得します
        /// </summary>
        /// <param name="fullPathName">ファイルの完全パス名</param>
        /// <returns>文字列</returns>
        public static Primitive GetFileName(
            Primitive fullPathName
            )
        {
            return fullPathName.ToString().GetFileName();
        }
        #endregion ファイルの完全パス名から拡張子付きのファイル名を取得します

        #region ファイルの完全パス名から拡張子無しのファイル名を取得します
        /// <summary>
        /// ファイルの完全パス名から拡張子無しのファイル名を文字列で取得します
        /// </summary>
        /// <param name="fullPathName">ファイルの完全パス名</param>
        /// <returns>文字列</returns>
        public static Primitive GetFileNameWithoutExtension(
            Primitive fullPathName
            )
        {
            return fullPathName.ToString().GetFileNameWithoutExtension();
        }
        #endregion ファイルの完全パス名から拡張子無しのファイル名を取得します

        #region ファイルの完全パス名からファイルの拡張子を取得します
        /// <summary>
        /// ファイルの完全パス名からファイルの拡張子を文字列で取得します
        /// </summary>
        /// <param name="fullPathName">ファイルの完全パス名</param>
        /// <returns>文字列</returns>
        public static Primitive GetExtension(
            Primitive fullPathName
            )
        {
            return fullPathName.ToString().GetExtension();
        }
        #endregion ファイルの完全パス名からファイルの拡張子を取得します

        #region ファイルの完全パス名から親ディレクトリの完全パス名を取得します
        /// <summary>
        /// ファイルの完全パス名から親ディレクトリの完全パス名を文字列で取得します
        /// </summary>
        /// <param name="fullPathName">ファイルの完全パス名</param>
        /// <returns>文字列</returns>
        public static Primitive GetDirectoryName(
            Primitive fullPathName
            )
        {
            return fullPathName.ToString().GetDirectoryName();
        }
        #endregion ファイルの完全パス名からファイル名を取得します

        #region ファイルの完全パス名からルートパスを取得します
        /// <summary>
        /// ファイルの完全パス名からルートパスを文字列で取得します
        /// </summary>
        /// <param name="fullPathName">ファイルの完全パス名</param>
        /// <returns>文字列</returns>
        public static Primitive GetPathRoot(
            Primitive fullPathName
            )
        {
            return fullPathName.ToString().GetPathRoot();
        }
        #endregion ファイルの完全パス名からルートパスを取得します

        #region ファイルの完全パス名からディレクトリ名の一覧を取得します
        /// <summary>
        /// ファイルの完全パス名からディレクトリ名の一覧を文字列配列で取得します
        /// 例)
        /// var result = GetDirectoryNameList("c:\aaa\bbb\ccc\ddd.jpg")
        /// result[1] = "c:"
        /// result[2] = "aaa"
        /// result[3] = "bbb"
        /// result[4] = "ddd.jpg"
        /// </summary>
        /// <param name="fullPathName">フルパスファイル名</param>
        /// <returns>文字列配列</returns>
        public static Primitive GetDirectoryNameList(
            Primitive fullPathName
            )
        {
            return fullPathName.ToString()
                .GetDirectoryNameList()
                .ConvertDimStringToPrimitive();
        }
        #endregion ファイルの完全パス名からディレクトリ名の一覧を取得します

        #region ファイルが存在するか確認します
        /// <summary>
        /// ファイルが存在するか検証します
        /// 存在する場合は、trueを返します
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>BOOL</returns>
        public static Primitive CheckFileExists(
            this Primitive fileName
            )
        {
            // 存在チェック
            return (Primitive)System.IO.File.Exists(fileName);
        }
        #endregion ファイルが存在するか確認します

        /// <summary>
        /// 文字列配列をファイルに出力します
        /// </summary>
        public static void OutputDimension(
            Primitive datas,
            Primitive filePathName
            )
        {
            // 配列をファイルに出力します
            datas.ConvertPrimitiveToDimString()
                .OutputList(filePathName.ToString());
        }

        #region ファイルまたはフォルダーをZip形式で圧縮します
        /// <summary>
        /// ファイルまたはフォルダーをZip形式で圧縮します
        /// 成功した場合は、trueを返します
        /// </summary>
        /// <param name="archiveFilePathName">圧縮後ファイル名</param>
        /// <param name="filePathName">圧縮するファイル(フォルダ)名</param>
        /// <param name="bDeleteOriginalFile"></param>
        /// <returns>BOOL</returns>
        public static Primitive CompressFile(
            Primitive archiveFilePathName,
            Primitive filePathName,
            Primitive bDeleteOriginalFile
            )
        {
            // 圧縮クラスを生成する
            var zip = new ClassZipArchiver();

            // データ加工
            string zipFile = archiveFilePathName.ToString();
            string basePath = filePathName.ToString().GetDirectoryName();
            string fileName = filePathName.ToString().GetFileName();

            // ファイルを圧縮する
            return zip.CompressFile(
                archiveFilePathName: zipFile,
                basePath: basePath,
                fileNames: new List<string>() { fileName },
                eCompressionLevel: System.IO.Compression.CompressionLevel.Optimal,
                bDeleteOriginalFile: false
                );
        }
        #endregion ファイルまたはフォルダーをZip形式で圧縮します

        #region zip形式のファイルを解凍します
        /// <summary>
        /// zip形式のファイルを解凍します
        /// 成功した場合は、trueを返します
        /// </summary>
        /// <param name="zipFilePathName">圧縮ファイルパス名</param>
        /// <param name="basePath">解凍フォルダ</param>
        /// <returns>BOOL</returns>
        public static Primitive DecompressFile(
            Primitive zipFilePathName,
            Primitive basePath
            )
        {
            // 圧縮クラスを生成する
            var zip = new ClassZipArchiver();

            // ファイルを圧縮する
            return zip.DecompressionFile(
                zipFilePathName: zipFilePathName.ToString(),
                basePath: basePath.ToString()
                );
        }
        #endregion zip形式のファイルを解凍します

        #endregion 公開メソッド
    }
    #endregion SmallBasic用ファイル
}
