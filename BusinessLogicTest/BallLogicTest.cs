using System;
using System.Linq;
using System.Threading;
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

            Assert.AreEqual(initialCount + 1, newCount);
        }


        [TestMethod]
        public void TestMoveBall()
        {
            ballLogic.CreateBall(2, 100, 100, 10, "blue", 1, 1);
            var ball = ballLogic.GetBalls().FirstOrDefault(b => b.ID == 2);

            Assert.IsNotNull(ball);

            double oldX = ball.X;
            double oldY = ball.Y;
            ball.IsMoving = true;

            ballLogic.MoveBall(ball);

            Assert.AreNotEqual(oldX, ball.X);
            Assert.AreNotEqual(oldY, ball.Y);

        }


        [TestMethod]
        public void TestElasticRebound()
        {
            ballLogic.CreateBall(4, 200, 200, 10, "yellow", 1, 0);
            ballLogic.CreateBall(5, 210, 200, 10, "purple", -1, 0);

            var ballOne = ballLogic.GetBalls().FirstOrDefault(b => b.ID == 4);
            var ballTwo = ballLogic.GetBalls().FirstOrDefault(b => b.ID == 5);

            Assert.IsNotNull(ballOne);
            Assert.IsNotNull(ballTwo);

            int oldX1 = ballOne.XDirection;
            int oldX2 = ballTwo.XDirection;

            ballLogic.ElasticRebound(ballOne, ballTwo);

            Assert.AreEqual(oldX2, ballOne.XDirection);
            Assert.AreEqual(oldX1, ballTwo.XDirection);

        }

        [TestMethod]
        public void TestCollisionOccurrence()
        {
            ballLogic.CreateBall(6, 300, 300, 10, "orange", 1, 0);
            ballLogic.CreateBall(7, 310, 300, 10, "black", -1, 0);

            var ballOne = ballLogic.GetBalls().FirstOrDefault(b => b.ID == 6);
            var ballTwo = ballLogic.GetBalls().FirstOrDefault(b => b.ID == 7);

            Assert.IsNotNull(ballOne);
            Assert.IsNotNull(ballTwo);

        }

        [TestMethod]
        public void TestClearMap()
        {
            ballLogic.CreateRandomBall();
            ballLogic.CreateRandomBall();

            Assert.IsTrue(ballLogic.GetBalls().Count > 0);

            ballLogic.ClearMap();

            Assert.AreEqual(0, ballLogic.GetBalls().Count);
        }

        [TestMethod]
        public void TestStartStopAnimation()
        {
            try
            {
                ballLogic.StartAnimation();
                Thread.Sleep(100);
                ballLogic.StopAnimation();

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception during animation start/stop: {ex.Message}");
            }
        }

        [TestMethod]
        public void ColorChangesAfterFunctionUse()
        {
            double mapWidth = 500;
            double mapHeight = 500;
            var logic = BallLogicAPI.CreateLogic(mapWidth, mapHeight);

            logic.CreateBall(1, 100, 100, 10, "red", 1, 1);
            var ball = logic.GetBalls().First();

            string initialColor = ball.Color;

            ball.ChangeColorRandomly();

            Assert.AreNotEqual(initialColor, ball.Color, "Kolor powinien się zmienić po wywołaniu ChangeColorRandomly");
        }
        
        // [TestMethod]
        // public void ColorChangesAfterTimerElapsed()
        // {
        //     double mapWidth = 500;
        //     double mapHeight = 500;
        //     var logic = BallLogicAPI.CreateLogic(mapWidth, mapHeight);

        //     logic.CreateBall(1, 100, 100, 10, "red", 1, 1);
        //     var ball = logic.GetBalls().First();

        //     string initialColor = ball.Color;

        //     Thread.Sleep(4000); // wait for the timer to trigger color change

        //     Assert.AreNotEqual(initialColor, ball.Color, "Kolor powinien się zmienić po czasie 3 sekund");
        // }
    }
}
