using System.Text.RegularExpressions;

namespace SistemasAnaliticosWEB.Helpers
{
    public static class InputSanitizer
    {
        // Sanitiza texto general
        public static string Clean(string? input, int maxLength = 5000)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // 1. Normalizar
            input = input.Trim();

            // 2. Prevenir Email Header Injection
            input = input.Replace("\r", "").Replace("\n", "");

            // 3. Quitar caracteres de control
            input = Regex.Replace(input, "[\x00-\x1F\x7F]", "");

            // 4. Limitar longitud
            if (input.Length > maxLength)
                input = input.Substring(0, maxLength);

            return input;
        }

        // Valida email seguro
        public static string CleanEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            email = Clean(email, 200);

            // Validación formal
            if (!Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                return string.Empty;
            }

            return email;
        }
    }
}
