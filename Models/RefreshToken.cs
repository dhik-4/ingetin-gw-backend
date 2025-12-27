using System;
using System.Collections.Generic;

namespace IngetinGwAPI.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int UsersId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public byte? IsRevoked { get; set; }
}
