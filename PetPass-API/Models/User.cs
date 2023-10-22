using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("User")]
public partial class User
{
    [Key]
    [Column("personID")]
    public int PersonId { get; set; }

    [Column("username")]
    [StringLength(30)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("userpassword")]
    [Unicode(false)]
    public string Userpassword { get; set; } = null!;

    /// <summary>
    /// A: Admin
    /// B: Brigadier
    /// O: Owner
    /// </summary>
    [Column("rol")]
    [StringLength(1)]
    [Unicode(false)]
    public string Rol { get; set; } = null!;

    /// <summary>
    /// 1: TRUE
    /// 0: FALSE
    /// </summary>
    [Column("firstSessionLogin")]
    [StringLength(1)]
    [Unicode(false)]
    public string FirstSessionLogin { get; set; } = null!;

    [Column("code_recovery")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CodeRecovery { get; set; }

    [InverseProperty("Person")]
    public virtual ICollection<Patrol>? Patrols { get; set; } = new List<Patrol>();

    [ForeignKey("PersonId")]
    [InverseProperty("User")]
    public virtual Person? Person { get; set; } = null!;

    [InverseProperty("UserPerson")]
    public virtual ICollection<PersonRegister>? PersonRegisters { get; set; } = new List<PersonRegister>();

    [InverseProperty("UserPerson")]
    public virtual ICollection<PetRegister>? PetRegisters { get; set; } = new List<PetRegister>();
}
