using System.ComponentModel;
using Microsoft.Windows.Controls;

namespace WpfDynamicDataGrid
{
    public class DynamicGridColumnInfo : IDynamicGridColumnInfo, INotifyPropertyChanged
    {
        public DynamicGridColumnInfo(int index, string header)
        {
            Index = index;
            Header = header;
        }

        private int mIndex;
        public int Index
        {
            get { return mIndex; }
            set
            {
                if (Equals(mIndex, value)) return;
                mIndex = value;
                Notify("Index");
            }
        }

        private string mHeader;
        public string Header
        {
            get { return mHeader; }
            set
            {
                if (Equals(mHeader, value)) return;
                mHeader = value;
                Notify("Header");
            }
        }

        private int mDisplayIndex;
        public int DisplayIndex
        {
            get { return mDisplayIndex; }
            set
            {
                if (Equals(mDisplayIndex, value)) return;
                mDisplayIndex = value;
                Notify("DisplayIndex");
            }
        }

        private DataGridLength mWidth;
        public DataGridLength Width
        {
            get { return mWidth; }
            set
            {
                if (Equals(mWidth, value)) return;
                mWidth = value;
                Notify("Width");
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