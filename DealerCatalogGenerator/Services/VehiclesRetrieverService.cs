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
	internal class VehiclesRetrieverService : IVehiclesRetrieverService
	{
		private readonly string baseUri;
		private readonly string vehiclesEndPoint;
		private readonly IDatasetRetrieverService datasetRetrieverService;
		private readonly IDealerRetrievalTrackerService dataRetrievalLogService;

		public VehiclesRetrieverService(IConfiguration configuration, IDatasetRetrieverService datasetRetrieverService, IDealerRetrievalTrackerService dataRetrievalLogService)
		{
			configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.datasetRetrieverService = datasetRetrieverService ?? throw new ArgumentNullException(nameof(datasetRetrieverService));
			this.dataRetrievalLogService = dataRetrievalLogService ?? throw new ArgumentNullException(nameof(dataRetrievalLogService));

			baseUri = configuration.GetValue<string>("BaseUri");
			vehiclesEndPoint = configuration.GetValue<string>("VehiclesEndPoint");
					
		}

		public async Task<Vehicles> RetrieveAsync()
		{
			Vehicles result = null;

			var dataset = datasetRetrieverService.RetrieveAsync();
			dataset.Wait();

			var uri = string.Format(vehiclesEndPoint, baseUri, dataRetrievalLogService.Dataset.DatasetId);

			using (var client = new HttpClient())
			{
				var response = await client.GetStringAsync(uri);
				result = JsonConvert.DeserializeObject<Vehicles>(response);
			}
			return result;
		}		
	}
}
