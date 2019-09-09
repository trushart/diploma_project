using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.SystemData
{
    public class ResourceProcessingStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResourceProcessingStatusId { get; set; }

        [ForeignKey("ResourceConfiguration")]
        public int ResourceConfigurationId { get; set; }

        public string TargetUrl { get; set; }

        public ProcessingStatus Status { get; set; }

        public int ProcessedMatches { get; set; }

        public virtual ResourceConfiguration ResourceConfiguration { get; set; }
    }

    public enum ProcessingStatus
    {
        Start,
        Processing,
        Finished
    }

}
