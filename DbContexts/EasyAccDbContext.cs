using EasyAccounts.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyAccounts.DbContexts
{
    public class EasyAccDbContext : DbContext
    {
        public EasyAccDbContext(DbContextOptions<EasyAccDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<GRN> GRNs { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>().ToTable("UserRole"); //name the table in sigularity

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Item - ItemCategory relationship
            modelBuilder.Entity<Item>()
                .HasOne(i => i.ItemCategory)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.ItemCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Item - GRN relationship
            modelBuilder.Entity<Item>()
                .HasOne(i => i.GRN)
                .WithMany(grn => grn.Items)
                .HasForeignKey(i => i.GRNId)
                .OnDelete(DeleteBehavior.Restrict);

            // Item - PurchaseOrder relationship
            modelBuilder.Entity<Item>()
                .HasOne(i => i.PurchaseOrder)
                .WithMany(po => po.Items)
                .HasForeignKey(i => i.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseOrder - Supplier relationship
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // GRN - Supplier relationship
            modelBuilder.Entity<GRN>()
                .HasOne(grn => grn.Supplier)
                .WithMany(g => g.GRNs)
                .HasForeignKey(grn => grn.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // GRN - PurchaseOrder relationship (with delete behavior specified)
            modelBuilder.Entity<GRN>()
                .HasOne(grn => grn.PurchaseOrder)
                .WithOne()
                .HasForeignKey<GRN>(gn => gn.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
