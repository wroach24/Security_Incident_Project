using System;
using System.Collections.Generic;

namespace Security_Response_Program.Models;

public partial class User
{
    public long UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? CreatedDate { get; set; } = null!;

}
