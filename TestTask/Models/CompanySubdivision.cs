using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace TestTaskDomain.Models
{
    public class CompanySubdivision
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [XmlElement(ElementName = "id")]
        public int Id { get; set; }
        [XmlElement(ElementName = "companySubdivisionId")]
        public int? CompanySubdivisionId { get; set; }
        [XmlIgnore]
        public bool CompanySubdivisionIdSpecified { get { return CompanySubdivisionId != null; } }
        [XmlElement(ElementName = "name")]
        public string? Name { get; set; }
        [XmlIgnore]
        public virtual CompanySubdivision? ParentSubdivision { get; set; }
        [XmlIgnore]
        public virtual ICollection<CompanySubdivision>? ChildSubdivisions { get; set; }


    }
}
