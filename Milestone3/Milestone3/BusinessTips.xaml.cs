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
  public class BusTip
  {
    public string Name { get; set; }
    public string Date { get; set; }
    public int Likes { get; set; }
    public string Text { get; set; }

    public BusTip(string newUname, string newDate, int newLikes, string newText)
    {
      Name = newUname;
      Date = newDate;
      Likes = newLikes;
      Text = newText;
    }
  }

  /// <summary>
  /// Interaction logic for BusinessTips.xaml
  /// </summary>
  public partial class BusinessTips : Window
  {
    public BusinessTips(string bid)
    {
      InitializeComponent();
      PopulateTips(bid);
    }

    private void PopulateTips(String bid)
    {
      List<BusTip> tips = new List<BusTip>();

      using (NpgsqlConnection sqlconn = new NpgsqlConnection(MainWindow.login))
      {//Start SQL interaction
        sqlconn.Open();
        using (NpgsqlCommand cmd = new NpgsqlCommand())
        {
          cmd.Connection = sqlconn;
          cmd.CommandText = "SELECT name, date, likes, text FROM Tip NATURAL JOIN Users WHERE bid = '" + bid + "';";

          using (NpgsqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              tips.Add(new BusTip(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3)));
            }
          }

          dataGrid.ItemsSource = tips;
        }
        sqlconn.Close();
      }//End SQL interaction
    }
  }
}
