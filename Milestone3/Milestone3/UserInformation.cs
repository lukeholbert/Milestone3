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

  public class Friend
  {
    public string Name { get; set; }
    public string AvgStars { get; set; }
    public string YelpingSince { get; set; }
    public string fid;

    public Friend(string newFid, string newRating, string newName, string newYsince)
    {
      Name = newName;
      AvgStars = newRating;
      YelpingSince = newYsince;
      fid = newFid;
    }
  }

  public class Tip
  {
    public string Name { get; set; }
    public string Business { get; set; }
    public string City { get; set; }
    public string Text { get; set; }

    public Tip(string newUname, string newBname, string newCity, string newText)
    {
      Name = newUname;
      Business = newBname;
      City = newCity;
      Text = newText;
    }
  }

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private List<Friend> friends = new List<Friend>();
    private List<Tip> tips = new List<Tip>();
    public String currentUser = "";


    private void setUserButton_Click(object sender, RoutedEventArgs e)
    {
      updateUserInfo();
    }

    private void removeButton_Click(object sender, RoutedEventArgs e)
    {
      if(friendDataGrid.SelectedIndex == -1)
      {
        return;
      }

      // Write query for removing user of selected friend (will need to keep fids stored from original query to find friend again)
      using (var sqlconn = new NpgsqlConnection(login))
      {
        sqlconn.Open();
        using (var cmd = new NpgsqlCommand())
        {
          cmd.Connection = sqlconn;
          cmd.CommandText = "DELETE FROM Friend WHERE uid ='" + idTextBox.Text + "' AND fid = '" + friends[friendDataGrid.SelectedIndex].fid + "';";
          cmd.ExecuteNonQuery();
        }

        sqlconn.Close();
      }

      updateUserInfo();
    }

    private void rateButton_Click(object sender, RoutedEventArgs e)
    {
      // Write query that updates the rating of a friend based on the new number input there

      if (friendDataGrid.SelectedIndex == -1)
      {
        return;
      }
      using (var sqlconn = new NpgsqlConnection(login))
      {
        sqlconn.Open();
        using (var cmd = new NpgsqlCommand())
        {
          cmd.Connection = sqlconn;
          double rating = (double.Parse(friends[friendDataGrid.SelectedIndex].AvgStars) + double.Parse(rateTextBox.Text))/2;
          cmd.CommandText = "UPDATE Users SET average_stars = " + rating + " WHERE uid ='" + friends[friendDataGrid.SelectedIndex].fid + "';";
          cmd.ExecuteNonQuery();
        }

        sqlconn.Close();
      }

      updateUserInfo();
    }

    private void updateUserInfo()
    {
      friends.Clear();
      tips.Clear();
      // friendDataGrid.

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
              currentUser = reader.GetString(0);
              nameTextBox.Text = reader.GetString(1);
              starsTextBox.Text = reader.GetDecimal(3).ToString();
              yelpSinceTextBox.Text = reader.GetString(4);
              fansTextBox.Text = reader.GetString(5);
              funnyTextBox.Text = reader.GetString(6);
              usefulTextBox.Text = reader.GetString(7);
              coolTextBox.Text = reader.GetString(8);
            }
          }

          cmd.CommandText = "SELECT fid, name, average_stars, yelping_since FROM Users INNER JOIN (SELECT * FROM Friend WHERE uid = '" + idTextBox.Text + "') fr ON Users.uid = fr.fid; ";
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              friends.Add(new Friend(reader.GetString(0), reader.GetDecimal(2).ToString(), reader.GetString(1), reader.GetString(3)));
            }

            friendDataGrid.ItemsSource = friends;
          }

          cmd.CommandText = "SELECT uname, name, city, text FROM Business NATURAL JOIN (SELECT uname, bid, text FROM Tip NATURAL JOIN (SELECT fid AS uid, name AS uname FROM Users INNER JOIN(SELECT * FROM Friend WHERE uid = '" + idTextBox.Text + "') fr ON Users.uid = fr.fid) AS frt) AS Tipfr;";
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              tips.Add(new Tip(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
            }

            tipDataGrid.ItemsSource = tips;
          }
        }
        sqlconn.Close();
      }

      friendDataGrid.Items.Refresh();
      tipDataGrid.Items.Refresh();
    }
  }
}
