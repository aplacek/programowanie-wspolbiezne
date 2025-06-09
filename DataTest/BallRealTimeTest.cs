using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System;
using System.Threading;

namespace DataTests
{
    [TestClass]
    public class BallRealTimeTests
    {
        private BallAPI ball;
        private System.Timers.Timer colorTimer;

        [TestInitialize]
        public void Setup()
        {
            colorTimer = new System.Timers.Timer(3000);
            colorTimer.AutoReset = true;
            colorTimer.Start();

            ball = BallAPI.createBall(
                ID: 1, 
                X: 100, 
                Y: 100, 
                radius: 5, 
                color: "red", 
                XDirection: 1, 
                YDirection: 0, 
                weight: 1,
                timer: colorTimer
            );
            ball.XSpeed = 10;
            ball.YSpeed = 0;
        }

        [TestMethod]
        public void MoveBall_RealTimeMovement_IncreasesByApproximatelySpeedTimesDelta()
        {
            double startX = ball.XPosition;

            ball.MoveBall(mapWidth: 500, mapHeight: 500);
            
            Thread.Sleep(200);

            ball.MoveBall(mapWidth: 500, mapHeight: 500);

            double deltaX = ball.XPosition - startX;

            Assert.IsTrue(
                deltaX > 1.5 && deltaX < 2.5,
                $"Oczekiwano przyrostu około 2, teraz wynosi {deltaX:F2}"
            );
        }

        [TestMethod]
        public void MoveBall_ImmediateSecondCall_AlmostNoMovement()
        {

            double x1 = ball.XPosition;
            ball.MoveBall(500, 500);
            double x2 = ball.XPosition;

            ball.MoveBall(500, 500);
            double x3 = ball.XPosition;

            double delta = x3 - x2;
            Assert.IsTrue(
                Math.Abs(delta) < 0.01,
                $"Przy natychmiastowym wywołaniu delta powinna być bliska 0, a jest {delta:F4}"
            );
        }
    }
}
