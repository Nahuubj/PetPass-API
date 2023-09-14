using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("PetVaccine")]
public partial class PetVaccine
{
    [Key]
    [Column("petVaccineID")]
    public int PetVaccineId { get; set; }

    [Column("petID")]
    public int PetId { get; set; }

    [Column("vaccineDate", TypeName = "date")]
    public DateTime VaccineDate { get; set; }

    [Column("vaccineType")]
    [StringLength(50)]
    [Unicode(false)]
    public string VaccineType { get; set; } = null!;

    [Column("vetName")]
    [StringLength(80)]
    [Unicode(false)]
    public string VetName { get; set; } = null!;

    [Column("campaignID")]
    public int CampaignId { get; set; }

    [ForeignKey("CampaignId")]
    [InverseProperty("PetVaccines")]
    public virtual Campaign Campaign { get; set; } = null!;

    [ForeignKey("PetId")]
    [InverseProperty("PetVaccines")]
    public virtual Pet Pet { get; set; } = null!;
}
