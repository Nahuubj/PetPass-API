using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("Zone")]
public partial class Zone
{
    [Key]
    [Column("zoneID")]
    public byte ZoneId { get; set; }

    [Column("name")]
    [StringLength(60)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Zone")]
    public virtual ICollection<Patrol> Patrols { get; set; } = new List<Patrol>();
}
