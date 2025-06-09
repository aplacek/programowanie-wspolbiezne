using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;

namespace Data
{
    public class DataLogger
    {
        //blockingcollection prevents buffer overflow and prevents reading from an empty buffer
        private readonly BlockingCollection<Dictionary<string, object>> logQueue = new();
        private readonly string pathToSave;
        private readonly Thread loggingThread;
        private List<Dictionary<string, object>> dataArray = new();

        public DataLogger()
        {
            pathToSave = Path.Combine(AppContext.BaseDirectory, "dataLogs.json");

            // Uncomment the line below to save logs in a different directory, in "Presentation/Logs" folder
            //pathToSave = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "Logs", "dataLogs.json");

            if (pathToSave == null)
            {
                throw new ArgumentNullException(nameof(pathToSave), "Path to save the log file cannot be null.");
            }   

            if (File.Exists(pathToSave))
            {
                try
                {
                    string input = File.ReadAllText(pathToSave);
                    var restored = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(input);
                    if (restored != null)
                        dataArray = restored;
                }
                catch
                {
                    dataArray = new List<Dictionary<string, object>>();
                }
            }

            //a new thread for log - it stops when the program stops
            loggingThread = new Thread(LoggingWorker)
            {
                IsBackground = true
            };

            loggingThread.Start();
        }

        public void AddLog(BallAPI ball)
        {
            var logEntry = new Dictionary<string, object>
            {
                { "ID", ball.ID },
                { "X", ball.X },
                { "Y", ball.Y },
                { "Radius", ball.Radius },
                { "Color", ball.color },
                { "XDirection", ball.XDirection },
                { "YDirection", ball.YDirection },
                { "Time", DateTime.Now.ToString("HH:mm:ss.fff") }
            };

            logQueue.Add(logEntry);
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
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    string json = JsonSerializer.Serialize(dataArray, options);
                    File.WriteAllText(pathToSave, json);
                    
                }catch (IOException)
                {
                    Console.WriteLine("no access to the file");
                    Thread.Sleep(500);
                    logQueue.Add(log); 
                }

            }
        }
    }
}
