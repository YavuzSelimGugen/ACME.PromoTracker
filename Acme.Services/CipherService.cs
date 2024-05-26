namespace Acme.Services
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class CipherService
    {
         private readonly string secretKey = "your-very-secure-secret-key"; // Replace with your actual secret key
        private static readonly RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private readonly byte[] secretKeyBytes;

        public CipherService()
        {
            this.secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        }

        public string GenerateCode(int length, string charSet)
        {
            var charArray = charSet.ToCharArray();
            var bufferLength = length/2;

            // Generate a random code of specified length
            var buffer = new byte[bufferLength];
            rngCsp.GetBytes(buffer);

            var code = new StringBuilder(length);
            foreach (var byteValue in buffer)
            {
                code.Append(charArray[byteValue % charArray.Length]);
            }

            // Generate HMAC signature for the code
            var signature = GenerateSignature(code.ToString(), charSet);

            // Truncate or pad the signature to fit within the length constraints
            var signatureLength = Math.Min(4, length / 2); // Use a 4-character signature
            signature = signature.Substring(0, signatureLength);

            // Append the signature to the code
            return code.Append(signature).ToString();
        }

        public bool ValidateCode(string code, int length, string charSet)
        {
            // The expected length includes the original code and the signature
            var bufferLength = length / 2;
            int totalLength = bufferLength + Math.Min(4, length / 2);

            if (code.Length != totalLength) return false;

            // Extract the original code and signature
            string originalCode = code.Substring(0, bufferLength);
            string signature = code.Substring(bufferLength);

            // Validate the character set
            var charArray = charSet.ToCharArray();
            if (!originalCode.All(c => charArray.Contains(c)) || !signature.All(c => charArray.Contains(c)))
                return false;

            // Validate the signature
            string expectedSignature = GenerateSignature(originalCode, charSet);
            expectedSignature = expectedSignature.Substring(0, signature.Length); // Adjust to expected signature length
            return signature.Equals(expectedSignature, StringComparison.Ordinal);
        }

        private string GenerateSignature(string code, string charSet)
        {
            using (var hmac = new HMACSHA256(secretKeyBytes))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(code));
                return ConvertToBase(hash, charSet);
            }
        }

        private string ConvertToBase(byte[] hash, string charSet)
        {
            var charArray = charSet.ToCharArray();
            var baseLength = charArray.Length;
            var result = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                int index = hash[i] % baseLength;
                result.Append(charArray[index]);
            }

            return result.ToString();
        }
    }
}