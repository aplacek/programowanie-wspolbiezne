using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace DataTests
{
    [TestClass]
    public class DataLoggerTests
    {
        private string logFilePath;
        private System.Timers.Timer colorTimer;

        [TestInitialize]
        public void Setup()
        {
            // plik logów jest w katalogu AppContext.BaseDirectory
            logFilePath = Path.Combine(AppContext.BaseDirectory, "dataLogs.json");
            if (File.Exists(logFilePath))
                File.Delete(logFilePath);

            colorTimer = new System.Timers.Timer(3000);
            colorTimer.AutoReset = true;
            colorTimer.Start();
        }

        [TestMethod]
        public void AddLog_WritesEntryToFile()
        {
            var logger = new DataLogger();

            var ball = BallAPI.createBall(
                ID: 42, 
                X: 123, 
                Y: 456, 
                radius: 10, 
                color: "blue", 
                XDirection: -1, 
                YDirection: 0, 
                weight: 2,
                timer: colorTimer
            );
            logger.AddLog(ball);

            Thread.Sleep(200);

            Assert.IsTrue(File.Exists(logFilePath), "Plik dataLogs.json nie został utworzony.");

            string json = File.ReadAllText(logFilePath);
            var deserialized = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
            Assert.IsNotNull(deserialized, "Nie można zdeserializować JSON.");

            var found = deserialized.FirstOrDefault(d =>
                d.ContainsKey("ID") && (int)JsonElementToObject(d["ID"]) == 42
            );
            Assert.IsNotNull(found, "Nie znaleziono wpisu z ID=42 w pliku logów.");
        }

        [TestMethod]
        public void Logging_DoesNotBlockMoveBall()
        {
            var logger = new DataLogger();

            var ball = BallAPI.createBall(
                ID: 7, 
                X: 0, 
                Y: 0, 
                radius: 5, 
                color: "green", 
                XDirection: 1, 
                YDirection: 0, 
                weight: 1,
                timer: colorTimer
            );
            ball.XSpeed = 50;
            ball.YSpeed = 0;

            double prevX = ball.XPosition;
            for (int i = 0; i < 5; i++)
            {
                logger.AddLog(ball);
                ball.MoveBall(500, 500);
            }
            double afterX = ball.XPosition;
            Assert.IsTrue(
                afterX - prevX > 0,
                $"Po ruchu z logowaniem kula nie zmieniła pozycji: prevX={prevX}, afterX={afterX}"
            );
        }

        private object JsonElementToObject(object element)
        {
            if (element is JsonElement je)
            {
                if (je.ValueKind == JsonValueKind.Number && je.TryGetInt32(out int i))
                    return i;
                if (je.ValueKind == JsonValueKind.String)
                    return je.GetString();
            }
            return element;
        }
    }
}
