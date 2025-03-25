using Domain.Enums;

namespace Domain.Entities
{
    public class EntityRole
    {
        public Guid Id { get; set; }

        public RoleType Name { get; set; }
    }
}
