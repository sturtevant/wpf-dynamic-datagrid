using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace WpfDynamicDataGrid.Demo
{
    /// <summary>
    /// Interaction logic for WindowMain.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class WindowMain : Window
// ReSharper restore RedundantExtendsListEntry
    {
        public WindowMain()
        {
            InitializeComponent();
            Columns = new ObservableCollection<DynamicGridColumnInfo>();
            Data = new ObservableCollection<ObservableCollection<MyItem>>();
            DataContext = this;
        }

        public ObservableCollection<DynamicGridColumnInfo> Columns { get; private set; }
        public ObservableCollection<ObservableCollection<MyItem>> Data { get; private set; }

        private void ButtonAddRow_Click(object sender, RoutedEventArgs e)
        {
            Data.Add(new ObservableCollection<MyItem>());
            if (CheckBoxAutoFill.IsChecked ?? false)
                FillData();
        }

        private void ButtonAddColumn_Click(object sender, RoutedEventArgs e)
        {
            Columns.Add(new DynamicGridColumnInfo(Columns.Count, "Column" + Columns.Count)
            {
                DisplayIndex = Columns.Count,
                Width = 100
            });
            if (CheckBoxAutoFill.IsChecked ?? false)
                FillData();
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            var index = Columns.IndexOf((DynamicGridColumnInfo)((FrameworkElement)sender).DataContext);
            if (index > 0)
                Columns.Move(index, index - 1);
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            var index = Columns.IndexOf((DynamicGridColumnInfo)((FrameworkElement)sender).DataContext);
            if (index < Columns.Count - 1)
                Columns.Move(index, index + 1);
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            var remove = (DynamicGridColumnInfo)((FrameworkElement)sender).DataContext;
            foreach (var column in Columns.Where(c => c.DisplayIndex >= remove.DisplayIndex))
                --column.DisplayIndex;
            Columns.Remove(remove);
        }

        private void ButtonFill_Click(object sender, RoutedEventArgs e)
        {
            FillData();
        }

        private void FillData()
        {
            foreach (var row in Data)
                while (row.Count < Columns.Count)
                    row.Add(new MyItem { Value = String.Format("NewItem{0}.{1}", row.Count, Data.IndexOf(row)) });
        }
    }
}
