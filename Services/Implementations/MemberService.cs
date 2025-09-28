using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintcsApi.Services.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly AppDbContext _context;
        private readonly ILedgerService _ledgerService;

        public MemberService(AppDbContext context, ILedgerService ledgerService)
        {
            _context = context;
            _ledgerService = ledgerService;
        }

        // Create a new member
        public async Task<ApiResponse<bool>> CreateMemberAsync(int societyId, MemberCreateUpdateDto dto)
        {
            var society = await _context.Societies.FindAsync(societyId);
            if (society == null) return ApiResponse<bool>.ErrorResponse("Society not found");

            var member = new Member
            {
                SocietyId = societyId,
                Name = dto.Name,
                FHName = dto.FHName,
                Mobile = dto.Mobile,
                Email = dto.Email,
                Status = dto.Status,
                OfficeAddress = dto.OfficeAddress,
                City = dto.City,
                PhoneOffice = dto.PhoneOffice,
                Branch = dto.Branch,
                PhoneRes = dto.PhoneRes,
                Designation = dto.Designation,
                ResidenceAddress = dto.ResidenceAddress,
                DOB = dto.DOB,
                DOJSociety = dto.DOJSociety,
                DOR = dto.DOR,
                Nominee = dto.Nominee,
                NomineeRelation = dto.NomineeRelation,
                CdAmount = dto.CdAmount,
                Email2 = dto.Email2,
                Mobile2 = dto.Mobile2,
                Pincode = dto.Pincode,
                BankName = dto.BankName,
                AccountNumber = dto.AccountNumber,
                PayableAt = dto.PayableAt,
                Share = dto.Share,
                CreatedAt = DateTime.UtcNow
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            await _ledgerService.CreateDefaultLedgersForMemberAsync(member.Id);

            return ApiResponse<bool>.SuccessResponse(true, "Member created successfully");
        }

        // Get member by Id
        public async Task<ApiResponse<MemberDto>> GetMemberByIdAsync(int memberId)
        {
            var member = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId);
            if (member == null) return ApiResponse<MemberDto>.ErrorResponse("Member not found");

            return ApiResponse<MemberDto>.SuccessResponse(MapToDto(member), "Member retrieved successfully");
        }

        // Get all members of a society
        public async Task<ApiResponse<List<MemberDto>>> GetAllMembersBySocietyAsync(int societyId)
        {
            var members = await _context.Members.AsNoTracking()
                                .Where(m => m.SocietyId == societyId)
                                .ToListAsync();

            var dtos = members.Select(MapToDto).ToList();
            return ApiResponse<List<MemberDto>>.SuccessResponse(dtos, "Members retrieved successfully");
        }

        // Update member
        public async Task<ApiResponse<bool>> UpdateMemberAsync(int memberId, MemberUpdateDto dto)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null) 
                return ApiResponse<bool>.ErrorResponse("Member not found");

            // Only update fields that are not null
            if (dto.Name != null) member.Name = dto.Name;
            if (dto.FHName != null) member.FHName = dto.FHName;
            if (dto.Mobile != null) member.Mobile = dto.Mobile;
            if (dto.Email != null) member.Email = dto.Email;
            if (dto.Status != null) member.Status = dto.Status;
            if (dto.OfficeAddress != null) member.OfficeAddress = dto.OfficeAddress;
            if (dto.City != null) member.City = dto.City;
            if (dto.PhoneOffice != null) member.PhoneOffice = dto.PhoneOffice;
            if (dto.Branch != null) member.Branch = dto.Branch;
            if (dto.PhoneRes != null) member.PhoneRes = dto.PhoneRes;
            if (dto.Designation != null) member.Designation = dto.Designation;
            if (dto.ResidenceAddress != null) member.ResidenceAddress = dto.ResidenceAddress;
            if (dto.DOB.HasValue) member.DOB = dto.DOB.Value;
            if (dto.DOJSociety.HasValue) member.DOJSociety = dto.DOJSociety.Value;
            if (dto.DOR.HasValue) member.DOR = dto.DOR.Value;
            if (dto.Nominee != null) member.Nominee = dto.Nominee;
            if (dto.NomineeRelation != null) member.NomineeRelation = dto.NomineeRelation;
            if (dto.CdAmount.HasValue) member.CdAmount = dto.CdAmount.Value;
            if (dto.Email2 != null) member.Email2 = dto.Email2;
            if (dto.Mobile2 != null) member.Mobile2 = dto.Mobile2;
            if (dto.Pincode != null) member.Pincode = dto.Pincode;
            if (dto.BankName != null) member.BankName = dto.BankName;
            if (dto.AccountNumber != null) member.AccountNumber = dto.AccountNumber;
            if (dto.PayableAt != null) member.PayableAt = dto.PayableAt;
            if (dto.Share.HasValue) member.Share = dto.Share.Value;

            member.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Member updated successfully");
        }


        // Delete member
        public async Task<ApiResponse<bool>> DeleteMemberAsync(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
                return ApiResponse<bool>.ErrorResponse("Member not found");

            // 1️⃣ Delete related ledger accounts first
            var ledgers = _context.LedgerAccounts.Where(l => l.MemberId == memberId);
            _context.LedgerAccounts.RemoveRange(ledgers);

            // 2️⃣ Delete the member
            _context.Members.Remove(member);

            // 3️⃣ Save changes
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Member and related ledger accounts deleted successfully");
        }


        // Map Member to MemberDto
        private MemberDto MapToDto(Member m)
        {
            return new MemberDto
            {
                Id = m.Id,
                SocietyId = m.SocietyId,
                Name = m.Name,
                FHName = m.FHName,
                Mobile = m.Mobile,
                Email = m.Email,
                Status = m.Status,
                OfficeAddress = m.OfficeAddress,
                City = m.City,
                PhoneOffice = m.PhoneOffice,
                Branch = m.Branch,
                PhoneRes = m.PhoneRes,
                Designation = m.Designation,
                ResidenceAddress = m.ResidenceAddress,
                DOB = m.DOB,
                DOJSociety = m.DOJSociety,
                DOR = m.DOR,
                Nominee = m.Nominee,
                NomineeRelation = m.NomineeRelation,
                CdAmount = m.CdAmount,
                Email2 = m.Email2,
                Mobile2 = m.Mobile2,
                Pincode = m.Pincode,
                BankName = m.BankName,
                AccountNumber = m.AccountNumber,
                PayableAt = m.PayableAt,
                Share = m.Share,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            };
        }
    }
}
