using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace surveyapi.Models
{
    public class Survey
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Question> Questions { get; set; } = new();
    }
}
