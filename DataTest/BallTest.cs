using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System.ComponentModel;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        private BallAPI ball;
        private System.Timers.Timer colorTimer;

        [TestInitialize]
        public void Setup()
        {
            colorTimer = new System.Timers.Timer(3000);
            colorTimer.AutoReset = true;
            colorTimer.Start();

            ball = BallAPI.createBall(1, 50, 50, 10, "red", 1, 1, 1, colorTimer);
        }

        [TestMethod]
        public void TestBallInitialization()
        {
            Assert.AreEqual(1, ball.ID);
            Assert.AreEqual(50, ball.X);
            Assert.AreEqual(50, ball.Y);
            Assert.AreEqual(10, ball.Radius);
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
