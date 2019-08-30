using Microsoft.EntityFrameworkCore;

namespace People_Dictionary.Classes
{
    public class PeopleContext : DbContext
    {
        private const string ConnectionString = "server=database-1.cbyec5csehre.us-west-2.rds.amazonaws.com;port=3306;database=People_Search;Uid=pieter;Pwd=33monkeys";

        public DbSet<People> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<People>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.StreetAddress).IsRequired();
                entity.Property(e => e.StreetAddressAdditional);
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.ZipCode).IsRequired();
                entity.Property(e => e.DateOfBirth).IsRequired();
                entity.Property(e => e.Interests).IsRequired();
                entity.Property(e => e.AvatarUrl).IsRequired();
                entity.Property(e => e.CreatedDate).IsRequired();
                entity.Property(e => e.UpdatedDate).IsRequired();
                entity.Property(e => e.Active).IsRequired();
            });
        }
    }
}
