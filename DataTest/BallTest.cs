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
            var ball = Ball.createBall(1,0, 1, 2, "black");
            Assert.AreEqual(1, ball.ID);
            Assert.AreEqual(0, ball.X);
            Assert.AreEqual(1, ball.Y);
            Assert.AreEqual(2, ball.Radius);
            Assert.AreEqual<string>("black", ball.color);
            
        }
    }
}