using System.ComponentModel;

namespace RentalsProAPIV8.Client.DataTransferObjects
{
    [Serializable]
    public record CompanyDTO
    {
        [DisplayName("#")]
        public int? ID { get; set; }

        [DisplayName("Name")]
        public string? Name { get; set; }

        [DisplayName("Address")]
        public AddressDTO? Address { get; set; }

        [DisplayName("Phone")]
        public string? Phone { get; set; }

        [DisplayName("Email")]
        public string? Email { get; set; }

        [DisplayName("Create Date")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Update Date")]
        public DateTime? UpdatedDate { get; set; }

        [DisplayName("Active")]
        public bool? Active { get; set; }

        public CompanyDTO() { }

        public CompanyDTO(int Id)
        {
            ID = Id;
        }
    }
}
