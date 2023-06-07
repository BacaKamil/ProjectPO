using System.Data.SqlClient;
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
            TextBlockInformations.Visibility = Visibility.Visible;
            ShadowTextBlockInformations.Visibility = Visibility.Visible;

            using (SqlConnection connection = new SqlConnection("Server=LAPTOPKAMIL;Database=ProjectPO;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT guestName, guestLastName, phoneNumber, emailAddress, roomNumber, checkIn, checkOut, nights, boardType, finallyPrice FROM Reservations JOIN Boards ON Boards.boardSignature = Reservations.boardSignature WHERE reservationID = @ReservationID", connection);
                command.Parameters.AddWithValue("@ReservationID", ListBoxReservations.SelectedItem.ToString().Split(' ')[0]);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TextBlockInformations.Inlines.Add(new Run("Name: ") { FontWeight = FontWeights.Bold});
                    TextBlockInformations.Inlines.Add(new Run(reader.GetString(0)));

                    TextBlockInformations.Inlines.Add(new LineBreak());
                    TextBlockInformations.Inlines.Add(new LineBreak());

                    TextBlockInformations.Inlines.Add(new Run("Last name: ") { FontWeight = FontWeights.Bold });
                    TextBlockInformations.Inlines.Add(new Run(reader.GetString(1)));

                    TextBlockInformations.Inlines.Add(new LineBreak());
                    TextBlockInformations.Inlines.Add(new LineBreak());

                    TextBlockInformations.Inlines.Add(new Run("Phone number: ") { FontWeight = FontWeights.Bold });
                    TextBlockInformations.Inlines.Add(new Run(reader.GetString(2)));

                    TextBlockInformations.Inlines.Add(new LineBreak());
                    TextBlockInformations.Inlines.Add(new LineBreak());

                    TextBlockInformations.Inlines.Add(new Run("E-mail address: ") { FontWeight = FontWeights.Bold });
                    TextBlockInformations.Inlines.Add(new Run(reader.GetString(3)));

                    TextBlockInformations.Inlines.Add(new LineBreak());
                    TextBlockInformations.Inlines.Add(new LineBreak());

                    TextBlockInformations.Inlines.Add(new Run("Room number: ") { FontWeight = FontWeights.Bold });
                    TextBlockInformations.Inlines.Add(new Run(reader.GetInt32(4).ToString()));

                    TextBlockInformations.Inlines.Add(new LineBreak());
                    TextBlockInformations.Inlines.Add(new LineBreak());

                    TextBlockInformations.Inlines.Add(new Run("Check in date: ") { FontWeight = FontWeights.Bold });
                    TextBlockInformations.Inlines.Add(new Run(reader.GetDateTime(5).ToString("d")));

                    TextBlockInformations.Inlines.Add(new LineBreak());
                    TextBlockInformations.Inlines.Add(new LineBreak());

                    TextBlockInformations.Inlines.Add(new Run("Check out date: ") { FontWeight = FontWeights.Bold });
                    TextBlockInformations.Inlines.Add(new Run(reader.GetDateTime(6).ToString("d")));

                    TextBlockInformations.Inlines.Add(new LineBreak());
                    TextBlockInformations.Inlines.Add(new LineBreak());

                    TextBlockInformations.Inlines.Add(new Run("Nights: ") { FontWeight = FontWeights.Bold });
                    TextBlockInformations.Inlines.Add(new Run(reader.GetInt32(7).ToString()));

                    TextBlockInformations.Inlines.Add(new LineBreak());
                    TextBlockInformations.Inlines.Add(new LineBreak());

                    TextBlockInformations.Inlines.Add(new Run("Board type: ") { FontWeight = FontWeights.Bold });
                    TextBlockInformations.Inlines.Add(new Run(reader.GetString(8)));

                    TextBlockInformations.Inlines.Add(new LineBreak());
                    TextBlockInformations.Inlines.Add(new LineBreak());

                    TextBlockInformations.Inlines.Add(new Run("Finally price: ") { FontWeight = FontWeights.Bold });
                    TextBlockInformations.Inlines.Add(new Run(reader.GetDecimal(9).ToString("F2") + " zł"));
                }
                connection.Close();
            }
        }
    }
}
