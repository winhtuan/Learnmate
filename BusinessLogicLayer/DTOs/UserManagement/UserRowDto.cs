namespace BusinessLogicLayer.DTOs.UserManagement;

public class UserRowDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string LastLogin { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;

    public string Initials => string.Concat(Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => w[0].ToString().ToUpper()));
    public string InitialsBg => Role == "Admin" ? "#e0e7ff" : Role == "Instructor" ? "#f3e8ff" : "#dbeafe";
    public string InitialsColor => Role == "Admin" ? "#4338ca" : Role == "Instructor" ? "#7c3aed" : "#1d4ed8";
}
