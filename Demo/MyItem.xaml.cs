using System.ComponentModel;

namespace WpfDynamicDataGrid.Demo
{
    public class MyItem : INotifyPropertyChanged
    {
        private string mValue;
        public string Value
        {
            get { return mValue; }
            set
            {
                if (Equals(mValue, value)) return;
                mValue = value;
                Notify("Value");
            }
        }

        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}