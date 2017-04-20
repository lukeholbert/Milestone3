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

namespace Milestone3
{
  /// <summary>
  /// Interaction logic for BusinessCategoryChart.xaml
  /// </summary>
  public partial class BusinessCategoryChart : Window
  {
    public BusinessCategoryChart()
    {
      InitializeComponent();
      // Placeholder data so we wont get null refs when loading
      List<KeyValuePair<String, int>> data = new List<KeyValuePair<string, int>>();
      data.Add(new KeyValuePair<string, int>("Sunday", 0));
      data.Add(new KeyValuePair<string, int>("Monday", 0));
      data.Add(new KeyValuePair<string, int>("Tuesday", 0));
      BusinessCategoryGraph.DataContext = data;
    }
  }
}
