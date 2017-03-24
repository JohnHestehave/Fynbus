using Microsoft.Win32;
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
using System.IO;

namespace Fynbus
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		string stamkunder;
		string tilbud;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ImportStam_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog o = new OpenFileDialog();
			o.Filter = "CSV Files (*.csv)|*.csv";
			if(o.ShowDialog() == true)
			{
				stamkunder = o.FileName;
			}
		}
		private void ImportTilbud_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog o = new OpenFileDialog();
			o.Filter = "CSV Files (*.csv)|*.csv";
			if (o.ShowDialog() == true)
			{
				tilbud = o.FileName;
			}
		}

		private void StartImport_Click(object sender, RoutedEventArgs e)
		{
			if(stamkunder != null && tilbud != null)
			{
				Encoding enc = Encoding.GetEncoding("iso-8859-1");
				var data = File.ReadAllLines(stamkunder, enc).Skip(1).Select(x => x.Split(';'))
					.Select(x=>new Contractor() {CompanyName = x[2], ManagerName = x[1], Email = x[3], CVR = x[4],
						Type2 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 2).Select(y => new Offer() { RegNr = y[0], RouteNr = y[1], Price = float.Parse(y[3]), ServicePrice = float.Parse(y[4]) }),
						Type3 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 3).Select(y => new Offer() { RegNr = y[0], RouteNr = y[1], Price = float.Parse(y[3]), ServicePrice = float.Parse(y[4]) }),
						Type5 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 5).Select(y => new Offer() { RegNr = y[0], RouteNr = y[1], Price = float.Parse(y[3]), ServicePrice = float.Parse(y[4]) }),
						Type6 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 6).Select(y => new Offer() { RegNr = y[0], RouteNr = y[1], Price = float.Parse(y[3]), ServicePrice = float.Parse(y[4]) }),
						Type7 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 7).Select(y => new Offer() { RegNr = y[0], RouteNr = y[1], Price = float.Parse(y[3]), ServicePrice = float.Parse(y[4]) })
					});
				foreach (var item in data)
				{
					item.Type2Count = item.Type2.Count();
					item.Type3Count = item.Type3.Count();
					item.Type5Count = item.Type5.Count();
					item.Type6Count = item.Type6.Count();
					item.Type7Count = item.Type7.Count();
					Contractor.AllContractors.Add(item);
				}
				StamGrid.ItemsSource = Contractor.AllContractors;
			}
		}

		private void StamGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Contractor c = (Contractor)StamGrid.SelectedItem;
			MessageBox.Show(c.Type2.First().RegNr);
		}
	}
}
