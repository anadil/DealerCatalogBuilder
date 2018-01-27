namespace DealerCatalogGenerator.Models
{
	internal class Dealer
	{
		public long DealerId { get; set; }
		public string Name { get; set; }
		public Vehicle[] Vehicles { get; set; }
	}
}
