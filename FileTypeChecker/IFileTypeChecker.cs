// <copyright file="IFileTypeChecker.cs" company="Dan Abramov">
// Copyright © 2015-2018 Dan Abramov. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace FileTypeChecker
{
    using System.Collections.Generic;
    using System.IO;

    public interface IFileTypeChecker
    {
        FileType GetFileType(Stream fileContent);

        IEnumerable<FileType> GetFileTypes(Stream stream);
    }
}
