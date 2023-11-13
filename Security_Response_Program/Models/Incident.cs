using System;
using System.Collections.Generic;

namespace Security_Response_Program.Models;

public partial class Incident
{
    public long IncidentId { get; set; }

    public byte[]? Date { get; set; }

    public string? IncidentType { get; set; }

    public string? Severity { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public string? IncidentLocation { get; set; }

    public long? AffectedSystemId { get; set; }

    public string? IncidentResponder { get; set; }

    public string? BreachDescription { get; set; }

    public string? DataCompromised { get; set; }

    public virtual System? AffectedSystem { get; set; }
}
