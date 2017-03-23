using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fynbus
{
	public class Contractor
	{
		public static List<Contractor> AllContractors = new List<Contractor>();
		public string CompanyName { get; set; }
		public string ManagerName { get; set; }
		public string Email { get; set; }
		//int CVR;

		public int Type2Count { get; set; }
		public int Type3Count { get; set; }
		public int Type5Count { get; set; }
		public int Type6Count { get; set; }
		public int Type7Count { get; set; }
		public IEnumerable<Offer> Type2 { get; set; }
		public IEnumerable<Offer> Type3 { get; set; }
		public IEnumerable<Offer> Type5 { get; set; }
		public IEnumerable<Offer> Type6 { get; set; }
		public IEnumerable<Offer> Type7 { get; set; }
		
	}
}
