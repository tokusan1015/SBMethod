using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用窓座標
    /// <summary>
    /// SmallBasic用窓座標
    /// </summary>
    [SmallBasicType]
    public static class SBWindowCoordinate
    {
        #region メンバ変数
        /// <summary>
        /// 窓座標
        /// </summary>
        private static WindowCoordinate _wc = null;
        /// <summary>
        /// 原点モード
        /// </summary>
        private static WindowCoordinate.OriginMode _om = WindowCoordinate.OriginMode.LeftTop;
        #endregion メンバ変数

        #region 公開メソッド
        /// <summary>
        /// 画面情報を設定します
        /// モード番号
        /// 0 : LeftTop      左上(標準)
        /// 1 : LeftButtom   左下
        /// 2 : RightTop     右上
        /// 3 : RightButtom  右下
        /// 4 : Center       中心
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="xMag">x値/ピクセル</param>
        /// <param name="yMag">x値/ピクセル</param>
        /// <param name="xOffset">x軸オフセット</param>
        /// <param name="yOffset">y軸オフセット</param>
        /// <param name="modeNo">モード番号</param>
        public static void SetWindow(
            Primitive width,
            Primitive height,
            Primitive xMag,
            Primitive yMag,
            Primitive xOffset,
            Primitive yOffset,
            Primitive modeNo
            )
        {
            // モード設定
            _om = GetIntToOriginMode(modeNo: modeNo);

            // 座標クラスを生成する
            _wc = new WindowCoordinate(
                width: (int)width,
                height: (int)height,
                widthMag: (double)xMag,
                heightMag: (double)yMag,
                widthOffsetPixel: (int)xOffset,
                heightOffsetPixel: (int)yOffset,
                widthOffsetValue: (double)0,
                heightOffsetValue: (double)0,
                om: _om
                );
        }
        /// <summary>
        /// モード用画面情報を設定します
        /// オフセットは0に固定されます
        /// モード番号
        /// 0 : LeftTop      左上(標準)
        /// 1 : LeftButtom   左下
        /// 2 : RightTop     右上
        /// 3 : RightButtom  右下
        /// 4 : Center       中心
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="xMag">x値/ピクセル</param>
        /// <param name="yMag">x値/ピクセル</param>
        /// <param name="modeNo">モード番号</param>
        public static void SetWindowMode(
            Primitive width,
            Primitive height,
            Primitive xMag,
            Primitive yMag,
            Primitive modeNo
            )
        {
            // モード設定
            _om = GetIntToOriginMode(modeNo: modeNo);

            // 座標クラスを生成する
            _wc = new WindowCoordinate(
                width: (int)width,
                height: (int)height,
                widthMag: (double)xMag,
                heightMag: (double)yMag,
                widthOffsetPixel: (int)0,
                heightOffsetPixel: (int)0,
                widthOffsetValue: (double)0,
                heightOffsetValue: (double)0,
                om: _om
                );
        }
        /// <summary>
        /// オフセット用画面情報を設定します
        /// 画面原点はLeftTopに固定されます
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="xMag">x値/ピクセル</param>
        /// <param name="yMag">x値/ピクセル</param>
        /// <param name="xOffset">x軸オフセット</param>
        /// <param name="yOffset">y軸オフセット</param>
        public static void SetWindowOffset(
            Primitive width,
            Primitive height,
            Primitive xMag,
            Primitive yMag,
            Primitive xOffset,
            Primitive yOffset
            )
        {
            // モード設定
            _om = WindowCoordinate.OriginMode.LeftTop;

            // 座標クラスを生成する
            _wc = new WindowCoordinate(
                width: (int)width,
                height: (int)height,
                widthMag: (double)xMag,
                heightMag: (double)yMag,
                widthOffsetPixel: (int)xOffset,
                heightOffsetPixel: (int)yOffset,
                widthOffsetValue: (double)0,
                heightOffsetValue: (double)0,
                om: _om
                );
        }
        /// <summary>
        /// グラフ用画面情報を設定します
        /// 画面サイズに合わせて自動的に描画位置を調整します
        /// 軸のオフセットは最小値, 最大値から自動計算されます
        /// 余白がほしい場合は、最小値, 最大値を広げてください
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="xMinValue">x軸最小値</param>
        /// <param name="xMaxValue">x軸最大値</param>
        /// <param name="yMinValue">y軸最小値</param>
        /// <param name="yMaxValue">y軸最大値</param>
        public static void SetWindowGraph(
            Primitive width,
            Primitive height,
            Primitive xMinValue,
            Primitive xMaxValue,
            Primitive yMinValue,
            Primitive yMaxValue
            )
        {
            // モード設定
            _om = WindowCoordinate.OriginMode.LeftButtom;

            // x軸の倍率およびオフセットを求める
            var xMagAndOffset = CalcMagAndOffset(
                size: (int)width,
                minValue: (double)xMinValue,
                maxValue: (double)xMaxValue
                );

            // y軸の倍率およびオフセットを求める
            var yMagAndOffset = CalcMagAndOffset(
                size: (int)height,
                minValue: (double)yMinValue,
                maxValue: (double)yMaxValue
                );

            // 座標クラスを生成する
            _wc = new WindowCoordinate(
                width: (int)width,
                height: (int)height,
                widthMag: xMagAndOffset.Item1,
                heightMag: yMagAndOffset.Item1,
                widthOffsetPixel: xMagAndOffset.Item2,
                heightOffsetPixel: yMagAndOffset.Item2,
                widthOffsetValue: xMinValue,
                heightOffsetValue: yMinValue,
                om: _om
                );
        }
        /// <summary>
        /// モード一覧を文字列配列で取得します
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetModeList()
        {
            // 結果を返す
            return typeof(WindowCoordinate.OriginMode).GetPrimitiveEnumNameList();
        }
        /// <summary>
        /// 原点モードをモード番号を数値で取得します
        /// 0 : LeftTop      左上(標準)
        /// 1 : LeftButtom   左下
        /// 2 : RightTop     右上
        /// 3 : RightButtom  右下
        /// 4 : Center       中心
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetMode()
        {
            // 初期化チェック
            CheckInit();

            // 結果を返す
            return (int)_om;
        }
        /// <summary>
        /// 原点モードを文字列で取得します
        /// 0 : LeftTop      左上(標準)
        /// 1 : LeftButtom   左下
        /// 2 : RightTop     右上
        /// 3 : RightButtom  右下
        /// 4 : Center       中心
        /// </summary>
        /// <returns>文字列</returns>
        public static Primitive GetModeText(
            )
        {
            // 初期化チェック
            CheckInit();

            // 結果を返す
            return _om.ToString();
        }
        /// <summary>
        /// X軸の反転情報
        /// </summary>
        public static Primitive GetX_Reverse()
        {
            return _wc.X.AxisReverse == Cordinate.Reverse.Reverse;
        }
        /// <summary>
        /// X軸のオフセットを数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetX_Offset()
        {
            return _wc.X.OffsetPixel;
        }
        /// <summary>
        /// X軸の倍率を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetX_Magnification()
        {
            return _wc.X.Magnification;
        }
        /// <summary>
        /// X軸最小ピクセル値を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetX_MinPixel()
        {
            return _wc.X.MinPixel;
        }
        /// <summary>
        /// X軸最大ピクセル値を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetX_MaxPixel()
        {
            return _wc.X.MaxPixel;
        }
        /// <summary>
        /// X軸最小値を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetX_MinValue()
        {
            return _wc.X.MinValue;
        }
        /// <summary>
        /// X軸最大値を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetX_MaxValue()
        {
            return _wc.X.MaxValue;
        }

        /// <summary>
        /// Y軸の反転情報をBOOLで取得します
        /// 反転の場合、true
        /// </summary>
        /// <returns>BOOL</returns>
        public static Primitive GetY_Reverse()
        {
            return _wc.Y.AxisReverse == Cordinate.Reverse.Reverse;
        }
        /// <summary>
        /// Y軸のオフセットを数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetY_Offset()
        {
            return _wc.Y.OffsetPixel;
        }
        /// <summary>
        /// Y軸の倍率を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetY_Magnification()
        {
            return _wc.Y.Magnification;
        }
        /// <summary>
        /// Y軸最小ピクセル値を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetY_MinPixel()
        {
            return _wc.Y.MinPixel;
        }
        /// <summary>
        /// Y軸最大ピクセル値を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetY_MaxPixel()
        {
            return _wc.Y.MaxPixel;
        }
        /// <summary>
        /// Y軸最小値を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetY_MinValue()
        {
            return _wc.Y.MinValue;
        }
        /// <summary>
        /// Y軸最大値を数値で取得します
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetY_MaxValue()
        {
            return _wc.Y.MaxValue;
        }

        /// <summary>
        /// 画面座標配列(x, y)を値配列(x, y)に変換し、
        /// 結果を数値配列で取得します
        /// d[n] : n = x or y, x = 1, y = 2
        /// </summary>
        /// <param name="screenXY">値配列(x, y)</param>
        /// <returns>数値配列</returns>
        public static Primitive ConvertXY_PixelToValue(
            this Primitive screenXY
            )
        {
            // 初期化チェック
            CheckInit();

            // 配列チェック
            var len = (int)screenXY.GetItemCount();
            if (len != 2)
                throw new ArgumentOutOfRangeException("次元数が2ではありません");

            // 戻り値生成
            var result = new Primitive();

            // x軸変換
            result[1] = _wc.X.PixelToValue(realPixel: screenXY[1]);

            // y軸変換
            result[2] = _wc.Y.PixelToValue(realPixel: screenXY[2]);

            // 結果を返す
            return result;
        }
        /// <summary>
        /// 値配列(x, y)を画面座標配列(x, y)に変換し、
        /// 結果を数値配列で取得します
        /// d[n] : n = x or y, x = 1, y = 2
        /// </summary>
        /// <param name="valueXY">値配列(x, y)</param>
        /// <returns>数値配列</returns>
        public static Primitive ConvertXY_ValueToPixel(
            this Primitive valueXY
            )
        {
            // 初期化チェック
            CheckInit();

            // 配列チェック
            var len = (int)valueXY.GetItemCount();
            if (len != 2)
                throw new ArgumentOutOfRangeException("次元数が2ではありません");

            // 戻り値生成
            var result = new Primitive();

            // x軸変換
            result[1] = _wc.X.ValueToPixel(value: valueXY[1]);

            // y軸変換
            result[2] = _wc.Y.ValueToPixel(value: valueXY[2]);

            // 結果を返す
            return result;
        }
        /// <summary>
        /// 値を画面座標xに変換し、
        /// 結果を数値で取得します
        /// </summary>
        /// <param name="x">x座標</param>
        /// <returns>数値</returns>
        public static Primitive ConvertX_PixelToValue(
            this Primitive x
            )
        {
            // 初期化チェック
            CheckInit();

            // 結果を返す
            return _wc.X.PixelToValue(
                realPixel: (int)x
                );
        }
        /// <summary>
        /// x座標を値に変換し、
        /// 結果を数値で取得します
        /// </summary>
        /// <param name="xValue">値</param>
        /// <returns>数値</returns>
        public static Primitive ConvertX_ValueToPixel(
            this Primitive xValue
            )
        {
            // 初期化チェック
            CheckInit();

            // 結果を返す
            return _wc.X.ValueToPixel(
                value: (double)xValue
                );
        }
        /// <summary>
        /// 値を画面座標yに変換し、
        /// 結果を数値で取得します
        /// </summary>
        /// <param name="y">y座標</param>
        /// <returns>数値</returns>
        public static Primitive ConvertY_PixelToValue(
            this Primitive y
            )
        {
            // 初期化チェック
            CheckInit();

            // 結果を返す
            return _wc.Y.PixelToValue(
                realPixel: (int)y
                );
        }
        /// <summary>
        /// y座標を値に変換し、
        /// 結果を数値で取得します
        /// </summary>
        /// <param name="yValue">値</param>
        /// <returns>数値</returns>
        public static Primitive ConvertY_ValueToPixel(
            this Primitive yValue
            )
        {
            // 初期化チェック
            CheckInit();

            // 結果を返す
            return _wc.Y.ValueToPixel(
                value: (double)yValue
                );
        }
        #endregion 公開メソッド

        #region メソッド
        /// <summary>
        /// 原点モード番号から原点モードを取得する
        /// </summary>
        /// <param name="modeNo">原点モード番号</param>
        /// <returns>結果</returns>
        private static WindowCoordinate.OriginMode GetIntToOriginMode(
            int modeNo
            )
        {
            var min = 0;
            var max = (int)WindowCoordinate.OriginMode.Unknown - 1;
            if (modeNo < min || modeNo > max)
                throw new ArgumentOutOfRangeException(
                    $"modeの値({modeNo})は{min}から{max}の範囲外です");

            return (WindowCoordinate.OriginMode)modeNo;
        }
        /// <summary>
        /// 最小値, 最大値から倍率, オフセット値を求める
        /// 結果は(倍率, オフセット)で返す
        /// </summary>
        /// <param name="size">サイズ(width, height)</param>
        /// <param name="minValue">最小値</param>
        /// <param name="maxValue">最大値</param>
        /// <returns>倍率, オフセット</returns>
        private static Tuple<double, int> CalcMagAndOffset(
            int size,
            double minValue,
            double maxValue
            )
        {
            // 入力チェック
            if (minValue >= maxValue)
                throw new ArgumentOutOfRangeException(
                    $"最小値({minValue})は最大値({maxValue})未満の数値"
                    );

            // 倍率を求める
            double mag = (maxValue - minValue) / (size - 1);

            // オフセットを求める
            int offset = (int)System.Math.Round(minValue * mag);

            // 結果を返す
            return new Tuple<double, int>(mag, offset);
        }
        /// <summary>
        /// 初期化チェック
        /// </summary>
        private static void CheckInit()
        {
            // nullの場合例外発生
            if (_wc == null)
                throw new ArithmeticException("初期化(SetWindow)が実行されていません");
        }
        #endregion メソッド
    }
    #endregion SmallBasic用窓座標

    #region 窓座標クラス
    /// <summary>
    /// 窓座標クラス
    /// 計算は通常に行い描画時に変換関数を使用する
    /// 値が画面の範囲外になった場合は、
    /// フラグの状態により処理を変える
    /// (1)-(5)の状態に応じて座標値を変換する
    /// (1)画面の中心を0とする
    /// (2)画面の左下を0とする
    /// (3)画面を左右反転する
    /// (4)画面を上下反転する
    /// (5)画面を上下左右反転する
    /// </summary>
    internal class WindowCoordinate
    {
        #region 列挙型

        #region 座標原点
        /// <summary>
        /// 座標原点モード
        /// </summary>
        public enum OriginMode : int
        {
            /// <summary>
            /// 左上原点(通常 : 一般座標系のY軸反転)
            /// 左上から右下に向かって座標(X,Y)の数値が増える
            /// </summary>
            LeftTop = 0,
            /// <summary>
            /// 左下原点(Y軸反転 : 一般座標系の通常)
            /// 左下から右上に向かって座標(X,Y)の数値が増える
            /// </summary>
            LeftButtom,
            /// <summary>
            /// 右上原点(X軸反転 : 一般座標系のXY軸反転)
            /// 右上から左下に向かって座標(X,Y)の数値が増える
            /// </summary>
            RightTop,
            /// <summary>
            /// 右下原点(XY軸反転 : 一般座標系のX軸反転)
            /// 右下から左上に向かって座標(X,Y)の数値が増える
            /// </summary>
            RightButtom,
            /// <summary>
            /// 中心原点(Y軸反転, XY軸オフセット : 一般座標系通常, XY軸オフセット)
            /// 中心から右上に向かって座標(X,Y)の数値が増える
            /// 中心から左下に向かって座標(X,Y)の数値が減る
            /// </summary>
            Center,
            /// <summary>
            /// 要素数
            /// </summary>
            Unknown
        }
        #endregion 座標原点

        #region 軸
        /// <summary>
        /// 軸
        /// </summary>
        public enum Axis : int
        {
            /// <summary>
            /// X軸
            /// </summary>
            X = 1,
            /// <summary>
            /// Y軸
            /// </summary>
            Y = 2,
        }
        #endregion 軸

        #endregion 列挙型

        #region メンバ変数
        /// <summary>
        /// 画面の幅(ピクセル)
        /// </summary>
        private int _Width = 0;
        /// <summary>
        /// 画面の高さ(ピクセル)
        /// </summary>
        private int _Height = 0;
        /// <summary>
        /// 原点位置(ピクセル)
        /// </summary>
        private OriginMode _OriginMode = OriginMode.LeftTop;
        /// <summary>
        /// X座標
        /// </summary>
        private Cordinate _X = null;
        /// <summary>
        /// Y座標
        /// </summary>
        private Cordinate _Y = null;
        #endregion メンバ変数

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// 原点モード       モード番号
        ///   左上(標準)      0
        ///   左下(グラフ)    1
        ///   右上            2
        ///   右下            3
        ///   中心(グラフ)    4
        /// </summary>
        /// <param name="width">画面の幅(ピクセル)</param>
        /// <param name="height">画面の高さ(ピクセル)</param>
        /// <param name="widthMag">幅の倍率(x値/ピクセル)</param>
        /// <param name="heightMag">高さの倍率(y値/ピクセル)</param>
        /// <param name="widthOffsetPixel">幅のオフセット(ピクセル)</param>
        /// <param name="heightOffsetPixel">高さのオフセット(ピクセル)</param>
        /// <param name="widthOffsetValue">幅のオフセット(値)</param>
        /// <param name="heightOffsetValue">高さのオフセット(値)</param>
        /// <param name="om">原点モード</param>
        public WindowCoordinate(
            int width,
            int height,
            double widthMag,
            double heightMag,
            int widthOffsetPixel,
            int heightOffsetPixel,
            double widthOffsetValue,
            double heightOffsetValue,
            OriginMode om
            )
        {
            // 初期値設定
            this._Width = width;
            this._Height = height;
            this._OriginMode = om;

            // x軸生成
            this._X = this.MakeCordinate(
                size: width,
                magnification: widthMag,
                offsetPixel: widthOffsetPixel,
                offsetValue: widthOffsetValue,
                om: om,
                func: this.GetReverseX
                );

            // y軸生成
            this._Y = this.MakeCordinate(
                size: height,
                magnification: heightMag,
                offsetPixel: heightOffsetPixel,
                offsetValue: heightOffsetValue,
                om: om,
                func: this.GetReverseY
                );
        }
        #endregion コンストラクタ

        #region 公開プロパティ
        /// <summary>
        /// 座標情報を取得する
        /// </summary>
        /// <param name="axis">軸</param>
        /// <returns>結果</returns>
        public Cordinate this [Axis axis]
        {
            get
            {
                switch (axis)
                {
                    case Axis.X:
                        return this._X;
                    case Axis.Y:
                        return this._Y;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        /// <summary>
        /// 幅
        /// </summary>
        public int Width
        {
            get { return this._Width; }
        }
        /// <summary>
        /// 高さ
        /// </summary>
        public int Height
        {
            get { return this._Height; }
        }
        /// <summary>
        /// モード
        /// </summary>
        public OriginMode Mode
        {
            get { return this._OriginMode; }
        }
        /// <summary>
        /// X座標
        /// </summary>
        public Cordinate X
        {
            get { return this._X; }
        }
        /// <summary>
        /// Y座標
        /// </summary>
        public Cordinate Y
        {
            get { return this._Y; }
        }
        #endregion 公開プロパティ

        #region メソッド
        /// <summary>
        /// Cordinateを生成する
        /// 画面サイズは width または height を設定する
        /// </summary>
        /// <param name="size">画面サイズ</param>
        /// <param name="magnification">倍率</param>
        /// <param name="offsetPixel">オフセット(ピクセル)</param>
        /// <param name="offsetValue">オフセット(値)</param>
        /// <param name="om">原点モード</param>
        /// <param name="func">反転情報取得関数</param>
        /// <returns>結果</returns>
        private Cordinate MakeCordinate(
            int size,
            double magnification,
            int offsetPixel,
            double offsetValue,
            OriginMode om,
            Func<OriginMode, Cordinate.Reverse> func
            )
        {
            // 軸オフセット値取得
            var newOffset = this.GetOffsetForOriginMode(
                screenSize: size,
                originMode: om
                ) + offsetPixel;

            // 軸生成
            return new Cordinate(
                size: size,
                magnefication: magnification,
                offsetPixel: newOffset,
                offsetValue: offsetValue,
                reverse: func(om)
                );
        }
        /// <summary>
        /// 原点モードからX軸の反転情報を取得する
        /// 注意：初期原点モードはLeftTopの為、
        /// 　　　値とは通常状態になる
        /// 　　　つまり、Rightの場合が対象となる
        /// </summary>
        /// <param name="om">原点モード</param>
        /// <returns>結果</returns>
        private Cordinate.Reverse GetReverseX(
            OriginMode om
            )
        {
            return om == OriginMode.RightTop
                || om == OriginMode.RightButtom
                ? Cordinate.Reverse.Reverse
                : Cordinate.Reverse.Normal;
        }
        /// <summary>
        /// 原点モードからY軸の反転情報を取得する
        /// 注意：初期原点モードはLeftTopの為、
        ///       値は通常にある
        /// 　　　つまり、center, bottomの場合が反転対象となる
        /// </summary>
        /// <param name="om">原点モード</param>
        /// <returns>結果</returns>
        private Cordinate.Reverse GetReverseY(
            OriginMode om
            )
        {
            return om == OriginMode.Center
                || om == OriginMode.LeftButtom
                || om == OriginMode.RightButtom
                ? Cordinate.Reverse.Reverse
                : Cordinate.Reverse.Normal;
        }
        /// <summary>
        /// 画面サイズから原点モードのオフセット値を取得する
        /// 画面サイズは width または height を設定する
        /// </summary>
        /// <param name="screenSize">画面サイズ(width or height)</param>
        /// <param name="originMode">原点モード</param>
        /// <returns>結果</returns>
        private int GetOffsetForOriginMode(
            int screenSize,
            OriginMode originMode
            )
        {
            // 原点モードが中央の場合
            if (originMode == OriginMode.Center)
                // オフセットを計算する
                return System.Math.Round(((double)screenSize - 1) / 2).intParse();
            else
                // その他はオフセットを0とする
                return 0;
        }
        #endregion メソッド
    }
    #endregion 窓座標クラス

    #region 座標クラス
    /// <summary>
    /// 座標クラス
    /// </summary>
    internal class Cordinate
    {
        #region 列挙体

        #region 反転
        /// <summary>
        /// 反転
        /// </summary>
        public enum Reverse
        {
            /// <summary>
            /// 通常(一般座標)
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 反転(画面座標としては標準)
            /// </summary>
            Reverse = 1,
        }
        #endregion 符号方向

        #endregion 列挙体

        #region メンバ変数
        /// <summary>
        /// 画面サイズ(ピクセル)
        /// </summary>
        private int _Size = 0;
        /// <summary>
        /// オフセット(ピクセル)
        /// </summary>
        private int _OffsetPixel = 0;
        /// <summary>
        /// オフセット(値)
        /// </summary>
        private double _OffsetValue = 0;
        /// <summary>
        /// 最小ピクセル値
        /// </summary>
        private int _MinPixel = 0;
        /// <summary>
        /// 最大ピクセル値
        /// </summary>
        private int _MaxPixel = 0;
        /// <summary>
        /// 倍率(値/ピクセル)
        /// </summary>
        private double _Magnification = 1;
        /// <summary>
        /// 軸反転
        /// </summary>
        private Reverse _AxisReverse = Reverse.Reverse;
        #endregion メンバ変数

        #region 公開プロパティ
        /// <summary>
        /// 画面サイズ(ピクセル)
        /// </summary>
        public int Size
        {
            get { return this._Size; }
        }
        /// <summary>
        /// オフセット(ピクセル)
        /// </summary>
        public int OffsetPixel
        {
            get { return this._OffsetPixel; }
        }
        /// <summary>
        /// オフセット(値)
        /// </summary>
        public double OffsetValue
        {
            get { return this._OffsetValue; }
        }
        /// <summary>
        /// 最小ピクセル値
        /// </summary>
        public int MinPixel
        {
            get
            {
                return this._MinPixel;
            }
        }
        /// <summary>
        /// 最大ピクセル値
        /// </summary>
        public int MaxPixel
        {
            get
            {
                return this._MaxPixel;
            }
        }
        /// <summary>
        /// 最小値
        /// </summary>
        public double MinValue
        {
            get
            {
                return this._MinPixel * this._Magnification + this._OffsetValue;
            }
        }
        /// <summary>
        /// 最大値
        /// </summary>
        public double MaxValue
        {
            get
            {
                return this._MaxPixel * this._Magnification + this._OffsetValue;
            }
        }
        /// <summary>
        /// 倍率(値/ピクセル)
        /// </summary>
        public double Magnification
        {
            get { return this._Magnification; }
        }
        /// <summary>
        /// 符号方向
        /// </summary>
        public Reverse AxisReverse
        {
            get { return this._AxisReverse; }
        }
        #endregion 公開プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="size">画面のピクセル数</param>
        /// <param name="magnefication">値変換用倍率</param>
        /// <param name="offsetPixel">オフセット(ピクセル)</param>
        /// <param name="offsetValue">オフセット(値)</param>
        /// <param name="reverse">反転</param>
        public Cordinate(
            int size,
            double magnefication,
            int offsetPixel,
            double offsetValue,
            Reverse reverse
            )
        {
            // 初期値設定
            this._Size = size;
            this._Magnification = magnefication;
            this._OffsetPixel = offsetPixel;
            this._OffsetValue = offsetValue;
            this._AxisReverse = reverse;

            // 最小実ピクセル値, 最大実ピクセル値を設定する
            this._MinPixel = 0;
            this._MaxPixel = size - 1;
        }
        #endregion コンストラクタ

        #region 公開メソッド
        /// <summary>
        /// 値から実ピクセル値を取得する
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>結果</returns>
        public int ValueToPixel(
            double value
            )
        {
            // 値の範囲チェック
            if (value < this.MinValue || value > this.MaxValue)
                throw new ArgumentOutOfRangeException(
                    $"値({value})は最小値({this.MinValue})と最大値({this.MaxValue})の範囲外です"
                    );

            // 値を仮想ピクセル値に変換
            int vp = System.Math.Round((value - this._OffsetValue) / this.Magnification).intParse();

            // 仮想ピクセル値を実ピクセル値に変換
            var p = this.ConvertVirtualPixelToRealPixel(
                virtualPixel: vp,
                offsetPixel: this._OffsetPixel
                );

            // 実ピクセル値を反転処理する
            if (this._AxisReverse == Reverse.Reverse)
                p = this._MaxPixel - p;

            // 結果を返す
            return p;
        }
        /// <summary>
        /// 実ピクセル値から値を取得する
        /// </summary>
        /// <param name="realPixel">実ピクセル</param>
        /// <returns>結果</returns>
        public double PixelToValue(
            int realPixel
            )
        {
            // 実ピクセル値の範囲チェック
            if (realPixel < this.MinPixel || realPixel > this.MaxPixel)
                throw new ArgumentOutOfRangeException(
                    $"実ピクセル値({realPixel})は最小値({this.MinPixel})と最大値({this.MaxPixel})の範囲外です"
                    );

            // 実ピクセル値を反転処理する
            int p = realPixel;
            if (this._AxisReverse == Reverse.Reverse)
                p = this._MaxPixel - p;

            // 実ピクセル値を仮想ピクセルに変換
            var vp = this.ConvertRealPixelToVirtualPixel(
                    realPixel: p,
                    offsetPixel: this._OffsetPixel
                    );

            // 仮想ピクセル値を値に変換して返す
            return vp * this._Magnification + _OffsetValue;
        }
        #endregion 公開メソッド

        #region 仮想ピクセル値から実ピクセル値に変換する
        /// <summary>
        /// 仮想ピクセル値から実ピクセル値に変換する
        /// オフセット
        /// </summary>
        /// <param name="virtualPixel">仮想ピクセル</param>
        /// <param name="offsetPixel">オフセット</param>
        /// <returns>結果</returns>
        private int ConvertVirtualPixelToRealPixel(
            int virtualPixel,
            int offsetPixel
            )
        {
            return virtualPixel + offsetPixel;
        }
        #endregion 仮想ピクセル値から実ピクセル値に変換する

        #region 実ピクセル値から仮想ピクセル値に変換する
        /// <summary>
        /// 実ピクセル値から仮想ピクセル値に変換する
        /// オフセット
        /// </summary>
        /// <param name="realPixel">ピクセル</param>
        /// <param name="offsetPixel">オフセット</param>
        /// <returns>結果</returns>
        private int ConvertRealPixelToVirtualPixel(
            int realPixel,
            int offsetPixel
            )
        {
            return realPixel - offsetPixel;
        }
        #endregion 実ピクセル値から仮想ピクセル値に変換する
    }
    #endregion 座標クラス
}
