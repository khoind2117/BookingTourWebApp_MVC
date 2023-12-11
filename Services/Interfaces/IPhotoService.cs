using CloudinaryDotNet.Actions;

namespace BookingTourWebApp_MVC.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoASync(IFormFile file);
        Task<DeletionResult> DeletePhotoASync(string publicId);
    }
}
