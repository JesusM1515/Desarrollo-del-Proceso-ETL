using Application.DTO;
using Domain.Entities.DWH.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IDWHRepository
    {
        Task SaveProcessedDimensionsAsync(DimSourceDataDTO data);
    }
}
