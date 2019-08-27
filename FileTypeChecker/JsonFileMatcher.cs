using System;
using System.IO;

namespace FileTypeChecker
{
    internal class JsonFileMatcher : FileTypeMatcher
    {
        protected override bool MatchesPrivate(Stream stream)
        {
            try
            {
                var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();
                // poor validation... maybe we could use regex...
                return content.StartsWith("{") && content.EndsWith("}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}