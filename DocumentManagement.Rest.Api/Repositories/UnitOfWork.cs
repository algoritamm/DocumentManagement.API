using DocumentManagement.Rest.Api.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DocumentSystemContext _context;
        public UnitOfWork(DocumentSystemContext context) => _context = context;

        private IAccountRepository _AccountRepository;
        public IAccountRepository AccountRepo
        {
            get
            {
                if (_AccountRepository == null)
                    _AccountRepository = new AccountRepository(_context);
                return _AccountRepository;
            }
        }

        private ICaseRepository _CaseRepo;
        public ICaseRepository CaseRepo
        {
            get
            {
                if (_CaseRepo == null)
                {
                    _CaseRepo = new CaseRepository(_context);
                }
                return _CaseRepo;
            }
        }

        private IDocumentRepository _DocumentRepo;
        public IDocumentRepository DocumentRepo
        {
            get
            {
                if (_DocumentRepo == null)
                {
                    _DocumentRepo = new DocumentRepository(_context);
                }
                return _DocumentRepo;
            }
        }

        private IDocumentParticipantProcessRepository _DocumentParticipantProcessRepo;
        public IDocumentParticipantProcessRepository DocumentParticipantProcessRepo
        {
            get
            {
                if (_DocumentParticipantProcessRepo == null)
                {
                    _DocumentParticipantProcessRepo = new DocumentParticipantProcessRepository(_context);
                }
                return _DocumentParticipantProcessRepo;
            }
        }

        private IDocumentWorkflowRepository _DocumentWorkflowRepo;
        public IDocumentWorkflowRepository DocumentWorkflowRepo
        {
            get
            {
                if (_DocumentWorkflowRepo == null)
                {
                    _DocumentWorkflowRepo = new DocumentWorkflowRepository(_context);
                }
                return _DocumentWorkflowRepo;
            }
        }

        private ITemplateRepository _TemplateRepo;
        public ITemplateRepository TemplateRepo
        {
            get
            {
                if (_TemplateRepo == null)
                {
                    _TemplateRepo = new TemplateRepository(_context);
                }
                return _TemplateRepo;
            }
        }

        private IServiceDefRepository _ServiceDefRepo;
        public IServiceDefRepository ServiceDefRepo
        {
            get
            {
                if (_ServiceDefRepo == null)
                {
                    _ServiceDefRepo = new ServiceDefRepository(_context);
                }
                return _ServiceDefRepo;
            }
        }

        private ICaseEntityRepository _CaseEntityRepo;
        public ICaseEntityRepository CaseEntityRepo
        {
            get
            {
                if (_CaseEntityRepo == null)
                {
                    _CaseEntityRepo = new CaseEntityRepository(_context);
                }
                return _CaseEntityRepo;
            }
        }

        private ICaseWorkflowRepository _CaseWorkflowRepo;
        public ICaseWorkflowRepository CaseWorkflowRepo
        {
            get
            {
                if (_CaseWorkflowRepo == null)
                {
                    _CaseWorkflowRepo = new CaseWorkflowRepository(_context);
                }
                return _CaseWorkflowRepo;
            }
        }

        private IEntityLegalRepository _EntityLegalRepo;
        public IEntityLegalRepository EntityLegalRepo
        {
            get
            {
                if (_EntityLegalRepo == null)
                {
                    _EntityLegalRepo = new EntityLegalRepository(_context);
                }
                return _EntityLegalRepo;
            }
        }

        private IEntityRepository _EntityRepository;
        public IEntityRepository EntityRepo
        {
            get
            {
                if (_EntityRepository == null)
                {
                    _EntityRepository = new EntityRepository(_context);
                }
                return _EntityRepository;
            }
        }

        private IRoleRepository _RoleRepository;
        public IRoleRepository RoleRepo
        {
            get
            {
                if (_RoleRepository == null)
                {
                    _RoleRepository = new RoleRepository(_context);
                }
                return _RoleRepository;
            }
        }

        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ResetEntity(object entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
