using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IRunes.Data.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Username)
                .IsRequired()
                .IsUnicode(false);

            builder
                .Property(x => x.Password)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .IsRequired()
                .IsUnicode(false);

            builder
                .HasMany(x => x.Albums)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
