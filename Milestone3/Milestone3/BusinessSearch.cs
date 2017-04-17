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
        //Member Variables
        private List<StringContainer> cityList = new List<StringContainer>();
        private List<StringContainer> zipList = new List<StringContainer>();
        private List<StringContainer> catList = new List<StringContainer>();


        //Member Class Declarations
        public class StringContainer
        {
            public string myString { get; set; }

            public StringContainer(string newString)
            {
                myString = newString;
            }

            public override String ToString()
            {
                return myString;
            }

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                StringContainer p = obj as StringContainer;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return myString.Equals(p.ToString());
            }

            public override int GetHashCode()
            {
                return myString.GetHashCode();
            }
        }
        

        //Functions
        //When you select an item from the 'State' dropdown box in the 'Select Location' group.
        private void selectStateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cityList.Clear();
            String state = (String)selectStateComboBox.SelectedItem;
            if(state == null) { cityDataGrid.Items.Refresh(); return; }

            using (NpgsqlConnection sqlconn = new NpgsqlConnection(login))
            {//Start SQL interaction
                sqlconn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = sqlconn;
                    cmd.CommandText = "SELECT distinct city FROM business WHERE state = '" + state + "'order by city; ";

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cityList.Add(new StringContainer(reader.GetString(0)));
                        }
                    }

                    cityDataGrid.ItemsSource = cityList;
                    cityDataGrid.Items.Refresh();
                }
                sqlconn.Close();
            }//End SQL interaction
        }

        //When you select an item from the 'City' list in the 'Select Location' group.
        private void cityDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zipList.Clear();
            StringContainer cityObj = (StringContainer)cityDataGrid.SelectedItem;
            String state = (String)selectStateComboBox.SelectedItem;
            if (cityObj == null || state == null) { zipCodeDataGrid.Items.Refresh(); return; }
            String city = cityObj.ToString();

            using (NpgsqlConnection sqlconn = new NpgsqlConnection(login))
            {//Start SQL interaction
                sqlconn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = sqlconn;
                    cmd.CommandText = "SELECT distinct zipcode FROM business WHERE state = '" + state + "' AND city = '" + city + "' order by zipcode; ";

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           zipList.Add(new StringContainer(reader.GetString(0)));
                        }
                    }

                    zipCodeDataGrid.ItemsSource = zipList;
                    zipCodeDataGrid.Items.Refresh();
                }
                sqlconn.Close();
            }//End SQL interaction
        }

        //When you press the 'Add' button in the 'Business Category' group.
        private void businessCategoryAddButton_Click(object sender, RoutedEventArgs e)
        {
            StringContainer catObj = (StringContainer)mainBusinessCategorydataGrid.SelectedItem;
            if (catObj == null || catList.Contains(catObj)) { return; }

            catList.Add(new StringContainer(catObj.ToString()));

            selectedBusinessCategorydataGrid.ItemsSource = catList;
            selectedBusinessCategorydataGrid.Items.Refresh();
        }

        //When you press the 'Remove' button in the 'Business Category' group.
        private void businessCategoryRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            StringContainer catObj = (StringContainer)selectedBusinessCategorydataGrid.SelectedItem;
            if (catObj == null) { return; }

            catList.Remove(catObj);

            selectedBusinessCategorydataGrid.ItemsSource = catList;
            selectedBusinessCategorydataGrid.Items.Refresh();
        }
    }
}
