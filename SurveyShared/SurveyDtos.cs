using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

namespace SurveyShared
{
    public class SurveyDtos
    {
        public class SurveyDto
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public List<QuestionDto> Questions { get; set; } = new();
        }
        public class QuestionDto
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public List<OptionDto> Options { get; set; } = new List<OptionDto>();
            public QuestionType QuestionType { get; set; }

            public int Order { get; set; }

        }

        public class OptionDto
        {
            public int Id { get; set; }
            public string? OptionText { get; set; }
        }

        public class AnswerListDto
        {
            public int SurveyId { get; set; }
            public List<AnswerDto> Answers { get; set; } = new();
        }
      
        public class AnswerDto
        {
            public int AnswerId { get; set; }
            public int QuestionId { get; set; }

            public QuestionType Type { get; set; }
            public string? TextAnswer { get; set; }
            public List<string>? SelectedOptions { get; set; }
            public int? NumericAnswer { get; set; }
           
        }

      

    }
}
