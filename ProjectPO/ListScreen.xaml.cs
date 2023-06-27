using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ProjectPO
{
    public partial class ListScreen : UserControl
    {
        public ListScreen()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ReservationsListGenerator();
        }

        private void ListBoxReservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlockInformations.Visibility = Visibility.Visible;
            TextBlockInformations.Text = string.Empty;
            ShadowTextBlockInformations.Visibility = Visibility.Visible;

            string databaseFile = "Database.db";
            SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
            string query = "SELECT guestName, guestLastName, phoneNumber, emailAddress, roomNumber, checkIn, checkOut, nights, boardType, totalPrice FROM Reservations JOIN Boards ON Boards.boardSignature = Reservations.boardSignature WHERE reservationID = @reservationID";

            try
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@reservationID", ListBoxReservations.SelectedItem.ToString().Split(' ')[0]);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string guestName = reader.GetString(0);
                            string guestLastName = reader.GetString(1);
                            string phoneNumber = reader.GetString(2);
                            string emailAddress = reader.GetString(3);
                            int roomNumber = reader.GetInt32(4);
                            DateTime checkIn = reader.GetDateTime(5);
                            DateTime checkOut = reader.GetDateTime(6);
                            int nights = reader.GetInt32(7);
                            string boardType = reader.GetString(8);
                            decimal totalPrice = reader.GetDecimal(9);

                            TextBlockInformations.Inlines.Add(new Run("Name: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(guestName));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Last name: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(guestLastName));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Phone number: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(phoneNumber));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("E-mail address: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(emailAddress));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Room number: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(roomNumber.ToString()));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Check in date: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(checkIn.ToString("d")));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Check out date: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(checkOut.ToString("d")));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Nights: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(nights.ToString()));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Board type: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(boardType));

                            TextBlockInformations.Inlines.Add(new LineBreak());
                            TextBlockInformations.Inlines.Add(new LineBreak());

                            TextBlockInformations.Inlines.Add(new Run("Total price: ") { FontWeight = FontWeights.Bold });
                            TextBlockInformations.Inlines.Add(new Run(totalPrice.ToString("F2") + " zł"));
                        }

                        connection.Close();
                    }
                }
            }
            catch (NullReferenceException) { }

            ButtonDeleteReservation.Visibility = Visibility.Visible;

        }

        private void ButtonDeleteReservation_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Are you sure to delete this reservation?", "Delete Reservation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
                string query = "DELETE FROM Reservations WHERE reservationID = @RowId";

                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RowId", ListBoxReservations.SelectedItem.ToString().Split(' ')[0]);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Row deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No rows deleted.");
                    }

                    ListBoxReservations.SelectedItem = null;
                    TextBlockInformations.Visibility = Visibility.Hidden;
                    ShadowTextBlockInformations.Visibility = Visibility.Hidden;
                    ButtonDeleteReservation.Visibility = Visibility.Hidden;
                    ReservationsListGenerator();
                }

                connection.Close();
            }
        }

        public void ReservationsListGenerator()
        {
            ListBoxReservations.Items.Clear();

            string databaseFile = "Database.db";
            SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
            string query = "SELECT reservationID, guestName, guestLastName, roomNumber FROM Reservations WHERE NOT (checkOut < @currentDate)";

            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@currentDate", DateTime.Today);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int reservationID = reader.GetInt32(reader.GetOrdinal("reservationID"));
                        string guestName = reader.GetString(reader.GetOrdinal("guestName"));
                        string guestLastName = reader.GetString(reader.GetOrdinal("guestLastName"));
                        int roomNumber = reader.GetInt32(reader.GetOrdinal("roomNumber"));
                        string reservation = $"{reservationID} {guestName} {guestLastName} {roomNumber}";

                        ListBoxReservations.Items.Add(reservation);
                    }
                }

                connection.Close();
            }
        }
    }
}
