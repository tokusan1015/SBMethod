using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    #region SmallBasic用About
    /// <summary>
    /// SmallBasic用About
    /// </summary>
    [SmallBasicType]
    public static class SBAbout
    {
        #region 固定値
        /// <summary>
        /// コンタクト
        /// </summary>
        private static readonly string _Contact = "tomsan_1015-contact@yahoo.co.jp";
        #endregion 固定値

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static SBAbout()
        {
            ;
        }
        #endregion コンストラクタ

        #region 公開プロパティ
        /// <summary>
        /// タイトル
        /// </summary>
        public static Primitive Title
        {
            get { return AssemblyTitle; }
        }
        /// <summary>
        /// バージョン
        /// </summary>
        public static Primitive Version
        {
            get { return AssemblyVersion; }
        }
        /// <summary>
        /// 説明
        /// </summary>
        public static Primitive Description
        {
            get { return AssemblyDescription; }
        }
        /// <summary>
        /// プロダクト
        /// </summary>
        public static Primitive Product
        {
            get { return AssemblyProduct; }
        }
        /// <summary>
        /// コピーライト
        /// </summary>
        public static Primitive Copyright
        {
            get { return AssemblyCopyright; }
        }
        /// <summary>
        /// 会社
        /// </summary>
        public static Primitive Company
        {
            get { return AssemblyCompany; }
        }
        /// <summary>
        /// コンタクト
        /// </summary>
        public static Primitive Contact
        {
            get { return _Contact; }
        }

        #endregion 公開プロパティ

        #region メソッド

        #region アセンブリ属性アクセサー

        #region タイトル
        /// <summary>
        /// タイトル
        /// </summary>
        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != string.Empty)
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }
        #endregion タイトル

        #region バージョン
        /// <summary>
        /// アセンブリバージョン
        /// </summary>
        private static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        #endregion バージョン

        #region 説明
        /// <summary>
        /// アセンブリ説明
        /// </summary>
        private static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }
        #endregion 説明

        #region プロダクト
        /// <summary>
        /// プロダクト
        /// </summary>
        private static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }
        #endregion プロダクト

        #region コピーライト
        /// <summary>
        /// コピーライト
        /// </summary>
        private static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
        #endregion コピーライト

        #region 会社
        /// <summary>
        /// 会社
        /// </summary>
        private static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion 会社

        #endregion アセンブリ属性アクセサー

        #endregion メソッド
    }
    #endregion SmallBasic用About
}
