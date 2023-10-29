using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("Config_Pet")]
public partial class ConfigPet
{
    [Key]
    [Column("petImageID")]
    public int PetImageId { get; set; }

    [Column("pathImages")]
    [Unicode(false)]
    public string PathImages { get; set; } = null!;

    [Column("petID")]
    public int PetId { get; set; }

    [ForeignKey("PetId")]
    [InverseProperty("ConfigPets")]
    public virtual Pet? Pet { get; set; } = null!;
}
