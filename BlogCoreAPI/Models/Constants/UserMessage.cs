namespace BlogCoreAPI.Models.Constants
{
    internal static class UserMessage
    {
        public static string CannotBeNull(string propertyName)
        {
            return propertyName + " cannot be null.";
        }

        public static string CannotBeNullOrEmpty(string propertyName)
        {
            return propertyName + " cannot be null or empty.";
        }

        public static string CannotExceed(string propertyName, int characterNumber)
        {
            return propertyName + " cannot exceed " + characterNumber + " characters.";
        }
    }
}
