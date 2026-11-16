using System.Security.Cryptography;

namespace Seguridad
{
    // Formato: PBKDF2-SHA256$<iteraciones>$<sal base64>$<hash base64>
    /// <summary>
    /// Genera y verifica hashes de contraseñas con PBKDF2-SHA256 y sal aleatoria. Se usa al crear usuarios y al iniciar sesión.
    /// </summary>
    public static class PasswordHasher
    {
        private const string Algoritmo = "PBKDF2-SHA256";
        private const int Iteraciones = 100_000;
        private const int TamañoSal = 16;
        private const int TamañoHash = 32;

        /// <summary>
        /// Genera un hash seguro de la contraseña con una sal aleatoria y el formato algoritmo$iteraciones$sal$hash. Se usa al guardar o cambiar una contraseña.
        /// </summary>
        public static string Crear(string contraseña)
        {
            if (string.IsNullOrEmpty(contraseña))
                throw new ArgumentException("La contraseña no puede estar vacía.");

            byte[] sal = RandomNumberGenerator.GetBytes(TamañoSal);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                contraseña, sal, Iteraciones, HashAlgorithmName.SHA256, TamañoHash);

            return $"{Algoritmo}${Iteraciones}${Convert.ToBase64String(sal)}${Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Compara una contraseña con un hash guardado en tiempo constante. Se usa al iniciar sesión para validar las credenciales.
        /// </summary>
        public static bool Verificar(string contraseña, string almacenado)
        {
            if (string.IsNullOrEmpty(contraseña) || string.IsNullOrEmpty(almacenado))
                return false;

            string[] partes = almacenado.Split('$');
            if (partes.Length != 4 || partes[0] != Algoritmo)
                return false;

            if (!int.TryParse(partes[1], out int iteraciones) || iteraciones <= 0)
                return false;

            try
            {
                byte[] sal = Convert.FromBase64String(partes[2]);
                byte[] esperado = Convert.FromBase64String(partes[3]);

                byte[] calculado = Rfc2898DeriveBytes.Pbkdf2(
                    contraseña, sal, iteraciones, HashAlgorithmName.SHA256, esperado.Length);

                return CryptographicOperations.FixedTimeEquals(calculado, esperado);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
