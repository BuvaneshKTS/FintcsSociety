using System;
using System.Collections.Generic;
using System.Linq;

namespace FintcsApi.DTOs
{
    // Request DTO for creating demand
    public class DemandCreateDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int? Cd { get; set; }
        public int? BuildingFund { get; set; }
        public int SocietyId { get; set; } // added society reference
    }

    // Response DTO for returning demand per member
    public class DemandViewDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }

        public int CD { get; set; }
        public int OD { get; set; }
        public int Share { get; set; }

        // Loan-wise demand details (grouped by loan type)
        public List<LoanDemandDto> LoanDemands { get; set; } = new List<LoanDemandDto>();

        // Penal amounts if any
        public decimal PenalAmount { get; set; }
        public decimal PenalInterest { get; set; }

        // Total demand computed automatically
        public decimal TotalDemand 
            => CD + OD + Share + LoanDemands.Sum(l => l.PendingAmount + l.Installment + l.Interest) 
               + PenalAmount + PenalInterest;
    }

    // Loan type specific demand
    public class LoanDemandDto
    {
        public string LoanType { get; set; } // General Loan, Emergency Loan, etc.
        public decimal PendingAmount { get; set; }
        public decimal Installment { get; set; }
        public decimal Interest { get; set; }
    }
}
