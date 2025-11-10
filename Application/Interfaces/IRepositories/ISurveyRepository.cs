using Domain.Entities.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface ISurveyRepository : IFileReaderRepository<ESurveys>
    {
    }
}
