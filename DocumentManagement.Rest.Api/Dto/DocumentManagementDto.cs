using System.ComponentModel.DataAnnotations;

namespace DocumentManagement.Rest.Api.Dto
{
    public class DocumentManagementDto
    {

        [Required(ErrorMessage = "User name is required.")]
        [MaxLength(50, ErrorMessage = "User name cannot be longer than 50 characters.")]
        public string InsertUser { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Document url is required.")]
        [FileExtensions(Extensions = ".pdf", ErrorMessage = "Document url must be a pdf file.")]
        [MaxLength(255, ErrorMessage = "Document url cannot be longer than 255 characters.")]
        public string DocumentUrl { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
        [MaxLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public string DocumentTitle { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Document ref. number field is required.")]
        public string DocumentRefNumber { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Description is required or is allowed empty string.")]
        [MaxLength(255, ErrorMessage = "Description cannot be longer than 255 characters.")]
        public string DocumentDescription { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Case ref. number field is required.")]
        [MaxLength(18, ErrorMessage = "Case ref. number cannot be longer than 18 characters")]
        public string CaseRefNo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Created date is required.")]
        [RegularExpression(@"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2,3})$", ErrorMessage = "Date must be in format yyyy-mm-dd HH:MM:SS.")]
        public string DocumentCreatedDate { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Comment is required or is allowed empty string.")]
        [MaxLength(500, ErrorMessage = "Comment cannot be longer than 500 characters")]
        public string DocumentComment { get; set; }

        [Required(ErrorMessage = "Template id is required.")]
        [Range(short.MinValue, short.MaxValue, ErrorMessage = "User id value must be in the interval from {1} to {2}.")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "User id number is not valid.")]
        public short TemplateID { get; set; }

        [Required(ErrorMessage = "Entity pib is required.")]
        [MaxLength(9, ErrorMessage = "Pib cannot be longer than 9 characters")]
        public string EntityPIB { get; set; }

        [Required(ErrorMessage = "Entity name is required.")]
        [MaxLength(200, ErrorMessage = "Comment cannot be longer than 200 characters")]
        public string EntityName { get; set; }

        public bool? IsSend { get; set; }
        public List<ParticipantDto> Participants { get; set; }
    }

    public class ParticipantDto
    {

        [Required(ErrorMessage = "Participant full name is required.")]
        [MinLength(100, ErrorMessage = "Participant full name cannot be longer than 100 characters.")]
        public string AccountDisplayName { get; set; }

        [Required(ErrorMessage = "Participant email address is required.")]
        [MinLength(50, ErrorMessage = "Participant email address cannot be longer than 50 characters.")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Participant telephone number is required.")]
        [MinLength(50, ErrorMessage = "Participant telephone number cannot be longer than 50 characters.")]
        public string AccountPhoneNo { get; set; }

        [Range(short.MinValue, short.MaxValue, ErrorMessage = "Approval order value must be in the interval from {1} to {2}.")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Approval order number is not valid.")]
        public byte? ApprovalOrder { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Sign type is required.")]
        [MaxLength(50, ErrorMessage = "Sign type cannot be longer than 50 characters")]
        public string SignType { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Sign role is required or is allowed empty string.")]
        [MaxLength(50, ErrorMessage = "Sign role cannot be longer than 50 characters")]
        public string SignRole { get; set; }
    }
}
