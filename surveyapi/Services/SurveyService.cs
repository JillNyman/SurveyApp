using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using surveyapi.Data;
using surveyapi.Models;
    

    namespace surveyapi.Services
    {
        public class SurveyService : ISurveyService
        {
            private readonly SurveyContext _context;

        public SurveyService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<Survey?> GetSurveyByIdAsync(int id)
        {
            return await _context.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task<List<Survey>> GetAllSurveysAsync()
        {
            return await _context.Surveys
                .Include(s => s.Questions)
                .ToListAsync();
        }


        // Skapa en ny enkät | POST: api/survey
        public async Task CreateSurveyAsync(Survey survey)
            {
                _context.Surveys.Add(survey);
                await _context.SaveChangesAsync();
                
            }

        //Uppdatera befintlig enkät | PUT Survey/id
        public async Task<bool> UpdateSurveyAsync(Survey survey)
        {
            var existingSurvey = await _context.Surveys
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.Id == survey.Id);

            if (existingSurvey == null)
            {
                return false;
            }

            _context.Entry(existingSurvey).CurrentValues.SetValues(survey);

            //Hantera frågor i enkäten
            var questionsToDelete = existingSurvey.Questions
                .Where(q => !survey.Questions.Any(sq => sq.Id == q.Id))
                .ToList();

            _context.Questions.RemoveRange(questionsToDelete);

            foreach (var question in survey.Questions)
            {
                var existingQuestion = existingSurvey.Questions
                    .FirstOrDefault(q => q.Id == question.Id);

                if (existingQuestion != null)
                {
                    _context.Entry(existingQuestion).CurrentValues.SetValues(question);
                }
                else 
                { 
                    existingSurvey.Questions.Add(question);
                }
            }
            
            await _context.SaveChangesAsync(); //spara
            return true;
        }

        public async Task<bool> DeleteSurveyAsync(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
             
            if (survey == null)
            {
                return false;
            }
            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
            return true;


        }
       }
    }


