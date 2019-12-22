// <copyright file="AsyncDirectory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AsyncIO.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    // TO-DO: option to be able to overwrite.

    /// <summary>
    /// Provides features for directory handling.
    /// </summary>
    public class AsyncDirectory
    {
        private readonly AsyncFile file;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDirectory"/> class.
        /// </summary>
        /// <param name="file">Needed features for file handling.</param>
        internal AsyncDirectory(AsyncFile file)
        {
            this.file = file;
        }

        /// <summary>
        /// Recursive copy with all contents and subfolders disallowing overwrite.
        /// </summary>
        /// <param name="sourcePath">Source directory path.</param>
        /// <param name="targetPath">Target directory path.</param>
        /// <param name="overwrite">Overwrite existing file.</param>
        public void Copy(string sourcePath, string targetPath, bool overwrite = false)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo target = new DirectoryInfo(targetPath);

            this.CopyAll(source, target, overwrite);
        }

        /// <summary>
        /// Recursive async copy with all contents and subfolders disallowing overwrite.
        /// </summary>
        /// <param name="sourcePath">Source directory path.</param>
        /// <param name="targetPath">Target directory path.</param>
        /// <param name="overwrite">Overwrite existing file.</param>
        /// <returns>Task.</returns>
        public async Task CopyAsync(string sourcePath, string targetPath, bool overwrite = false)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo target = new DirectoryInfo(targetPath);

            await this.CopyAllAsync(source, target, overwrite).ConfigureAwait(false);
        }

        /// <summary>
        /// Recursive copy with all contents and subfolders disallowing overwrite.
        /// </summary>
        /// <param name="sourcePath">Source directory path.</param>
        /// <param name="targetPath">Target directory path.</param>
        /// <param name="bufferLength">Buffer length for file copy.</param>
        /// <param name="overwrite">Overwrite existing file.</param>
        public void Copy(string sourcePath, string targetPath, int bufferLength, bool overwrite = false)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo target = new DirectoryInfo(targetPath);

            this.CopyAll(source, target, bufferLength, overwrite);
        }

        /// <summary>
        /// Recursive copy with all contents and subfolders disallowing overwrite.
        /// </summary>
        /// <param name="sourcePath">Source directory path.</param>
        /// <param name="targetPath">Target directory path.</param>
        /// <param name="bufferLength">Buffer length for file copy.</param>
        /// <param name="overwrite">Overwrite existing file.</param>
        /// <returns>Task.</returns>
        public async Task CopyAsync(string sourcePath, string targetPath, int bufferLength, bool overwrite = false)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo target = new DirectoryInfo(targetPath);

            await this.CopyAllAsync(source, target, bufferLength, overwrite).ConfigureAwait(false);
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target, bool overwrite)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo sourceFile in source.GetFiles())
            {
                var sourcePath = Path.Combine(source.FullName, sourceFile.Name);
                var targetPath = Path.Combine(target.FullName, sourceFile.Name);

                this.file.Copy(sourcePath, targetPath, overwrite);
            }

            foreach (DirectoryInfo subSource in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subSource.Name);
                this.CopyAll(subSource, nextTargetSubDir, overwrite);
            }
        }

        private async Task CopyAllAsync(DirectoryInfo source, DirectoryInfo target, bool overwrite)
        {
            Directory.CreateDirectory(target.FullName);

            var tasks = new List<ConfiguredTaskAwaitable>();
            foreach (FileInfo sourceFile in source.GetFiles())
            {
                var sourcePath = Path.Combine(source.FullName, sourceFile.Name);
                var targetPath = Path.Combine(target.FullName, sourceFile.Name);

                tasks.Add(this.file.CopyAsync(sourcePath, targetPath, overwrite).ConfigureAwait(false));
            }

            foreach (DirectoryInfo subSource in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subSource.Name);
                tasks.Add(this.CopyAllAsync(subSource, nextTargetSubDir, overwrite).ConfigureAwait(false));
            }

            foreach (var task in tasks)
            {
                await task;
            }
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target, int bufferLength, bool overwrite)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo sourceFile in source.GetFiles())
            {
                var sourcePath = Path.Combine(source.FullName, sourceFile.Name);
                var targetPath = Path.Combine(target.FullName, sourceFile.Name);

                this.file.Copy(sourcePath, targetPath, bufferLength, overwrite);
            }

            foreach (DirectoryInfo subSource in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subSource.Name);
                this.CopyAll(subSource, nextTargetSubDir, bufferLength, overwrite);
            }
        }

        private async Task CopyAllAsync(DirectoryInfo source, DirectoryInfo target, int bufferLength, bool overwrite)
        {
            Directory.CreateDirectory(target.FullName);

            var tasks = new List<ConfiguredTaskAwaitable>();
            foreach (FileInfo sourceFile in source.GetFiles())
            {
                var sourcePath = Path.Combine(source.FullName, sourceFile.Name);
                var targetPath = Path.Combine(target.FullName, sourceFile.Name);

                tasks.Add(this.file.CopyAsync(sourcePath, targetPath, bufferLength, overwrite).ConfigureAwait(false));
            }

            foreach (DirectoryInfo subSource in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subSource.Name);
                tasks.Add(this.CopyAllAsync(subSource, nextTargetSubDir, bufferLength, overwrite).ConfigureAwait(false));
            }

            foreach (var task in tasks)
            {
                await task;
            }
        }
    }
}
