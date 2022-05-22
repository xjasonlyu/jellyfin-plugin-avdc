using System;

namespace Jellyfin.Plugin.AVDC.Models;

public class BaseModel
{
    protected BaseModel()
    {
        Sources = Array.Empty<string>();
        Providers = Array.Empty<string>();
    }

    public string[] Sources { get; set; }
    public string[] Providers { get; set; }
}