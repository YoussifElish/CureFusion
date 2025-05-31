using CureFusion.Application.Services;

namespace CureFusion.Services;

public class FileService(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context) : IFileService
{
    private readonly string _filesPath = $"{webHostEnvironment.WebRootPath}/uploads";
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";
    private readonly ApplicationDbContext _context = context;





    public async Task<UploadedFile> UploadImagesAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        var uploadedFile = await UploadSingleImageAsync(image, cancellationToken);




        await _context.UploadedFiles.AddAsync(uploadedFile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return uploadedFile;
    }

    private async Task<UploadedFile> UploadSingleImageAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        var uploadedFile = new UploadedFile
        {
            FileName = image.FileName,
            ContentType = image.ContentType,
            StoredFileName = image.FileName,
            FileExtension = Path.GetExtension(image.FileName)

        };

        var path = Path.Combine(_filesPath, image.FileName);

        using var stream = File.Create(path);
        await image.CopyToAsync(stream, cancellationToken);

        return uploadedFile;
    }


}