using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("Patrol")]
public partial class Patrol
{
    [Key]
    [Column("patrolID")]
    public int PatrolId { get; set; }

    [Column("patrolDate", TypeName = "datetime")]
    public DateTime PatrolDate { get; set; }

    [Column("personID")]
    public int PersonId { get; set; }

    [Column("zoneID")]
    public byte ZoneId { get; set; }

    [Column("campaignID")]
    public int CampaignId { get; set; }

    [ForeignKey("CampaignId")]
    [InverseProperty("Patrols")]
    public virtual Campaign? Campaign { get; set; } = null!;

    [ForeignKey("PersonId")]
    [InverseProperty("Patrols")]
    public virtual User? Person { get; set; } = null!;

    [ForeignKey("ZoneId")]
    [InverseProperty("Patrols")]
    public virtual Zone? Zone { get; set; } = null!;
}
