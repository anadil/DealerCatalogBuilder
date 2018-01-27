using DealerCatalogGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DealerCatalogGenerator
{
	internal class Bootstrap
	{
		public static ServiceProvider ConfigureServices()
		{
			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			var serviceProvider = new ServiceCollection()
				.AddSingleton(typeof(IConfiguration), builder.Build())
				.AddSingleton<IDealerRetrievalTrackerService, DealerRetrievalTrackerService>()
				.AddSingleton<IAnswerService, AnswerService>()
				.AddTransient<IDatasetRetrieverService, DatasetRetrieverService>()
				.AddTransient<IVehiclesRetrieverService, VehiclesRetrieverService>()
				.AddTransient<IVehicleRetrieverService, VehicleRetrieverService>()
				.AddTransient<IDealerRetrieverService, DealerRetrieverService>()
				.BuildServiceProvider();

			return serviceProvider;
		}
	}
}
