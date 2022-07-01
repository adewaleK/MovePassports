using PassMoveAPI.Data.DTOs;

namespace PassMoveAPI.Services
{
    public interface IPhotoAcessor
    {
        Task<FileObject> AddPhoto(IFormFile photo);
        Task<FileObject> AddFile(IFormFile file);
    }
}
