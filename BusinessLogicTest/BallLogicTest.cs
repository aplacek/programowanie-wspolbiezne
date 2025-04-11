using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogicTest
{
    [TestClass]
    public class BallLogicTest
    {
        [TestMethod]
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

        [TestMethod]

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
    }
}