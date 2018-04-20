// ============================================================================
// DOMAIN: Defines the Markdown paragraph structure
// ============================================================================

namespace Markdown

type MarkdownSpan =
  | Literal of string
  | InlineCode of string
  | Strong of MarkdownSpans
  | Footnote of int * MarkdownSpans
  | Emphasis of MarkdownSpans
  | HyperLink of MarkdownSpans * string
  | Image of string * string
  | HardLineBreak

and MarkdownSpans = list<MarkdownSpan>

