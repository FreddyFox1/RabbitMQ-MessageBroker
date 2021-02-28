namespace WorkQueues_Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            Worker worker = new Worker();
            worker.Receive();
        }
    }
}
