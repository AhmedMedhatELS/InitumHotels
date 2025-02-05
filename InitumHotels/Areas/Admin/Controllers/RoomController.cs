using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels.AdminViewModels;
using Shared;
using Utility;

namespace InitumHotels.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoomController(
        IUnitOfWork unitOfWork,
        RoomHelper roomHelper
        ) : Controller
    {
        #region Start Up
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly RoomHelper _roomHelper = roomHelper;

        //TempData["SuccessMessage"]
        //TempData["ErrorMessage"]
        #endregion
        #region Room Types
        public IActionResult RoomTypes() => View(
            new RoomTypeView
            {
                RoomTypes = _unitOfWork.Repository<RoomType>().Get(e => !e.IsDeleted).ToList()
            });

        [HttpPost]
        public IActionResult NewRoomType(string Name)
        {

            if (string.IsNullOrEmpty(Name))
            {
                TempData["ErrorMessage"] = "Name is required.";
            }
            else
            {
                _unitOfWork.Repository<RoomType>().Add(new RoomType { Name = Name });
                TempData["SuccessMessage"] = "Room type added successfully.";
            }

            return RedirectToAction("RoomTypes");
        }

        [HttpPost]
        public IActionResult EditRoomType(RoomType room)
        {
            if (room == null)
            {
                TempData["ErrorMessage"] = "Room data cannot be null.";
            }
            else if (string.IsNullOrEmpty(room.Name))
            {
                TempData["ErrorMessage"] = "Room name is required.";
            }
            else
            {
                var roomType = _unitOfWork.Repository<RoomType>().GetOne(
                    e => e.RoomTypeId == room.RoomTypeId && !e.IsDeleted);

                if (roomType == null)
                {
                    TempData["ErrorMessage"] = "Room type not found.";
                }
                else
                {
                    roomType.Name = room.Name;
                    _unitOfWork.Repository<RoomType>().Update(roomType);
                    TempData["SuccessMessage"] = "Room type Edit successfully.";
                }
            }

            return RedirectToAction("RoomTypes");
        }

        public IActionResult DeleteRoomType(int id)
        {
            var roomType = _unitOfWork.Repository<RoomType>().GetOne(
                e => e.RoomTypeId == id && !e.IsDeleted);

            if (roomType == null)
                TempData["ErrorMessage"] = "No Room Type with this ID"; 
            else
            {
                roomType.IsDeleted = true;
                _unitOfWork.Repository<RoomType>().Update(roomType);
                TempData["SuccessMessage"] = "Room Type Has been Deleted";
            }   

            return RedirectToAction("RoomTypes");
        }
        #endregion
        #region New Room
        public IActionResult NewRoom() => View(new NewRoom 
        { roomTypes = _roomHelper.GetRoomTypes()});

        [HttpPost]
        public IActionResult AddRoom(NewRoom NewRoom)
        {
            if (ModelState.IsValid && NewRoom.ImagesFiles.Count > 0) 
            {
                var imagesNamesList = ImageManger.SaveImageList(NewRoom.ImagesFiles, ImageLocation.Rooms);

                if (imagesNamesList.Count == 0)
                    TempData["ErrorMessage"] = "An error occurred while saving the images.";
                else
                {
                    Room room = new()
                    {
                        RoomName = NewRoom.RoomName,
                        Description = NewRoom.Description,
                        MaxAdults = NewRoom.MaxAdults,
                        MaxChildren = NewRoom.MaxChildren,
                        NumBeds = NewRoom.NumBeds,
                        PricePerNight = NewRoom.PricePerNight,
                        RoomTypeId = NewRoom.RoomTypeId,                        
                    };
                    _unitOfWork.Repository<Room>().Add(room);

                    foreach (var image in imagesNamesList)
                        room.Images.Add(new RoomImage
                        {
                            ImageName = image,
                            RoomId = room.RoomId
                        });
                    _unitOfWork.Repository<RoomImage>().AddRange(room.Images);

                    TempData["SuccessMessage"] = "New room added successfully.";

                    return RedirectToAction("NewRoom",new NewRoom 
                    { roomTypes = _roomHelper.GetRoomTypes() });
                }
            }
            return RedirectToAction("NewRoom", NewRoom);
        }
        #endregion
        #region View Rooms
        public IActionResult ViewRooms() => View(_unitOfWork.Repository<Room>().Get(
            null, im => im.Images, rt => rt.RoomType));
        #endregion
        #region Edit Room
        public IActionResult EditRoom(int id)
        {  
            var room = _unitOfWork.Repository<Room>().GetOne(
                e => e.RoomId == id,im => im.Images);

            if (room == null)
            {
                TempData["ErrorMessage"] = "No Room Found With this ID";
                return RedirectToAction("ViewRooms");
            }

            EditRoomView view = new()
            {
                RoomId=room.RoomId,
                Description = room.Description,
                MaxAdults = room.MaxAdults,
                MaxChildren = room.MaxChildren,
                NumBeds = room.NumBeds,
                PricePerNight = room.PricePerNight,
                RoomName = room.RoomName,
                RoomTypeId = room.RoomTypeId,
                RoomTypes = _roomHelper.GetRoomTypes(),
                RoomImagesNames = room.Images.ToList(),
            };

            return View(view); 
        }

        [HttpPost]
        public IActionResult EditRoom(EditRoomView room)
        {
            if (ModelState.IsValid)
            {
                var oldRoom = _unitOfWork.Repository<Room>().GetOne(
                    e => e.RoomId == room.RoomId,im => im.Images);
               
                #region cheack for the corner cases
                if (oldRoom == null) 
                {
                    TempData["ErrorMessage"] = "No Room Found With This Id";
                    return RedirectToAction("ViewRooms");
                }

                if (oldRoom.Images.Count == 0 && room.ImagesFiles.Count == 0) 
                {
                    TempData["ErrorMessage"] = "Room Must Have Images";
                    return RedirectToAction("EditRoom",new {id = room.RoomId});
                }
                #endregion

                #region new Images

                if (room.ImagesFiles.Count > 0)
                {
                    var newImagesnames = ImageManger.SaveImageList(room.ImagesFiles, ImageLocation.Rooms);

                    List<RoomImage> images = [];

                    foreach (var image in newImagesnames)
                        images.Add(new RoomImage
                        {
                            RoomId = room.RoomId,
                            ImageName = image,
                        });

                    _unitOfWork.Repository<RoomImage>().AddRange(images);
                }
                #endregion

                #region update Room Details
                oldRoom.NumBeds = room.NumBeds;
                oldRoom.MaxAdults = room.MaxAdults;
                oldRoom.MaxChildren = room.MaxChildren;
                oldRoom.RoomName = room.RoomName;
                oldRoom.RoomTypeId = room.RoomTypeId;
                oldRoom.PricePerNight = room.PricePerNight;
                oldRoom.Description = room.Description;

                _unitOfWork.Repository<Room>().Update(oldRoom);
                #endregion

                TempData["SuccessMessage"] = "Room Edits Saved successfully.";
            }
            else
              TempData["ErrorMessage"] = "somthing went wrong no changes saved.";

            return RedirectToAction("ViewRooms");
        }
        #endregion
    }
}
