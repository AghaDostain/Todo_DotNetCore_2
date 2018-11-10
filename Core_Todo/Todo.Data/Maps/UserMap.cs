using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Entities;
namespace Todo.Data.Maps
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(e => e.UserName).HasColumnName("UserName");
            builder.Property(e => e.AccessFailedCount).HasColumnName("AccessFailedCount");
            builder.Property(e => e.DateOfBirth).HasColumnName("DateOfBirth");
            builder.Property(e => e.FirstName).HasColumnName("FirstName");
            builder.Property(e => e.LastName).HasColumnName("LastName");
            builder.Property(e => e.Email).HasColumnName("Email");
            builder.Property(e => e.EmailConfirmed).HasColumnName("EmailConfirmed");
            builder.Property(e => e.LockoutEnabled).HasColumnName("LockoutEnabled");
            builder.Property(e => e.PasswordHash).HasColumnName("PasswordHash");
            builder.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber");
            builder.Property(e => e.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
            builder.Property(e => e.SecurityStamp).HasColumnName("SecurityStamp");
            builder.Property(e => e.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
            builder.Property(e => e.LastLogin).HasColumnName("LastLogin");
            builder.Property(e => e.ProfilePicUrl).HasColumnName("ProfilePicUrl");
            builder.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(e => e.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(e => e.NormalizedEmail).HasColumnName("NormalizedEmail");
            builder.Property(e => e.NormalizedUserName).HasColumnName("NormalizedUserName");
            builder.Property(e => e.LockoutEnd).HasColumnName("LockoutEnd");
            builder.Property(e => e.ConcurrencyStamp).HasColumnName("ConcurrencyStamp");
            builder.Property(e => e.RoleId).HasColumnName("RoleId");
        }
    }
}