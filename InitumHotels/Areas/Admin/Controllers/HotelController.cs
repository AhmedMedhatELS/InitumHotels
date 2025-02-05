using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels.AdminViewModels;
using Shared;

namespace InitumHotels.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HotelController(
        IUnitOfWork unitOfWork,
        RoomHelper roomHelper,
        HotelHelper hotelHelper
        ) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly RoomHelper _roomHelper = roomHelper;
        private readonly HotelHelper _hotelHelper = hotelHelper;

        //TempData["SuccessMessage"]
        //TempData["ErrorMessage"]
        #endregion
        #region Hotel Branch
        public IActionResult HotelBranchs() => View(new HotelBranchView
        {
            ExsitHotelBranches = _unitOfWork.Repository<HotelBranch>().Get(e => !e.IsDeleted).ToList()
        });

        [HttpPost]
        public IActionResult AddEditHotelBranch(HotelBranch HotelBranch)
        {
            List<string> ErrorMessages = [];
            HotelBranch? branch = null;

            if (HotelBranch == null)
            {
                TempData["ErrorMessage"] = "Hotel branch data is not provided.";
            }
            else
            {
                if (string.IsNullOrEmpty(HotelBranch.HotelBranchName))
                    ErrorMessages.Add("Hotel branch name is required.");

                if(string.IsNullOrEmpty(HotelBranch.HotelBranchMapsLink))
                    ErrorMessages.Add("Maps link for the hotel branch is required.");

                if (string.IsNullOrEmpty(HotelBranch.HotelBranchLocation))
                    ErrorMessages.Add("Hotel branch location is required.");

                if (HotelBranch.HotelBranchId != 0)
                {
                    branch = _unitOfWork.Repository<HotelBranch>().GetOne(
                        e => e.HotelBranchId == HotelBranch.HotelBranchId && !e.IsDeleted);

                    if(branch == null)
                        ErrorMessages.Add($"No hotel branch found with ID {HotelBranch.HotelBranchId}. Please check the ID.");
                }

                if (ErrorMessages.Count > 0)
                {
                    TempData["ErrorMessage"] = string.Join("-", ErrorMessages);
                }
                else if (HotelBranch.HotelBranchId == 0)
                {
                    //save new hotel branch
                    _unitOfWork.Repository<HotelBranch>().Add(HotelBranch);
                    TempData["SuccessMessage"] = "Hotel branch has been successfully added.";
                }
                else if (branch != null)
                {
                    //Edit hotel branch
                    branch.HotelBranchName = HotelBranch.HotelBranchName;
                    branch.HotelBranchMapsLink = HotelBranch.HotelBranchMapsLink;
                    branch.HotelBranchLocation = HotelBranch.HotelBranchLocation;

                    _unitOfWork.Repository<HotelBranch>().Update(branch);
                    TempData["SuccessMessage"] = "Hotel branch has been successfully edited.";
                }
            }      

            return RedirectToAction("HotelBranchs");
        }

        public IActionResult DeleteHotelBranch(int id)
        {
            var hotelBranch = _unitOfWork.Repository<HotelBranch>().GetOne(
                e => e.HotelBranchId == id && !e.IsDeleted);

            if (hotelBranch == null)
                TempData["ErrorMessage"] = "No Branch with this ID";
            else
            {
                hotelBranch.IsDeleted = true;
                _unitOfWork.Repository<HotelBranch>().Update(hotelBranch);
                TempData["SuccessMessage"] = "Hotel Branch Has been Deleted";
            }

            return RedirectToAction("HotelBranchs");
        }
        #endregion
        #region Hotel Rooms
        public IActionResult HotelsRoomsView() => View(new HotelsRoomsList
            {
                HotelBranches = _hotelHelper.HotelList(),
                RoomSimpleList = _roomHelper.RoomList(),
            });
        
        [HttpPost]
        public IActionResult AddRoomToHotel(int HotelId, int RoomId)
        {
            var room = _unitOfWork.Repository<Room>().GetOne(e => e.RoomId == RoomId);
            var hotel = _unitOfWork.Repository<HotelBranch>().GetOne(
                e => e.HotelBranchId == HotelId && !e.IsDeleted);

            if (room == null || hotel == null)
                TempData["ErrorMessage"] = "Somthing want Wrong!!";
            else
            {
                var IsItAreadyExsit = _unitOfWork.Repository<HotelRoom>().GetOne(
                    e => e.HotelBranchId == HotelId && e.RoomId == RoomId) != null;

                if (IsItAreadyExsit)
                    TempData["ErrorMessage"] = "Room Is Aready Assignd To the Hotel Branch";
                else
                {
                    _unitOfWork.Repository<HotelRoom>().Add(new HotelRoom
                    {
                        HotelBranchId = HotelId,
                        RoomId = RoomId
                    });
                    TempData["SuccessMessage"] = "Room Added To The HotelBranch";
                }
            }

            return RedirectToAction("HotelsRoomsView");
        }

        public IActionResult DeleteRoomToHotel(int HotelId, int RoomId)
        {
            var hotelRoom = _unitOfWork.Repository<HotelRoom>().GetOne(
                e => e.RoomId == RoomId && e.HotelBranchId == HotelId);
            
            if (hotelRoom == null)
                TempData["ErrorMessage"] = "Somthing want Wrong!!";
            else
            {
                _unitOfWork.Repository<HotelRoom>().Remove(hotelRoom);
                TempData["SuccessMessage"] = "Room Removed from the HotelBranch";
            }

            return RedirectToAction("HotelsRoomsView");
        }
        #endregion
    }
}
