using System;

namespace FileSender.Dependencies
{
    public class FileSenderTc
    {
        public DateTime CreationDate { get; }
        public string Format { get; }

        public bool Recognized { get; }
        public bool Sent { get; }

        public FileSenderTc(DateTime time, string format = "3.1", bool recognized = true, bool sent = true)
        {
            CreationDate = time;
            Format = format;
            Recognized = recognized;
            Sent = sent;
        }
    }
}
