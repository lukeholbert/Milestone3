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
  /// Interaction logic for AverageStarsChart.xaml
  /// </summary>
  public partial class AverageStarsChart : Window
  {
    public AverageStarsChart(string zipcode, string city)
    {
      InitializeComponent();
      PopulateStarsChart(zipcode, city);
    }

    private void PopulateStarsChart(string zipcode, string city)
    {
      List<KeyValuePair<String, double>> data = new List<KeyValuePair<string, double>>();

      using (NpgsqlConnection sqlconn = new NpgsqlConnection(MainWindow.login))
      {//Start SQL interaction
        sqlconn.Open();
        using (NpgsqlCommand cmd = new NpgsqlCommand())
        {
          cmd.Connection = sqlconn;
          cmd.CommandText = "SELECT name, AVG(stars) From Category NATURAL JOIN (SELECT bid, stars FROM Business WHERE zipcode = '" + zipcode + "' AND city = '" + city + "') AS foo GROUP BY name;";

          using (NpgsqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              data.Add(new KeyValuePair<string, double>(reader.GetString(0), reader.GetDouble(1)));
            }
          }

          AverageStarsGraph.DataContext = data;
        }
        sqlconn.Close();
      }//End SQL interaction
    }
  }
}
