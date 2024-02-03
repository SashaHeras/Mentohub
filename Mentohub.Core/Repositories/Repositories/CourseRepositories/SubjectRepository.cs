using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RestSharp.Deserializers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Repositories.CourseRepositories
{
    public class SubjectRepository : Repository<CourseSubject>, ISubjectRepository
    {
        private readonly IDistributedCache _cache;

        public SubjectRepository(
            ProjectContext repositoryContext,
            IDistributedCache cache
            ) : base(repositoryContext)
        {
            _cache = cache;
        }

        public async Task<bool> InitRedisSubjects()
        {
            var subjectMember = await _cache.GetStringAsync("Subjects");
            if (subjectMember is null)
            {
                var data = this.GetAll().Select(x => new CourseSubjectDTO()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

                await _cache.SetStringAsync("Subjects", JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                }));
            }
            else
            {
                var result = JsonConvert.DeserializeObject<List<CourseSubjectDTO>>(subjectMember);
            }

            return true;
        }
    }
}
