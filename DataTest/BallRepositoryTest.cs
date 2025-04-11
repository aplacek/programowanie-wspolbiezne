using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTest{
    [TestClass]
    public class BallRepositoryTest{
        [TestMethod]
        public void OperationsOnRepository(){
            var ball = Ball.createBall(1,0, 1, 2, "black");
            var ballTwo = Ball.createBall(1,0, 1, 2, "white");
            var repository = BallRepository.createRepository();

            Assert.AreEqual(0,  repository.getSize());
            repository.AddBall(ball);
            Assert.AreEqual(1,  repository.getSize());
            repository.AddBall(ballTwo);
            Assert.AreEqual(2,  repository.getSize());
            //Assert.AreEqual<BallRepository>(repository, repository.GetAllBalls());
            repository.RemoveBall(ballTwo);
            Assert.AreEqual(1,  repository.getSize());
            repository.ClearStorage();
            Assert.AreEqual(0,  repository.getSize());
        }

    }
}

