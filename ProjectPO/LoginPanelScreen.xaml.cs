using System;
using System.Data.SqlClient;
using System.Windows;

namespace ProjectPO
{
    public partial class LoginPanelScreen : Window
    {
        public LoginPanelScreen()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text;
            string password = PasswordBoxPassword.Password;

            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT employeePassword FROM Employees WHERE employeeLogin = @Login";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);

                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(command.ExecuteScalar() as string);

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred during login: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
