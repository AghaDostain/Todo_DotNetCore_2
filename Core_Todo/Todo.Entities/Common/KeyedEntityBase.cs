namespace Todo.Entities
{
    public abstract class KeyedEntityBase<TValue>
    {
        public TValue Id { get; set; }
    }
}