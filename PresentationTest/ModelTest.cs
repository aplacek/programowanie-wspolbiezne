using Microsoft.VisualStudio.TestTools.UnitTesting;
using Presentation.Model;
using System.Linq;

namespace PresentationTest
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void CreateBalls_ShouldAddCorrectAmount()
        {
            MainAPI model = MainAPI.CreateMap(500, 500);

            model.CreateBalls(5);
            var balls = model.GetBalls();

            Assert.AreEqual(5, balls.Count);
        }

        [TestMethod]
        public void ClearMap_ShouldRemoveAllBalls()
        {
            MainAPI model = MainAPI.CreateMap(500, 500);

            model.CreateBalls(5);
            model.ClearMap();

            Assert.AreEqual(0, model.GetBalls().Count);
        }

        [TestMethod]
        public void Move_ShouldUpdateBallPosition()
        {
            MainAPI model = MainAPI.CreateMap(500, 500);

            model.CreateBalls(1);
            var ball = model.GetBalls().First();
            double initialX = ball.X;
            double initialY = ball.Y;

            model.Move();
            var updatedBall = model.GetBalls().First();

            bool hasMoved = initialX != updatedBall.X || initialY != updatedBall.Y;
            Assert.IsTrue(hasMoved, "Ball did not move after calling Move()");
        }
    }
}