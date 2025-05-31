using Microsoft.AspNetCore.Http;

namespace CureFusion.Application.Contracts.Files;

public record UploadImageRequest(
    IFormFile? Image

);