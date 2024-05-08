﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int UserID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string UserName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string FullName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Password { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Phone { get; set; }

    public int? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Image { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Status { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("User")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("User")]
    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [InverseProperty("PaymentUser")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("User")]
    public virtual ICollection<RentalOrder> RentalOrders { get; set; } = new List<RentalOrder>();

    [ForeignKey("UserID")]
    [InverseProperty("Users")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}