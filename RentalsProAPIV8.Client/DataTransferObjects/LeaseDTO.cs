using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Client.DataTransferObjects
{
    [Serializable]
    public record LeaseDTO
    {
        public int? ID { get; set; }
        public int? PropertyID { get; set; }
        public int? UnitID { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateFormatted => StartDate.ToShortDateString();
        public DateTime EndDate { get; set; }
        public string EndDateFormatted => EndDate.ToShortDateString();
        public decimal RentAmount { get; set; }
        public decimal? SecurityDeposit { get; set; }
        public decimal? PetDeposit { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
        public List<UserDTO>? Tenants { get; set; } = [];

        public LeaseDTO() { }

        public LeaseDTO(int? Id)
        {
            ID = Id;
        }

        public LeaseDTO(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
