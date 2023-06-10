using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public ReservationScreen()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            CheckInCalendar.DisplayDateStart = DateTime.Now.Date;
            CheckOutCalendar.DisplayDateStart = DateTime.Now.Date.AddDays(1);

            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT boardSignature, boardType FROM Boards ORDER BY boardPrice ASC", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string boardSignature = reader.GetString(0);
                    string boardType = reader.GetString(1);
                    string itemData = $"{boardSignature} - {boardType}";

                    ComboBoxBoards.Items.Add(itemData);
                }
                connection.Close();
            }
        }

        private void ComboBoxRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxBoards.IsEnabled = true;
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
                MessageBox.Show("Not passed Name !");
                return;
            }
            else if (TextBoxLastName.Text.Length < 1)
            {
                MessageBox.Show("Not passed Last Name !");
                return;
            }
            else if (TextBoxEmailAddress.Text.Length < 1)
            {
                MessageBox.Show("Not passed Mail Address !");
                return;
            }
            else if (TextBoxPhoneNumber.Text.Length < 9)
            {
                MessageBox.Show("Not passed Phone Number or length of Phone Number doesn't have 9 digits !");
                return;
            }
            else if (CheckInCalendar.SelectedDate.HasValue == false)
            {
                MessageBox.Show("Not passed Check In Date !");
                return;
            }
            else if (CheckOutCalendar.SelectedDate.HasValue == false)
            {
                MessageBox.Show("Not passed Check Out Date !");
                return;
            }
            else if (ComboBoxRooms.SelectedIndex == -1)
            {
                MessageBox.Show("Not passed Room !");
                return;
            }
            else
            {
                PriceForRoom();
                PriceForBoard();

                using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO Reservations (guestName, guestLastName, phoneNumber, emailAddress, roomNumber, checkIn, checkOut, nights, boardSignature, totalPrice) VALUES (@TextBoxNameValue, @TextBoxLastNameValue, @TextBoxPhoneNumberValue, @TextBoxMailAddressValue, @ComboBoxRoomsValue, @CheckInCalendarValue, @CheckOutCalendarValue, @Nights, @ComboBoxBoardsValue, @TotalPrice)", connection);

                    command.Parameters.AddWithValue("@TextBoxNameValue", TextBoxName.Text);
                    command.Parameters.AddWithValue("@TextBoxLastNameValue", TextBoxLastName.Text);
                    command.Parameters.AddWithValue("@TextBoxPhoneNumberValue", TextBoxPhoneNumber.Text);
                    command.Parameters.AddWithValue("@TextBoxMailAddressValue", TextBoxEmailAddress.Text);
                    command.Parameters.AddWithValue("@ComboBoxRoomsValue", ComboBoxRooms.SelectedItem.ToString().Substring(0, 3));
                    command.Parameters.AddWithValue("@CheckInCalendarValue", CheckInCalendar.SelectedDate);
                    command.Parameters.AddWithValue("@CheckOutCalendarValue", CheckOutCalendar.SelectedDate);
                    command.Parameters.AddWithValue("@Nights", nights.Days);
                    command.Parameters.AddWithValue("@ComboBoxBoardsValue", ComboBoxBoards.SelectedItem.ToString().Substring(0, 2));
                    command.Parameters.AddWithValue("@TotalPrice", (pricePerNight * nights.Days) + (boardPrice * nights.Days));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reservation successful.");
                    }
                    else
                    {
                        MessageBox.Show("Reservation failed.");
                    }
                    connection.Close();
                }
            }
        }

        public void PriceForRoom()
        {
            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                connection.Open();
                try
                {
                    SqlCommand command = new SqlCommand("SELECT pricePerNight FROM Rooms WHERE roomNumber = @ComboBoxRoomsValue", connection);
                    command.Parameters.AddWithValue("@ComboBoxRoomsValue", ComboBoxRooms.SelectedItem.ToString().Substring(0, 3));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        pricePerNight = reader.GetDecimal(0);
                    }
                }
                catch (NullReferenceException) { }
                connection.Close();
            }
        }

        public void PriceForBoard()
        {
            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                connection.Open();
                try
                {
                    SqlCommand command = new SqlCommand("SELECT boardPrice FROM Boards WHERE boardSignature = @ComboBoxBoardsValue", connection);
                    command.Parameters.AddWithValue("@ComboBoxBoardsValue", ComboBoxBoards.SelectedItem.ToString().Substring(0, 2));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        boardPrice = reader.GetDecimal(0);
                    }
                }
                catch (NullReferenceException) { }

                connection.Close();
            }
        }

        public void NightsCounter()
        {
            try
            {
                nights = CheckOutCalendar.SelectedDate.Value - CheckInCalendar.SelectedDate.Value;
            }
            catch (InvalidOperationException)
            {
                
            }
        }

        public void RoomAvailability() 
        {
            OutRoomsList.Clear();
            ComboBoxRooms.Items.Clear();

            if (CheckInCalendar.SelectedDate != null && CheckOutCalendar.SelectedDate != null)
            { 
                using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("SELECT roomNumber FROM Reservations WHERE (checkIn <= @SelectCheckInDate and checkOut >= @SelectCheckInDate) or (checkIn <= @SelectCheckOutDate and checkOut >= @SelectCheckOutDate)", connection);
                        command.Parameters.AddWithValue("@SelectCheckInDate", CheckInCalendar.SelectedDate);
                        command.Parameters.AddWithValue("@SelectCheckOutDate", CheckOutCalendar.SelectedDate);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            int roomNumber = reader.GetInt32(0);
                            OutRoomsList.Add(roomNumber);
                        }
                    }
                    catch(System.Data.SqlClient.SqlException) { }
                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
                {
                    connection.Open();
                    if (OutRoomsList.Count > 0)
                    {
                        foreach (int room in OutRoomsList)
                        {
                            SqlCommand command = new SqlCommand("SELECT roomNumber, roomType FROM Rooms WHERE NOT roomNumber = @OutRoom", connection);
                            command.Parameters.AddWithValue("@OutRoom", room);
                            SqlDataReader reader = command.ExecuteReader();

                            while (reader.Read())
                            {
                                string roomNumber = reader.GetInt32(0).ToString();
                                string roomType = reader.GetString(1);
                                string itemData = $"{roomNumber} {roomType}";

                                ComboBoxRooms.Items.Add(itemData);
                            }
                        }
                    }
                    else
                    {
                        SqlCommand command = new SqlCommand("SELECT roomNumber, roomType FROM Rooms", connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            string roomNumber = reader.GetInt32(0).ToString();
                            string roomType = reader.GetString(1);
                            string itemData = $"{roomNumber} {roomType}";

                            ComboBoxRooms.Items.Add(itemData);
                        }
                    }
                    connection.Close();

                }
            }
        }

        public void TotalPriceChanger()
        {
            string totalPrice = ((nights.Days * pricePerNight) + (nights.Days * boardPrice)).ToString("F") + "zł";

            if (totalPrice != "0,00zł")
            {
                LabelTotalPrice.Content = totalPrice;
            }
        }
        private void CheckInCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckOutCalendar.IsEnabled = true;
            CheckOutCalendar.SelectedDate = null;
            CheckOutCalendar.DisplayDateStart = CheckInCalendar.SelectedDate.Value.AddDays(1);
            ComboBoxBoards.SelectedIndex = -1;
            RoomAvailability();
            NightsCounter();
            TotalPriceChanger();
        }

        private void CheckOutCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RoomAvailability();
            NightsCounter();
            TotalPriceChanger();
            ComboBoxRooms.IsEnabled = true;
        }
    }
}