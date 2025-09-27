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
        public async Task<ApiResponse<bool>> UpdateMemberAsync(int memberId, MemberCreateUpdateDto dto)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null) return ApiResponse<bool>.ErrorResponse("Member not found");

            member.Name = dto.Name;
            member.FHName = dto.FHName;
            member.Mobile = dto.Mobile;
            member.Email = dto.Email;
            member.Status = dto.Status;
            member.OfficeAddress = dto.OfficeAddress;
            member.City = dto.City;
            member.PhoneOffice = dto.PhoneOffice;
            member.Branch = dto.Branch;
            member.PhoneRes = dto.PhoneRes;
            member.Designation = dto.Designation;
            member.ResidenceAddress = dto.ResidenceAddress;
            member.DOB = dto.DOB;
            member.DOJSociety = dto.DOJSociety;
            member.DOR = dto.DOR;
            member.Nominee = dto.Nominee;
            member.NomineeRelation = dto.NomineeRelation;
            member.CdAmount = dto.CdAmount;
            member.Email2 = dto.Email2;
            member.Mobile2 = dto.Mobile2;
            member.Pincode = dto.Pincode;
            member.BankName = dto.BankName;
            member.AccountNumber = dto.AccountNumber;
            member.PayableAt = dto.PayableAt;
            member.Share = dto.Share;
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
