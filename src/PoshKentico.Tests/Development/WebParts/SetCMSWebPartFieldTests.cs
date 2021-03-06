﻿// <copyright file="SetCMSWebPartFieldTests.cs" company="Chris Crutchfield">
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

using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using PoshKentico.Business.Development.WebParts;
using PoshKentico.Core.Services.Development.WebParts;

namespace PoshKentico.Tests.Development.WebParts
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SetCMSWebPartFieldTests
    {
        [TestCase]
        public void ShouldSetWebPartField()
        {
            var webPartFieldMock = new Mock<IWebPartField>();

            var webPartServiceMock = new Mock<IWebPartService>();

            var businessLayer = new SetCMSWebPartFieldBusiness
            {
                WebPartService = webPartServiceMock.Object,
            };

            businessLayer.Set(webPartFieldMock.Object);

            webPartServiceMock
                .Verify(x => x.Update(webPartFieldMock.Object));
        }
    }
}
