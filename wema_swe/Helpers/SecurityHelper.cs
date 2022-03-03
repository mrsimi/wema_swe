using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace wema_swe.Helpers
{
    public static class SecurityHelper
    {
        public static string GetHash(string input, string salt)
        {
            HashAlgorithm hashAlgorithm = SHA256.Create();

            string saltAndInput = String.Concat(input,salt);
            var byteComputedHashResult = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndInput));

            var hashResult = new StringBuilder();

            for(int i=0; i <byteComputedHashResult.Length; i++)
            {
                hashResult.Append(byteComputedHashResult[i].ToString("x2"));
            }

            return hashResult.ToString();

        }

        public static bool VerifyHash(string input, string salt, string storedHash)
        {
            var hashAlgorithm = SHA256.Create();
            string hashOfInput = GetHash(input, salt);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return  comparer.Compare(storedHash, hashOfInput) == 0;
        }
        public static string GetSalt()
        {
            string salt = string.Empty;
            using (var random = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[32];
                random.GetNonZeroBytes(bytes);
                salt = Convert.ToBase64String(bytes);
            }

            return salt;
        }

        public static string GenerateOtp()
        {
            return "12345";
        }

        public static string GenerateVerificationReference()
        {
            return "WM-12345";
        }


        public static bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
          
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one lower case letter";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one upper case letter";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one numeric value";
                return false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one special case characters";
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
