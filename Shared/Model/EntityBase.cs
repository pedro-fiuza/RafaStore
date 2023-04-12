using System.Globalization;

namespace RafaStore.Shared.Model
{
    public class EntityBase
    {
        public int? Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.ToLocalTime();
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.ToLocalTime();
    }
}
