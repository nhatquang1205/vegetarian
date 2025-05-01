namespace vegetarian.Commons.Schemas
{
    public class SelectData<T>
    {
        public T Id { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
    }
}