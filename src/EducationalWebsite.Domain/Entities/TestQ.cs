using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Common;
using EducationalWebsite.Domain.Entities.Questions;

namespace EducationalWebsite.Domain.Entities
{
    public class TestQ : BaseEntity
    
    {
        public string Title { get; set; }
        public List<Question> Questions { get; set; }
        public Guid UserId { get; set; }
        public string Result { get; set; }
    }
}