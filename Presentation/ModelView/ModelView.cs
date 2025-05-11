using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Presentation.Model;
using Presentation.ModelView.MVVMCore;
using System.ComponentModel;  
using System.Runtime.CompilerServices; 
using Data;
using System.Windows;

namespace Presentation.ModelView
{
    public class ModelView : BaseViewModel, INotifyPropertyChanged
    {
        private MainAPI modelLayer;

        private string _amount = "";
        public double _width;
        public double _height;
        private double _canvasHeight = 500;
        private double _canvasWidth = 800;

        private bool _pauseFlag = false;
        //private bool _resumeFlag = false;

        //private bool _summonFlag = true;
        private bool _clearFlag = false;

        public ModelView()
        {
       
            _width = _canvasWidth;
            _height = _canvasHeight;
            this.modelLayer = MainAPI.CreateMap(_width, _height); 

            SummonCommand = new RelayCommand(SummonBalls, () => !_clearFlag);
            ClearCommand = new RelayCommand(ClearBalls, () => _clearFlag);
            StartCommand = new RelayCommand(StartBalls, () => !_pauseFlag);
            StopCommand = new RelayCommand(StopBalls, () => _pauseFlag);
        }

        public ObservableCollection<CircleAPI> Balls { 
            get => this.modelLayer.Balls;
            set => this.modelLayer.Balls = value;

        }

        public RelayCommand SummonCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }

        public double CanvasWidth
        {
            get => _canvasWidth;
            set {
                _canvasWidth = value;
                RaisePropertyChanged();
            }

        }

             public double CanvasHeight
        {
            get => _canvasHeight;
            set {
                _canvasHeight = value;
                RaisePropertyChanged();
            }
            
        }

        public string Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                RaisePropertyChanged();
            }
        }
        


        private void SummonBalls()
        {
            try
            {
                int ballsNum = int.Parse(_amount);
                 SummonCommand.RaiseCanExecuteChanged();
                 ClearCommand.RaiseCanExecuteChanged();

                if (ballsNum < 0)
                    throw new ArgumentException("Not a positive integer");
                _clearFlag = true;
                modelLayer.CreateBalls(ballsNum);
                SummonCommand.RaiseCanExecuteChanged();
                ClearCommand.RaiseCanExecuteChanged();
            }
            catch (Exception)
            {
                _amount = "";
                RaisePropertyChanged(nameof(Amount));
            }
        }

        private void StartBalls()
        {
            Console.WriteLine("tutaj");
   
            _pauseFlag = true;
            //_resumeFlag = false;

            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();

            modelLayer.Move();
        }

        private void StopBalls()
        {
            //_resumeFlag = true;
            _pauseFlag = false;

            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();

            modelLayer.StopMovement();
           
        }


        public void ClearBalls()
        {
            modelLayer.ClearMap();
            _pauseFlag = false;
            _clearFlag = false;

            SummonCommand.RaiseCanExecuteChanged();
            ClearCommand.RaiseCanExecuteChanged();

            _amount = "";

            RaisePropertyChanged(nameof(Amount));
            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

    }
}