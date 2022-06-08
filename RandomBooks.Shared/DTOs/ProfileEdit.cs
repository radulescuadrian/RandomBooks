using RandomBooks.Shared.DatabaseModels;

namespace RandomBooks.Shared.DTOs;

public class ProfileEdit
{
    public CustomerDetails User { get; set; } = new();
    public string NewImage { get; set; } = string.Empty;
}
