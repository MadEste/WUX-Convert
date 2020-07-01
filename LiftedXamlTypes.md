# Lifted Xaml Type 
## Namespaces to target with the converter

Easier to simply load all the . namespaces or target all and remove the odd ones out?

### Windows.UI.Xaml (minus some types WIP)
### Windows.UI
- Windows.UI.Color (Stays)
- Windows.UI.Colors (Lifting)
- Windows.UI.ColorHelper (Lifting)

### Windows.UI.Text
Xaml has Text Object Model (TOM) types in the `Windows.UI.Text` namespace:
* CaretType
* FindOptions
* FontStretch (enum)
* FontStyle (enum)
* FontWeight (struct)
* FormatEffect
* HorizontalCharacterAlignment
* IRichEditTextRange
* ITextCharacterFormat
* ITextDocument
* ITextParagraphFormat
* ITextRange
* ITextSelection
* LetterCase
* LineSpacingRule
* LinkType
* MarkerAlignment
* MarkerStyle
* MarkerType
* ParagraphAlignment
* ParagraphStyle
* PointOptions
* RangeGravity
* RichEditTextDocument
* RichEditTextRange
* SelectionOptions
* SelectionType
* TabAlignment
* TabLeader
* TextConstants
* TextDecorations
* TextGetOptions
* TextRangeUnit
* TextScript
* TextSetOptions
* UnderlineType (enum)
* VerticalCharacterAlignment
 
Most of these are only used by Xaml (only referenced by types in Windows.UI.Xaml). The exceptions:
* `FontStretch`, `FontStyle`, `FontWeight`
* `UnderlineType` 

Not everything in Windows.UI.Text is part of Xaml.Today, Windows.UI.Text itself is almost entirely Xaml, with the non-Xaml types in Windows.UI.Text.Core.

All of the above listed types will be lifted to DCPP. 

### To Lift:
 
- Windows.UI.Xaml
- Windows.UI.Colors
- Windows.UI.ColorHelper
- Windows.UI.Text
- NOT windows.UI.Text.Core