namespace FileTypeChecker.Tests
{
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using NUnit.Framework;

    using Properties;

    [TestFixture]
    public class FileTypeCheckerTests
    {
        [TestFixture]
        public class WhenTheFileIsKnown
        {
            private MemoryStream bitmap;

            private MemoryStream pdf;

            private FileTypeChecker checker;

            [TestFixtureSetUp]
            public void SetUp()
            {
                bitmap = new MemoryStream();
                // LAND.bmp is from http://www.fileformat.info/format/bmp/sample/
                Resources.LAND.Save(bitmap, ImageFormat.Bmp);
                // http://boingboing.net/2015/03/23/free-pdf-advanced-quantum-the.html
                pdf = new MemoryStream(Resources.advancedquantumthermodynamics);
                checker = new FileTypeChecker();
            }

            [Test]
            public void ItDetectsPDFs()
            {
                var fileTypes = checker.GetFileTypes(pdf);
                CollectionAssert.AreEquivalent(
                    new[] { "Portable Document Format" },
                    fileTypes.Select(fileType => fileType.Name));
            }

            [Test]
            public void ItDetectsBMPs()
            {
                var fileTypes = checker.GetFileTypes(bitmap);
                CollectionAssert.AreEquivalent(
                    new[] { "Bitmap" },
                    fileTypes.Select(fileType => fileType.Name));
            }
        }

        [TestFixture]
        public class WhenTheFileIsUnknown
        {
            private MemoryStream bitmap;

            private MemoryStream pdf;

            private FileTypeChecker checker;

            [TestFixtureSetUp]
            public void SetUp()
            {
                bitmap = new MemoryStream();
                // LAND.bmp is from http://www.fileformat.info/format/bmp/sample/
                Resources.LAND.Save(bitmap, ImageFormat.Bmp);
                // http://boingboing.net/2015/03/23/free-pdf-advanced-quantum-the.html
                pdf = new MemoryStream(Resources.advancedquantumthermodynamics);
            }

            [Test]
            public void ItDoesntDetectPDFs()
            {
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

            [Test]
            public void ItDoesntDetectBMPs()
            {
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

        [TestFixture]
        public class WhenTheFileIsUnknownList
        {
            private MemoryStream bitmap;

            private MemoryStream pdf;

            private FileTypeChecker checker;

            [TestFixtureSetUp]
            public void SetUp()
            {
                bitmap = new MemoryStream();
                // LAND.bmp is from http://www.fileformat.info/format/bmp/sample/
                Resources.LAND.Save(bitmap, ImageFormat.Bmp);
                // http://boingboing.net/2015/03/23/free-pdf-advanced-quantum-the.html
                pdf = new MemoryStream(Resources.advancedquantumthermodynamics);
            }

            [Test]
            public void ItDoesntDetectPDFs()
            {
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

            [Test]
            public void ItDoesntDetectBMPs()
            {
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
