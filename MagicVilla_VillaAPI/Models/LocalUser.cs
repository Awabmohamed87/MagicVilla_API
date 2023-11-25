namespace MagicVilla_VillaAPI.Models
{
    public class LocalUser
    {
        public int ID{ get; set; }
        public string Name { get; set; }    
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
