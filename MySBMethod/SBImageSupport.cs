using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    /// <summary>
    /// SmallBasic用イメージ
    /// </summary>
    [SmallBasicType]
    public static class SBImageSupport
    {
        /// <summary>
        /// イメージをリサイズして結果をイメージとして取得します
        /// </summary>
        /// <param name="width">新しい幅</param>
        /// <param name="height">新しい高さ</param>
        /// <returns>イメージ</returns>
        public static Primitive Resize(
            Primitive width,
            Primitive height
            )
        {
            return (int)123;
        }
    }
}
