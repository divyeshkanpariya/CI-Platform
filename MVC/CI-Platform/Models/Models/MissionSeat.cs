using System;
using System.Collections.Generic;

namespace CI_Platform.Models.Models;

public partial class MissionSeat
{
    public long MissionSeatId { get; set; }

    public long MissionId { get; set; }

    public byte Islimited { get; set; }

    public int? TotalSeats { get; set; }

    public int? SeatsFilled { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission Mission { get; set; } = null!;
}
