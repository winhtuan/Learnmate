using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher.Assignments;
using BusinessLogicLayer.Services.Interfaces.Teacher.Assignments;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces.Teacher.Assignments;

namespace BusinessLogicLayer.Services.Teacher.Assignments;

public class TeacherAssignmentService(ITeacherAssignmentRepository assignmentRepo) : ITeacherAssignmentService
{
    public async Task<ApiResponse<object>> CreateAssignmentAsync(long teacherId, CreateAssignmentDto dto)
    {
        var newAssignment = new Assignment
        {
            TeacherId = teacherId,
            ClassId = dto.ClassId,
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Status = AssignmentStatus.DRAFT,
            CreatedAt = DateTime.UtcNow,
            Questions = dto.Questions.Select((q, qIndex) => new AssignmentQuestion
            {
                Content = q.Content,
                Type = q.Type,
                Order = qIndex + 1,
                Points = q.Points,
                Options = q.Options.Select((o, oIndex) => new AssignmentOption
                {
                    Content = o.Content,
                    IsCorrect = o.IsCorrect,
                    Order = oIndex + 1
                }).ToList()
            }).ToList()
        };
        
        await assignmentRepo.CreateAsync(newAssignment);
        
        return ApiResponse<object>.Ok(newAssignment.Id, "Assignment created successfully");
    }

    public async Task<List<AssignmentListItemDto>> GetAssignmentsByTeacherAsync(long teacherId)
    {
        var assignments = await assignmentRepo.GetByTeacherIdAsync(teacherId);

        return assignments.Select(a => new AssignmentListItemDto
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            Status = a.Status,
            DueDate = a.DueDate,
            ClassName = a.Class?.Name ?? "N/A",
            QuestionCount = a.Questions?.Count ?? 0,
            SubmissionCount = a.Submissions?.Count ?? 0,
            CreatedAt = a.CreatedAt
        }).ToList();
    }

    public async Task<AssignmentDetailDto?> GetAssignmentByIdAsync(long assignmentId)
    {
        var a = await assignmentRepo.GetByIdWithDetailsAsync(assignmentId);
        if (a == null) return null;

        return new AssignmentDetailDto
        {
            Id = a.Id,
            ClassId = a.ClassId,
            Title = a.Title,
            Description = a.Description,
            Status = a.Status,
            DueDate = a.DueDate,
            ClassName = a.Class?.Name ?? "N/A",
            CreatedAt = a.CreatedAt,
            Questions = a.Questions.Select(q => new AssignmentQuestionDetailDto
            {
                Id = q.Id,
                Content = q.Content,
                Type = q.Type,
                Order = q.Order,
                Points = q.Points,
                Options = q.Options.Select(o => new AssignmentOptionDetailDto
                {
                    Id = o.Id,
                    Content = o.Content,
                    IsCorrect = o.IsCorrect,
                    Order = o.Order
                }).ToList()
            }).ToList()
        };
    }

    public async Task<ApiResponse<object>> UpdateAssignmentAsync(long assignmentId, UpdateAssignmentDto dto)
    {
        var assignment = await assignmentRepo.GetByIdWithDetailsAsync(assignmentId);
        if (assignment == null)
            return ApiResponse<object>.Fail("Assignment not found");

        // Update basic fields
        assignment.Title = dto.Title;
        assignment.Description = dto.Description;
        assignment.DueDate = dto.DueDate;
        assignment.ClassId = dto.ClassId;
        assignment.Status = dto.Status;

        // Sync questions: repo deletes old ones, so all new with Id=0
        assignment.Questions.Clear();
        assignment.Questions = dto.Questions.Select((q, qIndex) => new AssignmentQuestion
        {
            AssignmentId = assignmentId,
            Content = q.Content,
            Type = q.Type,
            Order = qIndex + 1,
            Points = q.Points,
            Options = q.Options.Select((o, oIndex) => new AssignmentOption
            {
                Content = o.Content,
                IsCorrect = o.IsCorrect,
                Order = oIndex + 1
            }).ToList()
        }).ToList();

        await assignmentRepo.UpdateAsync(assignment);

        return ApiResponse<object>.Ok(null, "Assignment updated successfully");
    }
}
