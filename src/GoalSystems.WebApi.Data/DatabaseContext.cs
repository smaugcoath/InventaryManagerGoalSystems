namespace GoalSystems.WebApi.Data
{
    using GoalSystems.WebApi.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public partial class DatabaseContext : DbContext
    {
        private const string DEFAULT_SCHEMA = "App";

        public DatabaseContext():base()
        {
        }


        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ItemEntity> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => base.OnConfiguring(optionsBuilder);


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemEntity>(entity =>
            {
                entity.ToTable("Item", DEFAULT_SCHEMA)
                .HasKey(x => x.Id);

                entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.ExpirationDate)
                .HasColumnType("datetime2(0)")
                .IsRequired();

                entity.Property(x => x.Type)
                  .IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
