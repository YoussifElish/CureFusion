namespace CureFusion.Contracts.Files;

public record RegisterDoctorImageRequest(
 IFormFile ProfileImage,
    IFormFile CertificateImage
);