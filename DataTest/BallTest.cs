using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void BallConstructorTest()
        {
            var ball = BallAPI.createBall(1,0, 1, 2, "black", 1, 2);
            Assert.AreEqual(1, ball.ID);
            Assert.AreEqual(0, ball.X);
            Assert.AreEqual(1, ball.Y);
            Assert.AreEqual(2, ball.Radius);
            Assert.AreEqual(1, ball.XDirection);
            Assert.AreEqual(2, ball.YDirection);
            Assert.AreEqual<string>("black", ball.color);
            
        }
    }
}