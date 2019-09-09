using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.SystemData
{
    public class ResourceConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResourceConfigurationId { get; set; }

        public string ResourceDomain { get; set; }

        public ResourceDataFormat ResourceDataFormat { get; set; }
    }

    public enum ResourceDataFormat
    {
        Html,
        CSV
    }
}
