using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

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
            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT reservationID, guestName, guestLastName, roomNumber FROM Reservations", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int reservationID = reader.GetInt32(0);
                    string guestName = reader.GetString(1);
                    string guestLastName = reader.GetString(2);
                    int roomNumber = reader.GetInt32(3);
                    string itemData = $"{reservationID} {guestName} {guestLastName} {roomNumber}";

                    ListBoxReservations.Items.Add(itemData);
                }
                connection.Close();
            }
        }

        private void ListBoxReservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT guestName, guestLastName, phoneNumber, emailAddress, roomNumber, checkIn, checkOut, nights, boardType, finallyPrice FROM Reservations JOIN Boards ON Boards.boardSignature = Reservations.boardSignature WHERE reservationID = @ReservationID", connection);
                command.Parameters.AddWithValue("@ReservationID", ListBoxReservations.SelectedItem.ToString().Split(' ')[0]);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TextBlockInformations.Text = reader.GetString(0);
                    TextBlockInformations.Text += "\n" + reader.GetString(1);
                    TextBlockInformations.Text += "\n" + reader.GetString(2);
                    TextBlockInformations.Text += "\n" + reader.GetString(3);
                    TextBlockInformations.Text += "\n" + reader.GetInt32(4);
                    TextBlockInformations.Text += "\n" + reader.GetDateTime(5).ToString("dd.mm.yyyy");
                    TextBlockInformations.Text += "\n" + reader.GetDateTime(6).ToString("dd.mm.yyyy");
                    TextBlockInformations.Text += "\n" + reader.GetInt32(7);
                    TextBlockInformations.Text += "\n" + reader.GetString(8);
                    TextBlockInformations.Text += "\n" + reader.GetDecimal(9);
                }
                connection.Close();
            }
        }
    }
}
