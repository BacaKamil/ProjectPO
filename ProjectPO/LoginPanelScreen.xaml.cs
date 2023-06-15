using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace ProjectPO
{
    public partial class LoginPanelScreen : Window
    {
        public LoginPanelScreen()
        {
            InitializeComponent();
        }

        public static string EmployeeName { get; set; }
        public static string EmployeeLastName { get; set; }


        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text;
            string password = PasswordBoxPassword.Password;

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";

            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                try
                {
                    sql.Open();

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT employeeName, employeeLastName, employeePassword FROM Employees WHERE employeeLogin = @Login", sql))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("@Login", login);

                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        DataRow row = dataTable.Rows[0];
                        EmployeeName = row["employeeName"].ToString();
                        EmployeeLastName = row["employeeLastName"].ToString();
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(row["employeePassword"].ToString());

                        if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                        {
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid login or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    sql.Close();
                }

                catch (Exception)
                {
                    MessageBox.Show("Invalid login or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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