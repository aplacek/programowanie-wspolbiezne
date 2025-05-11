using Microsoft.VisualStudio.TestTools.UnitTesting;
using Presentation.ModelView;
using System.Linq;

namespace PresentationTests
{
    [TestClass]
    public class ModelViewTests
    {
        private ModelView viewModel;

        [TestInitialize]
        public void Setup()
        {
            viewModel = new ModelView();
        }

        [TestMethod]
        public void InitialState_ShouldBeCorrect()
        {
            Assert.AreEqual(800, viewModel.CanvasWidth);
            Assert.AreEqual(500, viewModel.CanvasHeight);
            Assert.AreEqual("", viewModel.Amount);
            Assert.IsFalse(viewModel.ClearCommand.CanExecute(null));
            Assert.IsTrue(viewModel.SummonCommand.CanExecute(null));
        }

        [TestMethod]
        public void SummonBalls_ShouldAddCorrectNumberOfBalls()
        {
            viewModel.Amount = "3";
            viewModel.SummonCommand.Execute(null);

            Assert.AreEqual(3, viewModel.Balls.Count);
            Assert.IsTrue(viewModel.ClearCommand.CanExecute(null));
        }

        [TestMethod]
        public void SummonBalls_InvalidInput_ShouldClearAmount()
        {
            viewModel.Amount = "abc";
            viewModel.SummonCommand.Execute(null);

            Assert.AreEqual("", viewModel.Amount);
        }

        [TestMethod]
        public void StartBalls_ShouldEnableStopAndPauseMovement()
        {
            viewModel.Amount = "2";
            viewModel.SummonCommand.Execute(null);
            viewModel.StartCommand.Execute(null);

            Assert.IsTrue(viewModel.StopCommand.CanExecute(null));
            Assert.IsFalse(viewModel.StartCommand.CanExecute(null));
        }

        [TestMethod]
        public void StopBalls_ShouldDisableStopEnableStart()
        {
            viewModel.Amount = "2";
            viewModel.SummonCommand.Execute(null);
            viewModel.StartCommand.Execute(null);
            viewModel.StopCommand.Execute(null);

            Assert.IsTrue(viewModel.StartCommand.CanExecute(null));
            Assert.IsFalse(viewModel.StopCommand.CanExecute(null));
        }

        [TestMethod]
        public void ClearBalls_ShouldResetAll()
        {
            viewModel.Amount = "4";
            viewModel.SummonCommand.Execute(null);

            viewModel.ClearCommand.Execute(null);

            Assert.AreEqual(0, viewModel.Balls.Count);
            Assert.AreEqual("", viewModel.Amount);
            Assert.IsFalse(viewModel.ClearCommand.CanExecute(null));
        }

        [TestMethod]
        public void SettingCanvasProperties_ShouldRaiseChange()
        {
            bool changed = false;
            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "CanvasWidth") changed = true;
            };

            viewModel.CanvasWidth = 600;

            Assert.IsTrue(changed);
            Assert.AreEqual(600, viewModel.CanvasWidth);
        }
    }
}
