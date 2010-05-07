using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Microsoft.Windows.Controls;

namespace WpfDynamicDataGrid
{
    public class DynamicGridColumn : DataGridTemplateColumn
    {
        public DynamicGridColumn(IDynamicGridColumnInfo info)
        {
            DynamicGrid.SetInfo(this, info);
            var notify = info as INotifyPropertyChanged;
            if (notify != null)
                notify.PropertyChanged += InfoPropertyChanged;
        }

        private void InfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Header":
                    Header = Info.Header;
                    break;
                case "DisplayIndex":
                    DisplayIndex = Info.DisplayIndex;
                    break;
            }
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            if (dataItem == null || dataItem == CollectionView.NewItemPlaceholder)
                return base.GenerateElement(cell, dataItem);

            var items = dataItem as IEnumerable;
            if (items == null)
                throw new Exception(
                    "When using DynamicGrid, ItemsSource must implement IEnumerable " +
                    "and its members must implement IEnumerable. For better performance " +
                    "its members should also implement IList.");
            var manager = new CellManager(cell, Info, items);
            DynamicGrid.SetCellManager(cell, manager);
            cell.SetBinding(FrameworkElement.DataContextProperty, new Binding("Data") {Source = manager});
            return base.GenerateElement(cell, manager.Data);
        }

        private IDynamicGridColumnInfo Info { get { return DynamicGrid.GetInfo(this); } }
    }
}