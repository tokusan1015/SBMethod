using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用DateTime
    /// <summary>
    /// SmallBasic用DateTime
    /// </summary>
    [SmallBasicType]
    public static class SBDateTime
    {
        #region 列挙型

        #region カルチャータイプ
        /// <summary>
        /// カルチャータイプ
        /// </summary>
        private enum enmCultureType : int
        {
            /// <summary>
            /// 標準
            /// </summary>
            Default = 0,
            /// <summary>
            /// 日本語
            /// </summary>
            Japanese,
        }
        #endregion カルチャー情報

        #endregion 列挙型

        #region 固定値
        /// <summary>
        /// 修正ユリウス日運用開始日
        /// </summary>
        private static readonly DateTime _START_MODIFIED_JULIAN =
            new DateTime(
                year: 1858,
                month: 11,
                day: 17
                );
        /// <summary>
        /// 十干
        /// </summary>
        private static readonly string[] _KAN = 
            new string[] { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸",  };
        /// <summary>
        /// 十二支
        /// </summary>
        private static readonly string[] _SHI = 
            new string[] { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥", };
        /// <summary>
        /// 六曜
        /// </summary>
        private static readonly string[] _JCSL = 
            new string[] { "大安", "赤口", "先勝", "友引", "先負", "仏滅", };
        /// <summary>
        /// 陰暦名
        /// </summary>
        private static readonly string[] _LCMN =
            { "睦月", "如月", "弥生", "卯月", "皐月", "水無月", "文月", "葉月", "長月", "神無月", "霜月", "師走", };
        /// <summary>
        /// 六十干支
        /// </summary>
        private static List<string> _KAN_SHI = new List<string>();
        /// <summary>
        /// 月齢の呼称
        /// </summary>
        private static readonly string[] _MOON =
        {
            // http://inmymemory.hatenablog.com/entry/20131211/moon_name
            "新月(しんげつ),朔(さく),朔日(さくじつ),月発ち(つきたち),初三の月(しょさんのつき),上り月(のぼりつき)",
            "繊月(せんげつ),二日月(ふつかづき),既朔(きさく),夕月(ゆうづき),黄昏月(たそがれづき),初三の月(しょさんのつき),上り月(のぼりつき)",
            "三日月(みかづき),若月(わかづき),眉月(まゆづき),蛾眉(がび),夕月(ゆうづき),黄昏月(たそがれづき),初三の月(しょさんのつき),上り月(のぼりつき)",
            "夕月(ゆうづき),黄昏月(たそがれづき),上り月(のぼりつき)",
            "夕月(ゆうづき),黄昏月(たそがれづき),上り月(のぼりつき)",
            "上り月(のぼりつき)",
            "上弦の月(じょうげんのつき),弓張り月(ゆみはりづき),半月(半月),上り月(のぼりつき)",
            "上弦の月(じょうげんのつき),弓張り月(ゆみはりづき),半月(半月),上り月(のぼりつき)",
            "上り月(のぼりつき)",
            "十日夜(とおかんや),上り月(のぼりつき)",
            "十日余の月(とおかあまりのつき),上り月(のぼりつき)",
            "上り月(のぼりつき)",
            "十三夜月(じゅうさんやづき)",
            "小望月(こもちづき),十四日月(じゅうよっかづき),待宵の月(まつよいのつき),幾望(きぼう)",
            "満月(まんげつ),十五夜(じゅうごや),望月(もちづき),三五の月(さんごのつき),仲秋の名月(ちゅうしゅうのめいげつ),芋名月(いもめいげつ),望(ぼう)",
            "十六夜(いざよい),既望(希望),有明の月(ありあけのつき)",
            "立待月(たちまちづき),有明の月(ありあけのつき)",
            "居待月(いまちづき),有明の月(ありあけのつき),降り月(くだりつき)",
            "寝待月(ねまちづき),臥待月(ふしまちづき),有明の月(ありあけのつき),降り月(くだりつき)",
            "更待月(ふけまちづき),亥中の月(いなかのつき),有明の月(ありあけのつき),降り月(くだりつき)",
            "有明の月(ありあけのつき),有明の月(ありあけのつき),降り月(くだりつき)",
            "有明の月(ありあけのつき),有明の月(ありあけのつき),降り月(くだりつき)",
            "下弦の月(かげんのつき),二十三夜月(二十三夜月),有明の月(ありあけのつき)",
            "下弦の月(かげんのつき),二十三夜月(二十三夜月),有明の月(ありあけのつき)",
            "有明の月(ありあけのつき)",
            "有明月(ありあけづき),暁月(ぎょうげつ)",
            "",
            "",
            "三十日月(みそかづき),晦日月(みそかづき),晦(つごもり)",
            "三十日月(みそかづき),晦日月(みそかづき),晦(つごもり),月隠り(つきこもり)"
        };
        /// <summary>
        /// 年インデックス
        /// </summary>
        private static readonly int _YEAR = 1;
        /// <summary>
        /// 月インデックス
        /// </summary>
        private static readonly int _MONTH = 2;
        /// <summary>
        /// 日インデックス
        /// </summary>
        private static readonly int _DAY = 3;
        /// <summary>
        /// 時インデックス
        /// </summary>
        private static readonly int _HOUR = 4;
        /// <summary>
        /// 分インデックス
        /// </summary>
        private static readonly int _MINUTE = 5;
        /// <summary>
        /// 秒インデックス
        /// </summary>
        private static readonly int _SECOND = 6;
        #endregion 固定値

        #region メンバ変数
        /// <summary>
        /// 標準フォーマット
        /// </summary>
        private static readonly string _FormatDefault = "yyyy年MM月dd日 HH:mm:ss";
        /// <summary>
        /// 和暦フォーマット
        /// </summary>
        private static readonly string _FormatJapanese = "ggyy年MM月dd日 HH:mm:ss";
        /// <summary>
        /// 休日オブジェクト
        /// </summary>
        private static ClassHoliday _Holiday = new ClassHoliday();
        /// <summary>
        /// 和暦オブジェクト
        /// </summary>
        private static JapaneseCalendar _JapaneseCalender = null;
        /// <summary>
        /// カルチャー情報リスト
        /// </summary>
        private static List<Culture> _CultureInfoList = new List<Culture>();
        /// <summary>
        /// カルチャー情報タイプ
        /// </summary>
        private static enmCultureType _CultureInfoType = enmCultureType.Default;
        /// <summary>
        /// 元号情報オブジェクト
        /// </summary>
        private static EraInfomation _EraInfomation = new EraInfomation();
        #endregion メンバ変数

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static SBDateTime()
        {
            // 六十干支を生成する
            // 初期化
            int tcs = -1, cz = -1;

            // 六十干支回回す
            for (int i = 0; i < 60; i++)
            {
                // 個々+1して指定数値で割り、あまりを求める
                tcs = ++tcs % 10;
                cz = ++cz % 12;

                // 十干の陰陽と干支の陰陽が同じ場合のみ追加する
                if (tcs % 2 == cz % 2)
                {
                    _KAN_SHI.Add(
                        _KAN[tcs] + _SHI[cz]
                        );
                }
            }

            // 和暦インスタンス生成
            _JapaneseCalender = new JapaneseCalendar();

            // 標準カルチャー情報を設定する
            _CultureInfoList.Add(
                new Culture(
                    ci: default(CultureInfo),
                    format: _FormatDefault
                    ));

            // 和暦カルチャー情報生成
            var jpn_ci = new CultureInfo("ja-JP", false);
            // 和暦フォーマット設定
            jpn_ci.DateTimeFormat.Calendar = _JapaneseCalender;
            // 和暦カルチャー情報を設定する
            _CultureInfoList.Add(
                new Culture(
                    ci: jpn_ci,
                    format: _FormatJapanese
                    ));
        }
        #endregion コンストラクタ

        #region 公開プロパティ
        /// <summary>
        /// 選択されているカルチャーのフォーマット
        /// </summary>
        public static Primitive Format
        {
            get { return _GetCultureFormat(); }
            set { _GetCulture().Format = value.ToString(); }
        }
        /// <summary>
        /// カルチャータイプ
        /// 存在しないカルチャータイプの場合は標準が選択される
        /// 標準 : "Default"
        /// 和暦 : "Japanese"
        /// </summary>
        public static Primitive CultureInfoType
        {
            get { return _CultureInfoType.ToString(); }
            set
            {
                // カルチャー情報タイプを設定する
                foreach (var e in typeof(enmCultureType).GetEnumValues())
                {
                    // 大文字比較で一致した場合、そのタイプを設定する
                    if (e.ToString().ToUpper() == value.ToString().ToUpper())
                    {
                        _CultureInfoType = (enmCultureType)e;
                        return;
                    }
                }
                // 一致しなかった場合、標準タイプとする
                _CultureInfoType = enmCultureType.Default;
            }
        }
        /// <summary>
        /// 年インデックス
        /// </summary>
        public static Primitive YearIndex
        {
            get { return _YEAR; }
        }
        /// <summary>
        /// 月インデックス
        /// </summary>
        public static Primitive MonthIndex
        {
            get { return _MONTH; }
        }
        /// <summary>
        /// 日インデックス
        /// </summary>
        public static Primitive DayIndex
        {
            get { return _DAY; }
        }
        /// <summary>
        /// 時インデックス
        /// </summary>
        public static Primitive HourIndex
        {
            get { return _HOUR; }
        }
        /// <summary>
        /// 分インデックス
        /// </summary>
        public static Primitive MinuteIndex
        {
            get { return _MINUTE; }
        }
        /// <summary>
        /// 秒インデックス
        /// </summary>
        public static Primitive SecondIndex
        {
            get { return _SECOND; }
        }
        #endregion 公開プロパティ

        #region 公開メソッド
        /// <summary>
        /// KanShiリストを文字列配列で取得します
        /// *** 非公開にすること ***
        /// </summary>
        /// <returns>文字列配列</returns>
        private static Primitive Get_KAN_SHIList(
            )
        {
            // 戻り値初期化
            var result = new Primitive();

            // カルチャータイプで回す
            int index = 0;
            foreach (var s in _KAN_SHI)
            {
                result[++index] = s;
            }

            // 結果を返す
            return result;
        }
        /// <summary>
        /// カルチャータイプ一覧を文字列配列で取得します
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetCultureTypeList(
            )
        {
            // 戻り値初期化
            var result = new Primitive();

            // カルチャータイプで回す
            int index = 0;
            foreach (var s in typeof(enmCultureType).GetEnumNames())
            {
                result[++index] = s;
            }

            // 結果を返す
            return result;
        }

        /// <summary>
        /// 現在日時を日時文字列で取得します
        /// </summary>
        /// <returns>日時文字列</returns>
        public static Primitive Now()
        {
            return DateTime.Now
                .ToString(
                    format: _GetCultureFormat(), 
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 年月日を日時文字列で取得します
        /// 時分秒は0で設定されます
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns></returns>
        public static Primitive SetDate(
            this Primitive year,
            Primitive month,
            Primitive day
            )
        {
            // 文字列を返す
            return new DateTime(
                year: (int)year,
                month: (int)month,
                day: (int)day,
                hour: (int)0,
                minute: (int)0,
                second: (int)0
                ).ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 配列に設定された年月日を日時文字列で取得します
        /// 時分秒は0で設定されます
        /// 年月日配列(1-6)は以下の様に数値設定している必要があります
        /// 配列)
        /// dts[1] : 年
        /// dts[2] : 月
        /// dts[3] : 日
        /// </summary>
        /// <param name="dts">年月日配列</param>
        /// <returns>日時文字列</returns>
        public static Primitive SetDate2(
            this Primitive dts
            )
        {
            // 配列数が6以外は例外
            if (dts.GetItemCount() != 6)
                throw new ArgumentOutOfRangeException();

            // 数値を日時文字列にして返す
           　return SetDateTime(
                year: (int)dts[_YEAR],
                month: (int)dts[_MONTH],
                day: (int)dts[_DAY],
                hour: (int)0,
                minute: (int)0,
                second: (int)0
                );
        }
        /// <summary>
        /// 年月日時分秒を日時文字列で取得します
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">時</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <returns>日時文字列</returns>
        public static Primitive SetDateTime(
            this Primitive year,
            Primitive month,
            Primitive day,
            Primitive hour,
            Primitive minute,
            Primitive second
            )
        {
            // 文字列を返す
            return new DateTime(
                year: (int)year,
                month: (int)month,
                day: (int)day,
                hour: (int)hour,
                minute: (int)minute,
                second: (int)second
                ).ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 配列に設定された年月日時分秒を日時文字列で取得します
        /// 日時配列(1-6)は以下の様に数値設定している必要があります
        /// 日時配列)
        /// dts[1] : 年
        /// dts[2] : 月
        /// dts[3] : 日
        /// dts[4] : 時
        /// dts[5] : 分
        /// dts[6] : 秒
        /// </summary>
        /// <param name="dts">年月日時分秒配列</param>
        /// <returns>日時文字列</returns>
        public static Primitive SetDateTime2(
            this Primitive dts
            )
        {
            // 配列数が6以外は例外
            if (dts.GetItemCount() != 6)
                throw new ArgumentOutOfRangeException();

            // 数値を日時文字列にして返す
           　return SetDateTime(
                year: dts[_YEAR],
                month: dts[_MONTH],
                day: dts[_DAY],
                hour: dts[_HOUR],
                minute: dts[_MINUTE],
                second: dts[_SECOND]
                );
        }
        /// <summary>
        /// 日時文字列を数値配列で取得します
        /// 日時配列(1-6)は以下の様に数値設定されます
        /// 日時配列)
        /// 結果[1] : 年
        /// 結果[2] : 月
        /// 結果[3] : 日
        /// 結果[4] : 時
        /// 結果[5] : 分
        /// 結果[6] : 秒
        /// </summary>
        /// <param name="dts">年月日時分秒配列</param>
        /// <returns>数値配列</returns>
        public static Primitive GetDateTime(
            this Primitive dts
            )
        {
            // 戻り値初期化
            var result = new Primitive();

            // DateItmeに変換
            var dt = _ConvertDateTime(dts: dts);

            // 配列に設定
            result[_YEAR] = dt.Year;
            result[_MONTH] = dt.Month;
            result[_DAY] = dt.Day;
            result[_HOUR] = dt.Hour;
            result[_MINUTE] = dt.Minute;
            result[_SECOND] = dt.Second;

            // 結果を返す
            return result;
        }
        /// <summary>
        /// 日時文字列の曜日を数値で取得します
        /// 日 = 0
        /// 月 = 1
        /// 火 = 2
        /// 水 = 3
        /// 木 = 4
        /// 金 = 5
        /// 土 = 6
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetDayOfWeek(
            this Primitive dts
            )
        {
            // DateTimeに変換
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return (int)dt.DayOfWeek;
        }
        /// <summary>
        /// 日時文字列の年を数値で取得します
        /// 年が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetYear(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.Year;
        }
        /// <summary>
        /// 日時文字列の月を数値で取得します
        /// 月が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetMonth(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.Month;
        }
        /// <summary>
        /// 日時文字列の日を数値で取得します
        /// 日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetDay(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.Day;
        }
        /// <summary>
        /// 日時文字列の時間を数値で取得します
        /// 時が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetHour(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.Hour;
        }
        /// <summary>
        /// 日時文字列の分を数値で取得します
        /// 分が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetMinute(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.Minute;
        }
        /// <summary>
        /// 日時文字列の秒を数値で取得します
        /// 秒が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetSecond(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.Second;
        }
        /// <summary>
        /// 日時文字列の年に値を加算して日時文字列取得します
        /// 年が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <param name="value">値</param>
        /// <returns>日時文字列</returns>
        public static Primitive AddYears(
            this Primitive dts,
            Primitive value
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.AddYears(value)
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の月に値を加算して日時文字列を取得します
        /// 年月が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <param name="value">値</param>
        /// <returns>日時文字列</returns>
        public static Primitive AddMonths(
            this Primitive dts,
            Primitive value
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.AddMonths(value)
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の日に値を加算して日時文字列を取得します
        /// 年月日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <param name="value">値</param>
        /// <returns>日時文字列</returns>
        public static Primitive AddDays(
            this Primitive dts,
            Primitive value
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.AddDays(value)
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の時に値を加算して日時文字列を取得します
        /// 年月日時が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <param name="value">値</param>
        /// <returns>日時文字列</returns>
        public static Primitive AddHours(
            this Primitive dts,
            Primitive value
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.AddHours(value)
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の分に値を加算して日時文字列を取得します
        /// 年月日時分が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <param name="value">値</param>
        /// <returns>日時文字列</returns>
        public static Primitive AddMinutes(
            this Primitive dts,
            Primitive value
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.AddMinutes(value)
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の秒に値を加算して日時文字列を取得します
        /// 年月日時分秒が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <param name="value">値</param>
        /// <returns>日時文字列</returns>
        public static Primitive AddSeconds(
            this Primitive dts,
            Primitive value
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt.AddSeconds(value)
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列1から日時文字列2までの日数を数値で取得します
        /// 年月日時分秒が使用されます
        /// </summary>
        /// <param name="dts1">日時文字列1</param>
        /// <param name="dts2">日時文字列2</param>
        /// <returns>日数値</returns>
        public static Primitive SpanDays(
            this Primitive dts1,
            Primitive dts2
            )
        {
            // DateTimeに変換する
            var dt1 = _ConvertDateTime(dts: dts1);
            var dt2 = _ConvertDateTime(dts: dts2);

            // 結果を返す
            TimeSpan ts = dt2 - dt1;
            return ts.Days;
        }
        /// <summary>
        /// 日時文字列1から日時文字列2までの時間数を数値で取得します
        /// 年月日時分秒が使用されます
        /// </summary>
        /// <param name="dts1">日時文字列1</param>
        /// <param name="dts2">日時文字列2</param>
        /// <returns>時間数値</returns>
        public static Primitive SpanHours(
            this Primitive dts1,
            Primitive dts2
            )
        {
            // DateTimeに変換する
            var dt1 = _ConvertDateTime(dts: dts1);
            var dt2 = _ConvertDateTime(dts: dts2);

            // 結果を返す
            TimeSpan ts = dt2 - dt1;
            return ts.Hours;
        }
        /// <summary>
        /// 日時文字列1から日時文字列2までの分数を数値で取得します
        /// 年月日時分秒が使用されます
        /// </summary>
        /// <param name="dts1">日時文字列1</param>
        /// <param name="dts2">日時文字列2</param>
        /// <returns>分数値</returns>
        public static Primitive SpanMinutes(
            this Primitive dts1,
            Primitive dts2
            )
        {
            // DateTimeに変換する
            var dt1 = _ConvertDateTime(dts: dts1);
            var dt2 = _ConvertDateTime(dts: dts2);

            // 結果を返す
            TimeSpan ts = dt2 - dt1;
            return ts.Minutes;
        }
        /// <summary>
        /// 日時文字列1から日時文字列2までの秒数を数値で取得します
        /// 年月日時分秒が使用されます
        /// </summary>
        /// <param name="dts1">日時文字列1</param>
        /// <param name="dts2">日時文字列2</param>
        /// <returns>秒数値</returns>
        public static Primitive SpanSeconds(
            this Primitive dts1,
            Primitive dts2
            )
        {
            // DateTimeに変換する
            var dt1 = _ConvertDateTime(dts: dts1);
            var dt2 = _ConvertDateTime(dts: dts2);

            // 結果を返す
            TimeSpan ts = dt2 - dt1;
            return ts.Seconds;
        }
        /// <summary>
        /// 日時文字列の年月に対して月の日数を数値で取得します
        /// 年月が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日数値</returns>
        public static Primitive GetDaysInMonth(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetDaysInMonth();
        }
        /// <summary>
        /// 日時文字列の年月日に対して前日を日時文字列で取得します
        /// 年月日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetPrevDay(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetPrevDay()
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月日に対して明日を日時文字列で取得します
        /// 年月日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetNextDay(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetNextDay()
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月日に対して月初日を日時文字列で取得します
        /// 年月日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetFirstDayOfMonth(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetFirstDayOfMonth()
                .ToString(
                    format: _GetCultureFormat(), 
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月日に対して月末を日時文字列で取得します
        /// 年月日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetLastDayOfMonth(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetLastDayOfMonth()
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月日に対して先月初日を日時文字列で取得します
        /// 年月日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetFirstDayOfPrevMonth(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetFirstDayOfPrevMonth()
                .ToString(
                    format: _GetCultureFormat(), 
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月日に対して先月末日を日時文字列で取得します
        /// 年月日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetLastDayOfPrevMonth(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetLastDayOfPrevMonth()
                .ToString(
                    format: _GetCultureFormat(), 
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月日に対して来月初日を日時文字列で取得します
        /// 年月日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetFirstDayOfNextMonth(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetFirstDayOfNextMonth()
                .ToString(
                    format: _GetCultureFormat(), 
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月日に対して来月末日を日時文字列で取得します
        /// 年月日が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetLastDayOfNextMonth(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetLastDayOfNextMonth()
                .ToString(
                    format: _GetCultureFormat(), 
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 祝祭日を追加します
        /// 祝祭日終了年は2099が設定されます
        /// </summary>
        /// <param name="dts">月日(年は無視されます)</param>
        /// <param name="startApplyYear">祝祭日開始年</param>
        /// <param name="name">名称</param>
        public static void AddNewHoliday(
            this Primitive dts,
            Primitive startApplyYear,
            Primitive name
            )
        {
            var dt = _ConvertDateTime(dts);

            _Holiday.AddHolidayInfo(
                dt: dt,
                hk: HolidayInfo.enmHolidayKind.国民の祝日,
                param: (int)0,
                startApplyYear: (int)startApplyYear,
                endApplyYear: ClassHoliday.MAX_YEAR,
                name: name.ToString(),
                definition: string.Format(
                    "{0}月{1}日",
                    dt.Month,
                    dt.Day
                    )
                );
        }
        /// <summary>
        /// 年月日文字列の祝祭日終了年を設定します
        /// 月日が使用されます
        /// 終了年の次年から祝祭日と判定されなくなります
        /// </summary>
        /// <param name="dts">年月日文字列(月日)</param>
        /// <param name="endApplyYear">終了年</param>
        public static void ChangeHolidayEndApplyYear(
            this Primitive dts,
            Primitive endApplyYear
            )
        {
            // 年月日文字列からHolidayInfoを取得する
            var hi = _Holiday.GetHolidayInfo(
                dt: _ConvertDateTime(dts)
                );

            // HolidayInfoが取得できなかった場合
            if (hi == null)
                // 例外発生
                throw new ArgumentOutOfRangeException($"対象日({dts.ToString()})は祝祭日ではありません");

            // 終了年を設定する
            hi._EndApplyYear = (int)endApplyYear;
        }

        /// <summary>
        /// 日時文字列の年に対して祝祭日一覧(文字列の配列)を取得します
        /// 有効な
        /// 年が使用されます
        /// 
        /// 結果は日時文字列 + 空白 + 祝祭日名の形式で返します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字配列</returns>
        public static Primitive GetHolidays(
            this Primitive dts
            )
        {
            // 戻り値初期化
            var result = new Primitive();

            // 対象日を取得する
            var dt = _ConvertDateTime(dts: dts);

            // 休日リストで回す
            int index = 0;
            foreach (KeyValuePair<DateTime, HolidayInfo> hi
                in _Holiday.GetAllHoliday(iYear: dt.Year))
            {
                // "日時 祝祭日名"を追加する
                result[++index] = hi.Value
                    ._Date.ToString(
                        format: _GetCultureFormat(), 
                        provider: _GetCultureInfo()
                        )
                    + " " + hi.Value._Name; 
            }

            // 結果を返す
            return result;
        }
        /// <summary>
        /// 日時文字列の年月日に対して祝祭日名を文字列で取得します
        /// 年月日が使用されます
        /// 休日でない場合は空文字となります
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字列</returns>
        public static Primitive GetHolidayName(
            this Primitive dts
            )
        {
            // 対象日を取得する
            var dt = _ConvertDateTime(dts: dts);

            // 休日リストで回す
            foreach (KeyValuePair<DateTime, HolidayInfo> hi
                in _Holiday.GetAllHoliday(iYear: dt.Year))
            {
                // 日付が一致する場合
                if (hi.Value._Date.Date == dt.Date)
                    // 祝祭日名を返す
                    return hi.Value._Name;
            }

            // 空文字を返す
            return string.Empty;
        }
        /// <summary>
        /// 日時文字列の年月日が祝祭日か調べます
        /// 年月日が使用されます
        /// 祝祭日の場合は"true", 祝祭日でない場合は"false"が返ります
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>BOOL</returns>
        public static Primitive CheckHoliday(
            this Primitive dts
            )
        {
            // 対象日を取得する
            var dt = _ConvertDateTime(dts: dts);

            // 休日リストで回す
            foreach (KeyValuePair<DateTime, HolidayInfo> hi 
                in _Holiday.GetAllHoliday(iYear: dt.Year))
            {
                // 日付が一致する場合
                if (hi.Value._Date.Date == dt.Date)
                    // trueを返す
                    return true;
            }

            // falseを返す
            return false;
        }
        /// <summary>
        /// 日時文字列の年に対して年の第何週かを数値で取得します
        /// 年を使用します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetWeekOfYear(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetWeekOfYear(
                    ci: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月に対して月の第何週かを数値で取得します
        /// 年月を使用します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetWeekOfMonth(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetWeekOfMonth();
        }
        /// <summary>
        /// 日時文字列の月に対して年度を数値で取得します
        /// 年度開始月は4月となります
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetFiscalYear(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetFiscalYear();
        }
        /// <summary>
        /// 日時文字列に対して先月の最終日曜日を日時文字列で取得します
        /// 年月を使用します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetBeforeMonthLastSunday(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetBeforeMonthLastSunday()
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月日以前の平日を日時文字列で取得します
        /// 年月日を使用します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetBeforeWeekday(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetBeforeWeekday()
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年月日以降の平日を日時文字列で取得します
        /// 年月日を使用します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetAfterWeekday(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetAfterWeekday()
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年始から月日までの日数を数値で取得します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetDaysFromBeginningOfYear(
            this Primitive dts
            )
        {
            // DateTimeに変換する
            var dt = _ConvertDateTime(dts: dts);

            // 年始を取得する
            var dt1 = new DateTime(
                year: dt.Year,
                month: 1,
                day: 1
                );

            // 年始からの日数を求め返す
            return (int)((dt - dt1).TotalDays);
        }
        /// <summary>
        /// 日時文字列の年に対して年六十干支を文字列で取得します
        /// 年が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字列</returns>
        public static Primitive GetYearSixtyZodiacName(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 年六十干支を取得する
            var index = _GetSexagenaryYear(dt);

            // 負数の場合は空文字を返す
            if (index < 0)
                return string.Empty;

            // 結果を返す
            return _KAN_SHI[index];
        }
        /// <summary>
        /// 月時文字列の年に対して月六十干支を文字列で取得します
        /// 年月が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字列</returns>
        public static Primitive GetMonthSixtyZodiacName(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 月六十干支を取得する
            var index = _GetSexagenaryMonth(dt);

            // 負数の場合は空文字を返す
            if (index < 0)
                return string.Empty;

            // 結果を返す
            return _KAN_SHI[index];
        }
        /// <summary>
        /// 日時文字列の年に対して日六十干支を文字列で取得します
        /// 年が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字列</returns>
        public static Primitive GetDaySixtyZodiacName(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 日六十干支を取得する
            var index = _GetSexagenaryDay(dt);

            // 負数の場合は逆移動とする
            if (index < 0)
                index = _KAN_SHI.Count - 1 + index;

            // 結果を返す
            return _KAN_SHI[index];
        }
        /// <summary>
        /// 日時文字列の年月に対して日六十干支を文字列で取得します
        /// 年月が使用されます
        /// </summary>
        /// <param name="dts">日時文字列1</param>
        /// <returns>文字列</returns>
        public static Primitive GetDaySixtyZodiacNameOfMondth(
            this Primitive dts
            )
        {
            // 戻り値取得
            var result = new Primitive();

            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);
            var dtf = dt._GetFirstDayOfMonth();
            var dtl = dt._GetLastDayOfMonth();

            // d1年月からd2年月まで繰り返す
            var index = 1;
            while (dtf.Date <= dtl.Date)
            {
                // 月六十干支を取得する
                var idx = _GetSexagenaryDay(dtf);

                // 負数の場合は空文字とする
                if (idx < 0)
                    return string.Empty;

                // 結果を保存する
                result[index++] = _KAN_SHI[idx];

                // 日を増やす
                dtf = dtf.AddDays(1);
            }

            // 結果を返す
            return result;
        }
        /// <summary>
        /// 六曜取得の有効最小年月日を日時文字列で取得します
        /// </summary>
        /// <returns>日時文字列</returns>
        public static Primitive GetMinSixLabels()
        {
            return (new JapaneseCalendar()).MinSupportedDateTime
                .ToString(
                    format: _FormatDefault
                    );
        }
        /// <summary>
        /// 六曜取得の有効最大年月日を日時文字列で取得します
        /// </summary>
        /// <returns>日時文字列</returns>
        public static Primitive GetMaxSixLabels()
        {
            return (new JapaneseCalendar()).MaxSupportedDateTime
                .ToString(
                    format: _FormatDefault
                    );
        }
        /// <summary>
        /// 日時文字列の年月日に対して六曜を文字列で取得します
        /// 範囲 : 1860年01月28日～2050年01月22日
        /// 年月日を使用します
        /// 範囲外の場合は空文字を返します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字列</returns>
        public static Primitive GetSixLabels(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return dt._GetJCSixLabels();
        }
        /// <summary>
        /// 日時文字列の年月に対して六曜を文字列の配列で取得します
        /// 範囲 : 1860年01月28日～2050年01月22日
        /// 年月日を使用します
        /// 範囲外の場合は空文字を返します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字列配列</returns>
        public static Primitive GetSixLabelsOfMonth(
            this Primitive dts
            )
        {
            // 戻り値取得
            var result = new Primitive();

            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);
            var dtf = dt._GetFirstDayOfMonth();
            var dtl = dt._GetLastDayOfMonth();

            // d1年月からd2年月まで繰り返す
            var index = 1;
            while (dtf.Date <= dtl.Date)
            {
                // 六曜を取得し配列に設定する
                result[index++] = _GetJCSixLabels(dtf);

                // 日を増やす
                dtf = dtf.AddDays(1);
            }

            // 結果を返す
            return result;
        }
        /// <summary>
        /// 日時文字列の年月に対して陰暦名を文字列で取得します
        /// 年月を使用します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字列</returns>
        public static Primitive GetLunarCalendarMonthName(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _LCMN[dt.Month - 1];
        }
        /// <summary>
        /// 修正ユリウス日の開始日を西暦の日時文字列で取得します
        /// </summary>
        /// <returns>日時文字列</returns>
        public static Primitive GetStartModifiedJulianDay()
        {
            return _START_MODIFIED_JULIAN
                .ToString(
                    format: _FormatDefault
                );
        }
        /// <summary>
        /// ユリウス通日を数値で取得します
        /// 小数点以下は時間になります
        /// 年月日時分秒(UTC)が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetJulianDayNumber(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _GetJulianDayNumber(dt);
        }
        /// <summary>
        /// 修正ユリウス日を数値で取得します
        /// 小数点以下は時間になります
        /// 年月日時分秒(UTC)が使用されます
        /// 修正ユリウス日の開始日より前は負の値になります
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetModifiedJulianDate(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _GetModifiedJulianDay(dt);
        }
        /// <summary>
        /// 対象日の月齢を簡易月齢計算法で求め数値で取得します
        /// 簡易月齢計算法
        /// aom = (((Y - 11) % 19) * 11 + c(M) + d) % 30
        /// c(M - 1) = { 0, 2, 0, 2, 2, 4, 5, 6, 7, 8, 9, 10 }
        /// </summary>
        /// <param name="dts">対象日</param>
        /// <returns>数値</returns>
        public static Primitive GetSimpleAgeOfMoon(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _GetSimpleAgeOfMoon(dt);
        }
        /// <summary>
        /// 日付文字列から正午月齢nを求め数値で取得します
        /// 日本時間(UTC+9)に固定
        /// 年月日を使用します
        /// 結果 : 0 ≦ n ＜ 30
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>数値</returns>
        public static Primitive GetAgeOfMoon(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 正午月齢とする(正午12時, 時差+9 なので 12 - 9 = 3)
            var ndt = new DateTime(
                year: dt.Year,
                month: dt.Month,
                day: dt.Day,
                hour: 3,
                minute: 0,
                second: 0
                );

            // 結果を返す
            return _GetAgeOfMoon(dt: ndt).Item1;
        }
        /// <summary>
        /// 日時文字列の年の冬至日の日時文字列を取得します
        /// 年が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetWinterSolstice(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _GetWinterSolstice(dt.Year)
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列の年の夏至日の日時文字列を取得します
        /// 年が使用されます
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetSummerSolstice(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _GetSummerSolstice(dt.Year)
                .ToString(
                    format: _GetCultureFormat(),
                    provider: _GetCultureInfo()
                    );
        }
        /// <summary>
        /// 日時文字列から元号名を文字列で取得します
        /// 元号の切り替え年月日が重なる年号の場合、新元号が選択されます
        /// 年月日が使用されます
        /// 発見できなかった場合は空文字を返します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字列</returns>
        public static Primitive GetEraName(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _EraInfomation
                .GetEraNameFromDateTime(dt: dt);
        }
        /// <summary>
        /// 日時文字列から元号名の読みを文字列で取得します
        /// 元号の切り替え年月日が重なる年号の場合、新元号が選択されます
        /// 年月日が使用されます
        /// 発見できなかった場合は空文字を返します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>文字列</returns>
        public static Primitive GetEraYomi(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _EraInfomation
                .GetEraYomiFromDateTime(dt: dt);
        }
        /// <summary>
        /// 扱える最小日時を日時文字列で取得します
        /// </summary>
        /// <returns>日時文字列</returns>
        public static Primitive GetMinDateTime()
        {
            return DateTime.MinValue
                .ToString(
                    format: _FormatDefault
                    );
        }
        /// <summary>
        /// 扱える最大日時を日時文字列で取得します
        /// </summary>
        /// <returns>日時文字列</returns>
        public static Primitive GetMaxDateTime()
        {
            return DateTime.MaxValue
                .ToString(
                    format: _FormatDefault
                    );
        }
        /// <summary>
        /// 日時文字列から取得した元号の開始日を西暦の日時文字列で取得します
        /// 元号の切り替え年月日が重なる年号の場合、新元号が選択されます
        /// 年月日が使用されます
        /// 発見できなかった場合は日付の最大値を返します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetEraStart(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _EraInfomation.GetStartDateTimeFromDateTime(dt: dt)
                .ToString(
                    format: _FormatDefault
                    );
        }
        /// <summary>
        /// 日時文字列から取得した元号の終了日を西暦の日時文字列で取得します
        /// 元号の切り替え年月日が重なる年号の場合、新元号が選択されます
        /// 年月日が使用されます
        /// 発見できなかった場合は日付の最大値を返します
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>日時文字列</returns>
        public static Primitive GetEraEnd(
            this Primitive dts
            )
        {
            // DateTimeを取得する
            var dt = _ConvertDateTime(dts: dts);

            // 結果を返す
            return _EraInfomation.GetEndDateTimeFromDateTime(dt: dt)
                .ToString(
                    format: _FormatDefault
                    );
        }
        /// <summary>
        /// 新しい元号を追加し、結果を取得します
        /// 最大10個の元号を追加できます
        /// 同じ番号を指定した場合上書きされます
        /// 成功した場合はtrueが返ります
        /// </summary>
        /// <param name="no">新元号の番号を0-9の数字で指定します</param>
        /// <param name="eraName">元号名</param>
        /// <param name="eraYomi">読み</param>
        /// <param name="dtsStart">開始年月日文字列</param>
        /// <param name="dtsEnd">終了年月日文字列</param>
        /// <returns>BOOL</returns>
        public static Primitive AddNewEra(
            this Primitive no,
            Primitive eraName,
            Primitive eraYomi,
            Primitive dtsStart,
            Primitive dtsEnd
            )
        {
            // 新元号を追加する
            return _EraInfomation.ChangeAddedEraData(
                addedEraNo: (int)no,
                name: eraName.ToString(),
                yomi: eraYomi.ToString(),
                startDateTime: _ConvertDateTime(dts: dtsStart.ToString()),
                endDateTime: _ConvertDateTime(dts: dtsEnd.ToString())
                );
        }
        /// <summary>
        /// 元号名または読みから元号データを修正し、
        /// 結果を取得します
        /// 成功した場合trueが返ります
        /// </summary>
        /// <param name="nameOrYomi">元号または読み</param>
        /// <param name="newEraName">新元号名</param>
        /// <param name="newEraYomi">新読み</param>
        /// <param name="newDtsStart">新開始年月日文字列</param>
        /// <param name="newDtsEnd">新終了年月日文字列</param>
        /// <returns>BOOL</returns>
        public static Primitive ModifyEra(
            this Primitive nameOrYomi,
            Primitive newEraName,
            Primitive newEraYomi,
            Primitive newDtsStart,
            Primitive newDtsEnd
            )
        {
            // 戻り値初期化
            var ret = true;

            // 開始年月日を修正する
            if (ret)
                ret = _EraInfomation.ModifyEraStartDate(
                eraNameOrYomi: nameOrYomi.ToString(),
                dt: _ConvertDateTime(newDtsStart.ToString())
                );

            // 終了年月日を修正する
            if (ret)
                ret = _EraInfomation.ModifyEraEndDate(
                    eraNameOrYomi: nameOrYomi.ToString(),
                    dt: _ConvertDateTime(newDtsEnd.ToString())
                    );

            // 読みを修正する
            if (ret)
                ret = _EraInfomation.ModifyEraYomi(
                    eraNameOrYomi: nameOrYomi.ToString(), 
                    yomi: newEraYomi.ToString()
                    );

            // 元号名を修正する
            if (ret)
                ret = _EraInfomation.ModifyEraName(
                    eraNameOrYomi: nameOrYomi.ToString(),
                    name: newEraName.ToString()
                    );

            return ret;
        }
        /// <summary>
        /// 元号名一覧を文字列配列で取得します
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetEraNameList()
        {
            return _EraInfomation.EraNameList.ConvertDimStringToPrimitive();
        }
        #endregion 公開メソッド

        #region デバッグ用非公開メソッド
        /// <summary>
        /// EraData取得
        /// 配列[1] = 元号名
        /// 配列[2] = 読み
        /// 配列[3] = 開始日
        /// 配列[4] = 終了日
        /// ※デバッグ用非公開メソッド
        /// </summary>
        /// <param name="eraNo">元号番号</param>
        /// <returns>結果</returns>
        private static Primitive GetEraData(
            Primitive eraNo
            )
        {
            var index = _EraInfomation.GetAddedEraIndex((int)eraNo);
            var eraData = _EraInfomation.EraDataList.ToList()[index];

            Primitive ret = new Primitive();

            ret[1] = eraData.Name;
            ret[2] = eraData.Yomi;
            ret[3] = eraData.Start;
            ret[4] = eraData.End;

            return ret;
        }
        #endregion デバッグ用非公開メソッド

        #region メソッド

        #region カルチャーを取得する
        /// <summary>
        /// カルチャーを取得する
        /// </summary>
        /// <returns>結果</returns>
        private static Culture _GetCulture()
        {
            return _GetCulture(cit: _CultureInfoType);
        }
        /// <summary>
        /// カルチャーを取得する
        /// </summary>
        /// <param name="cit">カルチャータイプ</param>
        /// <returns>結果</returns>
        private static Culture _GetCulture(
            this enmCultureType cit
            )
        {
            return _CultureInfoList[(int)cit];
        }
        #endregion カルチャーを取得する

        #region カルチャー情報を取得する
        /// <summary>
        /// カルチャー情報を取得する
        /// </summary>
        /// <returns>結果</returns>
        private static CultureInfo _GetCultureInfo()
        {
            return _GetCultureInfo(cit: _CultureInfoType);
        }
        /// <summary>
        /// カルチャー情報を取得する
        /// </summary>
        /// <param name="cit">カルチャータイプ</param>
        /// <returns>結果</returns>
        private static CultureInfo _GetCultureInfo(
            this enmCultureType cit
            )
        {
            return _CultureInfoList[(int)cit].Info;
        }
        #endregion カルチャー情報を取得する

        #region カルチャーフォーマットを取得する
        /// <summary>
        /// カルチャーフォーマットを取得する
        /// </summary>
        /// <returns>結果</returns>
        public static string _GetCultureFormat()
        {
            return _GetCultureFormat(cit: _CultureInfoType);
        }
        /// <summary>
        /// カルチャーフォーマットを取得する
        /// </summary>
        /// <param name="cit">カルチャータイプ</param>
        /// <returns>結果</returns>
        private static string _GetCultureFormat(
            this enmCultureType cit
            )
        {
            return _CultureInfoList[(int)cit].Format;
        }
        #endregion カルチャーフォーマットを取得する

        #region 日時文字列をDateTimeに変換する
        /// <summary>
        /// 日時文字列をDateTimeに変換する
        /// フォーマットが指定されている必要がある
        /// </summary>
        /// <param name="dts">日時文字列</param>
        /// <returns>結果</returns>
        public static DateTime _ConvertDateTime(
            this string dts
            )
        {
            DateTime dt;

            // 日付に変換する
            if (DateTime.TryParseExact(
                s: dts,
                format: _GetCultureFormat(),
                provider: _GetCultureInfo(),
                style: DateTimeStyles.None,
                result: out dt
                ))
                return dt;
            else
                throw new FormatException();
        }
        #endregion 日時文字列をDateTimeに変換する

        #region 該当年月の日数を返す
        /// <summary>
        /// 該当年月の日数を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>日数</returns>
        private static int _GetDaysInMonth(
            this DateTime dt
            )
        {
            // 該当年月の日数を返す
            return DateTime.DaysInMonth(
                year: dt.Year,
                month: dt.Month
                );
        }
        #endregion 該当年月の日数を返す

        #region 指定日の前日を返す
        /// <summary>
        /// 指定日の前日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>DateTime</returns>
        private static DateTime _GetPrevDay(
            this DateTime dt
            )
        {
            // 指定日の前日を返す
            return dt.AddDays(value: -1);
        }
        #endregion 指定日の前日を返す

        #region 指定日の明日を返す
        /// <summary>
        /// 指定日の明日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>DateTime</returns>
        private static DateTime _GetNextDay(
            this DateTime dt
            )
        {
            // 指定日の明日を返す
            return dt.AddDays(value: 1);
        }
        #endregion 指定日の明日を返す

        #region 指定日の月初日を返す
        /// <summary>
        /// 指定日の月初日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>Datetime</returns>
        private static DateTime _GetFirstDayOfMonth(
            this DateTime dt
            )
        {
            // 指定日の日数を引いて1日にする
            return dt.AddDays(value: (dt.Day - 1) * -1);
        }
        #endregion 指定日の月初日を返す

        #region 指定日の月末を返す
        /// <summary>
        /// 指定日の月末を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>DateTime</returns>
        private static DateTime _GetLastDayOfMonth(
            this DateTime dt
            )
        {
            // 指定日の月末を返す
            return new DateTime(
                year: dt.Year,
                month: dt.Month,
                day: _GetDaysInMonth(dt)
                );
        }
        #endregion 指定日の月末を返す

        #region 指定日の先月初日を返す
        /// <summary>
        /// 指定日の先月初日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>Datetime</returns>
        private static DateTime _GetFirstDayOfPrevMonth(
            this DateTime dt
            )
        {
            // 指定日の先月末日から月初を取得する
            return dt._GetLastDayOfPrevMonth()._GetFirstDayOfMonth();
        }
        #endregion 先月初日を返す

        #region 指定日の先月末日を返す
        /// <summary>
        /// 指定日の先月末日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>Datetime</returns>
        private static DateTime _GetLastDayOfPrevMonth(
            this DateTime dt
            )
        {
            // 指定日の月初から1日引く
            return dt._GetFirstDayOfMonth().AddDays(value: -1);
        }
        #endregion 先月末日を返す

        #region 指定日の来月初日を返す
        /// <summary>
        /// 指定日の来月初日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>Datetime</returns>
        private static DateTime _GetFirstDayOfNextMonth(
            this DateTime dt
            )
        {
            // 指定日の月末に1日足す
            return dt._GetLastDayOfMonth().AddDays(value: 1);
        }
        #endregion 指定日の来月初日を返す

        #region 指定日の来月末日を返す
        /// <summary>
        /// 指定日の来月末日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>Datetime</returns>
        private static DateTime _GetLastDayOfNextMonth(
            this DateTime dt
            )
        {
            // 指定日の来月初日の月末を求める
            return dt._GetFirstDayOfNextMonth()._GetLastDayOfMonth();
        }
        #endregion 指定日の来月末日を返す

        #region 対象年の第n週を取得する
        /// <summary>
        /// 対象年の第n週を取得する
        /// </summary>
        /// <param name="dt">対象年</param>
        /// <param name="ci">カルチャー情報</param>
        /// <param name="rule">ルール</param>
        /// <param name="firstDayOfWeek">週最初の曜日</param>
        /// <returns>年の第n週</returns>
        private static int _GetWeekOfYear(
            this DateTime dt,
            CultureInfo ci,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday
            )
        {
            // 週を求める
            return ci.Calendar
                .GetWeekOfYear(
                    time: dt,
                    rule: rule,
                    firstDayOfWeek: firstDayOfWeek
                    );
        }
        #endregion 対象年の第n週を取得する

        #region 対象月の第n週を取得する
        /// <summary>
        /// 対象月の第n週を取得する
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>月の第n週</returns>
        private static int _GetWeekOfMonth(
            this DateTime dt
            )
        {
            // カルチャーを取得する
            var ci = _GetCulture();

            // 週を求める
            return _GetWeekOfYear(
                dt: dt,
                ci: ci.Info
                ) - _GetWeekOfYear(
                    dt: dt._GetFirstDayOfMonth(),
                    ci: ci.Info
                    ) + 1;
        }
        #endregion 月の第n週を取得する

        #region 該当日付の年度を返す
        /// <summary>
        /// 該当日付の年度を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <param name="iStartingMonth">年度の開始月</param>
        /// <returns>年度</returns>
        private static int _GetFiscalYear(
            this DateTime dt,
            int iStartingMonth = 4
            )
        {
            return (dt.Month >= iStartingMonth) 
                ? dt.Year 
                : dt.Year - 1;
        }
        #endregion 該当日付の年度を返す

        #region 対象日が休日かチェックする
        /// <summary>
        /// 対象日が祝日かチェックする
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>休日の場合true</returns>
        private static bool _CheckHoliday(
            this DateTime dt
            )
        {
            // 休日リスト生成
            var hl = _Holiday.GetAllHoliday(dt.Year);

            // 休日チェック
            HolidayInfo hi = null;
            bool r = hl.TryGetValue(
                key: dt,
                value: out hi
                );
            return r;
        }
        #endregion 対象日が平日かチェックする

        #region 対象日以前の平日を取得する
        /// <summary>
        /// 対象日以前の平日(日曜と祝日除く)を取得する
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>DateTime</returns>
        private static DateTime _GetBeforeWeekday(
            this DateTime dt
            )
        {
            // 平日になるまで繰り返す
            bool noHit = true;
            while (noHit)
            {
                // 次日にする
                dt = dt.AddDays(value: 1);

                // 月曜～土曜チェック
                if (dt.DayOfWeek != DayOfWeek.Sunday)
                    // 休日チェック
                    noHit = dt._CheckHoliday();
            }
            return dt;
        }
        #endregion 対象日以前の平日を取得する

        #region 対象日以降の平日を取得する
        /// <summary>
        /// 対象日以降の平日(日曜と祝日除く)を取得する
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>DateTime</returns>
        private static DateTime _GetAfterWeekday(
            this DateTime dt
            )
        {
            // 平日になるまで繰り返す
            bool noHit = true;
            while (noHit)
            {
                // 前日にする
                dt = dt.AddDays(value: -1);

                // 月曜～土曜チェック
                if (dt.DayOfWeek != DayOfWeek.Sunday)
                    // 休日チェック
                    noHit = dt._CheckHoliday();
            }
            return dt;
        }
        #endregion 対象日以前の平日を取得する

        #region 先月の最終日曜日を取得する
        /// <summary>
        /// 先月の最終日曜日を取得する
        /// </summary>
        /// <param name="dt">対象年月</param>
        /// <returns>DateTime</returns>
        private static DateTime _GetBeforeMonthLastSunday(
            this DateTime dt
            )
        {
            // 先月の最終日曜日を取得する
            DateTime lastDate = dt._GetFirstDayOfPrevMonth()._GetLastDayOfMonth();
            int subDay = (int)(lastDate.DayOfWeek);
            DateTime ret = lastDate.AddDays(value: -subDay);
            return ret;
        }
        #endregion 先月の最終日曜日を取得する

        #region 年六十干支を取得する
        /// <summary>
        /// 年六十干支を取得する
        /// 結果は0-59の数値が返る
        /// (dt.Year - 4) % 60
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>結果</returns>
        private static int _GetSexagenaryYear(
            this DateTime dt
            )
        {
            return (dt.Year - 4) % 60;
        }
        #endregion 年六十干支を取得する

        #region 月六十干支を取得する
        /// <summary>
        /// 月六十干支を取得する
        /// 結果は0-59の数値が返る
        /// (y * 12 + m + 13) % 60
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>結果</returns>
        private static int _GetSexagenaryMonth(
            this DateTime dt
            )
        {
            return (dt.Year * 12 + dt.Month + 13) % 60;
        }
        #endregion 月六十干支を取得する

        #region 日六十干支を取得する
        /// <summary>
        /// 日六十干支を取得する
        /// 結果は0-59の数値が返る
        /// 修正ユリウス日 % 60
        /// 修正ユリウス日以前の場合は負の値を返します
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>結果</returns>
        private static int _GetSexagenaryDay(
            this DateTime dt
            )
        {
            // 結果を返す
            return (int)(_GetModifiedJulianDay(dt) + 50) % 60;
        }
        #endregion 日六十干支を取得する

        #region 六曜を取得する
        /// <summary>
        /// 対象日の六曜を取得する
        /// 対象日が範囲外の場合は空文字をかえす
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>結果</returns>
        private static string _GetJCSixLabels(
            this DateTime dt
            )
        {
            // 和暦、太陰太陽暦（旧暦）クラス生成
            var jlc = new JapaneseLunisolarCalendar();

            // 範囲チェック
            if (dt < jlc.MinSupportedDateTime || dt > jlc.MaxSupportedDateTime)
                return string.Empty;

            // 太陰太陽暦を使って六曜を求める
            int jlcMonth = jlc.GetMonth(dt);
            int jlcDay = jlc.GetDayOfMonth(dt);

            // 閏月算出のため太陰太陽暦での元日を取得する
            DateTime jocnyd = 
                jlc.AddDays(
                    time: dt, 
                    days: 1 - jlc.GetDayOfYear(dt)
                    );

            // 閏月を取得
            // たとえば2017年であれば、閏月=6 が返る。6番目の月が閏月、すなわち「閏月は5月」となる。
            // 閏月のない年では、閏月=0 が返る。
            int leapMonth = jlc.GetLeapMonth(
                year: jlc.GetYear(jocnyd),
                era: jlc.GetEra(jocnyd)
                );

            // 閏月がある場合の旧暦月の補正
            if ((leapMonth > 0) && (jlcMonth >= leapMonth))
                jlcMonth--;

            // 六曜を算出
            return _JCSL[
                (jlcMonth + jlcDay) % 6
                ];
        }
        #endregion 六曜を求める

        #region ユリウス通日を求める
        /// <summary>
        /// ユリウス通日を求める
        /// 小数点以下は時間になります
        /// 年月日時分秒が使用されます
        /// [365.25 * Y] + [Y / 400] - [Y / 100] + [30.59 * (M - 2)] + D + 1721088.5 + h / 24 + m / 1440 + s / 86400
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>結果</returns>
        private static double _GetJulianDayNumber(
            this DateTime dt
            )
        {
            // 年月日時分秒
            double Y = dt.Year;
            double M = dt.Month;
            double D = dt.Day;
            double h = dt.Hour;
            double m = dt.Minute;
            double s = dt.Second;

            // 1月、2月は前年の13月、14月として計算
            if (M == 1 || M == 2)
            {
                Y -= 1;
                M += 12;
            }

            // 結果を求める
            return
                System.Math.Floor(365.25 * Y)
                + System.Math.Floor(Y / 400)
                - System.Math.Floor(Y / 100)
                + System.Math.Floor(30.59 * (M - 2))
                + D
                + 1721088.5 + h / 24
                + m / 1440
                + s / 86400
                ;
        }
        #endregion ユリウス通日を求める

        #region グレゴリオ暦から修正ユリウス日を求める
        /// <summary>
        /// グレゴリオ暦(UTC)から修正ユリウス日を求める
        /// 小数点以下は時間になります
        /// 年月日時分秒(UTC)が使用されます
        /// ユリウス通日から2400000.5を差し引いたもの
        /// これにより1858年11月17日正子UTが元期となる
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>結果</returns>
        private static double _GetModifiedJulianDay(
            this DateTime dt
            )
        {
            // 結果を返す
            return _GetJulianDayNumber(dt) - 2400000.5;
        }
        #endregion グレゴリオ暦から修正ユリウス日を求める

        #region 対象日の月齢を簡易月齢計算法で求める
        /// <summary>
        /// 対象日の月齢を簡易月齢計算法で求める
        /// 簡易月齢計算法
        /// aom = (((Y - 11) % 19) * 11 + c(M) + d) % 30
        /// c(M - 1) = { 0, 2, 0, 2, 2, 4, 5, 6, 7, 8, 9, 10 }
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>結果</returns>
        private static double _GetSimpleAgeOfMoon(
            this DateTime dt
            )
        {
            // c(M)
            var c = new double[] { 0, 2, 0, 2, 2, 4, 5, 6, 7, 8, 9, 10 };

            double Y = dt.Year;
            int M = dt.Month;
            double D = dt.Day;

            // 結果を返す
            return (((Y - 11) % 19) * 11 + c[M - 1] + D) % 30;
        }
        #endregion 対象日の月齢を簡易月齢計算法で求める

        #region 対象日の月齢を求める
        /// <summary>
        /// 対象日の月齢を求める
        /// 正午の場合+12時間する
        /// 月齢 = ユリウス通日 - 新月日
        /// 新月直前の数値の場合前日の月齢が返る
        /// 結果：(月齢, 再計算フラグ)
        /// </summary>
        /// <param name="dt">対象日</param>
        /// <returns>結果</returns>
        public static Tuple<double, bool> _GetAgeOfMoon(
            this DateTime dt
            )
        {
            // ユリウス通日に時差を加算したもの
            var jdn = _GetJulianDayNumber(dt);

            // 新月日
            var nmd = _GetNewMoonDay(jdn);

            // jymd < nmdの場合は前日で再計算
            bool recalc = false;
            if (jdn < nmd)
            {
                nmd = _GetNewMoonDay(jdn - 1.0);
                recalc = true;
            }

            // 月齢(ユリウス通日 - 新月日)
            var aom = jdn - nmd;

            // 月齢を返す
            return new Tuple<double, bool>(aom, recalc);
        }
        /// <summary>
        /// 新月日を求める
        /// </summary>
        /// <param name="jdm">ユリウス通日</param>
        /// <returns>結果</returns>
        private static double _GetNewMoonDay(
            this double jdm
            )
        {
            // 下記コードを参考とした
            // 月齢計算 version 2.3                for JavaScript 1.1
            // https://news.local-group.jp/moonage/moonage.js.txt

            // 新月日を求める
            double k = System.Math.Floor((jdm - 2451550.09765) / 29.530589);
            double t = k / 1236.85;
            double nmd = 2451550.09765
                + 29.530589 * k
                + 0.0001337 * t * t
                - 0.40720 * System.Math.Sin((201.5643 + 385.8169 * k) * 0.017453292519943)
                + 0.17241 * System.Math.Sin((2.5534 + 29.1054 * k) * 0.017453292519943);

            // 結果を返す(ユリウス通日 - 新月日が現在時刻の月齢)
            return nmd;
        }
        #endregion 対象日の月齢を求める

        #region 1900年から2099年までの冬至を求める
        /// <summary>
        /// 1900年から2099年までの冬至を求める
        /// 2099年までの冬至計算式
        /// floor(22.6587 + 0.24274049 * (Y-1900)) - floor((Y - 1900) / 4))
        /// </summary>
        /// <param name="year">西暦年</param>
        /// <returns>結果</returns>
        private static DateTime _GetWinterSolstice(
            int year
            )
        {
            // 入力チェック
            if (year < 1900 || year > 2099)
                throw new ArgumentOutOfRangeException();

            // 2099年までの冬至を求める計算式
            // floor(22.6587 + 0.24274049 * (Y - 1900)) - floor((Y - 1900) / 4)
            var day =
                System.Math.Floor(22.6587 + 0.24274049 * ((double)year - 1900))
                - System.Math.Floor(((double)year - 1900) / 4);

            // 結果を返す
            return new DateTime(
                year: year,
                month: 12,
                day: (int)day
                );
        }
        #endregion 1900年から2099年までの冬至を求める

        #region 1900年から2099年までの夏至を求める
        /// <summary>
        /// 1900年から2099年までの夏至を求める
        /// 2099年までの夏至計算式
        /// floor(22.2747 + 0.241669 * (Y - 1900)) - floor((Y - 1900) / 4)
        /// </summary>
        /// <param name="year">西暦年</param>
        /// <returns>結果</returns>
        private static DateTime _GetSummerSolstice(
            int year
            )
        {
            // 入力チェック
            if (year < 1900 || year > 2099)
                throw new ArgumentOutOfRangeException();

            // 2099年までの夏至を求める計算式
            // floor(22.2747 + 0.241669 * (Y - 1900)) - floor((Y - 1900) / 4))
            var day =
                System.Math.Floor(22.2747 + 0.241669 * ((double)year - 1900))
                + System.Math.Floor(((double)year - 1900) / 4);
            
            // 結果を返す
            return new DateTime(
                year: year,
                month: 6,
                day: (int)day
                );
        }
        #endregion 1900年から2099年までの夏至を求める

        #endregion メソッド

        #region 元号情報クラス
        /// <summary>
        /// 元号情報クラス
        /// </summary>
        private class EraInfomation
        {

            #region 固定値
            /// <summary>
            /// エラー年月日(日付の最大値)
            /// </summary>
            private readonly DateTime _ERROR_DATETIME = DateTime.MaxValue;
            /// <summary>
            /// 元号データリスト{ _N = 元号, _Y = よみ, start: YYYYMMDD, end: YYYYMMDD }
            /// グレゴリオ暦開始日以前はユリウス暦を使用します
            /// グレゴリオ暦開始日以降はグレゴリオ暦を使用します
            /// </summary>
            private List<EraData> _EraDataList = new List<EraData>()
            {
                new EraData( era: EraData.enmEra.大化, yomi: "たいか",               start: 6450717,     end: 6500322 ),
                new EraData( era: EraData.enmEra.白雉, yomi: "はくち",               start: 6500322,     end: 6541124 ),
                new EraData( era: EraData.enmEra.朱鳥, yomi: "しゅちょう",           start: 6860814,     end: 6861001 ),
                new EraData( era: EraData.enmEra.大宝, yomi: "たいほう",             start: 7010503,     end: 7040616 ),
                new EraData( era: EraData.enmEra.慶雲, yomi: "けいうん",             start: 7040616,     end: 7080207 ),
                new EraData( era: EraData.enmEra.和銅, yomi: "わどう",               start: 7080207,     end: 7151003 ),
                new EraData( era: EraData.enmEra.霊亀, yomi: "れいき",               start: 7151003,     end: 7171224 ),
                new EraData( era: EraData.enmEra.養老, yomi: "ようろう",             start: 7171224,     end: 7240303 ),
                new EraData( era: EraData.enmEra.神亀, yomi: "じんき",               start: 7240303,     end: 7290902 ),
                new EraData( era: EraData.enmEra.天平, yomi: "てんぴょう",           start: 7290902,     end: 7490504 ),
                new EraData( era: EraData.enmEra.天平感宝, yomi: "てんぴょうかんぽう",   start: 7490504,     end: 7490819 ),
                new EraData( era: EraData.enmEra.天平勝宝, yomi: "てんぴょうしょうほう", start: 7490819,     end: 7570906 ),
                new EraData( era: EraData.enmEra.天平宝字, yomi: "てんぴょうほうじ",     start: 7570906,     end: 7650201 ),
                new EraData( era: EraData.enmEra.天平神護, yomi: "てんぴょうじんご",     start: 7650201,     end: 7670913 ),
                new EraData( era: EraData.enmEra.神護景雲, yomi: "じんごけいうん",       start: 7670913,     end: 7701023 ),
                new EraData( era: EraData.enmEra.宝亀, yomi: "ほうき",               start: 7701023,     end: 7810130 ),
                new EraData( era: EraData.enmEra.天応, yomi: "てんおう",             start: 7810130,     end: 7820930 ),
                new EraData( era: EraData.enmEra.延暦, yomi: "えんりゃく",           start: 7820930,     end: 8060608 ),
                new EraData( era: EraData.enmEra.大同, yomi: "だいどう",             start: 8060608,     end: 8101020 ),
                new EraData( era: EraData.enmEra.弘仁, yomi: "こうにん",             start: 8101020,     end: 8240208 ),
                new EraData( era: EraData.enmEra.天長, yomi: "てんちょう",           start: 8240208,     end: 8340214 ),
                new EraData( era: EraData.enmEra.承和, yomi: "じょうわ",             start: 8340214,     end: 8480716 ),
                new EraData( era: EraData.enmEra.嘉祥, yomi: "かしょう",             start: 8480716,     end: 8510601 ),
                new EraData( era: EraData.enmEra.仁寿, yomi: "にんじゅ",             start: 8510601,     end: 8541223 ),
                new EraData( era: EraData.enmEra.斉衡, yomi: "さいこう",             start: 8541223,     end: 8570320 ),
                new EraData( era: EraData.enmEra.天安, yomi: "てんあん",             start: 8570320,     end: 8590520 ),
                new EraData( era: EraData.enmEra.貞観, yomi: "じょうがん",           start: 8590520,     end: 8770601 ),
                new EraData( era: EraData.enmEra.元慶, yomi: "がんぎょう",           start: 8770601,     end: 8850311 ),
                new EraData( era: EraData.enmEra.仁和, yomi: "にんな",               start: 8850311,     end: 8890530 ),
                new EraData( era: EraData.enmEra.寛平, yomi: "かんぴょう",           start: 8890530,     end: 8980520 ),
                new EraData( era: EraData.enmEra.昌泰, yomi: "しょうたい",           start: 8980520,     end: 9010831 ),
                new EraData( era: EraData.enmEra.延喜, yomi: "えんぎ",               start: 9010831,     end: 9230529 ),
                new EraData( era: EraData.enmEra.延長, yomi: "えんちょう",           start: 9230529,     end: 9310516 ),
                new EraData( era: EraData.enmEra.承平, yomi: "じょうへい",           start: 9310516,     end: 9380622 ),
                new EraData( era: EraData.enmEra.天慶, yomi: "てんぎょう",           start: 9380622,     end: 9470515 ),
                new EraData( era: EraData.enmEra.天暦, yomi: "てんりゃく",           start: 9470515,     end: 9571121 ),
                new EraData( era: EraData.enmEra.天徳, yomi: "てんとく",             start: 9571121,     end: 9610305 ),
                new EraData( era: EraData.enmEra.応和, yomi: "おうわ",               start: 9610305,     end: 9640819 ),
                new EraData( era: EraData.enmEra.康保, yomi: "こうほう",             start: 9640819,     end: 9680908 ),
                new EraData( era: EraData.enmEra.安和, yomi: "あんな",               start: 9680908,     end: 9700503 ),
                new EraData( era: EraData.enmEra.天禄, yomi: "てんろく",             start: 9700503,     end: 9740116 ),
                new EraData( era: EraData.enmEra.天延, yomi: "てんえん",             start: 9740116,     end: 9760811 ),
                new EraData( era: EraData.enmEra.貞元, yomi: "じょうげん",           start: 9760811,     end: 9781231 ),
                new EraData( era: EraData.enmEra.天元, yomi: "てんげん",             start: 9781231,     end: 9830529 ),
                new EraData( era: EraData.enmEra.永観, yomi: "えいかん",             start: 9830529,     end: 9850519 ),
                new EraData( era: EraData.enmEra.寛和, yomi: "かんな",               start: 9850519,     end: 9870505 ),
                new EraData( era: EraData.enmEra.永延, yomi: "えいえん",             start: 9870505,     end: 9890910 ),
                new EraData( era: EraData.enmEra.永祚, yomi: "えいそ",               start: 9890910,     end: 9901126 ),
                new EraData( era: EraData.enmEra.正暦, yomi: "しょうりゃく",         start: 9901126,     end: 9950325 ),
                new EraData( era: EraData.enmEra.長徳, yomi: "ちょうとく",           start: 9950325,     end: 9990201 ),
                new EraData( era: EraData.enmEra.長保, yomi: "ちょうほう",           start: 9990201,     end: 10040808 ),
                new EraData( era: EraData.enmEra.寛弘, yomi: "かんこう",             start: 10040808,    end: 10130208 ),
                new EraData( era: EraData.enmEra.長和, yomi: "ちょうわ",             start: 10130208,    end: 10170521 ),
                new EraData( era: EraData.enmEra.寛仁, yomi: "かんにん",             start: 10170521,    end: 10210317 ),
                new EraData( era: EraData.enmEra.治安, yomi: "じあん",               start: 10210317,    end: 10240819 ),
                new EraData( era: EraData.enmEra.万寿, yomi: "まんじゅ",             start: 10240819,    end: 10280818 ),
                new EraData( era: EraData.enmEra.長元, yomi: "ちょうげん",           start: 10280818,    end: 10370509 ),
                new EraData( era: EraData.enmEra.長暦, yomi: "ちょうりゃく",         start: 10370509,    end: 10401216 ),
                new EraData( era: EraData.enmEra.長久, yomi: "ちょうきゅう",         start: 10401216,    end: 10441216 ),
                new EraData( era: EraData.enmEra.寛徳, yomi: "かんとく",             start: 10441216,    end: 10460522 ),
                new EraData( era: EraData.enmEra.永承, yomi: "えいしょう",           start: 10460522,    end: 10530202 ),
                new EraData( era: EraData.enmEra.天喜, yomi: "てんき",               start: 10530202,    end: 10580919 ),
                new EraData( era: EraData.enmEra.康平, yomi: "こうへい",             start: 10580919,    end: 10650904 ),
                new EraData( era: EraData.enmEra.治暦, yomi: "じりゃく",             start: 10650904,    end: 10690506 ),
                new EraData( era: EraData.enmEra.延久, yomi: "えんきゅう",           start: 10690506,    end: 10740916 ),
                new EraData( era: EraData.enmEra.承保, yomi: "じょうほう",           start: 10740916,    end: 10771205 ),
                new EraData( era: EraData.enmEra.承暦, yomi: "じょうりゃく",         start: 10771205,    end: 10810322 ),
                new EraData( era: EraData.enmEra.永保, yomi: "えいほう",             start: 10810322,    end: 10840315 ),
                new EraData( era: EraData.enmEra.応徳, yomi: "おうとく",             start: 10840315,    end: 10870511 ),
                new EraData( era: EraData.enmEra.寛治, yomi: "かんじ",               start: 10870511,    end: 10950123 ),
                new EraData( era: EraData.enmEra.嘉保, yomi: "かほう",               start: 10950123,    end: 10970103 ),
                new EraData( era: EraData.enmEra.永長, yomi: "えいちょう",           start: 10970103,    end: 10971227 ),
                new EraData( era: EraData.enmEra.承徳, yomi: "じょうとく",           start: 10971227,    end: 10990915 ),
                new EraData( era: EraData.enmEra.康和, yomi: "こうわ",               start: 10990915,    end: 11040308 ),
                new EraData( era: EraData.enmEra.長治, yomi: "ちょうじ",             start: 11040308,    end: 11060513 ),
                new EraData( era: EraData.enmEra.嘉承, yomi: "かしょう",             start: 11060513,    end: 11080909 ),
                new EraData( era: EraData.enmEra.天仁, yomi: "てんにん",             start: 11080909,    end: 11100731 ),
                new EraData( era: EraData.enmEra.天永, yomi: "てんえい",             start: 11100731,    end: 11130825 ),
                new EraData( era: EraData.enmEra.永久, yomi: "えいきゅう",           start: 11130825,    end: 11180425 ),
                new EraData( era: EraData.enmEra.元永, yomi: "げんえい",             start: 11180425,    end: 11200509 ),
                new EraData( era: EraData.enmEra.保安, yomi: "ほうあん",             start: 11200509,    end: 11240518 ),
                new EraData( era: EraData.enmEra.天治, yomi: "てんじ",               start: 11240518,    end: 11260215 ),
                new EraData( era: EraData.enmEra.大治, yomi: "だいじ",               start: 11260215,    end: 11310228 ),
                new EraData( era: EraData.enmEra.天承, yomi: "てんしょう",           start: 11310228,    end: 11320921 ),
                new EraData( era: EraData.enmEra.長承, yomi: "ちょうしょう",         start: 11320921,    end: 11350610 ),
                new EraData( era: EraData.enmEra.保延, yomi: "ほうえん",             start: 11350610,    end: 11410813 ),
                new EraData( era: EraData.enmEra.永治, yomi: "えいじ",               start: 11410813,    end: 11420525 ),
                new EraData( era: EraData.enmEra.康治, yomi: "こうじ",               start: 11420525,    end: 11440328 ),
                new EraData( era: EraData.enmEra.天養, yomi: "てんよう",             start: 11440328,    end: 11450812 ),
                new EraData( era: EraData.enmEra.久安, yomi: "きゅうあん",           start: 11450812,    end: 11510214 ),
                new EraData( era: EraData.enmEra.仁平, yomi: "にんぺい",             start: 11510214,    end: 11541204 ),
                new EraData( era: EraData.enmEra.久寿, yomi: "きゅうじゅ",           start: 11541204,    end: 11560518 ),
                new EraData( era: EraData.enmEra.保元, yomi: "ほうげん",             start: 11560518,    end: 11590509 ),
                new EraData( era: EraData.enmEra.平治, yomi: "へいじ",               start: 11590509,    end: 11600218 ),
                new EraData( era: EraData.enmEra.永暦, yomi: "えいりゃく",           start: 11600218,    end: 11610924 ),
                new EraData( era: EraData.enmEra.応保, yomi: "おうほう",             start: 11610924,    end: 11630504 ),
                new EraData( era: EraData.enmEra.長寛, yomi: "ちょうかん",           start: 11630504,    end: 11650714 ),
                new EraData( era: EraData.enmEra.永万, yomi: "えいまん",             start: 11650714,    end: 11660923 ),
                new EraData( era: EraData.enmEra.仁安, yomi: "にんあん",             start: 11660923,    end: 11690506 ),
                new EraData( era: EraData.enmEra.嘉応, yomi: "かおう",               start: 11690506,    end: 11710527 ),
                new EraData( era: EraData.enmEra.承安, yomi: "しょうあん",           start: 11710527,    end: 11750816 ),
                new EraData( era: EraData.enmEra.安元, yomi: "あんげん",             start: 11750816,    end: 11770829 ),
                new EraData( era: EraData.enmEra.治承, yomi: "じしょう",             start: 11770829,    end: 11810825 ),
                new EraData( era: EraData.enmEra.養和, yomi: "ようわ",               start: 11810825,    end: 11820629 ),
                new EraData( era: EraData.enmEra.寿永, yomi: "じゅえい",             start: 11820629,    end: 11840527 ),
                new EraData( era: EraData.enmEra.元暦, yomi: "げんりゃく",           start: 11840527,    end: 11850909 ),
                new EraData( era: EraData.enmEra.文治, yomi: "ぶんじ",               start: 11850909,    end: 11900516 ),
                new EraData( era: EraData.enmEra.建久, yomi: "けんきゅう",           start: 11900516,    end: 11990523 ),
                new EraData( era: EraData.enmEra.正治, yomi: "しょうじ",             start: 11990523,    end: 12010319 ),
                new EraData( era: EraData.enmEra.建仁, yomi: "けんにん",             start: 12010319,    end: 12040323 ),
                new EraData( era: EraData.enmEra.元久, yomi: "げんきゅう",           start: 12040323,    end: 12060605 ),
                new EraData( era: EraData.enmEra.建永, yomi: "けんえい",             start: 12060605,    end: 12071116 ),
                new EraData( era: EraData.enmEra.承元, yomi: "じょうげん",           start: 12071116,    end: 12110423 ),
                new EraData( era: EraData.enmEra.建暦, yomi: "けんりゃく",           start: 12110423,    end: 12140118 ),
                new EraData( era: EraData.enmEra.建保, yomi: "けんぽう",             start: 12140118,    end: 12190527 ),
                new EraData( era: EraData.enmEra.承久, yomi: "じょうきゅう",         start: 12190527,    end: 12220525 ),
                new EraData( era: EraData.enmEra.貞応, yomi: "じょうおう",           start: 12220525,    end: 12241231 ),
                new EraData( era: EraData.enmEra.元仁, yomi: "げんにん",             start: 12241231,    end: 12250528 ),
                new EraData( era: EraData.enmEra.嘉禄, yomi: "かろく",               start: 12250528,    end: 12280118 ),
                new EraData( era: EraData.enmEra.安貞, yomi: "あんてい",             start: 12280118,    end: 12290331 ),
                new EraData( era: EraData.enmEra.寛喜, yomi: "かんき",               start: 12290331,    end: 12320423 ),
                new EraData( era: EraData.enmEra.貞永, yomi: "じょうえい",           start: 12320423,    end: 12330525 ),
                new EraData( era: EraData.enmEra.天福, yomi: "てんぷく",             start: 12330525,    end: 12341127 ),
                new EraData( era: EraData.enmEra.文暦, yomi: "ぶんりゃく",           start: 12341127,    end: 12351101 ),
                new EraData( era: EraData.enmEra.嘉禎, yomi: "かてい",               start: 12351101,    end: 12381230 ),
                new EraData( era: EraData.enmEra.暦仁, yomi: "りゃくにん",           start: 12381230,    end: 12390313 ),
                new EraData( era: EraData.enmEra.延応, yomi: "えんおう",             start: 12390313,    end: 12400805 ),
                new EraData( era: EraData.enmEra.仁治, yomi: "にんじ",               start: 12400805,    end: 12430318 ),
                new EraData( era: EraData.enmEra.寛元, yomi: "かんげん",             start: 12430318,    end: 12470405 ),
                new EraData( era: EraData.enmEra.宝治, yomi: "ほうじ",               start: 12470405,    end: 12490502 ),
                new EraData( era: EraData.enmEra.建長, yomi: "けんちょう",           start: 12490502,    end: 12561024 ),
                new EraData( era: EraData.enmEra.康元, yomi: "こうげん",             start: 12561024,    end: 12570331 ),
                new EraData( era: EraData.enmEra.正嘉, yomi: "しょうか",             start: 12570331,    end: 12590420 ),
                new EraData( era: EraData.enmEra.正元, yomi: "しょうげん",           start: 12590420,    end: 12600524 ),
                new EraData( era: EraData.enmEra.文応, yomi: "ぶんおう",             start: 12600524,    end: 12610322 ),
                new EraData( era: EraData.enmEra.弘長, yomi: "こうちょう",           start: 12610322,    end: 12640327 ),
                new EraData( era: EraData.enmEra.文永, yomi: "ぶんえい",             start: 12640327,    end: 12750522 ),
                new EraData( era: EraData.enmEra.建治, yomi: "けんじ",               start: 12750522,    end: 12780323 ),
                new EraData( era: EraData.enmEra.弘安, yomi: "こうあん",             start: 12780323,    end: 12880529 ),
                new EraData( era: EraData.enmEra.正応, yomi: "しょうおう",           start: 12880529,    end: 12930906 ),
                new EraData( era: EraData.enmEra.永仁, yomi: "えいにん",             start: 12930906,    end: 12990525 ),
                new EraData( era: EraData.enmEra.正安, yomi: "しょうあん",           start: 12990525,    end: 13021210 ),
                new EraData( era: EraData.enmEra.乾元, yomi: "けんげん",             start: 13021210,    end: 13030916 ),
                new EraData( era: EraData.enmEra.嘉元, yomi: "かげん",               start: 13030916,    end: 13070118 ),
                new EraData( era: EraData.enmEra.徳治, yomi: "とくじ",               start: 13070118,    end: 13081122 ),
                new EraData( era: EraData.enmEra.延慶, yomi: "えんきょう",           start: 13081122,    end: 13110517 ),
                new EraData( era: EraData.enmEra.応長, yomi: "おうちょう",           start: 13110517,    end: 13120427 ),
                new EraData( era: EraData.enmEra.正和, yomi: "しょうわ",             start: 13120427,    end: 13170316 ),
                new EraData( era: EraData.enmEra.文保, yomi: "ぶんぽう",             start: 13170316,    end: 13190518 ),
                new EraData( era: EraData.enmEra.元応, yomi: "げんおう",             start: 13190518,    end: 13210322 ),
                new EraData( era: EraData.enmEra.元亨, yomi: "げんこう",             start: 13210322,    end: 13241225 ),
                new EraData( era: EraData.enmEra.正中, yomi: "しょうちゅう",         start: 13241225,    end: 13260528 ),
                new EraData( era: EraData.enmEra.嘉暦, yomi: "かりゃく",             start: 13260528,    end: 13290922 ),
                new EraData( era: EraData.enmEra.元徳, yomi: "げんとく",             start: 13290922,    end: 13310911 ),
                new EraData( era: EraData.enmEra.元弘, yomi: "げんこう",             start: 13310911,    end: 13340305 ),
                new EraData( era: EraData.enmEra.正慶, yomi: "しょうきょう",         start: 13320523,    end: 13330707 ),
                new EraData( era: EraData.enmEra.建武, yomi: "けんむ",               start: 13340305,    end: 13360411 ),
                new EraData( era: EraData.enmEra.延元, yomi: "えんげん",             start: 13360411,    end: 13400525 ),
                new EraData( era: EraData.enmEra.興国, yomi: "こうこく",             start: 13400525,    end: 13470120 ),
                new EraData( era: EraData.enmEra.正平, yomi: "しょうへい",           start: 13470120,    end: 13700816 ),
                new EraData( era: EraData.enmEra.建徳, yomi: "けんとく",             start: 13700816,    end: 13720504 ),
                new EraData( era: EraData.enmEra.文中, yomi: "ぶんちゅう",           start: 13720504,    end: 13750626 ),
                new EraData( era: EraData.enmEra.天授, yomi: "てんじゅ",             start: 13750626,    end: 13810306 ),
                new EraData( era: EraData.enmEra.弘和, yomi: "こうわ",               start: 13810306,    end: 13840518 ),
                new EraData( era: EraData.enmEra.元中, yomi: "げんちゅう",           start: 13840518,    end: 13921119 ),
                new EraData( era: EraData.enmEra.暦応, yomi: "りゃくおう",           start: 13381011,    end: 13420601 ),
                new EraData( era: EraData.enmEra.康永, yomi: "こうえい",             start: 13420601,    end: 13451115 ),
                new EraData( era: EraData.enmEra.貞和, yomi: "じょうわ",             start: 13451115,    end: 13500404 ),
                new EraData( era: EraData.enmEra.観応, yomi: "かんのう",             start: 13500404,    end: 13521104 ),
                new EraData( era: EraData.enmEra.文和, yomi: "ぶんな",               start: 13521104,    end: 13560429 ),
                new EraData( era: EraData.enmEra.延文, yomi: "えんぶん",             start: 13560429,    end: 13610504 ),
                new EraData( era: EraData.enmEra.康安, yomi: "こうあん",             start: 13610504,    end: 13621011 ),
                new EraData( era: EraData.enmEra.貞治, yomi: "じょうじ",             start: 13621011,    end: 13680307 ),
                new EraData( era: EraData.enmEra.応安, yomi: "おうあん",             start: 13680307,    end: 13750329 ),
                new EraData( era: EraData.enmEra.永和, yomi: "えいわ",               start: 13750329,    end: 13790409 ),
                new EraData( era: EraData.enmEra.康暦, yomi: "こうりゃく",           start: 13790409,    end: 13810320 ),
                new EraData( era: EraData.enmEra.永徳, yomi: "えいとく",             start: 13810320,    end: 13840319 ),
                new EraData( era: EraData.enmEra.至徳, yomi: "しとく",               start: 13840319,    end: 13871005 ),
                new EraData( era: EraData.enmEra.嘉慶, yomi: "かきょう",             start: 13871005,    end: 13890307 ),
                new EraData( era: EraData.enmEra.康応, yomi: "こうおう",             start: 13890307,    end: 13900412 ),
                new EraData( era: EraData.enmEra.明徳, yomi: "めいとく",             start: 13900412,    end: 13940802 ),
                new EraData( era: EraData.enmEra.応永, yomi: "おうえい",             start: 13940802,    end: 14280610 ),
                new EraData( era: EraData.enmEra.正長, yomi: "しょうちょう",         start: 14280610,    end: 14291003 ),
                new EraData( era: EraData.enmEra.永享, yomi: "えいきょう",           start: 14291003,    end: 14410310 ),
                new EraData( era: EraData.enmEra.嘉吉, yomi: "かきつ",               start: 14410310,    end: 14440223 ),
                new EraData( era: EraData.enmEra.文安, yomi: "ぶんあん",             start: 14440223,    end: 14490816 ),
                new EraData( era: EraData.enmEra.宝徳, yomi: "ほうとく",             start: 14490816,    end: 14520810 ),
                new EraData( era: EraData.enmEra.享徳, yomi: "きょうとく",           start: 14520810,    end: 14550906 ),
                new EraData( era: EraData.enmEra.康正, yomi: "こうしょう",           start: 14550906,    end: 14571016 ),
                new EraData( era: EraData.enmEra.長禄, yomi: "ちょうろく",           start: 14571016,    end: 14610201 ),
                new EraData( era: EraData.enmEra.寛正, yomi: "かんしょう",           start: 14610201,    end: 14660314 ),
                new EraData( era: EraData.enmEra.文正, yomi: "ぶんしょう",           start: 14660314,    end: 14670409 ),
                new EraData( era: EraData.enmEra.応仁, yomi: "おうにん",             start: 14670409,    end: 14690608 ),
                new EraData( era: EraData.enmEra.文明, yomi: "ぶんめい",             start: 14690608,    end: 14870809 ),
                new EraData( era: EraData.enmEra.長享, yomi: "ちょうきょう",         start: 14870809,    end: 14890916 ),
                new EraData( era: EraData.enmEra.延徳, yomi: "えんとく",             start: 14890916,    end: 14920812 ),
                new EraData( era: EraData.enmEra.明応, yomi: "めいおう",             start: 14920812,    end: 15010318 ),
                new EraData( era: EraData.enmEra.文亀, yomi: "ぶんき",               start: 15010318,    end: 15040316 ),
                new EraData( era: EraData.enmEra.永正, yomi: "えいしょう",           start: 15040316,    end: 15210923 ),
                new EraData( era: EraData.enmEra.大永, yomi: "だいえい",             start: 15210923,    end: 15280903 ),
                new EraData( era: EraData.enmEra.享禄, yomi: "きょうろく",           start: 15280903,    end: 15320829 ),
                new EraData( era: EraData.enmEra.天文, yomi: "てんぶん",             start: 15320829,    end: 15551107 ),
                new EraData( era: EraData.enmEra.弘治, yomi: "こうじ",               start: 15551107,    end: 15580318 ),
                new EraData( era: EraData.enmEra.永禄, yomi: "えいろく",             start: 15580318,    end: 15700527 ),
                new EraData( era: EraData.enmEra.元亀, yomi: "げんき",               start: 15700527,    end: 15730825 ),
                new EraData( era: EraData.enmEra.天正, yomi: "てんしょう",           start: 15730825,    end: 15930110 ),
                new EraData( era: EraData.enmEra.文禄, yomi: "ぶんろく",             start: 15930110,    end: 15961216 ),
                new EraData( era: EraData.enmEra.慶長, yomi: "けいちょう",           start: 15961216,    end: 16150905 ),
                new EraData( era: EraData.enmEra.元和, yomi: "げんな",               start: 16150905,    end: 16240417 ),
                new EraData( era: EraData.enmEra.寛永, yomi: "かんえい",             start: 16240417,    end: 16450113 ),
                new EraData( era: EraData.enmEra.正保, yomi: "しょうほう",           start: 16450113,    end: 16480407 ),
                new EraData( era: EraData.enmEra.慶安, yomi: "けいあん",             start: 16480407,    end: 16521020 ),
                new EraData( era: EraData.enmEra.承応, yomi: "じょうおう",           start: 16521020,    end: 16550518 ),
                new EraData( era: EraData.enmEra.明暦, yomi: "めいれき",             start: 16550518,    end: 16580821 ),
                new EraData( era: EraData.enmEra.万治, yomi: "まんじ",               start: 16580821,    end: 16610523 ),
                new EraData( era: EraData.enmEra.寛文, yomi: "かんぶん",             start: 16610523,    end: 16731030 ),
                new EraData( era: EraData.enmEra.延宝, yomi: "えんぽう",             start: 16731030,    end: 16811109 ),
                new EraData( era: EraData.enmEra.天和, yomi: "てんな",               start: 16811109,    end: 16840405 ),
                new EraData( era: EraData.enmEra.貞享, yomi: "じょうきょう",         start: 16840405,    end: 16881023 ),
                new EraData( era: EraData.enmEra.元禄, yomi: "げんろく",             start: 16881023,    end: 17040416 ),
                new EraData( era: EraData.enmEra.宝永, yomi: "ほうえい",             start: 17040416,    end: 17110611 ),
                new EraData( era: EraData.enmEra.正徳, yomi: "しょうとく",           start: 17110611,    end: 17160809 ),
                new EraData( era: EraData.enmEra.享保, yomi: "きょうほう",           start: 17160809,    end: 17360607 ),
                new EraData( era: EraData.enmEra.元文, yomi: "げんぶん",             start: 17360607,    end: 17410412 ),
                new EraData( era: EraData.enmEra.寛保, yomi: "かんぽう",             start: 17410412,    end: 17440403 ),
                new EraData( era: EraData.enmEra.延享, yomi: "えんきょう",           start: 17440403,    end: 17480805 ),
                new EraData( era: EraData.enmEra.寛延, yomi: "かんえん",             start: 17480805,    end: 17511214 ),
                new EraData( era: EraData.enmEra.宝暦, yomi: "ほうれき",             start: 17511214,    end: 17640630 ),
                new EraData( era: EraData.enmEra.明和, yomi: "めいわ",               start: 17640630,    end: 17721210 ),
                new EraData( era: EraData.enmEra.安永, yomi: "あんえい",             start: 17721210,    end: 17810425 ),
                new EraData( era: EraData.enmEra.天明, yomi: "てんめい",             start: 17810425,    end: 17890219 ),
                new EraData( era: EraData.enmEra.寛政, yomi: "かんせい",             start: 17890219,    end: 18010319 ),
                new EraData( era: EraData.enmEra.享和, yomi: "きょうわ",             start: 18010319,    end: 18040322 ),
                new EraData( era: EraData.enmEra.文化, yomi: "ぶんか",               start: 18040322,    end: 18180526 ),
                new EraData( era: EraData.enmEra.文政, yomi: "ぶんせい",             start: 18180526,    end: 18310123 ),
                new EraData( era: EraData.enmEra.天保, yomi: "てんぽう",             start: 18310123,    end: 18450109 ),
                new EraData( era: EraData.enmEra.弘化, yomi: "こうか",               start: 18450109,    end: 18480401 ),
                new EraData( era: EraData.enmEra.嘉永, yomi: "かえい",               start: 18480401,    end: 18550115 ),
                new EraData( era: EraData.enmEra.安政, yomi: "あんせい",             start: 18550115,    end: 18600408 ),
                new EraData( era: EraData.enmEra.万延, yomi: "まんえん",             start: 18600408,    end: 18610329 ),
                new EraData( era: EraData.enmEra.文久, yomi: "ぶんきゅう",           start: 18610329,    end: 18640327 ),
                new EraData( era: EraData.enmEra.元治, yomi: "げんじ",               start: 18640327,    end: 18650501 ),
                new EraData( era: EraData.enmEra.慶応, yomi: "けいおう",             start: 18650501,    end: 18681023 ),
                new EraData( era: EraData.enmEra.明治, yomi: "めいじ",               start: 18680123,    end: 19120729 ),
                new EraData( era: EraData.enmEra.大正, yomi: "たいしょう",           start: 19120730,    end: 19261224 ),
                new EraData( era: EraData.enmEra.昭和, yomi: "しょうわ",             start: 19261225,    end: 19890107 ),
                new EraData( era: EraData.enmEra.平成, yomi: "へいせい",             start: 19890108,    end: 20190430 ),
                new EraData( era: EraData.enmEra.令和, yomi: "れいわ",               start: 20190501,    end: 20991231 ),
                new EraData( era: EraData.enmEra.追加_0, yomi: "追加０",             start: 30000101,    end: 30000101 ),
                new EraData( era: EraData.enmEra.追加_1, yomi: "追加１",             start: 30000101,    end: 30000101 ),
                new EraData( era: EraData.enmEra.追加_2, yomi: "追加２",             start: 30000101,    end: 30000101 ),
                new EraData( era: EraData.enmEra.追加_3, yomi: "追加３",             start: 30000101,    end: 30000101 ),
                new EraData( era: EraData.enmEra.追加_4, yomi: "追加４",             start: 30000101,    end: 30000101 ),
                new EraData( era: EraData.enmEra.追加_5, yomi: "追加５",             start: 30000101,    end: 30000101 ),
                new EraData( era: EraData.enmEra.追加_6, yomi: "追加６",             start: 30000101,    end: 30000101 ),
                new EraData( era: EraData.enmEra.追加_7, yomi: "追加７",             start: 30000101,    end: 30000101 ),
                new EraData( era: EraData.enmEra.追加_8, yomi: "追加８",             start: 30000101,    end: 30000101 ),
                new EraData( era: EraData.enmEra.追加_9, yomi: "追加９",             start: 30000101,    end: 30000101 ),
            };
            #endregion 固定値

            #region メンバ変数
            /*
            /// <summary>
            /// 最終アクセスEraData
            /// </summary>
            private LastAccessEraData _LastAccessData = new LastAccessEraData();
            */
            #endregion メンバ変数

            #region プロパティ
            /// <summary>
            /// 元号データ取得
            /// </summary>
            /// <param name="en">元号</param>
            /// <returns>結果</returns>
            private EraData this [EraData.enmEra en]
            {
                get { return this[(int)en]; }
            }
            /// <summary>
            /// 元号データ取得
            /// </summary>
            /// <param name="index">インデックス</param>
            /// <returns>結果</returns>
            private EraData this [int index]
            {
                get { return this._EraDataList[index]; }
            }
            /// <summary>
            /// 元号データリスト取得
            /// </summary>
            public IEnumerable<EraData> EraDataList
            {
                get { return this._EraDataList; }
            }
            /// <summary>
            /// 元号名リスト取得
            /// </summary>
            public IEnumerable<string> EraNameList
            {
                get
                {
                    return this._EraDataList
                        .Select(x => x.Name);
                }
            }

            #endregion プロパティ

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public EraInfomation()
            {
                /*
                // 本日のデータを取得しラストアクセスデータに保存する
                this._LastAccessData.Ed = 
                    this.GetEraData(
                        dt: this._LastAccessData.Dt
                        );
                */
            }
            #endregion コンストラクタ

            #region 非公開メソッド

            #region 元号名または読みからEraDataを取得する
            /// <summary>
            /// 元号名または読みからEraDataを取得する
            /// 複数発見の場合は初めの一件を対象とする
            /// 最終アクセスデータ非対象
            /// 検索件数が0件の場合はnullを返す
            /// </summary>
            /// <param name="nameOrYomi">元号名または読み</param>
            /// <returns>結果</returns>
            private EraData GetEraData(
                string nameOrYomi
                )
            {
                // 最後にアクセスしたEraDataと一致していた場合は
                // 最後にアクセスしたEraDataを返す
                //if (this._LastAccessData.CheckLastAccess(nameOrYomi:  nameOrYomi))
                //    return this._LastAccessData.Ed;

                // 元号データリスト取得
                var ret = this._EraDataList
                    .Where(x => x.Name == nameOrYomi || x.Yomi == nameOrYomi)
                    .FirstOrDefault();

                // 取得できなかった場合はnullを返す
                if (ret == null)
                    return null;

                // ラストアクセスデータとして保存する
                //this._LastAccessData.Dt = ret.StartDateTime;
                //this._LastAccessData.Before = false;
                //this._LastAccessData.Ed = ret;

                return ret;
            }
            #endregion 元号名または読みからEraDataを取得する

            #region DateTimeからEraDataを取得する
            /// <summary>
            /// DateTimeからEraDataを取得する
            /// 元号の切り替え年月日が重なる年号の場合で
            /// 前の元号が必要な場合はbefore=trueにする
            /// 最終アクセスデータと比較して同じ場合、最終アクセスデータを使う
            /// 一致しなかった場合、絞込後に最終アクセスデータに保存する
            /// 検索件数が0件の場合はnullを返す
            /// </summary>
            /// <param name="dt">年月日</param>
            /// <param name="before">前の元号名を取得する</param>
            /// <returns>結果</returns>
            private EraData GetEraData(
                DateTime dt,
                bool before = false
                )
            {
                // 範囲チェック
                if (dt.Year > 2999 || dt.Year < 645)
                    return null;

                // 最後にアクセスしたDateTimeと一致していた場合は
                // 最後にアクセスしたEraDataを返す
                //if (this._LastAccessData.CheckLastAccess(dt: dt, before: before))
                //    return this._LastAccessData.Ed;

                // 元号データリスト取得
                var list = this._EraDataList
                    .Where(x => x.StartDateTime <= dt && dt <= x.EndDateTime)
                    .ToList();

                // 件数が0件の場合はnullを返す
                if (list.Count < 1)
                    return null;

                // beforeによって処理を変える
                var ret = before
                    ? list.First()
                    : list.Last();

                // ラストアクセスデータとして保存する
                //this._LastAccessData.Dt = dt;
                //this._LastAccessData.Before = before;
                //this._LastAccessData.Ed = ret;

                // 結果を返す
                return ret;
            }
            #endregion DateTimeからEraDataを取得する

            #endregion 非公開メソッド

            #region 公開メソッド

            #region 元号名を取得する
            /// <summary>
            /// 元号名を取得する
            /// </summary>
            /// <param name="en">元号</param>
            /// <returns>結果</returns>
            public string GetName(
                EraData.enmEra en
                )
            {
                return this[en]
                    .Name;
            }
            #endregion 元号名を取得する

            #region 読みを取得する
            /// <summary>
            /// 読みを取得する
            /// </summary>
            /// <param name="en">元号</param>
            /// <returns>結果</returns>
            public string GetYomi(
                EraData.enmEra en
                )
            {
                return this[en]
                    .Yomi;
            }
            #endregion 読みを取得する

            #region 開始年月日を取得する
            /// <summary>
            /// 開始年月日を取得する
            /// </summary>
            /// <param name="en">元号</param>
            /// <returns>結果</returns>
            public DateTime GetStartDateTime(
                EraData.enmEra en
                )
            {
                return this[en]
                    .StartDateTime;
            }
            #endregion 開始年月日を取得する

            #region 終了年月日を取得する
            /// <summary>
            /// 終了年月日を取得する
            /// </summary>
            /// <param name="en">元号</param>
            /// <returns>結果</returns>
            public DateTime GetEndDateTime(
                EraData.enmEra en
                )
            {
                return this[en]
                    .EndDateTime;
            }
            #endregion 終了年月日を取得する

            #region 元号名または読みから元号名を取得する
            /// <summary>
            /// 元号名または読みから元号名を取得する
            /// 発見できなかった場合は空文字を返す
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <returns>結果</returns>
            public string GetEraNameFromNameOrYomi(
                string eraNameOrYomi
                )
            {
                // EraDataを取得する
                var ret = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 結果を返す
                return ret != null
                    ? ret.Name
                    : string.Empty;
            }
            #endregion 元号名または読みから元号名を取得する

            #region DateTimeから元号名を取得する
            /// <summary>
            /// DateTimeから元号名を取得する
            /// 元号の切り替え年月日が重なる年号の場合で
            /// 前の元号が必要な場合はbefore=trueにする
            /// 発見できなかった場合は空文字を返す
            /// </summary>
            /// <param name="dt">年月日</param>
            /// <param name="before">前の元号名を取得する</param>
            /// <returns>結果</returns>
            public string GetEraNameFromDateTime(
                DateTime dt,
                bool before = false
                )
            {
                // EraDataを取得する
                var ret = this.GetEraData(dt: dt, before: before);

                // 結果を返す
                return ret != null
                    ? ret.Name
                    : string.Empty;
            }
            #endregion DateTimeから元号名を取得する

            #region 元号名または読みから元号名の読みを取得する
            /// <summary>
            /// 元号名または読みから元号名の読みを取得する
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <returns>結果</returns>
            public string GetEraYomiFromNameOrYomi(
                string eraNameOrYomi
                )
            {
                // EraDataを取得する
                var ret = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 結果を返す
                return ret != null
                    ? ret.Yomi
                    : string.Empty;
            }
            #endregion 元号名または読みから元号名の読みを取得する

            #region DateTimeから元号名の読みを取得する
            /// <summary>
            /// DateTimeから元号名の読みを取得する
            /// 元号の切り替え年月日が重なる年号の場合で
            /// 前の元号が必要な場合はbefore=trueにする
            /// 発見できなかった場合は空文字を返す
            /// </summary>
            /// <param name="dt">年月日</param>
            /// <param name="before">前の元号名を取得する</param>
            /// <returns>結果</returns>
            public string GetEraYomiFromDateTime(
                DateTime dt,
                bool before = false
                )
            {
                // EraDataを取得する
                var ret = this.GetEraData(dt: dt, before: before);

                // 結果を返す
                return ret != null
                    ? ret.Yomi
                    : string.Empty;
            }
            #endregion DateTimeから元号名の読みを取得する

            #region 元号名または読みから元号の開始日(DateTime)を取得する
            /// <summary>
            /// 元号名または読みから元号の開始日(DateTime)を取得する
            /// 発見できなかった場合は日付の最大値を返す
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <returns>結果</returns>
            public DateTime GetStartDateTimeFromNameOrYomi(
                string eraNameOrYomi
                )
            {
                // EraDataを取得する
                var ret = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 結果を返す
                return ret != null
                    ? ret.StartDateTime
                    : this._ERROR_DATETIME;
            }
            #endregion 元号名から元号の開始日(DateTime)を取得する

            #region DateTimeから元号の開始日(DateTime)を取得する
            /// <summary>
            /// DateTimeから元号の開始日(DateTime)を西暦で取得する
            /// 元号の切り替え年月日が重なる年号の場合で
            /// 前の元号が必要な場合はbefore=trueにする
            /// 発見できなかった場合は日付の最大値を返す
            /// </summary>
            /// <param name="dt">年月日</param>
            /// <param name="before">前の元号名を取得する</param>
            /// <returns>結果</returns>
            public DateTime GetStartDateTimeFromDateTime(
                DateTime dt,
                bool before = false
                )
            {
                // EraDataを取得する
                var ret = this.GetEraData(dt: dt, before: before);

                // 結果を返す
                return ret != null
                    ? ret.StartDateTime
                    : this._ERROR_DATETIME;
            }
            #endregion DateTimeから元号の開始日(DateTime)を取得する

            #region 元号名または読みから元号の終了日(DateTime)を取得する
            /// <summary>
            /// 元号名または読みから元号の終了日(DateTime)を取得する
            /// 発見できなかった場合は日付の最大値を返す
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <returns>結果</returns>
            public DateTime GetEndDateTimeFromNameOrYomi(
                string eraNameOrYomi
                )
            {
                // EraDataを取得する
                var ret = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 結果を返す
                return ret != null
                    ? ret.EndDateTime
                    : this._ERROR_DATETIME;
            }
            #endregion 元号名または読みから元号の終了日(DateTime)を取得する

            #region DateTimeから元号の終了日(DateTime)を取得する
            /// <summary>
            /// DateTimeから元号の終了日(DateTime)を取得する
            /// 元号の切り替え年月日が重なる年号の場合で
            /// 前の元号が必要な場合はbefore=trueにする
            /// 発見できなかった場合は日付の最大値を返す
            /// </summary>
            /// <param name="dt">年月日</param>
            /// <param name="before">前の元号名を取得する</param>
            /// <returns>結果</returns>
            public DateTime GetEndDateTimeFromDateTime(
                DateTime dt,
                bool before = false
                )
            {
                // EraDataを取得する
                var ret = this.GetEraData(dt: dt, before: before);

                // 結果を返す
                return ret != null
                    ? ret.EndDateTime
                    : this._ERROR_DATETIME;
            }
            #endregion DateTimeから元号の終了日(DateTime)を取得する

            #region 追加EraDataを変更する
            /// <summary>
            /// 追加EraDataを変更する
            /// 成功した場合trueが返る
            /// </summary>
            /// <param name="addedEraNo">追加元号番号</param>
            /// <param name="name">元号名</param>
            /// <param name="yomi">読み</param>
            /// <param name="startDateTime">開始年月日</param>
            /// <param name="endDateTime">終了年月日</param>
            /// <returns>結果</returns>
            public bool ChangeAddedEraData(
                int addedEraNo,
                string name,
                string yomi,
                DateTime startDateTime,
                DateTime endDateTime
                )
            {
                // 値を変更する
                return this._EraDataList[this.GetAddedEraIndex(addedEraNo: addedEraNo)]
                    .ChangeEraData(
                        name: name,
                        yomi: yomi,
                        startDateTime: startDateTime,
                        endDateTime: endDateTime
                        );
            }
            /// <summary>
            /// 追加EraDataを変更する
            /// </summary>
            /// <param name="addedEraNo">追加元号番号</param>
            /// <param name="name">元号名</param>
            /// <param name="yomi">読み</param>
            /// <param name="start">開始年月日数値</param>
            /// <param name="end">終了年月日数値</param>
            public void ChangeAddedEraData(
                int addedEraNo,
                string name,
                string yomi,
                int start,
                int end
                )
            {
                // 値を変更する
                this._EraDataList[this.GetAddedEraIndex(addedEraNo: addedEraNo)]
                    .ChangeEraData(
                        name: name,
                        yomi: yomi,
                        start: start,
                        end: end
                        );
            }
            /// <summary>
            /// 元号名または読みから追加EraDataを変更する
            /// 追加用元号名は、追加0, 追加1, ..., 追加9
            /// 再変更の場合は変更した元号名とする
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <param name="name">元号名</param>
            /// <param name="yomi">読み</param>
            /// <param name="startDateTime">開始日</param>
            /// <param name="endDateTime">終了日</param>
            public void ChangeAddedEraData(
                string eraNameOrYomi,
                string name,
                string yomi,
                DateTime startDateTime,
                DateTime endDateTime
                )
            {
                // EraDataを取得する
                var ed = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 値を変更する
                ed.ChangeEraData(
                    name: name,
                    yomi: yomi,
                    startDateTime: startDateTime,
                    endDateTime: endDateTime
                    );
            }
            /// <summary>
            /// 元号名または読みから追加EraDataを変更する
            /// enmEraは、追加0-9でなければならない
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <param name="name">元号名</param>
            /// <param name="yomi">読み</param>
            /// <param name="start">開始年月日数値</param>
            /// <param name="end">終了年月日数値</param>
            public void ChangeAddedEraData(
                string eraNameOrYomi,
                string name,
                string yomi,
                int start,
                int end
                )
            {
                // EraDataを取得する
                var ed = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 値を変更する
                ed.ChangeEraData(
                    name: name,
                    yomi: yomi,
                    start: start,
                    end: end
                    );
            }
            #endregion 追加EraDataを変更する

            #region 元号名または読みから元号名を修正する
            /// <summary>
            /// 元号名または読みからEraDataを変更する
            /// 発見できなかった場合はなにもしない
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <param name="name">元号名</param>
            /// <returns>結果</returns>
            public bool ModifyEraName(
                string eraNameOrYomi,
                string name
                )
            {
                // EraDataを取得する
                var ed = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 未発見時は何もしない
                if (ed == null) return false;

                // 元号名を変更する
                if (ed.Name != name)
                    return ed.ModifyName(name: name);

                return false;
            }
            #endregion 元号名または読みから元号名を修正する

            #region 元号名または読みから読みを修正する
            /// <summary>
            /// 元号名または読みから読みを変更する
            /// 発見できなかった場合はなにもしない
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <param name="yomi">読み</param>
            /// <returns>結果</returns>
            public bool ModifyEraYomi(
                string eraNameOrYomi,
                string yomi
                )
            {
                // EraDataを取得する
                var ed = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 未発見時は何もしない
                if (ed == null) return false;

                // 元号名を変更する
                if (ed.Yomi != yomi)
                    return ed.ModifyYomi(yomi: yomi);

                return false;
            }
            #endregion 元号名または読みから元号名を修正する

            #region 元号名または読みから開始年月日を修正する
            /// <summary>
            /// 元号名または読みから読みを変更する
            /// 発見できなかった場合はfalseを返します
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <param name="dt">開始年月日数値</param>
            /// <returns>結果</returns>
            public bool ModifyEraStartDate(
                string eraNameOrYomi,
                DateTime dt
                )
            {
                // EraDataを取得する
                var ed = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 未発見時は何もしない
                if (ed == null) return false;

                // 元号名を変更する
                if (ed.StartDateTime != dt)
                {
                    return ed.ModifyStartDate(dt: dt);
                }

                return false;
            }
            /// <summary>
            /// 元号名または読みから読みを変更する
            /// 発見できなかった場合はなにもしない
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <param name="start">開始年月日数値</param>
            public void ModifyEraStartDate(
                string eraNameOrYomi,
                int start
                )
            {
                // EraDataを取得する
                var ed = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 未発見時は何もしない
                if (ed == null) return;

                // 元号名を変更する
                if (ed.Start != start)
                    ed.ModifyStartDate(date: start);
            }
            #endregion 元号名または読みから元号名を修正する

            #region 元号名または読みから終了年月日を修正する
            /// <summary>
            /// 元号名または読みから読みを変更する
            /// 発見できなかった場合はなにもしない
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <param name="dt">開始年月日</param>
            /// <returns>結果</returns>
            public bool ModifyEraEndDate(
                string eraNameOrYomi,
                DateTime dt
                )
            {
                // EraDataを取得する
                var ed = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 未発見時は何もしない
                if (ed == null) return false;

                // 元号名を変更する
                if (ed.EndDateTime != dt)
                    return ed.ModifyEndDate(dt: dt);

                return false;
            }
            /// <summary>
            /// 元号名または読みから読みを変更する
            /// 発見できなかった場合はなにもしない
            /// </summary>
            /// <param name="eraNameOrYomi">元号名または読み</param>
            /// <param name="end">開始年月日数値</param>
            public void ModifyEraEndDate(
                string eraNameOrYomi,
                int end
                )
            {
                // EraDataを取得する
                var ed = this.GetEraData(nameOrYomi: eraNameOrYomi);

                // 未発見時は何もしない
                if (ed == null) return;

                // 元号名を変更する
                if (ed.End != end)
                    ed.ModifyEndDate(date: end);
            }
            #endregion 元号名または読みから元号名を修正する

            #endregion 公開メソッド

            #region メソッド

            #region 追加元号のインデックスを取得する
            /// <summary>
            /// 追加元号のインデックスを取得する
            /// </summary>
            /// <param name="addedEraNo">追加元号番号</param>
            /// <returns>結果</returns>
            public int GetAddedEraIndex(
                int addedEraNo
                )
            {
                // 新元号番号チェック
                if (addedEraNo < 0 || addedEraNo > 9)
                    throw new ArgumentOutOfRangeException($"追加元号番号({addedEraNo})は0-9の範囲でなければなりません");

                return (int)EraData.enmEra.追加_0 + addedEraNo;
            }
            #endregion 追加元号のインデックスを取得する

            #endregion メソッド

            /*
            #region 最終アクセス元号データクラス
            /// <summary>
            /// 最終アクセス元号データクラス
            /// </summary>
            private class LastAccessEraData
            {
                #region プロパティ
                /// <summary>
                /// 最後にアクセスしたDateTime
                /// </summary>
                public DateTime Dt { get; set; } = DateTime.Now;
                /// <summary>
                /// 最後にアクセスしたbefore
                /// </summary>
                public bool Before { get; set; } = false;
                /// <summary>
                /// 最後にアクセスしたEraData
                /// </summary>
                public EraData Ed { get; set; } = null;
                #endregion プロパティ

                #region メソッド

                #region ラストアクセスとデータと一致しているか調べる
                /// <summary>
                /// 元号または読みがラストデータと一致しているか調べる
                /// 一致している場合true
                /// </summary>
                /// <param name="nameOrYomi">元号または読み</param>
                /// <returns>結果</returns>
                public bool CheckLastAccess(
                    string nameOrYomi
                    )
                {
                    return
                        this.Ed.Name == nameOrYomi
                        || this.Ed.Yomi == nameOrYomi;
                }
                /// <summary>
                /// 年月日がラストアクセスデータと一致しているか調べる
                /// 一致している場合true
                /// </summary>
                /// <param name="dt">DateTime</param>
                /// <param name="before">before</param>
                /// <returns>結果</returns>
                public bool CheckLastAccess(
                    DateTime dt,
                    bool before
                    )
                {
                    // ラストアクセスと一致しているか調べる
                    return
                        dt == this.Dt
                        && before == this.Before;
                }
                #endregion ラストアクセスと一致しているか調べる

                #endregion メソッド
            }
            #endregion 最終アクセス元号データクラス
            */

            #region 元号データクラス
            /// <summary>
            /// 元号データ
            /// </summary>
            public class EraData
            {
                #region 固定値
                /// <summary>
                /// YYYYMMDDフォーマット
                /// </summary>
                private const string FORMAT_YYMMDD = "{0:D4}{1:D2}{2:D2}";
                /// <summary>
                /// ユリウス暦開始日(紀元前45年1月1日)
                /// 紀元前が扱えない為、西暦1年01月01日からとする
                /// </summary>
                private readonly DateTime _START_JULIAN_CALENDER =
                    new DateTime(
                        year: 1,
                        month: 1,
                        day: 1
                        );
                /// <summary>
                /// グレゴリオ暦開始日(1582年10月15日)
                /// </summary>
                private readonly DateTime _START_GREGORIAN_CALENDER =
                    new DateTime(
                        year: 1582,
                        month: 10,
                        day: 15
                        );
                #endregion 固定値

                #region 元号
                /// <summary>
                /// 元号
                /// グレゴリオ暦開始日以前はユリウス暦を使用します
                /// グレゴリオ暦開始日以降はグレゴリオ暦を使用します
                /// </summary>
                public enum enmEra : int
                {
                    /// <summary>
                    /// 大化	たいか	645年07月17日	650年03月22日
                    /// </summary>
                    大化 = 0,
                    /// <summary>
                    /// 白雉	はくち	650年03月22日	654年11月24日
                    /// </summary>
                    白雉,
                    /// <summary>
                    /// 朱鳥	しゅちょう	686年08月14日	686年10月01日
                    /// </summary>
                    朱鳥,
                    /// <summary>
                    /// 大宝	たいほう	701年05月03日	704年06月16日
                    /// </summary>
                    大宝,
                    /// <summary>
                    /// 慶雲	けいうん	704年06月16日	708年02月07日
                    /// </summary>
                    慶雲,
                    /// <summary>
                    /// 和銅	わどう	708年02月07日	715年10月03日
                    /// </summary>
                    和銅,
                    /// <summary>
                    /// 霊亀	れいき	715年10月03日	717年12月24日
                    /// </summary>
                    霊亀,
                    /// <summary>
                    /// 養老	ようろう	717年12月24日	724年03月03日
                    /// </summary>
                    養老,
                    /// <summary>
                    /// 神亀	じんき	724年03月03日	729年09月02日
                    /// </summary>
                    神亀,
                    /// <summary>
                    /// 天平	てんぴょう	729年09月02日	749年05月04日
                    /// </summary>
                    天平,
                    /// <summary>
                    /// 天平感宝	てんぴょうかんぽう	749年05月04日	749年08月19日
                    /// </summary>
                    天平感宝,
                    /// <summary>
                    /// 天平勝宝	てんぴょうしょうほう	749年08月19日	757年09月06日
                    /// </summary>
                    天平勝宝,
                    /// <summary>
                    /// 天平宝字	てんぴょうほうじ	757年09月06日	765年02月01日
                    /// </summary>
                    天平宝字,
                    /// <summary>
                    /// 天平神護	てんぴょうじんご	765年02月01日	767年09月13日
                    /// </summary>
                    天平神護,
                    /// <summary>
                    /// 神護景雲	じんごけいうん	767年09月13日	770年10月23日
                    /// </summary>
                    神護景雲,
                    /// <summary>
                    /// 宝亀	ほうき	770年10月23日	781年01月30日
                    /// </summary>
                    宝亀,
                    /// <summary>
                    /// 天応	てんおう	781年01月30日	782年09月30日
                    /// </summary>
                    天応,
                    /// <summary>
                    /// 延暦	えんりゃく	782年09月30日	806年06月08日
                    /// </summary>
                    延暦,
                    /// <summary>
                    /// 大同	だいどう	806年06月08日	810年10月20日
                    /// </summary>
                    大同,
                    /// <summary>
                    /// 弘仁	こうにん	810年10月20日	824年02月08日
                    /// </summary>
                    弘仁,
                    /// <summary>
                    /// 天長	てんちょう	824年02月08日	834年02月14日
                    /// </summary>
                    天長,
                    /// <summary>
                    /// 承和	じょうわ	834年02月14日	848年07月16日
                    /// </summary>
                    承和,
                    /// <summary>
                    /// 嘉祥	かしょう	848年07月16日	851年06月01日
                    /// </summary>
                    嘉祥,
                    /// <summary>
                    /// 仁寿	にんじゅ	851年06月01日	854年12月23日
                    /// </summary>
                    仁寿,
                    /// <summary>
                    /// 斉衡	さいこう	854年12月23日	857年03月20日
                    /// </summary>
                    斉衡,
                    /// <summary>
                    /// 天安	てんあん	857年03月20日	859年05月20日
                    /// </summary>
                    天安,
                    /// <summary>
                    /// 貞観	じょうがん	859年05月20日	877年06月01日
                    /// </summary>
                    貞観,
                    /// <summary>
                    /// 元慶	がんぎょう	877年06月01日	885年03月11日
                    /// </summary>
                    元慶,
                    /// <summary>
                    /// 仁和	にんな	885年03月11日	889年05月30日
                    /// </summary>
                    仁和,
                    /// <summary>
                    /// 寛平	かんぴょう	889年05月30日	898年05月20日
                    /// </summary>
                    寛平,
                    /// <summary>
                    /// 昌泰	しょうたい	898年05月20日	901年08月31日
                    /// </summary>
                    昌泰,
                    /// <summary>
                    /// 延喜	えんぎ	901年08月31日	923年05月29日
                    /// </summary>
                    延喜,
                    /// <summary>
                    /// 延長	えんちょう	923年05月29日	931年05月16日
                    /// </summary>
                    延長,
                    /// <summary>
                    /// 承平	じょうへい	931年05月16日	938年06月22日
                    /// </summary>
                    承平,
                    /// <summary>
                    /// 天慶	てんぎょう	938年06月22日	947年05月15日
                    /// </summary>
                    天慶,
                    /// <summary>
                    /// 天暦	てんりゃく	947年05月15日	957年11月21日
                    /// </summary>
                    天暦,
                    /// <summary>
                    /// 天徳	てんとく	957年11月21日	961年03月05日
                    /// </summary>
                    天徳,
                    /// <summary>
                    /// 応和	おうわ	961年03月05日	964年08月19日
                    /// </summary>
                    応和,
                    /// <summary>
                    /// 康保	こうほう	964年08月19日	968年09月08日
                    /// </summary>
                    康保,
                    /// <summary>
                    /// 安和	あんな	968年09月08日	970年05月03日
                    /// </summary>
                    安和,
                    /// <summary>
                    /// 天禄	てんろく	970年05月03日	974年01月16日
                    /// </summary>
                    天禄,
                    /// <summary>
                    /// 天延	てんえん	974年01月16日	976年08月11日
                    /// </summary>
                    天延,
                    /// <summary>
                    /// 貞元	じょうげん	976年08月11日	978年12月31日
                    /// </summary>
                    貞元,
                    /// <summary>
                    /// 天元	てんげん	978年12月31日	983年05月29日
                    /// </summary>
                    天元,
                    /// <summary>
                    /// 永観	えいかん	983年05月29日	985年05月19日
                    /// </summary>
                    永観,
                    /// <summary>
                    /// 寛和	かんな	985年05月19日	987年05月05日
                    /// </summary>
                    寛和,
                    /// <summary>
                    /// 永延	えいえん	987年05月05日	989年09月10日
                    /// </summary>
                    永延,
                    /// <summary>
                    /// 永祚	えいそ	989年09月10日	990年11月26日
                    /// </summary>
                    永祚,
                    /// <summary>
                    /// 正暦	しょうりゃく	990年11月26日	995年03月25日
                    /// </summary>
                    正暦,
                    /// <summary>
                    /// 長徳	ちょうとく	995年03月25日	999年02月01日
                    /// </summary>
                    長徳,
                    /// <summary>
                    /// 長保	ちょうほう	999年02月01日	1004年08月08日
                    /// </summary>
                    長保,
                    /// <summary>
                    /// 寛弘	かんこう	1004年08月08日	1013年02月08日
                    /// </summary>
                    寛弘,
                    /// <summary>
                    /// 長和	ちょうわ	1013年02月08日	1017年05月21日
                    /// </summary>
                    長和,
                    /// <summary>
                    /// 寛仁	かんにん	1017年05月21日	1021年03月17日
                    /// </summary>
                    寛仁,
                    /// <summary>
                    /// 治安	じあん	1021年03月17日	1024年08月19日
                    /// </summary>
                    治安,
                    /// <summary>
                    /// 万寿	まんじゅ	1024年08月19日	1028年08月18日
                    /// </summary>
                    万寿,
                    /// <summary>
                    /// 長元	ちょうげん	1028年08月18日	1037年05月09日
                    /// </summary>
                    長元,
                    /// <summary>
                    /// 長暦	ちょうりゃく	1037年05月09日	1040年12月16日
                    /// </summary>
                    長暦,
                    /// <summary>
                    /// 長久	ちょうきゅう	1040年12月16日	1044年12月16日
                    /// </summary>
                    長久,
                    /// <summary>
                    /// 寛徳	かんとく	1044年12月16日	1046年05月22日
                    /// </summary>
                    寛徳,
                    /// <summary>
                    /// 永承	えいしょう	1046年05月22日	1053年02月02日
                    /// </summary>
                    永承,
                    /// <summary>
                    /// 天喜	てんき	1053年02月02日	1058年09月19日
                    /// </summary>
                    天喜,
                    /// <summary>
                    /// 康平	こうへい	1058年09月19日	1065年09月04日
                    /// </summary>
                    康平,
                    /// <summary>
                    /// 治暦	じりゃく	1065年09月04日	1069年05月06日
                    /// </summary>
                    治暦,
                    /// <summary>
                    /// 延久	えんきゅう	1069年05月06日	1074年09月16日
                    /// </summary>
                    延久,
                    /// <summary>
                    /// 承保	じょうほう	1074年09月16日	1077年12月05日
                    /// </summary>
                    承保,
                    /// <summary>
                    /// 承暦	じょうりゃく	1077年12月05日	1081年03月22日
                    /// </summary>
                    承暦,
                    /// <summary>
                    /// 永保	えいほう	1081年03月22日	1084年03月15日
                    /// </summary>
                    永保,
                    /// <summary>
                    /// 応徳	おうとく	1084年03月15日	1087年05月11日
                    /// </summary>
                    応徳,
                    /// <summary>
                    /// 寛治	かんじ	1087年05月11日	1095年01月23日
                    /// </summary>
                    寛治,
                    /// <summary>
                    /// 嘉保	かほう	1095年01月23日	1097年01月03日
                    /// </summary>
                    嘉保,
                    /// <summary>
                    /// 永長	えいちょう	1097年01月03日	1097年12月27日
                    /// </summary>
                    永長,
                    /// <summary>
                    /// 承徳	じょうとく	1097年12月27日	1099年09月15日
                    /// </summary>
                    承徳,
                    /// <summary>
                    /// 康和	こうわ	1099年09月15日	1104年03月08日
                    /// </summary>
                    康和,
                    /// <summary>
                    /// 長治	ちょうじ	1104年03月08日	1106年05月13日
                    /// </summary>
                    長治,
                    /// <summary>
                    /// 嘉承	かしょう	1106年05月13日	1108年09月09日
                    /// </summary>
                    嘉承,
                    /// <summary>
                    /// 天仁	てんにん	1108年09月09日	1110年07月31日
                    /// </summary>
                    天仁,
                    /// <summary>
                    /// 天永	てんえい	1110年07月31日	1113年08月25日
                    /// </summary>
                    天永,
                    /// <summary>
                    /// 永久	えいきゅう	1113年08月25日	1118年04月25日
                    /// </summary>
                    永久,
                    /// <summary>
                    /// 元永	げんえい	1118年04月25日	1120年05月09日
                    /// </summary>
                    元永,
                    /// <summary>
                    /// 保安	ほうあん	1120年05月09日	1124年05月18日
                    /// </summary>
                    保安,
                    /// <summary>
                    /// 天治	てんじ	1124年05月18日	1126年02月15日
                    /// </summary>
                    天治,
                    /// <summary>
                    /// 大治	だいじ	1126年02月15日	1131年02月28日
                    /// </summary>
                    大治,
                    /// <summary>
                    /// 天承	てんしょう	1131年02月28日	1132年09月21日
                    /// </summary>
                    天承,
                    /// <summary>
                    /// 長承	ちょうしょう	1132年09月21日	1135年06月10日
                    /// </summary>
                    長承,
                    /// <summary>
                    /// 保延	ほうえん	1135年06月10日	1141年08月13日
                    /// </summary>
                    保延,
                    /// <summary>
                    /// 永治	えいじ	1141年08月13日	1142年05月25日
                    /// </summary>
                    永治,
                    /// <summary>
                    /// 康治	こうじ	1142年05月25日	1144年03月28日
                    /// </summary>
                    康治,
                    /// <summary>
                    /// 天養	てんよう	1144年03月28日	1145年08月12日
                    /// </summary>
                    天養,
                    /// <summary>
                    /// 久安	きゅうあん	1145年08月12日	1151年02月14日
                    /// </summary>
                    久安,
                    /// <summary>
                    /// 仁平	にんぺい	1151年02月14日	1154年12月04日
                    /// </summary>
                    仁平,
                    /// <summary>
                    /// 久寿	きゅうじゅ	1154年12月04日	1156年05月18日
                    /// </summary>
                    久寿,
                    /// <summary>
                    /// 保元	ほうげん	1156年05月18日	1159年05月09日
                    /// </summary>
                    保元,
                    /// <summary>
                    /// 平治	へいじ	1159年05月09日	1160年02月18日
                    /// </summary>
                    平治,
                    /// <summary>
                    /// 永暦	えいりゃく	1160年02月18日	1161年09月24日
                    /// </summary>
                    永暦,
                    /// <summary>
                    /// 応保	おうほう	1161年09月24日	1163年05月04日
                    /// </summary>
                    応保,
                    /// <summary>
                    /// 長寛	ちょうかん	1163年05月04日	1165年07月14日
                    /// </summary>
                    長寛,
                    /// <summary>
                    /// 永万	えいまん	1165年07月14日	1166年09月23日
                    /// </summary>
                    永万,
                    /// <summary>
                    /// 仁安	にんあん	1166年09月23日	1169年05月06日
                    /// </summary>
                    仁安,
                    /// <summary>
                    /// 嘉応	かおう	1169年05月06日	1171年05月27日
                    /// </summary>
                    嘉応,
                    /// <summary>
                    /// 承安	しょうあん	1171年05月27日	1175年08月16日
                    /// </summary>
                    承安,
                    /// <summary>
                    /// 安元	あんげん	1175年08月16日	1177年08月29日
                    /// </summary>
                    安元,
                    /// <summary>
                    /// 治承	じしょう	1177年08月29日	1181年08月25日
                    /// </summary>
                    治承,
                    /// <summary>
                    /// 養和	ようわ	1181年08月25日	1182年06月29日
                    /// </summary>
                    養和,
                    /// <summary>
                    /// 寿永	じゅえい	1182年06月29日	1184年05月27日
                    /// </summary>
                    寿永,
                    /// <summary>
                    /// 元暦	げんりゃく	1184年05月27日	1185年09月09日
                    /// </summary>
                    元暦,
                    /// <summary>
                    /// 文治	ぶんじ	1185年09月09日	1190年05月16日
                    /// </summary>
                    文治,
                    /// <summary>
                    /// 建久	けんきゅう	1190年05月16日	1199年05月23日
                    /// </summary>
                    建久,
                    /// <summary>
                    /// 正治	しょうじ	1199年05月23日	1201年03月19日
                    /// </summary>
                    正治,
                    /// <summary>
                    /// 建仁	けんにん	1201年03月19日	1204年03月23日
                    /// </summary>
                    建仁,
                    /// <summary>
                    /// 元久	げんきゅう	1204年03月23日	1206年06月05日
                    /// </summary>
                    元久,
                    /// <summary>
                    /// 建永	けんえい	1206年06月05日	1207年11月16日
                    /// </summary>
                    建永,
                    /// <summary>
                    /// 承元	じょうげん	1207年11月16日	1211年04月23日
                    /// </summary>
                    承元,
                    /// <summary>
                    /// 建暦	けんりゃく	1211年04月23日	1214年01月18日
                    /// </summary>
                    建暦,
                    /// <summary>
                    /// 建保	けんぽう	1214年01月18日	1219年05月27日
                    /// </summary>
                    建保,
                    /// <summary>
                    /// 承久	じょうきゅう	1219年05月27日	1222年05月25日
                    /// </summary>
                    承久,
                    /// <summary>
                    /// 貞応	じょうおう	1222年05月25日	1224年12月31日
                    /// </summary>
                    貞応,
                    /// <summary>
                    /// 元仁	げんにん	1224年12月31日	1225年05月28日
                    /// </summary>
                    元仁,
                    /// <summary>
                    /// 嘉禄	かろく	1225年05月28日	1228年01月18日
                    /// </summary>
                    嘉禄,
                    /// <summary>
                    /// 安貞	あんてい	1228年01月18日	1229年03月31日
                    /// </summary>
                    安貞,
                    /// <summary>
                    /// 寛喜	かんき	1229年03月31日	1232年04月23日
                    /// </summary>
                    寛喜,
                    /// <summary>
                    /// 貞永	じょうえい	1232年04月23日	1233年05月25日
                    /// </summary>
                    貞永,
                    /// <summary>
                    /// 天福	てんぷく	1233年05月25日	1234年11月27日
                    /// </summary>
                    天福,
                    /// <summary>
                    /// 文暦	ぶんりゃく	1234年11月27日	1235年11月01日
                    /// </summary>
                    文暦,
                    /// <summary>
                    /// 嘉禎	かてい	1235年11月01日	1238年12月30日
                    /// </summary>
                    嘉禎,
                    /// <summary>
                    /// 暦仁	りゃくにん	1238年12月30日	1239年03月13日
                    /// </summary>
                    暦仁,
                    /// <summary>
                    /// 延応	えんおう	1239年03月13日	1240年08月05日
                    /// </summary>
                    延応,
                    /// <summary>
                    /// 仁治	にんじ	1240年08月05日	1243年03月18日
                    /// </summary>
                    仁治,
                    /// <summary>
                    /// 寛元	かんげん	1243年03月18日	1247年04月05日
                    /// </summary>
                    寛元,
                    /// <summary>
                    /// 宝治	ほうじ	1247年04月05日	1249年05月02日
                    /// </summary>
                    宝治,
                    /// <summary>
                    /// 建長	けんちょう	1249年05月02日	1256年10月24日
                    /// </summary>
                    建長,
                    /// <summary>
                    /// 康元	こうげん	1256年10月24日	1257年03月31日
                    /// </summary>
                    康元,
                    /// <summary>
                    /// 正嘉	しょうか	1257年03月31日	1259年04月20日
                    /// </summary>
                    正嘉,
                    /// <summary>
                    /// 正元	しょうげん	1259年04月20日	1260年05月24日
                    /// </summary>
                    正元,
                    /// <summary>
                    /// 文応	ぶんおう	1260年05月24日	1261年03月22日
                    /// </summary>
                    文応,
                    /// <summary>
                    /// 弘長	こうちょう	1261年03月22日	1264年03月27日
                    /// </summary>
                    弘長,
                    /// <summary>
                    /// 文永	ぶんえい	1264年03月27日	1275年05月22日
                    /// </summary>
                    文永,
                    /// <summary>
                    /// 建治	けんじ	1275年05月22日	1278年03月23日
                    /// </summary>
                    建治,
                    /// <summary>
                    /// 弘安	こうあん	1278年03月23日	1288年05月29日
                    /// </summary>
                    弘安,
                    /// <summary>
                    /// 正応	しょうおう	1288年05月29日	1293年09月06日
                    /// </summary>
                    正応,
                    /// <summary>
                    /// 永仁	えいにん	1293年09月06日	1299年05月25日
                    /// </summary>
                    永仁,
                    /// <summary>
                    /// 正安	しょうあん	1299年05月25日	1302年12月10日
                    /// </summary>
                    正安,
                    /// <summary>
                    /// 乾元	けんげん	1302年12月10日	1303年09月16日
                    /// </summary>
                    乾元,
                    /// <summary>
                    /// 嘉元	かげん	1303年09月16日	1307年01月18日
                    /// </summary>
                    嘉元,
                    /// <summary>
                    /// 徳治	とくじ	1307年01月18日	1308年11月22日
                    /// </summary>
                    徳治,
                    /// <summary>
                    /// 延慶	えんきょう	1308年11月22日	1311年05月17日
                    /// </summary>
                    延慶,
                    /// <summary>
                    /// 応長	おうちょう	1311年05月17日	1312年04月27日
                    /// </summary>
                    応長,
                    /// <summary>
                    /// 正和	しょうわ	1312年04月27日	1317年03月16日
                    /// </summary>
                    正和,
                    /// <summary>
                    /// 文保	ぶんぽう	1317年03月16日	1319年05月18日
                    /// </summary>
                    文保,
                    /// <summary>
                    /// 元応	げんおう	1319年05月18日	1321年03月22日
                    /// </summary>
                    元応,
                    /// <summary>
                    /// 元亨	げんこう	1321年03月22日	1324年12月25日
                    /// </summary>
                    元亨,
                    /// <summary>
                    /// 正中	しょうちゅう	1324年12月25日	1326年05月28日
                    /// </summary>
                    正中,
                    /// <summary>
                    /// 嘉暦	かりゃく	1326年05月28日	1329年09月22日
                    /// </summary>
                    嘉暦,
                    /// <summary>
                    /// 元徳	げんとく	1329年09月22日	1331年09月11日
                    /// </summary>
                    元徳,
                    /// <summary>
                    /// 元弘	げんこう	1331年09月11日	1334年03月05日
                    /// </summary>
                    元弘,
                    /// <summary>
                    /// 正慶	しょうきょう	1332年05月23日	1333年07月07日
                    /// </summary>
                    正慶,
                    /// <summary>
                    /// 建武	けんむ	1334年03月05日	1336年04月11日
                    /// </summary>
                    建武,
                    /// <summary>
                    /// 延元	えんげん	1336年04月11日	1340年05月25日
                    /// </summary>
                    延元,
                    /// <summary>
                    /// 興国	こうこく	1340年05月25日	1347年01月20日
                    /// </summary>
                    興国,
                    /// <summary>
                    /// 正平	しょうへい	1347年01月20日	1370年08月16日
                    /// </summary>
                    正平,
                    /// <summary>
                    /// 建徳	けんとく	1370年08月16日	1372年05月04日
                    /// </summary>
                    建徳,
                    /// <summary>
                    /// 文中	ぶんちゅう	1372年05月04日	1375年06月26日
                    /// </summary>
                    文中,
                    /// <summary>
                    /// 天授	てんじゅ	1375年06月26日	1381年03月06日
                    /// </summary>
                    天授,
                    /// <summary>
                    /// 弘和	こうわ	1381年03月06日	1384年05月18日
                    /// </summary>
                    弘和,
                    /// <summary>
                    /// 元中	げんちゅう	1384年05月18日	1392年11月19日
                    /// </summary>
                    元中,
                    /// <summary>
                    /// 暦応	りゃくおう	1338年10月11日	1342年06月01日
                    /// </summary>
                    暦応,
                    /// <summary>
                    /// 康永	こうえい	1342年06月01日	1345年11月15日
                    /// </summary>
                    康永,
                    /// <summary>
                    /// 貞和	じょうわ	1345年11月15日	1350年04月04日
                    /// </summary>
                    貞和,
                    /// <summary>
                    /// 観応	かんのう	1350年04月04日	1352年11月04日
                    /// </summary>
                    観応,
                    /// <summary>
                    /// 文和	ぶんな	1352年11月04日	1356年04月29日
                    /// </summary>
                    文和,
                    /// <summary>
                    /// 延文	えんぶん	1356年04月29日	1361年05月04日
                    /// </summary>
                    延文,
                    /// <summary>
                    /// 康安	こうあん	1361年05月04日	1362年10月11日
                    /// </summary>
                    康安,
                    /// <summary>
                    /// 貞治	じょうじ	1362年10月11日	1368年03月07日
                    /// </summary>
                    貞治,
                    /// <summary>
                    /// 応安	おうあん	1368年03月07日	1375年03月29日
                    /// </summary>
                    応安,
                    /// <summary>
                    /// 永和	えいわ	1375年03月29日	1379年04月09日
                    /// </summary>
                    永和,
                    /// <summary>
                    /// 康暦	こうりゃく	1379年04月09日	1381年03月20日
                    /// </summary>
                    康暦,
                    /// <summary>
                    /// 永徳	えいとく	1381年03月20日	1384年03月19日
                    /// </summary>
                    永徳,
                    /// <summary>
                    /// 至徳	しとく	1384年03月19日	1387年10月05日
                    /// </summary>
                    至徳,
                    /// <summary>
                    /// 嘉慶	かきょう	1387年10月05日	1389年03月07日
                    /// </summary>
                    嘉慶,
                    /// <summary>
                    /// 康応	こうおう	1389年03月07日	1390年04月12日
                    /// </summary>
                    康応,
                    /// <summary>
                    /// 明徳	めいとく	1390年04月12日	1394年08月02日
                    /// </summary>
                    明徳,
                    /// <summary>
                    /// 応永	おうえい	1394年08月02日	1428年06月10日
                    /// </summary>
                    応永,
                    /// <summary>
                    /// 正長	しょうちょう	1428年06月10日	1429年10月03日
                    /// </summary>
                    正長,
                    /// <summary>
                    /// 永享	えいきょう	1429年10月03日	1441年03月10日
                    /// </summary>
                    永享,
                    /// <summary>
                    /// 嘉吉	かきつ	1441年03月10日	1444年02月23日
                    /// </summary>
                    嘉吉,
                    /// <summary>
                    /// 文安	ぶんあん	1444年02月23日	1449年08月16日
                    /// </summary>
                    文安,
                    /// <summary>
                    /// 宝徳	ほうとく	1449年08月16日	1452年08月10日
                    /// </summary>
                    宝徳,
                    /// <summary>
                    /// 享徳	きょうとく	1452年08月10日	1455年09月06日
                    /// </summary>
                    享徳,
                    /// <summary>
                    /// 康正	こうしょう	1455年09月06日	1457年10月16日
                    /// </summary>
                    康正,
                    /// <summary>
                    /// 長禄	ちょうろく	1457年10月16日	1461年02月01日
                    /// </summary>
                    長禄,
                    /// <summary>
                    /// 寛正	かんしょう	1461年02月01日	1466年03月14日
                    /// </summary>
                    寛正,
                    /// <summary>
                    /// 文正	ぶんしょう	1466年03月14日	1467年04月09日
                    /// </summary>
                    文正,
                    /// <summary>
                    /// 応仁	おうにん	1467年04月09日	1469年06月08日
                    /// </summary>
                    応仁,
                    /// <summary>
                    /// 文明	ぶんめい	1469年06月08日	1487年08月09日
                    /// </summary>
                    文明,
                    /// <summary>
                    /// 長享	ちょうきょう	1487年08月09日	1489年09月16日
                    /// </summary>
                    長享,
                    /// <summary>
                    /// 延徳	えんとく	1489年09月16日	1492年08月12日
                    /// </summary>
                    延徳,
                    /// <summary>
                    /// 明応	めいおう	1492年08月12日	1501年03月18日
                    /// </summary>
                    明応,
                    /// <summary>
                    /// 文亀	ぶんき	1501年03月18日	1504年03月16日
                    /// </summary>
                    文亀,
                    /// <summary>
                    /// 永正	えいしょう	1504年03月16日	1521年09月23日
                    /// </summary>
                    永正,
                    /// <summary>
                    /// 大永	だいえい	1521年09月23日	1528年09月03日
                    /// </summary>
                    大永,
                    /// <summary>
                    /// 享禄	きょうろく	1528年09月03日	1532年08月29日
                    /// </summary>
                    享禄,
                    /// <summary>
                    /// 天文	てんぶん	1532年08月29日	1555年11月07日
                    /// </summary>
                    天文,
                    /// <summary>
                    /// 弘治	こうじ	1555年11月07日	1558年03月18日
                    /// </summary>
                    弘治,
                    /// <summary>
                    /// 永禄	えいろく	1558年03月18日	1570年05月27日
                    /// </summary>
                    永禄,
                    /// <summary>
                    /// 元亀	げんき	1570年05月27日	1573年08月25日
                    /// </summary>
                    元亀,
                    /// <summary>
                    /// 天正	てんしょう	1573年08月25日	1593年01月10日
                    /// </summary>
                    天正,
                    /// <summary>
                    /// 文禄	ぶんろく	1593年01月10日	1596年12月16日
                    /// </summary>
                    文禄,
                    /// <summary>
                    /// 慶長	けいちょう	1596年12月16日	1615年09月05日
                    /// </summary>
                    慶長,
                    /// <summary>
                    /// 元和	げんな	1615年09月05日	1624年04月17日
                    /// </summary>
                    元和,
                    /// <summary>
                    /// 寛永	かんえい	1624年04月17日	1645年01月13日
                    /// </summary>
                    寛永,
                    /// <summary>
                    /// 正保	しょうほう	1645年01月13日	1648年04月07日
                    /// </summary>
                    正保,
                    /// <summary>
                    /// 慶安	けいあん	1648年04月07日	1652年10月20日
                    /// </summary>
                    慶安,
                    /// <summary>
                    /// 承応	じょうおう	1652年10月20日	1655年05月18日
                    /// </summary>
                    承応,
                    /// <summary>
                    /// 明暦	めいれき	1655年05月18日	1658年08月21日
                    /// </summary>
                    明暦,
                    /// <summary>
                    /// 万治	まんじ	1658年08月21日	1661年05月23日
                    /// </summary>
                    万治,
                    /// <summary>
                    /// 寛文	かんぶん	1661年05月23日	1673年10月30日
                    /// </summary>
                    寛文,
                    /// <summary>
                    /// 延宝	えんぽう	1673年10月30日	1681年11月09日
                    /// </summary>
                    延宝,
                    /// <summary>
                    /// 天和	てんな	1681年11月09日	1684年04月05日
                    /// </summary>
                    天和,
                    /// <summary>
                    /// 貞享	じょうきょう	1684年04月05日	1688年10月23日
                    /// </summary>
                    貞享,
                    /// <summary>
                    /// 元禄	げんろく	1688年10月23日	1704年04月16日
                    /// </summary>
                    元禄,
                    /// <summary>
                    /// 宝永	ほうえい	1704年04月16日	1711年06月11日
                    /// </summary>
                    宝永,
                    /// <summary>
                    /// 正徳	しょうとく	1711年06月11日	1716年08月09日
                    /// </summary>
                    正徳,
                    /// <summary>
                    /// 享保	きょうほう	1716年08月09日	1736年06月07日
                    /// </summary>
                    享保,
                    /// <summary>
                    /// 元文	げんぶん	1736年06月07日	1741年04月12日
                    /// </summary>
                    元文,
                    /// <summary>
                    /// 寛保	かんぽう	1741年04月12日	1744年04月03日
                    /// </summary>
                    寛保,
                    /// <summary>
                    /// 延享	えんきょう	1744年04月03日	1748年08月05日
                    /// </summary>
                    延享,
                    /// <summary>
                    /// 寛延	かんえん	1748年08月05日	1751年12月14日
                    /// </summary>
                    寛延,
                    /// <summary>
                    /// 宝暦	ほうれき	1751年12月14日	1764年06月30日
                    /// </summary>
                    宝暦,
                    /// <summary>
                    /// 明和	めいわ	1764年06月30日	1772年12月10日
                    /// </summary>
                    明和,
                    /// <summary>
                    /// 安永	あんえい	1772年12月10日	1781年04月25日
                    /// </summary>
                    安永,
                    /// <summary>
                    /// 天明	てんめい	1781年04月25日	1789年02月19日
                    /// </summary>
                    天明,
                    /// <summary>
                    /// 寛政	かんせい	1789年02月19日	1801年03月19日
                    /// </summary>
                    寛政,
                    /// <summary>
                    /// 享和	きょうわ	1801年03月19日	1804年03月22日
                    /// </summary>
                    享和,
                    /// <summary>
                    /// 文化	ぶんか	1804年03月22日	1818年05月26日
                    /// </summary>
                    文化,
                    /// <summary>
                    /// 文政	ぶんせい	1818年05月26日	1831年01月23日
                    /// </summary>
                    文政,
                    /// <summary>
                    /// 天保	てんぽう	1831年01月23日	1845年01月09日
                    /// </summary>
                    天保,
                    /// <summary>
                    /// 弘化	こうか	1845年01月09日	1848年04月01日
                    /// </summary>
                    弘化,
                    /// <summary>
                    /// 嘉永	かえい	1848年04月01日	1855年01月15日
                    /// </summary>
                    嘉永,
                    /// <summary>
                    /// 安政	あんせい	1855年01月15日	1860年04月08日
                    /// </summary>
                    安政,
                    /// <summary>
                    /// 万延	まんえん	1860年04月08日	1861年03月29日
                    /// </summary>
                    万延,
                    /// <summary>
                    /// 文久	ぶんきゅう	1861年03月29日	1864年03月27日
                    /// </summary>
                    文久,
                    /// <summary>
                    /// 元治	げんじ	1864年03月27日	1865年05月01日
                    /// </summary>
                    元治,
                    /// <summary>
                    /// 慶応	けいおう	1865年05月01日	1868年10月23日
                    /// </summary>
                    慶応,
                    /// <summary>
                    /// 明治	めいじ	1868年01月23日	1912年07月29日
                    /// </summary>
                    明治,
                    /// <summary>
                    /// 大正	たいしょう	1912年07月30日	1926年12月24日
                    /// </summary>
                    大正,
                    /// <summary>
                    /// 昭和	しょうわ	1926年12月25日	1989年01月07日
                    /// </summary>
                    昭和,
                    /// <summary>
                    /// 平成	へいせい	1989年01月08日	2019年04月30日
                    /// </summary>
                    平成,
                    /// <summary>
                    /// 令和    れいわ　　　2019年05月01日　3000年01月01日
                    /// </summary>
                    令和,
                    /// <summary>
                    /// 追加_0	追加０	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_0,
                    /// <summary>
                    /// 追加_1	追加１	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_1,
                    /// <summary>
                    /// 追加_2	追加２	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_2,
                    /// <summary>
                    /// 追加_3	追加３	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_3,
                    /// <summary>
                    /// 追加_4	追加４	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_4,
                    /// <summary>
                    /// 追加_5	追加５	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_5,
                    /// <summary>
                    /// 追加_6	追加６	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_6,
                    /// <summary>
                    /// 追加_7	追加７	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_7,
                    /// <summary>
                    /// 追加_8	追加８	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_8,
                    /// <summary>
                    /// 追加_9	追加９	3000年01月01日	3000年01月01日
                    /// </summary>
                    追加_9,
                }
                #endregion 元号

                #region メンバ変数
                /// <summary>
                /// 元号
                /// </summary>
                private enmEra _Era;
                /// <summary>
                /// 名前
                /// </summary>
                private string _Name;
                /// <summary>
                /// 読み
                /// </summary>
                private string _Yomi;
                /// <summary>
                /// 開始年月日
                /// </summary>
                private DateTime _StartDateTime;
                /// <summary>
                /// 終了年月日
                /// </summary>
                private DateTime _EndDateTime;
                #endregion メンバ変数

                #region プロパティ
                /// <summary>
                /// 元号
                /// </summary>
                public enmEra Era
                {
                    get { return this._Era; }
                    private set { this._Era = value; }
                }
                /// <summary>
                /// 名前
                /// </summary>
                public string Name
                {
                    get { return this._Name; }
                    private set { this._Name = value; }
                }
                /// <summary>
                /// 読み
                /// </summary>
                public string Yomi
                {
                    get { return this._Yomi; }
                    private set { this._Yomi = value; }
                }
                /// <summary>
                /// 開始年月日
                /// グレゴリオ暦開始日以前はユリウス暦を使用します
                /// グレゴリオ暦開始日以降はグレゴリオ暦を使用します
                /// </summary>
                public DateTime StartDateTime
                {
                    get { return this._StartDateTime; }
                    private set { this._StartDateTime = value; }
                }
                /// <summary>
                /// 終了年月日
                /// グレゴリオ暦開始日以前はユリウス暦を使用します
                /// グレゴリオ暦開始日以降はグレゴリオ暦を使用します
                /// </summary>
                public DateTime EndDateTime
                {
                    get { return this._EndDateTime; }
                    private set { this._EndDateTime = value; }
                }
                /// <summary>
                /// 開始年月日数値(YYYYMMDD)
                /// グレゴリオ暦開始日以前はユリウス暦を使用します
                /// グレゴリオ暦開始日以降はグレゴリオ暦を使用します
                /// </summary>
                public int Start
                {
                    get
                    {
                        return string.Format(
                            FORMAT_YYMMDD,
                            this._StartDateTime.Year,
                            this._StartDateTime.Month,
                            this._StartDateTime.Day
                            ).intParse();
                    }
                    private set
                    {
                        this._StartDateTime = this.ConvertDateTime(value);
                    }
                }
                /// <summary>
                /// 終了年月日数値(YYYYMMDD)
                /// グレゴリオ暦開始日以前はユリウス暦を使用します
                /// グレゴリオ暦開始日以降はグレゴリオ暦を使用します
                /// </summary>
                public int End
                {
                    get
                    {
                        // 終了年月日を数値に変換する
                        return string.Format(
                            FORMAT_YYMMDD,
                            this._EndDateTime.Year,
                            this._EndDateTime.Month,
                            this._EndDateTime.Day
                            ).intParse();
                    }
                    private set
                    {
                        // 終了年月日を設定する
                        this._EndDateTime = this.ConvertDateTime(value);
                    }
                }
                /// <summary>
                /// ユリウス暦開始日
                /// </summary>
                public DateTime START_JULIAN_CALENDER
                {
                    get { return _START_JULIAN_CALENDER; }
                }
                /// <summary>
                /// グレゴリオ暦開始日
                /// </summary>
                public DateTime START_GREGORIAN_CALENDER
                {
                    get { return _START_GREGORIAN_CALENDER; }
                }
                #endregion プロパティ

                #region コンストラクタ
                /// <summary>
                /// コンストラクタ
                /// 開始年月日数値と終了年月日数値はYYYYMMDD形式の数値
                /// グレゴリオ暦開始日以前はユリウス暦を使用します
                /// グレゴリオ暦開始日以降はグレゴリオ暦を使用します
                /// </summary>
                /// <param name="era">元号</param>
                /// <param name="yomi">よみ</param>
                /// <param name="start">開始年月日数値(YYYYMMDD)</param>
                /// <param name="end">終了年月日数値(YYYYMMDD)</param>
                public EraData(
                    enmEra era,
                    string yomi,
                    int start,
                    int end
                    )
                {
                    this.Era = era;
                    this.Name = era.ToString();
                    this.Yomi = yomi;
                    this.Start= start;
                    this.End = end;
                }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="era">元号</param>
                /// <param name="yomi">よみ</param>
                /// <param name="start">開始年月日</param>
                /// <param name="end">終了年月日</param>
                public EraData(
                    enmEra era,
                    string yomi,
                    DateTime start,
                    DateTime end
                    )
                {
                    this.Era = era;
                    this.Name = era.ToString();
                    this.Yomi = yomi;
                    this.StartDateTime = start;
                    this.EndDateTime = end;
                }
                #endregion コンストラクタ

                #region メソッド

                #region YYYYMMDDをDateTimeに変換する
                /// <summary>
                /// YYYYMMDDをDateTimeに変換する
                /// 年月日数値はYYYYMMDD形式の数値
                /// </summary>
                /// <param name="datetime">年月日数値</param>
                /// <returns>結果</returns>
                private DateTime ConvertDateTime(
                    int datetime
                    )
                {
                    // 範囲外は例外にする
                    // 9999年12月31日以降～1年01月01日
                    if (datetime > 99991231 || datetime < 10101)
                        throw new ArgumentOutOfRangeException();

                    // 年を取得する
                    var y = datetime / 10000;
                    if (y < 1 || y > 9999)
                        throw new ArgumentOutOfRangeException();

                    // 月を取得する
                    var m = (datetime - y * 10000) / 100;
                    if (m < 1 || m > 12)
                        throw new ArgumentOutOfRangeException();

                    // 日を取得する
                    var d = (datetime - y * 10000 - m * 100);
                    if (d < 1 || d > 31)
                        throw new ArgumentOutOfRangeException();

                    // 結果を返す
                    return new DateTime(
                        year: y,
                        month: m,
                        day: d
                        );
                }
                #endregion YYYYMMDDをDateTimeに変換する

                #region 追加データを変更する
                /// <summary>
                /// 追加データを変更する
                /// グレゴリオ暦開始日以前はユリウス暦を使用します
                /// グレゴリオ暦開始日以降はグレゴリオ暦を使用します
                /// 成功した場合trueが返る
                /// </summary>
                /// <param name="name">元号名</param>
                /// <param name="yomi">読み</param>
                /// <param name="startDateTime">開始年月日</param>
                /// <param name="endDateTime">終了年月日</param>
                /// <returns>結果</returns>
                public bool ChangeEraData(
                    string name,
                    string yomi,
                    DateTime startDateTime,
                    DateTime endDateTime
                    )
                {
                    // 引数チェック
                    if (this._Era < enmEra.追加_0 || this._Era > enmEra.追加_9)
                        throw new ArgumentOutOfRangeException("追加0-9以外は変更できません");

                    // データを変更する
                    this.Name = name;
                    this.Yomi = yomi;
                    this.StartDateTime = startDateTime;
                    this.EndDateTime = endDateTime;

                    return true;
                }
                /// <summary>
                /// 追加データを変更する
                /// 開始年月日数値と終了年月日数値はYYYYMMDD形式の数値
                /// グレゴリオ暦開始日以前はユリウス暦を使用します
                /// グレゴリオ暦開始日以降はグレゴリオ暦を使用します
                /// </summary>
                /// <param name="name">元号名</param>
                /// <param name="yomi">読み</param>
                /// <param name="start">開始年月日数値</param>
                /// <param name="end">終了年月日数値</param>
                public void ChangeEraData(
                    string name,
                    string yomi,
                    int start,
                    int end
                    )
                {
                    // 引数チェック
                    if (this._Era < enmEra.追加_0 || this._Era > enmEra.追加_9)
                        throw new ArgumentOutOfRangeException("追加0-9以外は変更できません");

                    // データを変更する
                    this.Name = name;
                    this.Yomi = yomi;
                    this.Start = start;
                    this.End = end;
                }
                #endregion 追加データを変更する

                #region 元号名を修正する
                /// <summary>
                /// 元号名を修正する
                /// </summary>
                /// <param name="name">元号名</param>
                /// <returns>結果</returns>
                public bool ModifyName(
                    string name
                    )
                {
                    this.Name = Name;
                    return true;
                }
                #endregion 元号名を修正する

                #region 読みを修正する
                /// <summary>
                /// 読みを修正する
                /// </summary>
                /// <param name="yomi">読み</param>
                /// <returns>結果</returns>
                public bool ModifyYomi(
                    string yomi
                    )
                {
                    this.Yomi = yomi;
                    return true;
                }
                #endregion 読みを修正する

                #region 開始年月日を修正する
                /// <summary>
                /// 開始年月日を修正する
                /// </summary>
                /// <param name="dt">DateTime</param>
                /// <returns>結果</returns>
                public bool ModifyStartDate(
                    DateTime dt
                    )
                {
                    this.StartDateTime = dt;
                    return true;
                }
                /// <summary>
                /// 開始年月日を修正する
                /// </summary>
                /// <param name="date">DateTime</param>
                /// <returns>結果</returns>
                public bool ModifyStartDate(
                    int date
                    )
                {
                    this.Start = date;
                    return true;
                }
                #endregion 開始年月日を変更する

                #region 終了年月日を修正する
                /// <summary>
                /// 終了年月日を修正する
                /// </summary>
                /// <param name="dt">DateTime</param>
                /// <returns>結果</returns>
                public bool ModifyEndDate(
                    DateTime dt
                    )
                {
                    this.EndDateTime = dt;
                    return true;
                }
                /// <summary>
                /// 終了年月日を修正する
                /// </summary>
                /// <param name="date">DateTime</param>
                /// <returns>結果</returns>
                public bool ModifyEndDate(
                    int date
                    )
                {
                    this.End = date;
                    return true;
                }
                #endregion 終了年月日を変更する

                #endregion メソッド

                #region ユリウス暦か調べる
                /// <summary>
                /// ユリウス暦か調べる
                /// 紀元前が扱えない為、西暦1年01月01日からとする
                /// ユリウス暦の場合true
                /// </summary>
                /// <param name="dt">年月日</param>
                /// <returns>結果</returns>
                public bool CheckJulianCalendar(
                    DateTime dt
                    )
                {
                    // グレゴリオ暦開始年月日以前
                    return dt < this._START_GREGORIAN_CALENDER;
                }
                #endregion ユリウス暦か調べる

                #region グレゴリオ暦か調べる
                /// <summary>
                /// グレゴリオ暦か調べる
                /// グレゴリオ暦の場合true
                /// </summary>
                /// <param name="dt">年月日</param>
                /// <returns>結果</returns>
                public bool CheckGregorianCalender(
                    DateTime dt
                    )
                {
                    // グレゴリオ暦開始年月日以降
                    return dt >= this._START_GREGORIAN_CALENDER;
                }
                #endregion グレゴリオ暦か調べる
            }
            #endregion 元号データクラス
        }
        #endregion 元号情報クラス

        #region カルチャー情報クラス
        /// <summary>
        /// カルチャー情報クラス
        /// </summary>
        private class Culture
        {
            #region メンバ変数
            /// <summary>
            /// カルチャー情報
            /// </summary>
            private CultureInfo _CultureInfo = null;
            /// <summary>
            /// フォーマット
            /// </summary>
            private string _Format = string.Empty;
            #endregion メンバ変数

            #region プロパティ
            /// <summary>
            /// カルチャー情報
            /// </summary>
            public CultureInfo Info
            {
                get { return this._CultureInfo; }
            }
            /// <summary>
            /// フォーマット
            /// </summary>
            public string Format
            {
                get { return this._Format; }
                set { this._Format = value; }
            }
            #endregion プロパティ

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="ci">カルチャー情報</param>
            /// <param name="format">フォーマット</param>
            internal Culture(
                CultureInfo ci,
                string format
                )
            {
                this._CultureInfo = ci;
                this._Format = format;
            }
            #endregion コンストラクタ
        }
        #endregion カルチャー情報クラス
    }
    #endregion SmallBasic用日付
}
