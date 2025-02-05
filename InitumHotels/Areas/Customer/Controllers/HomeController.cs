using System.Diagnostics;
using DataAccess.Repository.IRepository;
using InitumHotels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels.UserViewModels;
using Newtonsoft.Json;
using Shared;

namespace InitumHotels.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        RoomHelper roomHelper,
        HotelHelper hotelHelper
        ) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoomHelper _roomHelper = roomHelper;
        private readonly HotelHelper _hotelHelper = hotelHelper;

        //TempData["SuccessMessage"]
        //TempData["ErrorMessage"]
        #endregion

        #region Home Filter
        [HttpPost]
        public IActionResult FilterHome(ReservationFilter Filter)
        {
            if (Filter == null)
                TempData["ErrorMessage"] = "Error in Filteration Please Try Again";
            else
            {
                List<string> ErrorMessages = [];

                if(Filter.CheckInDate <  DateTime.Now)
                    ErrorMessages.Add("Check-in date cannot be in the past");

                if (Filter.CheckInDate >= Filter.CheckOutDate)
                    ErrorMessages.Add("Check-out date must be after the check-in date.");

                var hotelBranch = _unitOfWork.Repository<HotelBranch>().GetOne(
                    e => e.HotelBranchId == Filter.BranchId && !e.IsDeleted);

                if(hotelBranch == null)
                    ErrorMessages.Add("Selected hotel branch does not exist.");

                if(ErrorMessages.Count > 0)
                    TempData["ErrorMessage"] = string.Join("-", ErrorMessages);
                else
                {
                    TempData["Filter"] = JsonConvert.SerializeObject(Filter);
                }
            }

            return RedirectToAction("index");
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            HomeViewModel view = new()
            {
                RoomTypes = _roomHelper.GetRoomTypes(),
                HotelBranches = _hotelHelper.GetAllBranchs()
            };

            if (TempData["Filter"] != null)
            {
                #region User data
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    view.CustomerName = user.FullName;
                    view.PhoneNumber = user.PhoneNumber;
                    view.NationalId = user.NationalId;
                    view.Email = user.Email;
                }
                #endregion
                var filter = JsonConvert.DeserializeObject<ReservationFilter>(TempData["Filter"] as string ?? "") ?? new ReservationFilter();             

                view.ReservationFilter = filter;

                var hotelBranch = _unitOfWork.Repository<HotelBranch>().GetOne(
                    e => e.HotelBranchId == filter.BranchId && !e.IsDeleted,
                    r => r.Rooms) ?? new HotelBranch();

                view.RoomsHomeViews = _roomHelper.GetRoomsHomeViews(hotelBranch.Rooms.ToList());
            }
            else
            {
                var rooms = _unitOfWork.Repository<Room>().Get(
                    null,t => t.RoomType, m => m.Images);

                view.RoomsHomeViews = _roomHelper.GetRoomsHomeViews(rooms.ToList());
            }

            return View(view);
        }

    }
}
