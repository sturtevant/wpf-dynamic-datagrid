# wpf-dynamic-datagrid
## Allows the WPF Toolkit DataGrid to work with nested collections.

The WPF Toolkit DataGrid ([http://wpf.codeplex.com]) is a useful control for viewing and editing collections of objects in WPF.
By default the DataGrid is set up to display data from collection of objects where each object in the collection is a row, 
and each property in the object is a column. DynamicGrid provides the WPF developer with an alternative way to define the columns in their DataGrid.

The DynamicGrid class allows the DataGrid to bind to a collection of collections instead of a collection of objects.
A collection of IDynamicGridColumnInfo objects and a DataTemplate are provided to the DataGrid (through Attached Properties [http://msdn.microsoft.com/en-us/library/ms749011.aspx]).
These tell DynamicGrid how to format and display the information in the DataGrid.

Automatically exported from code.google.com/p/wpf-dynamic-datagrid
