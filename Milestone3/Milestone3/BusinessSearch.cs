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
        private List<Business> bizList = new List<Business>();

        private readonly String[] times = { "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "24:00" };


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

        public class Business
        {
            public String name { get; set; }
            public String address { get; set; }
            public String numtips { get; set; }
            public String totcheckins { get; set; }
            public string bid; 

            public Business(string name, string address, string numtips, string totcheckins, string bid)
            {
                this.name = name;
                this.address = address;
                this.numtips = numtips;
                this.totcheckins = totcheckins;
                this.bid = bid;
            }

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null) { return false; }

                // If parameter cannot be cast to Point return false.
                Business p = obj as Business;
                if ((System.Object)p == null) { return false; }

                // Return true if the fields match:
                return name.Equals(p.name) && address.Equals(p.address) && numtips.Equals(p.numtips) && totcheckins.Equals(p.totcheckins);
            }

            public override int GetHashCode()
            {
                return address.GetHashCode();
            }
        }


        //Functions

        //-----UI Response Functions------------------------------------------------------------------------
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

        private void businessDetailsShowCheckinsButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchResultsDataGrid.SelectedIndex != -1)
            {
                CheckinsChart popUpChart = new CheckinsChart(bizList[searchResultsDataGrid.SelectedIndex].bid);
                popUpChart.Show();
            }
        }

        //When you press the 'Search Businessess' button int he 'Business Category' group.
        private void searchBusinesButton_Click(object sender, RoutedEventArgs e)
        {
            using (NpgsqlConnection sqlconn = new NpgsqlConnection(login))
            {//Start SQL interaction
                bizList.Clear();
                sqlconn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = sqlconn;
                    cmd.CommandText = "SELECT name, full_address, review_count, numcheckins, bid FROM Business WHERE (" + LocationCondition() + ") AND (" + CategoryCondition() + ") AND (" + HoursCondition() + ") order by name; ";

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bizList.Add(new Business(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)));
                        }
                    }

                    searchResultsDataGrid.ItemsSource = bizList;
                    searchResultsDataGrid.Items.Refresh();
                }
                sqlconn.Close();
            }//End SQL interaction
        }


        //-----Utility Functions------------------------------------------------------------------------
        //Generates a conditional for a SQL query based upon the selected items in the 'Select Location' group.
        private String LocationCondition()
        {
            String state = (String)selectStateComboBox.SelectedItem;
            StringContainer city = (StringContainer)cityDataGrid.SelectedItem;
            StringContainer zip = (StringContainer)zipCodeDataGrid.SelectedItem;

            if(state == null)//No constraints selected.
            {
                return "true";
            }
            else if(city == null)//Only state selected.
            {
                return "state = '" + state + "'";
            }
            else if(zip == null)//Only state and city selected.
            {
                return "state = '" + state + "' AND city = '" + city.ToString() + "'";
            }
            else//State, City and Zip selected.
            {
                return "state = '" + state + "' AND city = '" + city.ToString() + "' AND zipcode = '" + zip.ToString() + "'";
            }
        }

        //Generates a conditional for a SQL query based upon the selected items in the 'Business Category' group.
        private String CategoryCondition()
        {
            if(catList.Count == 0) { return "true"; }

            StringBuilder conditional = new StringBuilder("bid IN (SELECT DISTINCT bid FROM Category WHERE ");

            foreach(StringContainer obj in catList)
            {
                conditional.Append("name = '" + obj.ToString() + "' AND ");
            }

            conditional.Remove(conditional.Length-5,5);
            conditional.Append(")");

            return conditional.ToString();
        }

        //Generates a conditional for a SQL query based upon the selected items in the 'Open Buisnessess' group.
        //Note: When you set a time in the 'From' box it means the Biz opens EXACTLY at that time, not before, not after.
        //      The same is true for the 'To' box. This is because 'before' is subjective and if we try to include a biz that opens early we'll just go around the clock...
        private String HoursCondition()
        {
            String day = (String)dayOfWeekComboBox.SelectedItem;
            String open = (String)fromComboBox.SelectedItem;
            String close = (String)toComboBox.SelectedItem;

            if(day == null || open == null || close == null) { return "true"; }

            StringBuilder conditional = new StringBuilder("bid IN (SELECT DISTINCT bid FROM Hours WHERE ");
            
            //Find where in the time array the open and close times are.
            int openIndex = -1;
            int closeIndex = -1;
            for(int i = 0; i < 24; i++)
            {
                if (times[i].Equals(open))
                    openIndex = i;
                if (times[i].Equals(close))
                    closeIndex = i;
            }

            //If the open/close time isn't in the time array or if the biz closes before, or when, it opens, invalid input.
            if (closeIndex <= openIndex || closeIndex == -1 || openIndex == -1) { return "true"; }

            //Add the day constraint.
            conditional.Append("day = '" + day + "' AND ");

            //Add the applicable open times constraint.
            conditional.Append("(");
            for(; openIndex >= 0; openIndex--)
            {
                conditional.Append("opentime = '" + times[openIndex] + "' OR ");
            }
            conditional.Remove(conditional.Length - 4, 4);
            conditional.Append(") AND ");

            //Add the applicable close times constraint.
            conditional.Append("(");
            for (; closeIndex < 24; closeIndex++)
            {
                conditional.Append("closetime = '" + times[closeIndex] + "' OR ");
            }
            conditional.Remove(conditional.Length - 4, 4);
            conditional.Append(")");

            
            conditional.Append(")");

            return conditional.ToString();
        }




        
    }
}
