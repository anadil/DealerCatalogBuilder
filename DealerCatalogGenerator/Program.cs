using DealerCatalogGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;


namespace DealerCatalogGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
			//Bootstrap application
			var serviceProvider = Bootstrap.ConfigureServices();

			//Retrieve Vehicle List for the Dataset
			var vehiclesService = serviceProvider.GetService<IVehiclesRetrieverService>();
			var vehiclesList = vehiclesService.RetrieveAsync();

			vehiclesList.Wait();

			//Retreive Vehicles and Dealers
			var tasks = new List<Task<Models.Vehicle>>();
			foreach (var vehicleId in vehiclesList.Result.VehicleIds)
			{
				var vehicleService = serviceProvider.GetService<IVehicleRetrieverService>();				
				tasks.Add(vehicleService.RetrieveAsync(vehicleId));
			}
			Task.WaitAll(tasks.ToArray());
			var vehicles = tasks.Select(t => t.Result);

			//Prepare and Submit Answer
			var answerService = serviceProvider.GetService<IAnswerService>();

			answerService.AddVehiclesToDealerCatalog(vehicles);
			answerService.SubmitAnswer();
						
			Console.ReadKey();
		}
    }
}
