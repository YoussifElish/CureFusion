using CureFusion.Abstactions;
using CureFusion.Contracts.Files;
using CureFusion.Contracts.Medicine;

namespace CureFusion.Services;

public interface IDrugService
{
    public Task<Result<IEnumerable<DrugResponse>>> GetAllDrugAsync(CancellationToken cancellationToken);
    public Task<Result<DrugResponse>> GetDrugAsync(int id,CancellationToken cancellationToken);
    public Task<Result<DrugResponse>> AddDrugAsync(DrugRequest request, UploadImageRequest drugImage, CancellationToken cancellationToken);
    public Task<Result> UpdateDrugAsync(int id, DrugRequest request,CancellationToken cancellationToken);
    public Task<Result> DeleteDrugAsync(int id, CancellationToken cancellationToken);
}
