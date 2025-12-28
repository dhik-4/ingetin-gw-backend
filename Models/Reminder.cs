using System;
using System.Collections.Generic;

namespace IngetinGwAPI.Models;

public partial class Reminder
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long RemindAt { get; set; }

    public long? EventAt { get; set; }
}
