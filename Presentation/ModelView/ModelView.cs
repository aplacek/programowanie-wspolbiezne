using BusinessLogic;
using Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Presentation.Model;

namespace Presentation.ModelView
{
    public class MainWindowModelView : INotifyPropertyChanged
    {
   
        private string _radius;
        private string _amount;
        private bool _isPaused = true;
        private MainAPI _mapApi {get; set; }

        public ObservableCollection<BallAPI> Balls { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                OnPropertyChanged(nameof(Radius)); 
            }
        }

        public string Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount)); 
            }
        }

        public RelayCommand StartCommand { get; }
        public RelayCommand PauseCommand { get; }
        public RelayCommand SummonCommand { get; }

        public MainWindowModelView()
        {
            Balls = new ObservableCollection<BallAPI>();

            _mapApi = MainAPI.CreateMap(1014, 600);  

            StartCommand = new RelayCommand(Start);
            PauseCommand = new RelayCommand(Pause);
            SummonCommand = new RelayCommand(Summon);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }


        public void Start()
        {
            _isPaused = false;
            _mapApi.Move(); 
        }

        public void Pause()
        {
            _isPaused = true;
        }


        public void Summon()
        {
            if (int.TryParse(Amount, out int amount) && double.TryParse(Radius, out double radius))
            {
                _mapApi.CreateBalls(amount, radius);  
                OnPropertyChanged("GetBalls");
      
                }
            }
        }
    }
