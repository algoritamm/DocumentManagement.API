﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Rest.Api.Entities;

[Table("ACCOUNT", Schema = "adm")]
[Index("UserName", Name = "AK_ACCOUNT_UserName", IsUnique = true)]
[Index("UserCode", Name = "FK_ACCOUNT_Code")]
[Index("LastName", "FirstName", Name = "FK_ACCOUNT_RoleTypeID")]
[Index("EmailAddress", Name = "IX_ACCOUNT_EmailAddress")]
public partial class Account
{
    [Key]
    [Column("AccountID")]
    public short AccountId { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string UserName { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string EmailAddress { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string TelephoneNo { get; set; }

    [Required]
    [StringLength(255)]
    public string DescriptionText { get; set; }

    [Required]
    [StringLength(150)]
    public string JobPosition { get; set; }

    [Required]
    [StringLength(255)]
    public string Department { get; set; }

    [Required]
    [StringLength(13)]
    [Unicode(false)]
    public string UserCode { get; set; }

    [Column("IsAD")]
    public bool IsAd { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsToReceiveNotification { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string DefaultUrl { get; set; }

    public byte[] AccountSign { get; set; }

    [Column("LanguageCountryID")]
    public byte LanguageCountryId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLoginDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PreviousLoginDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UserPassword { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string UserPasswordSalt { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastPasswordChangedDate { get; set; }

    public bool? IsMailSent { get; set; }

    public bool? IsForcePwdChange { get; set; }

    public byte? LoginAttempt { get; set; }

    public bool? IsLocked { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LockedDate { get; set; }

    [StringLength(50)]
    public string Addressing { get; set; }

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

    [InverseProperty("Account")]
    public virtual ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();

    [InverseProperty("AccountLocated")]
    public virtual ICollection<Case> CaseAccountLocateds { get; set; } = new List<Case>();

    [InverseProperty("Account")]
    public virtual ICollection<Case> CaseAccounts { get; set; } = new List<Case>();

    [InverseProperty("Account")]
    public virtual ICollection<CaseWorkflow> CaseWorkflows { get; set; } = new List<CaseWorkflow>();

    [InverseProperty("Account")]
    public virtual ICollection<DocumentParticipantProcess> DocumentParticipantProcesses { get; set; } = new List<DocumentParticipantProcess>();

    [InverseProperty("Account")]
    public virtual ICollection<DocumentWorkflow> DocumentWorkflows { get; set; } = new List<DocumentWorkflow>();

    [InverseProperty("Account")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}