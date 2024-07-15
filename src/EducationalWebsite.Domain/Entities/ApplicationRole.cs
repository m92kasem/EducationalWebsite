using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace EducationalWebsite.Domain.Entities
{
        public class ApplicationRole : MongoIdentityRole<Guid>
        {
            public ApplicationRole() : base()
            {
            }

            public ApplicationRole(string roleName) : base(roleName)
            {
            }
        }
}