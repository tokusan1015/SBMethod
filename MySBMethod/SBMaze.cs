using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用迷路生成
    /// <summary>
    /// SmallBasic用迷路生成
    /// </summary>
    [SmallBasicType]
    public static class SBMaze
    {
        #region 固定値
        /// <summary>
        /// 迷路データ未生成メッセージ
        /// </summary>
        private const string _MAZE_NOT_CREATE = "迷路が生成されていません";
        /// <summary>
        /// 迷路サイズエラー
        /// </summary>
        private const string _MAZE_SIZE_ERROR = "迷路サイズは 5(幅) x 5(高さ) 以上にしなければなりません";
        #endregion 固定値

        #region 列挙体
        /// <summary>
        /// XY軸
        /// </summary>
        public enum AxisXY : int
        {
            /// <summary>
            /// X軸
            /// </summary>
            X = 0,
            /// <summary>
            /// Y軸
            /// </summary>
            Y = 1
        }
        /// <summary>
        /// 方角
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// 北(上)
            /// </summary>
            North = 0,
            /// <summary>
            /// 東(右)
            /// </summary>
            East = 1,
            /// <summary>
            /// 南(下)
            /// </summary>
            South = 2,
            /// <summary>
            /// 西(左)
            /// </summary>
            West = 3
        }
        #endregion 列挙体

        #region メンバ変数
        /// <summary>
        /// 迷路データ
        /// </summary>
        private static int[,] _mazeData = null;
        #endregion メンバ変数

        #region 公開メソッド
        /// <summary>
        /// 幅(x),高さ(y)の迷路を生成して迷路データを数値配列で取得します
        /// 幅(x),高さ(y)は5以上の奇数を設定して下さい
        /// 偶数を設定した場合、+1した値に変更します
        /// 迷路データは[1][1]から[x][y]の2次元配列で構成されます
        /// 2次元配列には位置情報(通路(0)or壁(1))が格納されます
        /// </summary>
        /// <param name="width">幅(x)</param>
        /// <param name="height">高さ(y)</param>
        /// <returns>数値配列</returns>
        public static Primitive Create(
            Primitive width,
            Primitive height
            )
        {
            // 迷路インスタンス生成
            var maze = new Maze_Dig(
                width: width,
                height: height
                );

            // 迷路生成&保存
            _mazeData = maze.Create();

            // 迷路2次元配列を返す
            return GetMaze();
        }

        /// <summary>
        /// 生成した迷路データを数値配列で取得します
        /// 迷路データは[1][1]から[x][y]の2次元配列で構成されます
        /// 2次元配列には位置情報(通路(0)or壁(1))が格納されます
        /// </summary>
        /// <returns>数値配列</returns>
        public static Primitive GetMaze(
            )
        {
            // 迷路チェック
            _CheckMaze();

            // 結果を返す
            return _ConvertPrimitiveMazeData(
                mazeData: _mazeData
                );
        }

        /// <summary>
        /// 迷路の幅(x)を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetWidth()
        {
            // 迷路チェック
            _CheckMaze();

            // 結果を返す
            return _mazeData.GetLength((int)AxisXY.X);
        }

        /// <summary>
        /// 迷路の高さ(y)を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetHeight()
        {
            // 迷路チェック
            _CheckMaze();

            // 結果を返す
            return _mazeData.GetLength((int)AxisXY.Y);
        }
        /// <summary>
        /// 指定座標が壁か検証します
        /// 座標は1から始まる数字で指定する
        /// 壁の場合true
        /// </summary>
        /// <param name="x">座標x</param>
        /// <param name="y">座標y</param>
        /// <returns>BOOL</returns>
        public static Primitive IsWall(
            Primitive x,
            Primitive y
            )
        {
            // 迷路チェック
            _CheckMaze();

            // 座標位置が壁か調べ結果を返す
            return _IsPathOrWall(
                x: x - 1,
                y: y - 1
                ) == Maze_Dig.WALL;
        }
        /// <summary>
        /// 指定座標が通路か検証します
        /// 座標は1から始まる数字で指定する
        /// 通路の場合true
        /// </summary>
        /// <param name="x">座標x</param>
        /// <param name="y">座標y</param>
        /// <returns>BOOL</returns>
        public static Primitive IsPath(
            Primitive x,
            Primitive y
            )
        {
            // 迷路チェック
            _CheckMaze();

            // 範囲内は座標位置が通路か調べ結果を返す
            return _IsPathOrWall(
                x: x - 1,
                y: y - 1
                ) == Maze_Dig.PATH;
        }
        /// <summary>
        /// 指定座標から指定方向を見た時、
        /// その方向が壁か検証します
        /// 座標は1から始まる数字で指定する
        /// 方向は 北 = 0, 東 = 1, 南 = 2, 西 = 3 で指定する
        /// 壁の場合true
        /// 範囲外が指定された場合全て壁となる
        /// </summary>
        /// <param name="x">座標x</param>
        /// <param name="y">座標y</param>
        /// <param name="direction">方向</param>
        /// <returns>BOOL</returns>
        public static Primitive LookIsWall(
            Primitive x,
            Primitive y,
            Primitive direction
            )
        {
            // 迷路チェック
            _CheckMaze();

            // 見ている方向が壁の場合true
            return _Look(
                x: x - 1,
                y: y - 1,
                direction: direction
                ) == Maze_Dig.WALL;
        }
        /// <summary>
        /// 指定座標から指定方向を見た時、
        /// その方向が壁か検証します
        /// 座標は1から始まる数字で指定する
        /// 方向は 北 = 0, 東 = 1, 南 = 2, 西 = 3 で指定する
        /// 通路の場合true
        /// 範囲外が指定された場合全て壁となる
        /// </summary>
        /// <param name="x">座標x</param>
        /// <param name="y">座標y</param>
        /// <param name="direction">方向</param>
        /// <returns>BOOL</returns>
        public static Primitive LookIsPath(
            Primitive x,
            Primitive y,
            Primitive direction
            )
        {
            // 迷路チェック
            _CheckMaze();

            // 見ている方向が通路の場合true
            return _Look(
                x: x - 1,
                y: y - 1,
                direction: direction
                ) == Maze_Dig.PATH;
        }
        #endregion 公開メソッド

        #region メソッド
        /// <summary>
        /// 迷路チェックを行う
        /// 失敗した場合は例外となる
        /// </summary>
        private static void _CheckMaze()
        {
            // 生成チェック
            if (_mazeData == null)
                throw new ArgumentNullException(_MAZE_NOT_CREATE);
            // 迷路サイズチェック
            if (_mazeData.GetLength((int)AxisXY.X) < Maze_Dig.MIN_SIZE
                || _mazeData.GetLength((int)AxisXY.Y) < Maze_Dig.MIN_SIZE)
                throw new ArgumentException(_MAZE_SIZE_ERROR);
        }
        /// <summary>
        /// 迷路データ[x, y]をPrimitive配列[x][y]に変換する
        /// </summary>
        /// <param name="mazeData">迷路データ</param>
        /// <returns>結果</returns>
        private static Primitive _ConvertPrimitiveMazeData(
            int[,] mazeData
            )
        {
            // 戻り値生成
            var ret = new Primitive();

            for (int x = 0; x < mazeData.GetLength((int)AxisXY.X); x++)
            {
                var retY = new Primitive();
                for (int y = 0; y < mazeData.GetLength((int)AxisXY.Y); y++)
                {
                    retY[y + 1] = mazeData[x, y];
                }
                ret[x + 1] = retY;
            }

            return ret;
        }
        /// <summary>
        /// 指定座標の情報を返す
        /// 座標は0から始まる数字で指定する
        /// </summary>
        /// <param name="x">座標x</param>
        /// <param name="y">座標y</param>
        /// <returns>結果</returns>
        private static int _IsPathOrWall(
            int x,
            int y
            )
        {
            // 範囲外は全て壁
            if (y < 0 || y >= _mazeData.GetLength((int)AxisXY.Y))
                return Maze_Dig.WALL;
            if (x < 0 || x >= _mazeData.GetLength((int)AxisXY.X))
                return Maze_Dig.WALL;

            // 範囲内は座標位置の情報を返す
            return _mazeData[x, y];
        }
        /// <summary>
        /// 指定座標から指定方向を見た時、
        /// その方向の情報を取得する
        /// 座標は0から始まる数字で指定する
        /// 方向は 北 = 0, 東 = 1, 南 = 2, 西 = 3 で指定する
        /// 範囲外が指定された場合全て壁となる
        /// </summary>
        /// <param name="x">座標x</param>
        /// <param name="y">座標y</param>
        /// <param name="direction">方向</param>
        /// <returns>結果</returns>
        private static int _Look(
            int x,
            int y,
            int direction
            )
        {
            // 向きを数字に変換する
            var dire = (int)direction;

            // 戻り値初期化
            int ret = Maze_Dig.WALL;

            // 方向
            switch ((SBMaze.Direction)dire)
            {
                case Direction.North:
                    ret = _IsPathOrWall(x, y - 1);
                    break;
                case Direction.East:
                    ret = _IsPathOrWall(x + 1, y);
                    break;
                case Direction.South:
                    ret = _IsPathOrWall(x, y + 1);
                    break;
                case Direction.West:
                    ret = _IsPathOrWall(x - 1, y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }

            return ret;
        }
        #endregion メソッド
    }
    #endregion SmallBasic用迷路生成

    #region 迷路生成(穴掘り法)クラス
    /// <summary>
    /// 迷路生成(穴掘り法)クラス
    /// (1)迷路全体を構成する2次元配列を、幅高さ5以上の奇数で生成する
    /// (2)迷路の外周を通路(0)とし、それ以外を壁(1)とする
    /// (3)x, yともに奇数となる座標(任意)を穴掘り開始座標する
    /// (4)穴掘り
    ///   ・指定座標(開始座標)を通路(0)にする
    ///   ・掘り進める方向(1セル先が通路かつ2セル先が壁の方向)をランダムで決定し
    ///   　2セル先まで通路(0)とする
    ///   　掘り進められなくなるまで繰り返す
    ///   ・掘り進めた結果四方のどこにも進めなくなった場合
    ///   　すでに通路となった座標(x, yともに奇数)をランダムに取得し
    ///   　(4) の穴掘り処理を再帰的に呼び出す
    /// </summary>
    internal class Maze_Dig
    {
        #region 固定値
        /// <summary>
        /// 迷路最小サイズ
        /// </summary>
        public const int MIN_SIZE = 5;
        /// <summary>
        /// 通路
        /// </summary>
        public const int PATH = 0;
        /// <summary>
        /// 壁
        /// </summary>
        public const int WALL = 1;
        #endregion 固定値

        #region メンバ変数
        /// <summary>
        /// 2次元配列の迷路データ
        /// </summary>
        private int[,] _MazeData;
        /// <summary>
        /// 穴掘り開始候補座標
        /// </summary>
        private List<Cell> _StartPoint;
        #endregion メンバ変数

        #region 公開プロパティ
        /// <summary>
        /// 迷路データ
        /// </summary>
        public int[,] MazeData
        {
            get { return this._MazeData; }
        }
        #endregion 公開プロパティ

        #region プロパティ
        /// <summary>
        /// 迷路幅
        /// </summary>
        private int _Width { get; set; } = MIN_SIZE;
        /// <summary>
        /// 迷路高
        /// </summary>
        private int _Height { get; set; } = MIN_SIZE;
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// width, heightを偶数指定した場合1つ大きいサイズを生成する
        /// </summary>
        /// <param name="width">迷路幅(最小値5かつ奇数)</param>
        /// <param name="height">迷路高(最小値5かつ奇数)</param>
        public Maze_Dig(
            int width = MIN_SIZE,
            int height = MIN_SIZE
            )
        {
            // 5未満のサイズは生成できない
            if (width < MIN_SIZE || height < MIN_SIZE)
                throw new ArgumentOutOfRangeException();

            // 偶数の場合奇数にする
            if (width % 2 == 0) width++;
            if (height % 2 == 0) height++;

            // 迷路情報を初期化
            this._Width = width;
            this._Height = height;
            this._MazeData = new int[width, height];
            this._StartPoint = new List<Cell>();
        }
        #endregion コンストラクタ

        #region 公開メソッド

        #region 迷路を生成
        /// <summary>
        /// 迷路を生成
        /// </summary>
        /// <returns>迷路</returns>
        public int[,] Create()
        {
            // 外壁選別用匿名関数
            Func<int, int, bool> outerwall = (x, y) =>
                x == 0
                || y == 0
                || x == this._Width - 1
                || y == this._Height - 1;

            // 外壁は通路その他は壁にする
            for (int y = 0; y < this._Height; y++)
            {
                for (int x = 0; x < this._Width; x++)
                {
                    // 外壁選別
                    if (outerwall(x, y))
                        // 外壁は通路にする
                        this._MazeData[x, y] = PATH;
                    else
                        // その他は全て壁にする
                        this._MazeData[x, y] = WALL;
                }
            }

            // 穴掘り開始
            this.Dig(1, 1);

            // 外壁を壁に戻す
            for (int y = 0; y < this._Height; y++)
            {
                for (int x = 0; x < this._Width; x++)
                {
                    // 外壁選別
                    if (outerwall(x, y))
                        this._MazeData[x, y] = WALL;
                }
            }
            return this._MazeData;
        }
        #endregion 迷路を生成

        #endregion 公開メソッド

        #region メソッド

        #region 座標(x, y)から穴を掘る
        /// <summary>
        /// 座標(x, y)から穴を掘る
        /// </summary>
        /// <param name="x">開始座標x</param>
        /// <param name="y">開始座標y</param>
        private void Dig(
            int x,
            int y
            )
        {
            // 乱数生成
            var rnd = new Random(Environment.TickCount + 1);

            // 指定座標から掘れなくなるまで堀り続ける
            while (true)
            {
                // 掘り進めることができる方向のリストを作成
                var directions = new List<SBMaze.Direction>();
                if (this._MazeData[x, y - 1] == WALL && this._MazeData[x, y - 2] == WALL)
                    directions.Add(SBMaze.Direction.North);
                if (this._MazeData[x + 1, y] == WALL && this._MazeData[x + 2, y] == WALL)
                    directions.Add(SBMaze.Direction.East);
                if (this._MazeData[x, y + 1] == WALL && this._MazeData[x, y + 2] == WALL)
                    directions.Add(SBMaze.Direction.South);
                if (this._MazeData[x - 1, y] == WALL && this._MazeData[x - 2, y] == WALL)
                    directions.Add(SBMaze.Direction.West);

                // 掘り進められない場合、ループを抜ける
                if (directions.Count == 0) break;

                // 指定座標を通路とし穴掘り候補座標から削除
                this.SetPath(x, y);

                // 掘り進められる場合はランダムに方向を決めて掘り進める
                var dirIndex = rnd.Next(directions.Count);

                // 決まった方向に先2マス分を通路とする
                switch (directions[dirIndex])
                {
                    case SBMaze.Direction.North:
                        SetPath(x, --y);
                        SetPath(x, --y);
                        break;
                    case SBMaze.Direction.East:
                        SetPath(++x, y);
                        SetPath(++x, y);
                        break;
                    case SBMaze.Direction.South:
                        SetPath(x, ++y);
                        SetPath(x, ++y);
                        break;
                    case SBMaze.Direction.West:
                        SetPath(--x, y);
                        SetPath(--x, y);
                        break;
                }
            }

            // どこにも掘り進められない場合、穴掘り開始候補座標から掘りなおし
            var cell = this.GetStartCell();

            // 候補座標が存在しないとき、穴掘り完了
            if (cell != null)
            {
                this.Dig(cell.X, cell.Y);
            }
        }
        #endregion 座標(x, y)から穴を掘る

        #region 座標を通路とする(穴掘り開始座標候補の場合は保持)
        /// <summary>
        /// 座標を通路とする(穴掘り開始座標候補の場合は保持)
        /// </summary>
        /// <param name="x">座標x</param>
        /// <param name="y">座標y</param>
        private void SetPath(int x, int y)
        {
            this._MazeData[x, y] = PATH;
            if (x % 2 == 1 && y % 2 == 1)
            {
                // 穴掘り候補座標
                this._StartPoint.Add(new Cell() { X = x, Y = y });
            }
        }
        #endregion 座標を通路とする(穴掘り開始座標候補の場合は保持)

        #region 穴掘り開始位置をランダムに取得する
        /// <summary>
        /// 穴掘り開始位置をランダムに取得する
        /// 開始位置が存在しない場合はnullを返す
        /// </summary>
        /// <returns>結果</returns>
        private Cell GetStartCell()
        {
            // 開始位置が存在しない場合はnullを返す
            if (this._StartPoint.Count == 0) return null;

            // ランダムに開始座標を取得する
            var rnd = new Random(Environment.TickCount + 1);
            var index = rnd.Next(this._StartPoint.Count);
            var cell = this._StartPoint[index];
            this._StartPoint.RemoveAt(index);

            return cell;
        }
        #endregion 穴掘り開始位置をランダムに取得する

        #endregion メソッド

        #region セル情報クラス
        /// <summary>
        /// セル情報クラス
        /// </summary>
        private class Cell
        {
            /// <summary>
            /// X座標
            /// </summary>
            public int X { get; set; }
            /// <summary>
            /// Y座標
            /// </summary>
            public int Y { get; set; }
        }
        #endregion セル情報クラス
    }
    #endregion 迷路生成(穴掘り法)クラス
}
