﻿using CMS.PortalEngine;
using PoshKentico.Navigation.DynamicParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PoshKentico.Navigation.FileSystemItems
{
    public class WebPartCategoryFileSystemItem : AbstractFileSystemItem
    {

        #region Fields

        private IEnumerable<IFileSystemItem> _children;
        private WebPartCategoryInfo _webPartCategoryInfo;

        #endregion


        #region Properties

        public override IEnumerable<IFileSystemItem> Children
        {
            get
            {
                if (_children == null)
                {
                    IEnumerable<IFileSystemItem> childCategories = (from c in WebPartCategoryInfoProvider.GetCategories()
                                                                    where c.CategoryParentID == _webPartCategoryInfo.CategoryID
                                                                    select new WebPartCategoryFileSystemItem(c, this));
                    IEnumerable<IFileSystemItem> childWebParts = (from w in WebPartInfoProvider.GetAllWebParts(_webPartCategoryInfo.CategoryID)
                                                                  select new WebPartFileSystemItem(w, this));

                    _children = childCategories.Concat(childWebParts).ToArray();
                }

                return _children;
            }
        }     
                                                
        public override bool IsContainer => true;
        public override object Item => _webPartCategoryInfo;
        public override string Path => _webPartCategoryInfo.CategoryPath
            .Replace("/", "Development\\WebParts\\")
            .Replace("/", "\\");

        #endregion


        #region Constructors

        public WebPartCategoryFileSystemItem(WebPartCategoryInfo webPartCategoryInfo, IFileSystemItem parent)
            : base(parent)
        {
            _webPartCategoryInfo = webPartCategoryInfo;
        }

        #endregion


        #region Methods

        public static void Create(string displayName, string name, string imagePath, IFileSystemItem parent)
        {
            var parentCategoryItem = parent as WebPartCategoryFileSystemItem;
            if (parentCategoryItem == null) return;

            var newCategory = new WebPartCategoryInfo();
            newCategory.CategoryDisplayName = displayName;
            newCategory.CategoryName = name;
            newCategory.CategoryImagePath = imagePath;
            newCategory.CategoryParentID = parentCategoryItem._webPartCategoryInfo.CategoryParentID;

            WebPartCategoryInfoProvider.SetWebPartCategoryInfo(newCategory);
        }

        public override bool Delete(bool recurse)
        {
            if (recurse && !DeleteChildren()) return false;

            return _webPartCategoryInfo.Delete();
        }

        public override bool Exists(string path)
        {
            return FindPath(path) != null;
        }

        public override IFileSystemItem FindPath(string path)
        {
            var adjustedPath = path.ToLowerInvariant()
                .Replace("development\\webparts", string.Empty)
                .Replace('\\', '/');

            if (string.IsNullOrWhiteSpace(adjustedPath)) adjustedPath = "/";

            var webPartCategoryInfo = (from c in WebPartCategoryInfoProvider.GetCategories()
                                       where c.CategoryPath.Equals(adjustedPath, StringComparison.InvariantCultureIgnoreCase)
                                       select c).FirstOrDefault();

            if (webPartCategoryInfo != null)
                return new WebPartCategoryFileSystemItem(webPartCategoryInfo, this);
            else
                return null;
        }

        public override void NewItem(string name, string itemTypeName, object newItemValue)
        {
            switch (itemTypeName.ToLowerInvariant())
            {
                case "webpartcategory":
                    var dynamicParameter = newItemValue as NewWebPartCategoryDynamicParameter;
                    string displayName = dynamicParameter?.DisplayName ?? name;
                    string imagePath = dynamicParameter?.ImagePath;

                    Create(displayName, name, imagePath, this);
                    return;
                default:
                    throw new NotSupportedException($"Cannot create ItemType \"{itemTypeName}\" at \"{Path}\\{name}\".");
            }
        }

        #endregion

    }
}
