using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("Person")]
public partial class Person
{
    [Key]
    [Column("personID")]
    public int PersonId { get; set; }

    [Column("name")]
    [StringLength(60)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("firstName")]
    [StringLength(45)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("lastName")]
    [StringLength(45)]
    [Unicode(false)]
    public string? LastName { get; set; }

    [Column("ci")]
    [StringLength(30)]
    [Unicode(false)]
    public string Ci { get; set; } = null!;

    /// <summary>
    /// F : Female
    /// M : Male
    /// </summary>
    [Column("gender")]
    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [Column("address")]
    [StringLength(120)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [Column("phone")]
    public int Phone { get; set; }

    [Column("email")]
    [StringLength(80)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// 0 : Inactive
    /// 1: Active
    /// </summary>
    [Column("state")]
    public short State { get; set; }

    [InverseProperty("Person")]
    public virtual ICollection<PersonRegister> PersonRegisters { get; set; } = new List<PersonRegister>();

    [InverseProperty("Person")]
    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();

    [InverseProperty("Person")]
    public virtual User? User { get; set; }
}
