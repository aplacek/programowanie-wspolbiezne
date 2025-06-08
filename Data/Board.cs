namespace Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;


    public abstract class BoardAPI 
    {
        public static BoardAPI CreateBoard(double height, double width, string color)
        {
            return new Board(height, width, color);
        }

        public abstract double Width { get; }
        public abstract double Height { get; }
        public abstract string Color { get; }
        public abstract event PropertyChangedEventHandler PropertyChanged;

        public abstract void AddBall(BallAPI ball);
        public abstract void RemoveBalls();
        public abstract List<BallAPI> GetBalls();
    }

    public class Board : BoardAPI, INotifyPropertyChanged
    {
        private double height;
        private double width;
        private string color;
        private List<BallAPI> balls;

        public Board(double height, double width, string color)
        {
            this.height = height;
            this.width = width;
            this.color = color;
            this.balls = new List<BallAPI>();
        }

        public override void AddBall(BallAPI ball)
        {
            balls.Add(ball);
        }

        public override double Height => height;
        public override double  Width => width;
        public override string Color => color;

        public override List<BallAPI> GetBalls()
        {
            return balls;
        }

        public override void RemoveBalls(){
            balls.Clear();
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

