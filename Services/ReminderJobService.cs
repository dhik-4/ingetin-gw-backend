using IngetinGwAPI.Interfaces;

namespace IngetinGwAPI.Services
{
    public class ReminderJobService
    {
        private readonly IReminderRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public ReminderJobService(IReminderRepository repository, IUserRepository userRepository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public async Task SendReminderEmail(int reminderId, CancellationToken cancellationToken)
        {
            var reminder = await _repository.ViewReminder(reminderId, cancellationToken);

            if (reminder is null || reminder.UserId is null || reminder.IsEmailSent == 1)
                return;

            var _user = await _userRepository.GetUserById( reminder.UserId ?? 0);

            string mailTo = _user.Email;
            string subject = reminder.Title;
            string body = reminder.Description;

            await _emailService.SendMailpitAsync(
                mailTo,
                subject,
                body
            );

            reminder.IsEmailSent = 1;
            await _repository.UpdateEmailSentReminder(reminder, cancellationToken);
        }
    }
}