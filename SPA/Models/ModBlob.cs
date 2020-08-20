using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPA.Models
{
    public class ModBlob
    {
        public int Id {get; set;}
        public byte[] Blob { get; set; }
        public byte[] HashCode { get; set; }
        public List<ModFile> ModFiles { get; set; }
        
    }
}
