using System;
using System.ComponentModel.DataAnnotations;

namespace SPA.Models
{
    public class ModFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Date { get; set; }
        public string User { get; set; }
        public int ModBlobId { get; set; }
        public ModBlob ModBlob { get; set; }
    }
}
