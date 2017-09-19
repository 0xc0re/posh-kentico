using CMS.FormEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshKentico.Navigation.FileSystemItems
{
    public class FormFieldFileSystemItem : AbstractFileSystemItem
    {
        private FormFieldInfo formFieldInfo;
        private string path;

        public FormFieldFileSystemItem(FormFieldInfo formFieldInfo, IFileSystemItem parent)
            : base(parent)
        {
            this.formFieldInfo = formFieldInfo;
            this.path = $"{parent.Path.TrimEnd('\\')}\\{formFieldInfo.Name.TrimStart('\\')}";
        }

        /// <inheritdoc/>
        public override IEnumerable<IFileSystemItem> Children => null;

        public override bool IsContainer => false;

        public override object Item => this.formFieldInfo;

        public override string Path => this.path;

        public override bool Delete(bool recurse)
        {
            throw new NotImplementedException();
        }

        public override bool Exists(string path)
        {
            return this.path.Equals(path, StringComparison.InvariantCultureIgnoreCase) && this.formFieldInfo != null;
        }

        public override IFileSystemItem FindPath(string path)
        {
            if (this.path.Equals(path, StringComparison.InvariantCultureIgnoreCase))
            {
                return this;
            }

            return null;
        }

        public override void NewItem(string name, string itemTypeName, object newItemValue)
        {
            throw new NotImplementedException();
        }
    }
}
