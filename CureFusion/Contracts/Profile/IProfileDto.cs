namespace CureFusion.Contracts.Profile;

public interface IProfileDto
{
    string Id { get; set; }
    string FullName { get; set; }
    string Email { get; set; }
}