namespace TasksApi.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string typeName, ValueType key) : base($"Не найден экземпляр типа: {typeName}. Ключ: {key}") { }
    }
}
