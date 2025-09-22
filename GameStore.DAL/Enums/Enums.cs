

namespace GameStore.DAL.Enums
{
    public enum UserRole { User = 0,Publisher = 1, Admin = 2}
    public enum GameStatus { Pending = 0, Approved = 1, Rejected = 2 }
    public enum PaymentProvider { Paypal, Stripe}
    public enum PaymentStatus { Pending, Success, Failed }
    public enum OrderStatus { Pending, Completed, Failed }
    public enum EarningStatus { Pending, Paid }

    public enum PayoutStatus { Pending = 0, Approved = 1, Rejected = 2 }
}
