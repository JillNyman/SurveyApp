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

        //Den hämtade enkäten förvaras här
        public SurveyDtos.SurveyDto? Survey { get; set; }

        //Listan lagrar alla svar
        [BindProperty] public List<SurveyDtos.AnswerDto> Answers { get; set; } = new List<SurveyDtos.AnswerDto>();

      
        //GET: hämta hela den valda enkäten i en request
        public async Task<IActionResult> OnGetAsync(int surveyId)
        {
            //Hämta hela enkäten
                       Survey = await _httpClient.GetFromJsonAsync<SurveyDtos.SurveyDto>($"Survey/{surveyId}");
        
            if (Survey == null)
            {
                //Om enkäten inte finns, returnera till startsidan
                return NotFound("Enkäten hittades inte.");

            }

            Answers = new List<SurveyDtos.AnswerDto>();

            //Förbered en lista för svar
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

        //POST: skickar alla svar till databasen när användaren har fyllt i enkäten
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
                    ModelState.AddModelError("", $"Misslyckades med att skicka in svaret med id {answer.AnswerId}. Försök igen");
                    return Page();
                }
            }

            return RedirectToPage("/Survey/Complete");
        }
    }
}
