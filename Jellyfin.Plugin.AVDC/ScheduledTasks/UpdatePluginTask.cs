#if __EMBY__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AVDC.Extensions;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Serialization;
using MediaBrowser.Model.Tasks;

namespace Jellyfin.Plugin.AVDC.ScheduledTasks
{
    public class UpdatePluginTask : IScheduledTask
    {
        private readonly IApplicationPaths _applicationPaths;
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger _logger;
        private readonly IZipClient _zipClient;

        public UpdatePluginTask(IApplicationPaths applicationPaths, IHttpClient httpClient,
            IJsonSerializer jsonSerializer, ILogManager logManager, IZipClient zipClient)
        {
            _applicationPaths = applicationPaths;
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
            _logger = logManager.CreateLogger<UpdatePluginTask>();
            _zipClient = zipClient;
        }

        private static string CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string Key => $"{Constant.Avdc}UpdatePlugin";

        public string Name => "Update Plugin";

        public string Description => "Update this plugin to latest version.";

        public string Category => Constant.Avdc;

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            yield return new TaskTriggerInfo
            {
                Type = TaskTriggerInfo.TriggerDaily,
                TimeOfDayTicks = TimeSpan.FromHours(2).Ticks
            };
        }

        public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            await Task.Yield();
            progress?.Report(0);

            try
            {
                var response = await _httpClient.GetResponse(new HttpRequestOptions
                {
                    Url = "https://api.github.com/repos/xjasonlyu/jellyfin-plugin-avdc/releases/latest",
                    CancellationToken = cancellationToken,
                    EnableDefaultUserAgent = true
                });

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"api status code: {response.StatusCode}");

                var apiResult = _jsonSerializer.DeserializeFromStream<ApiResult>(response.Content);

                var currentVersion = ParseVersion(CurrentVersion);
                var remoteVersion = ParseVersion(apiResult.Tag_Name);

                if (currentVersion.CompareTo(remoteVersion) < 0)
                {
                    _logger.Info("[AVDC] New version found: {0}", remoteVersion);

                    var url = apiResult.Assets
                        .Where(asset => asset.Name.StartsWith("Emby") && asset.Name.EndsWith(".zip")).ToArray()
                        .FirstOrDefault()
                        ?.Browser_Download_Url;
                    if (string.IsNullOrEmpty(url))
                        throw new Exception("download url is empty");

                    var zipResp = await _httpClient.GetResponse(new HttpRequestOptions
                    {
                        Url = url,
                        CancellationToken = cancellationToken,
                        EnableDefaultUserAgent = true,
                        Progress = progress
                    });

                    _zipClient.ExtractAllFromZip(zipResp.Content, _applicationPaths.PluginsPath, true);

                    _logger.Info("[AVDC] Update is complete");
                }
                else
                {
                    _logger.Info("[AVDC] No need to update");
                }
            }
            catch (Exception e)
            {
                _logger.Error("[AVDC] Update failed: {0}", e.Message);
            }

            progress?.Report(100);
        }

        private static Version ParseVersion(string v)
        {
            return new Version(v.StartsWith("v") ? v.Substring(1) : v);
        }
    }

    public class ApiResult
    {
        public string Tag_Name { get; set; }
        public Asset[] Assets { get; set; }
    }

    public class Asset
    {
        public string Name { get; set; }
        public string Browser_Download_Url { get; set; }
    }
}

#endif