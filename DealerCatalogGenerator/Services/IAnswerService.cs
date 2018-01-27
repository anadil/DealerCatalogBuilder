using DealerCatalogGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealerCatalogGenerator.Services
{
    internal interface IAnswerService
    {
		void AddDealer(Dealer dealer);		
		void AddVehiclesToDealerCatalog(IEnumerable<Vehicle> vehicles);
		void SubmitAnswer();
    }
}
