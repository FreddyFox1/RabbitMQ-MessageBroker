namespace WorkQueues_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            NewTask NewTask = new NewTask();
            NewTask.Sender();
        }
    }
}
