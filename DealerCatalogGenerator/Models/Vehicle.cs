namespace DealerCatalogGenerator.Models
{
	internal class Vehicle
	{
		public long VehicleId { get; set; }		
		public long DealerId { get; set; }
		public int Year { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
	}
}