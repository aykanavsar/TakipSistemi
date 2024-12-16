namespace TakipSistemi.Helpers
{
    public static class Normalizer
    {
        public static string NormalizeString(string input)
        {
            return input
                .Replace("ş", "s").Replace("Ş", "S")
                .Replace("ı", "i").Replace("İ", "I")
                .Replace("ç", "c").Replace("Ç", "C")
                .Replace("ö", "o").Replace("Ö", "O")
                .Replace("ü", "u").Replace("Ü", "U")
                .Replace("ğ", "g").Replace("Ğ", "G")
                .Replace(" ", "");
        }
    }
}
