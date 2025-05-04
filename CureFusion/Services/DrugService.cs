using CureFusion.Abstactions;
using CureFusion.Contracts.Medicine;
using Mapster;
using CureFusion.Errors;
using CureFusion.Entities;
using Microsoft.EntityFrameworkCore;
using CureFusion.Contracts.Files;
using RealState.Services;
using CureFusion.Contracts.Articles;
using CureFusion.Helpers;

namespace CureFusion.Services;

public class DrugService(ApplicationDbContext Context, IFileService fileService) : IDrugService
{
    private readonly ApplicationDbContext _context = Context;
    private readonly IFileService _fileService = fileService;
    private readonly string _filesPath = $"https://curefusion2.runasp.net/Uploads";

    public async Task<Result<DrugResponse>> AddDrugAsync(DrugRequest request, UploadImageRequest drugImage, CancellationToken cancellationToken)
    {
        var isexsist = await _context.Drugs.AnyAsync(x => x.Name == request.Name);
        if (isexsist)
            return Result.Failure<DrugResponse>(DrugError.Duplicatedrug);

        var image = await _fileService.UploadImagesAsync(drugImage.Image, cancellationToken);
        var drug = request.Adapt<Drug>();
        drug.DrugImageId = image.Id;
        drug.DrugImage = image;
        await _context.AddAsync(drug, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(drug.Adapt<DrugResponse>());

    }

    public async Task<Result> DeleteDrugAsync(int id, CancellationToken cancellationToken)
    {
        var isexsist = await _context.Drugs.FindAsync(id);
        if (isexsist is null)
            return Result.Failure(DrugError.DrugNotFOund);
        var drugImage = await _context.UploadedFiles.FindAsync(isexsist.DrugImageId);
        var drugRemider = await _context.DrugReminders.Where(x => x.DrugId == id).ToListAsync();
        if (drugRemider is not null)
            _context.RemoveRange(drugRemider);
        _context.Remove(isexsist);
        _context.Remove(drugImage);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    public async Task<Result<PaginatedResult<DrugResponse>>> GetAllDrugsAsync(DrugQueryParameters queryParams, CancellationToken cancellationToken)
    {
        var query = _context.Drugs.Include(x=>x.DrugImage).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            var term = queryParams.SearchTerm.ToLower();
            query = query.Where(d =>
                d.Name.ToLower().Contains(term) ||
                d.Dosage.ToLower().Contains(term) ||
                d.SideEffect.ToLower().Contains(term));
        }

        query = query.OrderBy(x => x.Name);

        var totalItems = await query.CountAsync(cancellationToken);

        var drugs = await query
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync(cancellationToken);

     var response = drugs.Adapt<List<DrugResponse>>();


        var paginatedResult = new PaginatedResult<DrugResponse>(
            response,
            totalItems,
            queryParams.PageNumber,
            queryParams.PageSize
        );

        return Result.Success(paginatedResult);
    }

    public async Task<Result<DrugResponse>> GetDrugAsync(int id, CancellationToken cancellationToken)
    {
        var drug = await _context.Drugs.Include(x=>x.DrugImage).Where(x=>x.Id == id).FirstOrDefaultAsync();
        var response = drug.Adapt<DrugResponse>();
        return response is not null ? Result.Success(response) : Result.Failure<DrugResponse>(DrugError.DrugNotFOund);
    }

    public async Task<Result> UpdateDrugAsync(int id, DrugRequest request, CancellationToken cancellationToken)
    {
        var isexsist = await _context.Drugs.AnyAsync(x => x.Name == request.Name && x.Id != id);
        if (isexsist)
            return Result.Failure<DrugResponse>(DrugError.Duplicatedrug);
        var UpdatedDrug = await _context.Drugs.FindAsync(id);
        if (UpdatedDrug is null)
            return Result.Failure<DrugResponse>(DrugError.DrugNotFOund);

        UpdatedDrug.Name = request.Name;
        UpdatedDrug.Dosage = request.Dosage;
        UpdatedDrug.Interaction = request.Interaction;
        UpdatedDrug.SideEffect = request.SideEffect;
        UpdatedDrug.Description = request.Description;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();

        //check it again 


    }


}
