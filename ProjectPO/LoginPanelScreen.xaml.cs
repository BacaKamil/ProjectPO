using System;
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

            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT employeeName, employeeLastName, employeePassword FROM Employees WHERE employeeLogin = @Login";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            EmployeeName = reader["employeeName"].ToString();
                            EmployeeLastName = reader["employeeLastName"].ToString();
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(reader["employeePassword"].ToString());

                            if (hashedPassword != null && BCrypt.Net.BCrypt.Verify(password, hashedPassword))
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
                        else
                        {
                            MessageBox.Show("Invalid login or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred during login: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
