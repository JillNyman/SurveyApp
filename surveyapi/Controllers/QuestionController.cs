using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Microsoft.AspNetCore.Mvc;
using surveyapi.Services;
using surveyapi.Models;
using SurveyShared;

namespace surveyapi.Controllers
{
   
   
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }


        //HÄMTA SPECIFIK FRÅGA | GET: api/Question/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyDtos.QuestionDto>> GetQuestion(int id)
        {
            //Hämta enstaka fråga baserat på id
            var question = await _questionService.GetQuestionAsync(id); 
            if (question == null)
            {
                return NotFound($"Fråga med ID {id} hittades inte.");
            }

           

            //Omvandla till DTO
            var questionDto = new SurveyDtos.QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                QuestionType = question.Type,
                    Options = question.Options.Select(o => new SurveyDtos.OptionDto
                    {
                        Id = o.Id,
                        OptionText = o.Text
                    }).ToList()
            };

            return Ok(questionDto);
        }

        //HÄMTA ALLA FRÅGOR | GET: api/Question
        [HttpGet]
        public async Task<ActionResult<List<SurveyDtos.QuestionDto>>> GetQuestions()
        {
            //Hämta alla frågor från Questions-tabellen
            var questions = await _questionService.GetQuestionsAsync(); 
            if(questions == null)
            {
                return NotFound($"Det finns inga frågor i databasen.");
            }

            var questionDtos = questions.Select(q => new SurveyDtos.QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                QuestionType = q.Type,
                Options = q.Options.Select(o => new SurveyDtos.OptionDto
                {
                    Id = o.Id,
                    OptionText = o.Text
                }).ToList()
            }).ToList();

            return Ok(questionDtos);
        }

        
        // SKAPA FRÅGA | POST: api/Question
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] Question question)
        {        
            if(question == null)
            {
                return BadRequest("Frågan är obligatorisk.");
            }

            await _questionService.CreateQuestionAsync(question); 
            
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
        }


     

        //PUT /Question/questionId
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] Question updatedQuestion)
        {
            //kontrollera id
            if (updatedQuestion == null)
            {
                return BadRequest("Obligatoriskt fält.");
            }
            if (id != updatedQuestion.Id)
            {
                return BadRequest($"Fråge-ID i URL:en ({id}) matchar inte fråge-ID i kroppen ({updatedQuestion.Id}).");
            }


            //Anropa tjänsten för att uppdatera frågan
                var success = await _questionService.UpdateQuestionAsync(updatedQuestion);
                if(!success)
                {
                    return NotFound($"Fråga med ID {id} kunde inte uppdateras.");
                }
         
            
          //Returnerar ingenting om uppdateringen lyckades
                return NoContent();
           
        }

        //Lägg till alternativ, POST: api/Question/questionId/options
        [HttpPost("{questionId}/options")]
        public async Task<ActionResult<Option>> AddOptionToQuestion(int questionId, Option option)
        {
            var addedOption = await _questionService.AddOptionAsync(questionId, option);
            if (addedOption == null)
            {
                return NotFound($"Frågan med ID {questionId} finns inte.");
            }


            return Ok(addedOption);
        }

        //DELETE: api/question/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _questionService.DeleteQuestionAsync(id); 
            if (!question)
            {
                return NotFound($"Hittar inte frågan med ID {id}.");
            }

            return NoContent();
        }
    }
}
