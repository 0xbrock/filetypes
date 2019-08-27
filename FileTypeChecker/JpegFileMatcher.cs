using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileTypeChecker
{
    class JpegFileMatcher : FileTypeMatcher
    {
        // based on https://stackoverflow.com/questions/772388/c-sharp-how-can-i-test-a-file-is-a-jpeg
        // from answered Jan 6 '12 at 8:08 by Orwellophile
        protected override bool MatchesPrivate(Stream stream)
        {

            try
            {
                // 0000000: ffd8 ffe0 0010 4a46 4946 0001 0101 0048  ......JFIF.....H
                // 0000000: ffd8 ffe1 14f8 4578 6966 0000 4d4d 002a  ......Exif..MM.*    
                var br = new BinaryReader(stream);
                var soi = br.ReadUInt16();  // Start of Image (SOI) marker (FFD8)
                var marker = br.ReadUInt16(); // JFIF marker (FFE0) EXIF marker (FFE1)

                var isJpeg = soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff;

                return isJpeg;

            }
            catch
            {
                return false;
            }

        }
    }
}
