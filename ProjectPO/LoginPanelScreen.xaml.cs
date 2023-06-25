using System;
using System.Data.SQLite;
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

            string databaseFile = "Database.db";
            SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
            string query = "SELECT employeeName, employeeLastName, employeePassword FROM Employees WHERE employeeLogin = @Login";

            connection.Open();
            try
            {             
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        EmployeeName = reader["employeeName"].ToString();
                        EmployeeLastName = reader["employeeLastName"].ToString();
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(reader["employeePassword"].ToString());

                        if (!string.IsNullOrEmpty(hashedPassword) && BCrypt.Net.BCrypt.Verify(password, hashedPassword))
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
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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