using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTest{
    [TestClass]
    public class BallRepositoryTest{
        [TestMethod]
        public void OperationsOnRepository(){
            var ball = BallAPI.createBall(1,0, 1, 2, "black", -1, 2);
            var ballTwo = BallAPI.createBall(1,0, 1, 2, "white", -1, 2);
            var repository = BallRepositoryAPI.CreateRepository();

            Assert.AreEqual(0,  repository.GetSize());
            repository.AddBall(ball);
            Assert.AreEqual(1,  repository.GetSize());
            repository.AddBall(ballTwo);
            Assert.AreEqual(2,  repository.GetSize());
            //Assert.AreEqual<BallRepository>(repository, repository.GetAllBalls());
            repository.RemoveBall(ballTwo);
            Assert.AreEqual(1,  repository.GetSize());
            repository.ClearStorage();
            Assert.AreEqual(0,  repository.GetSize());
            
        }

    }
}

