using System;
using Light.SharedCore.Entities;

namespace DamageReportsApi.DatabaseAccess.Model;

public abstract class Person : GuidEntity
{
    public Guid DamageReportId { get; set; }
    public DamageReport DamageReport { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}