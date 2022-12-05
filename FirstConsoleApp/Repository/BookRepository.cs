using FirstConsoleApp.Data;
using FirstConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstConsoleApp.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context = null;
        private readonly IConfiguration _configuration;

        public BookRepository(BookStoreContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<int> AddNewBook(BookModel bookModel)
        {
            var newBook = new Book()
            {
                Author = bookModel.Author,
                Title = bookModel.Title,
                Description = bookModel.Description,
                TotalPages = bookModel.TotalPages.HasValue ? bookModel.TotalPages.Value : 0,
                LanguageId = bookModel.LanguageId,
                CoverImageUrl = bookModel.CoverImageUrl,
                BookPdfUrl = bookModel.BookPdfUrl,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            newBook.BookGallery = new List<BookGallery>();

            foreach (var file in bookModel.Galleries)
            {
                newBook.BookGallery.Add(new BookGallery()
                {
                    Name = file.Name,
                    Url = file.Url
                });
            }

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            return newBook.Id;
        }

        public async Task<List<BookModel>> GetAllBooks()
        {
            //var books = new List<BookModel>();

            //var allBooks = await _context.Books.Include(x => x.Language).ToListAsync();

            //if(allBooks?.Any() != null)
            //{
            //    foreach (var book in allBooks)
            //    {
            //        books.Add(new BookModel
            //        {
            //            Id = book.Id,
            //            Author = book.Author,
            //            Description = book.Description,
            //            Category = book.Category,
            //            LanguageId = book.LanguageId,
            //            Language = book.Language.Name,
            //            Title = book.Title,
            //            TotalPages = book.TotalPages,
            //            CoverImageUrl= book.CoverImageUrl,
            //        });
            //    }
            //}

            //return books;

            return await _context.Books.Select(book => new BookModel()
            {
                Id = book.Id,
                Author = book.Author,
                Description = book.Description,
                Category = book.Category,
                LanguageId = book.LanguageId,
                Language = book.Language.Name,
                Title = book.Title,
                TotalPages = book.TotalPages,
                CoverImageUrl = book.CoverImageUrl,
            }).ToListAsync();

        }

        public async Task<BookModel> GetBookByID(int id)
        {
            return await _context.Books.Where(x => x.Id == id)
                    .Select(book => new BookModel()
                    {
                        Id = book.Id,
                        Author = book.Author,
                        Description = book.Description,
                        Category = book.Category,
                        LanguageId = book.LanguageId,
                        Language = book.Language.Name,
                        Title = book.Title,
                        TotalPages = book.TotalPages,
                        CoverImageUrl = book.CoverImageUrl,
                        BookPdfUrl = book.BookPdfUrl,
                        Galleries = book.BookGallery.Select(g => new GalleryModel()
                        {
                            Id = g.Id,
                            Name = g.Name,
                            Url = g.Url
                        }).ToList()
                    }).FirstOrDefaultAsync();
        }

        public async Task<List<BookModel>> GetTopBooksAsync(int count)
        {
            return await _context.Books.Select(book => new BookModel()
            {
                Id = book.Id,
                Author = book.Author,
                Description = book.Description,
                Category = book.Category,
                LanguageId = book.LanguageId,
                Language = book.Language.Name,
                Title = book.Title,
                TotalPages = book.TotalPages,
                CoverImageUrl = book.CoverImageUrl,
            }).Take(count).ToListAsync();

        }

        public List<BookModel> SearchBook(string title, string authorName)
        {
            return null;
        }

        public string GetAppName()
        {
            return _configuration["AppName"];
        }

    }
}
