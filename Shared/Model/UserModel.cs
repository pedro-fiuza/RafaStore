namespace RafaStore.Shared.Model
{
    public class UserModel : EntityBase
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; }
    }
}
