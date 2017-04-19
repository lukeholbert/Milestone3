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
        public static readonly string login = "Host=localhost; Username=postgres; Password=password; Database = Milestone2DB";

        public MainWindow()
        {
            InitializeComponent();
            
            //Functions needed to be called on startup to populate options in the Business tab.
            PrimeStateDropDownBox();
            PrimeCategoryBox();
            PrimeBusinessHoursDropDownBoxes();
        }
        
        private void PrimeStateDropDownBox()
        {
            using (NpgsqlConnection sqlconn = new NpgsqlConnection(login))
            {//Start SQL interaction
                sqlconn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = sqlconn;
                    cmd.CommandText = "SELECT distinct state FROM business order by state; ";

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            selectStateComboBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                sqlconn.Close();
            }//End SQL interaction
        }

        private void PrimeCategoryBox()
        {
            using (NpgsqlConnection sqlconn = new NpgsqlConnection(login))
            {//Start SQL interaction
                sqlconn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = sqlconn;
                    cmd.CommandText = "SELECT distinct name FROM category order by name; ";

                    List<StringContainer> catlist = new List<StringContainer>();

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            catlist.Add(new StringContainer(reader.GetString(0)));
                        }
                    }

                    mainBusinessCategorydataGrid.ItemsSource = catlist;
                    mainBusinessCategorydataGrid.Items.Refresh();
                }
                sqlconn.Close();
            }//End SQL interaction
        }
        
        private void PrimeBusinessHoursDropDownBoxes()
        {
            dayOfWeekComboBox.Items.Add("Sunday");
            dayOfWeekComboBox.Items.Add("Monday");
            dayOfWeekComboBox.Items.Add("Tuesday");
            dayOfWeekComboBox.Items.Add("Wednesday");
            dayOfWeekComboBox.Items.Add("Thursday");
            dayOfWeekComboBox.Items.Add("Friday");
            dayOfWeekComboBox.Items.Add("Saturday");

            for(int i = 0; i < 24; i++)
            {
                fromComboBox.Items.Add(times[i]);
                toComboBox.Items.Add(times[i]);
            }
        }

        
    }
}
