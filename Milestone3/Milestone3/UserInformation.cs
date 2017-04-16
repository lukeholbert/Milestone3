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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace Milestone3
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    const string login = "Host=localhost; Username=postgres; Password=password; Database = Milestone2DB";

    private void setUserButton_Click(object sender, RoutedEventArgs e)
    {
      using (var sqlconn = new NpgsqlConnection(login))
      {
        sqlconn.Open();
        using (var cmd = new NpgsqlCommand())
        {
          cmd.Connection = sqlconn;
          cmd.CommandText = "SELECT * FROM Users WHERE uid = '" + idTextBox.Text + "'; ";

          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              nameTextBox.Text = reader.GetString(1);
              starsTextBox.Text = reader.GetDecimal(3).ToString();
              yelpSinceTextBox.Text = reader.GetString(4);
              fansTextBox.Text = reader.GetString(5);
              funnyTextBox.Text = reader.GetString(6);
              usefulTextBox.Text = reader.GetString(7);
              coolTextBox.Text = reader.GetString(8);
            }
          }

          cmd.CommandText = "SELECT name, average_stars, yelping_since FROM Users INNER JOIN (SELECT * FROM Friend WHERE uid = '" + idTextBox.Text + "') fr ON Users.uid = fr.fid; ";
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              // Bind query results to data view
              continue;
            }
          }

          // Last query for populating recent tips
        }
        sqlconn.Close();
      }
    }

    private void removeButton_Click(object sender, RoutedEventArgs e)
    {
      // Write query for removing user of selected friend (will need to keep fids stored from original query to find friend again)
    }

    private void rateButton_Click(object sender, RoutedEventArgs e)
    {
      // Write query that updates the rating of a friend based on the new number input there
    }
  }
}
