using Microsoft.Windows.Controls;

namespace WpfDynamicDataGrid
{
    public interface IDynamicGridColumnInfo
    {
        int Index { get; }
        string Header { get; }
        int DisplayIndex { get; }
        DataGridLength Width { get; }
    }
}