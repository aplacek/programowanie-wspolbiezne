using System.Collections.Concurrent;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//dotnet add package Newtonsoft.Json - for json
public class AsyncDataLogger
{
    private readonly BlockingCollection<JObject> logQueue = new();
    private readonly string pathToSave;
    private readonly Thread loggingThread;
    private readonly JArray dataArray = new();

    public AsyncDataLogger()
    {
        pathToSave = Path.Combine(AppContext.BaseDirectory, "dataLogs.json");

        if (File.Exists(pathToSave))
        {
            try
            {
                string input = File.ReadAllText(pathToSave);
                dataArray = JArray.Parse(input);
            }
            catch
            {
             
                dataArray = new JArray();
            }
        }
        //a new thread for log - it stops when the program stops
        loggingThread = new Thread(LoggingWorker);
        loggingThread.IsBackground = true;
        loggingThread.Start();
    }

    public void AddLog(BallAPI ball)
    {
        JObject logEntry = JObject.FromObject(ball);
        logEntry["Time"] = DateTime.Now.ToString("HH:mm:ss.fff");
        logQueue.Add(logEntry); // wrzucenie do bufora
    }

    private void LoggingWorker()
    {   
        //GetConsumingEnumerable() doesnt end the loop immediately when the queue is empty, the loop ends when logQueue.CompleteAdding() is invoked,
        //we don't use logQueue.CompleteAdding()  in our code, therefore the code runs in the background all the time
        foreach (var log in logQueue.GetConsumingEnumerable())
        {
            dataArray.Add(log);

            try
            {
                File.WriteAllText(pathToSave, dataArray.ToString(Formatting.Indented));
            }
            catch (IOException)
            {
                Console.WriteLine("no access to the file");
                Thread.Sleep(500); 
                logQueue.Add(log); 
            }
        }
    }
}
