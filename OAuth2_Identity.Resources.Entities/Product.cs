

using System.ComponentModel.DataAnnotations;

namespace OAuth2_Identity.Resources.Entities;

public class Product:EntityBase
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
    public string Location { get; set; }
    
}