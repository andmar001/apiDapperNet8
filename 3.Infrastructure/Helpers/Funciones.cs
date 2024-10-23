namespace _3.Infrastructure.Helpers
{
    public class Funciones
    {
        public static string GetReToken(string token)
        {
            DateTime hoy = DateTime.Now;
            string[] splitReTkn = token.Split('~');
            return string.Concat(splitReTkn[0].AsSpan(0, hoy.Month), splitReTkn[1]);
        }

        public static string GetPhrase(string reTkn)
        {
            DateTime hoy = DateTime.Now;
            return reTkn.Split('~')[0].Substring(hoy.Month);
        }
    }
}