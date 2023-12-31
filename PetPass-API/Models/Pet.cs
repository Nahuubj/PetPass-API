﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetPass_API.Models;

[Table("Pet")]
public partial class Pet
{
    [Key]
    [Column("petID")]
    public int PetId { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("specie")]
    [StringLength(25)]
    [Unicode(false)]
    public string Specie { get; set; } = null!;

    [Column("breed")]
    [StringLength(50)]
    [Unicode(false)]
    public string Breed { get; set; } = null!;

    /// <summary>
    /// F : Female
    /// M : Male
    /// </summary>
    [Column("gender")]
    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [Column("birthDate", TypeName = "date")]
    public DateTime BirthDate { get; set; }

    [Column("special_feature")]
    [StringLength(60)]
    [Unicode(false)]
    public string SpecialFeature { get; set; } = null!;

    /// <summary>
    /// 0 : Inactive
    /// 1: Active
    /// </summary>
    [Column("state")]
    public short State { get; set; }

    [Column("personID")]
    public int PersonId { get; set; }

    [InverseProperty("Pet")]
    public virtual ICollection<ConfigPet>? ConfigPets { get; set; } = new List<ConfigPet>();

    [ForeignKey("PersonId")]
    [InverseProperty("Pets")]
    public virtual Person? Person { get; set; } = null!;

    [InverseProperty("Pet")]
    public virtual ICollection<PetRegister>? PetRegisters { get; set; } = new List<PetRegister>();

    public Pet(int petId, string name, string specie, string breed, string gender, DateTime birthDate, string specialFeature, short state, int personId)
    {
        PetId = petId;
        Name = name;
        Specie = specie;
        Breed = breed;
        Gender = gender;
        BirthDate = birthDate;
        SpecialFeature = specialFeature;
        State = state;
        PersonId = personId;
    }
}
