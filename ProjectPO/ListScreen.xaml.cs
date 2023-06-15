using System;
using System.Data;
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
            SqlConnection sql = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
            sql.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT reservationID, guestName, guestLastName, roomNumber FROM Reservations WHERE NOT (checkOut < @currentDate)", sql);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@currentDate", DateTime.Today);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Reservations");

            DataTable reservationsTable = dataSet.Tables["Reservations"];

            foreach (DataRow row in reservationsTable.Rows)
            {
                int reservationID = (int)row["reservationID"];
                string guestName = (string)row["guestName"];
                string guestLastName = (string)row["guestLastName"];
                int roomNumber = (int)row["roomNumber"];

                string reservation = $"{reservationID} {guestName} {guestLastName} {roomNumber}";

                ListBoxReservations.Items.Add(reservation);
            }
            sql.Close();
        }

        private void ListBoxReservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlockInformations.Visibility = Visibility.Visible;
            ShadowTextBlockInformations.Visibility = Visibility.Visible;

            SqlConnection sql = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");

            sql.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT guestName, guestLastName, phoneNumber, emailAddress, roomNumber, checkIn, checkOut, nights, boardType, totalPrice FROM Reservations JOIN Boards ON Boards.boardSignature = Reservations.boardSignature WHERE reservationID = @reservationID", sql);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@reservationID", ListBoxReservations.SelectedItem.ToString().Split(' ')[0]);

            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                string guestName = row["guestName"].ToString();
                string guestLastName = row["guestLastName"].ToString();
                string phoneNumber = row["phoneNumber"].ToString();
                string emailAddress = row["emailAddress"].ToString();
                int roomNumber = (int)row["roomNumber"];
                DateTime checkIn = (DateTime)row["checkIn"];
                DateTime checkOut = (DateTime)row["checkOut"];
                int nights = (int)row["nights"];
                string boardType = row["boardType"].ToString();
                decimal totalPrice = (decimal)row["totalPrice"];

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

            sql.Close();

        }
    }
}
