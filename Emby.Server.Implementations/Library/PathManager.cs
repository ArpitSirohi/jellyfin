using System;
using System.Globalization;
using System.IO;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.IO;

namespace Emby.Server.Implementations.Library;

/// <summary>
/// IPathManager implementation.
/// </summary>
public class PathManager : IPathManager
{
    private readonly IServerConfigurationManager _config;
    private readonly IApplicationPaths _appPaths;

    /// <summary>
    /// Initializes a new instance of the <see cref="PathManager"/> class.
    /// </summary>
    /// <param name="config">The server configuration manager.</param>
    /// <param name="appPaths">The application paths.</param>
    public PathManager(
        IServerConfigurationManager config,
        IApplicationPaths appPaths)
    {
        _config = config;
        _appPaths = appPaths;
    }

    private string SubtitleCachePath => Path.Combine(_appPaths.DataPath, "subtitles");

    private string AttachmentCachePath => Path.Combine(_appPaths.DataPath, "attachments");

    /// <inheritdoc />
    public string GetAttachmentPath(string mediaSourceId, string fileName)
    {
        return Path.Join(GetAttachmentFolderPath(mediaSourceId), fileName);
    }

    /// <inheritdoc />
    public string GetAttachmentFolderPath(string mediaSourceId)
    {
        var id = Guid.Parse(mediaSourceId);
        return Path.Join(AttachmentCachePath, id.ToString("D", CultureInfo.InvariantCulture));
    }

    /// <inheritdoc />
    public string GetSubtitleFolderPath(string mediaSourceId)
    {
        var id = Guid.Parse(mediaSourceId);
        return Path.Join(SubtitleCachePath, id.ToString("D", CultureInfo.InvariantCulture));
    }

    /// <inheritdoc />
    public string GetSubtitlePath(string mediaSourceId, int streamIndex, string extension)
    {
        return Path.Join(GetSubtitleFolderPath(mediaSourceId), streamIndex.ToString(CultureInfo.InvariantCulture) + extension);
    }

    /// <inheritdoc />
    public string GetTrickplayDirectory(BaseItem item, bool saveWithMedia = false)
    {
        var basePath = _config.ApplicationPaths.TrickplayPath;
        var idString = item.Id.ToString("D", CultureInfo.InvariantCulture);

        return saveWithMedia
            ? Path.Combine(item.ContainingFolderPath, Path.ChangeExtension(item.Path, ".trickplay"))
            : Path.Combine(basePath, idString);
    }
}
