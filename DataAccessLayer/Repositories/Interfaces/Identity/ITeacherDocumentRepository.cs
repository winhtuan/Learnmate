using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface ITeacherDocumentRepository
{
    Task<IEnumerable<TeacherDocument>> GetByTeacherProfileIdAsync(long profileId);
    Task AddAsync(TeacherDocument document);
    Task DeleteAsync(long id);
}
