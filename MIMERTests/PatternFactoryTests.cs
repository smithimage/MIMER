using System;
using MIMER;
using NUnit.Framework;

namespace MIMERTests
{
    [TestFixture]
    public class PatternFactoryTests
    {
        [Test]
        public void GetTest()
        {
            IPattern pattern = PatternFactory.GetInstance().Get(typeof (MIMER.RFC822.Pattern.TokenPattern));
            Assert.IsInstanceOfType(typeof(MIMER.RFC822.Pattern.TokenPattern), pattern);
            IPattern pattern2 = PatternFactory.GetInstance().Get(typeof (MIMER.RFC822.Pattern.TokenPattern));
            Assert.AreSame(pattern, pattern2);
        }

        [Test]
        public void GetWrongTypeTest()
        {
            try
            {
                IPattern pattern = PatternFactory.GetInstance().Get(typeof (String));
                Assert.Fail("Expected exception " + typeof(ArgumentException).Name);
            }
            catch(Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentException), ex.GetType());
            }
        }
    }
}
