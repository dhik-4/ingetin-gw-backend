using IngetinGwAPI.Interfaces;
using IngetinGwAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IngetinGwAPI.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        AppDbContext _context;

        public ReminderRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Reminder>> ListReminders(int limit, CancellationToken cancellationToken)
        {
            List<Reminder>? _result = new List<Reminder>();
            var getReminder = await _context.Reminders
                .Take(limit)
                .OrderBy(r => r.RemindAt)
                .ToListAsync(cancellationToken);

            if (getReminder is not null && getReminder.Count > 0)
            {
                _result = getReminder;
            }

            return _result;
        }
    }
}
