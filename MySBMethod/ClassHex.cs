using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMethod
{
    /// <summary>
    /// シミュレーションボードゲームなどのＨＥＸ板(横目)を管理するクラス
    /// </summary>
    class ClassHexBoard
    {
        /// <summary>
        /// 隣接タイプ
        /// </summary>
        public enum eRelatedType
        {
            Unknown = 0x00,
            Normal  = 0x01,
            Wide    = 0x02,
            Super   = 0x04,
            Extra   = 0x08
        }

        // ＨＥＸ一覧
        private List<ClassHexInfomation> _hexs = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="xmax">Ｘ数</param>
        /// <param name="ymax">Ｙ数</param>
        public ClassHexBoard(
            int xmax,
            int ymax
            )
        {
            // HEXを初期化する
            this._hexs = new List<ClassHexInfomation>();
            for (int y = 0; y < ymax; y++)
                for (int x = 0; x < xmax; x++)
                    this._hexs.Add(new ClassHexInfomation(x, y));
        }
        #endregion コンストラクタ

        #region メソッド

        #region 移動ＨＥＸ計算

        /// <summary>
        /// 移動テンポラリ計算
        /// </summary>
        /// <param name="mp">移動量</param>
        /// <param name="x">Ｘ基準点</param>
        /// <param name="y">Ｙ基準点</param>
        public void CalcMT(
            int mp,
            int x,
            int y
            )
        {
            // 移動テンポラリクリア
            this.clear_mt_all();

            // 基準点設定
            List<ClassHexInfomation> hex = this._hexs.Where(
                h => h.p.x == x && h.p.y == y
                ).ToList();
            // １件以外は例外
            if (hex.Count() != 1)
                throw new ArgumentException();

            // 移動テンポラリ設定
            hex[0].setmt(mp);

            // 移動量計算


        }
        #region 移動テンポラリクリア
        /// <summary>
        /// 移動テンポラリクリア
        /// </summary>
        public void clear_mt_all()
        {
            // 移動テンポラリクリア
            foreach (var mt in _hexs)
                mt.clear_mt();
        }
        #endregion 移動テンポラリクリア

        #endregion 移動ＨＥＸ計算

        #region 隣接HEXリストを取得する
        /// <summary>
        /// 隣接HEXリストを取得する
        /// </summary>
        /// <param name="hexInfo">HEX情報</param>
        /// <param name="flag">隣接タイプ</param>
        /// <returns>隣接HEXリスト</returns>
        public IEnumerable<ClassHexInfomation> GetRelatedHexList(
            ClassHexInfomation hexInfo,
            eRelatedType flag
            )
        {
            var result = new List<ClassHexInfomation>();

            // Normal
            if (flag.HasFlag(eRelatedType.Normal))
                result = result.Union(this.GetNormalRelatedHexList(hexInfo.p.x, hexInfo.p.y)).ToList();

            // Wide
            if (flag.HasFlag(eRelatedType.Wide))
                result = result.Union(this.GetWideRelatedHexList(hexInfo.p.x, hexInfo.p.y)).ToList();

            // Super
            if (flag.HasFlag(eRelatedType.Super))
                result = result.Union(this.GetSuperRelatedHexList(hexInfo.p.x, hexInfo.p.y)).ToList();

            // Extra
            if (flag.HasFlag(eRelatedType.Extra))
                result = result.Union(this.GetExtraRelatedHexList(hexInfo.p.x, hexInfo.p.y)).ToList();

            return result;
        }
        /// <summary>
        /// 隣接HEXリスト(〇)を取得する
        /// -2-1 0 1 2 
        /// ・・・・・ -2
        /// ・・〇〇・  -1
        /// ・〇ｘ〇・  0
        /// ・・〇〇・   1
        /// ・・・・・  2
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <returns>隣接HEXリスト</returns>
        public IEnumerable<ClassHexInfomation> GetNormalRelatedHexList(
            int x,
            int y
            )
        {
            // 対象ＨＥＸ
            // y                && (x - 1 || x + 1) 
            // (y - 1 || y + 1) && (x     || x + 1)
            return this._hexs.Where(
                h => ((h.p.y == y)                            && (h.p.x == x - 1 || h.p.x == x + 1))
                  || ((h.p.y == y - 1 || h.p.y == y + 1) && (h.p.x == x     || h.p.x == x + 1))
                );
        }
        /// <summary>
        /// ワイド隣接HEXリスト(△)を取得する
        /// -3-2-1 0 1 2 3 4
        /// ・・・・・・・・  -3
        /// ・・△△△・・・ -2
        /// ・・△〇〇△・・  -1
        /// ・△〇ｘ〇△・・  0
        /// ・・△〇〇△・・   1
        /// ・・△△△・・・  2
        /// ・・・・・・・・   3
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <returns>ワイド隣接HEXリスト</returns>
        public IEnumerable<ClassHexInfomation> GetWideRelatedHexList(
            int x,
            int y
            )
        {
            // y                && (x - 2 || x + 2) 
            // (y - 1 || y + 1) && (x - 1 || x + 2)
            // (y - 2 || y + 2) && (x - 1 || x || x + 1) 
            return this._hexs.Where(
                h => ((h.p.y == y)                            && (h.p.x == x - 2 || h.p.x == x + 2))
                  || ((h.p.y == y - 1 || h.p.y == y + 1) && (h.p.x == x - 1 || h.p.x == x + 2))
                  || ((h.p.y == y - 2 || h.p.y == y + 2) && (h.p.x == x - 1 || h.p.x == x     || h.p.x == x + 1))
                );
        }
        /// <summary>
        /// スーパーワイド隣接HEXリスト(□)を取得する
        /// -4-3-2-1 0 1 2 3 4 
        /// ・・・・・・・・・ -4
        /// ・・・□□□□・・  -3
        /// ・・□△△△□・・ -2
        /// ・・□△〇〇△□・  -1
        /// ・□△〇ｘ〇△□・  0
        /// ・・□△〇〇△□・   1
        /// ・・□△△△□・・  2
        /// ・・・□□□□・・   3
        /// ・・・・・・・・・  4
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <returns>スーパーワイド隣接HEXリスト</returns>
        public IEnumerable<ClassHexInfomation> GetSuperRelatedHexList(
            int x,
            int y
            )
        {
            // y                && (x - 3 || x + 3) 
            // (y - 1 || y + 1) && (x - 2 || x + 3)
            // (y - 2 || y + 2) && (x - 2 || x + 2) 
            // (y - 3 || y + 3) && (x - 1 || x || x + 1 || x + 2) 
            return this._hexs.Where(
                h => ((h.p.y == y)                            && (h.p.x == x - 3 || h.p.x == x + 3))
                  || ((h.p.y == y - 1 || h.p.y == y + 1) && (h.p.x == x - 2 || h.p.x == x + 3))
                  || ((h.p.y == y - 2 || h.p.y == y + 2) && (h.p.x == x - 2 || h.p.x == x + 2))
                  || ((h.p.y == y - 3 || h.p.y == y + 3) && (h.p.x == x - 1 || h.p.x == x || h.p.x == x + 1 || h.p.x == x + 2))
                );
        }
        /// <summary>
        /// エクストラワイド隣接HEXリスト(◎)を取得する
        /// -5-4-3-2-1 0 1 2 3 4 5 
        /// ・・・・・・・・・・・  -5
        /// ・・・◎◎◎◎◎・・・ -4
        /// ・・・◎□□□□◎・・  -3
        /// ・・◎□△△△□◎・・ -2
        /// ・・◎□△〇〇△□◎・  -1
        /// ・◎□△〇ｘ〇△□◎・  0
        /// ・・◎□△〇〇△□◎・   1
        /// ・・◎□△△△□◎・・  2
        /// ・・・◎□□□□◎・・   3
        /// ・・・◎◎◎◎◎・・・  4
        /// ・・・・・・・・・・・   5
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <returns>エクストラワイド隣接HEXリスト</returns>
        public IEnumerable<ClassHexInfomation> GetExtraRelatedHexList(
            int x,
            int y
            )
        {
            // y                && (x - 4 || x + 4) 
            // (y - 1 || y + 1) && (x - 3 || x + 4)
            // (y - 2 || y + 2) && (x - 3 || x + 3) 
            // (y - 3 || y + 3) && (x - 2 || x + 3) 
            // (y - 4 || y + 4) && (x - 2 || x - 1 || x || x + 1 || x + 2) 
            return this._hexs.Where(
                h => ((h.p.y == y)                            && (h.p.x == x - 4 || h.p.x == x + 4))
                  || ((h.p.y == y - 1 || h.p.y == y + 1) && (h.p.x == x - 3 || h.p.x == x + 4))
                  || ((h.p.y == y - 2 || h.p.y == y + 2) && (h.p.x == x - 3 || h.p.x == x + 3))
                  || ((h.p.y == y - 3 || h.p.y == y + 3) && (h.p.x == x - 2 || h.p.x == x + 3))
                  || ((h.p.y == y - 4 || h.p.y == y + 4) && (h.p.x == x - 2 || h.p.x == x - 1 || h.p.x == x || h.p.x == x + 1 || h.p.x == x + 2))
                );
        }
        #endregion 隣接HEXリストを取得する

        #endregion メソッド
    }
    /// <summary>
    /// ＨＥＸ情報クラス
    /// </summary>
    class ClassHexInfomation
    {
        /// <summary>
        /// 座標
        /// </summary>
        public Point p { get; private set; }

        /// <summary>
        /// 移動ポイント
        /// </summary>
        public int mp { get; private set; } = 1;

        /// <summary>
        /// 移動量テンポラリ
        /// </summary>
        public int mt { get; private set; } = -1;

        /// <summary>
        /// 移動量テンポラリフラグ
        /// </summary>
        public bool mtf { get; set; } = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        public ClassHexInfomation(
            int x,
            int y
            )
        {
            this.p.y = y;
            this.p.x = x;
            this.setmt(-1);
        }

        /// <summary>
        /// 移動量テンポラリクリア
        /// </summary>
        public void clear_mt()
        {
            this.mt = -1;
            this.mtf = false;
        }

        /// <summary>
        /// 移動量テンポラリ設定
        /// </summary>
        /// <param name="p">移動量</param>
        public void setmt(
            int p
            )
        {
            this.mt = p;
            this.mtf = true;
        }
    }
}
