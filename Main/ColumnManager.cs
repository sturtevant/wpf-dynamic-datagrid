using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Windows.Controls;

namespace WpfDynamicDataGrid
{
    internal class ColumnManager
    {
        internal ColumnManager(DataGrid grid, IEnumerable columns)
        {
            this.grid = grid;
            this.columns = columns;

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
                    var remove = grid.Columns.Where(column => e.OldItems.Contains(DynamicGrid.GetInfo(column))).ToList();
                    remove.ForEach(c => grid.Columns.Remove(c));
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
            grid.Columns.Clear();
            AddColumns(columns);
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
                var column = new DynamicGridColumn(info) { CellTemplate = DynamicGrid.GetColumnTemplate(grid) };
                actions.Add(delegate
                {
                    column.Header = info.Header;
                    column.DisplayIndex = info.DisplayIndex;
                    column.Width = info.Width;
                });
                grid.Columns.Add(column);
            }
            // Performs the initialization of each column. Some properties (e.g. DisplayIndex) must be
            // set after all columns are added in order to avoid a potential ArgumentOutOfRangeException.
            foreach (var action in actions)
                action();
        }

        private readonly DataGrid grid;
        private readonly IEnumerable columns;
    }
}