namespace GameStore.BLL.ModelVM.Order
{
    public class OrderCreateModel
    {
        public int UserId { get; set; }
        public List<int> GameIds { get; set; } = new List<int>();
        public string PaymentMethod { get; set; } = string.Empty;
        
    }
}