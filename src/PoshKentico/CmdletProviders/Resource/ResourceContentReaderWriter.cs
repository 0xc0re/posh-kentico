﻿// <copyright file="ResourceProvider.cs" company="Chris Crutchfield">
// Copyright (C) 2017  Chris Crutchfield
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see &lt;http://www.gnu.org/licenses/&gt;.
// </copyright>

using PoshKentico.Core.Services.Resource;
using System.Collections;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace PoshKentico.CmdletProviders.Resource
{
    public class ResourceContentReaderWriter : IContentWriter, IContentReader
    {
        private IResourceReaderWriterService ReadWriteService { get; set; }

        public ResourceContentReaderWriter(IResourceReaderWriterService readWriteService)
        {
            this.ReadWriteService = readWriteService;
        }

        public void Close()
        {
            ReadWriteService.Close();
        }

        public void Dispose()
        {
            ReadWriteService.Dispose();
        }

        public IList Read(long readCount)
        {
            ArrayList list = new ArrayList();

            var content = ReadWriteService.Read();

            if (content != null)
                list.Add(content);

            return list;
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            throw new PSNotSupportedException();
        }

        public IList Write(IList content)
        {
            return ReadWriteService.Write(content.Cast<byte>().ToArray());
        }
    }
}
