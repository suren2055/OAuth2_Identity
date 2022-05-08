using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OAuth2_Identity.Entities;

public class User : EntityBase
{
    [Key] public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Hash { get; set; }
    public string Salt { get; set; }
    
    public Guid UserSecret { get; set; }
    [DefaultValue("false")] public bool Terminated { get; set; }
    [DefaultValue("false")] public bool TwoFactorEnabled { get; set; }
    
}