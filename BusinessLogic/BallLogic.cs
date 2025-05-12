using System;
using System.Collections.Generic;
using System.ComponentModel;
using Data;

namespace BusinessLogic
{
    public abstract class BallLogicAPI
    {
        public static BallLogicAPI CreateLogic(double mapWidth, double mapHeight)
        {
            return new BallLogic(mapWidth, mapHeight);
        }

        public abstract void CreateBall(int ballID, double x, double y, double radius, string color, int XDirection, int YDirection);
        public abstract double GetMapWidth();
        public abstract double GetMapHeight();
        public abstract List<BallAPI> GetBalls();
        public abstract void MoveBall(BallAPI ball);
        public abstract void CreateNBalls(int amount);
        public abstract void CreateRandomBall();
        public abstract void ClearMap();
        public abstract void Collision();
        public abstract double Distance(BallAPI ballOne, BallAPI ballTwo);
        public abstract void ElasticRebound(BallAPI ballOne, BallAPI ballTwo);
        public abstract bool CollisionOccurence(BallAPI ballOne, BallAPI ballTwo);
        public abstract BallAPI? CollisionBalls(BallAPI ballCurrent);
        public abstract void WindowCollision(BallAPI ball);
        public abstract void StopAnimation();
        public abstract void StartAnimation();

        private class BallLogic : BallLogicAPI
        {
            private readonly double mapWidth;
            private readonly double mapHeight;

            private readonly DataLayerAPI dataLayer;
            private  bool _inAction;

            public BallLogic(double mapWidth, double mapHeight)
            {
                this.mapWidth = mapWidth;
                this.mapHeight = mapHeight;
                this.dataLayer = DataLayerAPI.CreateData(mapWidth, mapHeight, "white"); 
                _inAction = false;
            }
            ///
            /// Creating a ball - radius and weight values are random
            /// 
            public override void CreateBall(int ballID, double x, double y, double radius, string color, int XDirection, int YDirection)
{
            Random random = new Random();
            double weight = (double)random.Next(1, 10);
            radius = weight + 10;

            int maxAttempts = 100;
            bool placed = false;

            for (int i = 0; i < maxAttempts && !placed; i++)
            {
                x = random.Next((int)radius, (int)(mapWidth - radius));
                y = random.Next((int)radius, (int)(mapHeight - radius));

                BallAPI ball = BallAPI.createBall(ballID, x, y, radius, color, XDirection, YDirection, weight);
                placed = true;

                foreach (var other in GetBalls())
                {
                    if (CollisionOccurence(ball, other))
                    {
                        placed = false;
                        break;
                    }
                }

                if (placed)
                {
                    dataLayer.AddBall(ball);
                    return;
                }
            }

            if (!placed)
            {
                throw new Exception("Nie udało się umieścić kulki bez kolizji po 100 próbach.");
            }
        }
            ///
            /// Creating N amount of balls
            /// 
            public override void CreateNBalls(int amount)
            {
                for (int i = 0; i < amount; i++)
                {
                    CreateRandomBall();
                }
             
            }
            ///
            /// Creating a ball with random coordinates 
            /// 
            public override void CreateRandomBall()
            {      
                double radius = 0;
                Random random = new Random();
                int xDirection = random.Next(2) == 0 ? -1 : 1;
                int yDirection = random.Next(2) == 0 ? -1 : 1;
                double x = random.Next((int)radius, (int)mapWidth - (int)radius);
                double y = random.Next((int)radius, (int)mapHeight - (int)radius);
                string color = "#" + random.Next(0x1000000).ToString("X6");

                CreateBall(dataLayer.GetBalls().Count + 1, x, y, radius, color, xDirection, yDirection);
            }
            ///
            /// Removing balls 
            /// 
            public override void ClearMap()
            {
                dataLayer.RemoveBalls();
            }
            ///
            ///Checks whether the collision occured, if so, performs an elastic rebound
            /// 
            public override void Collision()
            {
                foreach (var ball in GetBalls())
                {
                    var other = CollisionBalls(ball);
                    if (other != null)
                    {
                        ElasticRebound(ball, other);
                    }
                }
            }
            ///
            ///Calculates the distance between two balls
            /// 
            public override double Distance(BallAPI ballOne, BallAPI ballTwo)
            {
                return Math.Sqrt(Math.Pow(ballOne.X - ballTwo.X, 2) + Math.Pow(ballOne.Y - ballTwo.Y, 2));
            }
            ///
            /// Checks whetehr the collision occured
            /// 
            public override bool CollisionOccurence(BallAPI ballOne, BallAPI ballTwo)
            {
                double distanceBalls = Distance(ballOne, ballTwo);
                double radiusSum = ballOne.Radius + ballTwo.Radius;
                return distanceBalls <= radiusSum;
            }
            ///
            /// Checks each ball to determine whether a collision occurred
            /// 
            public override BallAPI? CollisionBalls(BallAPI ballCurrent)
            {
                foreach (var ball in GetBalls())
                {
                    if (!ball.Equals(ballCurrent) && CollisionOccurence(ballCurrent, ball))
                        return ball;
                }
                return null;
            }

             ///
            /// Checks whther the window collison occured
            /// 
           public override void WindowCollision(BallAPI ball)
            {
                if (ball.X - ball.Radius <= 0 || ball.X + ball.Radius >= mapWidth)
                {
                    ball.XDirection = -ball.XDirection;
                    ball.X = Math.Clamp(ball.X, ball.Radius, mapWidth - ball.Radius);
                }

                if (ball.Y - ball.Radius <= 0 || ball.Y + ball.Radius >= mapHeight)
                {
                    ball.YDirection = -ball.YDirection;
                    ball.Y = Math.Clamp(ball.Y, ball.Radius, mapHeight - ball.Radius);
                }
            }

           public void isMoving(object sender, PropertyChangedEventArgs e)
            {
                BallAPI ball = (BallAPI)sender;
                if (e.PropertyName == nameof(BallAPI.XPosition) || e.PropertyName == nameof(BallAPI.YPosition))
                {
    
                    MoveBall(ball);
                    ball.IsMoving = false;
                }
            }
            ///
            ///Changes the action flag from true to false
            /// 
            public override void StopAnimation()
            {
                _inAction = false;
            }

            public override void MoveBall(BallAPI ball)
            {
             
                if (!ball.IsMoving)
                    return;

                WindowCollision(ball);
                var secondBall = CollisionBalls(ball);
                if (secondBall != null)
                {
                    ElasticRebound(ball, secondBall); 
                }


                ball.X += ball.XDirection;
                ball.Y += ball.YDirection;
            }

            ///
            /// Calculates the elastic rebound
            /// 
            public override void ElasticRebound(BallAPI ballOne, BallAPI ballTwo)
            {
                int tempX = ballOne.XDirection;
                int tempY = ballOne.YDirection;

                ballOne.XDirection = ballTwo.XDirection;
                ballOne.YDirection = ballTwo.YDirection;

                ballTwo.XDirection = tempX;
                ballTwo.YDirection = tempY;
            }

           public override void StartAnimation()
            {
                _inAction = true;
                Task.Run(() =>
                {
                    while (_inAction)
                    {
                        foreach (var ball in GetBalls())
                        {
                            ball.IsMoving = true;
                            MoveBall(ball);
                        }
                        /// The break between animation frames - pauses the execution of the current thread - 60 FPS
                        Thread.Sleep(16);
                    }
                });
            }

            public override List<BallAPI> GetBalls()
            {
                return dataLayer.GetBalls();
            }

            public override double GetMapWidth() => mapWidth;

            public override double GetMapHeight() => mapHeight;

        }
    }
}
