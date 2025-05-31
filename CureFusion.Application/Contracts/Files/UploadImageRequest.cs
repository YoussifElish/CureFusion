using Microsoft.AspNetCore.Http;

namespace CureFusion.Application.Contracts.Files;

public record RegisterDoctorImageRequest(
 IFormFile ProfileImage,
    IFormFile CertificateImage
);