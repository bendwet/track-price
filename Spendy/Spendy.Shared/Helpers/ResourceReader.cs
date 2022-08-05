using System.Reflection;

namespace Spendy.Shared.Helpers;

public static class ResourceReader
{
    public static string ReadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);
        var sql = reader.ReadToEnd();

        return sql;
    }
}