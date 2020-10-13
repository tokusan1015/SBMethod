using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    /// <summary>
    /// 公式計算
    /// </summary>
    [SmallBasicType]
    public static class SBFormula
    {
        /// <summary>
        /// n!(階乗)を計算します
        /// </summary>
        /// <param name="n">階乗する数</param>
        /// <returns>数値</returns>
        public static Primitive Factorial(
            Primitive n
            )
        {
            return Utility.CalcFactorial((int)n);
        }
        /// <summary>
        /// nPm(順列)を求めます
        /// </summary>
        /// <param name="n">元の数</param>
        /// <param name="m">選ぶ数</param>
        /// <returns>数値</returns>
        public static Primitive Permutation(
            Primitive n,
            Primitive m
            )
        {
            return Utility.CalcPermutation((int)n, (int)m);
        }
        /// <summary>
        /// nCm(組み合わせ)を求めます
        /// </summary>
        /// <param name="n">元の数</param>
        /// <param name="m">選ぶ数</param>
        /// <returns>数値</returns>
        public static Primitive Combination(
            Primitive n,
            Primitive m
            )
        {
            return Utility.CalcCombination((int)n, (int)m);
        }
    }
}
