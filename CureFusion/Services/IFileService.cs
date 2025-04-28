using CureFusion.Contracts.Files;
using CureFusion.Entities;

namespace RealState.Services;

public interface IFileService
{

    Task<UploadedFile> UploadImagesAsync(IFormFile image, CancellationToken cancellationToken = default);


}