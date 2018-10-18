// <copyright file="FileType.cs" company="Dan Abramov">
// Copyright © 2015-2018 Dan Abramov. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace FileTypeChecker
{
    using System.IO;

    public class FileType
    {
        private static readonly FileType UnknownValue = new FileType("unknown", string.Empty, null);

        private readonly string name;

        private readonly string extension;

        private readonly FileTypeMatcher fileTypeMatcher;

        public FileType(string name, string extension, FileTypeMatcher matcher)
        {
            this.name = name;
            this.extension = extension;
            this.fileTypeMatcher = matcher;
        }

        public static FileType Unknown => UnknownValue;

        public string Name => this.name;

        public string Extension => this.extension;

        public bool Matches(Stream stream)
        {
            return this.fileTypeMatcher == null || this.fileTypeMatcher.Matches(stream);
        }
    }
}
