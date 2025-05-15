namespace Clean.Solutions.Vertical.Contracts
{
    public class GetTodoResponse
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOnUtc { get; set; }
    }
}
