using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncIO.Tests.Acceptance
{
    [TestFixture]
    public class AsyncFile
    {
        private static readonly string AppBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "tests/";
        private static readonly DummyModel Model = new DummyModel();
        private AsyncIO asyncIO;

        [OneTimeSetUp]
        public void Init()
        {
            this.asyncIO = new AsyncIO();
            Directory.CreateDirectory(AppBase);
        }

        [Test]
        public void WriteAndReadJson()
        {
            string path = Path.Combine(AppBase, "TestJson.json");
            void WriteAct() => this.asyncIO.File.WriteJson(path, Model);
            Assert.That(() => WriteAct(), Throws.Nothing);

            DummyModel result = null;
            DummyModel ReadAct() => this.asyncIO.File.ReadJson<DummyModel>(path);

            Assert.That(() => result = ReadAct(), Throws.Nothing);
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<DummyModel>(result);
        }

        [Test]
        public void WriteAndReadJsonAsync()
        {
            string path = Path.Combine(AppBase, "TestJsonAsync.json");
            async Task WriteAct() => await this.asyncIO.File.WriteJsonAsync(path, Model);
            Assert.That(async () => await WriteAct(), Throws.Nothing);

            DummyModel result = null;
            async Task<DummyModel> ReadAct() => await this.asyncIO.File.ReadJsonAsync<DummyModel>(path);

            Assert.That(async () => result = await ReadAct(), Throws.Nothing);
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<DummyModel>(result);
        }

        [Test]
        public void WriteAndReadBson()
        {
            string path = Path.Combine(AppBase, "TestBson.bson");
            void WriteAct() => this.asyncIO.File.WriteBson(path, Model);
            Assert.That(() => WriteAct(), Throws.Nothing);

            DummyModel result = null;
            DummyModel ReadAct() => this.asyncIO.File.ReadBson<DummyModel>(path);

            Assert.That(() => result = ReadAct(), Throws.Nothing);
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<DummyModel>(result);
        }

        [Test]
        public void WriteAndReadBsonAsync()
        {
            string path = Path.Combine(AppBase, "TestBsonAsync.bson");
            async Task WriteAct() => await this.asyncIO.File.WriteBsonAsync(path, Model);
            Assert.That(async () => await WriteAct(), Throws.Nothing);

            DummyModel result = null;
            async Task<DummyModel> ReadAct() => await this.asyncIO.File.ReadBsonAsync<DummyModel>(path);

            Assert.That(async () => result = await ReadAct(), Throws.Nothing);
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<DummyModel>(result);
        }

        [Test]
        public void WriteAndReadXml()
        {
            string path = Path.Combine(AppBase, "TestXml.xml");
            void WriteAct() => this.asyncIO.File.WriteXml(path, Model);
            Assert.That(() => WriteAct(), Throws.Nothing);

            DummyModel result = null;
            DummyModel ReadAct() => this.asyncIO.File.ReadXml<DummyModel>(path);

            Assert.That(() => result = ReadAct(), Throws.Nothing);
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<DummyModel>(result);
        }

        [Test]
        public void WriteAndReadXmlAsync()
        {
            string path = Path.Combine(AppBase, "TestXmlAsync.xml");
            async Task WriteAct() => await this.asyncIO.File.WriteXmlAsync(path, Model);
            Assert.That(async () => await WriteAct(), Throws.Nothing);

            DummyModel result = null;
            async Task<DummyModel> ReadAct() => await this.asyncIO.File.ReadXmlAsync<DummyModel>(path);

            Assert.That(async () => result = await ReadAct(), Throws.Nothing);
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<DummyModel>(result);
        }

        [Test]
        public void WriteAndReadCsv()
        {
            string path = Path.Combine(AppBase, "TestCsv.csv");
            void WriteAct() => this.asyncIO.File.WriteCsv(path, new[] { Model });
            Assert.That(() => WriteAct(), Throws.Nothing);

            IEnumerable<DummyModel> result = null;
            IEnumerable<DummyModel> ReadAct() => this.asyncIO.File.ReadCsv<DummyModel>(path);

            Assert.That(() => result = ReadAct(), Throws.Nothing);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void WriteAndReadCsvAsync()
        {
            string path = Path.Combine(AppBase, "TestCsvAsync.csv");
            async Task WriteAct() => await this.asyncIO.File.WriteCsvAsync(path, new[] { Model });
            Assert.That(async () => await WriteAct(), Throws.Nothing);

            IEnumerable<DummyModel> result = null;
            async Task<IEnumerable<DummyModel>> ReadAct() => await this.asyncIO.File.ReadCsvAsync<DummyModel>(path);

            Assert.That(async () => result = await ReadAct(), Throws.Nothing);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Copy()
        {
            string path = Path.Combine(AppBase, "TestCopy");
            string target = Path.Combine(AppBase, "TestCopyTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            void CopyAct() => this.asyncIO.File.Copy(path, target);
            Assert.That(() => CopyAct(), Throws.Nothing);
            Assert.AreEqual(File.Exists(target), true);
            Assert.AreEqual(File.ReadAllBytes(path), File.ReadAllBytes(target));
        }

        [Test]
        public void CopyAsync()
        {
            string path = Path.Combine(AppBase, "TestCopyAsync");
            string target = Path.Combine(AppBase, "TestCopyAsyncTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            async Task CopyAct() => await this.asyncIO.File.CopyAsync(path, target);
            Assert.That(async () => await CopyAct(), Throws.Nothing);
            Assert.AreEqual(File.Exists(target), true);
            Assert.AreEqual(File.ReadAllBytes(path), File.ReadAllBytes(target));
        }

        [Test]
        public void CopyWithBuffer()
        {
            string path = Path.Combine(AppBase, "TestCopyWithBuffer");
            string target = Path.Combine(AppBase, "TestCopyWithBufferTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            void CopyAct() => this.asyncIO.File.Copy(path, target, 512);
            Assert.That(() => CopyAct(), Throws.Nothing);
            Assert.AreEqual(File.Exists(target), true);
            Assert.AreEqual(File.ReadAllBytes(path), File.ReadAllBytes(target));
        }

        [Test]
        public void CopyWithBufferAsync()
        {
            string path = Path.Combine(AppBase, "TestCopyWithBufferAsync");
            string target = Path.Combine(AppBase, "TestCopyWithBufferAsyncTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            async Task CopyAct() => await this.asyncIO.File.CopyAsync(path, target, 512);
            Assert.That(async () => await CopyAct(), Throws.Nothing);
            Assert.AreEqual(File.Exists(target), true);
            Assert.AreEqual(File.ReadAllBytes(path), File.ReadAllBytes(target));
        }
    }
}
