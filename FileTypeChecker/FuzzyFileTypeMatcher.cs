// <copyright file="FuzzyFileTypeMatcher.cs" company="Dan Abramov">
// Copyright © 2015-2018 Dan Abramov. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace FileTypeChecker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FuzzyFileTypeMatcher : FileTypeMatcher
    {
        private readonly byte?[] bytes;

        public FuzzyFileTypeMatcher(IEnumerable<byte?> bytes)
        {
            this.bytes = bytes.ToArray();
        }

        protected override bool MatchesPrivate(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            foreach (var b in this.bytes)
            {
                var c = stream.ReadByte();
                if (c == -1 || (b.HasValue && c != b.Value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
