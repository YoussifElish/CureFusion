using CureFusion.Abstactions;
using CureFusion.Contracts.Medicine;
using Vonage.Voice;
using Mapster;

using CureFusion.Errors;
using CureFusion.Entities;
using Microsoft.EntityFrameworkCore;

namespace CureFusion.Services;

public class DrugService(ApplicationDbContext Context) : IDrugService
{
    private readonly ApplicationDbContext _context = Context;

    public async Task<Result<DrugResponse>> AddDrugAsync(DrugRequest request, CancellationToken cancellationToken)
    {
        var isexsist=await _context.Drugs.AnyAsync(x=>x.Name==request.Name);
        if (isexsist)
            return Result.Failure<DrugResponse>(DrugError.Duplicatedrug);
        var drug = request.Adapt<Drug>();

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


    public async Task<Result<IEnumerable<DrugResponse>>> GetAllDrugAsync(CancellationToken cancellationToken)

    {
        var drugs = await _context.Drugs.AsNoTracking().ToListAsync(cancellationToken);
        return drugs is not null ? Result.Success(drugs.Adapt<IEnumerable<DrugResponse>>()) : Result.Failure<IEnumerable<DrugResponse>>(DrugError.DrugNotFOund);

    }

    public async Task<Result<DrugResponse>> GetDrugAsync(int id, CancellationToken cancellationToken)
    {
       var drug=await _context.Drugs.FindAsync(id, cancellationToken);
        return drug is not null ? Result.Success(drug.Adapt<DrugResponse>()) : Result.Failure<DrugResponse>(DrugError.DrugNotFOund);
    }

    public async Task<Result> UpdateDrugAsync(int id, DrugRequest request, CancellationToken cancellationToken)
    {
        var isexsist = await _context.Drugs.AnyAsync(x => x.Name == request.Name&&x.Id!=id);
        if(isexsist)
        return Result.Failure<DrugResponse>(DrugError.Duplicatedrug);
        var UpdatedDrug=await _context.Drugs.FindAsync(id);
        if(UpdatedDrug is  null)
            return Result.Failure<DrugResponse>(DrugError.DrugNotFOund);

        UpdatedDrug.Name= request.Name;
        UpdatedDrug.Dosage= request.Dosage;
        UpdatedDrug.Interaction= request.Interaction;
        UpdatedDrug.SideEffect= request.SideEffect;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();

        //check it again 

            
    }
}
