using DealerCatalogGenerator.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DealerCatalogGenerator.Services
{
	internal class DatasetRetrieverService : IDatasetRetrieverService
	{
		private readonly string baseUri;
		private readonly string dataSetEndPoint;
		private readonly IDealerRetrievalTrackerService dataRetrievalLogService;

		public DatasetRetrieverService(IConfiguration configuration, IDealerRetrievalTrackerService dataRetrievalLogService)
		{
			configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.dataRetrievalLogService = dataRetrievalLogService ?? throw new ArgumentNullException(nameof(dataRetrievalLogService));

			baseUri = configuration.GetValue<string>("BaseUri");
			dataSetEndPoint = configuration.GetValue<string>("DataSetEndPoint");
		}

		public async Task RetrieveAsync()
		{				
			var uri = string.Format(dataSetEndPoint, baseUri);

			using (var client = new HttpClient())
			{
				var response = await client.GetStringAsync(uri);
				dataRetrievalLogService.Dataset = JsonConvert.DeserializeObject<Dataset>(response);
			}
		}
	}
}
