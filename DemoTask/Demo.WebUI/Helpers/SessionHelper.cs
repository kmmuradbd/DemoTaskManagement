using Newtonsoft.Json;

namespace Demo.WebUI.Helpers
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static string GetFromJson(this ISession session, string key)
        {
            return session.GetString(key) ?? string.Empty;
        }
    }
}
