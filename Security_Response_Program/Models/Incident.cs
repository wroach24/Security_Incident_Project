using System;
using System.Collections.Generic;

namespace Security_Response_Program.Models;

public partial class Incident
{
    public long IncidentId { get; set; }

    public byte[]? Date { get; set; }

    public string? Severity { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public long? AffectedSystem { get; set; }

    public virtual System? AffectedSystemNavigation { get; set; }
}
