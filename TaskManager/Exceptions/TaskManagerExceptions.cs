namespace TaskManager.Exceptions
{
    public class TaskManagerException : Exception
    {
        public TaskManagerException(string message) : base(message) { }
    }

    public class ProjectNotFoundException : TaskManagerException
    {
        public ProjectNotFoundException(int projectId)
            : base($"O Projeto com ID {projectId} n�o foi encontrado.") { }
    }

    public class TaskNotFoundException : TaskManagerException
    {
        public TaskNotFoundException(int taskId)
            : base($"A tarefa com ID {taskId} n�o foi encontrada.") { }
    }

    public class UserNotFoundException : TaskManagerException
    {
        public UserNotFoundException(int userId)
            : base($"Usu�rio com ID {userId} n�o foi encontrado.") { }
    }

    public class UnauthorizedOperationException : TaskManagerException
    {
        public UnauthorizedOperationException(string message) : base(message) { }
    }

    public class TaskLimitExceededException : TaskManagerException
    {
        public TaskLimitExceededException(int projectId)
            : base($"O Projeto com ID {projectId} atingiu o limite m�ximo de tarefas.") { }
    }

    public class ProjectDeletionException : TaskManagerException
    {
        public ProjectDeletionException(int projectId)
            : base($"O Projeto com ID {projectId} n�o pode ser removido pois possui tarefas pendentes.") { }
    }
}