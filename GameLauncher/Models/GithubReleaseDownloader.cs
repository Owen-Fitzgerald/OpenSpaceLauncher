using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Geraldine.GameLauncher.Models
{

    public class GitHubReleaseDownloader
    {
        private static readonly string launcherRepoName = "OpenSpaceLauncher";
        private static readonly string gameRepoName = "OpenSpace";

        private static readonly string latestLauncherUrl = "https://api.github.com/repos/Owen-Fitzgerald/OpenSpaceLauncher/releases/latest";
        private static readonly string latestGameUrl = "https://api.github.com/repos/Owen-Fitzgerald/OpenSpaceLauncher/releases/latest";

        internal static async Task<string> GetLatestLauncherVersionAsync()
        {
            return await GetLatestVersionAsync(launcherRepoName, latestLauncherUrl);
        }

        internal static async Task<string> GetLatestGameVersionAsync()
        {
            return await GetLatestVersionAsync(gameRepoName, latestGameUrl);
        }

        internal static async Task DownloadLauncherAsync()
        {
            string downloadUrl = await GetArtifactDownloadUrlAsync(launcherRepoName, latestLauncherUrl);

            if (!string.IsNullOrEmpty(downloadUrl))
            {
                // Step 3: Download the artifact
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "artifact.zip");
                await DownloadArtifactAsync(downloadUrl, outputPath);
            }
        }

        internal static async Task DownloadGameAsync()
        {
            string downloadUrl = await GetArtifactDownloadUrlAsync(gameRepoName, latestGameUrl);

            if (!string.IsNullOrEmpty(downloadUrl))
            {
                // Step 3: Download the artifact
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "artifact.zip");
                await DownloadArtifactAsync(downloadUrl, outputPath);
            }
        }

        private static async Task<string> GetLatestVersionAsync(string repo, string releaseUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                // Add required headers for GitHub API
                client.DefaultRequestHeaders.Add("User-Agent", repo);

                try
                {
                    // Make the request
                    var response = await client.GetStringAsync(releaseUrl);
                    JObject releaseData = JObject.Parse(response);

                    // Extract the tag name, which usually represents the version
                    string latestVersion = releaseData["tag_name"].ToString();
                    Console.WriteLine($"Latest version: {latestVersion}");

                    return latestVersion;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving latest release: " + ex.Message);
                    return null;
                }
            }
        }

        private static async Task<string> GetArtifactDownloadUrlAsync(string repo, string releaseUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                // Add required headers for GitHub API
                client.DefaultRequestHeaders.Add("User-Agent", repo);

                try
                {
                    // Make the request to get release info
                    var response = await client.GetStringAsync(releaseUrl);
                    JObject releaseData = JObject.Parse(response);

                    // Find the assets array and get the first asset's download URL
                    JArray assets = (JArray)releaseData["assets"];
                    if (assets.Count > 0)
                    {
                        string downloadUrl = assets[0]["browser_download_url"].ToString();
                        Console.WriteLine($"Download URL: {downloadUrl}");
                        return downloadUrl;
                    }
                    else
                    {
                        Console.WriteLine("No assets found in the latest release.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving artifact download URL: " + ex.Message);
                    return null;
                }
            }
        }

        private static async Task DownloadArtifactAsync(string downloadUrl, string outputPath)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(downloadUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await response.Content.CopyToAsync(fileStream);
                        }
                        Console.WriteLine($"Downloaded artifact to: {outputPath}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to download artifact. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error downloading artifact: " + ex.Message);
                }
            }
        }

        //public static async Task Main(string[] args)
        //{
        //    // Step 1: Get the latest release version
        //    string latestVersion = await GetLatestVersionAsync();

        //    // Step 2: Get the download URL for the artifact
        //    string downloadUrl = await GetArtifactDownloadUrlAsync();

        //    if (!string.IsNullOrEmpty(downloadUrl))
        //    {
        //        // Step 3: Download the artifact
        //        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "artifact.zip");
        //        await DownloadArtifactAsync(downloadUrl, outputPath);
        //    }
        //}
    }

}
