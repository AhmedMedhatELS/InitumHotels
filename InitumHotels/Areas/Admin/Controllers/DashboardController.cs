using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Models;
using Models.ViewModels.AdminViewModels;

namespace InitumHotels.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager
        ) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        //TempData["SuccessMessage"]
        //TempData["ErrorMessage"]
        #endregion
        #region Dashboard Home Page
        public IActionResult Index()
        {
            return View();
        }
        #endregion

       

    }
}
