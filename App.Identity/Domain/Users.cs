namespace App.Identity.Domain
{
    public class Users
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName{ get; set; }
        public string  PasswordHash { get; set; }

    }
}
