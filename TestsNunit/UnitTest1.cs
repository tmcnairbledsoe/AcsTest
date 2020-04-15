using NUnit.Framework;
using AcsTest;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TestsNunit
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            int score = AcsTest.Program.Score("xxxxxxxxxxxx");
            Assert.Equals(score,300);
        }
        [Test]
        public void Test2()
        {
            int score = AcsTest.Program.Score("12121212121212121212");
            Assert.Equals(score, 30);
        }
        [Test]
        public void Test3()
        {
            int score = AcsTest.Program.Score("5/500000000000000000");
            Assert.Equals(score, 20);
        }
        [Test]
        public void Test4()
        {
            int score = AcsTest.Program.Score("x530000000000000000");
            Assert.Equals(score, 26);
        }
        [Test]
        public void Test5()
        {
            int score = AcsTest.Program.Score("8/8052x4/258/x620/5");
            Assert.Equals(score, 133);
        }
    }
}