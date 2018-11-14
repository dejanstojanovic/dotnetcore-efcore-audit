using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sample.Auditing.Data.UnitsOfWork;

namespace Sample.Auditing.Web.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IUnitOfWork unitOfWork;
        protected IMapper mapper;
        public BaseController(IUnitOfWork unitOfWork, IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
    }
}