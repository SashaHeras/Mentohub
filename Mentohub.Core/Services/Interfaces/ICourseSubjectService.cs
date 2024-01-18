using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseSubjectService
    {
        List<KeyValuePair<int, string>> SubjectsList();
    }
}
