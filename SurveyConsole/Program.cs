using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using SurveyShared;
using surveyapi;

namespace SurveyConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5093/api/") };

            Console.WriteLine("Välkommen till enkätprogrammet!");

            // Ange enkät-ID
            Console.Write("Ange enkätnummer: ");
            if (!int.TryParse(Console.ReadLine(), out int surveyId))
            {
                Console.WriteLine("Felaktigt nummer. Avslutar programmet.");
                return;
            }

            // Hämta enkäten
            var survey = await httpClient.GetFromJsonAsync<SurveyDto>($"Survey/{surveyId}");
            if (survey == null)
            {
                Console.WriteLine("Enkäten hittades inte.");
                return;
            }

            Console.WriteLine($"\nEnkät: {survey.Title}\n");

            var answers = new List<string>();

            // Visa frågor och låt användaren besvara dem
            for (int i = 0; i < survey.Questions.Count; i++)
            {
                var question = survey.Questions[i];
                Console.WriteLine($"{i + 1}. {question.Text}");

                if (question.Type == "Text")
                {
                    Console.Write("Ditt svar: ");
                    answers.Add(Console.ReadLine());
                }
                else if (question.Type == "Checkbox")
                {
                    Console.WriteLine("Markera svar (separera med komma, t.ex. 1,3):");
                    for (int j = 0; j < question.Options.Count; j++)
                    {
                        Console.WriteLine($"  {j + 1}. {question.Options[j].Text}");
                    }
                    var input = Console.ReadLine();
                    answers.Add(input);
                }
                else if (question.Type == "NumericRange")
                {
                    Console.Write("Ange ett värde mellan 1 och 10: ");
                    var numericAnswer = Console.ReadLine();
                    answers.Add(numericAnswer);
                }
            }

            // Skicka in svaren
            var response = await httpClient.PostAsJsonAsync("Survey/Submit", answers);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("\nTack för att du besvarade enkäten!");
            }
            else
            {
                Console.WriteLine("\nNågot gick fel vid inlämning av enkäten.");
            }
        }
    }
}
