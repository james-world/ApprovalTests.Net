using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;
using NUnit.Framework;
using System.IO;

namespace ApprovalTests.Tests.Reporters
{
    [TestFixture]
    public class GenericDiffReporterTest
    {
        [Test]
        public void TestGetActualProgramFileEchos()
        {
            string NoneExistingFile = @"C:\ThisDirectoryShouldNotExist\ThisFileShouldNotExist.exe";
            Assert.AreEqual(NoneExistingFile, GenericDiffReporter.GetActualProgramFile(NoneExistingFile));
        }

        [Test]
        public void TestGetCurrentProject()
        {
            var file = PathUtilities.GetAdjacentFile("GenericDiffReporterTest.TestLaunchesBeyondCompareImage.approved.txt");
            string currentProjectFile = Path.GetFileName(VisualStudioProjectFileAdder.GetCurrentProjectFile(file));

            Assert.AreEqual("ApprovalTests.Tests.csproj", currentProjectFile);
        }

        [Test]
        public void TestGetCurrentProjectNotFound()
        {
            var project = VisualStudioProjectFileAdder.GetCurrentProjectFile("C:\\");

            Assert.AreEqual(null, project);
        }

        [Test]
        public void TestMissingDots()
        {
            var e =
                ExceptionUtilities.GetException(() => GenericDiffReporter.RegisterTextFileTypes(".exe", "txt", ".error", "asp"));
            Approvals.Verify(e);
        }

        [Test]
        public void TestProgramsExist()
        {
            Assert.IsFalse(new GenericDiffReporter("this_should_never_exist", "").IsWorkingInThisEnvironment("any.txt"));
        }

        [Test]
        public void TestRegisterWorks()
        {
            var r = new CodeCompareReporter();
            GenericDiffReporter.RegisterTextFileTypes(".myCrazyExtension");
            Assert.IsTrue(r.IsWorkingInThisEnvironment("file.myCrazyExtension"));
        }

        [Test]
        [UseReporter(typeof(ClipboardReporter))]
        public void TestEnsureFileExist()
        {
            var imageFile = PathUtilities.GetAdjacentFile("TestImage.png");
            if (File.Exists(imageFile))
            {
                File.Delete(imageFile);
            }
            GenericDiffReporter.EnsureFileExists(imageFile);
            Approvals.VerifyFile(imageFile);
        }
    }
}