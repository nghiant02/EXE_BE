﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Models;

[Table("Product")]
public partial class Product
{
    [Key]
    [Column("ProductID")]
    public int ProductId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ProductName { get; set; }

    [Column(TypeName = "text")]
    public string ProductDescription { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ProductImage { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string ProductStatus { get; set; }

    public double? ProductPrice { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ProductSize { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ProductColor { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ProductColorImage { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Product")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [InverseProperty("Product")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    [InverseProperty("Product")]
    public virtual ICollection<RentalOrderDetail> RentalOrderDetails { get; set; } = new List<RentalOrderDetail>();
}