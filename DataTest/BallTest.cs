using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System.ComponentModel;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        private BallAPI ball;

        [TestInitialize]
        public void Setup()
        {
            ball = BallAPI.createBall(1, 50, 50, 10, "red", 1, 1, 1);
        }

        [TestMethod]
        public void TestBallInitialization()
        {
            Assert.AreEqual(1, ball.ID);
            Assert.AreEqual(50, ball.X);
            Assert.AreEqual(50, ball.Y);
            Assert.AreEqual(10, ball.Radius);
            Assert.AreEqual("red", ball.color);
            Assert.AreEqual(1, ball.XDirection);
            Assert.AreEqual(1, ball.YDirection);
            Assert.AreEqual(1, ball.weight);
            Assert.AreEqual(1, ball.XSpeed);
            Assert.AreEqual(1, ball.YSpeed);
            Assert.IsTrue(ball.IsMoving);
        }

        [TestMethod]
        public void TestBallMovementChangesPosition()
        {
            double oldX = ball.XPosition;
            double oldY = ball.YPosition;

            ball.MoveBall(500, 500);

            Assert.AreNotEqual(oldX, ball.XPosition, "XPosition should change.");
            Assert.AreNotEqual(oldY, ball.YPosition, "YPosition should change.");
        }

        [TestMethod]
        public void TestBallReversesDirectionOnBoundary()
        {
            // Move ball close to right edge
            ball.XPosition = 500;
            ball.YPosition = 500;

            ball.MoveBall(500, 500);

            Assert.AreEqual(-1, ball.XDirection, "XDirection should reverse.");
            Assert.AreEqual(-1, ball.YDirection, "YDirection should reverse.");
        }
    }
}
