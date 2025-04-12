using BusinessLogic;
using Data;
using System.Collections.Generic;

namespace Presentation.Model
{
    public abstract class MainAPI
    {
        public static MainAPI CreateMap(int width, int height)
        {
            return new Main(width, height);
        }
    
        public abstract void Move();
        public abstract List<BallAPI> GetBalls();
        public abstract void CreateBalls(int amount, double radius);
        public abstract void ClearMap();

        private class Main : MainAPI
        {
            private int width;
            private int height;

            private readonly BallLogicAPI ballLogic;

            public Main(int width, int height)
            {
                this.width = width;
                this.height = height;
                this.ballLogic = BallLogicAPI.CreateLogic(width, height); 
            }
            public override void Move()
            {  
                this.ballLogic.UpdateBalls(); 
            }
            public override List<BallAPI> GetBalls()
            {
                return this.ballLogic.GetBalls(); 
            }
            public override void CreateBalls(int amount, double radius)
            {
                this.ballLogic.createNBalls(amount, radius); 
            }

            public override void ClearMap()
            {
                this.ballLogic.ClearMap(); 
            }
        }
    }
}
