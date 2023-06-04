using System.Windows;
using static ProjectPO.LoginPanelScreen;

namespace ProjectPO
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReservationButton_Click(object sender, RoutedEventArgs e)
        {
            View.Content = new ReservationScreen();
        }

        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            View.Content = new ListScreen();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            LabelUser.Content = $"{EmployeeName} {EmployeeLastName}";
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanelScreen loginPanel = new LoginPanelScreen();
            loginPanel.Show();
            Close();
        }
    }
}
