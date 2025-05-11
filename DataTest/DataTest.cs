using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System.Collections.Generic;
using System.Threading;

namespace DataTests

{
    [TestClass]
    public class DataLayerTests
    { 
        private DataLayerAPI dataLayer;

        [TestInitialize]
        public void Setup()
        {
            dataLayer = DataLayerAPI.CreateData(400, 500, "white");
        }

        [TestMethod]
        public void TestAddBall()
        {
            int initialCount = dataLayer.GetBalls().Count;

            var ball = BallAPI.createBall(1, 100, 100, 10, "red", 1, 1, 2);
            dataLayer.AddBall(ball);

            int newCount = dataLayer.GetBalls().Count;

            Assert.AreEqual(initialCount + 1, newCount);
        }

        [TestMethod]
        public void TestGetBalls()
        {
            var ball1 = BallAPI.createBall(2, 150, 150, 10, "blue", 1, 0, 2);
            var ball2 = BallAPI.createBall(3, 200, 200, 10, "green", 0, 1, 2);

            dataLayer.AddBall(ball1);
            dataLayer.AddBall(ball2);

            List<BallAPI> balls = dataLayer.GetBalls();

            Assert.AreEqual(2, balls.Count);
            Assert.IsTrue(balls.Exists(b => b.ID == 2), "Ball with ID 2 not found.");
            Assert.IsTrue(balls.Exists(b => b.ID == 3), "Ball with ID 3 not found.");
        }

        [TestMethod]
        public void TestRemoveBalls()
        {
            dataLayer.AddBall(BallAPI.createBall(4, 250, 250, 10, "yellow", 1, 1, 2));
            dataLayer.AddBall(BallAPI.createBall(5, 300, 300, 10, "purple", -1, 0, 2));

            dataLayer.RemoveBalls();

            List<BallAPI> balls = dataLayer.GetBalls();
            Assert.AreEqual(0, balls.Count);
        }

    }
}
