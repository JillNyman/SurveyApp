using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace surveyapi.Models
{
    public class Option
    {
        public int Id { get; set; } // Unikt ID för alternativet

        [Required]
        public string Text { get; set; } = string.Empty; // Text för alternativet

        public int QuestionId { get; set; } // Foreign key till Question
        [JsonIgnore] public Question? Question { get; set; } // Navigationsfält för relaterad fråga

    }
}
