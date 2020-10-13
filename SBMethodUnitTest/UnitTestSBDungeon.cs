using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SmallBasic.Library;
using SBMethod;

namespace SBMethodUnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestSBDungeon()
        {
            SBMethod.SBDungeon.Create(
                roomSize: 3,
                xAreaCounts: 5,
                yAreaCounts: 5,
                areaPercent: 33,
                roomPercent: 33
                );

            var dd = SBMethod.SBDungeon.GetDungeonData();
            ;
        }
        [TestMethod]
        public void TestClassHex()
        {

        }
    }
}
