namespace Gu.Units.Wpf
{
    public enum SymbolOptions
    {
        /// <summary>
        /// Same as <see cref="NotAllowed"/> without symbol in <see cref="System.Windows.Data.Binding.StringFormat"/>.
        /// Same as <see cref="Required"/> when <see cref="System.Windows.Data.Binding.StringFormat"/> contains symbol.
        /// </summary>
        Default,
        
        /// <summary>
        /// Valid input cannot contain a unit symbol
        /// </summary>
        NotAllowed,
       
        /// <summary>
        /// Input is valid with or without symbol.
        /// When no symbol the unit in the converter is used.
        /// </summary>
        Allowed,

        /// <summary>
        /// Valid input must contain a unit symbol.
        /// </summary>
        Required
    }
}