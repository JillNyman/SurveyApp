namespace SurveyWeb.Models
{
    public class Answer
    {
        public int QuestionId { get; set; }
        public List<string>? MultipleChoiceAnswers { get; set; }
        public int? NumericAnswer { get; set; }
        public string? TextAnswer { get; set; }
    }
}
