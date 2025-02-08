using Application.Services.BackgroundServices.BackgroundTask;

namespace Application.Services.BackgroundServices.ServiceTask
{
    public class ProduceTask
    {
        private readonly IBackgroundTaskQueue _taskQueue;

        public ProduceTask(IBackgroundTaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }

        private void PublishAssignmentNotification(Guid assignmentId)
        {

        }
    }
}
