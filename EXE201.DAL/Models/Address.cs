﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Models;

[Table("Address")]
public partial class Address
{
    [Key]
    public int AddressID { get; set; }

    public int? UserID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Street { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string City { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string State { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string PostalCode { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Country { get; set; }

    [ForeignKey("UserID")]
    [InverseProperty("Addresses")]
    public virtual User User { get; set; }
}