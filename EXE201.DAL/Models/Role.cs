﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Models;

[Table("Role")]
public partial class Role
{
    [Key]
    public int RoleID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string RoleName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Description { get; set; }

    [ForeignKey("RoleID")]
    [InverseProperty("Roles")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}