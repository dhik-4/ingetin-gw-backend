using IngetinGwAPI.Models;

namespace IngetinGwAPI.Interfaces
{
    public interface IReminderRepository
    {
        Task<List<Reminder>> ListReminders(int limit, CancellationToken cancellationToken);
    }
}
