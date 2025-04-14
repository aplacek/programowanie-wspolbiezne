using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogicTest

{
    [TestClass]
    public class BallLogicTest
    {
        [TestMethod] //test konstruktora kulek
        public void BallLogicConstructorTest()
        {
                var ballLogic = BallLogicAPI.CreateLogic(100, 200);
                Assert.AreEqual(100, ballLogic.GetMapWidth());
                Assert.AreEqual(200, ballLogic.GetMapHeight());
                ballLogic.createNBalls(5);
                Assert.AreEqual(ballLogic.GetSize() , 5);
                Assert.AreEqual(ballLogic.GetBallByID(1).ID, 1);
                ballLogic.RemoveBall(ballLogic.GetBallByID(1));
                Assert.AreEqual(ballLogic.GetSize(), 4);
                ballLogic.ClearMap();
                Assert.AreEqual(ballLogic.GetSize(), 0);

        }

        [TestMethod] //test ruchu kulek
        public void MoveBalltest(){
               var ballLogic = BallLogicAPI.CreateLogic(100, 200);
                Assert.AreEqual(100, ballLogic.GetMapWidth());
                Assert.AreEqual(200, ballLogic.GetMapHeight());
                ballLogic.createNBalls(5);
                double x_pos = ballLogic.GetBallByID(1).X;
                double y_pos = ballLogic.GetBallByID(1).Y;

                ballLogic.UpdateBalls();

                double x_pos_two = ballLogic.GetBallByID(1).X;
                double y_pos_two = ballLogic.GetBallByID(1).Y;

                Assert.AreNotEqual(x_pos, x_pos_two);
                Assert.AreNotEqual(y_pos, y_pos_two);

        }
        

        [TestMethod] //test, czy kulka wychodzi poza granice mapy
        public void BallCreationWithinBoundsTest()
        {
            var ballLogic = BallLogicAPI.CreateLogic(100, 200);
            ballLogic.createNBalls(10);

            foreach (var ball in ballLogic.GetBalls())
            {
                Assert.IsTrue(ball.X - ball.Radius >= 0, "Kulka wychodzi poza lewą krawędź.");
                Assert.IsTrue(ball.X + ball.Radius <= ballLogic.GetMapWidth(), "Kulka wychodzi poza prawą krawędź.");
                Assert.IsTrue(ball.Y - ball.Radius >= 0, "Kulka wychodzi poza górną krawędź.");
                Assert.IsTrue(ball.Y + ball.Radius <= ballLogic.GetMapHeight(), "Kulka wychodzi poza dolną krawędź.");
            }
        }


        [TestMethod] //test ClearMap
        public void ClearMapTest()
        {
            var ballLogic = BallLogicAPI.CreateLogic(100, 200);
            ballLogic.createNBalls(3);
            Assert.AreEqual(3, ballLogic.GetSize(), "Powinno być 3 kulki.");
            
            ballLogic.ClearMap();
            Assert.AreEqual(0, ballLogic.GetSize(), "Magazyn kulek powinien być pusty po wyczyszczeniu.");
        }

    }
}