using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Entities;
namespace Todo.Data.Maps
{
    public class UserTaskMap : KeyedEntityBaseMap<UserTask, int>
    {
        public UserTaskMap(EntityTypeBuilder<UserTask> builder) : base(builder)
        {
            // Table & Column Mappings
            builder.ToTable("UserTask");
            // Column Mappings
            builder.Property(t => t.DateCreated).HasColumnName("DateCreated");
            builder.Property(t => t.DateModified).HasColumnName("DateModified");
            builder.Property(t => t.Title).HasColumnName("Title").HasMaxLength(550);
            builder.Property(t => t.Description).HasColumnName("Description").HasMaxLength(550);
        }
    }
}