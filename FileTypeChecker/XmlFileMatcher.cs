using System;
using System.IO;

namespace FileTypeChecker
{
    internal class XmlFileMatcher : FileTypeMatcher
    {
        protected override bool MatchesPrivate(Stream stream)
        {
            try
            {
                var n = new System.Xml.XmlDocument();
                n.Load(stream);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}