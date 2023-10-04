namespace CloudNotes.Api.Models
{
    public interface IEntity
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
