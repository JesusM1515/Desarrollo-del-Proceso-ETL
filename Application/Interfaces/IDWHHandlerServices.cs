using Application.DTO;
using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDWHHandlerServices
    {
        Task<OperationResult<DimSourceDataDTO>> LoadDataWarehouseAsync();

        //Task LoadDataWarehouseAsync();
    }
}
