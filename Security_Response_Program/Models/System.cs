using System;
using System.Collections.Generic;

namespace Security_Response_Program.Models;

public partial class System
{
    public long SystemId { get; set; }

    public string? SystemName { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
