using System;

namespace Tests
{
    class HardcodedPasswordTest
    {
        static void Main(string[] args)
        {
            // ⚠️ Vulnerable: Hardcoded password
            string password = "mySecretPassword123";

            // ✅ Safe: Retrieved from a secure source (just for contrast)
            string securePassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        }
    }
}
