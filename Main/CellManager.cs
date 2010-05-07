using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Microsoft.Windows.Controls;

namespace WpfDynamicDataGrid
{
    internal class CellManager : INotifyPropertyChanged
    {
        internal CellManager(DataGridCell cell, IDynamicGridColumnInfo info, IEnumerable items)
        {
            this.cell = cell;
            this.info = info;
            var notify = info as INotifyPropertyChanged;
            if (notify != null)
                notify.PropertyChanged += InfoPropertyChanged;

            this.items = items;
            ilist = items as IList;
            if (ilist == null && System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
            var observable = items as INotifyCollectionChanged;
            if (observable != null)
                observable.CollectionChanged += ItemsCollectionChanged;
        }

        public object Data
        {
            get
            {
                var list = ilist ?? items.Cast<object>().ToList();
                return info.Index < list.Count ? list[info.Index] : null;
            }
        }

        private void InfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Equals(e.PropertyName, "Index"))
                Notify("Data");
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex <= info.Index && info.Index <= e.NewStartingIndex + e.NewItems.Count)
                        Notify("Data");
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex <= info.Index && info.Index <= e.OldStartingIndex + e.OldItems.Count)
                        Notify("Data");
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    if ((e.NewStartingIndex <= info.Index && info.Index <= e.NewStartingIndex + e.NewItems.Count) ||
                        (e.OldStartingIndex <= info.Index && info.Index <= e.OldStartingIndex + e.OldItems.Count))
                        Notify("Data");
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Notify("Data");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DataGridCell cell;
        private readonly IDynamicGridColumnInfo info;
        private readonly IEnumerable items;
        private readonly IList ilist;
    }
}