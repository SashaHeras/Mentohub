using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseLanguageService : IService
    {
        List<KeyValuePair<int, string>> GetLanguagesList(bool withCourseCount = false);
    }
}
