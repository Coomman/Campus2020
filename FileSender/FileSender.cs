using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FakeItEasy;
using FileSender.Dependencies;
using NUnit.Framework;

namespace FileSender
{
    public class FileSender
    {
        private readonly ICryptographer cryptographer;
        private readonly ISender sender;
        private readonly IRecognizer recognizer;

        public FileSender(ICryptographer cryptographer, ISender sender, IRecognizer recognizer)
        {
            this.cryptographer = cryptographer;
            this.sender = sender;
            this.recognizer = recognizer;
        }

        public Result SendFiles(File[] files, X509Certificate certificate)
        {
            return new Result
            {
                SkippedFiles = files
                    .Where(file => !TrySendFile(file, certificate))
                    .ToArray()
            };
        }

        private bool TrySendFile(File file, X509Certificate certificate)
        {
            if (!recognizer.TryRecognize(file, out var document))
                return false;

            if (!CheckFormat(document) || !CheckActual(document))
                return false;

            var signedContent = cryptographer.Sign(document.Content, certificate);
            return sender.TrySend(signedContent);
        }

        private bool CheckFormat(Document document)
        {
            return document.Format == "4.0" ||
                   document.Format == "3.1";
        }
        private bool CheckActual(Document document)
        {
            return document.Created.AddMonths(1) > DateTime.Now;
        }

        public class Result
        {
            public File[] SkippedFiles { get; set; }
        }
    }

    [TestFixture]
    public class FileSender_Should
    {
        private FileSender fileSender;
        private ICryptographer cryptographer;
        private ISender sender;
        private IRecognizer recognizer;

        private readonly X509Certificate certificate = new X509Certificate();
        private File file;
        private byte[] signedContent;

        [SetUp]
        public void SetUp()
        {
            file = new File("someFile", new byte[] {1, 2, 3});
            signedContent = new byte[] {1, 7};

            cryptographer = A.Fake<ICryptographer>();
            sender = A.Fake<ISender>();
            recognizer = A.Fake<IRecognizer>();
            fileSender = new FileSender(cryptographer, sender, recognizer);
        }

        public static IEnumerable<TestCaseData> GetOneFileTestCases()
        {
            yield return new TestCaseData(
                    new FileSenderTc(DateTime.Now))
                .Returns(false).SetName("Send_WhenGoodFormat_3.1");

            yield return new TestCaseData(
                    new FileSenderTc(DateTime.Now, "4.0"))
                .Returns(false).SetName("Send_WhenGoodFormat_4.0");

            yield return new TestCaseData(
                    new FileSenderTc(DateTime.Now, "2.0", true, false))
                .Returns(true).SetName("Skip_WhenBadFormat");

            yield return new TestCaseData(
                    new FileSenderTc(DateTime.Now, "3.1", false))
                .Returns(true).SetName("Skip_WhenNotRecognized");
        }

        public static IEnumerable<TestCaseData> GetMultipleFilesTestCases()
        {
            var correctFileCase = new FileSenderTc(DateTime.Now);
            var invalidFileCase = new FileSenderTc(DateTime.Now, "2.0", false);
            var notSentFileCase = new FileSenderTc(DateTime.Now, sent: false);

            yield return new TestCaseData(
                    (object) new[]
                        {correctFileCase, correctFileCase, invalidFileCase, correctFileCase, invalidFileCase})
                .Returns(true).SetName("IndependentlySend_WhenSeveralFilesAndSomeAreInvalid");

            yield return new TestCaseData(
                    (object) new[]
                        {correctFileCase, notSentFileCase, correctFileCase, correctFileCase, notSentFileCase})
                .Returns(true).SetName("IndependentlySend_WhenSeveralFilesAndSomeCouldNotSend");
        }


        [TestCaseSource(nameof(GetOneFileTestCases))]
        public bool OneFileTests(FileSenderTc testFile)
        {
            var document = 
                new Document(file.Name, file.Content, testFile.CreationDate, testFile.Format);

            A.CallTo(() => recognizer.TryRecognize(file, out document))
                .Returns(testFile.Recognized);
            A.CallTo(() => cryptographer.Sign(document.Content, certificate))
                .Returns(signedContent);
            A.CallTo(() => sender.TrySend(signedContent))
                .Returns(testFile.Sent);

            return fileSender.SendFiles(new[] {file}, certificate)
                .SkippedFiles.Any();
        }

        [TestCaseSource(nameof(GetMultipleFilesTestCases))]
        public bool MultipleFilesTests(FileSenderTc[] testFiles)
        {
            int skipFilesCount = 0;
            foreach (var tf in testFiles)
            {
                var doc = 
                    new Document(file.Name, file.Content, tf.CreationDate, tf.Format);

                A.CallTo(() => recognizer.TryRecognize(file, out doc))
                    .Returns(tf.Recognized);
                A.CallTo(() => cryptographer.Sign(doc.Content, certificate))
                    .Returns(signedContent);
                A.CallTo(() => sender.TrySend(signedContent))
                    .Returns(tf.Sent);

                if (fileSender.SendFiles(new[] {file}, certificate).SkippedFiles.Any())
                    skipFilesCount++;
            }

            return skipFilesCount != testFiles.Length;
        }
    }
}
