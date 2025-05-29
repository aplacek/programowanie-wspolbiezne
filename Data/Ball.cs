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

    private DateTime lastUpdateTime = DateTime.Now;

    public double weight { get; set; }
    public abstract double XSpeed { get; set; }
    public abstract double YSpeed { get; set; }

    public abstract double XPosition { get; set; }
    public abstract double YPosition { get; set; }

    public abstract bool IsMoving { get; set; }

    public abstract void MoveBall(double mapWidth, double mapHeight);

    public abstract event PropertyChangedEventHandler PropertyChanged;

    private class Ball : BallAPI, INotifyPropertyChanged
    {
        private double _xSpeed;
        private double _ySpeed ;
        private readonly object _syncObject = new object();
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
            this._xSpeed = 1;
            this._ySpeed = 1; 
         
        }
            public override double XPosition
        {
            get => X;
            set
            {
                if (X != value)
                {
                    X = value;
                    OnPropertyChanged(nameof(XPosition));
                }
            }
        }

        public override double YPosition
        {
            get => Y;
            set
            {
                if (Y != value)
                {
                    Y = value;
                    OnPropertyChanged(nameof(YPosition));
                }
            }
        }



        public override double XSpeed
        {
            get => _xSpeed;
            set
            {
                _xSpeed = value;
                OnPropertyChanged(nameof(XSpeed));
            }
        }

        public override double YSpeed
        {
            get => _ySpeed;
            set
            {
                _ySpeed = value;
                OnPropertyChanged(nameof(YSpeed));
            }
        }

        ///
        /// Critical section
        /// without locking multiple threads could simultaneously change the ball's position
         public override void MoveBall(double mapWidth, double mapHeight)
            {
                lock (_syncObject)
                {
                    DateTime now = DateTime.Now;
                    double timeDifference = (now - lastUpdateTime).TotalSeconds; 
                    lastUpdateTime = now;

                    double newX = XPosition + XSpeed * XDirection * timeDifference;
                    double newY = YPosition + YSpeed * YDirection * timeDifference;

                    if (newX < 0 || newX > mapWidth){
                        XDirection = -XDirection;
                        newX = XPosition + XSpeed * XDirection * timeDifference;
                    }

                    if (newY < 0 || newY > mapHeight){
                        YDirection = -YDirection;
                        newY = YPosition + YSpeed * YDirection * timeDifference;
                    }

                    XPosition = newX;
                    YPosition = newY;
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