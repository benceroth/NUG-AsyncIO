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
    public class AsyncDirectory
    {
        private static readonly string AppBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        private static readonly DummyModel Model = new DummyModel();
        private AsyncIO asyncIO;

        [OneTimeSetUp]
        public void Init()
        {
            this.asyncIO = new AsyncIO();
            Directory.CreateDirectory(AppBase + "tests");
        }


        [Test]
        public void Copy()
        {
            string path = Path.Combine(AppBase, "tests");
            string target = Path.Combine(AppBase, "TestCopyTarget");
            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }

            void CopyAct() => this.asyncIO.Directory.Copy(path, target);
            Assert.That(() => CopyAct(), Throws.Nothing);
            Assert.AreEqual(Directory.Exists(target), true);
            Assert.AreEqual(
                Directory.GetFiles(path).Select(Path.GetFileName), 
                Directory.GetFiles(target).Select(Path.GetFileName));
        }

        [Test]
        public void CopyAsync()
        {
            string path = Path.Combine(AppBase, "tests");
            string target = Path.Combine(AppBase, "TestCopyAsyncTarget");
            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }

            async Task CopyAct() => await this.asyncIO.Directory.CopyAsync(path, target);
            Assert.That(async () => await CopyAct(), Throws.Nothing);
            Assert.AreEqual(Directory.Exists(target), true);
            Assert.AreEqual(
                Directory.GetFiles(path).Select(Path.GetFileName),
                Directory.GetFiles(target).Select(Path.GetFileName));
        }

        [Test]
        public void CopyWithBuffer()
        {
            string path = Path.Combine(AppBase, "tests");
            string target = Path.Combine(AppBase, "TestCopyBufferTarget");
            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }

            void CopyAct() => this.asyncIO.Directory.Copy(path, target, 512);
            Assert.That(() => CopyAct(), Throws.Nothing);
            Assert.AreEqual(Directory.Exists(target), true);
            Assert.AreEqual(
                Directory.GetFiles(path).Select(Path.GetFileName),
                Directory.GetFiles(target).Select(Path.GetFileName));
        }

        [Test]
        public void CopyWithBufferAsync()
        {
            string path = Path.Combine(AppBase, "tests");
            string target = Path.Combine(AppBase, "TestCopyBufferAsyncTarget");
            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }

            async Task CopyAct() => await this.asyncIO.Directory.CopyAsync(path, target, 512);
            Assert.That(async () => await CopyAct(), Throws.Nothing);
            Assert.AreEqual(Directory.Exists(target), true);
            Assert.AreEqual(
                Directory.GetFiles(path).Select(Path.GetFileName),
                Directory.GetFiles(target).Select(Path.GetFileName));
        }
    }
}
