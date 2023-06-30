using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ProjectPO
{
    public partial class AddEmployeeScreen : UserControl
    {
        public static string EmployeeID { get; set; }

        public AddEmployeeScreen()
        {
            InitializeComponent();
        }

        private void ListBoxEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlockInformations.Visibility = Visibility.Visible;
            ButtonChangePassword.Visibility = Visibility.Visible;
            ButtonDeleteEmployee.Visibility = Visibility.Visible;
            TextBlockInformations.Text = string.Empty;
            InformationBlockGenerator();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            EmployessListGenerator();
        }

        public void EmployessListGenerator()
        {
            ListBoxEmployees.Items.Clear();

            string databaseFile = "Database.db";
            SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
            string query = "SELECT employeeID, employeeName, employeeLastName FROM Employees";

            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int employeeID = reader.GetInt32(reader.GetOrdinal("employeeID"));
                        string employeeName = reader.GetString(reader.GetOrdinal("employeeName"));
                        string employeeLastName = reader.GetString(reader.GetOrdinal("employeeLastName"));
                        string itemData = $"{employeeID} {employeeName} {employeeLastName}";

                        ListBoxEmployees.Items.Add(itemData);
                    }
                }
            }
            connection.Close();
        }

        public void InformationBlockGenerator()
        {
            string databaseFile = "Database.db";
            SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
            string query = "SELECT employeeName, employeeLastName, employeeLogin, employeePassword FROM Employees WHERE employeeID = @employeeID";

            try
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@employeeID", ListBoxEmployees.SelectedItem.ToString().Split(' ')[0]);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string employeeName = reader.GetString(reader.GetOrdinal("employeeName"));
                            string employeeLastName = reader.GetString(reader.GetOrdinal("employeeLastName"));
                            string employeeLogin = reader.GetString(reader.GetOrdinal("employeeLogin"));
                            string employeePassword = reader.GetString(reader.GetOrdinal("employeePassword"));

                            TextBlockInformations.Inlines.Clear();

                            TextBlockInformations.Inlines.Add(new Run("Name: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(employeeName));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Last name: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(employeeLastName));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Login: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(employeeLogin));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Password: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(employeePassword));
                        }
                    }
                }
                connection.Close();
            }
            catch (NullReferenceException) { }
        }

        private void ButtonAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxEmployeeName.Text.Length < 1)
            {
                MessageBox.Show("Not passed employee's Name !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TextBoxEmployeeName.Text.Length < 1)
            {
                MessageBox.Show("Not passed employee's Last Name !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TextBoxEmployeeLogin.Text.Length < 1)
            {
                MessageBox.Show("Not passed employee's Login !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TextBoxEmployeePassword.Password.Length < 8)
            {
                MessageBox.Show("Not passed employee's Passwords or length of Password doesn't have 9 digits !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
                string query = "INSERT INTO Employees (employeeName, employeeLastName, employeeLogin, employeePassword) VALUES (@EmployeeName, @EmployeeLastName,  @EmpoyeeLogin, @EmployeePassword)";

                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeName", TextBoxEmployeeName.Text);
                    command.Parameters.AddWithValue("@EmployeeLastName", TextBoxEmployeeLastName.Text);
                    command.Parameters.AddWithValue("@EmpoyeeLogin", TextBoxEmployeeLogin.Text);
                    command.Parameters.AddWithValue("@EmployeePassword", TextBoxEmployeePassword.Password);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee data has been successfully added to the database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add employee data to the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                connection.Close();

                TextBoxEmployeeName.Text = string.Empty;
                TextBoxEmployeeLastName.Text = string.Empty;
                TextBoxEmployeeLogin.Text = string.Empty;
                TextBoxEmployeePassword.Password = string.Empty;
            }
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            EmployeeID = ListBoxEmployees.SelectedItem.ToString().Split(' ')[0];

            ChangeEmpoyeePasswordWindow changeEmpoyeePassword = new ChangeEmpoyeePasswordWindow();
            changeEmpoyeePassword.Show();

            EmployessListGenerator();
            TextBlockInformations.Visibility = Visibility.Hidden;
            ButtonChangePassword.Visibility = Visibility.Hidden;
            ButtonDeleteEmployee.Visibility = Visibility.Hidden;
            TextBlockInformations.Text = string.Empty;
        }

        private void ButtonDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to delete this employee?", "Delete employee", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
                string query = "DELETE FROM Employees WHERE employeeID = @EmployeeId";

                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", ListBoxEmployees.SelectedItem.ToString().Split(' ')[0]);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee deleted successfully.");
                    }
                }
                connection.Close();

                TextBlockInformations.Visibility = Visibility.Hidden;
                ButtonChangePassword.Visibility = Visibility.Hidden;
                ButtonDeleteEmployee.Visibility = Visibility.Hidden;
                TextBlockInformations.Text = string.Empty;
            }
            else 
            {
                TextBlockInformations.Visibility = Visibility.Hidden;
                ButtonChangePassword.Visibility = Visibility.Hidden;
                ButtonDeleteEmployee.Visibility = Visibility.Hidden;
                TextBlockInformations.Text = string.Empty;
            }
            
            EmployessListGenerator();
        }
    }
}
