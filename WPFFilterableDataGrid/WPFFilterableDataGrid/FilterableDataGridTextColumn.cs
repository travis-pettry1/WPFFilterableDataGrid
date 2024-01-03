using NaturalSort.Extension;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFFilterableDataGrid
{
    public class FilterableDataGridTextColumn : DataGridBoundColumn
    {
        public Button _filterButton;
        private string _columnPropName;
        public FilterableDataGrid Parent { get; set; }

        private bool _isFiltered;
        public bool IsFiltered
        {
            get { return _isFiltered; }
            set
            {
                if (_isFiltered == value)
                    return;
                _isFiltered = value;
            }
        }

        public event Action<FilterOptionData, FilterableDataGridTextColumn> FilteredClicked;
        private bool _headerIsInitialized;

        public FilterableDataGridTextColumn()
        {
            _filterButton = new Button();

            var image = new Image { Width = 15, Height = 15 };
            image.BeginInit();
            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/WPFFilterableDataGrid;component/Images/Filter.png", UriKind.Absolute));
            image.EndInit();
            _filterButton.Content = image;
            _filterButton.Click += FilterButtonClicked;

            Header = _filterButton;
        }

        private Popup _popup;
        private FilterWindow _filterWindow;


        public void InitHeader()
        {
            if(!_headerIsInitialized)
            {
                _headerIsInitialized = true;

                var sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.Children.Add(new TextBlock { Text = Header.ToString() + "   " });
                sp.Children.Add(_filterButton);

                _popup = new Popup();
                _popup.Height = 300;
                _popup.PlacementTarget = _filterButton;

                Grid grid = new Grid();
                grid.Background = new SolidColorBrush(Colors.White);

                Border b = new Border();
                b.BorderThickness = new Thickness(1);
                b.BorderBrush = new SolidColorBrush(Colors.Black);

                _filterWindow = new FilterWindow();
                _filterWindow.OnOkClicked = FilterWindowOkClicked;

                b.Child = _filterWindow;

                grid.Children.Add(b);

                _popup.Child = grid;
                sp.Children.Add(_popup);

                Header = sp;
            }            
        }

        private void FilterWindowOkClicked(List<FilterOption> options)
        {
            Parent.FiltersChanged(options, _columnPropName, this);
            _popup.IsOpen = false;
        }

        private void FilterButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!_popup.IsOpen)
            {
                _filterWindow.Options = new System.Collections.ObjectModel.ObservableCollection<FilterOption>(Parent.GetOptions(_columnPropName).OrderBy(x => x.Value, StringComparison.OrdinalIgnoreCase.WithNaturalSort()));
                _filterWindow.ColName = _columnPropName;
            }

            _popup.IsOpen = !_popup.IsOpen;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            throw new NotImplementedException();
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            _columnPropName = ((Binding)cell.Column.ClipboardContentBinding).Path.Path;

            var text = "";

            var prop = dataItem.GetType().GetProperty(_columnPropName);
            var formattting = ((Binding)cell.Column.ClipboardContentBinding).StringFormat;

            if (prop != null)
            {
                var propValue = prop.GetValue(dataItem, null);
                if (propValue != null)
                {
                    text = propValue.ToString();
                    if (formattting != null)
                    {
                        text = string.Format(formattting, propValue);
                    }
                }

            }

            return new TextBlock { Text = text, Style = cell.Style };
        }
    }

    public struct FilterOptionData
    {
        public string PropName { get; set; }
        public List<string> Values { get; set; }
    }

}
