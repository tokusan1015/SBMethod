using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace SBMethod
{
    #region ファイル圧縮クラス
    /// <summary>
    /// ファイル圧縮クラス
    /// </summary>
    internal class ClassZipArchiver
    {
        #region 公開メソッド

        #region ファイルを圧縮(zip)する
        /// <summary>
        /// ファイルを圧縮(zip)する
        /// </summary>
        /// <param name="archiveFilePathName">圧縮後ファイルパス名</param>
        /// <param name="basePath">ベースパス</param>
        /// <param name="fileNames">ファイル名一覧</param>
        /// <param name="eCompressionLevel">圧縮レベル</param>
        /// <param name="bDeleteOriginalFile">オリジナル削除フラグ</param>
        /// <returns>成功したらtrue</returns>
        public bool CompressFile(
            string archiveFilePathName,
            string basePath,
            IEnumerable<string> fileNames,
            CompressionLevel eCompressionLevel = CompressionLevel.Optimal,
            bool bDeleteOriginalFile = false
            )
        {
            // ZIP圧縮処理
            var result = this.Compreession(
                archiveFilePathName: archiveFilePathName,
                basePath: basePath,
                fileNames: fileNames,
                eCompressionLevel: eCompressionLevel
              );

            // 削除フラグチェック
            if (result 
                && bDeleteOriginalFile)
            {
                // ファイル名で回す
                foreach (var s in fileNames)
                    // オリジナルファイルを削除する
                    System.IO.File.Delete(s);
            }

            return result;
        }
        #endregion ファイルを圧縮(zip)する

        #region ファイルを解凍(zip)する
        /// <summary>
        /// 圧縮されたファイルを解凍する
        /// </summary>
        /// <param name="zipFilePathName">圧縮ファイルパス名</param>
        /// <param name="basePath">解凍先ディレクトリパス</param>
        /// <returns>成功したらtrue</returns>
        public bool DecompressionFile(
            string zipFilePathName,
            string basePath
            )
        {
            return this.Decompression(
                zipFilePathName: zipFilePathName,
                basePath: basePath
                );
        }
        #endregion ファイルを解凍(zip)する

        #endregion 公開メソッド

        #region メソッド

        #region 指定パス以下に含まれるファイルパス一覧の作成
        /// <summary>
        /// 指定パス以下に含まれるファイルパス一覧の作成
        /// フォルダも展開する
        /// </summary>
        /// <param name="lstFilePath">ファイルパス一覧</param>
        /// <param name="sRootPath">一覧を作るディレクトリパス</param>
        private void GetFiles(
            List<string> lstFilePath,
            string sRootPath
            )
        {
            // ディレクトリの存在確認
            if (Directory.Exists(sRootPath))
            {
                // ルートパスをファイル一覧に保存
                lstFilePath.AddRange(Directory.GetFiles(sRootPath));

                // ルートパス以下のファイルで回す
                foreach (var dir in Directory.GetDirectories(sRootPath))
                    // ファイル一覧生成
                    this.GetFiles(lstFilePath, dir);
            }

            // ディレクトリが存在しない場合、ルートパス存在確認
            else if (System.IO.File.Exists(sRootPath))
            {
                // ルートパス追加
                lstFilePath.Add(sRootPath);
            }
        }
        #endregion 指定パス以下に含まれるファイルパス一覧の作成

        #region 圧縮する
        /// <summary>
        /// 圧縮する
        /// </summary>
        /// <param name="archiveFilePathName">圧縮後ファイルパス名</param>
        /// <param name="basePath">圧縮対象の基準ディレクトリ</param>
        /// <param name="fileNames">圧縮対象のファイル(フォルダ)名</param>
        /// <param name="eCompressionLevel">圧縮レベル。未指定でOptimal。</param>
        /// <returns>成功すればtrue</returns>
        private bool Compreession(
            string archiveFilePathName,
            string basePath,
            IEnumerable<string> fileNames,
            CompressionLevel eCompressionLevel = CompressionLevel.Optimal
            )
        {
            // ファイルの存在確認
            if (!Directory.Exists(path: basePath))
                return false;

            // zipファイルの存在確認
            if (System.IO.File.Exists(path: archiveFilePathName))
                return false;

            // パス結合処理
            if ((basePath[basePath.Length - 1] != '/') 
                && (basePath[basePath.Length - 1] != '\\'))
            {
                basePath += "\\";
            }

            //ファイル一覧の作成
            List<string> files = new List<string>();
            foreach (var path in fileNames)
                this.GetFiles(files, path);

            // Zipファイルオープン
            using (var zip = ZipFile.Open(
                archiveFileName: archiveFilePathName, 
                mode: ZipArchiveMode.Update
                ))
            {
                // 一覧のファイル数分繰り返す
                foreach (var path in files)
                {
                    // 圧縮パス生成
                    string compressPath =
                        path.Substring(
                            startIndex: basePath.Length,
                            length: path.Length - basePath.Length
                            );

                    // ファイル圧縮
                    zip.CreateEntryFromFile(
                        sourceFileName: path,
                        entryName: compressPath, 
                        compressionLevel: eCompressionLevel
                        );
                }
            }
            return true;
        }
        #endregion 圧縮する

        #region 解凍する
        /// <summary>
        /// 圧縮されたファイルを解凍する
        /// </summary>
        /// <param name="zipFilePathName">圧縮ファイルパス名</param>
        /// <param name="basePath">解凍先ディレクトリパス</param>
        /// <returns>成功したらtrue</returns>
        private bool Decompression(
            string zipFilePathName,
            string basePath
            )
        {
            // ファイルの存在確認
            if (!System.IO.File.Exists(zipFilePathName))
                return false;

            // ディレクトリの存在確認
            if (!Directory.Exists(basePath))
            {
                // 無ければ作成
                Directory.CreateDirectory(basePath);
            }

            // Zipファイルを開く
            using (var zip = ZipFile.OpenRead(zipFilePathName))
            {
                // ファイル数分繰り返し
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    // ファイルパス生成
                    string decompFilePath = Path.Combine(basePath, entry.FullName);
                    string decompPath = Path.GetDirectoryName(decompFilePath);
                    // ディレクトリの存在確認
                    if (!Directory.Exists(decompPath))
                    {
                        // 無ければ作成
                        Directory.CreateDirectory(decompPath);
                    }
                    // ファイルを展開
                    entry.ExtractToFile(Path.Combine(basePath, entry.FullName));
                }
            }
            return true;
        }
        #endregion 解凍する

        #endregion メソッド
    }
    #endregion ファイル圧縮クラス
}
