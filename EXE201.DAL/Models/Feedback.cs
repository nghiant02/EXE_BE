﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Models;

[Table("Feedback")]
public partial class Feedback
{
    [Key]
    [Column("FeedbackID")]
    public int FeedbackId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column(TypeName = "ntext")]
    public string FeedbackComment { get; set; }

    [StringLength(255)]
    public string FeedbackImage { get; set; }

    [StringLength(10)]
    public string FeedbackStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateGiven { get; set; }

    [StringLength(255)]
    public string FeedbackTitle { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Feedbacks")]
    public virtual Product Product { get; set; }

    [InverseProperty("Feedback")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    [ForeignKey("UserId")]
    [InverseProperty("Feedbacks")]
    public virtual User User { get; set; }
}