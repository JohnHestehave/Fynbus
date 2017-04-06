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
using CsvHelper;

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
			// Import stamkunder/contractors
			OpenFileDialog o = new OpenFileDialog();
			o.Filter = "CSV Files (*.csv)|*.csv";
			if(o.ShowDialog() == true)
			{
				stamkunder = o.FileName;
			}
		}
		private void ImportTilbud_Click(object sender, RoutedEventArgs e)
		{
			// Import tilbud/offers
			OpenFileDialog o = new OpenFileDialog();
			o.Filter = "CSV Files (*.csv)|*.csv";
			if (o.ShowDialog() == true)
			{
				tilbud = o.FileName;
			}
		}

		private void StartImport_Click(object sender, RoutedEventArgs e)
		{
			// Check if both contractors and offers csv's has been chosen
			if(stamkunder != null && tilbud != null)
			{
				// Start reading .csv files and instantiating contractors/offers objects
				Encoding enc = Encoding.GetEncoding("iso-8859-1");
				var data = File.ReadAllLines(stamkunder, enc).Skip(1).Select(x => x.Split(';'))
					.Select(x=>new Contractor() {CompanyName = x[2], ManagerName = x[1], Email = x[3], CVR = x[4],
						Type2 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 2).Select(y => new Offer(y[0], int.Parse(y[1]), float.Parse(y[3]), float.Parse(y[4]), int.Parse(y[5]))),
						Type3 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 3).Select(y => new Offer(y[0], int.Parse(y[1]), float.Parse(y[3]), float.Parse(y[4]), int.Parse(y[5]))),
						Type5 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 5).Select(y => new Offer(y[0], int.Parse(y[1]), float.Parse(y[3]), float.Parse(y[4]), int.Parse(y[5]))),
						Type6 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 6).Select(y => new Offer(y[0], int.Parse(y[1]), float.Parse(y[3]), float.Parse(y[4]), int.Parse(y[5]))),
						Type7 = File.ReadAllLines(tilbud, enc).Skip(1).Select(y => y.Split(';')).Where(y => y[2] == x[0] && int.Parse(y[5]) == 7).Select(y => new Offer(y[0], int.Parse(y[1]), float.Parse(y[3]), float.Parse(y[4]), int.Parse(y[5])))
					});
				foreach (Contractor item in data)
				{
					item.Type2Count = item.Type2.Count();
					item.Type3Count = item.Type3.Count();
					item.Type5Count = item.Type5.Count();
					item.Type6Count = item.Type6.Count();
					item.Type7Count = item.Type7.Count();
					Contractor.AllContractors.Add(item);
				}
				// Add each offer into the corresponding route
				foreach (Route r in Route.Routes)
				{
					foreach (Contractor c in Contractor.AllContractors)
					{
						foreach (Offer o in c.Type2)
						{
							o.Contractor = c;
							if (o.RouteNr == r.RouteID) r.Offers.Add(o);
						}
						foreach (Offer o in c.Type3)
						{
							o.Contractor = c;
							if (o.RouteNr == r.RouteID) r.Offers.Add(o);
						}
						foreach (Offer o in c.Type5)
						{
							o.Contractor = c;
							if (o.RouteNr == r.RouteID) r.Offers.Add(o);
						}
						foreach (Offer o in c.Type6)
						{
							o.Contractor = c;
							if (o.RouteNr == r.RouteID) r.Offers.Add(o);
						}
						foreach (Offer o in c.Type7)
						{
							o.Contractor = c;
							if (o.RouteNr == r.RouteID) r.Offers.Add(o);
						}
					}
				}
				// sort each list of offers from a route by weighted price
				foreach(Route r in Route.Routes)
				{
					r.Offers.Sort();
				}
				StamGrid.ItemsSource = Contractor.AllContractors;
				MessageBox.Show("Import completed.");
			}
		}
		

		private void ExportCSV_Click(object sender, RoutedEventArgs e)
		{
			// Exports the list of routes and sorted offers into .csv
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "CSV File(*.txt)|*.csv|All(*.*)|*";
			if(sfd.ShowDialog() == true)
			{
				StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding("iso-8859-1"));
				sw.AutoFlush = true;
				CsvWriter csv = new CsvWriter(sw);
				foreach (Route r in Route.Routes)
				{
					csv.WriteField("Rute;Firma;Vogntype;Pris(Køretid);Pris(Ventetid);Pris(Vægtet)");
					foreach (Offer o in r.Offers)
					{
						csv.NextRecord();
						csv.WriteField($"{r.RouteID};{o.Contractor.CompanyName};{o.Type};{o.Price};{o.ServicePrice};{o.WeightedPrice}");
					}
					csv.NextRecord();
					csv.NextRecord();
				}
				MessageBox.Show("Export completed.");
			}
		}
	}
}
