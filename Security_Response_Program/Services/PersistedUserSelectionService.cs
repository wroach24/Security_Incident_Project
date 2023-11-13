using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security_Response_Program.Models;

namespace Security_Response_Program.Services
{
    public class PersistedUserSelectionService
    {
        // global selected incident id
        public Incident? SelectedIncident{ get; set; }
        public PersistedUserSelectionService()
        {
        }
    }
}