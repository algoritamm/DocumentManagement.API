﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Rest.Api.Entities
{
    [Table("ENTITY", Schema = "ent")]
    [Index(nameof(EntityTypeId), Name = "FK_ENTITY_EntityTypeID")]
    [Index(nameof(EntityName), Name = "IX_ENTITY")]
    public partial class Entity
    {
        public Entity()
        {
            CaseEntities = new HashSet<CaseEntity>();
        }

        [Key]
        [Column("EntityID")]
        public int EntityId { get; set; }
        [Required]
        [StringLength(200)]
        public string EntityName { get; set; }
        [Column("EntityTypeID")]
        public byte EntityTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string InsertUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime InsertDate { get; set; }
        [StringLength(50)]
        public string UpdateUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey(nameof(EntityTypeId))]
        [InverseProperty("Entities")]
        public virtual EntityType EntityType { get; set; }
        [InverseProperty("Entity")]
        public virtual EntityIndividual EntityIndividual { get; set; }
        [InverseProperty("Entity")]
        public virtual EntityLegal EntityLegal { get; set; }
        [InverseProperty(nameof(CaseEntity.Entity))]
        public virtual ICollection<CaseEntity> CaseEntities { get; set; }
    }
}