using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using surveyapi.Services;
using surveyapi.Models;
using SurveyShared;
using surveyapi.Migrations;
using static SurveyShared.SurveyDtos;


namespace surveyapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> SubmitAnswers([FromBody] List<SurveyDtos.AnswerDto> answers)
        {
            if (answers == null)
            {
                return BadRequest("Misslyckades skicka in svar.");
            }

            var answerEntities = answers.Select(a => new Answer
            {
                QuestionId = a.QuestionId,
                TextAnswer = a.TextAnswer,
                NumericAnswer = a.NumericAnswer,
                SelectedOptions = a.SelectedOptions,
               
            }).ToList();

            await _answerService.SubmitAnswersAsync(answers);
            return Ok("Svaren har skickats in.");
           
        }

      

        [HttpPost] //Posta svar till databas
        public async Task<IActionResult> SubmitAnswer([FromBody] SurveyDtos.AnswerDto answerDto)
        {
            if (answerDto == null)
            {
                return BadRequest("Svaret kan inte vara tomt.");
            }

         
            //Koppla answerDto till answer
            var answer = new Answer
            {
                QuestionId = answerDto.QuestionId,
                TextAnswer = answerDto.TextAnswer,
                NumericAnswer = answerDto.NumericAnswer,
                SelectedOptions = answerDto.SelectedOptions,
                Type = answerDto.Type
            };

            //Skicka svaret och spara
            await _answerService.SubmitAnswerAsync(answer);

            //returnera sparat svar
            return Ok("Svaret har skickats in"); 
        }

        //DELETE: api/answer/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var success = await _answerService.DeleteAnswerAsync(id);
            if (!success)
            {
                return NotFound($"Svar med ID {id} finns inte.");
            }


            return NoContent();
        }
    }
}
