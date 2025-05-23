using BusinessLogic;
using Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Presentation.Model
{
    public abstract class MainAPI
    {
        

        public static MainAPI CreateMap(double width, double height)
        {
            return new Main(width, height);
        }
    
        public abstract void Move();
        public abstract List<BallAPI> GetBalls();
        public abstract void CreateBalls(int amount);
        public abstract void ClearMap();
        public abstract void StopMovement();
        private ObservableCollection<CircleAPI> _circle = new ObservableCollection<CircleAPI>();
        public ObservableCollection<CircleAPI> Balls
            {
                get => _circle;
                set => _circle = value;
            }

        private class Main : MainAPI
        {
            private double width;
            private double height;


            private readonly BallLogicAPI ballLogic;

            

            public Main(double width, double height)
            {
                this.width = width;
                this.height = height;
                this.ballLogic = BallLogicAPI.CreateLogic(width, height); 
            }
            public override void Move()
            {  
                this.ballLogic.StartAnimation();
            }

            public override void StopMovement(){

                this.ballLogic.StopAnimation();
            }

            public override List<BallAPI> GetBalls()
            {
                return this.ballLogic.GetBalls(); 
            }

            public override void CreateBalls(int amount)
            {
              
                this.ballLogic.CreateNBalls(amount);
                _circle.Clear();

                  foreach (BallAPI ball in ballLogic.GetBalls())
                {
                    _circle.Add(CircleAPI.CreateCircle(ball));
                }

            }

            public override void ClearMap()
            {
                this.ballLogic.ClearMap(); 
                _circle.Clear();

            }
        }
    }
}
