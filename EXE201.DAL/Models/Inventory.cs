﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Models;

[Table("Inventory")]
public partial class Inventory
{
    [Key]
    [Column("InventoryID")]
    public int InventoryId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    public int? QuantityAvailable { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Inventories")]
    public virtual Product Product { get; set; }
}