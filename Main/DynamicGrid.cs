using System.Collections;
using Microsoft.Windows.Controls;
using System.Windows;

namespace WpfDynamicDataGrid
{
    public static class DynamicGrid
    {
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached(
            "Columns", typeof(IEnumerable), typeof(DynamicGrid),
            new FrameworkPropertyMetadata(null, ColumnsChanged));
        private static void ColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = (DataGrid)sender;
            var columns = (IEnumerable)e.NewValue;
            var manager = new ColumnManager(grid, columns);
            SetColumnManager(grid, manager);
            manager.GenerateColumns();
        }
        public static void SetColumns(DataGrid element, IEnumerable value)
        {
            element.SetValue(ColumnsProperty, value);
        }
        public static IEnumerable GetColumns(DataGrid element)
        {
            return (IEnumerable)element.GetValue(ColumnsProperty);
        }

        public static readonly DependencyProperty ColumnTemplateProperty = DependencyProperty.RegisterAttached(
            "ColumnTemplate", typeof(DataTemplate), typeof(DynamicGrid),
            new FrameworkPropertyMetadata(null, ColumnTemplateChanged));
        private static void ColumnTemplateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = (DataGrid)sender;
            var manager = GetColumnManager(grid);
            if (manager != null)
                manager.GenerateColumns();
        }
        public static void SetColumnTemplate(DataGrid element, DataTemplate value)
        {
            element.SetValue(ColumnTemplateProperty, value);
        }
        public static DataTemplate GetColumnTemplate(DataGrid element)
        {
            return (DataTemplate)element.GetValue(ColumnTemplateProperty);
        }

        internal static readonly DependencyProperty ColumnManagerProperty = DependencyProperty.RegisterAttached(
            "ColumnManager", typeof(ColumnManager), typeof(DynamicGrid));
        internal static void SetColumnManager(DataGrid element, ColumnManager value)
        {
            element.SetValue(ColumnManagerProperty, value);
        }
        internal static ColumnManager GetColumnManager(DataGrid element)
        {
            return (ColumnManager)element.GetValue(ColumnManagerProperty);
        }

        internal static readonly DependencyProperty CellManagerProperty = DependencyProperty.RegisterAttached(
            "CellManager", typeof(CellManager), typeof(DynamicGrid));
        internal static void SetCellManager(DataGridCell element, CellManager value)
        {
            element.SetValue(CellManagerProperty, value);
        }
        internal static CellManager GetCellManager(DataGridCell element)
        {
            return (CellManager)element.GetValue(CellManagerProperty);
        }

        internal static readonly DependencyProperty InfoProperty = DependencyProperty.RegisterAttached(
            "Info", typeof(IDynamicGridColumnInfo), typeof(DynamicGrid));
        internal static void SetInfo(DataGridColumn element, IDynamicGridColumnInfo value)
        {
            element.SetValue(InfoProperty, value);
        }
        internal static IDynamicGridColumnInfo GetInfo(DataGridColumn element)
        {
            return (IDynamicGridColumnInfo)element.GetValue(InfoProperty);
        }
    }
}
