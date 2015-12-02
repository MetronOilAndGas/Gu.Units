namespace Gu.Units.Wpf
{
    public enum UnitInput
    {
        /// <summary>
        /// Same as <see cref="ScalarOnly"/> without symbol in <see cref="System.Windows.Data.Binding.StringFormat"/>.
        /// Same as <see cref="SymbolRequired"/> when <see cref="System.Windows.Data.Binding.StringFormat"/> contains symbol.
        /// </summary>
        Default,
        
        /// <summary>
        /// Valid input cannot contain a unit symbol
        /// </summary>
        ScalarOnly,
       
        /// <summary>
        /// Input is valid with or without symbol.
        /// When no symbol the unit in the converter is used.
        /// </summary>
        SymbolAllowed,

        /// <summary>
        /// Valid input must contain a unit symbol.
        /// </summary>
        SymbolRequired
    }
}