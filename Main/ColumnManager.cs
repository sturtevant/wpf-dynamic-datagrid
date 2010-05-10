using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Microsoft.Windows.Controls;

namespace WpfDynamicDataGrid
{
    internal class ColumnManager
    {
        internal ColumnManager(DataGrid grid, IEnumerable columns)
        {
            _grid = grid;
            _columns = columns;

            var observable = columns as INotifyCollectionChanged;
            if (observable != null)
                observable.CollectionChanged += ColumnsCollectionChanged;
        }

        private void ColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddColumns(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var remove = _grid.Columns.Where(column => e.OldItems.Contains(DynamicGrid.GetInfo(column))).ToList();
                    remove.ForEach(c => _grid.Columns.Remove(c));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    GenerateColumns();
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    GenerateColumns();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void GenerateColumns()
        {
            var remove = _grid.Columns.Where(c => DynamicGrid.GetInfo(c) != null).ToList();
            remove.ForEach(c => _grid.Columns.Remove(c));
            AddColumns(_columns);
        }

        private void AddColumns(IEnumerable items)
        {
            var actions = new List<Action>();
            foreach (var c in items)
            {
                var info = c as IDynamicGridColumnInfo;
                if (info == null)
                    throw new Exception(
                        "When using DynamicGrid, each element in Columns must implement IDynamicGridColumnInfo.");
                //var column = new DynamicGridColumn(info) { CellTemplate = DynamicGrid.GetColumnTemplate(_grid) };
                var column = new DynamicGridColumn(_grid, info);
                BindingOperations.SetBinding(column, DataGridTemplateColumn.CellTemplateProperty,
                                             new Binding
                                                 {
                                                     Path = new PropertyPath("(0)", DynamicGrid.ColumnTemplateProperty),
                                                     Source = _grid
                                                 });
                actions.Add(delegate
                {
                    column.Header = info.Header;
                    //if (info.DisplayIndex < _grid.Columns.Count)
                        column.DisplayIndex = info.DisplayIndex;
                    column.Width = info.Width;
                });
                _grid.Columns.Add(column);
            }
            // Performs the initialization of each column. Some properties (e.g. DisplayIndex) must be
            // set after all columns are added in order to avoid a potential ArgumentOutOfRangeException.
            foreach (var action in actions)
                action();
        }

        private readonly DataGrid _grid;
        private readonly IEnumerable _columns;
    }
}