using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using surveyapi.Services;
using surveyapi.Models;
using surveyapi.Data;
using SurveyShared;



namespace surveyapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService  surveyService)
        {
            _surveyService = surveyService;
        }

        //GET: api/survey
        [HttpGet]
        public async Task<ActionResult<List<SurveyDtos.SurveyDto>>> GetSurveys()
        {
            var surveys = await _surveyService.GetAllSurveysAsync();
            var surveyDtos = surveys.Select(survey => new SurveyDtos.SurveyDto
            {
                Id = survey.Id,
                Title = survey.Title,
                Questions = survey.Questions.Select(q => new SurveyDtos.QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Options = q.Options.Select(o => new SurveyDtos.OptionDto
                    {
                        Id = o.Id,
                        OptionText = o.Text
                    }).ToList()
                }).ToList()
            }).ToList();
                return Ok(surveyDtos);
        }

        //GET: api/survey/id
        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyDtos.SurveyDto>> GetSurvey(int id)
        {
            //Hämta enkät från databasen genom att anropa metod från SurveyService
            var survey = await _surveyService.GetSurveyByIdAsync(id);
            if (survey == null)
            {
                return NotFound($"Enkäten med ID {id} hittades inte.");
            }

            //Mappa hämtad enkät till en SurveyDto
            var surveyDto = new SurveyDtos.SurveyDto
            {
                Id = survey.Id,
                Title = survey.Title,
                Questions = survey.Questions.Select(q => new SurveyDtos.QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    QuestionType = q.Type,
                    Options = q.Options.Select(o => new SurveyDtos.OptionDto
                    {
                        Id = o.Id,
                        OptionText = o.Text
                    })
                    .ToList()               
                }).ToList()
            };
            return Ok(surveyDto);

        }     

        // SKAPA NY ENKÄT | POST: api/survey
        [HttpPost]
        public async Task<IActionResult> CreateSurvey([FromBody] Survey survey)
        {
            if (survey == null)
            {
                return BadRequest("Det finns ingen enkät");
            }
            await _surveyService.CreateSurveyAsync(survey);
            
            return CreatedAtAction(nameof(GetSurvey), new { id = survey.Id }, survey);
        }

        //Uppdatera befintlig enkät | PUT Survey/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSurvey(int id, [FromBody] Survey updatedSurvey)
        {
            
            if(updatedSurvey == null)
            {
                return BadRequest("Hittar ingen enkät.");
            }

            if (id != updatedSurvey.Id)
            {
                return BadRequest("Enkät-ID i URL:en ({id}) matchar inte enkät-ID i kroppen ({updatedSurvey.Id}).");
            }

            var existingSurvey = await _surveyService.UpdateSurveyAsync(updatedSurvey);
            if (!existingSurvey)
            {
                return NotFound($"Enkäten med ID {id} finns inte");
            }
            
            return NoContent();
        }

        // DELETE: api/survey/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurvey(int id)
        {
            var survey = await _surveyService.DeleteSurveyAsync(id);
                
            if (!survey)
            {
                return NotFound($"Enkät med ID {id} hittades inte.");
            }

            return NoContent();
        }

       
    }
}
