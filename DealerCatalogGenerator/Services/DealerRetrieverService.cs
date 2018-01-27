using System;
using System.Threading.Tasks;
using DealerCatalogGenerator.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;

namespace DealerCatalogGenerator.Services
{
	internal class DealerRetrieverService : IDealerRetrieverService
	{
		private readonly string baseUri;
		private readonly string dealerEndPoint;
		private readonly IDealerRetrievalTrackerService dataRetrievalLogService;
		private readonly IAnswerService answerService;
		public DealerRetrieverService(IConfiguration configuration, 
			IDealerRetrievalTrackerService dataRetrievalLogService,
			IAnswerService answerService)
		{
			configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.dataRetrievalLogService = dataRetrievalLogService ?? throw new ArgumentNullException(nameof(dataRetrievalLogService));
			this.answerService = answerService ?? throw new ArgumentNullException(nameof(answerService));

			baseUri = configuration.GetValue<string>("BaseUri");
			dealerEndPoint = configuration.GetValue<string>("DealerEndPoint");
		}
		public async Task RetrieveAsync(long dealerId)
		{
			var uri = string.Format(dealerEndPoint, baseUri, dataRetrievalLogService.Dataset.DatasetId, dealerId);

			using (var client = new HttpClient())
			{
				var response = await client.GetStringAsync(uri);
				var result = JsonConvert.DeserializeObject<Dealer>(response);

				answerService.AddDealer(result);
			}			
		}
	}
}
