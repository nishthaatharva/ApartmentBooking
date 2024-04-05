using System.ComponentModel;

namespace ApartmentBooking.Application.Extensions
{
    public static class CommonFunction
    {
        public static string GetEnumDisplayName(Enum status)
        {
            var fieldInfo = status.GetType().GetField(status.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

            return attribute != null ? attribute.Description : status.ToString();
        }

        public static string ConvertDateToStringForDisplay(DateTime date)
        {
            return date.ToString("MMM dd, yyyy");
        }
    }
}
