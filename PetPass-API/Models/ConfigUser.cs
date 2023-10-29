using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("Config_User")]
public partial class ConfigUser
{
    [Key]
    [Column("userImageID")]
    public int UserImageId { get; set; }

    [Column("pathImages")]
    [Unicode(false)]
    public string PathImages { get; set; } = null!;

    [Column("personID")]
    public int PersonId { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("ConfigUsers")]
    public virtual User? Person { get; set; } = null!;
}
