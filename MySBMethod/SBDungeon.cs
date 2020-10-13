using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用ダンジョン生成
    /// <summary>
    /// SmallBasic用ダンジョン生成
    /// </summary>
    [SmallBasicType]
    public static class SBDungeon
    {
        #region 固定値
        /// <summary>
        /// ダンジョンデータ未生成メッセージ
        /// </summary>
        private const string _DUNGEON_NOT_CREATE = "ダンジョンが生成されていません";
        /// <summary>
        /// 最小部屋サイズエラー
        /// </summary>
        private const string _ROOM_SIZE_ERROR = "部屋サイズは 3以上にしなければなりません";
        /// <summary>
        /// 最小エリア数エラー
        /// </summary>
        private const string _AREA_COUNT_ERROR = "エリア数は、1以上にしなければなりません。";
        /// <summary>
        /// ダンジョンサイズエラー
        /// </summary>
        private const string _DUNGEON_SIZE_ERROR = "ダンジョンサイズは 5(幅) x 5(高さ) 以上にしなければなりません";
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
        /// ダンジョンデータ
        /// </summary>
        private static int[,] _DungeonData = null;
        #endregion メンバ変数

        #region メソッド
        /// <summary>
        /// ダンジョンを生成する
        /// </summary>
        /// <param name="roomSize">部屋サイズ</param>
        /// <param name="xAreaCounts">x軸エリア数</param>
        /// <param name="yAreaCounts">y軸エリア数</param>
        /// <param name="areaPercent">エリア確率</param>
        /// <param name="roomPercent">部屋確率</param>
        public static void Create(
            Primitive roomSize,
            Primitive xAreaCounts,
            Primitive yAreaCounts,
            Primitive areaPercent,
            Primitive roomPercent
            )
        {
            // 部屋サイズチェック
            if (roomSize < 3)
                throw new ArgumentException(_ROOM_SIZE_ERROR);

            // エリア数チェック
            if (xAreaCounts < 1 || yAreaCounts < 1)
                throw new ArgumentException(_AREA_COUNT_ERROR);

            // 初期化
            _DungeonData = null;

            // ダンジョンを生成する
            var md = new MakeDungeon(
                roomSize: roomSize,
                xAreaCounts: xAreaCounts,
                yAreaCounts: yAreaCounts,
                areaPercent: areaPercent,
                roomPercent: roomPercent
                );

            // ダンジョンデータを取得する
            _DungeonData = md.DungeonData;
        }
        /// <summary>
        /// ダンジョンデータを取得する
        /// </summary>
        /// <returns>ダンジョンデータ</returns>
        public static Primitive GetDungeonData()
        {
            return _ConvertPrimitiveDungeonData(_DungeonData);
        }
        /// <summary>
        /// ダンジョン幅を取得する
        /// </summary>
        /// <returns>ダンジョン幅</returns>
        public static Primitive GetDungeonDataWidth()
        {
            return _DungeonData.GetLength(0);
        }
        /// <summary>
        /// ダンジョン高を取得する
        /// </summary>
        /// <returns>ダンジョン高</returns>
        public static Primitive GetDungeonDataHeight()
        {
            return _DungeonData.GetLength(1);
        }
        /// <summary>
        /// ダンジョンチェックを行う
        /// 失敗した場合は例外となる
        /// </summary>
        private static void _CheckDungeon()
        {
            // 生成チェック
            if (_DungeonData == null)
                throw new ArgumentNullException(_DUNGEON_NOT_CREATE);

            // 迷路サイズチェック
            if (_DungeonData.GetLength((int)AxisXY.X) < Maze_Dig.MIN_SIZE
                || _DungeonData.GetLength((int)AxisXY.Y) < Maze_Dig.MIN_SIZE)
                throw new ArgumentException(_DUNGEON_SIZE_ERROR);
        }
        /// <summary>
        /// ダンジョンデータ[x, y]をPrimitive配列[x][y]に変換する
        /// </summary>
        /// <param name="dungeonData">ダンジョンデータ</param>
        /// <returns>結果</returns>
        private static Primitive _ConvertPrimitiveDungeonData(
            int[,] dungeonData
            )
        {
            // 戻り値生成
            var ret = new Primitive();

            for (int x = 0; x < dungeonData.GetLength((int)AxisXY.X); x++)
            {
                var retY = new Primitive();
                for (int y = 0; y < dungeonData.GetLength((int)AxisXY.Y); y++)
                {
                    retY[y + 1] = dungeonData[x, y];
                }
                ret[x + 1] = retY;
            }

            return ret;
        }
        #endregion メソッド
    }
    #endregion SmallBasic用ダンジョン生成

    #region ダンジョン生成クラス
    /// <summary>
    /// ダンジョン生成クラス
    /// (1)全体を構成する2次元配列を最小エリアサイズの倍数+1で生成する
    ///    全てを壁で埋める
    /// (2)エリアに分割する。
    /// (3)エリア結合する。
    ///    結果を配列に反映する。
    /// (4)分割したエリア内に部屋を生成する。
    /// (5)部屋から分割線まで通路を引く。
    /// (6)通路と通路を繋ぐ。
    /// (7)分割線を壁にする。
    /// </summary>
    internal class MakeDungeon
    {
        #region 固定値
        /// <summary>
        /// 壁
        /// </summary>
        public const int WALL = 0;
        /// <summary>
        /// 通路
        /// </summary>
        public const int PATH = 1;
        /// <summary>
        /// 部屋
        /// </summary>
        public const int ROOM = 2;
        /// <summary>
        /// 分割線
        /// </summary>
        public const int SPLIT = 3;
        /// <summary>
        /// 最小部屋サイズ
        /// </summary>
        public const int MIN_ROOM = 3;
        #endregion 固定値

        #region エリアデータズクラス
        /// <summary>
        /// エリアデータズクラス
        /// </summary>
        internal class cAreaDatas
        {
            #region エリアデータクラス
            /// <summary>
            /// エリアデータクラス
            /// </summary>
            internal class cAreaData
            {
                #region コンストラクタ
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="areaNo">エリア番号</param>
                /// <param name="x">x座標</param>
                /// <param name="y">y座標</param>
                /// <param name="width">幅</param>
                /// <param name="height">高さ</param>
                /// <param name="roomNo">部屋番号</param>
                public cAreaData(
                    int areaNo,
                    int x,
                    int y,
                    int width,
                    int height,
                    int roomNo
                    )
                {
                    this.AreaNo = areaNo;
                    this.RoomNo = roomNo;
                    this.StartX = x;
                    this.StartY = y;
                    this.Width = width;
                    this.Height = height;
                }
                #endregion コンストラクタ

                #region プロパティ
                /// <summary>
                /// エリア番号
                /// </summary>
                public int AreaNo { get; set; } = -1;
                /// <summary>
                /// 部屋番号
                /// </summary>
                public int RoomNo { get; set; } = -1;
                /// <summary>
                /// X軸分割線
                /// </summary>
                public bool SplitX { get; set; } = true;
                /// <summary>
                /// Y軸分割線
                /// </summary>
                public bool SplitY { get; set; } = true;
                /// <summary>
                /// Ｘ軸開始座標
                /// </summary>
                public int StartX { get; set; } = -1;
                /// <summary>
                /// X軸終了座標
                /// </summary>
                public int EndX
                {
                    get { return StartX + Width - 1; }
                }
                /// <summary>
                /// Ｙ軸開始座標
                /// </summary>
                public int StartY { get; set; } = -1;
                /// <summary>
                /// Y軸終了座標
                /// </summary>
                public int EndY
                {
                    get { return StartY + Height - 1; }
                }
                /// <summary>
                /// 幅
                /// </summary>
                public int Width { get; set; } = 1;
                /// <summary>
                /// 高さ
                /// </summary>
                public int Height { get; set; } = 1;
                #endregion プロパティ
            }
            #endregion エリアデータクラス

            #region メンバ変数
            /// <summary>
            /// X軸エリア数
            /// </summary>
            public int XAreaCounts { get; private set; } = 1;
            /// <summary>
            /// Y軸エリア数
            /// </summary>
            public int YAreaCounts { get; private set; } = 1;
            /// <summary>
            /// 部屋サイズ
            /// </summary>
            public int RoomSize { get; private set; } = 3;
            /// <summary>
            /// エリア確率
            /// </summary>
            public int AreaPercent { get; private set; } = 33;
            /// <summary>
            /// 部屋確率
            /// </summary>
            public int RoomPercent { get; private set; } = 33;
            #endregion メンバ変数

            #region プロパティ
            /// <summary>
            /// エリアデータリスト
            /// </summary>
            public List<cAreaData> AreaDataList { get; private set; } = null;
            /// <summary>
            /// 2次元配列のダンジョンデータ
            /// </summary>
            public int[,] DungeonData { get; private set; } = null;
            /// <summary>
            /// ダンジョン幅
            /// </summary>
            public int Width
            {
                get { return XAreaCounts * AreaSize + 1; }
            }
            /// <summary>
            /// ダンジョン高
            /// </summary>
            public int Height
            {
                get { return YAreaCounts * AreaSize + 1; }
            }
            /// <summary>
            /// エリアサイズ
            /// 部屋サイズ + 壁(2) + 区分線(1)
            /// ######
            /// #11111
            /// #10001
            /// #10001
            /// #10001
            /// #11111
            /// </summary>
            public int AreaSize
            {
                get { return RoomSize + 3; }
            }
            /// <summary>
            /// エリア確率結果取得
            /// </summary>
            public bool GetAreaPercentage
            {
                get { return Utility.GetParcentage(AreaPercent); }
            }
            /// <summary>
            /// 部屋確率結果取得
            /// </summary>
            public bool GetRoomPercentage
            {
                get { return Utility.GetParcentage(RoomPercent); }
            }
            #endregion プロパティ

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="roomSize">部屋サイズ</param>
            /// <param name="xAreaCounts">x軸エリア数</param>
            /// <param name="yAreaCounts">y軸エリア数</param>
            /// <param name="areaPercent">エリア拡大確率</param>
            /// <param name="roomPercent">部屋生存確率</param>
            internal cAreaDatas(
                int roomSize = 3,
                int xAreaCounts = 5,
                int yAreaCounts = 5,
                int areaPercent = 33,
                int roomPercent = 33
                )
            {
                // 部屋サイズ
                RoomSize = roomSize;
                // X軸エリア数
                XAreaCounts = xAreaCounts;
                // Y軸エリア数
                YAreaCounts = yAreaCounts;
                // エリア拡大確率
                AreaPercent = areaPercent;
                // 部屋数生存確率
                RoomPercent = roomPercent;
                // エリアデータリスト生成
                AreaDataList = new List<cAreaData>();
                // ダンジョンを生成する
                DungeonData = this.CreateDungeon();
            }
            #endregion コンストラクタ

            #region メソッド

            #region 指定エリア番号とルーム番号が一致しているエリアデータを取得する
            /// <summary>
            /// 指定エリア番号とルーム番号が一致しているエリアを取得する
            /// </summary>
            /// <param name="adList">エリアデータリスト</param>
            /// <param name="areaNo">エリア番号</param>
            /// <returns>エリアデータ</returns>
            private cAreaData GetBaseArea(
                IEnumerable<cAreaData> adList,
                int areaNo
                )
            {
                var al = adList.Where(
                    x => x.AreaNo == areaNo && x.RoomNo == areaNo
                    );
                if (al.Count() != 1) throw new ArgumentException();
                return al.Last();
            }
            #endregion 指定エリア番号とルーム番号が一致しているエリアを取得する

            #region 隣接するエリアを取得する
            /// <summary>
            /// 対象エリア番号に隣接するエリアを取得する
            /// </summary>
            /// <param name="adList">エリアデータリスト</param>
            /// <param name="areaNo">対象エリア番号</param>
            /// <returns>エリアリスト</returns>
            private IEnumerable<cAreaData> GetAdjacentAreaXY(
                IEnumerable<cAreaData> adList,
                int areaNo
                )
            {
                // エリアデータ番号のcAreaDataを取得する
                var list = GetAdjacentAreaX(adList: adList, areaNo: areaNo).ToList();
                list.AddRange(collection: GetAdjacentAreaY(adList: adList, areaNo: areaNo));

                return list;
            }
            /// <summary>
            /// 対象エリア番号に隣接するエリアを取得する
            /// </summary>
            /// <param name="adList">エリアデータリスト</param>
            /// <param name="areaNo">対象エリア番号</param>
            /// <returns>エリアリスト</returns>
            private IEnumerable<cAreaData> GetAdjacentAreaX(
                IEnumerable<cAreaData> adList,
                int areaNo
                )
            {
                // エリアデータ番号のcAreaDataを取得する
                cAreaData ad = adList.Where(x => x.AreaNo == areaNo).Last();

                // x,yを取得する
                int px = ad.StartX;
                int py = ad.StartY;

                return adList.Where(x =>
                    x.StartY == py && x.StartX == px + AreaSize
                    );
            }
            /// <summary>
            /// 対象エリア番号に隣接するエリアを取得する
            /// </summary>
            /// <param name="adList">エリアデータリスト</param>
            /// <param name="areaNo">対象エリア番号</param>
            /// <returns>エリアリスト</returns>
            private IEnumerable<cAreaData> GetAdjacentAreaY(
                IEnumerable<cAreaData> adList,
                int areaNo
                )
            {
                // エリアデータ番号のcAreaDataを取得する
                cAreaData ad = adList.Where(x => x.AreaNo == areaNo).Last();

                // x,yを取得する
                int px = ad.StartX;
                int py = ad.StartY;

                return adList.Where(
                    x => x.StartX == px && x.StartY == py + AreaSize
                    );
            }
            #endregion 隣接するエリアを取得する

            #region ダンジョンを生成する
            /// <summary>
            /// ダンジョンを生成する
            /// </summary>
            /// <returns>ダンジョン配列</returns>
            private int[,] CreateDungeon()
            {
                // (1)全体を構成する2次元配列生成する
                //    全てを壁で埋める
                DungeonData = CreateDungeonData();

                // (2)エリアデータクラスを使用し、
                //    エリアサイズで分割する。
                //    左上(1,1)のエリアはGroupNo = 1に設定される
                AreaDataList = this.CreateAreaDatas(adList: AreaDataList).ToList();

                // (3)エリアをグループ化する。
                AreaDataList = this.GroupAreas(adList: AreaDataList).ToList();

                // (4)エリアズデータからダンジョンを生成する。
                DungeonData = this.SetAreas(
                    dungeonData: DungeonData,
                    adList: AreaDataList
                    );

                // (5)部屋から分割線まで通路を引く。

                // (6)通路と通路を繋ぐ。

                // (7)分割線を壁にする。

                // 生成したダンジョンを返す
                return DungeonData;
            }
            #endregion ダンジョンを生成する

            #region ダンジョンデータを生成する
            /// <summary>
            /// ダンジョンデータを生成する
            /// </summary>
            private int[,] CreateDungeonData()
            {
                // 配列生成
                return FillWall(new int[Width, Height]);
            }
            #endregion ダンジョンデータを生成する

            #region 全てを壁で埋める
            /// <summary>
            /// 全てを壁で埋める
            /// </summary>
            /// <param name="data">ダンジョンデータ</param>
            /// <returns>ダンジョンデータ</returns>
            private int[,] FillWall(int[,] data)
            {
                // ダンジョンデータチェック
                if (data == null) throw new ArgumentNullException();

                // 全てを壁で埋める
                for (int x = 0; x < data.GetLength(0); x++)
                    for (int y = 0; y < data.GetLength(1); y++)
                        data[x, y] = WALL;

                // 戻り値
                return data;
            }
            #endregion 全てを壁で埋める

            #region エリアデータ生成
            /// <summary>
            /// エリアデータ生成
            /// x = 1, Y = 1 は、roomNo = 1 に設定する
            /// その他は、roomNo = -1 に設定する
            /// </summary>
            /// <param name="adList">エリアデータリスト</param>
            /// <returns>エリアデータリスト</returns>
            private IEnumerable<cAreaData> CreateAreaDatas(
                IEnumerable<cAreaData> adList
                )
            {
                // エリアデータ生成
                var adl = adList.ToList();
                int areaNo = 0;
                for (int y = 0; y < YAreaCounts; y++)
                    for (int x = 0; x < XAreaCounts; x++)
                        adl.Add(
                            new cAreaData(
                                areaNo: ++areaNo,
                                x: x * AreaSize + 1, 
                                y: y * AreaSize + 1, 
                                width: AreaSize, 
                                height: AreaSize,
                                roomNo: x == 0 && y == 0 ? 1 : -1
                                ));
                return adl;
            }
            #endregion エリアデータ生成

            #region エリアをグループ化する
            /// <summary>
            /// エリアをグループ化する
            /// </summary>
            /// <param name="adList">エリアデータリスト</param>
            /// <param name="update">trueの場合、部屋番号の上書きを許可</param>
            /// <returns>エリアデータリスト</returns>
            private IEnumerable<cAreaData> GroupAreas(
                IEnumerable<cAreaData> adList,
                bool update = false
                )
            {
                // リストに変換
                var adl = adList.ToList();

                // 全てのエリアで実行
                foreach (var ad in adl)
                {
                    // x軸でのグループ化
                    foreach (var x in GetAdjacentAreaX(adList: adl, areaNo: ad.AreaNo))
                    {
                        if (x.RoomNo < 0)
                            if (GetAreaPercentage)
                            {
                                x.RoomNo = ad.RoomNo < 0 ? x.AreaNo : ad.RoomNo;
                                x.SplitY = x.RoomNo == x.AreaNo;
                            }
                    }
                    // y軸でのグループ化
                    foreach (var y in GetAdjacentAreaY(adList: adl, areaNo: ad.AreaNo))
                    {
                        if (y.RoomNo < 0)
                            if (GetAreaPercentage)
                            {
                                y.RoomNo = ad.RoomNo < 0 ? y.AreaNo : ad.RoomNo;
                                y.SplitX = y.RoomNo == y.AreaNo;
                            }
                    }
                }

                return adl;
            }
            #endregion エリアをグループ化する

            #region 全てのルーム番号を指定番号に設定する
            /// <summary>
            /// 全てのルーム番号を指定番号に設定する
            /// </summary>
            /// <param name="areaNo">エリア番号</param>
            private void SetAllRoomNo(
                int areaNo = -1
                )
            {
                foreach (var ad in AreaDataList)
                        ad.RoomNo = areaNo;
            }
            #endregion 全てのルーム番号を指定番号に設定する

            #region チェックルーム番号
            /// <summary>
            /// チェックルーム番号
            /// </summary>
            /// <returns>全てのルーム番号が設定されている場合true</returns>
            private bool CheckRoomNo()
            {
                return AreaDataList.Where(
                    x => x.AreaNo < 1
                    ).Count() == 0;
            }
            #endregion チェックルーム番号

            #region エリアズデータを設定する
            /// <summary>
            /// エリアズデータを設定する
            /// </summary>
            /// <param name="dungeonData">ダンジョンデータ</param>
            /// <param name="adList">エリアデータリスト</param>
            /// <returns>ダンジョンデータ</returns>
            private int[,] SetAreas(
                int [,] dungeonData,
                IEnumerable<cAreaData> adList
                )
            {
                foreach (var ad in adList)
                {
                    dungeonData = this.SetArea(
                        dungeonData: dungeonData,
                        ad: ad
                        );
                }
                return dungeonData;
            }
            #endregion エリアズデータを設定する

            #region エリアデータを設定する
            /// <summary>
            /// エリアデータを設定する
            /// </summary>
            /// <param name="dungeonData"></param>
            /// <param name="ad">エリアデータ</param>
            /// <returns>ダンジョンデータ</returns>
            private int[,] SetArea(
                int [,] dungeonData,
                cAreaData ad
                )
            {
                // 部屋設定
                for (int y = ad.StartY; y <= ad.EndY; y++)
                    for (int x = ad.StartX; x <= ad.EndX; x++)
                    {
                        // X軸分割線、その他は部屋
                        if (x == ad.StartX)
                            dungeonData[x, y] = ad.SplitY ? SPLIT : ROOM;
                        else if (y == ad.StartY)
                            dungeonData[x, y] = ad.SplitX ? SPLIT : ROOM;
                        else
                            // 部屋
                            dungeonData[x, y] = ROOM;
                    }
                return dungeonData;
            }
            #endregion エリアデータを設定する

            #endregion メソッド
        }
        #endregion エリアデータズクラス

        #region メンバ変数
        #endregion メンバ変数

        #region プロパティ
        /// <summary>
        /// ダンジョンデータ
        /// </summary>
        public int[,] DungeonData
        {
            get { return AreaDatas.DungeonData; }
        }
        /// <summary>
        /// エリアデータズ
        /// </summary>
        public cAreaDatas AreaDatas { get; private set; } = null;
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="roomSize">部屋サイズ</param>
        /// <param name="xAreaCounts">x軸エリア数</param>
        /// <param name="yAreaCounts">y軸エリア数</param>
        /// <param name="areaPercent">エリア拡大確率</param>
        /// <param name="roomPercent">部屋生存確率</param>
        public MakeDungeon(
            int roomSize = 3,
            int xAreaCounts = 5,
            int yAreaCounts = 5,
            int areaPercent = 33,
            int roomPercent = 33
            )
        {
            // エリアデータズ初期化
            AreaDatas = new cAreaDatas(
                roomSize: roomSize,
                xAreaCounts: xAreaCounts,
                yAreaCounts: yAreaCounts,
                areaPercent: areaPercent,
                roomPercent: roomPercent
                );
        }
        #endregion 初期化
    }
    #endregion ダンジョン生成クラス
}
