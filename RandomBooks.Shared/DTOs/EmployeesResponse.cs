namespace RandomBooks.Shared.DTOs;

public class EmployeesResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int OrdersFulfilled { get; set; } = 0;
    public DateTime DateHired { get; set; } = DateTime.Now;
    public bool Deactivated { get; set; } = false;
}
