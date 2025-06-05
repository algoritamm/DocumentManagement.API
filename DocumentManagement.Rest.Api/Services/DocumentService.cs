using DocumentManagement.Rest.Api.Dto;
using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Enums;
using DocumentManagement.Rest.Api.Exceptions;
using DocumentManagement.Rest.Api.Interfaces.Repositories;
using DocumentManagement.Rest.Api.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Transactions;


namespace DocumentManagement.Rest.Api.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<AppSettings> _settings;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(IUnitOfWork unitOfWork, IOptions<AppSettings> settings, ILogger<DocumentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _settings = settings;
        }
        public (bool, string) ValidateDocument(DocumentManagementDto document)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(document, null, null);
            bool isValid = Validator.TryValidateObject(document, validationContext, validationResults, true);
            var message = isValid ? ValidationResult.Success : validationResults.FirstOrDefault();
            return (isValid, message?.ErrorMessage ?? string.Empty);
        }
        public (bool, string) DocumentImport(DocumentManagementDto dataDto)
        {
            _logger.LogInformation($"{nameof(DocumentImport)} with param: {dataDto}.");
            string successMessage = string.Empty;
            try
            {
                Account user = _unitOfWork.AccountRepo.GetAccountByUsername(dataDto.InsertUser);
                if (user is null || user.AccountRoles is null)
                {
                    _logger.LogError($"There is an error with User roles or user name {dataDto.InsertUser}.");
                    return (false, $"There is an error with user roles or user name {dataDto.InsertUser}.");
                }

                var _document = _unitOfWork.DocumentRepo.GetDocumentByRefNo(dataDto.DocumentRefNumber.Trim());
                if (_document is not null)
                {
                    _logger.LogError($"Document with ref. number {dataDto.DocumentRefNumber} already exists.");
                    return (false, $"Document with ref. number {dataDto.DocumentRefNumber} already exists.");
                }
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    var documentId = CreateDocument(dataDto, user);

                    InsertDocumentWorkflow(documentId, user);

                    CreateParticipantsByDocument(documentId, dataDto.Participants, user, dataDto.TemplateID);

                    scope.Complete();
                    successMessage += $"Document with ref. number {dataDto.DocumentRefNumber} successfully created. Document id = {documentId}.{Environment.NewLine}";

                    _logger.LogInformation($"Successfully saved data. Document id = {documentId}");
                    return (true, successMessage);
                }

            }
            catch (RoleException ex)
            {
                _logger.LogError(ex, $"RoleException occurred while importing document with ref number {dataDto.DocumentRefNumber}.");
                return (false, ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.LogError(ex, $"DirectoryNotFoundException occurred while accessing the upload folder: {_settings.Value.UploadFolderPath}.");
                return (false, ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, $"FileNotFoundException occurred for file: {dataDto.DocumentUrl}.");
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred while importing document with ref number {dataDto.DocumentRefNumber}.");
                return (false, "An unexpected error occurred.");
            }

        }


        #region Private methods
        private int CreateDocument(DocumentManagementDto document, Account user)
        {
            int caseId = 0;
            string fileUrl = string.Empty;
            DateTime now = DateTime.Now;
            string fileName = document.DocumentUrl.Split('\\').Last();
            if (!File.Exists(Path.Combine(_settings.Value.UploadFolderPath, fileName)))
            {
                fileUrl = now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + Path.GetExtension(fileName);
                string filePath = Path.Combine(_settings.Value.UploadFolderPath, fileUrl);

                using (var inputStream = File.OpenRead(document.DocumentUrl))
                using (var outputStream = File.Create(filePath))
                {
                    inputStream.CopyTo(outputStream);
                }
            }
            else
            {
                fileUrl = $"{Path.GetFileNameWithoutExtension(document.DocumentUrl)}.pdf";
            }

            Case _case = _unitOfWork.CaseRepo.GetCaseByRefNo(document.CaseRefNo.Trim());

            if (_case is null)
            {
                caseId = CreateCase(document, user);
            }
            else
            {
                caseId = _case.CaseId;
            }

            var documentToInsert = new Document()
            {
                DocumentTypeId = (byte)DocumentTypeEnum.EksternaDatotekaZahtevSpecifikacija,
                DocumentStatusId = (byte)DocumentStatusEnum.KreiranDokument,
                AccountId = user.AccountId,
                TemplateId = document.TemplateID,
                CaseId = caseId,
                DocumentTitle = document.DocumentTitle.Trim(),
                DocumentDesription = document.DocumentDescription.Trim(),
                DocumentCreatedDate = DateTime.Parse(document.DocumentCreatedDate),
                DocumentRefNumber = document.DocumentRefNumber.Trim(),
                DocumentUrl = fileUrl,
                DocumentComment = document.DocumentComment.Trim(),
                IsMailSent = false,
                IsFileSent2Dms = false,
                IsConfidential = false,
                IsMustSign = true,
                IsValidForm = true,
                Is4Send = document?.IsSend ?? true,
                InsertUser = user.UserName,
                InsertDate = now,
                IsMerged = false
            };

            _unitOfWork.DocumentRepo.InsertDocument(documentToInsert);

            _unitOfWork.Complete();

            var documentVersion = new DocumentVersion()
            {
                DocumentId = documentToInsert.DocumentId,
                DocumentUrl = documentToInsert.DocumentUrl,
                InsertUser = user.UserName,
                InsertDate = now
            };
            _unitOfWork.DocumentRepo.InsertDocumentVersion(documentVersion);
            _unitOfWork.Complete();
            return documentToInsert.DocumentId;
        }

        private int CreateCase(DocumentManagementDto dto, Account user)
        {
            Case caseToInsert = new Case();
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                short[] roleIds = user.AccountRoles.Select(x => x.RoleId).ToArray();
                var bp = _unitOfWork.ServiceDefRepo.GetServiceDefWithBPByTemplateIdAndRoles(dto.TemplateID, roleIds).FirstOrDefault();
                if (bp is null)
                {
                    _logger.LogError($"There is no service definition for roles {roleIds} and template id {dto.TemplateID}");
                    return 0;
                }
                var template = _unitOfWork.TemplateRepo.GetTemplateById(dto.TemplateID);
                caseToInsert = new Case
                {

                    CaseCreatedDate = DateTime.Now,
                    CaseYear = DateTime.Now.Year.ToString(),
                    CaseRefNo = dto.CaseRefNo.Trim(),
                    CaseTitle = template.TemplateType.TemplateTypeNameLat,
                    CaseDescription = template.TemplateType.TemplateTypeDescription ?? string.Empty,
                    CaseResolutionDate = _settings.Value.ResolutionDateNumOfDays is null ? DateTime.MaxValue : (_settings.Value.ResolutionDateNumOfDays == 0 ? DateTime.MaxValue : DateTime.Now.AddDays((short)_settings.Value.ResolutionDateNumOfDays)),
                    CaseStatusId = (byte)CaseStatusEnum.KreiranPredmet,
                    CaseClassificationId = template.CaseClasificationId,
                    RoleId = bp.RoleId,
                    AccountId = user.AccountId,
                    RoleLocatedId = bp.RoleId,
                    AccountLocatedId = user.AccountId,
                    BussinesProcessStepId = bp.BusinessProcess.BusinessProcessSteps.FirstOrDefault()?.BusinessProcessStepId ?? 0,
                    CaseComment = string.Empty,
                    TemplateTypeId = template.TemplateTypeId,
                    InsertUser = user.UserName,
                    InsertDate = DateTime.Now
                };

                _unitOfWork.CaseRepo.InsertCase(caseToInsert);
                _unitOfWork.Complete();

                //UPSER ENTITY AND ENTITY LEGAL
                var entityId = UpsertEntity(dto);

                //INSERT CASE ENTITY
                _unitOfWork.CaseEntityRepo.InsertCaseEntity(new CaseEntity { CaseId = caseToInsert.CaseId, EntityId = entityId });

                // INSERT CASE_WORKFLOW
                var caseWorkflow = new CaseWorkflow
                {
                    CaseId = caseToInsert.CaseId,
                    CaseWorkFlowDateTime = DateTime.Now,
                    BussinesProcessStepId = caseToInsert.BussinesProcessStepId.Value,
                    CaseStatusId = (byte)CaseStatusEnum.KreiranPredmet,
                    RoleId = caseToInsert.RoleId,
                    AccountId = caseToInsert.AccountId,
                    CaseResolutionDate = caseToInsert.CaseResolutionDate,
                    CaseWorkflowComment = string.Empty,
                    InsertUser = user.UserName,
                    InsertDate = DateTime.Now
                };
                _unitOfWork.CaseWorkflowRepo.InsertCaseWorkflow(caseWorkflow);
                _unitOfWork.Complete();

                scope.Complete();
            }
            _logger.LogInformation($"Successfully created case. Case id = {caseToInsert.CaseId}");
            return caseToInsert.CaseId;

        }

        private void InsertDocumentWorkflow(int documentId, Account user)
        {
            Case _case = _unitOfWork.CaseRepo.GetCaseByDocumentId(documentId);
            short bpId;
            if (_case != null)
            {
                bpId = (short)_case.BussinesProcessStepId;
            }
            else
            {
                bpId = 0;
            }
            var workflow = new DocumentWorkflow
            {
                DocumentId = documentId,
                DocumentWorkFlowDateTime = DateTime.Now,
                BussinesProcessStepId = bpId,
                DocumentStatusId = (byte)DocumentStatusEnum.KreiranDokument,
                AccountId = user.AccountId,
                DocumentWorkflowComment = string.Empty,
                InsertUser = user.UserName,
                InsertDate = DateTime.Now,
                IsDeleted = false
            };

            _unitOfWork.DocumentWorkflowRepo.InsertDocumentWorkflow(workflow);
            _unitOfWork.Complete();

        }

        private void CreateParticipantsByDocument(int documentId, List<ParticipantDto> participants, Account user, short templateId)
        {
            var allRoles = _unitOfWork.RoleRepo.GetRoles().ToDictionary(x => x.RoleNameAd, x => x);
            IEnumerable<DocumentParticipantProcess> participientsToInsert = participants.Select(participant =>
            {
                if (!allRoles.TryGetValue(participant.SignRole.Trim(), out var signRole))
                {
                    throw new RoleException($"Role with name {participant.SignRole} does not exist.");
                }

                short accountId = UpsertParticipantAccount(participant, signRole);
                return new DocumentParticipantProcess
                {
                    DocumentId = documentId,
                    AccountId = accountId,
                    DefinitionDate = DateTime.Now,
                    ExpiredDate = _settings.Value.ResolutionDateNumOfDays is null ? DateTime.MaxValue : (_settings.Value.ResolutionDateNumOfDays == 0 ? DateTime.MaxValue : DateTime.Now.AddDays((short)_settings.Value.ResolutionDateNumOfDays)),
                    ApprovalOrder = participant.ApprovalOrder != null ? ((byte)participant.ApprovalOrder == 0 ? GetApprovalOrderFromDB(templateId, signRole.RoleId) : (byte)participant.ApprovalOrder) : GetApprovalOrderFromDB(templateId, signRole.RoleId),
                    SignType = participant.SignType ?? string.Empty,
                    CommentNote = string.Empty,
                    RoleId = signRole.RoleId,
                    InsertDate = DateTime.Now,
                    InsertUser = user.UserName
                };
            }).Where(p => p != null)
            .Cast<DocumentParticipantProcess>();

            _unitOfWork.DocumentParticipantProcessRepo.InsertDocumentParticipantsProcess(participientsToInsert);

            _unitOfWork.Complete();

        }
        private byte GetApprovalOrderFromDB(short templateId, short roleId)
        {
            var template = _unitOfWork.TemplateRepo.GetApprovalOrderByTemplateId(templateId, roleId);
            if (template is null)
            {
                return 0;
            }
            return template.ApprovalOrder;
        }
        private int UpsertEntity(DocumentManagementDto dto)
        {
            int entityID;

            var el = _unitOfWork.EntityLegalRepo.GetEntityLegal(dto.EntityPIB);
            if (el is null)
            {
                //INSERT ENTITY
                Entity entity = new Entity
                {
                    EntityName = dto.EntityName.Trim(),
                    EntityTypeId = (byte)EntityTypeEnum.LegalEntity,
                    InsertUser = "web_app",
                    InsertDate = DateTime.Now
                };

                _unitOfWork.EntityRepo.InsertEntity(entity);
                _unitOfWork.Complete();

                //INSERT ENTITY LEGAL
                EntityLegal entityLegal = new EntityLegal
                {
                    EntityId = entity.EntityId,
                    Pib = dto.EntityPIB.Trim(),
                    Name = dto.EntityName.Trim(),
                    EmailAddress = string.Empty,
                    TelephoneNo = string.Empty,
                    Address = string.Empty,
                    City = string.Empty,
                    Zip = string.Empty,
                    CustomerId = int.TryParse(dto.EntityPIB, out int res) ? res : 0,
                    InsertUser = "web_app",
                    InsertDate = DateTime.Now
                };

                _unitOfWork.EntityLegalRepo.InsertEntityLegal(entityLegal);
                _unitOfWork.Complete();
                entityID = entity.EntityId;
            }
            else
            {
                //UPDATE ENTITY
                Entity ent = _unitOfWork.EntityRepo.GetEntityById(el.EntityId);
                ent.EntityName = dto.EntityName.Trim();
                ent.UpdateUser = "web_app";
                ent.UpdateDate = DateTime.Now;
                //UPDATE ENTITY
                el.Name = dto.EntityName.Trim();
                el.UpdateUser = "web_app";
                el.UpdateDate = DateTime.Now;

                _unitOfWork.EntityRepo.UpdateEntity(ent);
                _unitOfWork.EntityLegalRepo.UpdateEntityLegal(el);
                _unitOfWork.Complete();
                entityID = ent.EntityId;
            }
            return entityID;
        }

        private short UpsertParticipantAccount(ParticipantDto participant, Role role)
        {
            short accountID;

            var acc = _unitOfWork.AccountRepo.GetAccountByUsername(participant.AccountEmail);
            if (acc is null)
            {
                //INSERT ACCOUNT
                Account account = new Account()
                {
                    UserName = participant.AccountEmail,
                    FirstName = participant.AccountDisplayName.Trim().Split(" ").First(),
                    LastName = string.Join(" ", participant.AccountDisplayName.Trim().Split(" ").Skip(1)),
                    UserCode = participant.AccountPhoneNo.Trim().Replace("+", "").PadLeft(13, '0'),
                    EmailAddress = participant.AccountEmail.Trim(),
                    TelephoneNo = participant.AccountPhoneNo.Trim().Replace("+", ""),
                    JobPosition = string.Empty,
                    Department = string.Empty,
                    IsAd = false,
                    IsEnabled = true,
                    IsToReceiveNotification = true,
                    IsMailSent = false,
                    LoginAttempt = 0,
                    IsLocked = false,
                    DescriptionText = string.Empty,
                    LanguageCountryId = 1,
                    DefaultUrl = $"/",
                    InsertUser = "web_app",
                    InsertDate = DateTime.Now
                };

                _unitOfWork.AccountRepo.InsertAccount(account);
                _unitOfWork.Complete();

                //INSERT ACCOUNT ROLE

                if (role != null)
                {
                    AccountRole accRole = new AccountRole()
                    {
                        AccountId = account.AccountId,
                        RoleId = role.RoleId,
                        CreatedDate = DateTime.Now,
                        TakeOverCaseId = 1,
                        InsertDate = DateTime.Now,
                        InsertUser = "web_app"
                    };

                    _unitOfWork.RoleRepo.InsertAccountRole(accRole);
                    _unitOfWork.Complete();
                }

                accountID = account.AccountId;
            }
            else
            {
                //UPDATE ACCOUNT
                acc.FirstName = participant.AccountDisplayName.Trim().Split(" ").First();
                acc.LastName = string.Join(" ", participant.AccountDisplayName.Trim().Split(" ").Skip(1));
                acc.TelephoneNo = participant.AccountPhoneNo.Trim().Replace("+", "");
                acc.UpdateUser = "web_app";
                acc.UpdateDate = DateTime.Now;

                _unitOfWork.AccountRepo.UpdateAccount(acc);
                _unitOfWork.Complete();

                if (acc.AccountRoles.Count() == 0)
                {
                    //INSERT ACCOUNT ROLE

                    if (role != null)
                    {
                        AccountRole accRole = new AccountRole()
                        {
                            AccountId = acc.AccountId,
                            RoleId = role.RoleId,
                            CreatedDate = DateTime.Now,
                            TakeOverCaseId = 1,
                            InsertDate = DateTime.Now,
                            InsertUser = "web_app"
                        };

                        _unitOfWork.RoleRepo.InsertAccountRole(accRole);
                        _unitOfWork.Complete();
                    }
                }

                accountID = acc.AccountId;

            }

            return accountID;
        }
        #endregion

    }
}
