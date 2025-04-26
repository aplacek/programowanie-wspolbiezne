using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Data;

namespace Data
{
    public abstract class DataLayerAPI
    {
        public static DataLayerAPI CreateData(int height, int width, string colour)
        {
            return new DataLayer(height, width, colour);
        }

        public abstract void AddBall(BallAPI ball);
        public abstract void StartMoving();
        public abstract void StopMoving();
        public abstract List<BallAPI> GetBalls();

        public abstract void RemoveBalls();

        private class DataLayer : DataLayerAPI
        {
            private List<Thread> _threads;
            private bool _moving = false;
            private object _lock = new object();
            private BoardAPI board;

            public DataLayer(int height, int width, string colour)
            {
                this.board = BoardAPI.CreateBoard(height, width, colour); 
                this._threads = new();
        
            }

            public override void AddBall(BallAPI ball)
            {
          
           
                board.AddBall(ball);

                Thread t = new Thread(() =>
                {
                    while (_moving)
                    {
                        lock (_lock)
                        {
                            ball.MoveBall();
                        }
                        Thread.Sleep(10); //in order not to overheeat cpu :P
                    }
                });

                _threads.Add(t); 
            }

        public override void StartMoving()
        {
            if (_moving) return;
            _moving = true;

            Task.Run(async () =>
            {
                while (_moving)
                {
                    foreach (var ball in board.GetBalls()) 
                    {
                        ball.X += ball.XDirection;
                        ball.Y += ball.YDirection;
                    }
                    await Task.Delay(20); 
                }
            });
        }

            public override void StopMoving()
            {
                _moving = false;
            }

            public override List<BallAPI> GetBalls()
            {
                return board.GetBalls();
            }

            public override void RemoveBalls(){
                board.RemoveBalls();
            }

        }
    }
}
