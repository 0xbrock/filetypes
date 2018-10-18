// <copyright file="ExactFileTypeMatcher.cs" company="Dan Abramov">
// Copyright © 2015-2018 Dan Abramov. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace FileTypeChecker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ExactFileTypeMatcher : FileTypeMatcher
    {
        private readonly byte[] bytes;

        public ExactFileTypeMatcher(IEnumerable<byte> bytes)
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
                if (stream.ReadByte() != b)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
