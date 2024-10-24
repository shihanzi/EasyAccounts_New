namespace EasyAccounts.Dtos
{
    public class RegisterRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        //public List<string> Roles { get; set; }
        public List<int> Roles { get; set; }
    }
}
