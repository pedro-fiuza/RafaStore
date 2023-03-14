using RafaStore.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RafaStore.Shared.ViewModel
{
    public class GeneratePdfViewModel
    {
        public CustomerModel Customer { get; set; }
        public NoteViewModel Note { get; set; }
    }
}
