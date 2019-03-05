namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }     
        public string UserName { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] PassSalt { get; set; }
    }
}