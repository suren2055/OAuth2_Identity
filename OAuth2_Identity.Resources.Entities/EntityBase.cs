using System.ComponentModel.DataAnnotations;

namespace OAuth2_Identity.Resources.Entities;

public class EntityBase
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    [MaxLength(100)]
    public string CreatedBy { get; set; }
    [MaxLength(100)]
    public string? UpdatedBy { get; set; }
        
}