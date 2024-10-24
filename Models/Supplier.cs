namespace EasyAccounts.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual GRN GRN { get; set; }
    }
}
