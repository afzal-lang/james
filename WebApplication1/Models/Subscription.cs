namespace WebApplication1.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public string PlanType { get; set; }      
        public decimal Amount { get; set; }       
        public string PaymentMethod { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }

     
        public virtual User User { get; set; }
    }
}
