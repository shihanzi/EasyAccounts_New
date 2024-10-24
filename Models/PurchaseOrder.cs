namespace EasyAccounts.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }  
        public string? Customer { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }

        private double _amount;
        public double Amount
        {
            get
            {
                return _amount == 0 ? (double)Rate * Qty : _amount;
            }
            set
            {
                _amount = value;
            }
        }
        public bool IsReceived { get; set; }

        //relationships
        public List<Item> Items { get; set; } = new List<Item>();  // One-to-Many
        public Supplier Supplier { get; set; }  // One-to-One
        public int SupplierId { get; set; }  // Foreign Key

    }
}
