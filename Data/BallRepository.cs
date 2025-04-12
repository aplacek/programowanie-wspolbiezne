namespace Data;

public abstract class BallRepositoryAPI
{
    public static BallRepositoryAPI CreateRepository()
    {
        return new BallRepository();
    }

    public abstract void AddBall(BallAPI obj);
    public abstract void ClearStorage();
    public abstract List<BallAPI> GetAllBalls();
    public abstract void RemoveBall(BallAPI obj);
    public abstract int GetSize();
    public abstract BallAPI GetBallByID(int id);

    private class BallRepository : BallRepositoryAPI
    {
        private readonly List<BallAPI> balls = new();

        public override void AddBall(BallAPI obj)
        {
            balls.Add(obj);
        }

        public override void ClearStorage()
        {
            balls.Clear();
        }

        public override List<BallAPI> GetAllBalls()
        {
            return balls;
        }

        public override void RemoveBall(BallAPI obj)
        {
            balls.Remove(obj);
        }

        public override int GetSize()
        {
            return balls.Count;
        }

        public override BallAPI GetBallByID(int id)
        {
            foreach (BallAPI ball in balls)
            {
                if (ball.ID == id)
                {
                    return ball;
                }
            }
            return null;
        }
    }
}
