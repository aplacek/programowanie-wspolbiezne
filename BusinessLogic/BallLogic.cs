using System;
using System.Collections.Generic;
using System.ComponentModel;
using Data;

namespace BusinessLogic
{
    public abstract class BallLogicAPI
    {
        public static BallLogicAPI CreateLogic(int mapWidth, int mapHeight)
        {
            return new BallLogic(mapWidth, mapHeight);
        }

        public abstract void CreateBall(int ballID, double x, double y, double radius, string color, int XDirection, int YDirection);
        public abstract int GetMapWidth();
        public abstract int GetMapHeight();
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
            private readonly int mapWidth;
            private readonly int mapHeight;
            private readonly DataLayerAPI dataLayer;
            private  bool _inAction;
            public BallLogic(int mapWidth, int mapHeight)
            {
                this.mapWidth = mapWidth;
                this.mapHeight = mapHeight;
                this.dataLayer = DataLayerAPI.CreateData(mapWidth, mapHeight, "white"); // Default color
                _inAction = false;
            }

            public override void CreateBall(int ballID, double x, double y, double radius, string color, int XDirection, int YDirection)
            {
                double weight = 50;
                BallAPI ball = BallAPI.createBall(ballID, x, y, radius, color, XDirection, YDirection, weight);
                while (ball.X + radius > mapWidth || ball.X - radius < 0 || ball.Y + radius > mapHeight || ball.Y - radius < 0)
                {
                    ball = BallAPI.createBall(ballID, x, y, radius, color, XDirection, YDirection, weight);
                }
                dataLayer.AddBall(ball);
            }

    
            public override void CreateNBalls(int amount)
            {
                for (int i = 0; i < amount; i++)
                {
                    CreateRandomBall();
                }
            }

            public override void CreateRandomBall()
            {      
                double radius = 12;
                Random random = new Random();
                int xDirection = random.Next(2) == 0 ? -1 : 1;
                int yDirection = random.Next(2) == 0 ? -1 : 1;
                double x = random.Next((int)radius, mapWidth - (int)radius);
                double y = random.Next((int)radius, mapHeight - (int)radius);
                string color = "#" + random.Next(0x1000000).ToString("X6");

                CreateBall(dataLayer.GetBalls().Count + 1, x, y, radius, color, xDirection, yDirection);
            }

            public override void ClearMap()
            {
                dataLayer.RemoveBalls();
            }

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

            public override double Distance(BallAPI ballOne, BallAPI ballTwo)
            {
                return Math.Sqrt(Math.Pow(ballOne.X - ballTwo.X, 2) + Math.Pow(ballOne.Y - ballTwo.Y, 2));
            }

            public override bool CollisionOccurence(BallAPI ballOne, BallAPI ballTwo)
            {
                double distanceBalls = Distance(ballOne, ballTwo);
                double radiusSum = ballOne.Radius + ballTwo.Radius;
                return distanceBalls <= radiusSum;
            }

            public override BallAPI? CollisionBalls(BallAPI ballCurrent)
            {
                foreach (var ball in GetBalls())
                {
                    if (!ball.Equals(ballCurrent) && CollisionOccurence(ballCurrent, ball))
                        return ball;
                }
                return null;
            }

            public override void WindowCollision(BallAPI ball)
            {
                if (ball.X + ball.XDirection - ball.Radius < 0 || ball.X + ball.XDirection + ball.Radius > mapWidth)
                {
                    ball.XDirection = -ball.XDirection;
                }

                if (ball.Y + ball.YDirection - ball.Radius < 0 || ball.Y + ball.YDirection + ball.Radius > mapHeight)
                {
                    ball.YDirection = -ball.YDirection;
                }
            }

            public override void MoveBall(BallAPI ball)
            {
                if (!ball.IsMoving)
                    return; // 👉 Jeśli kulka nie "rusza się", to jej w ogóle nie przesuwamy

                WindowCollision(ball);

                var secondBall = CollisionBalls(ball);
                if (secondBall != null)
                {
                    ElasticRebound(ball, secondBall);
                }

                ball.X += ball.XDirection;
                ball.Y += ball.YDirection;
            }

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
                if (_inAction) return;
                _inAction = true;

                foreach (var ball in GetBalls())
                {
                    ball.IsMoving = true; // ✨ Kulki zaczynają się ruszać
                }

                Task.Run(async () =>
                {
                    while (_inAction)
                    {
                        foreach (var ball in GetBalls())
                        {
                            MoveBall(ball);
                        }
                        await Task.Delay(20);
                    }
                });
            }
            public override void StopAnimation()
            {
                _inAction = false;

                foreach (var ball in GetBalls())
                {
                    ball.IsMoving = false; // ❄️ Kulki się zatrzymują
                }
            }

            public override List<BallAPI> GetBalls()
            {
                return dataLayer.GetBalls();
            }

            public override int GetMapWidth() => mapWidth;

            public override int GetMapHeight() => mapHeight;

        }
    }
}
