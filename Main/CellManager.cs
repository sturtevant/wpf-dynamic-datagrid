using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Microsoft.Windows.Controls;

namespace WpfDynamicDataGrid
{
    internal class CellManager : DependencyObject
    {
        internal CellManager(DataGrid grid, object source, IDynamicGridColumnInfo info)
        {
            _source = source;
            _info = info;
            var notify = info as INotifyPropertyChanged;
            if (notify != null)
                notify.PropertyChanged += InfoPropertyChanged;
            BindingOperations.SetBinding(this, DataPathProperty,
                                         new Binding
                                         {
                                             Path = new PropertyPath("(0)", DynamicGrid.DataPathProperty),
                                             Source = grid
                                         });
            UpdateItems();
        }

        private void InfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Equals(e.PropertyName, "Index"))
                UpdateData();
        }

        public static readonly DependencyProperty DataPathProperty = DependencyProperty.Register(
            "DataPath", typeof(string), typeof(CellManager),
            new FrameworkPropertyMetadata(null, DataPathChanged));
        private static void DataPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((CellManager)sender).UpdateItems();
        }
        public string DataPath
        {
            get { return (string)GetValue(DataPathProperty); }
            set { SetValue(DataPathProperty, value); }
        }

        private void UpdateItems()
        {
            if (DataPath == null || Equals(DataPath.Trim(), String.Empty))
                BindingOperations.SetBinding(this, ItemsProperty, new Binding { Source = _source });
            else
                BindingOperations.SetBinding(this, ItemsProperty, new Binding(DataPath) { Source = _source });
        }
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", typeof(IEnumerable), typeof(CellManager),
            new FrameworkPropertyMetadata(null, ItemsChanged));
        private static void ItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var manager = (CellManager)sender;
            var items = (IEnumerable)e.NewValue;
            var previous = (IEnumerable)e.OldValue;
            manager.OnItemsChanged(items, previous);
        }
        private void OnItemsChanged(IEnumerable items, IEnumerable previous)
        {
            var observable = previous as INotifyCollectionChanged;
            if (observable != null)
                observable.CollectionChanged -= ItemsCollectionChanged;

            observable = items as INotifyCollectionChanged;
            if (observable != null)
                observable.CollectionChanged += ItemsCollectionChanged;

            UpdateData();
        }
        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        private void UpdateData()
        {
            var list = Items as IList ?? Items.Cast<object>().ToList();
            var data = _info.Index < list.Count ? list[_info.Index] : null;
            if (Equals(data, Data)) return;
            Data = data;
        }
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(object), typeof(CellManager));
        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateData();
        }

        private readonly object _source;
        private readonly IDynamicGridColumnInfo _info;
    }
}