namespace FileTypeChecker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FileTypeChecker : IFileTypeChecker
    {
        private IList<FileType> knownFileTypes;

        public FileTypeChecker()
        {
            this.knownFileTypes = new List<FileType>
                {
                    new FileType("Bitmap", ".bmp", new ExactFileTypeMatcher(new byte[] {0x42, 0x4d})),
                    new FileType("Portable Network Graphic", ".png",
                        new ExactFileTypeMatcher(new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A})),
                    new FileType("JPEG", ".jpg",
                        new FuzzyFileTypeMatcher(new byte?[] {0xFF, 0xD, 0xFF, 0xE0, null, null, 0x4A, 0x46, 0x49, 0x46, 0x00})),
                    new FileType("Graphics Interchange Format 87a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x37, 0x61})),
                    new FileType("Graphics Interchange Format 89a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x39, 0x61})),
                    new FileType("Portable Document Format", ".pdf", new RangeFileTypeMatcher(new ExactFileTypeMatcher(new byte[] { 0x25, 0x50, 0x44, 0x46 }), 1019)),
                    new FileType("JPEG", ".jpg", new JpegFileMatcher()),
                    new FileType("JSON", ".json", new JsonFileMatcher()),
                    new FileType("XML", ".xml", new XmlFileMatcher())
                    // ... Potentially more in future
                };
        }

        public bool IsValidExtension(Stream stream, string extension)
        {

            var aux = (extension ?? "").Trim().ToLower();

            if (aux.StartsWith(".") == false) aux = "." + aux;
            if (aux == ".jpeg") aux = ".jpg";

            var types = knownFileTypes.Where(f => f.Extension == aux).ToArray();

            if (types.Length == 0) return false;

            return knownFileTypes.Any(fileType => fileType.Matches(stream));

        }

        public FileTypeChecker(IList<FileType> knownFileTypes)
        {
            this.knownFileTypes = knownFileTypes;
        }

        public FileType GetFileType(Stream fileContent)
        {
            return GetFileTypes(fileContent).FirstOrDefault() ?? FileType.Unknown;
        }

        public IEnumerable<FileType> GetFileTypes(Stream stream)
        {
            return knownFileTypes.Where(fileType => fileType.Matches(stream));
        }
    }
}
