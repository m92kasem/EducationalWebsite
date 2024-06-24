using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalWebsite.Infrastructure.MongoDB
{
    public class MongoDbConnection
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}