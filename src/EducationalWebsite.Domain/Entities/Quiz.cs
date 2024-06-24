using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Common;
using EducationalWebsite.Domain.Entities.Questions;
using MongoDB.Bson.Serialization.Attributes;

namespace EducationalWebsite.Domain.Entities
{
    public class Quiz : BaseEntity
    {
        [Required(ErrorMessage = "The title is required.")]
        [StringLength(100, ErrorMessage = "The title cannot exceed 100 characters.")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "At least one question is required.")]
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        [BsonIgnore]
        public TestQ TestQ { get; set; }
    }
}