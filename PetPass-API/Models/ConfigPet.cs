using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Keyless]
[Table("Config_Pet")]
public partial class ConfigPet
{
    [Column("pathImages")]
    [StringLength(300)]
    [Unicode(false)]
    public string PathImages { get; set; } = null!;
}
