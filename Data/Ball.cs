namespace Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public abstract class BallAPI{


    public static BallAPI createBall(int ID, double X, double Y, double radius, string color, int XDirection, int YDirection, double weight){
        return new Ball(ID,  X,  Y, radius,  color,XDirection, YDirection, weight);
    }

    public int ID { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Radius { get; set; }
    public string color { get; set; }
    public int XDirection { get; set; }
    public int YDirection { get; set; }
    private bool _moving = true;

    public double weight { get; set; }
    public double Xspeed { get; set; }
    public double Yspeed { get; set; }

    public abstract double XPosition { get; set; }
    public abstract double YPosition { get; set; }
    public abstract double SpeedX { get; set; }
    public abstract double SpeedY { get; set; }

    public abstract bool IsMoving { get; set; }

    public abstract void MoveBall();

    public abstract event PropertyChangedEventHandler PropertyChanged;

    private class Ball : BallAPI, INotifyPropertyChanged
    {
        public Ball(int ID, double X, double Y, double radius, string color, int XDirection, int YDirection, double weight)
        {
            this.ID = ID;
            this.X = X;
            this.Y = Y;
            this.color = color;
            this.Radius = radius;
            this.XDirection = XDirection;
            this.weight = weight;
            this.YDirection = YDirection;
            
        }
                public override double XPosition
        {
            get => X;
            set
            {
                X = value;
                OnPropertyChanged(nameof(XPosition));
            }
        }

        public override double YPosition
        {
            get => Y;
            set
            {
                Y = value;
                OnPropertyChanged(nameof(YPosition));
            }
        }

              public override double SpeedX
        {
            get =>  Xspeed;
            set
            {
                Xspeed = value;
                OnPropertyChanged(nameof(SpeedX));
            }
        }
              public override double SpeedY
        {
            get =>  Yspeed;
            set
            {
                Yspeed = value;
                OnPropertyChanged(nameof(SpeedY));
            }
        }

        public override void MoveBall()
        {
       
            lock (this)
                {
                    X += Xspeed;
                    Y += Yspeed;
                }

                
        }

        public override bool IsMoving
        {
            get => _moving;
            set {
                 _moving = value; 
                  OnPropertyChanged(nameof(IsMoving));

            }
        }
        public override event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

    }