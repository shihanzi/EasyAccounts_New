﻿namespace EasyAccounts.Models
{
    public class ItemCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; } // One category can have many items
    }
}
