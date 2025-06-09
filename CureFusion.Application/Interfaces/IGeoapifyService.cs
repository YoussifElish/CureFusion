using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CureFusion.Domain.Abstactions;
using CureFusion.Domain.Entities;

namespace CureFusion.Application.Interfaces;
public interface IGeoapifyService
{
    Task<PageinatedList<Hospital>> GetNearbyHospitalsAsync(double latitude, double longitude, int radius = 5000, int pageNumber = 1, int pageSize = 10);
}
