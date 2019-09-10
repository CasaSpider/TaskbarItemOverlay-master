using System.ComponentModel;
using System.Windows;
using System.Threading;

namespace WpfApplication1 {
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            var viewModel = new ViewModel();
            viewModel.Phase = "A";
            DataContext = viewModel;

            Loaded += delegate {
                var thread = new Thread(() => {
                while (true) {
                    Thread.Sleep(1000);
                    //viewModel.Count++;
                    if (viewModel.Phase == "D")
                        viewModel.Phase = "A";
                    else
                        viewModel.Phase = "D";
                    };
                }) {
                    IsBackground = true
                };
                thread.Start();
            };
        }
    }

    public enum Phases
    {
        Development,
        Test,
        Acceptance,
        Production,
    }

    public class ViewModel : INotifyPropertyChanged {
        private int _count;
        private string _phase;
        public event PropertyChangedEventHandler PropertyChanged;

        //public int Count {
        //    get { return _count; }
        //    set {
        //        if (_count != value) {
        //            _count = value;
        //            var handler = PropertyChanged;
        //            if (handler != null) {
        //                handler(this, new PropertyChangedEventArgs("Count"));
        //            }
        //        }
        //    }
        //}

        public string Phase
        {
            get { return _phase;  }
            set
            {
                if(_phase != value)
                {
                    _phase = value;
                    var handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Phase"));
                    }
                }
            }
        }
    }
}