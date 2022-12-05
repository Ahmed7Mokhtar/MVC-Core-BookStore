using BookStore.Models;
using BookStore.Services;
using FirstConsoleApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace FirstConsoleApp.Controllers
{
    //[Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        #region ViewData attribute
        // ViewData attribute
        //[ViewData]
        //public string CustomProperty { get; set; }
        //[ViewData]
        //public string Title { get; set; } 
        #endregion


        public HomeController(IConfiguration configuration, IUserService userService, IEmailService emailService)
        {
            _configuration = configuration;
            _userService = userService;
            _emailService = emailService;
        }

        // ViewResult to retuen a view
        // default route page
        // ~/ overrides route setting 
        //[Route("~/")]
        public async Task<ViewResult> Index()
        {
            //UserEmailOptions options = new UserEmailOptions()
            //{
            //    ToEmails = new List<string>() { "test@gmail.com" },
            //    PlaceHolders = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("{{userName}}", "Ahmed") }
            //};

            //await _emailService.SendTestEmail(options);


            #region passing anonymous type
            //// passing anonymous type to ViewBag
            //dynamic data = new ExpandoObject();
            //data.Id = 1;
            //data.Name = "Ali";

            //ViewBag.Data = data;

            //ViewBag.Type = new BookModel()
            //{
            //    Id = 5,
            //    Author = "This is Author"
            //}; 
            #endregion

            #region ViewData
            //ViewData["property1"] = "Ahmed";

            //ViewData["book"] = new BookModel() { Author = "Ali", Id = 1}; 
            #endregion

            #region ViewData Attribute
            //Title = "Home page from controller";
            //CustomProperty = "custom value"; 
            #endregion



            // retuens a view with the same name as the action method
            return View();


            //return View("AboutUs", obj);
        }

        // define route in two methods
        //[Route("about-us")]
        //[HttpGet("about-us", Name = "aboutus", Order = 1)]
        // alpha means only alphapets and minimum lenght is 6
        //[Route("about-us/{name:alpha:minlength(6):regex()}")]
        public ViewResult AboutUs()
        {

            // retuens a view with the same name as the action method
            return View();
        }

        //[Route("contact-us", Name = "contact-us")]
        public ViewResult ContactUs()
        {

            // retuens a view with the same name as the action method
            return View();
        }
    }
}
