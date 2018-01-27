using System;
using System.Collections.Generic;
using System.Text;
using DealerCatalogGenerator.Models;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DealerCatalogGenerator.Services
{
	internal class AnswerService : IAnswerService
	{
		private List<Dealer> dealers;
		private readonly string baseUri;
		private readonly string answerEndPoint;
		private readonly IDealerRetrievalTrackerService dataRetrievalLogService;
		public AnswerService(IConfiguration configuration, IDealerRetrievalTrackerService dataRetrievalLogService)
		{
			configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.dataRetrievalLogService = dataRetrievalLogService ?? throw new ArgumentNullException(nameof(dataRetrievalLogService));

			dealers = new List<Dealer>();
			baseUri = configuration.GetValue<string>("BaseUri");
			answerEndPoint = configuration.GetValue<string>("AnswerEndPoint");
		}
				
		public void AddDealer(Dealer dealer)
		{
			dealers.Add(dealer);
		}

		public void AddVehiclesToDealerCatalog(IEnumerable<Vehicle> vehicles)
		{
			foreach (var dealer in dealers)
			{
				dealer.Vehicles = vehicles
					.Where(v => v.DealerId == dealer.DealerId)
					.ToArray();
			}
		}

		public async void SubmitAnswer()
		{
			var uri = string.Format(answerEndPoint, baseUri, dataRetrievalLogService.Dataset.DatasetId);
			using (var client = new HttpClient())
			{
				var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
				var requestJson = JsonConvert.SerializeObject(new { Dealers = dealers },
					new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
				requestMessage.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

				requestMessage.Headers.Add("Accept", "application/json");
				var response = await client.SendAsync(requestMessage);

				Console.WriteLine(await response.Content.ReadAsStringAsync());
			}
		}
	}
}
