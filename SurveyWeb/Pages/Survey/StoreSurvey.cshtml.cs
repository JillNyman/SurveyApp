using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using SurveyShared;

namespace SurveyWeb.Pages.Survey
{
    public class StoreSurveyModel : PageModel
    {
        private readonly HttpClient _httpClient;

        //Konstruktor
        public StoreSurveyModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("surveyapi");
        }

        //Den h�mtade enk�ten f�rvaras h�r
        public SurveyDtos.SurveyDto? Survey { get; set; }

        //Listan lagrar alla svar
        [BindProperty] public List<SurveyDtos.AnswerDto> Answers { get; set; } = new List<SurveyDtos.AnswerDto>();

      
        //GET: h�mta hela den valda enk�ten i en request
        public async Task<IActionResult> OnGetAsync(int surveyId)
        {
            //H�mta hela enk�ten
                       Survey = await _httpClient.GetFromJsonAsync<SurveyDtos.SurveyDto>($"Survey/{surveyId}");
        
            if (Survey == null)
            {
                //Om enk�ten inte finns, returnera till startsidan
                return NotFound("Enk�ten hittades inte.");

            }

            Answers = new List<SurveyDtos.AnswerDto>();

            //F�rbered en lista f�r svar
            foreach (var question in Survey.Questions)
            {
                var answerDto = new SurveyDtos.AnswerDto
                {
                    QuestionId = question.Id,
                    TextAnswer = string.Empty,
                    NumericAnswer = null,
                    SelectedOptions = new List<string>()
                };
               
               Answers.Add(answerDto);

            }
            return Page();
        }

        //POST: skickar alla svar till databasen n�r anv�ndaren har fyllt i enk�ten
        public async Task<IActionResult> OnPostAsync()
        {
            //Kontrollera att modell-bindingen fungerar
            if (!ModelState.IsValid)
            {
                return Page();
            }

            foreach (var answer in Answers)
            { 
                var response = await _httpClient.PostAsJsonAsync("Answer", answer);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", $"Misslyckades med att skicka in svaret med id {answer.AnswerId}. F�rs�k igen");
                    return Page();
                }
            }

            return RedirectToPage("/Survey/Complete");
        }
    }
}
