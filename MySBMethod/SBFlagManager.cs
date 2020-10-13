using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用フラグマネージャ
    /// <summary>
    /// SmallBasic用フラグマネージャ
    /// </summary>
    [SmallBasicType]
    public class SBFlagManager
    {
        #region メンバ変数
        /// <summary>
        /// フラグリスト
        /// </summary>
        private static Dictionary<int, FlagData> _FlagList = 
            new Dictionary<int, FlagData>();
        #endregion メンバ変数

        #region 公開メソッド
        /// <summary>
        /// フラグを追加する
        /// </summary>
        /// <param name="id">id番号</param>
        /// <param name="messageId">メッセージ番号</param>
        public static void Add(
            Primitive id,
            Primitive messageId
            )
        {
            _FlagList.Add(
                key: id,
                value: new FlagData
                {
                    Id = id,
                    MessageId = messageId,
                    Flag = false,
                });
        }
        /// <summary>
        /// 関連Idを追加する
        /// </summary>
        /// <param name="id">id番号</param>
        /// <param name="relationId">関連id番号</param>
        public static void AddRelationId(
            Primitive id,
            Primitive relationId
            )
        {
            _GetFlagData((int)id).RelationIdList.Add(relationId);
        }
        #endregion 公開メソッド

        #region メソッド
        /// <summary>
        /// フラグデータ取得
        /// </summary>
        /// <param name="id">id番号</param>
        /// <returns>結果</returns>
        private static FlagData _GetFlagData(
            int id
            )
        {
            FlagData fd = null;
            _FlagList.TryGetValue(
                key: id,
                value: out fd
                );
            return fd;
        }
        #endregion メソッド
    }
    #endregion SmallBasic用フラグマネージャ

    #region フラグデータクラス
    /// <summary>
    /// フラグデータクラス
    /// </summary>
    internal class FlagData
    {
        #region プロパティ
        /// <summary>
        /// id番号
        /// </summary>
        public int Id { get; set; } = 0;
        /// <summary>
        /// フラグ状態
        /// </summary>
        public bool Flag { get; set; } = false;
        /// <summary>
        /// 対応メッセージId
        /// </summary>
        public int MessageId { get; set; } = 0;
        /// <summary>
        /// 関連フラグ
        /// </summary>
        public List<int> RelationIdList { get; } = new List<int>();
        #endregion プロパティ
    }
    #endregion フラグデータクラス
}
