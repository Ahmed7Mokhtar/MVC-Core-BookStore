using FirstConsoleApp.Models;
using FirstConsoleApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirstConsoleApp.Controllers
{
    // capital or small letters doesn't matter to url
    //[Route("[controller]/[action]")]
    public class BookController : Controller
    {

        private readonly IBookRepository _bookRepository = null;
        private readonly ILanguageRepository _languageRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IBookRepository bookRepository,
            ILanguageRepository languageRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _languageRepository = languageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("all-books")]
        public async Task<ViewResult> GetAllBooks()
        {
            var data = await _bookRepository.GetAllBooks();

            return View(data);
        }

        [Route("book-details/{id:int:min(1)}", Name = "bookDetailsRoute")]
        public async Task<ViewResult> GetBook(int Id)
        {
            var data = await _bookRepository.GetBookByID(Id);
            
            return View(data);
        }

        // query string ?bookname-asda&authorname=sada
        // https://localhost:53471/book/searchbooks?bookname=MVCBook&authorname=Ahmed
        public List<BookModel> SearchBooks(string bookName, string authorName)
        {
            return _bookRepository.SearchBook(bookName, authorName);
        }

        [Authorize]
        public async Task<ViewResult> AddNewBook(bool isSuccess = false, int booId = 0)
        {
            //passing default language to select tag
            var model = new BookModel();

            ViewBag.IsSuccess = isSuccess;
            ViewBag.bookID = booId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBook(BookModel bookModel)
        {
            if(ModelState.IsValid)
            {
                if(bookModel.CoverPhoto != null)
                {
                    string folder = "books/cover/";
                    bookModel.CoverImageUrl = await UploadFile(folder, bookModel.CoverPhoto);
                }

                if (bookModel.GalleryFiles != null)
                {
                    string folder = "books/gallery/";

                    bookModel.Galleries = new List<GalleryModel>();

                    foreach (var file in bookModel.GalleryFiles)
                    {
                        var gallery = new GalleryModel
                        {
                            Name = file.FileName,
                            Url = await UploadFile(folder, file)
                        };

                        bookModel.Galleries.Add(gallery);
                    }
                }

                if (bookModel.BookPdf != null)
                {
                    string folder = "books/pdf/";
                    bookModel.BookPdfUrl = await UploadFile(folder, bookModel.BookPdf);
                }


                int id = await _bookRepository.AddNewBook(bookModel);
                if(id > 0)
                {
                    // geting the name of params as a string
                    // passing data to AddNewBook get method
                    return RedirectToAction(nameof(AddNewBook), new { isSuccess = true, booId = id}); 
                }
            }

            // custom message to ModelsState
            //ModelState.AddModelError("", "This is my custom error message!"); 

            return View();
        }

        private async Task<string> UploadFile(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
    }
}
