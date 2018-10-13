using ByTheCakeApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByTheCakeApp.Data.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .IsUnicode();

            builder
                .Property(x => x.Username)
                .IsRequired()
                .IsUnicode(false);

            builder
                .Property(x => x.Password)
                .IsRequired()
                .IsUnicode();

            builder
                .Property(x => x.DateOfRegistration)
                .HasDefaultValueSql("GETDATE()");

            builder
                .HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
