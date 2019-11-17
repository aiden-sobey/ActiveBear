using ActiveBear.Models;

namespace ActiveBear.Services
{
    public class BaseService
    {
        protected ActiveBearContext context;

        public BaseService(ActiveBearContext _context)
        {
            context = _context;
        }
    }
}
