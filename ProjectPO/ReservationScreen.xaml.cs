using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT roomNumber, roomType FROM Rooms", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string roomNumber = reader.GetInt32(0).ToString();
                    string roomType = reader.GetString(1);
                    string itemData = $"{roomNumber} {roomType}";

                    ComboBoxRooms.Items.Add(itemData);
                }
                connection.Close();
            }
        }
        private void ButtonReservation_Click(object sender, RoutedEventArgs e)
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
            else if (TextBoxMailAddress.Text.Length < 1)
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
                using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(
                        "INSERT INTO Reservations (guestName, guestLastName, phoneNumber, mailAddress, roomNumber, checkIn, checkOut, nights, boardSignature) VALUES (@TextBoxNameValue, @TextBoxLastNameValue, @TextBoxPhoneNumberValue, @TextBoxMailAddressValue, @ComboBoxRoomsValue, @CheckInCalendarValue, @CheckOutCalendarValue, @Nights, @ComboBoxBoardsValue)", connection);

                    command.Parameters.AddWithValue("@TextBoxNameValue", TextBoxName.Text);
                    command.Parameters.AddWithValue("@TextBoxLastNameValue", TextBoxLastName.Text);
                    command.Parameters.AddWithValue("@TextBoxPhoneNumberValue", TextBoxPhoneNumber.Text);
                    command.Parameters.AddWithValue("@TextBoxMailAddressValue", TextBoxMailAddress.Text);
                    command.Parameters.AddWithValue("@ComboBoxRoomsValue", ComboBoxRooms.SelectedItem.ToString().Substring(0, 3));
                    command.Parameters.AddWithValue("@CheckInCalendarValue", CheckInCalendar.SelectedDate.Value.ToShortDateString());
                    command.Parameters.AddWithValue("@CheckOutCalendarValue", CheckOutCalendar.SelectedDate.Value.ToShortDateString());

                    TimeSpan nights = CheckOutCalendar.SelectedDate.Value - CheckInCalendar.SelectedDate.Value;
                    command.Parameters.AddWithValue("@Nights", nights.Days);
                    command.Parameters.AddWithValue("@ComboBoxBoardsValue", ComboBoxBoards.SelectedItem.ToString().Substring(0, 2));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reservation Complited");

                        TextBoxName.Text = string.Empty;
                        TextBoxLastName.Text = string.Empty;
                        TextBoxMailAddress.Text = string.Empty;
                        TextBoxPhoneNumber.Text = string.Empty;
                        ComboBoxRooms.SelectedIndex = -1;
                        // CheckInCalendar.SelectedDate                      <--------- DO POPRAWY !!!
                        CheckOutCalendar.IsEnabled = false;
                        ComboBoxBoards.SelectedIndex = -1;

                        connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Reservation Failed");
                    }
                }
            }
        }
    }
}
