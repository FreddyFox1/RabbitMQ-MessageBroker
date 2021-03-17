namespace WorkQueues_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            NewTask NewTask = new NewTask();
            Messags = new string [] { "Task1.......", "Task3......", "Task2..", "Task5.......", "Task4......", "Task6.." };
            NewTask.Sender();
        }
    }
}
