using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using surveyapi.Models;

namespace surveyapi.Services
{
    public interface IQuestionService
    {
        Task<Question?> GetQuestionAsync(int questionId);
        Task<List<Question>> GetQuestionsAsync();
        Task CreateQuestionAsync(Question question);
        Task<bool> UpdateQuestionAsync(Question question);
        Task<Option?> AddOptionAsync(int questionId, Option option);
        Task<bool> DeleteQuestionAsync(int id);
    }
}
