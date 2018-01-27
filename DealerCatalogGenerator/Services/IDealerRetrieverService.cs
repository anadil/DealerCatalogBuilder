using DealerCatalogGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DealerCatalogGenerator.Services
{
	internal interface IDealerRetrieverService
    {
		Task RetrieveAsync(long dealerId);
	}
}
