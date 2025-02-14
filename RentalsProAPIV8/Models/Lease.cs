﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RentalsProAPIV8.Models;

public partial class Lease
{
    public int ID { get; set; }

    public int PropertyID { get; set; }

    public int? UnitID { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal RentAmount { get; set; }

    public decimal? SecurityDeposit { get; set; }

    public decimal? PetDeposit { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool Active { get; set; }

    public virtual Property Property { get; set; }

    public virtual Unit Unit { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}