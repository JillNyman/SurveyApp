using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using SurveyShared;
using System.Text.Json;


namespace SurveyWeb.Pages.Survey
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("surveyapi");
        }

        [BindProperty] public SurveyDtos.SurveyDto? Survey {  get; set; }
        [BindProperty] public List<string> Answers { get; set; } = new();
        [BindProperty] public int SurveyId { get; set; }

        [BindProperty] public List<SurveyDtos.SurveyDto>? Surveys { get; set; } = new List<SurveyDtos.SurveyDto>();

        public async Task OnGetAsync()
        {
            //Hämta alla enkäter 
            Surveys = await _httpClient
                .GetFromJsonAsync<List<SurveyDtos.SurveyDto>>("Survey")
                    ?? new List<SurveyDtos.SurveyDto>();
           
        }                  
    }      
    
}
