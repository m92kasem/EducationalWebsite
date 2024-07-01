using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EducationalWebsite.Domain.Entities.Questions
{
    public class Question : BaseEntity
    {
        [Required(ErrorMessage = "The question text is required.")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Options are required.")]
        [MinLength(2, ErrorMessage = "There must be at least two options.")]
        public ICollection<string> Options { get; set; } = new List<string>();

        [Required(ErrorMessage = "The correct answer is required.")]
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}