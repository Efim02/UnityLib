namespace UnityLib.Common.GO.Logger
{
    using System;
    using System.IO;
    using System.Linq;

    using UnityEngine;

    using UnityLib.Common.Utils;

    /// <summary>
    /// Игровой логгер.
    /// </summary>
    public static class GameLogger
    {
        public static readonly string DirectoryLogPath = $"{Application.persistentDataPath}/logs";
        public static readonly string FileLogName = $"log - {DateTime.Now:dd.MM.yyyy HH.mm.ss}.txt";

        public static readonly string FileLogPath;
        public static StreamWriter StreamWriter;

        static GameLogger()
        {
            var directoryInfo = Directory.CreateDirectory(DirectoryLogPath);
            var fileInfos = directoryInfo.GetFiles();
            if (fileInfos.Length > 10)
            {
                var lastFileInfo = fileInfos.OrderBy(fi => fi.CreationTimeUtc).First();
                File.Delete(lastFileInfo.FullName);
            }

            FileLogPath = Path.Combine(directoryInfo.FullName, FileLogName);

            var stream = new FileStream(FileLogPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter = new StreamWriter(stream);

            AppUtils.Quiting += Save;
        }

        /// <summary>
        /// Залогировать ошибку
        /// </summary>
        /// <param name="message"> Сообщение. </param>
        public static void Error(string message)
        {
            Debug.LogError($"GameLogger: {message}");
            WriteLog($"Error | {message}.\n" +
                     $" - Unity StackTrace: {StackTraceUtility.ExtractStackTrace()}");
        }

        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="message"> Message. </param>
        /// <param name="exception"> Exception. </param>
        public static void Error(Exception exception, string message = null)
        {
            if (message != null)
                Debug.LogError($"Игровой журнал: {message}\nСтек-трейс ниже: {exception.Message}.");
            Debug.LogException(exception);

            WriteLog($"Ошибка | {message} | {exception.Message}.\n" +
                     $" - Exception StackTrace: {exception.StackTrace}\n" +
                     $" - Unity StackTrace: {StackTraceUtility.ExtractStackTrace()}");
        }

        /// <summary>
        /// Log info.
        /// </summary>
        /// <param name="message"> Message. </param>
        public static void Info(string message)
        {
            Debug.Log($"Игровой журнал: {message}");
            WriteLog($"Информация | {message}.");
        }

        /// <summary>
        /// Сохранить текущий лог из буфера в память.
        /// </summary>
        public static void Save()
        {
            StreamWriter.Close();
            StreamWriter.Dispose();

            var stream = new FileStream(FileLogPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter = new StreamWriter(stream);
        }

        /// <summary>
        /// Log warning.
        /// </summary>
        /// <param name="message"> Message. </param>
        public static void Warning(string message)
        {
            Debug.LogWarning($"Игровой журнал: {message}");
            WriteLog($"Предупреждение | {message}.");
        }

        private static void WriteLog(string log)
        {
            StreamWriter.WriteLine(log);
        }
    }
}