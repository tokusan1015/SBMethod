using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMethod
{
    /// <summary>
    /// ベクトルクラス
    /// </summary>
    class Vector
    {
        /// <summary>
        /// 長さ
        /// </summary>
        public double length { set; get; } = 0;
        /// <summary>
        /// 角度
        /// </summary>
        public double theta { set; get; } = 0;
    }
    #region 座標クラス
    /// <summary>
    /// 座標クラス
    /// </summary>
    class Point
    {
        #region プロパティ
        /// <summary>
        /// Ｘ座標
        /// </summary>
        public int x { set; get; } = 0;
        /// <summary>
        /// Ｙ座標
        /// </summary>
        public int y { set; get; } = 0;
        /// <summary>
        /// 角度
        /// </summary>
        public double theta { get { return System.Math.Atan(y / x); } }
        /// <summary>
        /// 斜辺長
        /// </summary>
        public double length { get { return System.Math.Sqrt(x * x + y * y); } }
        #endregion プロパティ
    }
    #endregion 座標クラス

    /// <summary>
    /// ポリゴンクラス
    /// </summary>
    class ClassPolygon
    {
        #region メンバ変数
        /// <summary>
        /// XY座標リスト
        /// </summary>
        private List<Point> _Points;
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// XY座標リスト
        /// </summary>
        IEnumerable<Point> Points
        {
            get { return this._Points; }
            set { this._Points = value.ToList(); }
        }
        #endregion プロパティ

        #region ピックの定理を用いて多角形の面積を求める
        /// <summary>
        /// ピックの定理を用いて多角形の面積を求める
        /// 等間隔に点が存在する平面上にある多角形の面積を求める
        /// 多角形の頂点は全て格子点（等間隔に配置されている点）上にあり、
        /// 内部に穴は開いていないものとする
        /// 多角形の内部にある格子点の個数を i、
        /// 辺上にある格子点の個数を b とすると
        /// この多角形の面積 S は
        /// S = i + b / 2 - 1
        /// となる
        /// xy座標は一筆書きになるように設定する必要がある
        /// また、座標は正の整数でなければならない
        /// 交差する場合はエラーとなる
        /// ***** 未作成 *****
        /// </summary>
        /// <param name="points">xyの座標</param>
        /// <returns>多角形の面積</returns>
        decimal PicksTheorem(
            IEnumerable<Point> points
            )
        {
            decimal result = 0;

            for (int i = 0; i < points.Count(); i++)
            {
                // 最後のデータかチェック
                if (i == points.Count() - 1)
                {
                    // 最後のデータの場合、先頭のデータでチェックする

                }
                else
                {
                    // 最後のデータでない場合、次のデータでチェックする

                }
            }

            return result;
        }

        /// <summary>
        /// 線分(A, B)と線分(C, D)の公差判定を行う
        /// </summary>
        /// <param name="a">座標A</param>
        /// <param name="b">座標B</param>
        /// <param name="c">座標C</param>
        /// <param name="d">座標D</param>
        /// <returns>公差している場合true</returns>
        bool CrossingLine(
            Point a,
            Point b,
            Point c,
            Point d
            )
        {
            // 準備の計算
            double abx = a.x - b.x;
            double aby = a.y - b.y;
            double cdx = c.x - d.x;
            double cdy = c.y - d.y;

            // 傾きが同じ場合は平行とする
            if (abx != 0 && cdx != 0)
                if (aby / abx == cdy / cdx) return false;

            // 傾きが異なる場合は交差チェック
            double ta = cdx * (a.y - c.y) + cdy * (c.x - a.x);
            double tb = cdx * (b.y - c.y) + cdy * (c.x - b.x);
            double tc = abx * (c.y - a.y) + aby * (a.x - c.x);
            double td = abx * (d.y - a.y) + aby * (a.x - d.x);

            return ta * tb < 0 && tc * td < 0;
        }

        /// <summary>
        /// 座標xyが多角形listXYの内側にあるかを判定する
        /// ルール1: 上向きの辺は、開始点を含み終点を含まない。
        /// ルール2: 下向きの辺は、開始点を含まず終点を含む。
        /// ルール3: 水平線Rと辺が水平でない(がRと重ならない)。
        /// ルール4: 水平線Rと辺の交点は厳密に点Pの右側になくてはならない。
        /// </summary>
        /// <param name="points">多角形座標</param>
        /// <param name="point">座標</param>
        /// <returns>内包している場合true</returns>
        bool CrossingNumberAlgorithm(
            IEnumerable<Point> points, 
            Point point
            )
        {
            // 戻り値
            int ret = 0;
            // 引数をListに変換
            var lp = (List<Point>)points;
            // 座標数繰り返す
            for (int i = 0; i < points.Count() - 1; i++)
            {
                // 上向きの辺。点Pがy軸方向について、始点と終点の間にある。ただし、終点は含まない。(ルール1)
                if (((lp[i].y <= point.y) && (lp[i + 1].y > point.y))
                    // 下向きの辺。点Pがy軸方向について、始点と終点の間にある。ただし、始点は含まない。(ルール2)
                    || ((lp[i].y > point.y) && (lp[i + 1].y <= point.y)))
                {
                    // ルール1,ルール2を確認することで、ルール3も確認できている。
                    // 辺は点pよりも右側にある。ただし、重ならない。(ルール4)
                    // 辺が点pと同じ高さになる位置を特定し、その時のxの値と点pのxの値を比較する。
                    var vt = (point.y - lp[i].y) / (lp[i + 1].y - lp[i].y);
                    if (point.x < (lp[i].x + (vt * (lp[i + 1].x - lp[i].x))))
                    {
                        ++ret;
                    }
                }
            }
            return ret % 2 != 0;
        }
        #endregion ピックの定理を用いて多角形の面積を求める

    }
}
