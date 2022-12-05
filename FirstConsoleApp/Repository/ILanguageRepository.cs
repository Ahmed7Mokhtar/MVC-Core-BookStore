using FirstConsoleApp.Models;

namespace FirstConsoleApp.Repository
{
    public interface ILanguageRepository
    {
        Task<List<LanguageModel>> GetLanguages();
    }
}