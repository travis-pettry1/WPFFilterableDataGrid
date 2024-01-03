using System.Collections.ObjectModel;
using System.Windows;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>
        {
            new User("John", "Smith", Profession.SoftwareEngineer),
            new User("Billy", "BoJangles", Profession.Carpenter),
            new User("Steven", "Clark", Profession.Doctor)
        };


        public MainWindow()
        {
            InitializeComponent();
            self.DataContext = this;
        }
    }

    public record User(string FirstName, string LastName, Profession Profession);

    public enum Profession
    {
        SoftwareEngineer,
        Doctor,
        Plumber,
        Welder,
        Electrician,
        Carpenter,
        Mechanic,
        Teacher,
        Lawyer,
    }
}