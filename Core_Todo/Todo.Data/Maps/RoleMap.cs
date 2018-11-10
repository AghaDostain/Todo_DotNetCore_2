using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Entities;
namespace Todo.Data.Maps
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.Property(e => e.Description).IsRequired().IsUnicode(false);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50).IsUnicode(false);
            //base.Configure(builder);
        }
    }
}