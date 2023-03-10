using System.Net;
using NLog;
using Service.Strategy;
using Web.Utils;

var log = LogManager.GetCurrentClassLogger();
log.Info("服务正在启动中......");

var listener = new HttpListener();
listener.Prefixes.Add("http://localhost:80/");
// listener.Prefixes.Add("http://localhost:8080/");
listener.Start();

log.Info("服务已启动!");

var task = HttpServer.Run(listener, MeanStrategyExecutor.Execute);
task.GetAwaiter().GetResult();

listener.Close();