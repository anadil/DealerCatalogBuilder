using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using DealerCatalogGenerator.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DealerCatalogGenerator.Services
{
	internal class VehicleRetrieverService : IVehicleRetrieverService
	{
		private readonly string baseUri;
		private readonly string vehicleEndPoint;
		private readonly IDealerRetrievalTrackerService dataRetrievalLogService;
		private readonly IDealerRetrieverService dealerRetrieverService;
		

		public VehicleRetrieverService(IConfiguration configuration, 
			IDealerRetrieverService dealerRetrieverService,
			IDealerRetrievalTrackerService dataRetrievalLogService)
		{
			configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.dealerRetrieverService = dealerRetrieverService ?? throw new ArgumentNullException(nameof(dealerRetrieverService));
			this.dataRetrievalLogService = dataRetrievalLogService ?? throw new ArgumentNullException(nameof(dataRetrievalLogService));
			

			baseUri = configuration.GetValue<string>("BaseUri");
			vehicleEndPoint = configuration.GetValue<string>("VehicleEndPoint");

		}

		

		public async Task<Vehicle> RetrieveAsync(long vehicleId)
		{
			Vehicle result = null;			

			var uri = string.Format(vehicleEndPoint, baseUri, dataRetrievalLogService.Dataset.DatasetId, vehicleId);

			using (var client = new HttpClient())
			{
				client.Timeout = TimeSpan.FromSeconds(5);								
				var response = await client.GetStringAsync(uri);

				result = JsonConvert.DeserializeObject<Vehicle>(response);

				if (dataRetrievalLogService.MarkDealerRetrievalStarted(result.DealerId))
				{
					await dealerRetrieverService.RetrieveAsync(result.DealerId);
				}								
			}
			return result;
		}		
	}
}
