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

        public MainWindow()
        {
            InitializeComponent();

            PrimeStateDropDownBox();
            PrimeCategoryBox();
            //PrimeBusinessHoursDropDownBoxs();
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
        /*
        private void PrimeBusinessHoursDropDownBoxs()
        {
            dayOfWeekComboBox.Items.Add("Sunday");
            dayOfWeekComboBox.Items.Add("Monday");
            dayOfWeekComboBox.Items.Add("Tuesday");
            dayOfWeekComboBox.Items.Add("Wednesday");
            dayOfWeekComboBox.Items.Add("Thursday");
            dayOfWeekComboBox.Items.Add("Friday");
            dayOfWeekComboBox.Items.Add("Saturday");

            fromComboBox.Items.Add("01:00");
            fromComboBox.Items.Add("02:00");
            fromComboBox.Items.Add("03:00");
            fromComboBox.Items.Add("04:00");
            fromComboBox.Items.Add("05:00");
            fromComboBox.Items.Add("06:00");
            fromComboBox.Items.Add("07:00");
            fromComboBox.Items.Add("08:00");
            fromComboBox.Items.Add("09:00");
            fromComboBox.Items.Add("10:00");
            fromComboBox.Items.Add("11:00");
            fromComboBox.Items.Add("12:00");
            fromComboBox.Items.Add("13:00");
            fromComboBox.Items.Add("14:00");
            fromComboBox.Items.Add("15:00");
            fromComboBox.Items.Add("16:00");
            fromComboBox.Items.Add("17:00");
            fromComboBox.Items.Add("18:00");
            fromComboBox.Items.Add("19:00");
            fromComboBox.Items.Add("20:00");
            fromComboBox.Items.Add("21:00");
            fromComboBox.Items.Add("22:00");
            fromComboBox.Items.Add("23:00");
            fromComboBox.Items.Add("24:00");

            toComboBox.Items.Add("01:00");
            toComboBox.Items.Add("02:00");
            toComboBox.Items.Add("03:00");
            toComboBox.Items.Add("04:00");
            toComboBox.Items.Add("05:00");
            toComboBox.Items.Add("06:00");
            toComboBox.Items.Add("07:00");
            toComboBox.Items.Add("08:00");
            toComboBox.Items.Add("09:00");
            toComboBox.Items.Add("10:00");
            toComboBox.Items.Add("11:00");
            toComboBox.Items.Add("12:00");
            toComboBox.Items.Add("13:00");
            toComboBox.Items.Add("14:00");
            toComboBox.Items.Add("15:00");
            toComboBox.Items.Add("16:00");
            toComboBox.Items.Add("17:00");
            toComboBox.Items.Add("18:00");
            toComboBox.Items.Add("19:00");
            toComboBox.Items.Add("20:00");
            toComboBox.Items.Add("21:00");
            toComboBox.Items.Add("22:00");
            toComboBox.Items.Add("23:00");
            toComboBox.Items.Add("24:00");
        }
        */
    }
}
