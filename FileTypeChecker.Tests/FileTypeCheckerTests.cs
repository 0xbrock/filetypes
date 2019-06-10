namespace FileTypeChecker.Tests
{

    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Drawing.Imaging;

    [TestClass]
    public class FileTypeCheckerTests
    {

        public static MemoryStream LoadFile(string file)
        {
            MemoryStream memory;
            // LAND.bmp is from http://www.fileformat.info/format/bmp/sample/
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                memory = new MemoryStream(bytes);
            }

            return memory;
        }

        [TestClass]
        public class WhenTheFileIsKnown
        {
            private FileTypeChecker checker;

            [TestInitialize]
            public void SetUp()
            {
                checker = new FileTypeChecker();
            }

            [TestMethod]
            public void ItDetectsPDFs()
            {
                var pdf = LoadFile("Resources/pdf.pdf");
                var fileTypes = checker.GetFileTypes(pdf);
                CollectionAssert.AreEquivalent(
                    new[] { "Portable Document Format" },
                    fileTypes.Select(fileType => fileType.Name).ToArray());
            }

            [TestMethod]
            public void ItDetectsBMPs()
            {
                var bitmap = LoadFile("Resources/bmp.bmp");
                var fileTypes = checker.GetFileTypes(bitmap);
                CollectionAssert.AreEquivalent(
                    new[] { "Bitmap" },
                    fileTypes.Select(fileType => fileType.Name).ToArray());
            }


            [TestMethod]
            public void ItDetectsPNGs()
            {
                var bitmap = LoadFile("Resources/png.png");
                var fileTypes = checker.GetFileTypes(bitmap);
                CollectionAssert.AreEquivalent(
                    new[] { "Portable Network Graphic" },
                    fileTypes.Select(fileType => fileType.Name).ToArray());
            }


            [TestMethod]
            public void ItDetectsJPGs()
            {
                using(var bitmap = LoadFile("Resources/jpg.jpg"))
                {
                    Assert.IsTrue(checker.IsValidExtension(bitmap, ".jpg"));
                }                
            }

            [TestMethod]
            public void ItDetectsGIFs()
            {
                var bitmap = LoadFile("Resources/gif.gif");
                var fileTypes = checker.GetFileTypes(bitmap);
                CollectionAssert.AreEquivalent(
                    new[] { "Graphics Interchange Format 89a" },
                    fileTypes.Select(fileType => fileType.Name).ToArray());
            }
        }

        [TestClass]
        public class WhenTheFileIsUnknown
        {
            private FileTypeChecker checker;

            [TestMethod]
            public void ItDoesntDetectPDFs()
            {
                var pdf = LoadFile("Resources/pdf.pdf");
                checker = new FileTypeChecker(new List<FileType>
                {
                    new FileType("Bitmap", ".bmp", new ExactFileTypeMatcher(new byte[] {0x42, 0x4d})),
                    new FileType("Portable Network Graphic", ".png",
                        new ExactFileTypeMatcher(new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A})),
                    new FileType("JPEG", ".jpg",
                        new FuzzyFileTypeMatcher(new byte?[] {0xFF, 0xD, 0xFF, 0xE0, null, null, 0x4A, 0x46, 0x49, 0x46, 0x00})),
                    new FileType("Graphics Interchange Format 87a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x37, 0x61})),
                    new FileType("Graphics Interchange Format 89a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x39, 0x61}))
                    // ... Potentially more in future
                });
                var fileType = checker.GetFileType(pdf);
                Assert.AreEqual(
                    "unknown",
                    fileType.Name);
            }

            [TestMethod]
            public void ItDoesntDetectBMPs()
            {
                var bitmap = LoadFile("Resources/bmp.bmp");
                checker = new FileTypeChecker(new List<FileType>
                {
                    new FileType("Portable Network Graphic", ".png",
                        new ExactFileTypeMatcher(new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A})),
                    new FileType("JPEG", ".jpg",
                        new FuzzyFileTypeMatcher(new byte?[] {0xFF, 0xD, 0xFF, 0xE0, null, null, 0x4A, 0x46, 0x49, 0x46, 0x00})),
                    new FileType("Graphics Interchange Format 87a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x37, 0x61})),
                    new FileType("Graphics Interchange Format 89a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x39, 0x61})),
                    new FileType("Portable Document Format", ".pdf", new RangeFileTypeMatcher(new ExactFileTypeMatcher(new byte[] { 0x25, 0x50, 0x44, 0x46 }), 1019))
                    // ... Potentially more in future
                });
                var fileType = checker.GetFileType(bitmap);
                Assert.AreEqual(
                    "unknown",
                    fileType.Name);
            }

        }

        [TestClass]
        public class WhenTheFileIsUnknownList
        {
            private FileTypeChecker checker;

            [TestMethod]
            public void ItDoesntDetectPDFs()
            {
                var pdf = LoadFile("Resources/pdf.pdf");
                checker = new FileTypeChecker(new List<FileType>
                {
                    new FileType("Bitmap", ".bmp", new ExactFileTypeMatcher(new byte[] {0x42, 0x4d})),
                    new FileType("Portable Network Graphic", ".png",
                        new ExactFileTypeMatcher(new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A})),
                    new FileType("JPEG", ".jpg",
                        new FuzzyFileTypeMatcher(new byte?[] {0xFF, 0xD, 0xFF, 0xE0, null, null, 0x4A, 0x46, 0x49, 0x46, 0x00})),
                    new FileType("Graphics Interchange Format 87a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x37, 0x61})),
                    new FileType("Graphics Interchange Format 89a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x39, 0x61}))
                    // ... Potentially more in future
                });
                var fileTypes = checker.GetFileTypes(pdf);
                Assert.AreEqual(0, fileTypes.Count());
            }

            [TestMethod]
            public void ItDoesntDetectBMPs()
            {
                var bitmap = LoadFile("Resources/bmp.bmp");
                checker = new FileTypeChecker(new List<FileType>
                {
                    new FileType("Portable Network Graphic", ".png",
                        new ExactFileTypeMatcher(new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A})),
                    new FileType("JPEG", ".jpg",
                        new FuzzyFileTypeMatcher(new byte?[] {0xFF, 0xD, 0xFF, 0xE0, null, null, 0x4A, 0x46, 0x49, 0x46, 0x00})),
                    new FileType("Graphics Interchange Format 87a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x37, 0x61})),
                    new FileType("Graphics Interchange Format 89a", ".gif",
                        new ExactFileTypeMatcher(new byte[] {0x47, 0x49, 0x46, 0x38, 0x39, 0x61})),
                    new FileType("Portable Document Format", ".pdf", new RangeFileTypeMatcher(new ExactFileTypeMatcher(new byte[] { 0x25, 0x50, 0x44, 0x46 }), 1019))
                    // ... Potentially more in future
                });
                var fileTypes = checker.GetFileTypes(bitmap);
                Assert.AreEqual(0, fileTypes.Count());
            }
        }
    }
}
