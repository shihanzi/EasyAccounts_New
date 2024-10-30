namespace EasyAccounts.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public virtual List<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

        // One-to-Many relationship: A Supplier can have many GRNs
        public virtual List<GRN> GRNs { get; set; } = new List<GRN>();
    }
}
