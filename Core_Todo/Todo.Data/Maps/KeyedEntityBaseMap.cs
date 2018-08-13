using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Entities;

namespace Todo.Data.Maps
{
    public class KeyedEntityBaseMap<TEntity, TId>
    where TEntity : KeyedEntityBase<TId>
    where TId : struct
    {
        public KeyedEntityBaseMap(EntityTypeBuilder<TEntity> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Properties
            builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();
        }
    }
}
