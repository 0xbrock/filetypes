// <copyright file="FileTypeMatcher.cs" company="Dan Abramov">
// Copyright © 2015-2018 Dan Abramov. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace FileTypeChecker
{
    using System;
    using System.IO;

    public abstract class FileTypeMatcher
    {
        public bool Matches(Stream stream)
        {
            return this.Matches(stream, true);
        }

        public bool Matches(Stream stream, bool resetPosition)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (!stream.CanRead || (stream.Position != 0 && !stream.CanSeek))
            {
                throw new ArgumentException("File contents must be a readable stream", "stream");
            }

            if (stream.Position != 0 && resetPosition)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return this.MatchesPrivate(stream);
        }

        protected abstract bool MatchesPrivate(Stream stream);
    }
}
