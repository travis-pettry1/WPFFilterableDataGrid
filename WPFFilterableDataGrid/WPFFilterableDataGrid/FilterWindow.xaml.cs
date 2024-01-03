using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NaturalSort.Extension;
using System.Runtime.CompilerServices;

namespace WPFFilterableDataGrid
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : INotifyPropertyChanged
    {
        public event Action<string, FilterWindow> ResetFilter;
        public string ColName { get; set; }
        
        private ObservableCollection<FilterOption> _options;
        public ObservableCollection<FilterOption> Options 
        {
            get { return _options; }
            set 
            {
                if (_options == value)
                    return;
                _options = value;
                FilteredOptions = new ObservableCollection<FilterOption>(value);
            }
        }
        public string SearchText { get; set; }

        private ObservableCollection<FilterOption> _filteredOptions;

        public ObservableCollection<FilterOption> FilteredOptions 
        {
            get
            {
                return _filteredOptions;
            }
            set
            {
                if (_filteredOptions == value)
                    return;
                _filteredOptions = value;
                NotifyPropertyChanged("FilteredOptions");
            }
        }



        public Action<List<FilterOption>> OnOkClicked
        {
            get { return (Action<List<FilterOption>>)GetValue(OnOkClickedProperty); }
            set { SetValue(OnOkClickedProperty, value); }
        }
        public static readonly DependencyProperty OnOkClickedProperty = DependencyProperty.Register("OnOkClicked", typeof(Action<List<FilterOption>>), typeof(FilterWindow), new PropertyMetadata(null));



        public FilterWindow()
        {
            SearchText = "";
            InitializeComponent();
        }

        public FilterWindow(IEnumerable<FilterOption> items, string colName)
        {
            SearchText = "";
            ColName = colName;
            InitializeComponent();
            Options = new ObservableCollection<FilterOption>(items.ToList());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void Okay_Click(object sender, RoutedEventArgs e)
        {
            OnOkClicked?.Invoke(FilteredOptions.ToList());
        }

        private void OrderAZ_Click(object sender, RoutedEventArgs e)
        {
            FilteredOptions = new ObservableCollection<FilterOption>(FilteredOptions.OrderBy(x => x.Value, StringComparison.OrdinalIgnoreCase.WithNaturalSort()));
        }

        private void OrderZA_Click(object sender, RoutedEventArgs e)
        {
            FilteredOptions = new ObservableCollection<FilterOption>(FilteredOptions.OrderByDescending(x => x.Value, StringComparison.OrdinalIgnoreCase.WithNaturalSort()));
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilteredOptions = new ObservableCollection<FilterOption>(Options.Where(x => x.Value.ToLower().Contains(SearchText.ToLower())));            
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in FilteredOptions)
            {
                item.IsSelected = true;
            }
            FilterOptions.Items.Refresh();
        }

        private void DeselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in FilteredOptions)
            {
                item.IsSelected = false;
            }
            FilterOptions.Items.Refresh();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (ResetFilter != null)
                ResetFilter.Invoke(ColName, this);
            FilteredOptions = new ObservableCollection<FilterOption>(Options);
        }
    }

   
}
