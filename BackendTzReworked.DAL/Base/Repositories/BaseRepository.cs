using AutoMapper;
using BackendTzReworked.DAL.EntityFeamework;



namespace BackendTzReworked.DAL.Base.Repositories
{
    public class BaseRepository
    {
        public BaseRepository(Context _context, IMapper _mapper)
        {
            this._context = _context;
            this._mapper = _mapper;
        }

        protected readonly Context _context;
        protected readonly IMapper _mapper;



        public async Task<bool> Save()
            => await _context.SaveChangesAsync() > 0;
    }
}