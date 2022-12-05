using FirstConsoleApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FirstConsoleApp.Components
{
    public class TopBooksViewComponent : ViewComponent
    {
        private readonly IBookRepository _bookRepository;

        public TopBooksViewComponent(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            var book = await _bookRepository.GetTopBooksAsync(count);

            return View(book);
        }
    }
}
