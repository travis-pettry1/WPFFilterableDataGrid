using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Collections;

namespace WPFFilterableDataGrid
{
    public class FilterableDataGrid : DataGrid
    {
        private ObservableCollection<object> _filteredItems;
        private Dictionary<string, List<string>> _filters;

        public ObservableCollection<object> FilteredItems
        {
            get { return _filteredItems; }
            set
            {
                _filteredItems = value;
                base.ItemsSource = value;
            }
        }

        public FilterableDataGrid()
        {
            FilteredItems = new ObservableCollection<object>();
            Loaded += new RoutedEventHandler(OnLoad);
            _filters = new Dictionary<string, List<string>>();
        }

        private void OnLoad(object sender, RoutedEventArgs args)
        {
            Columns.ToList().ForEach(x =>
            {
                if (x.GetType() == typeof(FilterableDataGridTextColumn))
                {
                    var col = (FilterableDataGridTextColumn)x;
                    col.Parent = this;
                    col.UpdateHeader();
                }
            });
        }


        #region DP
        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static new readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(FilterableDataGrid), new PropertyMetadata((x, y) =>
            {
                ((FilterableDataGrid)x).FilteredItems = new ObservableCollection<object>((IEnumerable<object>)y.NewValue);
            }));


        #endregion

        public void FiltersChanged(List<FilterOption> options, string propName, FilterableDataGridTextColumn col)
        {
            var items = GetOptions(propName);

            var selectedItems = options.Where(x => x.IsSelected).ToList();

            if (!_filters.ContainsKey(propName) && selectedItems.Count() == items.Count())
                col.IsFiltered = false;
            else if (_filters.ContainsKey(propName))
                _filters[propName] = selectedItems.Select(x => x.Value).ToList();
            else if (!_filters.ContainsKey(propName))
                _filters.Add(propName, selectedItems.Select(x => x.Value).ToList());

            FilteredItems = new ObservableCollection<object>(ApplyFilters());
        }

        public List<FilterOption> GetOptions(string propName)
        {
            List<object> objects = ItemsSourceToList();

            Type type = objects[0].GetType();
            PropertyInfo prop = type.GetProperty(propName);

            List<FilterOption> result = new List<FilterOption>();

            HashSet<string> keys = new HashSet<string>();

            foreach (object obj in objects)
            {
                string val = prop.GetValue(obj, null).ToString();

                if (!keys.Contains(val))
                {
                    keys.Add(val);
                    result.Add(new FilterOption
                    {
                        Value = val,
                        IsSelected = FilteredItems.Contains(obj)
                    });
                }

            }

            return result;
        }

        private IEnumerable<object> ApplyFilters()
        {
            var temp = ItemsSourceToList();

            foreach (KeyValuePair<string, List<string>> item in _filters)
            {
                temp = temp.Where(x => item.Value.Contains(x.GetType().GetProperty(item.Key).GetValue(x, null) != null
                                                            ? x.GetType().GetProperty(item.Key).GetValue(x, null).ToString()
                                                            : "")).ToList();
            }

            return temp;
        }


        private List<object> ItemsSourceToList()
        {
            if (ItemsSource == null)
            {
                throw new ArgumentNullException(nameof(ItemsSource));
            }

            List<object> list = new List<object>();

            foreach (object item in ItemsSource)
            {
                list.Add(item);
            }

            return list;
        }
    }
}
