﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Models;

[Table("Image")]
public partial class Image
{
    [Key]
    [Column("ImageID")]
    public int ImageId { get; set; }

    [Column("ImageURL")]
    [StringLength(255)]
    public string ImageUrl { get; set; }

    [InverseProperty("Image")]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}