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
  /// Interaction logic for BusinessCategoryChart.xaml
  /// </summary>
  public partial class BusinessCategoryChart : Window
  {
    public BusinessCategoryChart(List<string> bids)
    {
      InitializeComponent();
      PopulateBusinessChart(bids);
    }

    private void PopulateBusinessChart(List<string> bids)
    {
      Dictionary<string, int> data = new Dictionary<string, int>();

      using (NpgsqlConnection sqlconn = new NpgsqlConnection(MainWindow.login))
      {//Start SQL interaction
        sqlconn.Open();
        using (NpgsqlCommand cmd = new NpgsqlCommand())
        {
          cmd.Connection = sqlconn;

          foreach (var bid in bids)
          {
            cmd.CommandText = "SELECT Category.name FROM Category JOIN Business ON Category.bid = Business.bid WHERE Category.bid = '" + bid + "';";

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
              while (reader.Read())
              {
                string cat = reader.GetString(0);
                if(!data.ContainsKey(cat))
                {
                  data.Add(cat, 0);
                }

                data[cat]++;
              }
            }
          }

          BusinessCategoryGraph.DataContext = data.ToList();
        }
        sqlconn.Close();
      }//End SQL interaction
    }
  }
}
