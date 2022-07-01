using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using PassMoveAPI.Data.DTOs;

namespace PassMoveAPI.Services
{
    public class PhotoAcessor : IPhotoAcessor
    {
        private readonly string connectionString;
        private readonly string containerName;
        public PhotoAcessor(IConfiguration configuration)
        {
            containerName = configuration["Person"];
            connectionString = configuration["AzureBlobStorageConnectionString"];
        }
        public async Task<FileObject> AddPhoto(IFormFile photo)
        {
            byte[] content;
            string extension;
            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                content = memoryStream.ToArray();
                extension = Path.GetExtension(photo.FileName);
            }
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = container.GetBlockBlobReference(fileName);
            await blob.UploadFromByteArrayAsync(content, 0, content.Length);
            blob.Properties.ContentType = photo.ContentType;
            await blob.SetPropertiesAsync();
            var fileResponse = new FileObject()
            {
                url = blob.Uri.ToString(),
                bytes = photo.Length,
                original_filename = photo.FileName,
                createdAt = DateTime.Now,
                format = Path.GetExtension(photo.FileName),
                secure_url = blob.Uri.ToString()
            };
            return fileResponse;
        }
        public async Task<FileObject> AddFile(IFormFile file)
        {
            byte[] content;
            string extension;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                content = memoryStream.ToArray();
                extension = Path.GetExtension(file.FileName);
            }
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = container.GetBlockBlobReference(fileName);
            await blob.UploadFromByteArrayAsync(content, 0, content.Length);
            blob.Properties.ContentType = file.ContentType;
            await blob.SetPropertiesAsync();
            var fileResponse = new FileObject()
            {
                url = blob.Uri.ToString(),
                bytes = file.Length,
                original_filename = file.FileName,
                createdAt = DateTime.Now,
                format = Path.GetExtension(file.FileName),
                secure_url = blob.Uri.ToString()
            };
            return fileResponse;
        }
    }
}
