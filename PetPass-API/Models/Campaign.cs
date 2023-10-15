using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("Campaign")]
public partial class Campaign
{
    [Key]
    [Column("campaignID")]
    public int CampaignId { get; set; }

    [Column("name")]
    [StringLength(80)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("campaignDate", TypeName = "date")]
    public DateTime CampaignDate { get; set; }

    /// <summary>
    /// 0 : Inactive
    /// 1: Active
    /// </summary>
    [Column("state")]
    public byte State { get; set; }

    [InverseProperty("Campaign")]
    public virtual ICollection<Patrol>? Patrols { get; set; } = new List<Patrol>();

    [InverseProperty("Campaign")]
    public virtual ICollection<PetVaccine>? PetVaccines { get; set; } = new List<PetVaccine>();
}
