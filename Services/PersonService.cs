using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.DataMovement;
using PassMoveAPI.Data.DTOs;
using PassMoveAPI.Data.Entities;
using PassMoveAPI.Data.Repository;
using PassMoveAPI.Helpers;
using System.Net;

namespace PassMoveAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepo;
        private readonly IPhotoAcessor _photoAcessor;
        private readonly string connectionString;

        public PersonService(IPersonRepository personRepo, IPhotoAcessor photoAcessor, IConfiguration configuration)
        {
            _personRepo = personRepo;
            _photoAcessor = photoAcessor;
            connectionString = configuration["AzureBlobStorageConnectionString"];
        }
        public async Task<PersonDto> AddPersonAsync(FileModel file)
        {
            FileObject imageUrl = new FileObject();
            imageUrl = await _photoAcessor.AddPhoto(file.ImageFile);

            var person = await _personRepo.GetPerson(file.Name.ToLower());

            if (person != null) throw new RestException(HttpStatusCode.BadRequest, "Person already exist");

            var newPerson = new Person { Name = file.Name, Passport = imageUrl.url };
            await _personRepo.AddPersonAsync(newPerson);
            return new PersonDto { Name = newPerson.Name, Passport = newPerson.Passport };
        }

        public async Task<IEnumerable<PersonDto>> GetAllPersons()
        {
            var persons = await _personRepo.GetPersonsAsync();
            if (persons == null)
                throw new RestException(HttpStatusCode.NotFound, "No persons found");
            var personsToReturn = persons.Select(x => new PersonDto { Name = x.Name, Passport = x.Passport });
            return personsToReturn;
        }

        public async Task Move(List<Person> persons)
        {
            string storageConnectionString = connectionString;
            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);

            foreach (var p in persons)
            {
                Uri uri = new Uri(p.Passport);
                CloudBlockBlob blob = GetBlob(account, p.Name, "person2");
                TransferCheckpoint checkpoint = null;
                SingleTransferContext context = GetSingleTransferContext(checkpoint);
                CancellationTokenSource cancellationSource = new CancellationTokenSource();
                Task task = TransferManager.CopyAsync(uri, blob, true, null, context, cancellationSource.Token);
            }

        }

        public static SingleTransferContext GetSingleTransferContext(TransferCheckpoint checkpoint)
        {
            SingleTransferContext context = new SingleTransferContext(checkpoint);
            return context;
        }

        public static CloudBlockBlob GetBlob(CloudStorageAccount account, string blobName, string containerName)
        {
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExistsAsync().Wait();
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            return blob;
        }
    }
}
