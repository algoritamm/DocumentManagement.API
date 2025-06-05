using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Rest.Api.Entities
{
    [Table("DOCUMENT_PARTICIPANT_PROCESS", Schema = "algo")]
    [Index("DocumentId", Name = "IX_DOCUMENT_VERSION_DocumentID")]
    public class DocumentVersion
    {
        [Key]
        [Column("DocumentVersionID")]
        public int DocumentVersionID { get; set; }
        [Column("DocumentID")]
        public int DocumentId { get; set; }
        [StringLength(255)]
        public string DocumentUrl { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string InsertUser { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime InsertDate { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string UpdateUser { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
        [ForeignKey("DocumentId")]
        [InverseProperty("DocumentVersions")]
        public virtual Document Document { get; set; }
    }
}
