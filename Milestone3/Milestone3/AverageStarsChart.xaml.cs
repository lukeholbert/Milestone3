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
  /// Interaction logic for AverageStarsChart.xaml
  /// </summary>
  public partial class AverageStarsChart : Window
  {
    public AverageStarsChart()
    {
      InitializeComponent();
      // placeholder data so we dont get null refs when opening
      List<KeyValuePair<String, int>> data = new List<KeyValuePair<string, int>>();
      data.Add(new KeyValuePair<string, int>("Sunday", 0));
      data.Add(new KeyValuePair<string, int>("Monday", 0));
      data.Add(new KeyValuePair<string, int>("Tuesday", 0));
      AverageStarsGraph.DataContext = data;
    }
  }
}
