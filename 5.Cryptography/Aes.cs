using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace _5.Cryptography
{
    public static class Aes
    {
        public static string Phrase { get; set; }

        private static int tagSizeBytes = 16; // 128 bit encryption / 8 bit = 16 bytes
        private static int ivSizeBytes = 12; // 12 bytes iv

        public static string Encrypt(string plain)
        {
            // Validations
            if (Phrase == null || Phrase.Length <= 0)
                throw new ArgumentException("phrase");

            // Generate key
            byte[] key = System.Text.Encoding.UTF8.GetBytes(Phrase);
            AesGcm _aes = new(key);

            // Get bytes of plaintext string
            byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

            // Get parameter sizes
            int nonceSize = AesGcm.NonceByteSizes.MaxSize;
            int tagSize = AesGcm.TagByteSizes.MaxSize;
            int cipherSize = plainBytes.Length;

            // We write everything into one big array for easier encoding
            int encryptedDataLength = 4 + nonceSize + 4 + tagSize + cipherSize;
            Span<byte> encryptedData = encryptedDataLength < 1024 ? stackalloc byte[encryptedDataLength] : new byte[encryptedDataLength].AsSpan();

            // Copy parameters
            BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(0, 4), nonceSize);
            BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4), tagSize);
            var nonce = encryptedData.Slice(4, nonceSize);
            var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
            var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

            // Generate secure nonce
            RandomNumberGenerator.Fill(nonce);

            // Encrypt
            _aes.Encrypt(nonce, plainBytes.AsSpan(), cipherBytes, tag);

            // Encode for transmission
            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(string cipher)
        {
            // Validations
            if (Phrase == null || Phrase.Length <= 0)
                throw new ArgumentException("phrase");

            // Generate key
            byte[] key = System.Text.Encoding.UTF8.GetBytes(Phrase);
            AesGcm _aes = new(key);

            // Decode
            Span<byte> encryptedData = Convert.FromBase64String(cipher).AsSpan();

            // Extract parameter sizes
            int nonceSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(0, 4));
            int tagSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4));
            int cipherSize = encryptedData.Length - 4 - nonceSize - 4 - tagSize;

            // Extract parameters
            var nonce = encryptedData.Slice(4, nonceSize);
            var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
            var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

            // Decrypt
            Span<byte> plainBytes = cipherSize < 1024 ? stackalloc byte[cipherSize] : new byte[cipherSize];
            _aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

            // Convert plain bytes back into string
            return Encoding.UTF8.GetString(plainBytes);
        }

        public static string EncryptToSendFrontEnd(string value, string kBase64)
        {
            try
            {
                var k = Convert.FromBase64String(kBase64).AsSpan();
                string vBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
                var plainBytes = Convert.FromBase64String(vBase64).AsSpan();

                int cipherSize = plainBytes.Length;

                byte[] cipherBytes = new byte[cipherSize];

                int encryptedDataLength = ivSizeBytes + cipherSize;

                Span<byte> encryptedData = encryptedDataLength < 1024
                                         ? stackalloc byte[encryptedDataLength]
                                         : new byte[encryptedDataLength].AsSpan();

                byte[] tag = new byte[tagSizeBytes];

                Random rnd = new Random();
                Byte[] iv = new Byte[ivSizeBytes];
                rnd.NextBytes(iv);

                using var aes = new AesGcm(k);
                aes.Encrypt(iv, plainBytes, cipherBytes, tag);

                byte[] rv = new byte[iv.Length + plainBytes.Length + tag.Length];
                System.Buffer.BlockCopy(iv.ToArray(), 0, rv, 0, iv.ToArray().Length);
                System.Buffer.BlockCopy(cipherBytes, 0, rv, iv.Length, cipherSize);
                System.Buffer.BlockCopy(tag, 0, rv, cipherSize + iv.Length, tag.Length);

                return Convert.ToBase64String(rv);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string DecryptFromFrontEnd(string K, string cipherText)
        {
            try
            {
                // convert from base64 to raw bytes spans
                var encryptedData = Convert.FromBase64String(cipherText).AsSpan();
                var kk = Convert.FromBase64String(K).AsSpan();

                // ciphertext size is whole data - iv - tag
                var cipherSize = encryptedData.Length - tagSizeBytes - ivSizeBytes;

                // extract iv (nonce) 12 bytes prefix
                var iv = encryptedData.Slice(0, ivSizeBytes);

                // followed by the real ciphertext
                var cipherBytes = encryptedData.Slice(ivSizeBytes, cipherSize);

                // followed by the tag (trailer)
                var tagStart = ivSizeBytes + cipherSize;
                var tag = encryptedData.Slice(tagStart);

                // now that we have all the parts, the decryption
                Span<byte> plainBytes = cipherSize < 1024
                    ? stackalloc byte[cipherSize]
                    : new byte[cipherSize];

                using var aes = new AesGcm(kk);
                aes.Decrypt(iv, cipherBytes, tag, plainBytes);

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}