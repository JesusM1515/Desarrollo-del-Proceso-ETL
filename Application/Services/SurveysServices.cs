using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Domain.Entities.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SurveysServices : ISurveys
    {
        private readonly ISurveyRepository _ISurveyRepository;
        private readonly ILogger<SurveysServices> _Logger;

        public SurveysServices(ISurveyRepository iSurveyRepository, ILogger<SurveysServices> logger) 
        { 
            this._ISurveyRepository = iSurveyRepository;
            this._Logger = logger;
        }
        async Task<OperationResult> ISurveys.GetSurveysAsync()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
