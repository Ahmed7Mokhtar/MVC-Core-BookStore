using FirstConsoleApp.Data;
using FirstConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstConsoleApp.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly BookStoreContext _context = null;

        public LanguageRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<LanguageModel>> GetLanguages()
        {
            // convert to LanguageModel
            return await _context.Languages.Select(x => new LanguageModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToListAsync();
        }
    }
}
