using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISurveys
    {
        public Task<OperationResult> GetSurveysAsync();
    }
}
