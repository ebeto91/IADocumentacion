using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto
{
    public class Catalog
    {
        public string Code { get; set; }
        public string Collection { get; set; }
        public string Description { get; set; }
        public string DisplayLabel { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public string Ref4 { get; set; }
        public string RefDescriptions { get; set; }
        public bool Enabled { get; set; }
    }
}
