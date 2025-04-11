namespace Data;
public class Ball

{
    public int ID { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double VelocityX { get; set; }
    public double VelocityY { get; set; }
    public double Radius { get; set; }
    public string color { get; set; }

    public int XDirection { get; set; }
    public int YDirection { get; set; }

    public Ball(int ID, double X, double Y, double radius, string color, int XDirection, int YDirection)
    {
        this.ID = ID;
        this.X = X;
        this.Y = Y;
        this.color = color;
        this.Radius = radius;
        this.XDirection = XDirection;
        this.YDirection = YDirection;
        var random = new Random();
        VelocityX = random.NextDouble() * 2 - 1; // Prędkość w zakresie -1 do 1
        VelocityY = random.NextDouble() * 2 - 1;
    }


    // Metoda przesuwająca kulę w czasie
    public void UpdatePosition(double timeInterval)
    {
        X += VelocityX * timeInterval;
        Y += VelocityY * timeInterval;
    }

    // Metoda, która sprawdza czy kula nie wychodzi poza granice
    public void CheckBoundary(double width, double height)
    {
        if (X - Radius < 0 || X + Radius > width)
        {
            VelocityX = -VelocityX; // Odbicie od ściany
        }
        if (Y - Radius < 0 || Y + Radius > height)
        {
            VelocityY = -VelocityY; // Odbicie od ściany
        }
    }

    public static Ball createBall(int ID, double X, double Y, double radius, string color, int XDirection, int YDirection){
        return new Ball(ID, X, Y, radius, color, XDirection, YDirection);
    }
}
