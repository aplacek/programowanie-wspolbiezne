using System;
using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;

namespace BusinessLogicTests
{
    [TestClass]
    public class BallLogicTests
    {
        private BallLogicAPI ballLogic;

        [TestInitialize]
        public void Setup()
        {
            ballLogic = BallLogicAPI.CreateLogic(500, 400);
        }

        [TestMethod]
        public void TestCreateBall()
        {
            int initialCount = ballLogic.GetBalls().Count;
            ballLogic.CreateBall(1, 50, 50, 10, "red", 1, 1);
            int newCount = ballLogic.GetBalls().Count;

            Assert.AreEqual(initialCount + 1, newCount, "Ball count did not increase after creating a ball.");
        }

        [TestMethod]
        public void TestMoveBall()
        {
            ballLogic.CreateBall(2, 100, 100, 10, "blue", 1, 1);
            var ball = ballLogic.GetBalls().Find(b => b.ID == 2);

            double oldX = ball.X;
            double oldY = ball.Y;

            ballLogic.MoveBall(ball);

            Assert.AreNotEqual(oldX, ball.X, "Ball X coordinate did not change after moving.");
            Assert.AreNotEqual(oldY, ball.Y, "Ball Y coordinate did not change after moving.");
        }

        [TestMethod]
        public void TestWindowCollision()
        {
            ballLogic.CreateBall(3, 5, 5, 5, "green", -1, -1);
            var ball = ballLogic.GetBalls().Find(b => b.ID == 3);

            int oldXDir = ball.XDirection;
            int oldYDir = ball.YDirection;

            ballLogic.MoveBall(ball);

            Assert.AreEqual(-oldXDir, ball.XDirection, "Ball XDirection not reversed after window collision.");
            Assert.AreEqual(-oldYDir, ball.YDirection, "Ball YDirection not reversed after window collision.");
        }

        [TestMethod]
        public void TestElasticRebound()
        {
            ballLogic.CreateBall(4, 200, 200, 10, "yellow", 1, 0);
            ballLogic.CreateBall(5, 210, 200, 10, "purple", -1, 0);

            var ballOne = ballLogic.GetBalls().Find(b => b.ID == 4);
            var ballTwo = ballLogic.GetBalls().Find(b => b.ID == 5);

            int oldBallOneXDir = ballOne.XDirection;
            int oldBallTwoXDir = ballTwo.XDirection;

            ballLogic.ElasticRebound(ballOne, ballTwo);

            Assert.AreEqual(oldBallTwoXDir, ballOne.XDirection, "BallOne XDirection did not swap correctly after collision.");
            Assert.AreEqual(oldBallOneXDir, ballTwo.XDirection, "BallTwo XDirection did not swap correctly after collision.");
        }

        [TestMethod]
        public void TestCollisionOccurrence()
        {
            ballLogic.CreateBall(6, 300, 300, 10, "orange", 1, 0);
            ballLogic.CreateBall(7, 310, 300, 10, "black", -1, 0);

            var ballOne = ballLogic.GetBalls().Find(b => b.ID == 6);
            var ballTwo = ballLogic.GetBalls().Find(b => b.ID == 7);

            bool collisionOccurred = ballLogic.CollisionOccurence(ballOne, ballTwo);

            Assert.IsTrue(collisionOccurred, "Collision should have occurred but didn't.");
        }

        [TestMethod]
        public void TestClearMap()
        {
            ballLogic.CreateRandomBall();
            ballLogic.CreateRandomBall();
            ballLogic.ClearMap();

            Assert.AreEqual(0, ballLogic.GetBalls().Count, "Ball list should be empty after clearing map.");
        }

        [TestMethod]
        public void TestStartStopAnimation()
        {
            try
            {
                ballLogic.StartAnimation();
                ballLogic.StopAnimation();
                Assert.IsTrue(true); // No exception = test passed
            }
            catch (Exception)
            {
                Assert.Fail("Exception thrown during StartAnimation/StopAnimation.");
            }
        }
    }
}
