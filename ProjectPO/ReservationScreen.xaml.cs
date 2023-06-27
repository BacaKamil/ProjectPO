using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace ProjectPO
{
    public partial class ReservationScreen : UserControl
    {
        public TimeSpan nights;
        public decimal pricePerNight = 0;
        public decimal boardPrice = 0;
        public List<int> OutRoomsList = new List<int>();
        public bool IsCheckedCheckbox = false;

        public ReservationScreen()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            CheckInCalendar.DisplayDateStart = DateTime.Now.Date;
            CheckOutCalendar.DisplayDateStart = DateTime.Now.Date.AddDays(1);

            string databaseFile = "Database.db";
            SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};");
            string query = "SELECT boardSignature, boardType FROM Boards ORDER BY boardPrice ASC";

            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string boardSignature = reader["boardSignature"].ToString();
                        string boardType = reader["boardType"].ToString();
                        string itemData = $"{boardSignature} - {boardType}";

                        ComboBoxBoards.Items.Add(itemData);
                    }
                }
            }

            connection.Close();
        }

        private void ComboBoxRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriceForRoom();
            TotalPriceChanger();
        }

        private void ComboBoxBoards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriceForBoard();
            TotalPriceChanger();
        }

        private void ButtonBook_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text.Length < 1)
            {
                MessageBox.Show("Not passed Name !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TextBoxLastName.Text.Length < 1)
            {
                MessageBox.Show("Not passed Last Name !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TextBoxEmailAddress.Text.Length < 1)
            {
                MessageBox.Show("Not passed E-mail Address !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TextBoxPhoneNumber.Text.Length < 9)
            {
                MessageBox.Show("Not passed Phone Number or length of Phone Number doesn't have 9 digits !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (CheckInCalendar.SelectedDate.HasValue == false)
            {
                MessageBox.Show("Not passed Check In Date !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (CheckOutCalendar.SelectedDate.HasValue == false)
            {
                MessageBox.Show("Not passed Check Out Date !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (ComboBoxRooms.SelectedIndex == -1)
            {
                MessageBox.Show("Not passed Room !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (CheckboxCorrect.IsChecked == false)
            {
                MessageBox.Show("Checkbox is not checked !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                PriceForRoom();
                PriceForBoard();

                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};");
                string query = "INSERT INTO Reservations(guestName, guestLastName, phoneNumber, emailAddress, roomNumber, checkIn, checkOut, nights, boardSignature, totalPrice) VALUES(@guestName, @guestLastName, @phoneNumber, @emailAddress, @roomNumber, @checkIn, @checkOut, @nights, @boardSignature, @totalPrice)";

                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@guestName", TextBoxName.Text);
                    command.Parameters.AddWithValue("@guestLastName", TextBoxLastName.Text);
                    command.Parameters.AddWithValue("@phoneNumber", TextBoxPhoneNumber.Text);
                    command.Parameters.AddWithValue("@emailAddress", TextBoxEmailAddress.Text);
                    command.Parameters.AddWithValue("@roomNumber", ComboBoxRooms.SelectedItem.ToString().Substring(0, 3));
                    command.Parameters.AddWithValue("@checkIn", CheckInCalendar.SelectedDate);
                    command.Parameters.AddWithValue("@checkOut", CheckOutCalendar.SelectedDate);
                    command.Parameters.AddWithValue("@nights", nights.Days);
                    command.Parameters.AddWithValue("@boardSignature", ComboBoxBoards.SelectedItem.ToString().Substring(0, 2));
                    command.Parameters.AddWithValue("@totalPrice", (pricePerNight * nights.Days) + (boardPrice * nights.Days));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reservation successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while adding the reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    connection.Close();
                }

                ResetAll();
            }
        }

        public void PriceForRoom()
        {
            try
            {
                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};");
                string query = "SELECT pricePerNight FROM Rooms WHERE roomNumber = @roomNumber";

                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@roomNumber", ComboBoxRooms.SelectedItem.ToString().Substring(0, 3));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pricePerNight = reader.GetDecimal(reader.GetOrdinal("pricePerNight"));
                        }
                        else
                        {
                            MessageBox.Show("An error occurred while retrieving the price per night.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }

                connection.Close();
            }
            catch (System.NullReferenceException) { }
        }

        public void PriceForBoard()
        {
            try
            {
                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};");
                string query = "SELECT boardPrice FROM Boards WHERE boardSignature = @boardSignature";

                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@boardSignature", ComboBoxBoards.SelectedItem.ToString().Substring(0, 2));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            boardPrice = reader.GetDecimal(reader.GetOrdinal("boardPrice"));
                        }
                        else
                        {
                            MessageBox.Show("An error occurred while retrieving the board price.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }

                connection.Close();
            }
            catch (System.NullReferenceException) { }
        }

        public void NightsCounter()
        {
            try
            {
                nights = CheckOutCalendar.SelectedDate.Value - CheckInCalendar.SelectedDate.Value;
            }
            catch (InvalidOperationException) { }
        }

        public void RoomAvailability()
        {
            OutRoomsList.Clear();
            ComboBoxRooms.Items.Clear();

            if (CheckInCalendar.SelectedDate != null && CheckOutCalendar.SelectedDate != null)
            {
                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};");
                string query = "SELECT roomNumber FROM Reservations WHERE (checkIn <= @checkIn AND checkOut >= @checkIn) OR (checkIn <= @checkOut AND checkOut >= @checkOut)";

                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@checkIn", CheckInCalendar.SelectedDate);
                    command.Parameters.AddWithValue("@checkOut", CheckOutCalendar.SelectedDate);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int roomNumber = reader.GetInt32(reader.GetOrdinal("roomNumber"));
                            OutRoomsList.Add(roomNumber);
                        }
                    }
                }

                connection.Close();
            }


            if (OutRoomsList.Count > 0)
            {
                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};");

                string excludedRooms = string.Join(",", OutRoomsList);
                string query = $"SELECT roomNumber, roomType FROM Rooms WHERE roomNumber NOT IN ({excludedRooms})";

                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int roomNumber = reader.GetInt32(reader.GetOrdinal("roomNumber"));
                            string roomType = reader.GetString(reader.GetOrdinal("roomType"));
                            string itemData = $"{roomNumber} {roomType}";

                            ComboBoxRooms.Items.Add(itemData);
                        }
                    }
                }

                connection.Close();
            }
            else
            {
                string databaseFile = "Database.db";
                SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFile};");
                string query = "SELECT roomNumber, roomType FROM Rooms";

                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int roomNumber = reader.GetInt32(reader.GetOrdinal("roomNumber"));
                            string roomType = reader.GetString(reader.GetOrdinal("roomType"));
                            string itemData = $"{roomNumber} {roomType}";

                            ComboBoxRooms.Items.Add(itemData);
                        }
                    }
                }

                connection.Close();
            }
        }

        public void TotalPriceChanger()
        {
            LabelTotalPrice.Content = "";
            string totalPrice = ((nights.Days * pricePerNight) + (nights.Days * boardPrice)).ToString("F") + "zł";

            if (totalPrice != "0,00zł")
            {
                LabelTotalPrice.Content = totalPrice;
            }
        }
        private void CheckInCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CheckOutCalendar.IsEnabled = true;
                CheckOutCalendar.SelectedDate = null;
                CheckOutCalendar.DisplayDateStart = CheckInCalendar.SelectedDate.Value.AddDays(1);
                ComboBoxBoards.SelectedIndex = -1;
                LabelTotalPrice.Content = "";
                RoomAvailability();
                NightsCounter();
            }
            catch (Exception) { }
        }

        private void CheckOutCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RoomAvailability();
            NightsCounter();
            TotalPriceChanger();
        }
        public void ResetAll()
        {
            TextBoxName.Text = string.Empty;
            TextBoxLastName.Text = string.Empty;
            TextBoxPhoneNumber.Text = string.Empty;
            TextBoxEmailAddress.Text = string.Empty;
            ComboBoxRooms.SelectedIndex = -1;
            ComboBoxBoards.SelectedIndex = -1;
            CheckInCalendar.SelectedDate = null;
            CheckOutCalendar.SelectedDate = null;
            CheckOutCalendar.IsEnabled = false;
        }
    }
}