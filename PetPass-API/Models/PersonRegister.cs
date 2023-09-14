using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("Person_Register")]
public partial class PersonRegister
{
    [Key]
    [Column("person_RegisterID")]
    public int PersonRegisterId { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("user_PersonID")]
    public int UserPersonId { get; set; }

    [Column("personID")]
    public int PersonId { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PersonRegisters")]
    public virtual Person Person { get; set; } = null!;

    [ForeignKey("UserPersonId")]
    [InverseProperty("PersonRegisters")]
    public virtual User UserPerson { get; set; } = null!;
}
