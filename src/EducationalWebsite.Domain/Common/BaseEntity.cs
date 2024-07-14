using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EducationalWebsite.Domain.Common
{
    public class BaseEntity
    {
        [BsonId]
        public Guid Id { get; set; } 

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonIgnoreIfDefault]
        public DateTime? UpdatedAt { get; set; }
    }
}