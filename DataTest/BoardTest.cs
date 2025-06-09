using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System.ComponentModel;
using System.Linq;

namespace DataTest
{
    [TestClass]
    public class BoardTest
    {
        private BoardAPI board;
        private System.Timers.Timer colorTimer;

        [TestInitialize]
        public void Setup()
        {
            board = BoardAPI.CreateBoard(300, 400, "white");
            colorTimer = new System.Timers.Timer(3000);
            colorTimer.AutoReset = true;
            colorTimer.Start();
        }

        [TestMethod]
        public void TestBoardInitialization()
        {
            Assert.AreEqual(300, board.Height, "Height not set correctly.");
            Assert.AreEqual(400, board.Width, "Width not set correctly.");
            Assert.AreEqual("white", board.Color, "Color not set correctly.");
        }

        [TestMethod]
        public void TestAddBall()
        {
            var ball = BallAPI.createBall(1, 50, 50, 10, "red", 1, 1, 5, colorTimer);
            board.AddBall(ball);

            Assert.AreEqual(1, board.GetBalls().Count, "Ball not added.");
            Assert.AreSame(ball, board.GetBalls().First(), "Incorrect ball added.");
        }

        [TestMethod]
        public void TestRemoveBalls()
        {
            var ball1 = BallAPI.createBall(1, 50, 50, 10, "red", 1, 1, 5, colorTimer);
            var ball2 = BallAPI.createBall(2, 100, 100, 10, "blue", -1, -1, 5, colorTimer);

            board.AddBall(ball1);
            board.AddBall(ball2);
            Assert.AreEqual(2, board.GetBalls().Count);

            board.RemoveBalls();

            Assert.AreEqual(0, board.GetBalls().Count, "Balls not removed.");
        }

    }
}
