using BusinessLogic;
using Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;  
using System.Runtime.CompilerServices; 

namespace Presentation.Model
{
    public abstract class CircleAPI : INotifyPropertyChanged
    {
          public static CircleAPI CreateCircle(BallAPI ball)
        {
            return new Circle(ball);
        }
        public abstract double X { get; set; }
        public abstract double Y { get; set; }
        public abstract string Color { get; set; }
        public abstract double Radius { get; set; }

        public abstract event PropertyChangedEventHandler PropertyChanged;

        public class Circle : CircleAPI
        {
            private double _x;
            private double _y;
            private double _radius;
            private string _color;
   

            private PropertyChangedEventHandler _propertyChanged;


            public override event PropertyChangedEventHandler PropertyChanged
            {
                add
                {
                    _propertyChanged += value;
                }
                remove
                {
                    _propertyChanged -= value;
                }
            }

            public Circle(BallAPI ball)
            {
                ball.PropertyChanged += BallPropertyChanged;
                X = ball.X;
                Y = ball.Y;
                Radius = ball.Radius;
                Color = ball.Color;
            }


            public override double Radius
            {
                get => _radius;
                set
                {
                    _radius = value;
                    RaisePropertyChanged("Radius");
                }
            }

               public override string Color
            {
                get => _color;
                set
                {
                    _color = value;
                    RaisePropertyChanged("Color");
                }
            }


            public override double X
            {
                get => _x;
                set
                {
                    _x = value;
                    RaisePropertyChanged("X");
                }
            }

            public override double Y
            {
                get => _y;
                set
                {
                    _y = value;
                    RaisePropertyChanged("Y");
                }
            }

            public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            {
                _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private void BallPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                BallAPI ball = (BallAPI)sender;
                X = ball.X;
                Y = ball.Y;
                Color = ball.Color;
            }
        }


       
    }
}