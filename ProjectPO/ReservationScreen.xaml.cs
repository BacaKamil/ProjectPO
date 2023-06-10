using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace ProjectPO
{
    public partial class ReservationScreen : UserControl
    {
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
                SqlCommand command = new SqlCommand("SELECT boardSignature, boardType FROM Boards", connection);
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

        private void CheckInCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckOutCalendar.IsEnabled = true;
            CheckOutCalendar.DisplayDateStart = CheckInCalendar.SelectedDate.Value.AddDays(1);
        }

        private void CheckOutCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxRooms.IsEnabled = true;

            List<int> OutRoomsList = new List<int>();

            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT roomNumber FROM Reservations WHERE (checkIn <= @SelectCheckInDate and checkOut >= @SelectCheckInDate)", connection);
                command.Parameters.AddWithValue("@SelectCheckInDate", CheckInCalendar.SelectedDate);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int roomNumber = reader.GetInt32(0);

                    OutRoomsList.Add(roomNumber);
                }
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                connection.Open();

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
                connection.Close();
            }
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
                MessageBox.Show("Not passed Phone Number or lenght of Phone Number desn't have a 9 signs !");
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
            else if (ComboBoxRooms.SelectedIndex == 0)
            {
                MessageBox.Show("Not passed Room !");
                return;
            }
            else
            {
                decimal pricePerNight = 0;
                decimal boardPrice = 0;
                TimeSpan nights;

                using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT pricePerNight FROM Rooms WHERE roomNumber = @ComboBoxRoomsValue", connection);
                    command.Parameters.AddWithValue("@ComboBoxRoomsValue", ComboBoxRooms.SelectedItem.ToString().Substring(0, 3));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        pricePerNight = reader.GetDecimal(0);
                    }

                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT boardPrice FROM Boards WHERE boardSignature = @ComboBoxBoardsValue", connection);
                    command.Parameters.AddWithValue("@ComboBoxBoardsValue", ComboBoxBoards.SelectedItem.ToString().Substring(0, 2));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        boardPrice = reader.GetDecimal(0);
                    }

                    connection.Close();
                }

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
                    nights = CheckOutCalendar.SelectedDate.Value - CheckInCalendar.SelectedDate.Value;
                    command.Parameters.AddWithValue("@Nights", nights.Days);
                    command.Parameters.AddWithValue("@ComboBoxBoardsValue", ComboBoxBoards.SelectedItem.ToString().Substring(0, 2));
                    command.Parameters.AddWithValue("@TotalPrice", (pricePerNight * nights.Days)+(boardPrice * nights.Days));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("INSERT operation successful.");
                    }
                    else
                    {
                        MessageBox.Show("INSERT operation failed.");
                    }
                    connection.Close();
                }
            }
        }
        
        //total price label
    }
}
