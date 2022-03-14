﻿using System.Net;

namespace Shimakaze.Tools.Csf.Server;

/// <summary>
/// Request Extension
/// </summary>
public static class RequestExtensions
{
    /// <summary>
    /// Get Query Parameters
    /// </summary>
    /// <param name="request">Web Request</param>
    /// <returns>Parameters</returns>
    public static Dictionary<string, string> GetQueries(this HttpListenerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Url?.Query))
            return new();

        return request.Url!.Query[1..]
                   .Split('&')
                   .Select(x => x.Split('='))
                   .ToDictionary(x => x[0], x => x[1]);
    }
}
