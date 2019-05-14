using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestCore.Common.Log
{
    public class LogUtils
    {

        /// <summary>
        /// 锁标识
        /// </summary>
        private static readonly object LockObject = new object();
        /// <summary>
        /// Logger缓存
        /// </summary>
        private static readonly Dictionary<string, ILog> Loggers = new Dictionary<string, ILog>();

        private static ILog _log; 

        private static ILoggerRepository repository;

        public static ILoggerRepository Repository
        {
            get
            {
                if (repository == null)
                {
                    repository = LogManager.CreateRepository("Core");
                    XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
                }
                return repository;
            }
        }


        //public static void Init(string repositoryName)
        //{
        //    if (Repository == null)
        //    {
        //        Repository = LogManager.CreateRepository(repositoryName);
        //        XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
        //    }
        //}

        /// <summary>
        /// 根据给定源获取Logger
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ILog GetLogger(Type type)
        {
            ILog logger;

            if (Loggers.TryGetValue(type.FullName, out logger)) return logger;

            lock (LockObject)
            {
                if (Loggers.TryGetValue(type.FullName, out logger)) return logger;

                logger = LogManager.GetLogger(Repository.Name, type);
                Loggers.Add(type.FullName, logger);
            }
            return logger;
        }

        /// <summary>
        /// 根據類型獲取Logger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ILog GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }

    }
}
