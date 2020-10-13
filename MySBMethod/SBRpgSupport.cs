using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用ロールプレイングゲームサポート
    /// <summary>
    /// SmallBasic用ロールプレイングゲームサポート
    /// </summary>
    [SmallBasicType]
    public static class SBRpgSupport
    {
        #region メンバ変数
        /// <summary>
        /// 乱数
        /// </summary>
        private static System.Random _rnd = new Random(Environment.TickCount + 1);
        /// <summary>
        /// ゲーム内時間開始日時
        /// </summary>
        private static DateTime _StartGameTime = DateTime.Now;
        /// <summary>
        /// 最大ステータス
        /// </summary>
        private static StatusManager _MaxStatus = new StatusManager();
        /// <summary>
        /// アイテムリスト
        /// </summary>
        private static ItemManager _ItemList = new ItemManager();
        /// <summary>
        /// イベントリスト
        /// </summary>
        private static EventManager _EventList = new EventManager();
        #endregion メンバ変数

        #region 公開メソッド
        /// <summary>
        /// 現在日時をゲーム開始日時として設定します
        /// SetStartGameTimeを実行していない場合は、
        /// 正確な時刻を取得できません
        /// </summary>
        public static void SetStartGameTime(
            )
        {
            _StartGameTime = DateTime.Now;
        }
        /// <summary>
        /// ゲーム経過時間を日時分秒の文字列で取得します
        /// SetStartGameTimeを実行していない場合は、
        /// 正確な値を取得できません
        /// </summary>
        /// <returns>文字列</returns>
        public static Primitive GetGameTime()
        {
            return (DateTime.Now - _StartGameTime).ToString();
        }
        /// <summary>
        /// ゲーム経過時間を日で取得します
        /// SetStartGameTimeを実行していない場合は、
        /// 正確な値を取得できません
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetGameTimeDays()
        {
            return (DateTime.Now - _StartGameTime).TotalDays;
        }
        /// <summary>
        /// ゲーム経過時間を時間で取得します
        /// SetStartGameTimeを実行していない場合は、
        /// 正確な値を取得できません
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetGameTimeHours()
        {
            return (DateTime.Now - _StartGameTime).TotalHours;
        }
        /// <summary>
        /// ゲーム経過時間を分で取得します
        /// SetStartGameTimeを実行していない場合は、
        /// 正確な値を取得できません
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetGameTimeMinutes()
        {
            return (DateTime.Now - _StartGameTime).TotalMinutes;
        }
        /// <summary>
        /// ゲーム経過時間を秒で取得します
        /// SetStartGameTimeを実行していない場合は、
        /// 正確な値を取得できません
        /// </summary>
        /// <returns>数値</returns>
        public static Primitive GetGameTimeSeconds()
        {
            return (DateTime.Now - _StartGameTime).TotalSeconds;
        }
        /// <summary>
        /// 遭遇情報を取得し、結果をBOOLで取得します
        /// 遭遇確率は歩数毎に％で上がって行く数値です
        /// 結果は遭遇した場合trueが返ります
        /// 例)
        /// 1)noEventSteps = 0, eventProbabilityPerStep = 5
        ///   一歩進む毎に遭遇確率が5%づつ上昇します
        ///   1歩目 = 10%, 20歩目 = 100%
        /// 2)noEventSteps = 5, eventProbabilityPerStep = 10
        ///   5歩までは0%, 6歩目から一歩進む毎に遭遇確率が10%づつ上昇します
        ///   1歩目から5歩目 = 0%, 6歩目 = 10%, 15歩目 = 100%
        /// </summary>
        /// <param name="steps">歩数</param>
        /// <param name="eventProbabilityPerStep">遭遇確率増加量(%)</param>
        /// <param name="noEventSteps">イベントの発生しない歩数</param>
        /// <returns>BOOL</returns>
        public static Primitive Encounter(
            Primitive steps,
            Primitive eventProbabilityPerStep,
            Primitive noEventSteps
            )
        {
            // 歩数がイベントの発生しない歩数より小さい場合
            if ((int)steps <= (int)noEventSteps)
                // 必ずfalseを返す
                return false;

            // 遭遇率を求める
            int eventProbability = ((int)steps - (int)noEventSteps) * (int)eventProbabilityPerStep;

            // 遭遇率よりも乱数値(1,100)が小さい場合はエンカウントとする
            return  eventProbability <= _rnd.Next(1, 100);
        }
        /// <summary>
        /// 最大ステータス名一覧を文字列配列で取得します
        /// 配列番号-1が番号になります(使用する場合は配列番号-1してください)
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetMaxStatusNo()
        {
            return typeof(StatusData.StatusDataNo).GetPrimitiveEnumNameList();
        }
        /// <summary>
        /// 最大ステータス情報を追加します
        /// 結果は最大ステータス番号(MaxStatusNo)順で取得できます
        /// status[1]  : 管理番号(No)
        /// status[2]  : 最大HP(HP)
        /// status[3]  : 最大MP(MP)
        /// status[4]  : 敏捷性(AGI)
        /// status[5]  : 運(LUK)
        /// status[6]  : 攻撃(ATK)
        /// status[7]  : 命中率(DEX)
        /// status[8]  : 防御(DEF)
        /// status[9]  : 回避率(EVA)
        /// status[10] : 魔法攻撃(MAT)
        /// status[11] : 魔法防御(MDE)
        /// </summary>
        /// <param name="status">ステータス</param>
        public static void AddMaxStatus(
            Primitive status
            )
        {
            // 入力チェック
            if (status.GetItemCount() != (int)StatusData.StatusDataNo.Unknown)
                throw new ArgumentOutOfRangeException("配列数が異常です");

            // ステータスを追加する
            StatusData rs = new StatusData();

            // ステータスを設定する
            for (int i = 0; i < rs.Length; i++)
            {
                rs[i] = (int)status[i + 1];
            }

            // 追加する
            _MaxStatus.Add(rs);
        }
        /// <summary>
        /// 最大ステータスが登録されているか検証し、
        /// 結果をBOOLで取得します
        /// 存在する場合はtrueを取得します
        /// </summary>
        /// <param name="no">管理番号</param>
        /// <returns>BOOL</returns>
        public static Primitive ExistMaxStatus(
            Primitive no
            )
        {
            return _MaxStatus.GetRpgStatus((int)no) != null;
        }
        /// <summary>
        /// 最大ステータスを数値配列で取得します
        /// 事前にExistMaxStatusメソッドで
        /// 存在することを確認してください
        /// </summary>
        /// <param name="no">管理番号</param>
        /// <returns>数値配列</returns>
        public static Primitive GetMaxStatus(
            Primitive no
            )
        {
            // 戻り値生成
            Primitive result = new Primitive();

            // ステータスを取得
            var status = _MaxStatus.GetRpgStatus((int)no);

            // 配列に設定
            for (int i = 0; i < (int)StatusData.StatusDataNo.Unknown; i++)
            {
                result[i + 1] = (int)status[i];
            }

            return result;
        }
        /// <summary>
        /// 分類1の名称一覧を文字列配列で取得します
        /// 配列番号-1が番号になります(使用する場合は配列番号-1してください)
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetItemCategory1()
        {
            return typeof(ItemData.ItemCategory1).GetPrimitiveEnumNameList();
        }
        /// <summary>
        /// 分類2の名称一覧を文字列配列で取得します
        /// 配列番号-1が番号になります(使用する場合は配列番号-1してください)
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetItemCategory2()
        {
            return typeof(ItemData.ItemCategory2).GetPrimitiveEnumNameList();
        }
        /// <summary>
        /// 効果の名称一覧を文字列配列で取得します
        /// 配列番号-1が番号になります(使用する場合は配列番号-1してください)
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetItemEffect()
        {
            return typeof(ItemData.ItemEffect).GetPrimitiveEnumNameList();
        }
        /// <summary>
        /// アイテム情報の名称一覧を文字列配列で取得します
        /// 配列番号-1が番号になります(使用する場合は配列番号-1してください)
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetItemInfoNo()
        {
            return typeof(ItemData.ItemDataNo).GetPrimitiveEnumNameList();
        }
        /// <summary>
        /// アイテム情報追加
        ///   分類1    : GetItemCategory1メソッドで取得します
        ///   分類2    : GetItemCategory2メソッドで取得します
        ///   効果番号 : GetEffectメソッドで取得します
        /// 結果はアイテム情報番号(ItemInfoNo)順の配列で返されます
        /// [1] : アイテム番号
        /// [2] : 分類1
        /// [3] : 分類2
        /// [4] : 効果番号
        /// [5] : 効果量
        /// [6] : 効果時間
        /// </summary>
        /// <param name="itemInfo">アイテム情報</param>
        public static void AddItemInfo(
            Primitive itemInfo
            )
        {
            // ステータスを追加する
            ItemData id = new ItemData();

            // 入力チェック
            if (itemInfo.GetItemCount() != id.Length)
                throw new ArgumentOutOfRangeException("配列数が異常です");

            for (int i = 0; i < id.Length; i++)
            {
                id[i] = (int)itemInfo[i + 1];
            }

            // 追加する
            _ItemList.Add(id);
        }
        /// <summary>
        /// アイテム番号が存在するか検証し、
        /// 結果をBOOLで取得します
        /// 存在した場合trueを取得します
        /// </summary>
        /// <param name="no">アイテム番号</param>
        /// <returns>BOOL</returns>
        public static Primitive ExistItemInfo(
            Primitive no
            )
        {
            return _ItemList.GetItemData((int)no) != null;
        }
        /// <summary>
        /// アイテム情報を数値配列で取得します
        ///   分類1    : GetItemCategory1メソッドで取得します
        ///   分類2    : GetItemCategory2メソッドで取得します
        ///   効果番号 : GetEffectメソッドで取得します
        /// 戻り値
        /// [1] : アイテム番号
        /// [2] : 分類1
        /// [3] : 分類2
        /// [4] : 効果番号
        /// [5] : 効果量
        /// [6] : 効果時間
        /// </summary>
        /// <param name="no"></param>
        /// <returns>数値配列</returns>
        public static Primitive GetItemInfo(
            Primitive no
            )
        {
            Primitive result = new Primitive();

            var id = _ItemList.GetItemData((int)no);

            for (int i = 0; i < id.Length; i++)
            {
                result[i + 1] = (int)id[i];
            }

            return result;
        }
        /// <summary>
        /// イベント情報の一覧を文字列配列で取得します
        /// 配列番号-1が番号になります(使用する場合は配列番号-1してください)
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetEventInfoNo()
        {
            return typeof(EventData.EventInfoNo).GetPrimitiveEnumNameList();
        }
        /// <summary>
        /// イベントフラグの一覧を文字列配列で取得します
        /// </summary>
        /// <returns>文字列配列</returns>
        public static Primitive GetEventFlags()
        {
            return typeof(EventData.EventFlags).GetPrimitiveEnumNameList();
        }
        /// <summary>
        /// イベント情報を追加します
        /// eventInfos[1]  = 管理番号
        /// eventInfos[2]  = イベントフラグ(組み合わせ)
        /// eventInfos[3]  = ストーリー番号(ストーリー進展)
        /// eventInfos[4]  = ミッション番号(ミッション完了)
        /// eventInfos[5]  = クエスト番号(クエスト完了)
        /// eventInfos[6]  = サブクエスト番号(サブクエスト完了)
        /// eventInfos[7]  = 地域番号(地域移動)
        /// eventInfos[8]  = NPC番号(NPC会話)
        /// eventInfos[9]  = ポイント番号(ポイント選択)
        /// eventInfos[10] = アイテム番号(アイテム取得)
        /// </summary>
        public static void AddEventInfos(
            Primitive eventInfos
            )
        {
            // ステータスを追加する
            EventData ed = new EventData();

            // 入力チェック
            if (eventInfos.GetItemCount() != ed.Length)
                throw new ArgumentOutOfRangeException("配列数が異常です");

            for (int i = 0; i < ed.Length; i++)
            {
                ed[i] = (int)eventInfos[i + 1];
            }

            // 追加する
            _EventList.Add(ed);
        }
        /// <summary>
        /// アイテム番号が存在するか検証し、
        /// 結果をBOOLで取得します
        /// 存在した場合trueを取得します
        /// </summary>
        /// <param name="no">アイテム番号</param>
        /// <returns>BOOL</returns>
        public static Primitive ExistEventInfo(
            Primitive no
            )
        {
            return _EventList.GetEventData((int)no) != null;
        }
        /// <summary>
        /// イベント情報を数値配列で取得します
        /// 配列番号-1が番号になります(使用する場合は配列番号-1してください)
        /// </summary>
        /// <returns>数値配列</returns>
        public static Primitive GetEventData(
            Primitive no
            )
        {
            // 戻り値生成
            Primitive result = new Primitive();

            // ステータスを取得
            var eventData = _EventList.GetEventData((int)no);

            // 配列に設定
            for (int i = 0; i < (int)EventData.EventInfoNo.Unknown; i++)
            {
                result[i + 1] = (int)eventData[i];
            }

            return result;
        }
        #endregion 公開メソッド

        #region ステータス管理
        /// <summary>
        /// ステータス管理
        /// </summary>
        internal class StatusManager
        {
            #region メンバ変数
            /// <summary>
            /// ステータス辞書
            /// </summary>
            private Dictionary<int, StatusData> _StatusList = new Dictionary<int, StatusData>();
            #endregion メンバ変数

            #region 公開メソッド

            #region ステータス情報を追加する
            /// <summary>
            /// ステータス情報を追加する
            /// 登録済の場合は例外とする
            /// </summary>
            /// <param name="rs">ステータス</param>
            public void Add(
                StatusData rs
                )
            {
                // 登録済チェック
                if (this.GetRpgStatus(rs[StatusData.StatusDataNo.No]) != null)
                    throw new ArgumentOutOfRangeException($"No({rs[StatusData.StatusDataNo.No]})は既に登録されています");

                // 追加する
                this._StatusList.Add(rs[StatusData.StatusDataNo.No], rs);
            }
            #endregion ステータス情報を追加する

            #region 管理番号のステータス情報を取得する
            /// <summary>
            /// 管理番号のステータス情報を取得する
            /// 取得できない場合はnullを返す
            /// </summary>
            /// <param name="no">管理番号</param>
            /// <returns>結果</returns>
            public StatusData GetRpgStatus(
                int no
                )
            {
                StatusData rs = null;
                this._StatusList.TryGetValue(no, out rs);
                return rs;
            }
            #endregion 管理番号のステータス情報を取得する

            #endregion 公開メソッド
        }
        #endregion ステータス管理

        #region ステータス
        /// <summary>
        /// ステータス
        /// </summary>
        internal class StatusData
        {
            #region 列挙体
            /// <summary>
            /// ステータス番号
            /// </summary>
            public enum StatusDataNo : int
            {
                /// <summary>
                /// 管理番号
                /// </summary>
                No = 0,
                /// <summary>
                /// 最大HP(Max Helth Point)
                /// </summary>
                MHP,
                /// <summary>
                /// 最大MP(Max Magic Point)
                /// </summary>
                MMP,
                /// <summary>
                /// 敏捷性(Agility)
                /// </summary>
                AGI,
                /// <summary>
                /// 運(Luck)
                /// </summary>
                LUK,
                /// <summary>
                /// 攻撃(Attack)
                /// </summary>
                ATK,
                /// <summary>
                /// 命中率(Dexterity)
                /// </summary>
                DEX,
                /// <summary>
                /// 防御(Defense)
                /// </summary>
                DEF,
                /// <summary>
                /// 回避率(Evasion)
                /// </summary>
                EVA,
                /// <summary>
                /// 魔法攻撃(Magic Attack)
                /// </summary>
                MAT,
                /// <summary>
                /// 魔法防御(Magic Defense)
                /// </summary>
                MDE,
                /// <summary>
                /// 要素数
                /// </summary>
                Unknown
            }
            #endregion 列挙体

            #region メンバ変数
            /// <summary>
            /// ステータス配列
            /// </summary>
            private int[] _Status = new int[(int)StatusDataNo.Unknown];
            #endregion メンバ変数

            #region プロパティ
            /// <summary>
            /// ステータス取得
            /// </summary>
            /// <param name="sn">ステータス番号</param>
            /// <returns>結果</returns>
            public int this [StatusDataNo sn]
            {
                get { return this[(int)sn]; }
                set { this[(int)sn] = value; }
            }
            /// <summary>
            /// ステータス取得
            /// </summary>
            /// <param name="index">インデックス</param>
            /// <returns>結果</returns>
            public int this [int index]
            {
                get { return this._Status[index]; }
                set { this._Status[index] = value; }
            }
            /// <summary>
            /// ステータス配列
            /// </summary>
            public int[] StatusArray
            {
                get { return this._Status; }
            }
            /// <summary>
            /// ステータス配列要素数
            /// </summary>
            public int Length
            {
                get { return this._Status.Length; }
            }
            #endregion プロパティ
        }
        #endregion プレイヤー管理

        #region アイテム管理
        /// <summary>
        /// アイテム管理
        /// </summary>
        internal class ItemManager
        {
            #region メンバ変数
            /// <summary>
            /// アイテムリスト
            /// </summary>
            public List<ItemData> _ItemList = new List<ItemData>();
            #endregion メンバ変数

            #region 公開メソッド

            #region 追加
            /// <summary>
            /// 追加
            /// </summary>
            /// <param name="id">ItemData</param>
            public void Add(
                ItemData id
                )
            {
                this._ItemList.Add(id);
            }
            #endregion 追加

            #region ItemData取得
            /// <summary>
            /// ItemData取得
            /// </summary>
            /// <param name="no">アイテム番号</param>
            /// <returns>結果</returns>
            public ItemData GetItemData(
                int no
                )
            {
                return this._ItemList
                    .FirstOrDefault(x => x[ItemData.ItemDataNo.No] == no);
            }
            #endregion ItemData取得

            #endregion 公開メソッド
        }
        #endregion アイテム管理

        #region アイテムデータ
        /// <summary>
        /// アイテムデータ
        /// </summary>
        internal class ItemData
        {
            #region メンバ変数
            /// <summary>
            /// アイテム情報配列
            /// </summary>
            private int[] _ItemsInfo = new int[(int)ItemDataNo.Unknown];
            #endregion メンバ変数

            #region 列挙体
            /// <summary>
            /// アイテム情報
            /// </summary>
            public enum ItemDataNo : int
            {
                /// <summary>
                /// アイテム番号
                /// </summary>
                No = 0,
                /// <summary>
                /// 分類1
                /// </summary>
                Category1,
                /// <summary>
                /// 分類2
                /// </summary>
                Category2,
                /// <summary>
                /// 効果
                /// </summary>
                Effect,
                /// <summary>
                /// 効果量
                /// </summary>
                EffectSize,
                /// <summary>
                /// 効果(耐久)時間
                /// </summary>
                EffectTime,
                /// <summary>
                /// 要素数
                /// </summary>
                Unknown,
            }
            /// <summary>
            /// 分類1
            /// </summary>
            public enum ItemCategory1 : int
            {
                /// <summary>
                /// アイテム
                /// </summary>
                Item = 0,
                /// <summary>
                /// 武器
                /// </summary>
                Weapon,
                /// <summary>
                /// 防具
                /// </summary>
                Armor,
                /// <summary>
                /// 重要アイテム
                /// </summary>
                ImportantItem,
                /// <summary>
                /// 要素数
                /// </summary>
                Unknown,
            }
            /// <summary>
            /// 分類2
            /// </summary>
            public enum ItemCategory2 : int
            {
                /// <summary>
                /// 頭(首から上の部分)
                /// </summary>
                Head = 0,
                /// <summary>
                /// 銅
                /// </summary>
                Body,
                /// <summary>
                /// 腕
                /// </summary>
                Arm,
                /// <summary>
                /// 脚
                /// </summary>
                Leg,
                /// <summary>
                /// 足
                /// </summary>
                Foot,
                /// <summary>
                /// 両手
                /// </summary>
                BothHands,
                /// <summary>
                /// 右手
                /// </summary>
                RightHand,
                /// <summary>
                /// 左手
                /// </summary>
                LeftHand,
                /// <summary>
                /// 要素数
                /// </summary>
                Unknown,
            }
            /// <summary>
            /// アイテム効果
            /// </summary>
            public enum ItemEffect : int
            {
                /// <summary>
                /// 無し
                /// </summary>
                Nothing = 0,
                /// <summary>
                /// 最大HP(Max Helth Point)
                /// </summary>
                MHP,
                /// <summary>
                /// 最大MP(Max Magic Point)
                /// </summary>
                MMP,
                /// <summary>
                /// 敏捷性(Agility)
                /// </summary>
                AGI,
                /// <summary>
                /// 運(Luck)
                /// </summary>
                LUK,
                /// <summary>
                /// 攻撃(Attack)
                /// </summary>
                ATK,
                /// <summary>
                /// 命中率(Dexterity)
                /// </summary>
                DEX,
                /// <summary>
                /// 防御(Defense)
                /// </summary>
                DEF,
                /// <summary>
                /// 回避率(Evasion)
                /// </summary>
                EVA,
                /// <summary>
                /// 魔法攻撃(Magic Attack)
                /// </summary>
                MAT,
                /// <summary>
                /// 魔法防御(Magic Defense)
                /// </summary>
                MDE,
                /// <summary>
                /// 要素数
                /// </summary>
                Unknown
            }
            #endregion 列挙体

            #region 公開プロパティ
            /// <summary>
            /// アイテム情報取得
            /// </summary>
            /// <param name="iin">ItemInfoNo</param>
            /// <returns>結果</returns>
            public int this [ItemDataNo iin]
            {
                get { return this[(int)iin]; }
                set { this[(int)iin] = value; }
            }
            /// <summary>
            /// アイテム情報取得
            /// </summary>
            /// <param name="index">インデックス</param>
            /// <returns>結果</returns>
            public int this [int index]
            {
                get { return this._ItemsInfo[index]; }
                set { this[index] = value; }
            }
            /// <summary>
            /// アイテム情報配列
            /// </summary>
            public int[] ItemInfoArray
            {
                get { return this._ItemsInfo; }
            }
            /// <summary>
            /// アイテム情報配列要素数
            /// </summary>
            public int Length
            {
                get { return this._ItemsInfo.Length; }
            }
            #endregion 公開プロパティ
        }
        #endregion アイテムデータ

        #region イベント管理
        /// <summary>
        /// イベント管理
        /// </summary>
        internal class EventManager
        {
            #region メンバ変数
            /// <summary>
            /// イベントリスト
            /// </summary>
            public List<EventData> _EventList = new List<EventData>();
            #endregion メンバ変数

            #region 公開メソッド

            #region 追加
            /// <summary>
            /// 追加
            /// </summary>
            /// <param name="ed">EventData</param>
            public void Add(
                EventData ed
                )
            {
                this._EventList.Add(ed);
            }
            #endregion 追加

            #region EventData取得
            /// <summary>
            /// EventData取得
            /// </summary>
            /// <param name="no">イベント番号</param>
            /// <returns>結果</returns>
            public EventData GetEventData(
                int no
                )
            {
                return this._EventList
                    .FirstOrDefault(x => x[EventData.EventInfoNo.No] == no);
            }
            #endregion ItemData取得

            #endregion 公開メソッド
        }
        #endregion アイテム管理

        #region イベントデータ
        /// <summary>
        /// イベントデータ
        /// </summary>
        internal class EventData
        {
            #region 列挙体
            public enum EventInfoNo : int
            {
                /// <summary>
                /// 管理番号
                /// </summary>
                No,
                /// <summary>
                /// イベントフラグ
                /// </summary>
                RaiseEventFlags,
                /// <summary>
                /// ストーリー番号
                /// </summary>
                StoryNo,
                /// <summary>
                /// ミッション番号
                /// </summary>
                MissionNo,
                /// <summary>
                /// クエスト番号
                /// </summary>
                QuestNo,
                /// <summary>
                /// サブクエスト番号
                /// </summary>
                SubQuestNo,
                /// <summary>
                /// 地域番号
                /// </summary>
                AreaNo,
                /// <summary>
                /// NPC番号
                /// </summary>
                NpcNo,
                /// <summary>
                /// ポイント番号
                /// </summary>
                PointNo,
                /// <summary>
                /// アイテム番号
                /// </summary>
                ItemNo,
                /// <summary>
                /// 要素数
                /// </summary>
                Unknown,
            }
            /// <summary>
            /// イベント発生条件
            /// </summary>
            [Flags]
            public enum EventFlags : uint
            {
                /// <summary>
                /// 常に発生
                /// </summary>
                Always = 0x0000,
                /// <summary>
                /// ストーリー進展
                /// </summary>
                StoryNo = 0x0001,
                /// <summary>
                /// ミッション完了
                /// </summary>
                MissionNo = 0x0002,
                /// <summary>
                /// クエスト完了
                /// </summary>
                QuestNo = 0x0004,
                /// <summary>
                /// サブクエスト完了
                /// </summary>
                SubQuestNo = 0x0008,
                /// <summary>
                /// エリア到達
                /// </summary>
                AreaNo = 0x0010,
                /// <summary>
                /// NPCとの会話
                /// </summary>
                NpcNo = 0x0020,
                /// <summary>
                /// ポイント選択
                /// </summary>
                PointNo = 0x0040,
                /// <summary>
                /// アイテム取得
                /// </summary>
                ItemNo = 0x0080,
            }
            #endregion 列挙体

            #region メンバ変数
            /// <summary>
            /// イベント情報配列
            /// </summary>
            private int[] _EventInfos = new int[(int)EventInfoNo.Unknown];
            #endregion メンバ変数

            #region 公開プロパティ
            /// <summary>
            /// イベント情報取得
            /// </summary>
            /// <param name="edn">イベント情報番号</param>
            /// <returns>結果</returns>
            public int this [EventInfoNo edn]
            {
                get { return this[(int)edn]; }
                set { this[(int)edn] = value; }
            }
            /// <summary>
            /// イベント情報取得
            /// </summary>
            /// <param name="index">インデックス</param>
            /// <returns>結果</returns>
            public int this [int index]
            {
                get { return this._EventInfos[index]; }
                set { this[index] = value; }
            }
            /// <summary>
            /// イベント情報の総素数を取得する
            /// </summary>
            public int Length
            {
                get { return this._EventInfos.Length; }
            }
            /// <summary>
            /// イベント情報配列
            /// </summary>
            public int[] EventInfos
            {
                get { return this._EventInfos; }
            }
            #endregion 公開プロパティ

            #region 公開メソッド
            /// <summary>
            /// イベントフラグを検証する
            /// </summary>
            /// <param name="flag">フラグ</param>
            /// <returns></returns>
            public bool CheckRaizeEventFlag(
                EventFlags flag
                )
            {
                EventFlags flags = (EventFlags)
                    this._EventInfos[(int)EventInfoNo.RaiseEventFlags];

                return flags.CheckManagementFlags(flag);
            }
            /// <summary>
            /// イベントフラグをセットする
            /// </summary>
            /// <param name="flag">フラグ</param>
            /// <returns></returns>
            public EventFlags SetRaizeEventFlag(
                EventFlags flag
                )
            {
                EventFlags flags = (EventFlags)
                    this._EventInfos[(int)EventInfoNo.RaiseEventFlags];

                return (EventFlags)flags.SetManagementFlags(flag);
            }
            /// <summary>
            /// イベントフラグをリセットする
            /// </summary>
            /// <param name="flag">フラグ</param>
            /// <returns></returns>
            public EventFlags ResetRaizeEventFlag(
                EventFlags flag
                )
            {
                EventFlags flags = (EventFlags)
                    this._EventInfos[(int)EventInfoNo.RaiseEventFlags];

                return (EventFlags)flags.ResetManagementFlags(flag);
            }
            #endregion 公開メソッド
        }
        #endregion イベントデータ
    }
    #endregion SmallBasic用ロールプレイングゲームサポート
}
