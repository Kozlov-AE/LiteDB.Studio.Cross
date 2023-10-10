using DatabaseGenerator.Models.LiteDbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseGenerator.Models;

public partial class Employee
{
    public long EmployeeId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public byte[]? BirthDate { get; set; }

    public string? Photo { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public EmployeeLDb MapToLDb() {
        return new EmployeeLDb() {
            EmployeeId = this.EmployeeId,
            BirthDate =
                DateTime.Parse(
                    Encoding.Default.GetString(BirthDate?.Where(x => x != 0).ToArray() ?? Array.Empty<byte>())),
            LastName = this.LastName,
            FirstName = this.FirstName,
            Notes = this.Notes,
            Photo = this.Photo
        };
    }
}
