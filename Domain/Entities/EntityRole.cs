using Domain.Constants;

namespace Domain.Entities
{
    public class EntityRole
    {
        public Guid Id { get; init; }

        public required string Name { get; init; }
    }
}
