using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Domain
{
    public class DemoItem : INotifyPropertyChanged
    {
        private string name;

        private object content;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return this.name; }
            set { this.MutateVerbose(ref this.name, value, this.RaisePropertyChanged()); }
        }

        public object Content
        {
            get { return this.content; }
            set { this.MutateVerbose(ref this.content, value, this.RaisePropertyChanged()); }
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => this.PropertyChanged?.Invoke(this, args);
        }
    }
}
