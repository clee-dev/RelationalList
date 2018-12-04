using Microsoft.VisualStudio.TestTools.UnitTesting;
using RelationalList;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class RelationalListTests
    {
        private RelationalList<string, int> Test;

        [TestInitialize]
        public void Initialize()
        {
            Test = new RelationalList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void Brackets_Get()
        {
            Assert.AreEqual(1, Test["one"]);
            Assert.AreEqual(2, Test["two"]);
            Assert.AreEqual(3, Test["three"]);
            Assert.AreEqual("one", Test[1]);
            Assert.AreEqual("two", Test[2]);
            Assert.AreEqual("three", Test[3]);
        }

        [TestMethod]
        public void Brackets_Set()
        {
            Test["one"] = 10;
            Test[5] = "fifteen";
            Assert.AreEqual("one", Test[10]);
            Assert.AreEqual(5, Test["fifteen"]);
        }

        [TestMethod]
        public void ToRelationalList()
        {
            int[] numbers = new int[] { 1, 2, 3, 4, 5 };
            string[] strings = new string[] { "one", "two", "three", "four", "five" };

            var test = numbers.ToRelationalList(strings);

            Assert.AreEqual("one", test[1]);
            Assert.AreEqual("two", test[2]);
            Assert.AreEqual("three", test[3]);
            Assert.AreEqual("four", test[4]);
            Assert.AreEqual("five", test[5]);
            Assert.AreEqual(1, test.ElementAt(0).X);
            Assert.AreEqual("five", test.ElementAt(4).Y);
        }

        [TestMethod]
        public void ForEach()
        {
            StringBuilder sb = new StringBuilder();
            foreach (RelationalListPair<string, int> x in Test)
            {
                sb.Append(x.X + x.Y.ToString());
            }

            Assert.AreEqual("one1two2three3four4five5", sb.ToString());
        }

        [TestMethod]
        public void UniqueTypes()
        {
            try
            {
                var test = new RelationalList<string, string>();
                Assert.Fail();
            }
            catch
            {
                //pass - threw an exception when given invalid types
            }
        }

        [TestMethod]
        public void ElementAt()
        {
            Assert.AreEqual(1, Test.ElementAt(0).Y);
            Assert.AreEqual(3, Test.ElementAt(2).Y);
            Assert.AreEqual(5, Test.ElementAt(4).Y);
        }

        [TestMethod]
        public void Add_Individual()
        {
            Test.Add("abc", 123);

            Assert.AreEqual(123, Test["abc"]); //test it was added
            Assert.AreEqual("abc", Test.Last().X); //test it was added *to the end*
            Assert.AreEqual(6, Test.Count); //test count was increased
        }

        [TestMethod]
        public void Add_Pair()
        {
            Test.Add(new RelationalListPair<string, int>("abc", 123));

            Assert.AreEqual(123, Test["abc"]); //test it was added
            Assert.AreEqual("abc", Test.Last().X); //test it was added *to the end*
            Assert.AreEqual(6, Test.Count); //test count was increased
        }

        [TestMethod]
        public void AddRange_Individual()
        {
            int c = Test.Count;

            List<string> strings = new List<string> { "six", "seven" };
            List<int> ints = new List<int> { 6, 7 };

            Test.AddRange(strings, ints);

            Assert.AreEqual(6, Test["six"]); //test it was added
            Assert.AreEqual(7, Test["seven"]); //test it was added
            Assert.AreEqual("seven", Test.Last().X); //test it was added *to the end*
            Assert.AreEqual(7, Test.Count); //test count was increased
        }

        [TestMethod]
        public void AddRange_Pair()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Insert_Individual()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Insert_Pair()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void InsertRange_Individual()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void InsertRange_Pair()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Remove_Individual()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Remove_Pair()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RemoveRange()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RemoveAt()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Clear()
        {
            Test.Clear();

            Assert.AreEqual(0, Test.Count);
            Assert.AreEqual(0, Test.X.Count);
            Assert.AreEqual(0, Test.Y.Count);
        }

        [TestMethod]
        public void IndexOf_Individual()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void IndexOf_Pair()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Contains_Individual()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Contains_Pair()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CopyTo()
        {
            object[,] arr = new object[,]
            {
                { 8, "eight" },
                { 12893, "twelve thousand eight hundred ninety three" },
                { 10, "ten" },
                { 3859892359823395235, "n/a" },
                { 1, "one more" }
            };
            Test.CopyTo(arr);

            Assert.AreEqual("five", arr[4, 0]);
            Assert.AreEqual("one", arr[0, 0]);
            Assert.AreEqual(5, arr[4, 1]);
        }

        [TestMethod]
        public void ToArray()
        {
            var a = Test.ToArray();

            Assert.AreEqual(2, a[1, 1]);
            Assert.AreEqual("five", a[4, 0]);
            Assert.AreEqual("three", a[2, 0]);
        }

        [TestMethod]
        public void ToList()
        {
            var l = Test.ToList();

            Assert.AreEqual("three", l[2].X);
            Assert.AreEqual(1, l[0].Y);
            Assert.AreEqual(4, l[3].Y);
        }

        [TestMethod]
        public void Reverse()
        {
            Test.Reverse();

            Assert.AreEqual(5, Test.First().Y);
            Assert.AreEqual(1, Test.Last().Y);
            Assert.AreEqual("three", Test.ElementAt(2).X);
        }

        [TestMethod]
        public void Reversed()
        {
            var t = Test.Reversed();

            Assert.AreEqual(5, t.First().Y);
            Assert.AreEqual(1, Test.First().Y);
            Assert.AreEqual(1, t.Last().Y);
            Assert.AreEqual(5, Test.Last().Y);
        }

        [TestMethod]
        public void FromDictionary()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ConvertTo()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Duplicates_NotAllowed()
        {
            try
            {
                Test.Add("one", 1);
                Assert.Fail();
            }
            catch
            {
                //pass - threw an exception when given duplicate data
            }
        }
    }
}