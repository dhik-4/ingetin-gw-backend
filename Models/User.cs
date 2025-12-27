using System;
using System.Collections.Generic;

namespace IngetinGwAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public byte? IsActive { get; set; }

    public string Password { get; set; } = null!;
}
