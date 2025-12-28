namespace IngetinGwAPI.CustomModels
{
    public class Reminder_input
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public long Remind_at { get; set; }

        public long Event_at { get; set; }
    }
}
