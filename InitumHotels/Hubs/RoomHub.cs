using DataAccess.Repository;
using Microsoft.AspNetCore.SignalR;
using Models;
using Utility;

namespace InitumHotels.Hubs
{
    public class RoomHub(
        IServiceProvider serviceProvider
        ) : Hub
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task RemoveImage(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var _unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var image = _unitOfWork.Repository<RoomImage>().GetOne(
                e => e.RoomImageId == id);
            bool IsItDeleted = false;

            if (image != null) 
            {
                ImageManger.DeleteImage(image.ImageName, ImageLocation.Rooms);
                _unitOfWork.Repository<RoomImage>().Remove(image);
                IsItDeleted = true;
            }

            await Clients.Caller.SendAsync("DeleteResult", IsItDeleted, id);
        }
        #region #
        //public async Task RemoveAllImage(int id)
        //{
        //    using var scope = _serviceProvider.CreateScope();
        //    var _unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

        //    var room = _unitOfWork.Repository<Room>().GetOne(
        //        e => e.RoomId == id, im => im.Images);

        //    bool IsItDeleted = false;

        //    if (room != null)
        //    {
        //        ImageManger.DeleteImageList(
        //            room.Images.Select(e => e.ImageName).ToList(), ImageLocation.Rooms);
        //        _unitOfWork.Repository<RoomImage>().RemoveRange(room.Images);
        //        IsItDeleted = true;
        //    }

        //    await Clients.Caller.SendAsync("DeleteAllResult", IsItDeleted);
        //}
        #endregion
    }
}
