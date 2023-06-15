using System.Windows;
using System.Windows.Input;
using static ProjectPO.LoginPanelScreen;

namespace ProjectPO
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonReservation_Click(object sender, RoutedEventArgs e)
        {
            View.Content = new ReservationScreen();
        }

        private void ButtonList_Click(object sender, RoutedEventArgs e)
        {
            View.Content = new ListScreen();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            LabelUser.Content = $"{EmployeeName} {EmployeeLastName}";
            if (EmployeeName == "Admin" && EmployeeLastName == "Admin")
            {
                ButtonAddEmployee.Visibility = Visibility.Visible;
            }
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginPanelScreen loginPanel = new LoginPanelScreen();
            loginPanel.Show();
            Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
