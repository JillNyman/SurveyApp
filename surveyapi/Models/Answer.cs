using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SurveyShared;

namespace surveyapi.Models
{
    public class Answer
    {
        public int Id { get; set; } //Unikt id för svaret
        public int QuestionId { get; set; } //frågans id
        public int SurveyId { get; set; } //Enkätens id
         
        public QuestionType Type { get; set; } //Type används också i klassen question vilket förenklar hantering av frågor/svar
        public string? TextAnswer { get; set; }  //användarens textsvar
        
        public int? NumericAnswer { get; set; } //När svaret ska vara numeriskt, t ex NumericRange

        public List<string>? SelectedOptions { get; set; } //Svar på multiple-choicefråga
    }
}
