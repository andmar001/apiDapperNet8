using _5.Cryptography;

namespace _3.Infrastructure.Security
{
    internal class Crypt
    {
        internal enum Algoritmo
        {
            Aes
        }

        internal static string Encripta(string Texto, Algoritmo Algoritmo, string Phrase)
        {
            if (Phrase.Length == 0)
                throw new Exception("Error de seguridad");

            switch (Algoritmo)
            {
                case Algoritmo.Aes:
                    Aes.Phrase = Phrase;
                    return Aes.Encrypt(Texto);
                default:
                    throw new Exception("Algoritmo no implementado");
            }
        }

        internal static string Desencripta(string Texto, Algoritmo Algoritmo, string Phrase)
        {
            if (Phrase.Length == 0)
                throw new Exception("Error de seguridad");

            switch (Algoritmo)
            {
                case Algoritmo.Aes:
                    Aes.Phrase = Phrase;
                    return Aes.Decrypt(Texto);
                default:
                    throw new Exception("Algoritmo no implementado");
            }
        }

        internal static string EncriptaFrontEnd(string Texto, Algoritmo Algoritmo, string Phrase)
        {
            if (Phrase.Length == 0)
                throw new Exception("Error de seguridad");

            switch (Algoritmo)
            {
                case Algoritmo.Aes:
                    Aes.Phrase = Phrase;
                    return Aes.EncryptToSendFrontEnd(Phrase, Texto);
                default:
                    throw new Exception("Algoritmo no implementado");
            }
        }

        internal static string DesencriptaFrontEnd(string Texto, Algoritmo Algoritmo, string Phrase)
        {
            if (Phrase.Length == 0)
                throw new Exception("Error de seguridad");

            switch (Algoritmo)
            {
                case Algoritmo.Aes:
                    Aes.Phrase = Phrase;
                    return Aes.DecryptFromFrontEnd(Phrase, Texto);
                default:
                    throw new Exception("Algoritmo no implementado");
            }
        }
    }
}
