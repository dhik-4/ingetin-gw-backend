using IngetinGwAPI.CustomModels;
using IngetinGwAPI.Models;

namespace IngetinGwAPI.Interfaces
{
    public interface IReminderRepository
    {
        Task<List<Reminder>> ListReminders(int limit, CancellationToken cancellationToken);
        Task<Reminder> CreateReminder(Reminder_input input, int UserId, CancellationToken cancellationToken);
        Task<Reminder> ViewReminder(int id, CancellationToken cancellationToken);
        Task<Reminder> EditReminder(int id, Reminder_input data, CancellationToken cancellationToken);
        Task<bool> DeleteReminder(int id, CancellationToken cancellationToken);

        Task<bool> UpdateEmailSentReminder(Reminder data, CancellationToken cancellationToken);
    }
}
