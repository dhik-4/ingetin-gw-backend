using System;
using System.Collections.Generic;

namespace IngetinGwAPI.Models;

public partial class Reminder
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long Remind_at { get; set; }

    public long? Event_at { get; set; }
}
