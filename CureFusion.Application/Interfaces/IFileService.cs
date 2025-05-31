using CureFusion.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CureFusion.Application.Services;

public interface IFileService
{

    Task<UploadedFile> UploadImagesAsync(IFormFile image, CancellationToken cancellationToken = default);


}