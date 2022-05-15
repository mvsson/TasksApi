using System.Collections.Concurrent;

namespace TasksApi.Application.Utils
{
    public static class EnumExtension
    {
        private static readonly ConcurrentDictionary<Enum, string> EnumStringValues = new();
        public static string ToStringCached(this Enum @enum)
        {
            if (EnumStringValues.TryGetValue(@enum, out var textValue))
            {
                return textValue;
            }
            else
            {
                textValue = @enum.ToString();
                EnumStringValues[@enum] = textValue;
                return textValue;
            }
        }
    }
}
