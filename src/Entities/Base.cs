namespace Clean.Solutions.Vertical.Entities;

public class Base
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}
