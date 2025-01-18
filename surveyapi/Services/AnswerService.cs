using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using surveyapi.Data;
using surveyapi.Models;
using static SurveyShared.SurveyDtos;


namespace surveyapi.Services
{
    public class AnswerService: IAnswerService
    {
        private readonly SurveyContext _context;

        public AnswerService(SurveyContext context)
        {
            _context = context;
        }

        public async Task SubmitAnswersAsync(List<Answer> answers)
        {
            foreach(var answer in answers)
            {
                _context.Answers.Add(answer);
            }
            /*foreach (var answer in answers)
            {
                var dbAnswer = new Answer
                {
                    SurveyId = answer.SurveyId,
                    QuestionId = answer.QuestionId,
                    TextAnswer = answer.TextAnswer,
                    SelectedOptions = answer.SelectedOptions,
                    NumericAnswer = answer.NumericAnswer
                };

                _context.Answers.Add(dbAnswer);
            }*/
            await _context.SaveChangesAsync();
        }

        // Skicka in ett svar
        public async Task SubmitAnswerAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
          
        }

        
        //Radera svar kopplade till en fråga
        public async Task<bool> DeleteAnswerAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return false;
            }

            _context.Answers.Remove(answer); //markera som borttagen
            await _context.SaveChangesAsync(); //spara ändring till databas
            
            return true;
        }
    }
}
