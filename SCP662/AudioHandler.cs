using System.Reflection;
using SecretLabNAudio.Core;
using SecretLabNAudio.Core.Extensions;
using SecretLabNAudio.Core.Extensions.Processors;
using SecretLabNAudio.Core.Pools;
using SecretLabNAudio.Core.Processors;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace SCP662;

public class AudioHandler
{
    private static readonly Lazy<AudioHandler> _instance =
        new (() => new AudioHandler());

    private AudioHandler()
    {
    }
    
    public static AudioHandler Instance => _instance.Value;
    
    private static string? Initialize(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        var resourcePath = Path.Combine(Path.GetTempPath(), resourceName);
        
        if (!File.Exists(resourcePath))
        {
            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                Logger.Error($"Could not find embedded resource '{resourceName}'.");
                return null;
            }

            using FileStream fileStream = File.Create(resourcePath);
            stream.CopyTo(fileStream);
            return resourcePath;
        }

        return resourcePath;
    }
    
    public void PlayAudio(string resourcePath, Vector3 position)
    {
        var tempPath = Initialize(resourcePath);
        if (tempPath == null)
        {
            return;
        }

        AudioPlayerPool.RentDefault(position)
            .PoolOnEnd()
            .UseFile(tempPath)
            .Volume = 0.5f;

    }
}