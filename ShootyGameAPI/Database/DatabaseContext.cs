using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database.Entities.Interfaces;

namespace ShootyGameAPI.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<UserWeapon> UserWeapons { get; set; }
        public DbSet<WeaponType> WeaponTypes { get; set; }

        public override int SaveChanges()
        {
            HandleDelete();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            HandleDelete();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void HandleDelete()
        {
            foreach (var entity in ChangeTracker.Entries<ISoftDelete>()
                .Where(x => x.State == EntityState.Deleted))
            {
                entity.State = EntityState.Modified;
                entity.CurrentValues["IsDeleted"] = true;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure global query filter for soft delete
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Score>().HasQueryFilter(x => !x.User.IsDeleted);
            modelBuilder.Entity<Friend>().HasQueryFilter(x => !x.User1.IsDeleted);
            modelBuilder.Entity<Friend>().HasQueryFilter(x => !x.User2.IsDeleted);
            modelBuilder.Entity<FriendRequest>().HasQueryFilter(x => !x.Requester.IsDeleted);
            modelBuilder.Entity<FriendRequest>().HasQueryFilter(x => !x.Receiver.IsDeleted);
            modelBuilder.Entity<UserWeapon>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserWeapon>().HasQueryFilter(x => !x.User.IsDeleted);
            modelBuilder.Entity<UserWeapon>().HasQueryFilter(x => !x.Weapon.IsDeleted);
            modelBuilder.Entity<Score>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Score>().HasQueryFilter(x => !x.User.IsDeleted);
            modelBuilder.Entity<Weapon>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Weapon>().HasQueryFilter(x => !x.WeaponType.IsDeleted);
            modelBuilder.Entity<WeaponType>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserName).HasColumnType("nvarchar(64)").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnType("nvarchar(200)").IsRequired();
                entity.Property(e => e.Email).HasColumnType("nvarchar(64)").IsRequired();
                entity.Property(e => e.PlayerTag).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.Money);
                entity.Property(e => e.Role);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasColumnType("bit");

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.PlayerTag).IsUnique();
            });

            modelBuilder.Entity<Weapon>(entity =>
            {
                entity.HasKey(e => e.WeaponId);
                entity.Property(e => e.WeaponTypeId).IsRequired();
                entity.Property(e => e.Name).HasColumnType("nvarchar(64)").IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.ReloadSpeed).IsRequired();
                entity.Property(e => e.MagSize).IsRequired();
                entity.Property(e => e.FireRate).IsRequired();
                entity.Property(e => e.FireMode).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasColumnType("bit");
            });

            modelBuilder.Entity<WeaponType>(entity =>
            {
                entity.HasKey(e => e.WeaponTypeId);
                entity.Property(e => e.Name).HasColumnType("nvarchar(64)").IsRequired();
                entity.Property(e => e.EquipmentSlot).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasColumnType("bit");
            });

            modelBuilder.Entity<UserWeapon>(entity =>
            {
                // define composite key
                entity.HasKey(e => new { e.UserId, e.WeaponId });

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.WeaponId).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasColumnType("bit");

                // define foreign keys
                entity.HasOne(e => e.User)
                    .WithMany(e => e.UserWeapons)
                    .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Weapon)
                    .WithMany(e => e.UserWeapons)
                    .HasForeignKey(e => e.WeaponId);
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                // define composite key
                entity.HasKey(e => new { e.User1Id, e.User2Id });

                entity.Property(e => e.User1Id).IsRequired();
                entity.Property(e => e.User2Id).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasColumnType("bit");

                // define foreign keys
                entity.HasOne(e => e.User1)
                    .WithMany()
                    .HasForeignKey(e => e.User1Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.User2)
                    .WithMany()
                    .HasForeignKey(e => e.User2Id)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<FriendRequest>(entity =>
            {
                entity.HasKey(e => e.FriendRequestId);
                entity.Property(e => e.RequesterId).IsRequired();
                entity.Property(e => e.ReceiverId).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.ResponseAt).HasColumnType("datetime");
                entity.Property(e => e.Status).IsRequired();

                // define foreign keys
                entity.HasOne(e => e.Requester)
                    .WithMany()
                    .HasForeignKey(e => e.RequesterId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Receiver)
                    .WithMany()
                    .HasForeignKey(e => e.ReceiverId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.HasKey(e => e.ScoreId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.ScoreValue).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasColumnType("bit");

                // define foreign key
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Scores)
                    .HasForeignKey(e => e.UserId);
            });
        }
    }
}
