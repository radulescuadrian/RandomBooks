namespace RandomBooks.Shared.DatabaseModels;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] Password { get; set; }
    public string Role { get; set; } = "Customer";

    public CustomerDetails CustomerDetails { get; set; }
    public EmployeeDetails EmployeeDetails { get; set; }

    public bool Deactivated { get; set; } = false;
}