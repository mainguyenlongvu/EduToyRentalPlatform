namespace ToyShop.ModelViews.DeliveryModelView
{
	public class CreateDeliveryModel
	{
		public DateTime DateSend { get; set; }        
		public DateTime DateReceived { get; set; }
		public decimal DeliveryCost { get; set; }
		public string? ContractId { get; set; }
	}
}
