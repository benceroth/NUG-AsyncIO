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

        [OneTimeSetUp]
        public void Init()
        {
            Directory.CreateDirectory(AppBase);
        }

        [Test]
        public void WriteAndRollbackJson()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestJsonRollback.json");
            File.Delete(path);

            io.BeginTransaction();
            io.File.WriteJson(path, Model);
            Assert.AreEqual(true, File.Exists(path));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackJsonAsync()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestJsonRollbackAsync.json");
            File.Delete(path);

            io.BeginTransaction();
            io.File.WriteJsonAsync(path, Model).Wait();
            Assert.AreEqual(true, File.Exists(path));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackBson()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestBsonRollback.json");
            File.Delete(path);

            io.BeginTransaction();
            io.File.WriteBson(path, Model);
            Assert.AreEqual(true, File.Exists(path));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackBsonAsync()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestBsonRollbackAsync.json");
            File.Delete(path);

            io.BeginTransaction();
            io.File.WriteBsonAsync(path, Model).Wait();
            Assert.AreEqual(true, File.Exists(path));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackXml()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestXmlRollback.json");
            File.Delete(path);

            io.BeginTransaction();
            io.File.WriteXml(path, Model);
            Assert.AreEqual(true, File.Exists(path));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackXmlAsync()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestXmlRollbackAsync.json");
            File.Delete(path);

            io.BeginTransaction();
            io.File.WriteXmlAsync(path, Model).Wait();
            Assert.AreEqual(true, File.Exists(path));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackCsv()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestCsvRollback.json");
            File.Delete(path);

            io.BeginTransaction();
            io.File.WriteCsv(path, new[] { Model });
            Assert.AreEqual(true, File.Exists(path));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        public void WriteAndRollbackCsvAsync()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestCsvRollbackAsync.json");
            File.Delete(path);

            io.BeginTransaction();
            io.File.WriteCsvAsync(path, new[] { Model }).Wait();
            Assert.AreEqual(true, File.Exists(path));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(path));
        }

        [Test]
        [Sequential]
        public void CopyAndRollback([Values(false, true)] bool overwrite)
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestCopyRollback");
            string target = Path.Combine(AppBase, "TestCopyRollbackTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            io.BeginTransaction();
            io.File.Copy(path, target, overwrite);
            Assert.AreEqual(true, File.Exists(target));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(target));
        }

        [Test]
        [Sequential]
        public void CopyAndRollbackAsync([Values(false, true)] bool overwrite)
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestCopyRollbackAsync");
            string target = Path.Combine(AppBase, "TestCopyRollbackTargetAsync");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            io.BeginTransaction();
            io.File.CopyAsync(path, target, overwrite).Wait();
            Assert.AreEqual(true, File.Exists(target));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(target));
        }

        [Test]
        [Sequential]
        public void CopyAndRollbackWithBuffer([Values(false, true)] bool overwrite)
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestCopyRollbackBuffer");
            string target = Path.Combine(AppBase, "TestCopyRollbackBufferTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            io.BeginTransaction();
            io.File.Copy(path, target, 512, overwrite);
            Assert.AreEqual(true, File.Exists(target));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(target));
        }

        [Test]
        [Sequential]
        public void CopyAndRollbackWithBufferAsync([Values(false, true)] bool overwrite)
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "TestCopyRollbackBufferAsync");
            string target = Path.Combine(AppBase, "TestCopyRollbackBufferAsyncTarget");
            File.WriteAllText(path, new string(' ', 10000));
            File.Delete(target);

            io.BeginTransaction();
            io.File.CopyAsync(path, target, 512, overwrite).Wait();
            Assert.AreEqual(true, File.Exists(target));
            io.Rollback();
            Assert.AreEqual(false, File.Exists(target));
        }

        [Test]
        [Sequential]
        public void CopyDirectoryAndRollback([Values(false, true)] bool overwrite)
        {
            IO io = new IO();
            string path = AppBase;
            string target = BinBase + "TestCopyRollbackTarget";

            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }

            io.BeginTransaction();
            io.Directory.Copy(path, target, overwrite);
            Assert.AreEqual(true, Directory.Exists(target));
            io.Rollback();
            Assert.AreEqual(false, Directory.Exists(target));
        }

        [Test]
        [Sequential]
        public void CopyDirectoryAndRollbackAsync([Values(false, true)] bool overwrite)
        {
            IO io = new IO();
            string path = AppBase;
            string target = BinBase + "TestCopyRollbackTargetAsync";

            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }

            io.BeginTransaction();
            io.Directory.CopyAsync(path, target, overwrite).Wait();
            Assert.AreEqual(true, Directory.Exists(target));
            io.Rollback();
            Assert.AreEqual(false, Directory.Exists(target));
        }

        [Test]
        [Sequential]
        public void CopyDirectoryAndRollbackWithBuffer([Values(false, true)] bool overwrite)
        {
            IO io = new IO();
            string path = AppBase;
            string target = BinBase + "TestCopyRollbackBufferTarget";

            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }

            io.BeginTransaction();
            io.Directory.Copy(path, target, 1024, overwrite);
            Assert.AreEqual(true, Directory.Exists(target));
            io.Rollback();
            Assert.AreEqual(false, Directory.Exists(target));
        }

        [Test]
        [Sequential]
        public void CopyDirectoryAndRollbackWithBufferAsync([Values(false, true)] bool overwrite)
        {
            IO io = new IO();
            string path = AppBase;
            string target = BinBase + "TestCopyRollbackBufferAsyncTarget";

            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }

            io.BeginTransaction();
            io.Directory.CopyAsync(path, target, 1024, overwrite).Wait();
            Assert.AreEqual(true, Directory.Exists(target));
            io.Rollback();
            Assert.AreEqual(false, Directory.Exists(target));
        }

        [Test]
        public async Task IgnoreExistingFile()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "IgnoreExistingFile");
            File.WriteAllText(path, new string(' ', 10000));
            await Task.Delay(50);
            try
            {
                io.BeginTransaction();
                io.File.Copy(path, path);
                await io.File.CopyAsync(path, path);
                io.File.Copy(path, path, 512);
                await io.File.CopyAsync(path, path, 512);
                io.Commit();
            }
            catch (Exception)
            {
                io.Rollback();
            }

            Assert.AreEqual(true, File.Exists(path));
        }

        [Test]
        public void OverwriteExistingFile()
        {
            IO io = new IO();
            string path = Path.Combine(AppBase, "OverwriteExistingFile");
            string target = Path.Combine(AppBase, "OverwriteExistingFileTarget");
            File.WriteAllText(path, new string(' ', 10000));

            Assert.That(async () =>
            {
                try
                {
                    io.BeginTransaction();
                    io.File.Copy(path, target, true);
                    await io.File.CopyAsync(path, target, true);
                    io.File.Copy(path, target, 512, true);
                    await io.File.CopyAsync(path, target, 512, true);
                    io.Commit();
                }
                catch (Exception)
                {
                    io.Rollback();
                }
            }, Throws.Nothing);

            Assert.AreEqual(true, File.Exists(target));
        }
    }
}
