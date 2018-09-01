﻿// <copyright file="SetCMSWebPartFieldBusiness.cs" company="Chris Crutchfield">
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

using System.ComponentModel.Composition;
using PoshKentico.Core.Services.Development.WebParts;

namespace PoshKentico.Business.Development.WebParts
{
    /// <summary>
    /// Business layer for the Set-CMSWebPartCategory cmdlet.
    /// </summary>
    [Export(typeof(SetCMSWebPartFieldBusiness))]
    public class SetCMSWebPartFieldBusiness : WebPartBusinessBase
    {
        /// <summary>
        /// Sets an <see cref="IWebPartField"/> in Kentico associated with an <see cref="IWebPart"/>.
        /// </summary>
        /// <param name="field">The <see cref="IWebPartField"/> to update.</param>
        public void Set(IWebPartField field)
        {
            this.WebPartService.Update(field);
        }
    }
}
