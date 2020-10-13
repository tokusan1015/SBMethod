using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用イメージ
    /// <summary>
    /// SmallBasic用イメージ
    /// </summary>
    [SmallBasicType]
    class SBImage
    {
        /// <summary>
        /// ※※※制作中の為動作しません※※※
        /// イメージを幅、高さのサイズに変更し、
        /// 結果をイメージで取得します
        /// ※※※制作中の為動作しません※※※
        /// </summary>
        /// <param name="image">イメージ</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns>イメージ</returns>
        public static Primitive Resize(
            Primitive image,
            Primitive width,
            Primitive height
            )
        {
            var img = (object)image;

            var typ = img.GetType();

            return typ.Name;
        }
    }
    #endregion SmallBasic用イメージ
}
