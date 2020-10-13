using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用単位
    /// <summary>
    /// SmallBasic用単位
    /// プロパティは以下の命名規則によってグループ化されています
    /// 単位   : UnitXxxxx
    /// 長さ   : LengthXxxxx
    /// 面積   : AreaXxxxx
    /// 体積   : CubeXxxxx
    /// 時間   : TimeXxxxx
    /// 速さ   : SpeedXxxxx
    /// 加速度 : AccelerationXxxxx
    /// 質量   : MassXxxxx
    /// 力     : NewtonXxxxx
    /// 圧力   : PressureXxxxx
    /// 角度   : AngleXxxxx
    /// </summary>
    [SmallBasicType]
    public static class Unit
    {
        #region プロパティ

        #region 単位
        /// <summary>
        /// ヨタ(yotta) : Y : septillion : 一𥝱
        /// 10e24(10^24)
        /// </summary>
        public static Primitive UnitYotta { get; } = (double)10e24;
        /// <summary>
        /// ゼタ(zetta) : Z : sextillion : 十垓
        /// 10e21(10^21)
        /// </summary>
        public static Primitive UnitZetta { get; } = (double)10e21;
        /// <summary>
        /// エクサ(exa) : E : quintillion : 百京
        /// 10e18(10^18)
        /// </summary>
        public static Primitive UnitExa { get; } = (double)10e18;
        /// <summary>
        /// ペタ(peta) : P : quadrillion : 千兆
        /// 10e15(10^15)
        /// </summary>
        public static Primitive UnitPeta { get; } = (double)10e15;
        /// <summary>
        /// テラ(tera) : T : trillion : 一兆
        /// 10e12(10^12)
        /// </summary>
        public static Primitive UnitTera { get; } = (double)10e12;
        /// <summary>
        /// ギガ(giga) : G : billion : 十億
        /// 10e9(10^9)
        /// </summary>
        public static Primitive UnitGiga { get; } = (double)10e9;
        /// <summary>
        /// メガ(mega) : M : million : 百万
        /// 10e6(10^6)
        /// </summary>
        public static Primitive UnitMega { get; } = (double)10e6;
        /// <summary>
        /// キロ(kilo) : k : thousand : 千
        /// 10e3(10^3)
        /// </summary>
        public static Primitive UnitKilo { get; } = (double)10e3;
        /// <summary>
        /// ヘクト(hecto) : h : hundred : 百
        /// 10e2(10^2)
        /// </summary>
        public static Primitive UnitHecto { get; } = (double)10e2;
        /// <summary>
        /// デカ(deca) : da : ten : 十
        /// 10e1(10^1)
        /// </summary>
        public static Primitive UnitDeca { get; } = (double)10e1;
        /// <summary>
        /// デシ(deci) : d : tenth : 一分
        /// 10e-1(10^-1)
        /// </summary>
        public static Primitive UnitDeci { get; } = (double)10e-1;
        /// <summary>
        /// センチ(centi) : c : hundredth : 一厘
        /// 10e-2(10^-2)
        /// </summary>
        public static Primitive UnitCenti { get; } = (double)10e-2;
        /// <summary>
        /// ミリ(milli) : m : thousandth : 一毛
        /// 10e-3(10^-3)
        /// </summary>
        public static Primitive UnitMilli { get; } = (double)10e-3;
        /// <summary>
        /// マイクロ(micro) : μ : millionth : 一微
        /// 10e-6(10^-6))
        /// </summary>
        public static Primitive UnitMicro { get; } = (double)10e-6;
        /// <summary>
        /// ナノ(nano) : n : billionth : 一塵
        /// 10e-9(10^-9)
        /// </summary>
        public static Primitive UnitNano { get; } = (double)10e-9;
        /// <summary>
        /// ピコ(pico) : p : trillionth : 一漠
        /// 10e-12(10^-12)
        /// </summary>
        public static Primitive UnitPico { get; } = (double)10e-12;
        /// <summary>
        /// ファムト(famto) : f : quadrillionth : 一須臾
        /// 10e-15(10^-15)
        /// </summary>
        public static Primitive UnitFamto { get; } = (double)10e-15;
        /// <summary>
        /// アト(atto) : a : quintillionth : 一刹那
        /// 10e-18(10^-18)
        /// </summary>
        public static Primitive UnitAtto { get; } = (double)10e-18;
        /// <summary>
        /// ゼプト(zepto) : z : sextillionth : 一清浄
        /// 10e-21(10^-21)
        /// </summary>
        public static Primitive UnitZepto { get; } = (double)10e-21;
        /// <summary>
        /// ヨクト(yocto) : y : septillionth : 一涅槃寂静
        /// 10e-24(10^24)
        /// </summary>
        public static Primitive UnitYocto { get; } = (double)10e-24;
        #endregion 単位

        #region 長さ・距離
        /// <summary>
        /// ミル(mil) : mil
        /// 1 mil = 0.0254 m
        /// </summary>
        public static Primitive LengthMil { get; } = (double)0.0254;
        /// <summary>
        /// インチ(inch) : in
        /// 1 in = 25.4 m
        /// </summary>
        public static Primitive LengthIn { get; } = (double)25.4;
        /// <summary>
        /// フィート(feet) : ft
        /// 1 ft = 0.3048 m
        /// </summary>
        public static Primitive LengthFt { get; } = (double)0.3048;
        /// <summary>
        /// ヤード(yard) : yd
        /// 1 yd = 0.9144 m
        /// </summary>
        public static Primitive LengthYd { get; } = (double)0.9144;
        /// <summary>
        /// マイル(mile) : mi
        /// 1 mi = 1609.344 m
        /// </summary>
        public static Primitive LengthMi { get; } = (double)1609.344;
        /// <summary>
        /// 里(り) : 里
        /// 1 里(日本) ≒ 3927.2723927 m
        /// </summary>
        public static Primitive LengthRi { get; } = (double)3927.2723927;
        /// <summary>
        /// 町(ちょう) : 町
        /// 1 町(日本) ≒ 109.090109 m
        /// </summary>
        public static Primitive LengthChou { get; } = (double)109.090109;
        /// <summary>
        /// 間(けん) : 間
        /// 1 間(日本) ≒ 1.818181818 m
        /// </summary>
        public static Primitive LengthKen { get; } = (double)1.818181818;
        /// <summary>
        /// 丈(じょう) : 丈
        /// 1 丈(日本) ≒ 3.0303030303 m
        /// </summary>
        public static Primitive LengthJou { get; } = (double)3.0303030303;
        /// <summary>
        /// 尺(しゃく) : 尺
        /// 1 尺(日本) ≒ 0.303030303 m
        /// </summary>
        public static Primitive LengthShaku { get; } = (double)0.303030303;
        /// <summary>
        /// 天文単位(astronomical unit) : au
        /// 1 au = 149597870700 m
        /// </summary>
        public static Primitive LengthAstronomicalUnit { get; } = (double)149597870700;
        /// <summary>
        /// 光年(light-year) : ly
        /// 1 ly = 9460730472580800 m
        /// </summary>
        public static Primitive LengthLightYear { get; } = (double)9460730472580800;
        /// <summary>
        /// パーセク(parsec) : pc
        /// 1 pc ≒ 3.085677581e16 m
        /// </summary>
        public static Primitive LengthParsec { get; } = (double)3.085677581e16;
        /// <summary>
        /// 国際海里(international nautical mile) : M, nm
        /// 1 nm = 1852 m
        /// </summary>
        public static Primitive LengthInternationalNauticalMile { get; } = (double)1852;
        #endregion 長さ・距離

        #region 面積
        /// <summary>
        /// アール(are) : a
        /// 1 a = 100 m^2
        /// </summary>
        public static Primitive AreaAre { get; } = (double)100;
        /// <summary>
        /// ヘクタール(hectare) : ha
        /// 1 ha = 10000 m^2
        /// </summary>
        public static Primitive AreaHectare { get; } = (double)10000;
        /// <summary>
        /// 町(ちょう) : 町
        /// 1 町 ≒ 9917.354 m^2
        /// </summary>
        public static Primitive AreaCyou { get; } = (double)9917.3554;
        /// <summary>
        /// 反(たん) : 反, 段(たん)
        /// 1 反 ≒ 991.7354 m^2
        /// 元は米1石の収穫が上げられる田の面積として定義されたもの
        /// </summary>
        public static Primitive AreaTan { get; } = (double)991.73554;
        /// <summary>
        /// 畝(せ) : 畝
        /// 1 畝 ≒ 99.173554 m^2
        /// </summary>
        public static Primitive AreaSe { get; } = (double)99.173554;
        /// <summary>
        /// 坪(つぼ) : 坪
        /// 1 坪 = 3.305785124 m^2
        /// </summary>
        public static Primitive AreaTubo { get; } = (double)3.305785124;
        #endregion 面積

        #region 体積
        /// <summary>
        /// リットル(litre) : L, l
        /// 1 L = 10e-3(10^-3) ml
        /// </summary>
        public static Primitive CubeLitre { get; } = (double)10e-3;
        /// <summary>
        /// 勺(しゃく) : 勺
        /// 1 勺 ≒ 0.018039 L
        /// </summary>
        public static Primitive CubeShaku { get; } = (double)0.018039;
        /// <summary>
        /// 合(ごう) : 合
        /// 1 合 ≒ 0.18039 L
        /// </summary>
        public static Primitive CubeGou { get; } = (double)0.18039;
        /// <summary>
        /// 升(しょう) : 升
        /// 1 升 ≒ 1.8039 L
        /// </summary>
        public static Primitive CubeSyou { get; } = (double)1.8039;
        /// <summary>
        /// 斗(と) : 斗
        /// 1 斗 ≒ 18.039 L
        /// </summary>
        public static Primitive CubeTo { get; } = (double)18.039;
        /// <summary>
        /// 石(こく) : 石
        /// 1 石 ≒ 180.39 L
        /// 日本では、1食に米1合、1日3合がおおむね成人一人の消費量と
        /// されているので、1石は成人1人が1年間に消費する量にほぼ等しいと
        /// 見なされ、示準として換算されてきた（1000合/1日3合で333日分)
        /// </summary>
        public static Primitive CubeKoku { get; } = (double)180.39;
        /// <summary>
        /// 日本ガロン(gallon) : gal
        /// 1 gal = 3.785412 L
        /// </summary>
        public static Primitive CubeGallon { get; } = (double)3.785412;
        #endregion 体積

        #region 時間
        /// <summary>
        /// 日(day) : d
        /// 1 d = 86400 s
        /// </summary>
        public static Primitive TimeDay { get; } = (double)86400;
        /// <summary>
        /// 時間(hour) : h, hr, Hr
        /// 1 h = 3600 s
        /// </summary>
        public static Primitive TimeHour { get; } = (double)3600;
        /// <summary>
        /// 分(minute) : min
        /// 1 min = 60 s
        /// </summary>
        public static Primitive TimeMinute { get; } = (double)60;
        #endregion 時間

        #region 速度
        /// <summary>
        /// ノット(knot) : kn, kt
        /// 1 kn = 0.514444 m/s
        /// </summary>
        public static Primitive SpeedKnot { get; } = (double)0.514444;
        /// <summary>
        /// 光速(speed of light) : c, c0
        /// 1 c = 299792458 m/s
        /// </summary>
        public static Primitive SpeedOfLight { get; } = (double)299792458;
        #endregion 速度

        #region 加速度
        /// <summary>
        /// ガル(gal) : Gal
        /// 1 Gal = 0.01 m/s^2
        /// </summary>
        public static Primitive AccelerationGal { get; } = (double)0.01;
        /// <summary>
        /// 地球の重力加速度(gravity) : G
        /// 1 G = 9.80665 m/s^2(地球の標準重力加速度)
        /// </summary>
        public static Primitive AccelerationEarthGravitational { get; } = (double)9.80665;
        /// <summary>
        /// 月の重力加速度(gravity) : G
        /// 1 G = 1.622 m/s^2(月の重力加速度)
        /// </summary>
        public static Primitive AccelerationMoonGravitational { get; } = (double)1.622;
        #endregion 加速度

        #region 質量
        /// <summary>
        /// トン(metric ton) : t
        /// 日本を始めとするメートル法を広く使用している国では、
        /// 質量の単位として現在使われるトンとは、キログラム (kg) を
        /// 基準に定義されたメトリックトン (metric ton) のことを指す
        /// 1 t = 1000000 g
        /// </summary>
        public static Primitive MassTonne { get; } = (double)1000000;
        /// <summary>
        /// 常用(または国際)オンス(avoirdupois ounce) : oz, av
        /// 1 oz = 28.349523125 g
        /// </summary>
        public static Primitive MassOunce { get; } = (double)28.349523125;
        /// <summary>
        /// 常用(または国際)ポンド(avoirdupois pound) : lb
        /// 人間が1日に消費する大麦の重さに由来
        /// 1959年以降（ただし日本では1993年以降）は、
        /// 1 ポンド = 0.453 592 37 kg　である
        /// 1 lb = 453.59237 g
        /// </summary>
        public static Primitive MassPond { get; } = (double)453.59237;
        /// <summary>
        /// カラット(carat) : ct
        /// ダイヤモンドなどの宝石の質量を表す単位
        /// 1 ct = 0.2 g
        /// </summary>
        public static Primitive MassCarat { get; } = (double)0.2;
        /// <summary>
        /// 匁(もんめ) : 匁
        /// 銭貨の質量が由来
        /// 1 匁 = 3.75 g
        /// </summary>
        public static Primitive MassMonme { get; } = (double)3.75;
        /// <summary>
        /// 貫(かん) : 貫
        /// 銭貨1000枚の重さが由来
        /// 1 貫 = 3750 g
        /// </summary>
        public static Primitive MassKan { get; } = (double)3750;
        /// <summary>
        /// 斤(きん) : 斤
        /// 現在の日本では「斤」は、食パンの計量の単位としてのみ使われている
        /// これはパンが英斤を単位として売買された歴史に由来する
        /// ただし、1斤として売られたパンの質量は時代とともに少なくなった
        /// 現在、公正競争規約は、食パンの1斤=340グラム（以上）と定めている
        /// 1 斤 = 600 g(日本)
        /// </summary>
        public static Primitive MassKin { get; } = (double)600;
        #endregion 質量

        #region 力・重量
        /// <summary>
        /// 重量キログラム(kilogram-force, kilogram-weight) : kgf, kgw
        /// 現在の日本の中学教育では質量約100グラムの物体にはたらく重力を
        /// 1ニュートンと定義して教えている(1 N = 101.97 gf)
        /// 1 kgf = 9.80665 N
        /// </summary>
        public static Primitive NewtonKilogramWeight { get; } = (double)9.80665;
        /// <summary>
        /// ダイン(dyne) : dyn
        /// 1 g の質量に 1 cm/s^2 の加速度を生じさせる力
        /// 1 dyn = 1e-5 N
        /// </summary>
        public static Primitive NewtonDyne { get; } = (double)1e-5;
        #endregion 力・重量

        #region 圧力
        /// <summary>
        /// パスカル(pascal) : Pa
        /// 1 Pa = 1 N/m^2
        /// </summary>
        public static Primitive PressurePascal { get; } = (double)1;
        /// <summary>
        /// 標準気圧(standard atmosphere) : atm
        /// 1 atm = 101325 Pa
        /// </summary>
        public static Primitive PressureStandardAtmosphere { get; } = (double)101325;
        /// <summary>
        /// バール(bar) : bar
        /// 1 bar = 1e5 Pa
        /// </summary>
        public static Primitive PressureBar { get; } = (double)1e5;
        #endregion 圧力

        #endregion プロパティ
    }
    #endregion SmallBasic用単位
}
