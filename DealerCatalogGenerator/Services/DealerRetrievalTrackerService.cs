using System.Collections.Generic;
using DealerCatalogGenerator.Models;

namespace DealerCatalogGenerator.Services
{
	internal class DealerRetrievalTrackerService : IDealerRetrievalTrackerService
	{
		private List<long> data;

		public DealerRetrievalTrackerService()
		{
			data = new List<long>();
		}

		public Dataset Dataset { get; set; }				

		public bool MarkDealerRetrievalStarted(long id)
		{
			if (data.Contains(id))
				return false;
			else
				data.Add(id);
			return true;
		}		
	}
}
