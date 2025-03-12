using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BackendTzReworked.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController(IMapper _mapper)
        {
            this._mapper = _mapper;
        }

        protected readonly IMapper _mapper;
    }
}
