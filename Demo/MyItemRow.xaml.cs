using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WpfDynamicDataGrid.Demo
{
    public class MyItemRow : INotifyPropertyChanged
    {
        public ObservableCollection<MyItem> Items { get { return _items; } }

        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ObservableCollection<MyItem> _items = new ObservableCollection<MyItem>();
    }
}