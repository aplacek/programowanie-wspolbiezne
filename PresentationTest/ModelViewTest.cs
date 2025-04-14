using Presentation.ModelView;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PresentationTest
{
    [TestClass]
    public class ModelViewTest
    {
        [TestMethod]
        public void ModelView_InitializationTest()
        {
            var modelView = new ModelView();

            Assert.IsNotNull(modelView.Balls);
            Assert.AreEqual(0, modelView.Balls.Count);
            Assert.AreEqual(700, modelView._width); 
            Assert.AreEqual(500, modelView._height); 
        }


        [TestMethod]
        public void SummonBallsTest_ValidInput()
        {
            var modelView = new ModelView();
            modelView.Amount = "5";

            modelView.SummonCommand.Execute(null);

            Assert.AreEqual(5, modelView.Balls.Count);
        }

        [TestMethod]
        public void SummonBallsTest_InvalidInput()
        {
            var modelView = new ModelView();
            modelView.Amount = "-1"; //niepoprawna wartosc

            modelView.SummonCommand.Execute(null);

            Assert.AreEqual("", modelView.Amount);
            Assert.AreEqual(0, modelView.Balls.Count); 
        }


        [TestMethod]
        public void ClearBallsTest()
        {
            var modelView = new ModelView();
            
            modelView.Amount = "5";
            modelView.SummonCommand.Execute(null);

            Assert.AreEqual(5, modelView.Balls.Count);

            modelView.ClearCommand.Execute(null);

            Assert.AreEqual(0, modelView.Balls.Count);
            Assert.AreEqual("", modelView.Amount); 
        }

    }
}