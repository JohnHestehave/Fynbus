using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fynbus
{
	public class Offer : IComparable
	{
		public string RegNr { get; set; }
		public int RouteNr { get; set; }
		public float Price { get; set; }
		public float ServicePrice { get; set; }
		public float WeightedPrice { get; set; }
		public int Type { get; set; }
		public Contractor Contractor;

		public Offer(string reg, int route, float price, float sprice, int type)
		{
			RegNr = reg;
			RouteNr = route;
			Price = price;
			ServicePrice = sprice;
			Type = type;
			WeightedPrice = ((Price * 0.7f) + (ServicePrice * 0.3f));

			// Checks if a route already exists for an imported offer, create a new route if not
			foreach (Route r in Route.Routes)
			{
				if(r.RouteID == RouteNr)
				{
					return;
				}
			}
			Route.Routes.Add(new Route(RouteNr));
		}
		public int CompareTo(object other)
		{
			Offer offer = (Offer)other;
			return WeightedPrice.CompareTo(offer.WeightedPrice);
		}
	}
}
