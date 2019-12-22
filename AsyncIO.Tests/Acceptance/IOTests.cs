using AsyncIO.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AsyncIO.Tests.Acceptance
{
    [TestFixture]
    public class IOTests
    {
        private static readonly string BinBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        private static readonly string AppBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "tests/";
        private static readonly DummyModel Model = new DummyModel();
        private IO io;

        [OneTimeSetUp]
        public void Init()
        {
            this.io = new IO();
            Directory.CreateDirectory(AppBase);
        }

        [Test]
        public void WriteAndRollbackJson()
        {
            string path = Path.Combine(AppBase, "TestJsonRollback.json");
            File.Delete(path);

            this.io.BeginTransaction();
            this.io.File.WriteJson(path, Model);
            Assert.AreEqual(true, File.Exists(path));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackJsonAsync()
        {
            string path = Path.Combine(AppBase, "TestJsonRollbackAsync.json");
            File.Delete(path);

            this.io.BeginTransaction();
            this.io.File.WriteJsonAsync(path, Model).Wait();
            Assert.AreEqual(true, File.Exists(path));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackBson()
        {
            string path = Path.Combine(AppBase, "TestBsonRollback.json");
            File.Delete(path);

            this.io.BeginTransaction();
            this.io.File.WriteBson(path, Model);
            Assert.AreEqual(true, File.Exists(path));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackBsonAsync()
        {
            string path = Path.Combine(AppBase, "TestBsonRollbackAsync.json");
            File.Delete(path);

            this.io.BeginTransaction();
            this.io.File.WriteBsonAsync(path, Model).Wait();
            Assert.AreEqual(true, File.Exists(path));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackXml()
        {
            string path = Path.Combine(AppBase, "TestXmlRollback.json");
            File.Delete(path);

            this.io.BeginTransaction();
            this.io.File.WriteXml(path, Model);
            Assert.AreEqual(true, File.Exists(path));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackXmlAsync()
        {
            string path = Path.Combine(AppBase, "TestXmlRollbackAsync.json");
            File.Delete(path);

            this.io.BeginTransaction();
            this.io.File.WriteXmlAsync(path, Model).Wait();
            Assert.AreEqual(true, File.Exists(path));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackCsv()
        {
            string path = Path.Combine(AppBase, "TestCsvRollback.json");
            File.Delete(path);

            this.io.BeginTransaction();
            this.io.File.WriteCsv(path, new[] { Model });
            Assert.AreEqual(true, File.Exists(path));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackCsvAsync()
        {
            string path = Path.Combine(AppBase, "TestCsvRollbackAsync.json");
            File.Delete(path);

            this.io.BeginTransaction();
            this.io.File.WriteCsvAsync(path, new[] { Model }).Wait();
            Assert.AreEqual(true, File.Exists(path));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void CopyAndRollback()
        {
            string path = Path.Combine(AppBase, "TestCopyRollback");
            string target = Path.Combine(AppBase, "TestCopyRollbackTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            this.io.BeginTransaction();
            this.io.File.Copy(path, target);
            Assert.AreEqual(true, File.Exists(target));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(target));
        }

        [Test]
        public void CopyAndRollbackAsync()
        {
            string path = Path.Combine(AppBase, "TestCopyRollbackAsync");
            string target = Path.Combine(AppBase, "TestCopyRollbackTargetAsync");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            this.io.BeginTransaction();
            this.io.File.CopyAsync(path, target).Wait();
            Assert.AreEqual(true, File.Exists(target));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(target));
        }

        [Test]
        public void CopyAndRollbackWithBuffer()
        {
            string path = Path.Combine(AppBase, "TestCopyRollbackBuffer");
            string target = Path.Combine(AppBase, "TestCopyRollbackBufferTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            this.io.BeginTransaction();
            this.io.File.Copy(path, target, 512);
            Assert.AreEqual(true, File.Exists(target));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(target));
        }

        [Test]
        public void CopyAndRollbackWithBufferAsync()
        {
            string path = Path.Combine(AppBase, "TestCopyRollbackBufferAsync");
            string target = Path.Combine(AppBase, "TestCopyRollbackBufferAsyncTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            this.io.BeginTransaction();
            this.io.File.CopyAsync(path, target, 512).Wait();
            Assert.AreEqual(true, File.Exists(target));
            this.io.Rollback();
            Assert.AreEqual(false, File.Exists(target));
        }

        [Test]
        public void CopyDirectoryAndRollback()
        {
            string path = AppBase;
            string target = BinBase + "TestCopyRollbackTarget";

            this.io.BeginTransaction();
            this.io.Directory.Copy(path, target);
            this.io.Rollback();
            Assert.AreEqual(0, Directory.GetFiles(target).Length);
        }

        [Test]
        public void CopyDirectoryAndRollbackAsync()
        {
            string path = AppBase;
            string target = BinBase + "TestCopyRollbackTargetAsync";

            this.io.BeginTransaction();
            this.io.Directory.CopyAsync(path, target).Wait();
            this.io.Rollback();
            Assert.AreEqual(0, Directory.GetFiles(target).Length);
        }

        [Test]
        public void CopyDirectoryAndRollbackWithBuffer()
        {
            string path = AppBase;
            string target = BinBase + "TestCopyRollbackBufferTarget";

            this.io.BeginTransaction();
            this.io.Directory.Copy(path, target, 1024);
            this.io.Rollback();
            Assert.AreEqual(0, Directory.GetFiles(target).Length);
        }

        [Test]
        public void CopyDirectoryAndRollbackWithBufferAsync()
        {
            string path = AppBase;
            string target = BinBase + "TestCopyRollbackBufferAsyncTarget";

            this.io.BeginTransaction();
            this.io.Directory.CopyAsync(path, target, 1024).Wait();
            this.io.Rollback();
            Assert.AreEqual(0, Directory.GetFiles(target).Length);
        }
    }
}
