using System.Collections.Generic;
using System.IO;

namespace FileTypeChecker
{
    public interface IFileTypeChecker
    {
        FileType GetFileType(Stream fileContent);
        IEnumerable<FileType> GetFileTypes(Stream stream);
        bool IsValidExtension(Stream stream, string extension);
    }
}