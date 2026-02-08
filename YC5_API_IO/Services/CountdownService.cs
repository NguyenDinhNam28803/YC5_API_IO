using Microsoft.EntityFrameworkCore;
using YC5_API_IO.Data;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Services
{
    public class CountdownService : ICountdownInterface
    {
        private readonly ApplicationDbContext _context;

        public CountdownService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountDown>> GetCountdownsAsync(string userId)
        {
            return await _context.CountDowns
                                 .Where(cd => cd.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<CountDown?> GetCountdownByIdAsync(string userId, string countdownId)
        {
            return await _context.CountDowns
                                 .Where(cd => cd.UserId == userId && cd.CountDownId == countdownId)
                                 .FirstOrDefaultAsync();
        }

        public async Task<CountDown> CreateCountdownAsync(string userId, CreateCountdownDto createCountdownDto)
        {
            var countdown = new CountDown
            {
                UserId = userId,
                CountDownName = createCountdownDto.CountDownName,
                CountDownDescription = createCountdownDto.CountDownDescription,
                TargetDate = createCountdownDto.TargetDate,
                CountDownStatus = CountDownStatus.Active // Default status
            };

            _context.CountDowns.Add(countdown);
            await _context.SaveChangesAsync();
            return countdown;
        }

        public async Task<CountDown?> UpdateCountdownAsync(string userId, string countdownId, UpdateCountdownDto updateCountdownDto)
        {
            var countdown = await _context.CountDowns
                                         .Where(cd => cd.UserId == userId && cd.CountDownId == countdownId)
                                         .FirstOrDefaultAsync();

            if (countdown == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateCountdownDto.CountDownName))
            {
                countdown.CountDownName = updateCountdownDto.CountDownName;
            }
            if (!string.IsNullOrEmpty(updateCountdownDto.CountDownDescription))
            {
                countdown.CountDownDescription = updateCountdownDto.CountDownDescription;
            }
            if (updateCountdownDto.TargetDate.HasValue)
            {
                countdown.TargetDate = updateCountdownDto.TargetDate.Value;
            }
            if (!string.IsNullOrEmpty(updateCountdownDto.CountDownStatus) && Enum.TryParse(updateCountdownDto.CountDownStatus, true, out CountDownStatus newStatus))
            {
                countdown.CountDownStatus = newStatus;
            }
            
            _context.CountDowns.Update(countdown);
            await _context.SaveChangesAsync();
            return countdown;
        }

        public async Task<bool> DeleteCountdownAsync(string userId, string countdownId)
        {
            var countdown = await _context.CountDowns
                                         .Where(cd => cd.UserId == userId && cd.CountDownId == countdownId)
                                         .FirstOrDefaultAsync();

            if (countdown == null)
            {
                return false;
            }

            _context.CountDowns.Remove(countdown);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
