using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectPO;
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
    }
}
