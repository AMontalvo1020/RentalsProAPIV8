﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RentalsProAPIV8.Models;

public partial class Property
{
    public int ID { get; set; }

    public int? OwnerID { get; set; }

    public int? CompanyID { get; set; }

    public int StatusID { get; set; }

    public int TypeID { get; set; }

    public int PaymentStatusID { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZipCode { get; set; }

    public decimal? Bedrooms { get; set; }

    public decimal? Bathrooms { get; set; }

    public int? Size { get; set; }

    public string Amenities { get; set; }

    public decimal? PurchasePrice { get; set; }

    public DateTime PurchaseDate { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual User Owner { get; set; }

    public virtual PaymentStatus PaymentStatus { get; set; }

    public virtual PropertyStatus Status { get; set; }

    public virtual PropertyType Type { get; set; }

    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}