using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<ProjectResponse> GetProjectByIdAsync(int projectId)
        {
            Project? ret = await _projectRepository.GetByIdAsync(projectId);
            if(ret != null)
            {
                return new ProjectResponse
                {
                    ProjectId = ret.ProjectId,
                    Name = ret.Name,
                    Description = ret.Description,
                    CreatedAt = ret.CreatedAt,
                    TaskCount = 0
                };
            }
            else
                throw new ArgumentNullException(nameof(projectId));
        }

        public async Task<IEnumerable<ProjectResponse>> GetUserProjectsAsync(int userId)
        {
            var projects = await _projectRepository.GetUserProjectsAsync(userId);
            var projectResponses = new List<ProjectResponse>();

            foreach (var project in projects)
            {
                var taskCount = await _projectRepository.GetTaskCountAsync(project.ProjectId);
                projectResponses.Add(new ProjectResponse
                {
                    ProjectId = project.ProjectId,
                    Name = project.Name,
                    Description = project.Description,
                    CreatedAt = project.CreatedAt,
                    TaskCount = taskCount
                });
            }

            return projectResponses;
        }

        public async Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new UnauthorizedAccessException("User not found");

            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            return await GetProjectByIdAsync(await _projectRepository.CreateAsync(project));
        }
    }
}