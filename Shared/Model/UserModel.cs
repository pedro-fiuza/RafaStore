namespace RafaStore.Shared.Model
{
    public class UserModel : EntityBase
    {
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
