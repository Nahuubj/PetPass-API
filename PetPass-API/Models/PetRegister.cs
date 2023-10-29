using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("Pet_Register")]
public partial class PetRegister
{
    [Key]
    [Column("pet_RegisterID")]
    public int PetRegisterId { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("user_PersonID")]
    public int UserPersonId { get; set; }

    [Column("petID")]
    public int PetId { get; set; }

    [ForeignKey("PetId")]
    [InverseProperty("PetRegisters")]
    public virtual Pet? Pet { get; set; } = null!;

    [ForeignKey("UserPersonId")]
    [InverseProperty("PetRegisters")]
    public virtual User? UserPerson { get; set; } = null!;
}
