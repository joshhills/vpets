using System.ComponentModel;
using System.Reflection;

namespace VPets.Extensions
{
    /// <summary>
    /// Inspired by
    /// <see href="https://www.freecodecamp.org/news/87b818123e28/">here</see>
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Map an enum to its description attribute using reflection.
        /// </summary>
        /// <typeparam name="TEnum">A generic enum type</typeparam>
        /// <param name="enum">An arbitrary enum value</param>
        /// <returns>A string describing the enum</returns>
        public static string ToDescriptionString<TEnum>(this TEnum @enum)
        {
            FieldInfo info = @enum.GetType().GetField(@enum.ToString());
            var attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes?[0].Description ?? @enum.ToString();
        }
    }
}