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
        public static DataLayerAPI CreateData(double height, double width, string colour)
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
            private object _barrier = new object();
             private int _counter = 0;
             private int _countOfBalls = 0;
             private double height;
             private double width;

            public DataLayer(double height, double width, string colour)
            {
                this.board = BoardAPI.CreateBoard(height, width, colour); 
                this.height = height;
                this.width = width;
                this._threads = new();
                
        
            }

           public override void AddBall(BallAPI ball)
            {
                lock (_lock)
                {
                    board.AddBall(ball);
                }
            }

        public override void StartMoving()
        {

            _moving = true;
            foreach (var ball in board.GetBalls())
                {
            Thread t = new Thread(() =>
            {
                while (_moving)
                {
                 
                    lock (_lock)
                    {
                        ball.MoveBall(width, height);
                    }
                    Thread.Sleep(10); 
                }
            })
            {
                IsBackground = true
            };
            _threads.Add(t);
            t.Start();
        }
        }

            public override void StopMoving()
            {
                _moving = false;
                  foreach (var thread in _threads)
                {
                    thread.Join();
                }
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
