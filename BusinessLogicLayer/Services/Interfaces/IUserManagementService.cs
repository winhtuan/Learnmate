using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IUserManagementService
{
    Task<IEnumerable<DTOs.UserManagement.UserRowDto>> GetUsersAsync(string roleFilter = "", string statusFilter = "", string searchQuery = "");
    Task<ApiResponse<object?>> CreateUserAsync(DTOs.UserManagement.NewUserRequestDto request);
    Task<ApiResponse<object?>> ChangeUserStatusAsync(long userId, bool isActive, string reason, string notes = "");
    Task<ApiResponse<object?>> DeleteUserAsync(long userId);
}
