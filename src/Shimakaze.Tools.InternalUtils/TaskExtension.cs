namespace Shimakaze.Tools.InternalUtils;

internal static class TaskExtension
{
    public static T RunSync<T>(this Task<T> task)
    {
        task.RunSynchronously();
        return task.Result;
    }
}