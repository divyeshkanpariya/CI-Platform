using System;
using System.Collections.Generic;

namespace CI_Platform.Models.Models;

public partial class ContactU
{
    public long ContactId { get; set; }

    public long UserId { get; set; }

    public string Subject { get; set; } = null!;

    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
