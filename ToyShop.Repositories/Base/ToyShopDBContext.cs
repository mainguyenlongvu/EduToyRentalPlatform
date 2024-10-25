using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Repositories.Entity;

namespace ToyShop.Repositories.Base
{
    public class ToyShopDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaims, ApplicationUserRoles, ApplicationUserLogins, ApplicationRoleClaims, ApplicationUserTokens>
    {
        // constructor default
        public ToyShopDBContext() { }
        public ToyShopDBContext(DbContextOptions<ToyShopDBContext> options) : base(options) { }

        // user
        public virtual DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public virtual DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
        public virtual DbSet<ApplicationUserClaims> ApplicationUserClaims => Set<ApplicationUserClaims>();
        public virtual DbSet<ApplicationUserRoles> ApplicationUserRoles => Set<ApplicationUserRoles>();
        public virtual DbSet<ApplicationUserLogins> ApplicationUserLogins => Set<ApplicationUserLogins>();
        public virtual DbSet<ApplicationRoleClaims> ApplicationRoleClaims => Set<ApplicationRoleClaims>();
        public virtual DbSet<ApplicationUserTokens> ApplicationUserTokens => Set<ApplicationUserTokens>();


        //other
        public virtual DbSet<Toy> Toys => Set<Toy>();
        public virtual DbSet<Chat> Chats => Set<Chat>();
        public virtual DbSet<ContractEntity> ContractEntitys => Set<ContractEntity>();
        public virtual DbSet<Delivery> Deliveries => Set<Delivery>();
        public virtual DbSet<FeedBack> Feedbacks => Set<FeedBack>();
        public virtual DbSet<Message> Messages => Set<Message>();
        public virtual DbSet<RestoreToy> RestoreToys => Set<RestoreToy>();
        public virtual DbSet<RestoreToyDetail> RestoreToyDetails=> Set<RestoreToyDetail>();
        public virtual DbSet<Transaction> Transactions => Set<Transaction>();
        public virtual DbSet<ContractDetail> ContractDetails => Set<ContractDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableAnnotation = entityType.GetAnnotation("Relational:TableName");
                string tableName = tableAnnotation?.Value?.ToString() ?? "";
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            // Configure relationships for Contract -> ApplicationUser
            modelBuilder.Entity<ContractEntity>()
                .HasOne(c => c.ApplicationUser)  // Assuming 'User' navigation property in 'Contract'
                .WithMany(u => u.ContractEntitys)  // Assuming 'Contracts' in 'ApplicationUser'
                .HasForeignKey(c => c.UserId)  // Foreign key in 'Contract' table
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete
                                                     // Configure relationships for Contract -> ApplicationUser
            modelBuilder.Entity<ContractDetail>()
                .HasOne(c => c.Contract)  
                .WithMany(u => u.ContractDetails) 
                .HasForeignKey(c => c.ContractId)  
                .OnDelete(DeleteBehavior.Restrict);  

            // Configure relationships for Feedback -> Toy
            modelBuilder.Entity<FeedBack>()
                .HasOne(f => f.Toy)  // Feedback has a Toy
                .WithMany(t => t.FeedBacks)  // Feedbacks in Toy
                .HasForeignKey(f => f.ToyId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete feedbacks if a toy is deleted

            // Configure relationships for Delivery -> Contract
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.ContractEntity)  // Delivery has a Contract
                .WithMany(c => c.Deliveries)  // Deliveries in Contract
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion of contract if there are deliveries

            // Configure relationships for Transaction -> Contract
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ContractEntity)  // Transaction has a Contract
                .WithMany(c => c.Transactions)  // Transactions in Contract
                .HasForeignKey(t => t.ContractId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion of contract if there are transactions

            // Configure relationships for RestoreToy -> Contract
            modelBuilder.Entity<RestoreToy>()
                .HasOne(r => r.ContractEntity)  // RestoreToy has a reference to Contract
                .WithOne(c => c.RestoreToy)  // Contract has a reference to RestoreToy
                .HasForeignKey<RestoreToy>(r => r.ContractId)  // Corrected syntax for Foreign Key
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete RestoreToy if Contract is deleted

            modelBuilder.Entity<RestoreToyDetail>()
                .HasOne(c => c.RestoreToy)
                .WithMany(u => u.RestoreToyDetails)
                .HasForeignKey(c => c.RestoreToyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationships for Message -> Chat
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)  // Message has a Chat
                .WithMany(c => c.Messages)  // Messages in Chat
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete messages if a chat is deleted

            // Configure relationships for Chat -> ApplicationUser
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.ApplicationUser)  // Chat has a User
                .WithMany(u => u.Chats)  // Chats in ApplicationUser
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Restrict deletion of user if they have chats

            //Seed data of Toy

            modelBuilder.Entity<Toy>().HasData(
                new Toy
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ToyName = "Stacking Rings",
                    ToyImg = "stacking_rings.webp",
                    ToyDescription = "Classic colorful stacking rings toy for toddlers.",
                    ToyPriceSale = 150000000,
                    ToyPriceRent = 1500,
                    ToyRemainingQuantity = 20,
                    ToyQuantitySold = 5,
                    Option = "Stackable Rings",
                    CreatedBy = "Admin",
                    LastUpdatedBy = "Admin",
                    CreatedTime = DateTime.Parse("2024-09-29"),
                    LastUpdatedTime = DateTime.Parse("2024-09-29"),
                    DeletedTime = null
                },
                new Toy
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ToyName = "Wooden Puzzle",
                    ToyImg = "wooden_puzzle.webp",
                    ToyDescription = "A wooden puzzle with animal shapes and numbers.",
                    ToyPriceSale = 120000,
                    ToyPriceRent = 100,
                    ToyRemainingQuantity = 15,
                    ToyQuantitySold = 6,
                    Option = "Puzzle",
                    CreatedBy = "Admin",
                    LastUpdatedBy = "Admin",
                    CreatedTime = DateTime.Parse("2024-09-29"),
                    LastUpdatedTime = DateTime.Parse("2024-09-29"),
                    DeletedTime = null
                },
                new Toy
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ToyName = "Educational Toy Set",
                    ToyImg = "1.webp",
                    ToyDescription = "A vibrant interactive toy set designed for toddlers to learn shapes, numbers, and colors.",
                    ToyPriceSale = 200000000,
                    ToyPriceRent = 1000,
                    ToyRemainingQuantity = 12,
                    ToyQuantitySold = 8,
                    Option = "Interactive Learning",
                    CreatedBy = "Admin",
                    LastUpdatedBy = "Admin",
                    CreatedTime = DateTime.Parse("2024-09-29"),
                    LastUpdatedTime = DateTime.Parse("2024-09-29"),
                    DeletedTime = null
                }
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure Lazy Loading Proxies here
            optionsBuilder.UseLazyLoadingProxies(); // or optionsBuilder.UseChangeTrackingProxies();

            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=ToyShop;Integrated Security=True;Trust Server Certificate=True", b => b.MigrationsAssembly("ToyShop.Repositories"));
        }
    }
}

