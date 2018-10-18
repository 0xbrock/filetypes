// <copyright file="RangeFileTypeMatcher.cs" company="Dan Abramov">
// Copyright © 2015-2018 Dan Abramov. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace FileTypeChecker
{
    using System;
    using System.IO;

    public class RangeFileTypeMatcher : FileTypeMatcher
    {
        private readonly FileTypeMatcher matcher;

        private readonly int maximumStartLocation;

        public RangeFileTypeMatcher(FileTypeMatcher matcher, int maximumStartLocation)
        {
            this.matcher = matcher;
            this.maximumStartLocation = maximumStartLocation;
        }

        protected override bool MatchesPrivate(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            for (var i = 0; i < this.maximumStartLocation; i++)
            {
                stream.Position = i;
                if (this.matcher.Matches(stream, resetPosition: false))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
