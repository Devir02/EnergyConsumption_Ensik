﻿
using System.Diagnostics.Contracts;
using EnergyConsumption.Repository.Entities;
using EnergyConsumption.Repository.Interfaces;
using NLog;

namespace LoggerService
    {
        public class LoggerManager : ILoggerManager
        {
            private static ILogger logger = LogManager.GetCurrentClassLogger();

            public void LogDebug(string message) => logger.Debug(message);

            public void LogError(string message) => logger.Error(message);

            public void LogInfo(string message) => logger.Info(message);

            public void LogWarn(string message) => logger.Warn(message);
        }
    }
