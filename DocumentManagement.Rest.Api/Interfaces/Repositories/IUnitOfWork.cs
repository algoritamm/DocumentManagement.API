namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public ICaseRepository CaseRepo { get; }
        public IDocumentRepository DocumentRepo { get; }
        public IAccountRepository AccountRepo { get; }
        public IDocumentParticipantProcessRepository DocumentParticipantProcessRepo { get; }
        public IDocumentWorkflowRepository DocumentWorkflowRepo { get; }
        public ITemplateRepository TemplateRepo { get; }
        public IServiceDefRepository ServiceDefRepo { get; }
        public ICaseEntityRepository CaseEntityRepo { get; }
        public ICaseWorkflowRepository CaseWorkflowRepo { get; }
        public IEntityRepository EntityRepo { get; }
        public IEntityLegalRepository EntityLegalRepo { get; }
        public IRoleRepository RoleRepo { get; }

        int Complete();
        void Dispose();
        void ResetEntity(object entity);
    }
}
