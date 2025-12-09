using Application.DTO;
using Domain.Entities.Base;

namespace Application.Interfaces
{
    public interface ISurveys
    {
        public Task<OperationResult<IEnumerable<CSVSurveyDTO>>> GetSurveysAllAsync();
    }
}
