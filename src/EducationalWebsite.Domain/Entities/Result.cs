using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Common;
using MongoDB.Bson;

namespace EducationalWebsite.Domain.Entities
{
    public class Result : BaseEntity
    {
    [Required]
    public Guid TestQId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public double Score { get; set; }

    public string? Comments { get; set; } 
        
    }
}