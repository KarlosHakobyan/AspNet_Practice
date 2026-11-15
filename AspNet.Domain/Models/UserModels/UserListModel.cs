namespace AspNet.Domain.Models.Users
{

    public class UserListModel
    {
        public string Id { get; set; } = default!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; } = default!;

        public IList<string> Roles { get; set; } = [];
    }
}