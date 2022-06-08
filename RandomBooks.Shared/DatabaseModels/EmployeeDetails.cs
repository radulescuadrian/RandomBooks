namespace RandomBooks.Shared.DatabaseModels;

public class EmployeeDetails
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public DateTime HireDate { get; set; } = DateTime.Now;
    public Blob? Image { get; set; }
}
