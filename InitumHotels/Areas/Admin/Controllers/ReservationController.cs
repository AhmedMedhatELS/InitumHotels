using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Models;
using Shared;
using Utility;

namespace InitumHotels.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReservationController(
        IUnitOfWork unitOfWork,
        ReservationHelper reservationHelper,
        IEmailSender emailSender
        ) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ReservationHelper _reservationHelper = reservationHelper;
        private readonly IEmailSender _emailSender = emailSender;

        //TempData["SuccessMessage"]
        //TempData["ErrorMessage"]
        #endregion
        public IActionResult ReservationsView() => View(_reservationHelper.GetAllReservations());
        
        public async Task<IActionResult> EditReservationStatus(int reservationId,ReservationStatus status, string EmailMassage)
        {
            var reservation = _unitOfWork.Repository<Reservation>().GetOne(
                e => e.ReservationId == reservationId);

            if (reservation == null)
                TempData["ErrorMessage"] = "No Reservation with this ID!!";
            else
            {
                reservation.Status = status;
                _unitOfWork.Repository<Reservation>().Update(reservation);
                #region email sending 
                await _emailSender.SendEmailAsync(
                 reservation.Email,
                 "Response to Your Hotel Booking Reservation",
                 $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
                    <p>Dear <strong>{reservation.CustomerName}</strong>,</p>

                    <p>{EmailMassage}</p>

                    <p>If you have any further questions or need additional assistance, feel free to contact us.</p>

                    <p style='margin-top: 20px; font-weight: bold;'>Best regards,</p>
                    <p>Initum Hotels Team</p>
                </body>
                </html>"
                );
                #endregion
                TempData["SuccessMessage"] = "reservation status updated successfully";                
            }
            return RedirectToAction("ReservationsView");
        }
    }
}
