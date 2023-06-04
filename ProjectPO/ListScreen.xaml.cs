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
                SqlCommand command = new SqlCommand("SELECT guestName, guestLastName, roomNumber FROM Reservations", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string guestName = reader.GetString(0);
                    string guestLastName = reader.GetString(1);
                    int roomNumber = reader.GetInt32(2);
                    string itemData = $"{guestName} {guestLastName} {roomNumber}";

                    ListBoxReservations.Items.Add(itemData);
                }
                connection.Close();
            }
        }

        //select listbox item --> textboxinformations
    }
}
