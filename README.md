# Heart.Parsing
Pattern based string parsing

# Functionality
- Parsing Expression Grammar
- Operator Precedence Parsing

# Basic Usage Example
## Example Code
```csharp
using System;
using Heart.Parsing;
using Heart.Parsing.Patterns;

namespace demo
{
    class Program
    {
        static void Main()
        {
            // Text to match against
            string inputText = " [ a + b * c ] ";

            // Initialize ParserContext with inputText
            var ctx = new ParserContext(inputText);

            // Create PatternParser from grammar file
            var parser = ParsingHelper.BuildPatternParser("path/to/grammar/file");

            // Select the root pattern, explictally trim (left & right) using the _ pattern
            var pattern = parser.Patterns["root"].Trim(parser.Patterns["_"]);

            // Match the pattern against the ParserContext
            var node = pattern.TryMatch(parser, ctx);

            // Convert the IParseNode tree to a string for demonstration purposes
            // Will display expressions in the unambigious postfix notation
            string postfixNotation = StringCompiler.Compile(node);
            Console.WriteLine(postfixNotation);
        }
    }
}
```
## Example Grammar File
```
# Nonsignificant rule
# _ is used between sequences of matches implicitly to handle nonsignificant patterns e.g. whitespace and comments
# Uses `` to indicate regex
_ -> `\s*`

# Root rule
# Sequence of open square bracker, expression, close square bracket
# Uses '' to indicate plain text
root -> '[' expr ']'

# Expression rule
# Format
# Key LeftPrecedence RightPrecedence PegPattern
# Lower precedence results in tighter binding
expr ->
    [
        #WantOperand (Nullary, Prefix)
        '()'         none none '(' expr ')'
        'u+'         none 1    '+'
        'u-'         none 1    '-'
        '~'          none 1    '~'
        'real'       none none `\d+\.\d+`
        'integral'   none none `\d+`
        'boolean'    none none 'true' / 'false'
        'identifier' none none `[_a-zA-Z]\w*`

        #HaveOperand (Postfix, Infix)
        '$'          0    none '(' (expr (',' expr)*)? ')'
        '*'          3    3    '*'
        '/'          3    3    '/'
        '+'          4    4    '+'
        '-'          4    4    '-'
        '<='         5    5    '<='
        '>='         5    5    '>='
        '<'          5    5    '<'
        '>'          5    5    '>'
        '=='         6    6    '=='
        '!='         6    6    '!='
        '&'          7    7    '&'
        '^'          8    8    '^'
        '|'          9    9    '|'
        '?:'         10   99   '?' expr ':'
        '!'          2    none '!'
    ]
```
## Postfix Notation Output
```
[ (+ a (* b c)) ]
```
# How it works
## Parsing Expression Grammars
Recursive descent parsing using a pattern tree, recursion is handled by key-based lookups against `PatternParser.Patterns`
### Implemented rules:
  - Sequence: e1 e2
  - Ordered choice: e1 / e2
  - Zero-or-more: e*
  - One-or-more: e+
  - Optional: e?
  - And-predicate: &e
  - Not-predicate: !e

## Operator Precedence Parsing
### Precedence
  - Operators have 2 nullable precedences, `left?` and `right?`
  - Operators with equal precedence are left associative
  - By using asymmetric precedence, right associativity can be achieved
  - Precedence is parsed using a modified Shunting-yard algorithm

### Operator Categories based on precedence
  - `Nullary` (null, null)
  - `Prefix` (null, *)
  - `Postfix` (*, null)
  - `Infix` (*, *)

### Parsing States
  - There are 2 states while parsing `WantOperand` and `HaveOperand`
  - `Nullary`/`Prefix` operators can be matched during `WantOperand`
  - `Postfix`/`Infix` operators can be matched during `HaveOperand`
  - State is initialized to `WantOperand`
  - Parsing a `Nullary` operator will transition state from `WantOperand` -> `HaveOperand`
  - Parsing an `Infix` operator will transition state from `HaveOperand` -> `WantOperand`
  - Parsing a `Prefix` or `Postfix` operator will not cause a state transition allowing them to be chained

Operators match using an arbitrary `IPattern` which may contain a recursive `ExpressionPattern`, this is how parentheses, ternary and method call operators are handled

## Nonsignificant patterns
Whitespace and comments are handled using the reserved `_` rule name\
It is implicitly inserted between and only between any sequence of terminal matches (`SequencePattern`, `QuantifierPattern`, `ExpressionPattern`)

# Further Reading
## Parsing Expression Grammars
  - https://en.wikipedia.org/wiki/Parsing_expression_grammar
  - https://github.com/kragen/peg-bootstrap/blob/master/peg.md
## Operator Precedence Parsing
  - https://en.wikipedia.org/wiki/Shunting-yard_algorithm
  - https://stackoverflow.com/questions/16380234/handling-extra-operators-in-shunting-yard/16392115#16392115
  - https://github.com/matklad/minipratt
