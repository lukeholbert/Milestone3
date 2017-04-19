using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace Milestone3
{
    /// <summary>
    /// Interaction logic for CheckinsChart.xaml
    /// </summary>
    public partial class CheckinsChart : Window
    {
        public CheckinsChart(string bid)
        {
            InitializeComponent();

          PopulateCheckinsChart(bid);
    }
        
        private void PopulateCheckinsChart(String bid)
        {
            List<KeyValuePair<String, int>> data = new List<KeyValuePair<string, int>>();
            data.Add(new KeyValuePair<string, int>("Sunday", 0));
            data.Add(new KeyValuePair<string, int>("Monday", 0));
            data.Add(new KeyValuePair<string, int>("Tuesday", 0));
            data.Add(new KeyValuePair<string, int>("Wednesday", 0));
            data.Add(new KeyValuePair<string, int>("Thursday", 0));
            data.Add(new KeyValuePair<string, int>("Friday", 0));
            data.Add(new KeyValuePair<string, int>("Saturday", 0));

      using (NpgsqlConnection sqlconn = new NpgsqlConnection(MainWindow.login))
            {//Start SQL interaction
                sqlconn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = sqlconn;
                    cmd.CommandText = "SELECT dayofweek, SUM(num_checkins) FROM Checkin WHERE bid = '" + bid + "' group by dayofweek;";

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data[data.IndexOf(new KeyValuePair<string, int>(reader.GetString(0), 0))] = new KeyValuePair<string, int>(reader.GetString(0), int.Parse(reader.GetString(1)));
                        }
                    }

                    CheckinGraph.DataContext = data;
                }
                sqlconn.Close();
            }//End SQL interaction
        }
    }
}
