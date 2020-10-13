using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用計算
    /// <summary>
    /// SmallBasic用計算
    /// </summary>
    [SmallBasicType]
    public static class SBMath
    {
        #region メンバ変数
        /// <summary>
        /// 計算
        /// </summary>
        private static Math _Math = new Math();
        #endregion メンバ変数

        #region 三平方の定理
        /// <summary>
        /// 直角三角形の辺a,辺bから三平方の定理を使用して斜辺cを取得します
        /// 直角三角形の斜辺を 辺 c として、残る直角をはさむ二辺を 辺 a 
        /// および 辺 b とした場合に a^2 + b^2 = c^2 となる。 
        /// c = sqrt(a^2 + b^2)
        /// </summary>
        /// <param name="a">辺aの長さ</param>
        /// <param name="b">辺bの長さ</param>
        /// <returns>数値</returns>
        public static Primitive PythagoreanTheorem(
            Primitive a,
            Primitive b
            )
        {
            return System.Math.Sqrt((double)a * (double)a + (double)b * (double)b);
        }
        #endregion 三平方の定理

        #region 正弦定理
        /// <summary>
        /// 三角形の辺aと角度A(rad)から外接円の半径rを取得します
        /// 三角形ABC において、BC = a, CA = b, AB = c, 外接円の半径を r とすると
        /// a / sin(A) = b / sin(B) = c / sin(C) = 2r が成り立つ
        /// r =  a / (2 * sin(A))
        /// </summary>
        /// <param name="a">辺a</param>
        /// <param name="A">角度A(rad)</param>
        /// <returns>数値</returns>
        public static Primitive LawOfSines(
            Primitive a,
            Primitive A
            )
        {
            return (double)a / (2 * System.Math.Sin(A));
        }
        #endregion 正弦定理

        #region 余弦定理
        /// <summary>
        /// 三角形辺a,辺b,辺cから角度A(rad)を取得します
        /// 三角形ABCにおいて、BC = a, CA = B, AB = cとすると
        ///   A = ACos((b^2 + c^2 - a^2) / (2 * b * c))
        ///   B = Acos((a^2 + c^2 - b^2) / (2 * a * c))
        ///   C = Acos((b^2 + a^2 - c^2) / (2 * a * b))
        /// が成り立つ
        /// 第一余弦定理
        ///   a = b Cos(C) + c Cos(B)
        ///   b = c Cos(A) + a Cos(C)
        ///   c = a Cos(B) + b Cos(A)
        /// 第二余弦定理
        ///   a^2 = b^2 + c^2 - 2bc Cos(A)
        ///   b^2 = c^2 + a^2 - 2ca Cos(B)
        ///   c^2 = a^2 + b^2 - 2ab Cos(C)
        /// </summary>
        /// <param name="a">辺a</param>
        /// <param name="b">辺b</param>
        /// <param name="c">辺c</param>
        /// <returns>数値</returns>
        public static Primitive LawOfCosines(
            Primitive a,
            Primitive b,
            Primitive c
            )
        {
            return System.Math.Acos(
                ((double)b * (double)b 
                + (double)c * (double)c 
                - (double)a *(double)a)
                / (2 * (double)b * (double)c)
                );
        }
        #endregion 余弦定理

        #region 半径rの球の表面積(S)を取得します
        /// <summary>
        /// 半径rの球の表面積(S)を取得します
        /// S = 4 * PI * r^2
        /// </summary>
        /// <param name="r">半径</param>
        /// <returns>数値</returns>
        public static Primitive SurfaceAreaOfSphere(
            Primitive r
            )
        {
            return 4 * System.Math.PI * (double)r * (double)r;
        }
        #endregion 半径rの球の表面積(S)を取得します

        #region 半径(r)の球の体積(V)を取得します
        /// <summary>
        /// 半径(r)の球の体積(V)を取得します
        /// </summary>
        /// <param name="r">半径</param>
        /// <returns>数値</returns>
        public static Primitive VolumeOfSphere(
            Primitive r
            )
        {
            return 4 / 3 * System.Math.PI * (double)r * (double)r * (double)r;
        }
        #endregion 半径(r)の球の体積(V)を取得します

        #region 三角形の面積(S)を取得します
        /// <summary>
        /// 三角形の2辺a,bとその間の角C(rad)から三角形の面積(S)を取得します
        /// S = 1 / 2 * a * b * Sin(C)
        /// </summary>
        /// <param name="a">辺a</param>
        /// <param name="b">辺b</param>
        /// <param name="C">角度C(rad)</param>
        /// <returns>数値</returns>
        public static Primitive TriangleOfArea(
            Primitive a,
            Primitive b,
            Primitive C
            )
        {
            return 1 / 2 * (double)a * (double)b * System.Math.Sin((double)C);
        }
        /// <summary>
        /// 三角形の3辺a,b,cから三角形の面積(S)を取得します
        /// ヘロンの公式
        ///   s = (a + b + c) / 2
        ///   S = Sqrt(s * (s - a) * (s - b) * (s - c))
        /// </summary>
        /// <param name="a">辺a</param>
        /// <param name="b">辺b</param>
        /// <param name="c">辺c</param>
        /// <returns>数値</returns>
        public static Primitive TriangleOfArea2(
            Primitive a,
            Primitive b,
            Primitive c
            )
        {
            var s = ((double)a + (double)b + (double)c) / 2; 
            return System.Math.Sqrt(s * (s - (double)a) * (s - (double)b) * (s - (double)c));
        }
        #endregion 三角形の面積(S)を取得します

        #region 黄金比を求める
        /// <summary>
        /// 長方形短辺aの長辺bを黄金比で取得します
        /// 線分を a, b の長さで 2 つに分割するときに、
        /// a : b = b : (a + b) が成り立つように
        /// 分割したときの比 a : b のことであり、
        /// 最も美しい比(黄金比)とされる
        /// a : b = 1 : (1 + sqrt(5)) / 2
        /// b = (1 + sqrt(5)) / 2 * a
        /// </summary>
        /// <param name="a">短辺a</param>
        /// <returns>数値</returns>
        public static Primitive GetGoldenRatio(
            Primitive a
            )
        {
            return _Math.GetGoldenRatio((double)a);
        }
        #endregion 黄金比を求める

        #region 白銀比を求める
        /// <summary>
        /// 長方形短辺aの長辺bを白銀比で取得します
        /// 白銀比は用紙サイズに利用されます
        /// a : b = 1 : (1 + sqrt(2))
        /// b = (1 + sqrt(2)) * a
        /// </summary>
        /// <param name="a">短辺a</param>
        /// <returns>数値</returns>
        public static Primitive GetSilverRatio(
            Primitive a
            )
        {
            return _Math.GetSilverRatio((double)a);
        }
        #endregion 白銀比を求める

        #region フィボナッチ数一覧を取得する
        /// <summary>
        /// n番目までのフィボナッチ数一覧を取得します
        /// </summary>
        /// <param name="n">n番目</param>
        /// <returns>数値配列</returns>
        public static Primitive GetFibonacciNumbers(
            Primitive n
            )
        {
            var result = new List<double>() { 0, 1, 1 };

            for (int i = 0; i < (int)n; i++)
            {
                var idx = result.Count();
                result.Add(_Math.GetFibonacciNumber(result[idx - 2], result[idx - 1]));
            }

            return result.ConvertDimDoubleToPrimitive();
        }
        #endregion フィボナッチ数を取得する

        #region 最小値から最大値までの等差級数和(公差1)を取得します
        /// <summary>
        /// 最小値から最大値までの等差級数和(公差1)を取得します
        /// (max * (max + 1) - min * (min - 1)) / 2
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>結果</returns>
        public static Primitive SumArithmeticSeries(
            Primitive min,
            Primitive max
            )
        {
            var mi = (int)min;
            var ma = (int)max;
            return (ma * (ma + 1) - mi * (mi - 1)) / 2;
        }
        #endregion 最小値から最大値までの等差級数和(公差1)を取得します

        #region 3-nの素数一覧を取得します
        /// <summary>
        /// 3-nの素数一覧を取得します
        /// </summary>
        /// <param name="n">値</param>
        /// <returns>数値配列</returns>
        public static Primitive GetPrimeNumbers(
            Primitive n
            )
        {
            return _Math.GetPrimeNumbers((int)n).ConvertDimIntToPrimitive();
        }
        #endregion 3-nの素数一覧を取得します

        #region 最大公約数を求める
        /// <summary>
        /// 最大公約数(GreatestCommonDivisor)を求めます
        /// </summary>
        /// <param name="a">値a</param>
        /// <param name="b">値b</param>
        /// <returns>数値</returns>
        public static Primitive Gcd(
            Primitive a,
            Primitive b
            )
        {
            return _Math.Gcd((int)a, (int)b);
        }
        #endregion 最大公約数を求める

        #region 最小公倍数を求める
        /// <summary>
        /// 最小公倍数(LeastCommonMultiple)を求めます
        /// </summary>
        /// <param name="a">値a</param>
        /// <param name="b">値b</param>
        /// <returns>数値</returns>
        public static Primitive Lcm(
            Primitive a,
            Primitive b
            )
        {
            return _Math.Lcm((int)a, (int)b);
        }
        #endregion 最小公倍数を求める

        #region n!(階乗)を求める
        /// <summary>
        /// n!(階乗)を求めます
        /// </summary>
        /// <param name="n">値</param>
        /// <returns>数値</returns>
        public static Primitive Factorial(
            Primitive n
            )
        {
            return _Math.Factorial((int)n);
        }
        #endregion n!(階乗)を求める

        #region nPm(Permutation)を計算する
        /// <summary>
        /// nPm(Permutation)を計算します
        /// 異なる n個のものから m個を選んで並べる順列の総数を求めます
        /// nPm = n! / (n - m)!
        /// nPm = n(n - 1)(n - 2) ... (n - m + 1)
        /// </summary>
        /// <param name="n">元の数</param>
        /// <param name="m">選ぶ数</param>
        /// <returns>数値</returns>
        public static Primitive Permutation(
            Primitive n,
            Primitive m
            )
        {
            return _Math.Permutation((int)n, (int)m);
        }
        #endregion nPm(Permutation)を計算する

        #region nCm(Combination)を計算する
        /// <summary>
        /// nCm(Combination)を計算します
        /// 異なるｎ個のもの(元の数)から異なるｍ個のもの(選ぶ数)を
        /// 並べる順番の違いを区別せずに並べたもの＝重複（ちょうふく）を
        /// 持たない組合せを求めます
        /// nCm = n! / (m! * (n - m)!)
        /// nCm = nPm / m!
        /// nCm = n * (n - 1) * (n - 2) ... (n - m + 1) / (m * (m - 1) * (m - 2) ... * 1)
        /// </summary>
        /// <param name="n">元の数</param>
        /// <param name="m">選ぶ数</param>
        /// <returns>組み合わせ数</returns>
        public static Primitive Combination(
            Primitive n,
            Primitive m
            )
        {
            return _Math.Combination((int)n, (int)m);
        }
        #endregion nCm(Combination)を計算する

        #region 小数点以下の指定桁＋１桁で四捨五入します
        /// <summary>
        /// 小数点以下の指定桁＋１桁で四捨五入し、
        /// 結果を指定桁で取得します
        /// </summary>
        /// <param name="d">四捨五入する値</param>
        /// <param name="n">取得する小数点以下の桁(0 - 15)</param>
        /// <returns>四捨五入した値</returns>
        public static Primitive Round(
            Primitive d,
            Primitive n
            )
        {
            return _Math.Round((double)d, (int)n);
        }
        #endregion 小数点以下の指定桁＋１桁で四捨五入します

        #region 小数点以下の指定桁数＋１桁で切り上げます
        /// <summary>
        /// 小数点以下の指定桁＋１で切り上げし、
        /// 結果を指定桁で取得します
        /// </summary>
        /// <param name="d">切り上げる値</param>
        /// <param name="n">取得する小数点以下の桁(0 - 15)</param>
        /// <returns>切り上げた値</returns>
        public static Primitive Ceiling(
            Primitive d,
            Primitive n
            )
        {
            return _Math.Ceiling((double)d, (int)n);
        }
        #endregion 小数点以下の指定桁数＋１桁で切り上げます

        #region 小数点以下の指定桁数＋１桁で切り捨てます
        /// <summary>
        /// 小数点以下の指定桁＋１で切り捨てし、
        /// 結果を指定桁で取得します
        /// </summary>
        /// <param name="d">切り捨てる値</param>
        /// <param name="n">取得する小数点以下の桁(0 - 15)</param>
        /// <returns>切り捨てた値</returns>
        public static Primitive Floor(
            Primitive d,
            Primitive n
            )
        {
            return _Math.Floor((double)d, (int)n);
        }
        #endregion 小数点以下の指定桁数＋１桁で切り捨てます
    }
    #endregion SmallBasic用計算

    #region 計算
    /// <summary>
    /// 計算
    /// </summary>
    internal class Math
    {
        #region 黄金比を求める
        /// <summary>
        /// 長方形短辺aの長辺を黄金比で求める
        /// 線分を a, b の長さで 2 つに分割するときに、
        /// a : b = b : (a + b) が成り立つように
        /// 分割したときの比 a : b のことであり、
        /// 最も美しい比とされる
        /// 1 : (1 + sqrt(5)) / 2
        /// b = (1 + sqrt(5)) / 2 * a
        /// </summary>
        /// <param name="a">短辺a</param>
        /// <returns>数値</returns>
        public double GetGoldenRatio(
            double a
            )
        {
            return (1 + System.Math.Sqrt(5)) / 2 * a;
        }
        #endregion 黄金比を求める

        #region 白銀比を求める
        /// <summary>
        /// 長方形短辺aの長辺を白銀比で求めます
        /// a : b = 1 : (1 + sqrt(2))
        /// b = (1 + sqrt(2)) * a
        /// </summary>
        /// <param name="a">短辺a</param>
        /// <returns>数値</returns>
        public double GetSilverRatio(
            double a
            )
        {
            return (1 + System.Math.Sqrt(2)) * a;
        }
        #endregion 白銀比を求める

        #region 素数一覧を求める
        /// <summary>
        /// 1-nの素数一覧(2を除く)を求める
        /// </summary>
        /// <param name="n">値</param>
        /// <returns>数値配列</returns>
        public IEnumerable<int> GetPrimeNumbers(
            int n
            )
        {
            // 素数一覧を生成する
            var result = new List<int>();

            for (int i = 3; i < n; i+=2)
            {
                if (this.CheckPrimeNumber(i, result))
                    result.Add(i);
            }

            return result;
        }
        /// <summary>
        /// nが素数か検証する
        /// 素数の場合trueを返す
        /// </summary>
        /// <param name="n">検証する値</param>
        /// <param name="pnl">n未満の素数リスト</param>
        /// <returns>数値配列</returns>
        private bool CheckPrimeNumber(
            int n,
            IEnumerable<int> pnl
            )
        {
            foreach (var v in pnl)
            {
                if (n % v == 0)
                    return false;
            }

            return true;
        }
        #endregion 素数一覧を求める

        #region フィボナッチ数を取得する
        /// <summary>
        /// フィボナッチ数を取得する
        /// fn2 = fn0 + fn1
        /// </summary>
        /// <param name="fn0">fn0</param>
        /// <param name="fn1">fn1</param>
        /// <returns>値</returns>
        public double GetFibonacciNumber(
            double fn0,
            double fn1
            )
        {
            // オーバーフローチェック
            checked
            {
                return fn0 + fn1;
            }
        }
        #endregion フィボナッチ数を取得する

        #region 最小値から最大値までの等差級数和(公差1)を求める
        /// <summary>
        /// 最小値から最大値までの等差級数和(公差1)を求める
        /// (max * (max + 1) - min * (min - 1)) / 2
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>結果</returns>
        public int SumArithmeticSeries(
            int min,
            int max
            )
        {
            return (max * (max + 1) - min * (min - 1)) / 2;
        }
        #endregion 最小値から最大値までの等差級数和(公差1)を求める

        #region 最大公約数を求める
        /// <summary>
        /// 最大公約数を求める
        /// </summary>
        /// <remarks>
        /// ユークリッド互除法
        /// 2 つの自然数 a, b (a ≧ b) について、
        /// a の b による剰余を r とすると、 
        /// a と b との最大公約数は 
        /// b と r との最大公約数に等しい
        /// という性質が成り立つ。
        /// この性質を利用して、 
        /// b を r で割った剰余、 
        /// 除数 r をその剰余で割った剰余、
        /// と剰余を求める計算を逐次繰り返すと、
        /// 剰余が 0 になった時の除数が a と b との
        /// 最大公約数となる。 
        /// </remarks>
        /// <param name="a">値a</param>
        /// <param name="b">値b</param>
        /// <returns>最大公約数</returns>
        public int Gcd(
            int a,
            int b
            )
        {
            /*
            // a >= bでない場合は入れ替える
            if (a < b)
            {
                // a, bを入れ替えて自分を呼び出す
                return EuclideanAlgorithm(b, a);
            }
            // ユークリッド互除法を実行する
            while (b != 0)
            {
                var r = a % b;
                a = b;
                b = r;
            } 

            return a;
            */
            return a >= b
                ? this.EuclideanAlgorithmBase(a, b)
                : this.EuclideanAlgorithmBase(b, a);

        }
        /// <summary>
        /// ユークリッド互除法を実行する
        /// </summary>
        /// <param name="a">値a</param>
        /// <param name="b">値b</param>
        /// <returns>結果</returns>
        private int EuclideanAlgorithmBase(
            int a,
            int b
            )
        {
            return b != 0
                ? this.EuclideanAlgorithmBase(b, a % b)
                : a;
        }
        #endregion 最大公約数を求める

        #region 最小公倍数を求める
        /// <summary>
        /// 最小公倍数を求める
        /// </summary>
        /// <remarks>
        /// 最小公倍数 * 最大公約数 = a * b
        /// 正の整数 a,b に対して，それらの最大公約数を g，
        /// 最小公倍数を l とおくと ab = gl となる。
        /// 最大公約数と最小公倍数の積がもとの二つの数の積に等しい
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>結果</returns>
        public int Lcm(
            int a,
            int b
            )
        {
            // 最小公倍数を求める
            return a * b / this.Gcd(a, b);
        }
        #endregion 最小公倍数を求める

        #region nの階乗(n!)を計算する
        /// <summary>
        /// nの階乗(n!)を計算します
        /// n >= 0
        /// 負数の場合例外発生
        /// </summary>
        /// <param name="n">n</param>
        /// <returns>n!</returns>
        public double Factorial(
            int n
            )
        {
            // 負数チェック
            if (n < 0)
                throw new ArgumentOutOfRangeException("n is less 0");

            // nの階乗を計算する
            return _Factorial(n);
        }
        private double _Factorial(
            int n
            )
        {
            // オーバーフローをチェックする
            checked
            {
                return n == 0 ? 1L : n * _Factorial(n - 1);
            }
        }
        #endregion 階乗(!)を計算する

        #region nPm(Permutation)を計算する
        /// <summary>
        /// nPm(Permutation)を計算します
        /// 異なる n個のものから m個を選んで並べる順列の総数を求めます
        /// nPm = n! / (n - m)!
        /// nPm = n(n - 1)(n - 2) ... (n - m + 1)
        /// </summary>
        /// <param name="n">元の数</param>
        /// <param name="m">選ぶ数</param>
        /// <returns>並べ方数</returns>
        public decimal Permutation(
            int n,
            int m
            )
        {
            decimal result = 1;
            int f = n - m;

            while (n > f)
            {
                result *= n--;
            }

            return result;
        }
        #endregion nPm(Permutation)を計算する

        #region nCm(Combination)を計算する
        /// <summary>
        /// nCm(Combination)を計算する
        /// 異なるｎ個のもの(元の数)から異なるｍ個のもの(選ぶ数)を
        /// 並べる順番の違いを区別せずに並べたもの＝重複（ちょうふく）を
        /// 持たない組合せ
        /// nCm = n! / (m! * (n - m)!)
        /// nCm = nPm / m!
        /// nCm = n * (n - 1) * (n - 2) ... (n - m + 1) / (m * (m - 1) * (m - 2) ... * 1)
        /// </summary>
        /// <param name="n">元の数</param>
        /// <param name="m">選ぶ数</param>
        /// <returns>組み合わせ数</returns>
        public decimal Combination(
            int n,
            int m
            )
        {
            // nCm = n * (n - 1) * (n - 2) ... (n - m + 1) / (m * (m - 1) * (m - 2) ... * 1)
            decimal result = 1;
            int f = n - m;

            while (n > f)
            {
                result *= (n-- / (m <= 1 ? 1 : m--));
            }

            return result;
        }
        #endregion nCm(コンビネーション)を計算する

        #region 小数点以下の指定桁＋１桁で四捨五入します
        /// <summary>
        /// 小数点以下の指定桁＋１桁で四捨五入し、
        /// 結果を指定桁で取得します
        /// </summary>
        /// <param name="value">四捨五入する値</param>
        /// <param name="decimals">取得する桁(0 - 15)</param>
        /// <returns>結果</returns>
        public double Round(
            double value,
            int decimals
            )
        {
            // 入力チェック
            if (decimals < 0 || decimals > 15)
                throw new ArgumentOutOfRangeException();

            return System.Math.Round(value, decimals, MidpointRounding.AwayFromZero);
        }
        #endregion 小数点第n位で四捨五入する

        #region 小数点以下の指定桁＋１桁で切り上げます
        /// <summary>
        /// 小数点以下の指定桁＋１桁で切り上げし、
        /// 結果を指定桁で取得します
        /// </summary>
        /// <param name="value">切り上げする値</param>
        /// <param name="decimals">取得する桁(0 - 15)</param>
        /// <returns>結果</returns>
        public double Ceiling(
            double value,
            int decimals
            )
        {
            double pow = System.Math.Pow(10, decimals);
            return value > 0 
                ? System.Math.Ceiling(value * pow) / pow
                : System.Math.Floor(value * pow) / pow;
        }
        #endregion 小数点以下の指定桁＋１桁で切り上げます

        #region 小数点以下の指定桁＋１桁で切り捨てます
        /// <summary>
        /// 小数点以下の指定桁＋１桁で切り捨てし、
        /// 結果を指定桁で取得します
        /// </summary>
        /// <param name="value">切り捨てする値</param>
        /// <param name="decimals">取得する桁(0 - 15)</param>
        /// <returns>結果</returns>
        public double Floor(
            double value,
            int decimals
            )
        {
            double pow = System.Math.Pow(10, decimals);
            return value > 0
                ? System.Math.Floor(value * pow) / pow
                : System.Math.Ceiling(value * pow) / pow;
        }
        #endregion 小数点以下の指定桁＋１桁で切り捨てます
    }
    #endregion 計算
}
