using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IngetinGwAPI.Models;

public partial class Reminder
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long Remind_at { get; set; }

    public long? Event_at { get; set; }

    [JsonIgnore]
    public int? UserId { get; set; }

    [JsonIgnore]
    public byte? IsEmailSent { get; set; }
}
