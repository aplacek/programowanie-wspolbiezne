using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Presentation.Model;
using Presentation.ModelView.MVVMCore;
using Data;

namespace Presentation.ModelView
{
    public class ModelView : BaseViewModel
    {
        private MainAPI modelLayer;

        private string _amount = "";

        public int _width = 700;
        public int _height = 500;

        private bool _pauseFlag = false;
        //private bool _resumeFlag = false;

        //private bool _summonFlag = true;
        private bool _clearFlag = false;

        public ModelView()
        {
            this.modelLayer = MainAPI.CreateMap(_width, _height);
            Balls = new ObservableCollection<BallAPI>(modelLayer.GetBalls());

            SummonCommand = new RelayCommand(SummonBalls, () => !_clearFlag);
            ClearCommand = new RelayCommand(ClearBalls, () => _clearFlag);
            StartCommand = new RelayCommand(StartBalls, () => !_pauseFlag);
            StopCommand = new RelayCommand(StopBalls, () => _pauseFlag);
        }

        public ObservableCollection<BallAPI> Balls { get; set; }

        public RelayCommand SummonCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }

   
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
                _clearFlag = true;
                 SummonCommand.RaiseCanExecuteChanged();
                 ClearCommand.RaiseCanExecuteChanged();

                if (ballsNum < 0)
                    throw new ArgumentException("Not a positive integer");

                modelLayer.CreateBalls(ballsNum);
                Balls = new ObservableCollection<BallAPI>(modelLayer.GetBalls());
                RaisePropertyChanged(nameof(Balls));
            }
            catch (Exception)
            {
                _amount = "";
                RaisePropertyChanged(nameof(Amount));
            }
        }

        private void StartBalls()
        {
   
            _pauseFlag = true;
            //_resumeFlag = false;

            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();

            Tick();
        }

        private void StopBalls()
        {
        
            _pauseFlag = false;
            //_resumeFlag = true;

            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

        public async void Tick()
        {
            while (_pauseFlag)
            {
                await Task.Delay(10);
                modelLayer.Move();

                Balls = new ObservableCollection<BallAPI>(modelLayer.GetBalls());
                RaisePropertyChanged(nameof(Balls));
              
            }
        }

        public void ClearBalls()
        {
            modelLayer.ClearMap();
            _pauseFlag = false;
            _clearFlag = false;

            SummonCommand.RaiseCanExecuteChanged();
            ClearCommand.RaiseCanExecuteChanged();

            _amount = "";

            
            Balls = new ObservableCollection<BallAPI>(modelLayer.GetBalls());
            RaisePropertyChanged(nameof(Balls));
            RaisePropertyChanged(nameof(Amount));
            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
              
        }
    }
}