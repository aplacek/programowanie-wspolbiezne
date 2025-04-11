namespace Data;

    public class BallRepository
    {
        private List<Ball> balls = new();

        public void AddBall(Ball obj)
        {
            balls.Add(obj);
        }

        public void ClearStorage()
        {
            balls.Clear();
        }

        public List<Ball> GetAllBalls()
        {
            return balls;
        }

        public void RemoveBall(Ball obj)
        {
            balls.Remove(obj);
        }

        public int getSize()
        {
            return balls.Count();
        }

        public static BallRepository createRepository()
        {
            return new BallRepository();
        }

        public Ball getBallByID(int id) {
            foreach (Ball ball in balls) {  
                if (ball.ID == id) {
                    return ball; 
                }
        }
         return null;  
        }

    }
