using DAL.Model;
using Microsoft.Data.Entity;

namespace DAL.DbContext
{
    public class FamilyBudgetContext : Microsoft.Data.Entity.DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=FamileBudget.db");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Asset>()
            //   .HasOne(p => p.User)
            //   .WithMany(b => b.Assets).HasForeignKey(p => p.UserId);

            //modelBuilder.Entity<User>()
            //    .HasMany(b => b.Assets).WithOne(x => x.User);

            //modelBuilder.Entity<User>().HasMany(s => s.Assets).WithOne(x => x.User).HasForeignKey(e => e.UserId);

            //modelBuilder.Entity<Asset>().HasOne(s => s.User);

            //modelBuilder.Entity<User>()
            //.HasOne(s => s.Message)
            //.WithMany()
            //.HasForeignKey(e => e.Message_Id)
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //one-to-many 
        //    modelBuilder.Entity<Student>()
        //                .HasRequired<Standard>(s => s.Standard) // Student entity requires Standard 
        //                .WithMany(s => s.Students); // Standard entity includes many Students entities

        //}
    }
}