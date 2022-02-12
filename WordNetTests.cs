using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML.WordNetLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ML.WordNetLibrary.Tests
{
    [TestClass()]
    public class WordNetTests
    {
        

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [DataSource("ToWordNodesDataSource")]
        [TestMethod()]
        public void ToWordNodeTest()
        {
            string word =  TestContext.DataRow["Word"].ToString().ToWordNode().Word;
            string expected = TestContext.DataRow["ExpectedWord"] as string;
            Assert.AreEqual(word, expected);
        }

        [DataSource("ToWordNodesDataSource")]
        [TestMethod()]
        public void RootNodeTest()
        {
            
        }


        [TestMethod()]
        public void ToWordNodesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IncludeToStopWordsDictionaryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IncludeToKnowWordsDictionaryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsInStopWordsDictionaryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsInKnownWordsDictionaryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetKnownWordsListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetStopWordsListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSemanticNodeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ConnectToSemanticNodeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToSemanticNodesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ConnectAsSynonymTest()
        {
         
        }

        [TestMethod()]
        public void ExportSpaceTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ImportSpaceTest()
        {
            Assert.Fail();
        }
    }
}
