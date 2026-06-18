namespace SupplyManagement.API.Service.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string subDirectory);
    }
}
