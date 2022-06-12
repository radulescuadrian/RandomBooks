namespace RandomBooks.Shared.DTOs;

public class EmployeeListResult
{
    public List<EmployeesResponse> Employees { get; set; }
    public int Page { get; set; } = 1;
    public int Pages { get; set; } = 0;
}
