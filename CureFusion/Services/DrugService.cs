using CureFusion.Abstactions;
using CureFusion.Contracts.Medicine;
using Mapster;
using CureFusion.Errors;
using CureFusion.Entities;
using Microsoft.EntityFrameworkCore;
using CureFusion.Contracts.Files;
using RealState.Services;
using CureFusion.Contracts.Articles;

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
        await _context.AddAsync(drug, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(drug.Adapt<DrugResponse>());

    }

    public async Task<Result> DeleteDrugAsync(int id, CancellationToken cancellationToken)
    {
        var isexsist = await _context.Drugs.FindAsync(id);
        if (isexsist is null)
            return Result.Failure(DrugError.DrugNotFOund);
        _context.Remove(isexsist);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    public async Task<Result<IEnumerable<DrugResponse>>> GetAllDrugsAsync(DrugQueryParameters queryParams, CancellationToken cancellationToken)
    {
        // إنشاء الاستعلام الأساسي
        var query = _context.Drugs.AsNoTracking();

        // Filtering
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            query = query.Where(d => d.Name.ToLower().Contains(queryParams.SearchTerm.ToLower()) ||
                                     d.Dosage.ToLower().Contains(queryParams.SearchTerm.ToLower()) ||
                                     d.SideEffect.ToLower().Contains(queryParams.SearchTerm.ToLower()));
        }


        // Sorting
          

                query = query.OrderBy(x=>x.Name);

        // Pagination
        var totalItems = await query.CountAsync(cancellationToken);
        var drugs = await query
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync(cancellationToken);

        // تحويل الأدوية إلى DTOs
        var response = drugs.Select(drug => new DrugResponse(
        drug.Id,
        drug.Name,
        drug.Dosage,
        drug.Interaction,
        drug.SideEffect,
        drug.DrugImage != null ? $"{_filesPath}/{drug.DrugImage.StoredFileName}" : null
    ));

        // إرجاع النتيجة
        return Result.Success(response);
    }

    public async Task<Result<DrugResponse>> GetDrugAsync(int id, CancellationToken cancellationToken)
    {
        var drug = await _context.Drugs.FindAsync(id, cancellationToken);
        return drug is not null ? Result.Success(drug.Adapt<DrugResponse>()) : Result.Failure<DrugResponse>(DrugError.DrugNotFOund);
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
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();

        //check it again 


    }


}
