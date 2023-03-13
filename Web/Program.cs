using System.Net;
using NLog;
using Service.Config;
using Service.Strategy;
using Web.Utils;

var log = LogManager.GetCurrentClassLogger();

//历史数据目录
var path = args.Length >= 1 ? args[0] : StockConstant.HistoryPath;
var cache = args.Length >= 2 && bool.Parse(args[1]);

log.Debug("历史数据读取目录: {0}, 是否使用缓存: {1}", path, cache);

log.Info("服务正在启动中......");

var listener = new HttpListener();
listener.Prefixes.Add("http://*:80/");
listener.Start();

log.Info("服务已启动!");

var task = HttpServer.Run(listener, parameters => MeanStrategyExecutor.Execute(path, cache, parameters));
task.GetAwaiter().GetResult();

listener.Close();