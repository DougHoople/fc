using Microsoft.VisualStudio.TestTools.UnitTesting;
using fc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fc.Tests
{
    [TestClass()]
    public class VocabEntryTests
    {
        [TestMethod()]
        public void PredictPluralTest()
        {
            var vocabEntry = new VocabEntry { gender = Gender.feminine, portuguese = "Tochter" };
            //var vocabEntry = new VocabEntry { gender = Gender.feminine, portuguese = "Mutter" };
            var plural = vocabEntry.PredictPlural();
            Assert.IsTrue(plural == "Töchter");
            // Assert.IsTrue(plural == "Mütter");
        }
    }
}