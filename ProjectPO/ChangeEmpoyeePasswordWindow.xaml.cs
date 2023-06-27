using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using static ProjectPO.AddEmployeeScreen;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ProjectPO
{
    public partial class ChangeEmpoyeePasswordWindow : Window
    {
        public ChangeEmpoyeePasswordWindow()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string databaseFile = "Database.db";
            SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
            string query = "SELECT employeeName, employeeLastName FROM Employees WHERE employeeID = @EmployeeId";

            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EmployeeId", EmployeeID);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    LabelEmployeeName.Content = $"{reader["employeeName"]} {reader["employeeLastName"]}";
                }
            }
            connection.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = PasswordBoxNewPassword.Password;
            string repeatPassword = PasswordBoxRepeatPassword.Password;

            if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(repeatPassword) && (newPassword == repeatPassword))
            {
                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
                string query = "UPDATE Employees SET employeePassword = @NewPassword WHERE employeeID = @EmployeeId";

                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", EmployeeID);
                    command.Parameters.AddWithValue("@NewPassword", newPassword);

                    int rowsAffected = command.ExecuteNonQuery();

                    if(rowsAffected > 0)
                        MessageBox.Show("Password has been changed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                connection.Close();
                Close();
            }
            else
            {
                MessageBox.Show("Password has been changed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PasswordBoxNewPassword.Password = null;
                PasswordBoxRepeatPassword.Password = null;
            }
        }
    }
}
