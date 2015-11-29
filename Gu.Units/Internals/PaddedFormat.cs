namespace Gu.Units
{
    internal class PaddedFormat
    {
        internal readonly string PrePadding;
        internal readonly string Format;
        internal readonly string PostPadding;

        public PaddedFormat(string prePadding,
            string format,
            string postPadding)
        {
            this.PrePadding = prePadding;
            this.Format = format;
            this.PostPadding = postPadding;
        }
    }
}
