using BusinessLogic;
using Data;
using System.Collections.Generic;

namespace PresentationModel;

public abstract class MainAPI
{
    public static MainAPI createMain(int width, int height)
    {
        return new Main(width, height);
    }

    public abstract void Move();
    public abstract List<Ball> GetBalls();
    public abstract void CreateBall(int amount);
    public abstract void ClearMap();

    private class Main : MainAPI
    {
        private readonly BallLogicAPI ballLogic;
        private int width;
        private int height;

        public Main(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.ballLogic = BallLogicAPI.CreateLogic(width, height); 
        }

        public override void CreateBall(int amount)
        {
            ballLogic.createNBalls(amount); 
        }

        public override void ClearMap()
        {
            ballLogic.ClearMap(); 
        }

        public override List<Ball> GetBalls()
        {
            return ballLogic.GetBalls();
        }

        public override void Move()
        {
            ballLogic.UpdateBalls(); 
        }
    }
}