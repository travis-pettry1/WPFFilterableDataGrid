using System.Windows;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<User> Users { get; set; } = new List<User>
        {
            new User("John", "Smith"),
            new User("Billy", "BoJangles"),
            new User("Steven", "Clark")
        };


        public MainWindow()
        {
            InitializeComponent();
            self.DataContext = this;
        }
    }

    public record User(string FirstName, string LastName);
}