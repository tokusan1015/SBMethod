using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SBMethod
{
    #region 休日情報クラス
    /// <summary>
    /// 休日情報クラス
    /// </summary>
    internal class HolidayInfo
    {
        #region 列挙体
        /// <summary>
        /// 休日種類
        /// </summary>
        public enum enmHolidayKind : int
        {
            /// <summary>
            /// 平日
            /// </summary>
            平日 = 0,
            /// <summary>
            /// 祝日
            /// </summary>
            国民の祝日,
            /// <summary>
            /// 振替休日
            /// </summary>
            振替休日,
            /// <summary>
            /// 祝日と祝日の間の日
            /// </summary>
            国民の休日,
        }
        #endregion 列挙体

        #region プロパティ
        /// <summary>
        /// 日付
        /// </summary>
        public DateTime _Date { set; get; } = DateTime.Now;
        /// <summary>
        /// 種類
        /// </summary>
        public enmHolidayKind _HolidayKind { set; get; } = enmHolidayKind.国民の祝日;
        /// <summary>
        /// パラメタ(-1:春分,-2:秋分,0:固定,1-n:第n週月曜)
        /// </summary>
        public int _Param { set; get; } = 0;
        /// <summary>
        /// 適用開始年
        /// </summary>
        public int _StartApplyYear { set; get; } = 0;
        /// <summary>
        /// 適用終了年(-1はチェックしない)
        /// </summary>
        public int _EndApplyYear { set; get; } = 0;
        /// <summary>
        /// 名称
        /// </summary>
        public string _Name { get; set; } = string.Empty;
        /// <summary>
        /// 祝日の定義
        /// </summary>
        public string _Definition { get; set; } = string.Empty;
        #endregion プロパティ
    }
    #endregion 休日情報クラス

    #region 休日クラス
    /// <summary>
    /// 休日クラス
    /// </summary>
    internal class ClassHoliday
    {
        #region 固定値

        #region メッセージ
        /// <summary>
        /// 対象年メッセージ
        /// </summary>
        private const string TARGET_DATE_MESSAGE = @"休日対応可能な年は{0}年から{1}年までです。";
        #endregion メッセージ

        #region その他
        /// <summary>
        /// 祝日を計算する年(下限値)
        /// </summary>
        public const int MIN_YEAR = 2000;
        /// <summary>
        /// 祝日を計算する年(上限値)
        /// </summary>
        public const int MAX_YEAR = 2099;
        /// <summary>
        /// 登録標準年(意味なし)
        /// </summary>
        private const int DEFAULT_YEAR = 2017;
        #endregion その他

        #region 休日
        /// <summary>
        /// 休日配列
        /// </summary>
        private List<HolidayInfo> _HolidayInfo = new List<HolidayInfo>
        {
            // 元日(1948～)
            // 元日：1月1日 年のはじめを祝う。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 1, 1),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1948,
                _EndApplyYear = MAX_YEAR,
                _Name = "元日",
                _Definition = "1月1日"
            },
            // 成人の日(1948年～1999年)
            // 成人の日：1月15日 おとなになったことを自覚し、みずから生き抜こうとする青年を祝いはげます。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 1, 15),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1948,
                _EndApplyYear = 1999,
                _Name = "成人の日",
                _Definition = "1月15日"
            },
            // 成人の日(2000年～)
            // 成人の日：1月の第2月曜 おとなになったことを自覚し、みずから生き抜こうとする青年を祝いはげます。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 1, 15),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 2,
                _StartApplyYear = 2000,
                _EndApplyYear = MAX_YEAR,
                _Name = "成人の日",
                _Definition = "1月の第2月曜"
            },
            // 建国記念の日(1948年～)
            // 建国記念の日：2月11日 政令で定める日 建国をしのび、国を愛する心を養う。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 2, 11),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1948,
                _EndApplyYear = MAX_YEAR,
                _Name = "建国記念の日",
                _Definition = "2月11日"
            },
            // 春分の日(閣議決定：1948年～)
            // 春分の日：3月19日～22日 自然をたたえ、生物をいつくしむ。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 3, 19),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = -1,
                _StartApplyYear = 1948,
                _EndApplyYear = MAX_YEAR,
                _Name = "春分の日",
                _Definition = "3月19日～22日のいずれか"
            },
            // 天皇誕生日(1948年～1988年)
            // 天皇誕生日：4月29日 昭和天皇誕生日
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 4, 29),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1948,
                _EndApplyYear = 1988,
                _Name = "天皇誕生日",
                _Definition = "4月29日"
            },
            // みどりの日(1989年～2006年)
            // みどりの日：4月29日 自然に親しむとともにその恩恵に感謝し、豊かな心をはぐくむ。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 4, 29),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1989,
                _EndApplyYear = 2006,
                _Name = "みどりの日",
                _Definition = "4月29日"
            },
            // 昭和の日(2007年～)
            // 昭和の日：4月29日 激動の日々を経て、復興を遂げた昭和の時代を顧み、国の将来に思いをいたす。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 4, 29),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 2007,
                _EndApplyYear = MAX_YEAR,
                _Name = "昭和の日",
                _Definition = "4月29日"
            },
            // 憲法記念日(1948年～)
            // 憲法記念日：5月3日 日本国憲法の施行を記念し、国の成長を期する。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 5, 3),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1948,
                _EndApplyYear = MAX_YEAR,
                _Name = "憲法記念日",
                _Definition = "5月3日"
            },
            // みどりの日(2007年～)
            // みどりの日：5月4日 自然に親しむとともにその恩恵に感謝し、豊かな心をはぐくむ。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 5, 4),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 2007,
                _EndApplyYear = MAX_YEAR,
                _Name = "みどりの日",
                _Definition = "5月4日"
            },
            // こどもの日(1948年～)
            // こどもの日：5月5日 こどもの人格を重んじ、こどもの幸福をはかるとともに、母に感謝する。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 5, 5),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1948,
                _EndApplyYear = MAX_YEAR,
                _Name = "こどもの日",
                _Definition = "5月5日"
            },
            // 海の日(1996年～2002年)
            // 海の日：7月20日 海の恩恵に感謝するとともに、海洋国日本の繁栄を願う。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 7, 20),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1996,
                _EndApplyYear = 2002,
                _Name = "海の日",
                _Definition = "7月20日"
            },
            // 海の日(2003年～)
            // 海の日：7月の第3月曜日 海の恩恵に感謝するとともに、海洋国日本の繁栄を願う。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 7, 20),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 3,
                _StartApplyYear = 2003,
                _EndApplyYear = MAX_YEAR,
                _Name = "海の日",
                _Definition = "7月の第3月曜"
            },
            // 山の日(2016年～)
            // 山の日：8月11日 山に親しむ機会を得て、山の恩恵に感謝する。(2016年より) 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 8, 11),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 2016,
                _EndApplyYear = MAX_YEAR,
                _Name = "山の日",
                _Definition = "8月11日"
            },
            // 敬老の日(1966年～2002年)
            // 敬老の日：9月15日 多年にわたり社会につくしてきた老人を敬愛し、長寿を祝う。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 9, 15),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1966,
                _EndApplyYear = 2002,
                _Name = "山の日",
                _Definition = "9月15日"
            },
            // 敬老の日(2003年～)
            // 敬老の日：9月の第3月曜日 多年にわたり社会につくしてきた老人を敬愛し、長寿を祝う。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 9, 15),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 3,
                _StartApplyYear = 2003,
                _EndApplyYear = MAX_YEAR,
                _Name = "敬老の日",
                _Definition = "9月の第3月曜日"
            },
            // 秋分の日(1948年～)
            // 秋分の日： 9月22日～24日 祖先をうやまい、なくなった人々をしのぶ。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 9, 22),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = -2,
                _StartApplyYear = 1948,
                _EndApplyYear = MAX_YEAR,
                _Name = "秋分の日",
                _Definition = "9月22日～24日のいずれか"
            },
            // 体育の日(1966年～1999年)
            // 体育の日：10月の第2月曜日 スポーツにしたしみ、健康な心身をつちかう。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 10, 10),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1966,
                _EndApplyYear = 1999,
                _Name = "体育の日",
                _Definition = "10月10日"
            },
            // 体育の日(2000年～)
            // 体育の日：10月10日 スポーツにしたしみ、健康な心身をつちかう。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 10, 10),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 2,
                _StartApplyYear = 2000,
                _EndApplyYear = MAX_YEAR,
                _Name = "体育の日",
                _Definition = "10月の第2月曜日"
            },
            // 文化の日(1948年～)
            // 文化の日：11月3日 自由と平和を愛し、文化をすすめる。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 11, 3),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1948,
                _EndApplyYear = MAX_YEAR,
                _Name = "文化の日",
                _Definition = "11月3日"
            },
            // 勤労感謝の日(1948年～)
            // 勤労感謝の日：11月23日 勤労をたっとび、生産を祝い、国民たがいに感謝しあう。 
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 11, 23),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1948,
                _EndApplyYear = MAX_YEAR,
                _Name = "勤労感謝の日",
                _Definition = "11月23日"
            },
            // 平成天皇誕生日(1989年～)
            // 天皇誕生日：12月23日
            new HolidayInfo()
            {
                _Date = new DateTime(DEFAULT_YEAR, 12, 23),
                _HolidayKind = HolidayInfo.enmHolidayKind.国民の祝日,
                _Param = 0,
                _StartApplyYear = 1989,
                _EndApplyYear = MAX_YEAR,
                _Name = "天皇誕生日",
                _Definition = "12月23日"
            },
        };
        #endregion 休日

        #endregion 固定値

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClassHoliday()
        {
            ;
        }
        #endregion コンストラクタ

        #region 公開メソッド

        #region 祝祭日を追加する
        /// <summary>
        /// 祝祭日を追加する
        /// パラメタ
        ///  -2   : 秋分の日
        ///  -1   : 春分の日
        ///   0   : 月日固定
        ///   1-n : 第n週月曜日
        /// 休日終了年：最大値(2099)を設定する
        /// </summary>
        /// <param name="dt">月日指定(年は意味なし)</param>
        /// <param name="hk">休日分類</param>
        /// <param name="param">パラメタ</param>
        /// <param name="startApplyYear">休日開始年</param>
        /// <param name="endApplyYear">休日終了年</param>
        /// <param name="name">祝日の名称</param>
        /// <param name="definition">祝日の定義</param>
        public void AddHolidayInfo(
            DateTime dt,
            HolidayInfo.enmHolidayKind hk,
            int param,
            int startApplyYear,
            int endApplyYear,
            string name,
            string definition
            )
        {
            this.AddHolidayInfo(new HolidayInfo
            {
                _Date = dt,
                _HolidayKind = hk,
                _Param = param,
                _StartApplyYear = startApplyYear,
                _EndApplyYear = endApplyYear,
                _Name = name,
                _Definition = definition
            });
        }
        /// <summary>
        /// 休日を追加する
        /// </summary>
        /// <param name="hi">休日情報</param>
        public void AddHolidayInfo(
            HolidayInfo hi
            )
        {
            this._HolidayInfo.Add(
                item: hi
                );
        }
        #endregion 祝日を追加する

        #region 祝祭日情報を取得する
        /// <summary>
        /// 祝祭日情報を取得する
        /// DateTimeの月日のみ使用する
        /// 発見できなかった場合はnullを返す
        /// </summary>
        /// <param name="dt">月日</param>
        /// <returns>結果</returns>
        public HolidayInfo GetHolidayInfo(
            DateTime dt
            )
        {
            return this._HolidayInfo
                .Where(x => x._Date.Month == dt.Month && x._Date.Day == dt.Day)
                .FirstOrDefault();
        }
        /// <summary>
        /// 祝祭日情報を取得する
        /// 発見できなかった場合はnullを返す
        /// </summary>
        /// <param name="name">休日名</param>
        /// <returns>結果</returns>
        public HolidayInfo GetHolidayInfo(
            string name
            )
        {
            return this._HolidayInfo
                .Where(x => x._Name == name)
                .FirstOrDefault();
        }
        #endregion 祝祭日情報を取得する

        #region 対象年が有効か調べる
        /// <summary>
        /// 対象年が有効な場合はtrue
        /// </summary>
        /// <param name="iYear">対象年</param>
        /// <returns>結果</returns>
        public bool CheckMinMaxYear(
            int iYear
            )
        {
            return iYear >= MIN_YEAR && MAX_YEAR >= iYear;
        }
        #endregion 対象年が有効か調べる

        #region year年の全ての祝日を取得する
        /// <summary>
        /// 対象年の全ての祝日を取得する
        /// 有効範囲 : 2000年～2099年
        /// 有効範囲外の場合は空の祝祭日一覧を返す
        /// </summary>
        /// <param name="iYear">対象年</param>
        /// <returns>祝祭日一覧</returns>
        public SortedDictionary<DateTime, HolidayInfo> GetAllHoliday(
            int iYear
            )
        {
            // 祝日を格納するコレクション
            var holidays = new SortedDictionary<DateTime, HolidayInfo>();

            // 対象年が有効範囲外の場合は空の祝祭日一覧を返す
            if (!this.CheckMinMaxYear(iYear))
                return holidays;

            // 休日情報配列から休日カレンダーを生成する
            for (int i = 0; i < this._HolidayInfo.Count; i++)
            {
                // 休日情報を取得
                HolidayInfo hi = this._HolidayInfo[i];
                // 対象年内かチェックする
                if (hi._StartApplyYear <= iYear && hi._EndApplyYear >= iYear)
                {
                    // 対象年内なら所定の計算方法を使用して処理する

                    // 1．日付が固定の祝日(_Param == 0)
                    if (hi._Param == 0)
                    {
                        // 固定祝日を取得
                        hi._Date = new DateTime(
                            year: iYear,
                            month: hi._Date.Month,
                            day: hi._Date.Day
                            );
                    }
                    // 2. 日付けが「○月第△月曜日」のパターン(_Param > 0)
                    else if (hi._Param > 0)
                    {
                        // 〇月△週月曜日祝日を取得
                        hi._Date = GetNthMonday(
                            iNthMonday: hi._Param,
                            iYear: iYear,
                            iMonth: hi._Date.Month
                            );
                    }
                    // 3. 春分の日(_Param == -1)
                    else if (hi._Param == -1)
                    {
                        // 春分の日を取得
                        hi._Date = CalcVernalEquinoxDay(iYear: iYear);
                    }
                    // 4. 秋分の日(_Param == -2)
                    else if (hi._Param == -2)
                    {
                        // 秋分の日を取得
                        hi._Date = CalcAutumnalEquinoxDay(iYear: iYear);
                    }
                    else
                    {
                        // その他の場合は例外処理
                        throw new ArgumentOutOfRangeException();
                    }
                    // 休日に追加
                    holidays.Add(key: hi._Date, value: hi);
                }
            }

            // 5. 振替休日(1973年～)
            // 国民の祝日に関する法律 第3条2
            //「国民の祝日」が日曜日に当たるときは、その日後において
            // その日に最も近い「国民の祝日」でない日を休日とする。
            var substituteHolidays = GetSubstituteHolidays(sdHolidays: holidays);
            foreach (var s in substituteHolidays)
                holidays.Add(key: s._Date, value: s);

            // 6. 国民の休日(1985年～)
            // 国民の祝日に関する法律 第3条3
            // その前日及び翌日が「国民の祝日」である日（「国民の祝日」でない日に限る。）は、休日とする。
            var sandwichedHolidays = GetSandwichedHolidays(sdHolidays: holidays);
            foreach (var s in sandwichedHolidays)
                holidays.Add(key: s._Date, value: s);

            return holidays;
        }
        #endregion year年の全ての祝日を取得する

        #endregion 公開メソッド

        #region メソッド

        #region 第〇月曜日を求める
        /// <summary>
        /// 第〇月曜日を求める
        /// </summary>
        /// <param name="iNthMonday">週</param>
        /// <param name="iYear">年</param>
        /// <param name="iMonth">月</param>
        /// <returns>年月日</returns>
        private DateTime GetNthMonday(
            int iNthMonday,
            int iYear,
            int iMonth
            )
        {
            // 指定された月の日数
            int days = DateTime.DaysInMonth(year: iYear, month: iMonth);

            // 指定された月の全ての日から、月曜日だけを取り出す
            IEnumerable<DateTime> allMondays
              = Enumerable.Range(start: 1, count: days) // 1日～月末の日付（int型のコレクション）を作り
                .Select(d => new DateTime(year: iYear, month: iMonth, day: d)) // DateTime型のコレクションに変換して
                .Where(dt => dt.DayOfWeek == DayOfWeek.Monday); // そこから月曜日だけを取り出す

            // N番目の月曜日を求める
            return allMondays.ElementAt(index: iNthMonday - 1);
        }
        #endregion 第〇月曜日を求める

        #region 春分の日を求める(2000年～2099年まで)
        /// <summary>
        /// 春分の日を求める(2000年～2099年まで)
        /// </summary>
        /// <param name="iYear"></param>
        /// <returns></returns>
        private DateTime CalcVernalEquinoxDay(
            int iYear
            )
        {
            // 3. 春分の日と秋分の日
            // 国民の祝日に関する法律 第2条の一部
            //
            // 公式には前年の2月の官報で発表される。
            //
            // ここでは西暦2099年まで合っているとされる実験式を使う。
            // http://koyomi8.com/reki_doc/doc_0330.htm
            //１．2000年の太陽の春分点通過日
            //3月20.69115日
            //例．20.69115　（これは、期間中変化しません）
            //２．１年ごとの春分点通過日の移動量
            //（西暦年－2000年）×0.242194　（日）
            //例．(2010 - 2000) × 0.242194 = 2.42194 
            //３．閏年によるリセット量
            //INT｛（西暦年－2000年）/ 4｝　（日）
            //例．INT{(2010 - 2000) / 4} = INT(2.5) = 2 
            //４．求める年の春分日の計算
            //INT｛（１）＋（２）－（３）｝　（日）
            //例．INT{20.69115 + 2.42194 - 2} = INT(21.11309) = 21
            // 　　結果：2010年の春分日は　3/21日 
            //秋分日に関しては、春分日の説明の（１）を9/23.09日とする
            //※ 西暦2100は閏年ではないので、その年以降はこの計算式ではズレてしまう
            //
            // また、国立天文台のWebページ http://www.nao.ac.jp/faq/a0301.html には、
            // 西暦2030年までの表が掲載されている（春分の日と秋分の日がその通りになるとは限らない）。

            // 1. 2000年の太陽の春分点通過日
            double 基準日 = 20.69115;

            // 2. 春分点通過日の移動量＝（西暦年－2000年）×0.242194
            double 移動量 = (iYear - 2000) * 0.242194;

            // 3. 閏年によるリセット量＝INT｛（西暦年－2000年）/ 4｝
            int 閏年補正 = (int)((iYear - 2000) / 4.0);

            // 求める年の春分日＝INT｛（1）＋（2）－（3）｝
            int 春分日 = (int)(基準日 + 移動量 - 閏年補正);

            return new DateTime(year: iYear, month: 3, day: 春分日);
        }
        #endregion 春分の日を求める(2000年～2099年まで)

        #region 秋分の日を求める(2000年～2099年まで)
        /// <summary>
        /// 秋分の日を求める(2000年～2099年まで)
        /// </summary>
        /// <param name="iYear">年</param>
        /// <returns>年月日</returns>
        private DateTime CalcAutumnalEquinoxDay(
            int iYear
            )
        {
            // 3. 春分の日と秋分の日
            // 国民の祝日に関する法律 第2条の一部
            //
            // 公式には前年の2月の官報で発表される。
            //
            // ここでは西暦2099年まで合っているとされる実験式を使う。 
            // http://koyomi8.com/reki_doc/doc_0330.htm
            //１．2000年の太陽の春分点通過日
            //3月20.69115日
            //例．20.69115　（これは、期間中変化しません）
            //２．１年ごとの春分点通過日の移動量
            //（西暦年－2000年）×0.242194　（日）
            //例．(2010 - 2000) × 0.242194 = 2.42194 
            //３．閏年によるリセット量
            //INT｛（西暦年－2000年）/ 4｝　（日）
            //例．INT{(2010 - 2000) / 4} = INT(2.5) = 2 
            //４．求める年の春分日の計算
            //INT｛（１）＋（２）－（３）｝　（日）
            //例．INT{20.69115 + 2.42194 - 2} = INT(21.11309) = 21
            // 　　結果：2010年の春分日は　3/21日 
            //秋分日に関しては、春分日の説明の（１）を9/23.09日とする
            //※ 西暦2100は閏年ではないので、その年以降はこの計算式ではズレてしまう
            //
            // また、国立天文台のWebページ http://www.nao.ac.jp/faq/a0301.html には、
            // 西暦2030年までの表が掲載されている（春分の日と秋分の日がその通りになるとは限らない）。

            // 1. 2000年の太陽の秋分点通過日
            double 基準日 = 23.09; // 秋分点の揺らぎ補正済みの値

            // 2. 秋分点通過日の移動量＝（西暦年－2000年）×0.242194
            double 移動量 = (iYear - 2000) * 0.242194;

            // 3. 閏年によるリセット量＝INT｛（西暦年－2000年）/ 4｝
            int 閏年補正 = (int)((iYear - 2000) / 4.0);

            // 求める年の秋分日＝INT｛（1）＋（2）－（3）｝
            int 秋分日 = (int)(基準日 + 移動量 - 閏年補正);

            return new DateTime(year: iYear, month: 9, day: 秋分日);
        }
        #endregion 秋分の日を求める(2000年～2099年まで)

        #region 振替休日を求める
        /// <summary>
        /// 振替休日を求める
        /// </summary>
        /// <param name="sdHolidays"></param>
        /// <returns></returns>
        private IEnumerable<HolidayInfo> GetSubstituteHolidays(
            SortedDictionary<DateTime, HolidayInfo> sdHolidays
            )
        {
            // 振替休日を格納するためのコレクション
            var substituteHolidays = new List<HolidayInfo>();

            // これまでに求めた祝日を全部チェックする
            foreach (var holiday in sdHolidays.Values)
            {
                if (holiday._Date.DayOfWeek != DayOfWeek.Sunday)
                    continue; // 日曜でなければ除外する

                // 翌日（＝月曜日）を仮に振替休日とする
                DateTime substitute = holiday._Date.AddDays(value: 1.0);

                // その日がすでに祝日ならば振替休日はさらにその翌日
                while (sdHolidays.ContainsKey(key: substitute))
                    substitute = substitute.AddDays(value: 1.0);

                // 見つかった振替休日をコレクションに追加する
                var substituteHoliday = new HolidayInfo()
                {
                    _Date = substitute,
                    _HolidayKind = HolidayInfo.enmHolidayKind.振替休日,
                    _Param = 0,
                    _StartApplyYear = 1973,
                    _EndApplyYear = MAX_YEAR,
                    _Name = HolidayInfo.enmHolidayKind.振替休日.ToString(),
                    _Definition =
                        string.Format(
                            format: "{0}の{1}",
                            args: new object[]
                            {
                                    holiday._Name,
                                    HolidayInfo.enmHolidayKind.振替休日.ToString()
                            })
                };
                substituteHolidays.Add(item: substituteHoliday);
            }
            return substituteHolidays;
        }
        #endregion 振替休日を求める

        #region 全ての国民の休日を求める
        /// <summary>
        /// 全ての国民の休日を求める
        /// </summary>
        /// <param name="sdHolidays"></param>
        /// <returns>結果</returns>
        private IEnumerable<HolidayInfo> GetSandwichedHolidays(
            SortedDictionary<DateTime, HolidayInfo> sdHolidays
            )
        {
            var sandwichedHolidays = new List<HolidayInfo>();

            // これまでに求めた祝日を全部チェックする
            foreach (var holiday0 in sdHolidays.Values)
            {
                if (holiday0._HolidayKind != HolidayInfo.enmHolidayKind.国民の祝日)
                    continue; // その休日が国民の祝日でなければ除外する

                var day0 = holiday0._Date;

                var day2 = day0.AddDays(value: 2.0); // 2日後
                if (!sdHolidays.ContainsKey(key: day2))
                    continue; // 2日後が祝日でないときは除外する
                var holiday2 = sdHolidays[day2];
                if (holiday2._HolidayKind != HolidayInfo.enmHolidayKind.国民の祝日)
                    continue; // 2日後が祝日であっても国民の祝日でなければ除外する

                var day1 = day0.AddDays(1.0); // 1日後=国民の祝日で挟まれた日
                if (day1.DayOfWeek == DayOfWeek.Sunday)
                    continue; // その日が日曜（＝もともと休日）のときは除外する
                if (sdHolidays.ContainsKey(key: day1))
                    continue; // その日がすでに祝日のときは除外する

                // 見つかった国民の休日をコレクションに追加する
                var sandwichedHoliday = new HolidayInfo(
                    )
                {
                    _Date = day1,
                    _HolidayKind = HolidayInfo.enmHolidayKind.国民の休日,
                    _Name = HolidayInfo.enmHolidayKind.国民の休日.ToString(),
                    _Param = 0,
                    _StartApplyYear = 1985,
                    _EndApplyYear = MAX_YEAR,
                    _Definition = string.Format(
                        format: "{0}と{1}の間の日",
                        args: new object[]
                        {
                                holiday0._Name,
                                holiday2._Name
                        }),
                };
                sandwichedHolidays.Add(item: sandwichedHoliday);
            }
            return sandwichedHolidays;
        }
        #endregion 全ての国民の休日を求める

        #endregion メソッド
    }
    #endregion 休日クラス
}
