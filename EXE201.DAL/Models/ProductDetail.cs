﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Models;

[Table("ProductDetail")]
public partial class ProductDetail
{
    [Key]
    [Column("ProductDetailID")]
    public int ProductDetailId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column(TypeName = "ntext")]
    public string Description { get; set; }

    [Column(TypeName = "ntext")]
    public string AdditionalInformation { get; set; }

    [Column(TypeName = "ntext")]
    public string ShippingAndReturns { get; set; }

    [Column(TypeName = "ntext")]
    public string SizeChart { get; set; }

    [Column(TypeName = "ntext")]
    public string Reviews { get; set; }

    [Column(TypeName = "ntext")]
    public string Questions { get; set; }

    [Column(TypeName = "ntext")]
    public string VendorInfo { get; set; }

    [Column(TypeName = "ntext")]
    public string MoreProducts { get; set; }

    [Column(TypeName = "ntext")]
    public string ProductPolicies { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductDetails")]
    public virtual Product Product { get; set; }
}