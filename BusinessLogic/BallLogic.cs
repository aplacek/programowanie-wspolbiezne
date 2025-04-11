using System;
using System.Timers;
using Data;
using System.Collections.Generic;

namespace BusinessLogic
{
    public abstract class BallLogicAPI
    {
        public static BallLogicAPI CreateLogic(int mapWidth, int mapHeight, BallRepository? repo = null)
        {
            return new BallLogic(mapWidth, mapHeight, repo ?? new BallRepository());
        }

        public abstract void CreateBall(int ballID, double x, double y, double radius, string color, int XDirection, int YDirection);
        public abstract void RemoveBall(Ball ball);
        public abstract int GetMapWidth();
        public abstract int GetMapHeight();
        public abstract Ball GetBallByID(int id);
        public abstract List<Ball> GetBalls();  
        public abstract void MoveBall(Ball ball);
        public abstract void UpdateBalls();

        public abstract int GetSize();

        public abstract void createNBalls(int amount);

        public abstract void createRandomBall();

        public abstract void ClearMap();

        private class BallLogic : BallLogicAPI
        {
            private readonly int mapWidth;
            private readonly int mapHeight;
            private readonly BallRepository repository;

            public BallLogic(int mapWidth, int mapHeight, BallRepository repository)
            {
                this.repository = repository;
                this.mapHeight = mapHeight;
                this.mapWidth = mapWidth;
            }

            public override void CreateBall(int ballID, double x, double y, double radius, string color, int XDirection, int YDirection)
            {
                var ball = Ball.createBall(repository.getSize() + 1, x, y, radius, color, XDirection, YDirection);  // Zakładając, że `createBall` jest statyczną metodą klasy `Ball`
                repository.AddBall(ball);
            }

            public override void RemoveBall(Ball ball)
            {
                repository.RemoveBall(ball);
            }

            public override Ball GetBallByID(int id)
            {
                return repository.getBallByID(id);
            }

            public override List<Ball> GetBalls()  // Poprawiona metoda
            {
                return repository.GetAllBalls();
            }

            public override int GetMapWidth()
            {
                return mapWidth;
            }

            public override int GetMapHeight()
            {
                return mapHeight;
            }

            public override void MoveBall(Ball ball)
            {
                // Sprawdzenie odbicia od ścian poziomych (X)
                if (ball.X + ball.XDirection - ball.Radius < 0 || ball.X + ball.XDirection + ball.Radius > mapWidth)
                {
                    ball.XDirection = -ball.XDirection;  // Odbicie w poziomie
                }

                // Sprawdzenie odbicia od ścian pionowych (Y)
                if (ball.Y + ball.YDirection - ball.Radius < 0 || ball.Y + ball.YDirection + ball.Radius > mapHeight)
                {
                    ball.YDirection = -ball.YDirection;  // Odbicie w pionie
                }

                // Zaktualizowanie pozycji kuli
                ball.X += ball.XDirection;
                ball.Y += ball.YDirection;

                // Ręczna kontrola, aby kula nie wyszła poza mapę
                if (ball.X - ball.Radius < 0)
                {
                    ball.X = ball.Radius;  // Ustaw krawędź kuli na lewą granicę mapy
                }
                else if (ball.X + ball.Radius > mapWidth)
                {
                    ball.X = mapWidth - ball.Radius;  // Ustaw krawędź kuli na prawą granicę mapy
                }

                if (ball.Y - ball.Radius < 0)
                {
                    ball.Y = ball.Radius;  // Ustaw krawędź kuli na górną granicę mapy
                }
                else if (ball.Y + ball.Radius > mapHeight)
                {
                    ball.Y = mapHeight - ball.Radius;  // Ustaw krawędź kuli na dolną granicę mapy
                }
            }

            public override void UpdateBalls()
            {
                foreach (var ball in repository.GetAllBalls())
                {
                    MoveBall(ball);
                }
            }

            public override void createNBalls(int amount){

                for(int i = 0; i< amount; i++){
                    createRandomBall();
                }

            }

           public override void createRandomBall()
                {
                Random random = new Random();

                int xRandom = 0;
                int yRandom = 0;

                while (xRandom == 0 && yRandom == 0)
                {
                    xRandom = random.Next(-5, 6); 
                    yRandom = random.Next(-5, 6);
                }

                double radius = random.Next(10, 20); 
                double x = random.Next((int)radius, mapWidth - (int)radius);
                double y = random.Next((int)radius, mapHeight - (int)radius);

                string color = "#" + random.Next(0x1000000).ToString("X6"); 

                this.CreateBall(repository.getSize() + 1, x, y, radius, color, xRandom, yRandom);
                }

                public override int GetSize(){
                    return this.repository.getSize();
                }

            public override void ClearMap()
            {
               this.repository.ClearStorage();
            }
                }
            }
    }


