using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;
using CS301_Spend_Transactions.Models;


namespace CS301_Spend_Transactions
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<CS301_Spend_Transactions.Models.Program> Programs { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Exclusion> Exclusions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Points> Points { get; set; }
        public DbSet<PointsType> PointsTypes { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<FailedTransaction> FailedTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // Define primary key and primary key constraint name
                entity.HasKey(e => e.Id)
                    .HasName("users_pkey");
                
                entity.ToTable("users");

                // Map entity properties to DB column names
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.PhoneNo).HasColumnName("phone_no");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                
                // One User to many Cards
                entity.HasMany(u => u.Cards)
                    .WithOne(c => c.User)
                    // Define foreign key for this relationship
                    .HasForeignKey(c => c.UserId)
                    // Foreign Key constraint name 
                    .HasConstraintName("card_user_fkey");
            });
            
            modelBuilder.Entity<Card>(entity =>
            {
                // Define primary key and primary key constraint name
                entity.HasKey(c => c.Id)
                    .HasName("cards_pkey");
                
                entity.ToTable("cards");

                // Map entity properties to DB column names
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CardPan).HasColumnName("card_pan");
                entity.Property(e => e.CardType).HasColumnName("card_type");

                // Many Cards to one User
                entity.HasOne(c => c.User)
                    .WithMany(u => u.Cards)
                    // Define foreign key for this relationship
                    .HasForeignKey(c => c.UserId)
                    // Foreign Key constraint name 
                    .HasConstraintName("card_user_fkey");
                
                // One Card to many Transactions
                entity.HasMany(c => c.Transactions)
                    .WithOne(t => t.Card)
                    // Define foreign key for this relationship
                    .HasForeignKey(t => t.CardId)
                    // Foreign Key constraint name 
                    .HasConstraintName("transaction_card_fkey");
            });

            modelBuilder.Entity<Rule>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("rules_pkey");
                
                entity.ToTable("rules");
                
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CardType).HasColumnName("card_type");
                entity.Property(e => e.MinSpend).HasColumnName("min_spend");
                entity.Property(e => e.MaxSpend).HasColumnName("max_spend");
                entity.Property(e => e.ForeignSpend).HasColumnName("foreign_spend");
                

                // Many Rules to one PointsType
                entity.HasOne(r => r.PointsType)
                    .WithMany(p => p.Rules)
                    // Define foreign key for this relationship
                    .HasForeignKey(r => r.PointsTypeId)
                    // Foreign Key constraint name 
                    .HasConstraintName("rule_pointsType_fkey");
            });

            // Program itself is a reserved keyword so we need to explicitly Define namespace for this entity
            modelBuilder.Entity<Models.Program>(entity =>
            {
                entity.Property(e => e.Multiplier).HasColumnName("multiplier");
            });
            
            modelBuilder.Entity<Campaign>(entity =>
            {
                // Map entity properties to DB column names
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.StartDate).HasColumnName("start_date");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
                entity.Property(e => e.Multiplier).HasColumnName("multiplier");
                
                // Many Campaigns to one Merchant
                entity.HasOne(c => c.Merchant)
                    .WithMany(r => r.Campaigns)
                    // Define foreign key for this relationship
                    .HasForeignKey(r => r.MerchantName)
                    // Foreign Key constraint name 
                    .HasConstraintName("campaign_merchant_fkey");
            });
            
            modelBuilder.Entity<Exclusion>(entity =>
            {
                // Define primary key and primary key constraint name
                entity.HasKey(e => e.Id)
                    .HasName("exclusions_pkey");
                
                entity.ToTable("exclusions");
                
                // Map entity properties to DB column names
                entity.Property(e => e.CardType).HasColumnName("card_type");
                entity.Property(e => e.MCC).HasColumnName("mcc");
            });
            
            
            modelBuilder.Entity<Merchant>(entity =>
            {
                // Define primary key and primary key constraint name
                entity.HasKey(e => e.Name)
                    .HasName("merchants_pkey");
                
                entity.ToTable("merchants");

                // Map entity properties to DB column names
                entity.Property(e => e.MCC).HasColumnName("mcc");
                entity.Property(e => e.Name).HasColumnName("name");

                // One Merchant to many Campaigns
                entity.HasMany(e => e.Campaigns)
                    .WithOne(c => c.Merchant)
                    // Define foreign key for this relationship
                    .HasForeignKey(c => c.MerchantName)
                    // Foreign Key constraint name 
                    .HasConstraintName("campaign_merchant_fkey");
            });
            
            modelBuilder.Entity<Points>(entity =>
            {
                // Define primary key and primary key constraint name
                entity.HasKey(e => e.Id)
                    .HasName("points_pkey");
                
                entity.ToTable("points");

                // Map entity properties to DB column names
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.ProcessedDate).HasColumnName("processed_date");

                // Many Points to One Transaction
                entity.HasOne(p => p.Transaction)
                    .WithMany(t => t.AccumulatedPoints)
                    // Define foreign key for this relationship
                    .HasForeignKey(p => p.TransactionId)
                    // Foreign Key Constraint name 
                    .HasConstraintName("point_transaction_fkey");
                
                // Many Points to one PointType
                entity.HasOne(p => p.PointsType)
                    .WithMany(pt => pt.CreditedPoints)
                    // Define foreign key for this relationship
                    .HasForeignKey(p => p.PointsTypeId)
                    // Foreign Key Constraint name 
                    .HasConstraintName("point_pointsType_fkey");
            });
            
            modelBuilder.Entity<PointsType>(entity =>
            {
                // Define primary key and primary key constraint name
                entity.HasKey(c => c.Id)
                    .HasName("rewards_pkey");
                
                entity.ToTable("points_type");

                // Map entity properties to DB column names
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Unit).HasColumnName("unit");

                // One PointsType to many Points
                entity.HasMany(r => r.CreditedPoints)
                    .WithOne(p => p.PointsType)
                    // Define foreign key for this relationship
                    .HasForeignKey(p => p.PointsTypeId)
                    // Foreign Key Constraint name 
                    .HasConstraintName("pointsType_point_fkey");
                
                // One PointsType to many Rules
                entity.HasMany(pt => (ICollection<Rule>) pt.Rules)
                    .WithOne(r => r.PointsType)
                    // Define foreign key for this relationship
                    .HasForeignKey(r => r.PointsTypeId)
                    // Foreign Key Constraint name
                    .HasConstraintName("pointsType_rule_fkey");
            });
            
            modelBuilder.Entity<Transaction>(entity =>
            {
                // Define primary key and primary key constraint name
                entity.HasKey(c => c.Id)
                    .HasName("transactions_pkey");
                
                entity.ToTable("transactions");

                // Map entity properties to DB column names
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
                entity.Property(e => e.Currency).HasColumnName("currency");
                entity.Property(e => e.Amount).HasColumnName("amount");
                
                
                // Many Transactions to one Card
                entity.HasOne(t => t.Card)
                    .WithMany(c => c.Transactions)
                    // Define foreign key for this relationship
                    .HasForeignKey(t => t.CardId)
                    // Foreign Key Constraint name 
                    .HasConstraintName("transaction_card_fkey");
                
                // Many transactions to one Merchant
                entity.HasOne(t => t.Merchant)
                    .WithMany(m => m.Transactions)
                    // Define foreign key for this relationship
                    .HasForeignKey(t => t.MerchantName)
                    // Foreign Key Constraint name 
                    .HasConstraintName("transaction_merchant_fkey");
            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.HasKey(g => g.MinMCC)
                    .HasName("groups_pkey");
                
                entity.ToTable("groups");

                entity.Property(g => g.MinMCC).HasColumnName("min_mcc");
                entity.Property(g => g.MaxMCC).HasColumnName("max_mcc");
                entity.Property(g => g.Name).HasColumnName("name");
            });

            modelBuilder.Entity<FailedTransaction>(entity =>
            {
                entity.HasKey(f => f.Transaction_Id)
                    .HasName("failed_transactions_pkey");

                entity.ToTable("failed_transactions");
                
                entity.Property(f => f.Id).HasColumnName("id");
                entity.Property(f => f.Transaction_Id).HasColumnName("transaction_id");
                entity.Property(f => f.Merchant).HasColumnName("mercant");
                entity.Property(f => f.MCC).HasColumnName("mcc");
                entity.Property(f => f.Currency).HasColumnName("currency");
                entity.Property(f => f.Amount).HasColumnName("amount");
                entity.Property(f => f.Transaction_Date).HasColumnName("transaction_date");
                entity.Property(f => f.Card_Id).HasColumnName("card_id");
                entity.Property(f => f.Card_Pan).HasColumnName("card_pan");
                entity.Property(f => f.Card_Type).HasColumnName("card_type");
            });

            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}