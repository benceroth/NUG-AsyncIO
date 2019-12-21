using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AsyncIO
{
    public class AsyncFile
    {
        private readonly Conversions conversions;

        internal AsyncFile(Conversions conversions)
        {
            this.conversions = conversions ?? throw new ArgumentNullException(nameof(conversions));
        }

        public T ReadJson<T>(string path)
        {
            return this.conversions.FromJson<T>(File.ReadAllText(path));
        }

        public async Task<T> ReadJsonAsync<T>(string path)
        {
            return this.conversions.FromJson<T>(await File.ReadAllTextAsync(path).ConfigureAwait(false));
        }

        public T ReadBson<T>(string path)
        {
            return this.conversions.FromBson<T>(File.ReadAllBytes(path));
        }

        public async Task<T> ReadBsonAsync<T>(string path)
        {
            return this.conversions.FromBson<T>(await File.ReadAllBytesAsync(path).ConfigureAwait(false));
        }

        public T ReadXml<T>(string path)
            where T : class
        {
            return this.conversions.FromXml<T>(File.ReadAllText(path));
        }

        public async Task<T> ReadXmlAsync<T>(string path)
            where T : class
        {
            return this.conversions.FromXml<T>(await File.ReadAllTextAsync(path).ConfigureAwait(false));
        }

        public IEnumerable<T> ReadCsv<T>(string path)
        {
            foreach(T item in this.conversions.FromCsv<T>(File.ReadAllText(path)))
            {
                yield return item;
            }
        }

        public async Task<IEnumerable<T>> ReadCsvAsync<T>(string path)
        {
            return this.conversions.FromCsv<T>(await File.ReadAllTextAsync(path).ConfigureAwait(false));
        }

        public void WriteJson(string path, object item)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, this.conversions.ToJson(item));
        }

        public async Task WriteJsonAsync(string path, object item)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            await File.WriteAllTextAsync(path, this.conversions.ToJson(item)).ConfigureAwait(false);
        }

        public void WriteBson(string path, object item)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, this.conversions.ToBson(item));
        }

        public async Task WriteBsonAsync(string path, object item)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            await File.WriteAllBytesAsync(path, this.conversions.ToBson(item)).ConfigureAwait(false);
        }

        public void WriteXml(string path, object item)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, this.conversions.ToXml(item));
        }

        public async Task WriteXmlAsync(string path, object item)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            await File.WriteAllTextAsync(path, this.conversions.ToXml(item)).ConfigureAwait(false);
        }

        public void WriteCsv<T>(string path, IEnumerable<T> items)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, this.conversions.ToCsv(items));
        }

        public async Task WriteCsvAsync<T>(string path, IEnumerable<T> items)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            await File.WriteAllTextAsync(path, this.conversions.ToCsv(items)).ConfigureAwait(false);
        }

        public void Copy(string sourcePath, string targetPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
            File.Copy(sourcePath, targetPath);
        }

        public async Task CopyAsync(string sourcePath, string targetPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
            await File.WriteAllBytesAsync(targetPath, await File.ReadAllBytesAsync(sourcePath).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public void Copy(string sourcePath, string targetPath, int bufferLength)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
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

        public async Task CopyAsync(string sourcePath, string targetPath, int bufferLength)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
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
    }
}
