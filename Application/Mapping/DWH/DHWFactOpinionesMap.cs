using Application.DTO;
using Domain.Entities.CSV;

namespace Application.Mapping.DWH
{
    public class DHWFactOpinionesMap
    {
        public static FactOpinionesDTO FactDTOMap(ESurveys survey)
        {
            return new FactOpinionesDTO
            {
                ID_Opiniones = survey.ID_Opiniones,
                Comentario = survey.Comentario,
                ClasificacionRaw = survey.ClasificacionRaw,
                PuntajeSatisfaccion = survey.PuntajeSatisfaccion,
                FechaCarga = survey.FechaCarga,
                FK_Clientes = survey.FK_Clientes,
                FK_Producto = survey.FK_Producto,
                ID_FuenteDatos = survey.Fuente
            };
        }
    }
}
