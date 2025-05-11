using Microsoft.VisualStudio.TestTools.UnitTesting;
using Presentation.Model;
using System.Linq;

namespace PresentationTest
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void TestCreateBalls()
        {
            MainAPI model = MainAPI.CreateMap(500, 500);

            model.CreateBalls(5);
            var balls = model.GetBalls();

            Assert.AreEqual(5, balls.Count);
        }

        [TestMethod]
        public void TestClearMap()
        {
            MainAPI model = MainAPI.CreateMap(500, 500);

            model.CreateBalls(5);
            model.ClearMap();

            Assert.AreEqual(0, model.GetBalls().Count);
        }
   
    }
}