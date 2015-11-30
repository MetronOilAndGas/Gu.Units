namespace Gu.Units
{
    internal struct PaddedFormat
    {
        internal static readonly PaddedFormat NullFormat = new PaddedFormat(null, null, null);

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

        internal static PaddedFormat CreateUnknown(string prePadding, string postPadding)
        {
            return new PaddedFormat(prePadding, FormatCache.UnknownFormat, postPadding);
        }

        internal PaddedFormat AsUnknownFormat()
        {
            return new PaddedFormat(this.PrePadding, FormatCache.UnknownFormat, this.PostPadding);
        }
    }
}
