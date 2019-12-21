﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncIO
{
    public class AsyncDirectory
    {
        private readonly AsyncFile file;

        internal AsyncDirectory(AsyncFile file)
        {
            this.file = file;
        }

        public void Copy(string sourcePath, string targetPath)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo target = new DirectoryInfo(targetPath);

            CopyAll(source, target);
        }

        public async Task CopyAsync(string sourcePath, string targetPath)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo target = new DirectoryInfo(targetPath);

            await CopyAllAsync(source, target).ConfigureAwait(false);
        }

        public void Copy(string sourcePath, string targetPath, int bufferLength)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo target = new DirectoryInfo(targetPath);

            CopyAll(source, target, bufferLength);
        }

        public async Task CopyAsync(string sourcePath, string targetPath, int bufferLength)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo target = new DirectoryInfo(targetPath);

            await CopyAllAsync(source, target, bufferLength).ConfigureAwait(false);
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo sourceFile in source.GetFiles())
            {
                var sourcePath = Path.Combine(source.FullName, sourceFile.Name);
                var targetPath = Path.Combine(target.FullName, sourceFile.Name);

                this.file.Copy(sourcePath, targetPath);
            }

            foreach (DirectoryInfo subSource in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subSource.Name);
                CopyAll(subSource, nextTargetSubDir);
            }
        }

        private async Task CopyAllAsync(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            var tasks = new List<ConfiguredTaskAwaitable>();
            foreach (FileInfo sourceFile in source.GetFiles())
            {
                var sourcePath = Path.Combine(source.FullName, sourceFile.Name);
                var targetPath = Path.Combine(target.FullName, sourceFile.Name);

                tasks.Add(this.file.CopyAsync(sourcePath, targetPath).ConfigureAwait(false));
            }

            foreach (DirectoryInfo subSource in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subSource.Name);
                tasks.Add(this.CopyAllAsync(subSource, nextTargetSubDir).ConfigureAwait(false));
            }

            foreach (var task in tasks)
            {
                await task;
            }
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target, int bufferLength)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo sourceFile in source.GetFiles())
            {
                var sourcePath = Path.Combine(source.FullName, sourceFile.Name);
                var targetPath = Path.Combine(target.FullName, sourceFile.Name);

                this.file.Copy(sourcePath, targetPath, bufferLength);
            }

            foreach (DirectoryInfo subSource in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subSource.Name);
                this.CopyAll(subSource, nextTargetSubDir, bufferLength);
            }
        }

        private async Task CopyAllAsync(DirectoryInfo source, DirectoryInfo target, int bufferLength)
        {
            Directory.CreateDirectory(target.FullName);

            var tasks = new List<ConfiguredTaskAwaitable>();
            foreach (FileInfo sourceFile in source.GetFiles())
            {
                var sourcePath = Path.Combine(source.FullName, sourceFile.Name);
                var targetPath = Path.Combine(target.FullName, sourceFile.Name);

                tasks.Add(this.file.CopyAsync(sourcePath, targetPath, bufferLength).ConfigureAwait(false));
            }

            foreach (DirectoryInfo subSource in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subSource.Name);
                tasks.Add(this.CopyAllAsync(subSource, nextTargetSubDir, bufferLength).ConfigureAwait(false));
            }

            foreach (var task in tasks)
            {
                await task;
            }
        }
    }
}