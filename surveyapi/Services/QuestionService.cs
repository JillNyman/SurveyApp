using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using surveyapi.Data;
using surveyapi.Models;



namespace surveyapi.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly SurveyContext _context;

        public QuestionService(SurveyContext context)
        {
            _context = context;
        }

        //Hämta fråga baserat på id | GET: api/Question/{id}
        public async Task<Question?> GetQuestionAsync(int id)
        {
            return await _context.Questions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
               
        }

        //Hämta samtliga frågor | GET api/Question
        public async Task<List<Question>> GetQuestionsAsync()
        {
            return await _context.Questions
                .Include(q => q.Options)
                .ToListAsync();
        }

        //SKAPA FRÅGA | POST api/question
        public async Task CreateQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
           
        }

        //Uppdatera befintlig fråga | PUT Question/questionId
        public async Task<bool> UpdateQuestionAsync(Question question)
        {
            //Hämtar befintlig fråga från databas
            var existingQuestion = await _context.Questions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == question.Id);

            if (existingQuestion == null)
            {
                return false;
            }

            //Uppdatera grundläggande frågeinformation
            _context.Entry(existingQuestion).CurrentValues.SetValues(question);

            //Ta bort alternativ
            var optionsToDelete = existingQuestion.Options
                .Where(o => !question.Options.Any(qo => qo.Id == o.Id))
                .ToList();

            _context.Options.RemoveRange(optionsToDelete);

            existingQuestion.Options.Clear();
            
                //Lägg till/uppdatera nya options
                foreach (var option in question.Options)
                {
                    var existingOption = existingQuestion.Options
                        .FirstOrDefault(o => o.Id == option.Id);

                    if(existingOption != null)
                    {
                        //Uppdatera befintligt alternativ
                        existingOption.Text = option.Text;
                    }
                    else
                    {
                        //Lägg till nytt alternativ
                        existingQuestion.Options.Add(new Option
                        {
                            Text = option.Text,
                            QuestionId = existingQuestion.Id
                        });
                    }
                        
                }
            await _context.SaveChangesAsync(); //spara
            return true;
        }
           
          
        

        //Lägg till ett alternativ till en fråga | POST: api/Question/questionId/options
        public async Task<Option?> AddOptionAsync(int questionId, Option option)
        {
            var question = await _context.Questions.Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == questionId);
            if(question == null)
            {  
                return null; 
            }

            option.QuestionId = questionId;
            _context.Options.Add(option);
            await _context.SaveChangesAsync();
            return option;
        }

        //Ta bort fråga baserat på ID | DELETE: api/question/{id}
        public async Task<bool> DeleteQuestionAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return false;
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
