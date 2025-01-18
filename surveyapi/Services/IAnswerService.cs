using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using surveyapi.Models;
using static SurveyShared.SurveyDtos;

namespace surveyapi.Services
{
    public interface IAnswerService
    {

        Task SubmitAnswersAsync(List<AnswerDto> answers)
        {
            throw new NotImplementedException();
        }
        Task SubmitAnswerAsync(Answer answer);
        
        Task<bool> DeleteAnswerAsync(int id);

    }
}
