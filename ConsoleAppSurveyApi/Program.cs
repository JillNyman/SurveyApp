using System;
using System.Net.Http;
using System.Net.Http.Json;
using SurveyShared;
using static SurveyShared.SurveyDtos;
using surveyapi;
using surveyapi.Migrations;


namespace SurveyConsoleApp
{
    public class Program
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

            if (survey.Questions == null || survey.Questions.Count == 0)
            {
                Console.WriteLine("Den valda enkäten innehåller inga frågor.");
                return;
            }

            Console.WriteLine($"\nEnkät: {survey.Title}\n");

            var answers = new AnswerListDto { SurveyId = surveyId };

            // Do...while-loop för att ställa frågor
            int questionIndex = 0;

            try
            {
                do
                {
                    if (survey.Questions == null || survey.Questions.Count == 0)
                    {
                        Console.WriteLine("Det finns inga frågor i enkäten.");
                        break;
                    }

                    var question = survey.Questions[questionIndex];
                    Console.WriteLine($"{questionIndex + 1}. {question.Text}");

                    if (question.QuestionType == QuestionType.TextAnswer)
                    {
                        Console.Write("Ditt svar: ");
                        var textAnswer = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(textAnswer))
                        {
                            answers.Answers.Add(new AnswerDto
                            {
                                QuestionId = question.Id,
                                TextAnswer = textAnswer
                            });
                        }
                        else
                        {
                            Console.WriteLine("Du måste ange ett svar.");
                            continue;
                        }
                    }
                    else if (question.QuestionType == QuestionType.MultipleChoice)
                    {
                        Console.WriteLine("Markera svar (separera med komma, t.ex. 1,3):");
                        for (int j = 0; j < question.Options?.Count; j++)
                        {
                            Console.WriteLine($"  {j + 1}. {question.Options[j].OptionText}");
                        }

                        var input = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            var selectedOptions = input.Split(',')
                                                       .Select(option => question.Options?[int.Parse(option) - 1].OptionText)
                                                       .Where(option => option != null)
                                                       .Cast<string>()
                                                       .ToList();

                            answers.Answers.Add(new AnswerDto
                            {
                                QuestionId = question.Id,
                                SelectedOptions = selectedOptions
                            });
                        }
                        else
                        {
                            Console.WriteLine("Du måste ange minst ett alternativ.");
                            continue;
                        }
                    }
                    else if (question.QuestionType == QuestionType.NumericRange)
                    {
                        Console.Write("Ange ett värde mellan 1 och 10: ");
                        var numericAnswer = Console.ReadLine();
                        if (int.TryParse(numericAnswer, out int numericValue) && numericValue >= 1 && numericValue <= 10)
                        {
                            answers.Answers.Add(new AnswerDto
                            {
                                QuestionId = question.Id,
                                NumericAnswer = numericValue
                            });
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt värde. Ange ett nummer mellan 1 och 10.");
                            continue;
                        }
                    }

                    questionIndex++;

                    if (answers.Answers.Count == survey.Questions.Count)
                    {
                        // När alla frågor är besvarade, gör POST-anropet
                        var response = await httpClient.PostAsJsonAsync("answer/list", answers);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("\nTack för att du besvarade enkäten!");
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"\nNågot gick fel vid inlämning av enkäten. Felmeddelande: {errorContent}");
                        }
                        break;
                    }
                } while (true);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Ett fel inträffade: {ex.Message}");
            }

          
        }
    }
}

