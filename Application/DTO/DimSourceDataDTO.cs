using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class DimSourceDataDTO
    {
        public IEnumerable<CSVClienteDTO> ClienteDTOs { get; set; } = Enumerable.Empty<CSVClienteDTO>();
        public IEnumerable<CSVProductoDTO> ProductoDTO { get; set; } = Enumerable.Empty<CSVProductoDTO>();
        public IEnumerable<CSVCategoriaDTO> CategoriaDTO { get; set; } = Enumerable.Empty<CSVCategoriaDTO>();
        public IEnumerable<CSVFuentesDTO> FuentesDTO { get; set; } = Enumerable.Empty<CSVFuentesDTO>();
    }
}
