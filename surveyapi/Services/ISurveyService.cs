using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using surveyapi.Models;

namespace surveyapi.Services
{
    public interface ISurveyService
    {
        Task<Survey?> GetSurveyByIdAsync(int id);
        Task<List<Survey>> GetAllSurveysAsync();
        Task CreateSurveyAsync(Survey survey);
        Task<bool> UpdateSurveyAsync(Survey survey);
        Task<bool> DeleteSurveyAsync(int id);
    }
}
