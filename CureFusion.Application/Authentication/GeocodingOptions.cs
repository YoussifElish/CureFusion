using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace CureFusion.Application.Authentication;
public class GeocodingOptions
{
    public string ApiKey { get; set; } = null!;
}
