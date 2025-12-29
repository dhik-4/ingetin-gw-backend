using IngetinGwAPI.CustomModels;
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
                .OrderBy(r => r.Remind_at)
                .ToListAsync(cancellationToken);

            if (getReminder is not null && getReminder.Count > 0)
            {
                _result = getReminder;
            }

            return _result;
        }

        public async Task<Reminder> CreateReminder(Reminder_input input, int UserId, CancellationToken cancellationToken)
        {
            try
            {
                Reminder data = new Reminder()
                {
                    Title = input.Title,
                    Description = input.Description,
                    UserId = UserId,
                    Event_at = input.Event_at,
                    Remind_at = input.Remind_at
                };

                _context.Reminders.Add(data);
                await _context.SaveChangesAsync(cancellationToken);

                _context.ChangeTracker.Clear();

                //int newId = data.Id;

                return data;
            }
            catch (Exception ex)
            {
            }

            return new Reminder();
        }

        public async Task<Reminder> ViewReminder(int id, CancellationToken cancellationToken)
        {
            try
            {
                var getReminder = await _context.Reminders
                    .Where(r => r.Id == id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (getReminder is not null)
                {
                    return getReminder;
                }
            }
            catch (Exception ex)
            {
            }

            return new Reminder();
        }

        public async Task<Reminder> EditReminder(int id, Reminder_input data, CancellationToken cancellationToken)
        {
            int Result = 0;
            try
            {
                Result = await _context.Reminders.Where(t => t.Id == id)
                    .ExecuteUpdateAsync(s =>
                        s.SetProperty(v => v.Title, data.Title)
                        .SetProperty(v => v.Description, data.Description)
                        .SetProperty(v => v.Remind_at, data.Remind_at)
                        .SetProperty(v => v.Event_at, data.Event_at), cancellationToken
                    );

                _context.ChangeTracker.Clear();

                if (Result >= 1)
                {
                    return new Reminder()
                    {
                        Event_at = data.Event_at,
                        Remind_at = data.Remind_at,
                        Description = data.Description,
                        Title = data.Title,
                        Id = id
                    };
                }
            }
            catch (Exception ex)
            {
            }

            return new Reminder();
        }

        public async Task<bool> DeleteReminder(int id, CancellationToken cancellationToken)
        {
            int Result = 0;
            try
            {
                Result = await _context.Reminders.Where(t => t.Id == id)
                    .ExecuteDeleteAsync(cancellationToken);

                _context.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
            }

            return Result >= 1;
        }
    }
}