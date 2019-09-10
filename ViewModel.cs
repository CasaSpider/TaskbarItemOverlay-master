using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _phase;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Phase
        {
            get { return _phase; }
            set
            {
                if (_phase != value)
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
