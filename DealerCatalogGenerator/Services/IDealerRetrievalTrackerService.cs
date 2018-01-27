using System;
using System.Collections.Generic;
using System.Text;

namespace DealerCatalogGenerator.Services
{
    internal interface IDealerRetrievalTrackerService
    {		
		bool MarkDealerRetrievalStarted(long id);
		Models.Dataset Dataset { get; set; }
	}
}
