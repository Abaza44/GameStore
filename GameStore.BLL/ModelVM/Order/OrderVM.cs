using System;
using System.Collections.Generic;

namespace GameStore.BLL.ModelVM.Order
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string? PaymentTransactionId { get; set; }

        // للتواريخ
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // لعرض أسماء الألعاب في Success.cshtml أو Invoice
        public List<string> Games { get; set; } = new List<string>();
    }
}