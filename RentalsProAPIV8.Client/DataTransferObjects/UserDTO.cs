using static RentalsProAPIV8.Client.Constants.Enums;

namespace RentalsProAPIV8.Client.DataTransferObjects
{
    [Serializable]
    public record UserDTO
    {
        public int? ID { get; set; }
        public int? CompanyID { get; set; }
        public CompanyDTO? Company { get; set; }
        public int? LeaseID { get; set; }
        public LeaseDTO? Lease { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int Role { get; set; }
        public DateOnly? Birthdate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; } = true;
        public bool? IsOwner { get; set; }
        public string RoleName => GetEnumDescription((UserRole)Role);
        public string? FullName => $"{FirstName} {LastName}";
    }
}
