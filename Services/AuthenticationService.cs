using EasyAccounts.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EasyAccounts.Services
{
    public class AuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordManager _passwordManager;

        public AuthenticationService(IConfiguration configuration,PasswordManager passwordManager)
        {
            _configuration = configuration;
            _passwordManager = passwordManager;
        }

        public void RegisterUser(string username, string password)
        {
            // Hash the password before storing it in the database
            string hashedPassword = _passwordManager.HashPassword(password);

            // Save the username and hashedPassword to the database
            // (Assuming you have a method to save user details in your database)
            SaveUserToDatabase(username, hashedPassword);
        }

        public bool ValidateUserCredentials(string enteredUsername, string enteredPassword, string storedPasswordHash)
        {
            // Validate user credentials during login
            // Check if the username exists and the password is correct
            if (UserExists(enteredUsername) && _passwordManager.VerifyPassword(enteredPassword, storedPasswordHash))
            {
                // User credentials are valid
                return true;
            }

            // User credentials are invalid
            return false;
        }

        // Other authentication-related methods...

        private bool UserExists(string username)
        {
            // Implement logic to check if the user with the given username exists in the database
            // Return true if the user exists, false otherwise
            // (Assuming you have a method to check user existence in your database)
            return DoesUserExistInDatabase(username);
        }

        private void SaveUserToDatabase(string username, string hashedPassword)
        {
            // Implement logic to save the user details (username and hashedPassword) to the database
            // (Assuming you have a method to save user details in your database)
            SaveUserDetailsToDatabase(username, hashedPassword);
        }

        public bool DoesUserExistInDatabase(string username)
        {
            // Placeholder implementation for checking user existence
            // Replace this with your actual logic to check if the user exists in the database
            // You might want to query your database here and return true if the user exists, false otherwise
            // Example: return _userRepository.DoesUserExist(username);
            return false;
        }

        public void SaveUserDetailsToDatabase(string username, string hashedPassword)
        {
            // Placeholder implementation for saving user details to the database
            // Replace this with your actual logic to save user details
            // Example: _userRepository.SaveUserDetails(username, hashedPassword);
            // Ensure that you're saving the username and hashedPassword to your database
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                //new Claim(ClaimTypes.Role, user.Role),
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
