using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RafaStore.Shared.Dto
{
    public class CreateResumeDto
    {
        public string Bed { get; set; } = string.Empty;
        public DateTime? AdmissionDate { get; set; } 
        public string Surgeries { get; set; } = string.Empty;
        public string MainDiagnosis { get; set; } = string.Empty;
        public string Complications { get; set; } = string.Empty;
        public string ProposalOfTheDay { get; set; } = string.Empty;
        public bool Editing { get; set; } = false;
        public bool IsNew { get; set; } = false;
        public int PatientId { get; set; }
    }
}
