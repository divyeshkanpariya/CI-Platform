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

    public DateTime CreatedTime { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public DateTime? DeletedTime { get; set; }

    public virtual Mission Mission { get; set; } = null!;
}
