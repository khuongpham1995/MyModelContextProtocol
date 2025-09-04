namespace McpServer.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }

    public void Create(string name, string email, string? phone = null, string? address = null, string? avatar = null)
    {
        Avatar = avatar;
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
        CreatedAt = DateTime.Now;
    }

    public void Update(string name, string email, string? phone = null, string? address = null, string? avatar = null)
    {
        Avatar = avatar;
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
        UpdatedAt = DateTime.Now;
    }
}