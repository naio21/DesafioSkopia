namespace TaskManager.Exceptions
{
    public class TaskManagerException : Exception
    {
        public TaskManagerException(string message) : base(message) { }
    }

    public class ProjectNotFoundException : TaskManagerException
    {
        public ProjectNotFoundException(int projectId)
            : base($"O Projeto com ID {projectId} não foi encontrado.") { }
    }

    public class TaskNotFoundException : TaskManagerException
    {
        public TaskNotFoundException(int taskId)
            : base($"A tarefa com ID {taskId} não foi encontrada.") { }
    }

    public class UserNotFoundException : TaskManagerException
    {
        public UserNotFoundException(int userId)
            : base($"Usuário com ID {userId} não foi encontrado.") { }
    }

    public class UnauthorizedOperationException : TaskManagerException
    {
        public UnauthorizedOperationException(string message) : base(message) { }
    }

    public class TaskLimitExceededException : TaskManagerException
    {
        public TaskLimitExceededException(int projectId)
            : base($"O Projeto com ID {projectId} atingiu o limite máximo de tarefas.") { }
    }

    public class ProjectDeletionException : TaskManagerException
    {
        public ProjectDeletionException(int projectId)
            : base($"O Projeto com ID {projectId} não pode ser removido pois possui tarefas pendentes.") { }
    }
}