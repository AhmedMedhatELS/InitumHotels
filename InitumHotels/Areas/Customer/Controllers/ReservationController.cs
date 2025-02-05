using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels.ReservationViewModel;
using Models.ViewModels.UserViewModels;
using Newtonsoft.Json;
using Utility;
using Shared;
using Microsoft.AspNetCore.Authorization;

namespace InitumHotels.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ReservationController(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        RoomHelper roomHelper,
        HotelHelper hotelHelper,
        ReservationHelper reservationHelper
        ) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoomHelper _roomHelper = roomHelper;
        private readonly HotelHelper _hotelHelper = hotelHelper;
        private readonly ReservationHelper _reservationHelper = reservationHelper;

        //TempData["SuccessMessage"]
        //TempData["ErrorMessage"]
        #endregion
        #region new reservation
        public IActionResult MakeReservaion(ReservationData Reservation)
        {
            if (Reservation == null)
                TempData["ErrorMessage"] = "Error in Reservation Please Try Again";
            else
            {
                List<string> ErrorMessages = [];
                Dictionary<int,string>? RoomsDatapairs = [];

                #region check filteration vaildation
                if (Reservation.CheckInDate < DateTime.Now)
                    ErrorMessages.Add("Check-in date cannot be in the past");

                if (Reservation.CheckInDate >= Reservation.CheckOutDate)
                    ErrorMessages.Add("Check-out date must be after the check-in date.");

                var hotelBranch = _unitOfWork.Repository<HotelBranch>().GetOne(
                    e => e.HotelBranchId == Reservation.BranchId && !e.IsDeleted);

                if (hotelBranch == null)
                    ErrorMessages.Add("Selected hotel branch does not exist.");
                #endregion
                #region cheack User Data
                if (string.IsNullOrEmpty(Reservation.CustomerName))
                    ErrorMessages.Add("Customer name is required.");

                if (string.IsNullOrEmpty(Reservation.NationalId))
                    ErrorMessages.Add("National ID is required.");

                if (string.IsNullOrEmpty(Reservation.PhoneNumber))
                    ErrorMessages.Add("Phone number is required.");

                if (string.IsNullOrEmpty(Reservation.Email))
                    ErrorMessages.Add("Email is required.");
                #endregion
                #region cheack Reservation Data
                RoomsDatapairs = _reservationHelper.GetRoomIDs(Reservation.ReservationRoomsData);

                if (RoomsDatapairs == null ||
                    !_reservationHelper.IsAllTheRoomExsitsAndInBranch(
                        Reservation.BranchId, RoomsDatapairs.Keys.ToList()))
                    ErrorMessages.Add("There was an issue with the room data. Please try again.");                
                #endregion

                if (ErrorMessages.Count > 0)
                    TempData["ErrorMessage"] = string.Join("-", ErrorMessages);
                else
                {
                    //Get rooms data in the orginal form
                    var RoomsOrginalData = _reservationHelper.GetRoomsData(RoomsDatapairs ?? []);
                    
                    //final cheack
                    if (RoomsOrginalData == null)
                        TempData["ErrorMessage"] = "There was an issue with the room data. Please try again.";
                    else
                    {
                        var userId = _userManager.GetUserId(User);

                        #region discount cheack
                        DiscountDetail PayementDetails = new()
                        {
                            Total = _reservationHelper.TotalPrice(
                                RoomsOrginalData,Reservation.CheckInDate,Reservation.CheckOutDate)
                        };

                        PayementDetails = _reservationHelper.GetDiscountAmount(
                            PayementDetails.Total, Reservation.NationalId,userId);

                        #endregion
                        #region create reservation
                        Reservation newReservation = new()
                        {
                            Total = PayementDetails.Total,
                            DiscountOn = PayementDetails.IsDiscounted,
                            NationalId = Reservation.NationalId,
                            PhoneNumber = Reservation.PhoneNumber,
                            CustomerName = Reservation.CustomerName,
                            ApplicationUserId = userId,
                            CheckInDate = Reservation.CheckInDate,
                            CheckOutDate = Reservation.CheckOutDate,
                            HotelBranchId = Reservation.BranchId,
                            Status = ReservationStatus.Pending,
                            Email = Reservation.Email,
                        };
                        _unitOfWork.Repository<Reservation>().Add(newReservation);

                        foreach (var roomId in RoomsDatapairs?.Keys.ToList() ?? [])
                            newReservation.Rooms.Add(new ReservationRoom
                            {
                                ReservationId = newReservation.ReservationId,
                                RoomId = roomId,
                                RoomDetails = RoomsDatapairs?[roomId] ?? ""
                            });

                        _unitOfWork.Repository<ReservationRoom>().AddRange(newReservation.Rooms);
                        #endregion
                        #region Success Massage
                        if (PayementDetails.IsDiscounted)
                            TempData["SuccessMessage"] = $"Your reservation was successful! You've received a discount of {PayementDetails.DiscountAmount} (5%).";
                        else
                            TempData["SuccessMessage"] = "Your reservation was successful. See you soon!";
                        #endregion
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region user reservation
        [Authorize]
        public IActionResult UserReservation()
        {
            var userId = _userManager.GetUserId(User);

            if(userId == null) return RedirectToAction("Index", "Home");

            return View(_reservationHelper.GetAllReservations(userId));
        }
        #endregion
    }
}
