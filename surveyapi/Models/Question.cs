using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json;
using System.Text.Json.Serialization;
using SurveyShared;
using Swashbuckle.AspNetCore.Annotations; 



namespace surveyapi.Models
{
    public class Question
    {
        public int Id { get; set; } // Unikt ID för frågan, genereras automatiskt

        [Required] public string Text { get; set; } = string.Empty; // Frågetexten

        [Required] public QuestionType Type { get; set; } // Typ av fråga: text, multiple choice, numeric range etc.

        public int Order { get; set; }
        
        public int SurveyId { get; set; } // Foreign key, enkäten som frågan tillhör
        [JsonIgnore]
        public Survey? Survey { get; set; } //navigationsfält för Survey

        public List<Option> Options { get; set; } = new List<Option>();

        [Timestamp][JsonIgnore] public byte[]? RowVersion { get; set; }
    }

   
}

  
