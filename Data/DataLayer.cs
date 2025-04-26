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
                this.board = BoardAPI.CreateBoard(height, width, colour); // 🛠 FIXED: Capital C
                this._threads = new();
        
            }

            public override void AddBall(BallAPI ball)
            {
                double weight = 2;
           
                board.AddBall(ball);

                Thread t = new Thread(() =>
                {
                    while (_moving)
                    {
                        lock (_lock)
                        {
                            ball.MoveBall();
                        }
                        Thread.Sleep(10); //in order not to overheeat cpu 
                    }
                });

                _threads.Add(t); 
            }

            public override void StartMoving()
            {
                _moving = true;
                foreach (Thread thread in _threads)
                {
                    if (thread.ThreadState == ThreadState.Unstarted)
                        thread.Start();
                }
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
