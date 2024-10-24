using System.ComponentModel.DataAnnotations;

namespace EasyAccounts.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public int Qty { get; set; }

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
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; }
        public int ItemCategoryId { get; set; }   // Foreign key to ItemCategory
        public ItemCategory ItemCategory { get; set; } // Navigation property for the foreign key

        public int? PurchaseOrderId { get; set; } // Nullable since item might not always belong to a PO
        public PurchaseOrder PurchaseOrder { get; set; } // Navigation property
        public int? GRNId { get; set; } // Nullable for similar reasons
        public GRN GRN { get; set; } // Navigation property
    }
}
