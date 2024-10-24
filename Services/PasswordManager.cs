namespace EasyAccounts.Services
{
    public class PasswordManager
    {       public string HashPassword(string password)
            {
                // Generate a salt and hash the password
                return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
            }

            public bool VerifyPassword(string enteredPassword, string storedPasswordHash)
            {
                // Compare enteredPassword with storedPasswordHash
                return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
            }
    }
}
