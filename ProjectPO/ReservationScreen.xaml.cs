using System;
using System.Collections.Generic;
using System.Data;
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

            SqlConnection sql = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
            sql.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT boardSignature, boardType FROM Boards ORDER BY boardPrice ASC", sql);

            DataTable boardsTable = new DataTable();
            dataAdapter.Fill(boardsTable);

            foreach (DataRow row in boardsTable.Rows)
            {
                string boardSignature = row["boardSignature"].ToString();
                string boardType = row["boardType"].ToString();
                string itemData = $"{boardSignature} - {boardType}";

                ComboBoxBoards.Items.Add(itemData);
            }
            sql.Close();
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

                SqlConnection sql = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
                sql.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                string insertQuery = "INSERT INTO Reservations (guestName, guestLastName, phoneNumber, emailAddress, roomNumber, checkIn, checkOut, nights, boardSignature, totalPrice) VALUES (@guestName, @guestLastName, @phoneNumber, @emailAddress, @roomNumber, @checkIn, @checkOut, @nights, @boardSignature, @totalPrice)";

                dataAdapter.InsertCommand = new SqlCommand(insertQuery, sql);
                dataAdapter.InsertCommand.Parameters.AddWithValue("@guestName", TextBoxName.Text);
                dataAdapter.InsertCommand.Parameters.AddWithValue("@guestLastName", TextBoxLastName.Text);
                dataAdapter.InsertCommand.Parameters.AddWithValue("@phoneNumber", TextBoxPhoneNumber.Text);
                dataAdapter.InsertCommand.Parameters.AddWithValue("@emailAddress", TextBoxEmailAddress.Text);
                dataAdapter.InsertCommand.Parameters.AddWithValue("@roomNumber", ComboBoxRooms.SelectedItem.ToString().Substring(0, 3));
                dataAdapter.InsertCommand.Parameters.AddWithValue("@checkIn", CheckInCalendar.SelectedDate);
                dataAdapter.InsertCommand.Parameters.AddWithValue("@checkOut", CheckOutCalendar.SelectedDate);
                dataAdapter.InsertCommand.Parameters.AddWithValue("@nights", nights.Days);
                dataAdapter.InsertCommand.Parameters.AddWithValue("@boardSignature", ComboBoxBoards.SelectedItem.ToString().Substring(0, 2));
                dataAdapter.InsertCommand.Parameters.AddWithValue("@totalPrice", (pricePerNight * nights.Days) + (boardPrice * nights.Days));

                dataAdapter.InsertCommand.ExecuteNonQuery();
                sql.Close();

                ResetAll();
            }
        }

        public void PriceForRoom()
        {
            try
            {
                SqlConnection sql = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
                sql.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT pricePerNight FROM Rooms WHERE roomNumber = @roomNumber", sql);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@roomNumber", ComboBoxRooms.SelectedItem.ToString().Substring(0, 3));

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                pricePerNight = (decimal)dataTable.Rows[0]["pricePerNight"];
                sql.Close();
            }
            catch (System.NullReferenceException) { }
        }

        public void PriceForBoard()
        {
            SqlConnection sql = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
            sql.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT boardPrice FROM Boards WHERE boardSignature = @boardSignature", sql);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@boardSignature", ComboBoxBoards.SelectedItem.ToString().Substring(0, 2));

            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            boardPrice = (decimal)dataTable.Rows[0]["boardPrice"];
            sql.Close();
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
                SqlConnection sql = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
                sql.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT roomNumber FROM Reservations WHERE (checkIn <= @checkIn AND checkOut >= @checkIn) OR (checkIn <= @checkOut AND checkOut >= @checkOut)", sql);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@checkIn", CheckInCalendar.SelectedDate);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@checkOut", CheckOutCalendar.SelectedDate);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                List<int> outRoomsList = new List<int>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int roomNumber = (int)row["roomNumber"];
                    outRoomsList.Add(roomNumber);
                }
                sql.Close();

                sql = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
                sql.Open();
                if (OutRoomsList.Count > 0)
                {
                    List<string> comboBoxItems = new List<string>();

                    foreach (int room in OutRoomsList)
                    {
                        dataAdapter = new SqlDataAdapter("SELECT roomNumber, roomType FROM Rooms WHERE NOT roomNumber = @OutRoom", sql);
                        dataAdapter.SelectCommand.Parameters.AddWithValue("@OutRoom", room);

                        dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            int roomNumber = (int)row["roomNumber"];
                            string roomType = (string)row["roomType"];
                            string itemData = $"{roomNumber} {roomType}";

                            ComboBoxRooms.Items.Add(itemData);
                        }
                    }
                }
                else
                {
                    dataAdapter = new SqlDataAdapter("SELECT roomNumber, roomType FROM Rooms", sql);

                    dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        int roomNumber = (int)row["roomNumber"];
                        string roomType = (string)row["roomType"];
                        string itemData = $"{roomNumber} {roomType}";

                        ComboBoxRooms.Items.Add(itemData);
                    }
                }
                sql.Close();
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
            CheckOutCalendar.IsEnabled = true;
            CheckOutCalendar.SelectedDate = null;
            CheckOutCalendar.DisplayDateStart = CheckInCalendar.SelectedDate.Value.AddDays(1);
            ComboBoxBoards.SelectedIndex = -1;
            LabelTotalPrice.Content = "";
            RoomAvailability();
            NightsCounter();
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