// <copyright file="AsyncFile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AsyncIO.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using CsvHelper;

    /// <summary>
    /// Provides features for file handling.
    /// </summary>
    public class AsyncFile
    {
        private readonly Conversions conversions;
        private readonly Transaction transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncFile"/> class.
        /// </summary>
        /// <param name="conversions">Needed conversion features.</param>
        /// <param name="transaction">Needed transaction features.</param>
        internal AsyncFile(Conversions conversions, Transaction transaction)
        {
            this.conversions = conversions ?? throw new ArgumentNullException(nameof(conversions));
            this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        /// <summary>
        /// Reads file's content and deserializes as Json.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="path">File path.</param>
        /// <returns>Deserialized item.</returns>
        public T ReadJson<T>(string path)
        {
            return this.conversions.FromJson<T>(File.ReadAllText(path));
        }

        /// <summary>
        /// Reads file's content async and deserializes as Json.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="path">File path.</param>
        /// <returns>Task with deserialized item.</returns>
        public async Task<T> ReadJsonAsync<T>(string path)
        {
            return this.conversions.FromJson<T>(await File.ReadAllTextAsync(path).ConfigureAwait(false));
        }

        /// <summary>
        /// Reads file's content and deserializes as Bson.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="path">File path.</param>
        /// <returns>Deserialized item.</returns>
        public T ReadBson<T>(string path)
        {
            return this.conversions.FromBson<T>(File.ReadAllBytes(path));
        }

        /// <summary>
        /// Reads file's content async and deserializes as Bson.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="path">File path.</param>
        /// <returns>Task with deserialized item.</returns>
        public async Task<T> ReadBsonAsync<T>(string path)
        {
            return this.conversions.FromBson<T>(await File.ReadAllBytesAsync(path).ConfigureAwait(false));
        }

        /// <summary>
        /// Reads file's content and deserializes as Xml.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="path">File path.</param>
        /// <returns>Deserialized item.</returns>
        public T ReadXml<T>(string path)
            where T : class
        {
            return this.conversions.FromXml<T>(File.ReadAllText(path));
        }

        /// <summary>
        /// Reads file's content async and deserializes as Xml.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="path">File path.</param>
        /// <returns>Task with deserialized item.</returns>
        public async Task<T> ReadXmlAsync<T>(string path)
            where T : class
        {
            return this.conversions.FromXml<T>(await File.ReadAllTextAsync(path).ConfigureAwait(false));
        }

        /// <summary>
        /// Reads file's content and deserializes as Csv.
        /// </summary>
        /// <typeparam name="T">Items type.</typeparam>
        /// <param name="path">File path.</param>
        /// <returns>Deserialized items.</returns>
        public IEnumerable<T> ReadCsv<T>(string path)
        {
            foreach (T item in this.conversions.FromCsv<T>(File.ReadAllText(path)))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Reads file's content async and deserializes as Csv.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="path">File path.</param>
        /// <returns>Task with deserialized items.</returns>
        public async Task<IEnumerable<T>> ReadCsvAsync<T>(string path)
        {
            return this.conversions.FromCsv<T>(await File.ReadAllTextAsync(path).ConfigureAwait(false));
        }

        /// <summary>
        /// Writes file with serialized content as Json.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="item">Item to be serialized.</param>
        public void WriteJson(string path, object item)
        {
            var targetFolder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(path);
            File.WriteAllText(path, this.conversions.ToJson(item));
        }

        /// <summary>
        /// Writes file async with serialized content as Json.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="item">Item to be serialized.</param>
        /// <returns>Task.</returns>
        public async Task WriteJsonAsync(string path, object item)
        {
            var targetFolder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(path);
            await File.WriteAllTextAsync(path, this.conversions.ToJson(item)).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes file with serialized content as Bson.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="item">Item to be serialized.</param>
        public void WriteBson(string path, object item)
        {
            var targetFolder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(path);
            File.WriteAllBytes(path, this.conversions.ToBson(item));
        }

        /// <summary>
        /// Writes file async with serialized content as Bson.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="item">Item to be serialized.</param>
        /// <returns>Task.</returns>
        public async Task WriteBsonAsync(string path, object item)
        {
            var targetFolder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(path);
            await File.WriteAllBytesAsync(path, this.conversions.ToBson(item)).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes file with serialized content as Xml.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="item">Item to be serialized.</param>
        public void WriteXml(string path, object item)
        {
            var targetFolder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(path);
            File.WriteAllText(path, this.conversions.ToXml(item));
        }

        /// <summary>
        /// Writes file async with serialized content as Xml.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="item">Item to be serialized.</param>
        /// <returns>Task.</returns>
        public async Task WriteXmlAsync(string path, object item)
        {
            var targetFolder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(path);
            await File.WriteAllTextAsync(path, this.conversions.ToXml(item)).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes file with serialized content as Csv.
        /// </summary>
        /// <typeparam name="T">Items' type.</typeparam>
        /// <param name="path">File path.</param>
        /// <param name="items">Items to be serialized.</param>
        public void WriteCsv<T>(string path, IEnumerable<T> items)
        {
            var targetFolder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(path);
            File.WriteAllText(path, this.conversions.ToCsv(items));
        }

        /// <summary>
        /// Writes file async with serialized content as Csv.
        /// </summary>
        /// <typeparam name="T">Items' type.</typeparam>
        /// <param name="path">File path.</param>
        /// <param name="items">Items to be serialized.</param>
        /// <returns>Task.</returns>
        public async Task WriteCsvAsync<T>(string path, IEnumerable<T> items)
        {
            var targetFolder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(path);
            await File.WriteAllTextAsync(path, this.conversions.ToCsv(items)).ConfigureAwait(false);
        }

        /// <summary>
        /// Copies file disallowing overwriting.
        /// </summary>
        /// <param name="sourcePath">Source directory path.</param>
        /// <param name="targetPath">Target directory path.</param>
        public void Copy(string sourcePath, string targetPath)
        {
            var targetFolder = Path.GetDirectoryName(targetPath);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(targetPath);
            File.Copy(sourcePath, targetPath);
        }

        /// <summary>
        /// Copies file async disallowing overwriting.
        /// </summary>
        /// <param name="sourcePath">Source directory path.</param>
        /// <param name="targetPath">Target directory path.</param>
        /// <returns>Task.</returns>
        public async Task CopyAsync(string sourcePath, string targetPath)
        {
            var targetFolder = Path.GetDirectoryName(targetPath);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(targetPath);
            await File.WriteAllBytesAsync(targetPath, await File.ReadAllBytesAsync(sourcePath).ConfigureAwait(false)).ConfigureAwait(false);
        }

        /// <summary>
        /// Copies file with given buffer length disallowing overwriting.
        /// </summary>
        /// <param name="sourcePath">Source directory path.</param>
        /// <param name="targetPath">Target directory path.</param>
        /// <param name="bufferLength">Buffer length.</param>
        public void Copy(string sourcePath, string targetPath, int bufferLength)
        {
            var targetFolder = Path.GetDirectoryName(targetPath);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(targetPath);
            using (var sourceStream = new FileStream(sourcePath, FileMode.Open))
            {
                using (var destinationStream = new FileStream(targetPath, FileMode.CreateNew))
                {
                    var buffer = new byte[bufferLength];
                    int readCount = sourceStream.Read(buffer, 0, bufferLength);
                    while (readCount != 0)
                    {
                        destinationStream.Write(buffer, 0, readCount);
                        readCount = sourceStream.Read(buffer, 0, bufferLength);
                    }
                }
            }
        }

        /// <summary>
        /// Copies file async with given buffer length disallowing overwriting.
        /// </summary>
        /// <param name="sourcePath">Source directory path.</param>
        /// <param name="targetPath">Target directory path.</param>
        /// <param name="bufferLength">Buffer length.</param>
        /// <returns>Task.</returns>
        public async Task CopyAsync(string sourcePath, string targetPath, int bufferLength)
        {
            var targetFolder = Path.GetDirectoryName(targetPath);
            Directory.CreateDirectory(targetFolder);
            this.HandleTransaction(targetPath);
            using (var sourceStream = new FileStream(sourcePath, FileMode.Open))
            {
                using (var destinationStream = new FileStream(targetPath, FileMode.CreateNew))
                {
                    var buffer = new byte[bufferLength];
                    int readCount = await sourceStream.ReadAsync(buffer, 0, bufferLength).ConfigureAwait(false);
                    while (readCount != 0)
                    {
                        await destinationStream.WriteAsync(buffer, 0, readCount).ConfigureAwait(false);
                        readCount = await sourceStream.ReadAsync(buffer, 0, bufferLength).ConfigureAwait(false);
                    }
                }
            }
        }

        private void HandleTransaction(string path)
        {
            if (this.transaction.Running)
            {
                var start = DateTime.Now.AddMilliseconds(-50);
                var folder = Path.GetDirectoryName(path);
                this.transaction.Actions.Push(() =>
                {
                    if (Directory.Exists(folder))
                    {
                        if (Directory.GetCreationTime(folder) >= start)
                        {
                            Directory.Delete(folder, true);
                        }
                        else if (File.Exists(path) && File.GetLastWriteTime(path) >= start)
                        {
                            File.Delete(path);
                        }
                    }
                });
            }
        }
    }
}
