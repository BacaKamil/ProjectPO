using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Data.SQLite;

namespace ProjectPO
{
    public partial class AddEmployeeScreen : UserControl
    {
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
             
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
