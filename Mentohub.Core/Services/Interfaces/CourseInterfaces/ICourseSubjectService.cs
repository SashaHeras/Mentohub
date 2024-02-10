namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseSubjectService : IService
    {
        List<KeyValuePair<int, string>> SubjectsList(bool withCourseCount = false);
    }
}
