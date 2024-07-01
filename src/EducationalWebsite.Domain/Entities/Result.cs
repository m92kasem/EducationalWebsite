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
    [Required(ErrorMessage = "The TestQId is required.")]
    public Guid TestQId { get; set; }
    
    [Required(ErrorMessage = "The UserId is required.")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "The Score is required.")]
    [Range(0, 100, ErrorMessage = "The Score must be between 0 and 100.")]
    public double Score { get; set; }

    public string? Comments { get; set; } = string.Empty;
        
    }
}