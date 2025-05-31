using CureFusion.API.Helpers;
using CureFusion.Application.Contracts.Articles;
using CureFusion.Application.Contracts.Files;
using CureFusion.Application.Contracts.Medicine;
using CureFusion.Domain.Abstactions;

namespace CureFusion.Application.Services;

public interface IDrugService
{
    Task<Result<PaginatedResult<DrugResponse>>> GetAllDrugsAsync(DrugQueryParameters queryParams, CancellationToken cancellationToken);
    public Task<Result<DrugResponse>> GetDrugAsync(int id, CancellationToken cancellationToken);
    public Task<Result<DrugResponse>> AddDrugAsync(DrugRequest request, UploadImageRequest drugImage, CancellationToken cancellationToken);
    public Task<Result> UpdateDrugAsync(int id, DrugRequest request, CancellationToken cancellationToken);
    public Task<Result> DeleteDrugAsync(int id, CancellationToken cancellationToken);
}
