using System;
using System.ComponentModel.DataAnnotations;

namespace OAuth2_Identity.Entities;

public class Client : EntityBase
{
    [Key] public Guid ClientID { get; set; }

    public string ClientKey { get; set; }

    public string ClientName { get; set; }
}