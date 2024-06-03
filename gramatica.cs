using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
using com.calitha.commons;

namespace com.calitha.goldparser
{

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF        =  0, // (EOF)
        SYMBOL_ERROR      =  1, // (Error)
        SYMBOL_WHITESPACE =  2, // Whitespace
        SYMBOL_MINUS      =  3, // '-'
        SYMBOL_LPAREN     =  4, // '('
        SYMBOL_RPAREN     =  5, // ')'
        SYMBOL_TIMES      =  6, // '*'
        SYMBOL_COMMA      =  7, // ','
        SYMBOL_DIV        =  8, // '/'
        SYMBOL_CARET      =  9, // '^'
        SYMBOL_PLUS       = 10, // '+'
        SYMBOL_COS        = 11, // cos
        SYMBOL_DECIMAL    = 12, // Decimal
        SYMBOL_ENTERO     = 13, // Entero
        SYMBOL_EXP        = 14, // exp
        SYMBOL_SIN        = 15, // sin
        SYMBOL_SQRT       = 16, // sqrt
        SYMBOL_TAN        = 17, // tan
        SYMBOL_E          = 18, // <E>
        SYMBOL_F          = 19, // <F>
        SYMBOL_G          = 20, // <G>
        SYMBOL_H          = 21, // <H>
        SYMBOL_I          = 22, // <I>
        SYMBOL_J          = 23, // <J>
        SYMBOL_K          = 24, // <K>
        SYMBOL_T          = 25  // <T>
    };

    enum RuleConstants : int
    {
        RULE_E_PLUS                     =  0, // <E> ::= <E> '+' <T>
        RULE_E_MINUS                    =  1, // <E> ::= <E> '-' <T>
        RULE_E                          =  2, // <E> ::= <T>
        RULE_T_TIMES                    =  3, // <T> ::= <T> '*' <F>
        RULE_T_DIV                      =  4, // <T> ::= <T> '/' <F>
        RULE_T                          =  5, // <T> ::= <F>
        RULE_F_LPAREN_RPAREN            =  6, // <F> ::= '(' <E> ')'
        RULE_F_ENTERO                   =  7, // <F> ::= Entero
        RULE_F_DECIMAL                  =  8, // <F> ::= Decimal
        RULE_F                          =  9, // <F> ::= <G>
        RULE_G_SIN_LPAREN_RPAREN        = 10, // <G> ::= sin '(' <E> ')'
        RULE_G                          = 11, // <G> ::= <H>
        RULE_H_COS_LPAREN_RPAREN        = 12, // <H> ::= cos '(' <E> ')'
        RULE_H                          = 13, // <H> ::= <I>
        RULE_I_TAN_LPAREN_RPAREN        = 14, // <I> ::= tan '(' <E> ')'
        RULE_I                          = 15, // <I> ::= <J>
        RULE_J_EXP_CARET_LPAREN_RPAREN  = 16, // <J> ::= exp '^' '(' <E> ')'
        RULE_J                          = 17, // <J> ::= <K>
        RULE_K_SQRT_LPAREN_COMMA_RPAREN = 18  // <K> ::= sqrt '(' <E> ',' <E> ')'
    };

    public class MyParser
    {
        private LALRParser parser;

        public MyParser(string filename)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public MyParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public MyParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

            parser.OnReduce += new LALRParser.ReduceHandler(ReduceEvent);
            parser.OnTokenRead += new LALRParser.TokenReadHandler(TokenReadEvent);
            parser.OnAccept += new LALRParser.AcceptHandler(AcceptEvent);
            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
        }

        public void Parse(string source)
        {
            parser.Parse(source);

        }

        private void TokenReadEvent(LALRParser parser, TokenReadEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
            }
            catch (Exception e)
            {
                args.Continue = false;
                //todo: Report message to UI?
            }
        }

        private Object CreateObject(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                    //'-'
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_LPAREN :
                    //'('
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_RPAREN :
                    //')'
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_TIMES :
                    //'*'
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_COMMA :
                    //','
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_DIV :
                    //'/'
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_CARET :
                    //'^'
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_PLUS :
                    //'+'
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_COS :
                    //cos
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_DECIMAL :
                    //Decimal
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_ENTERO :
                    //Entero
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_EXP :
                    //exp
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_SIN :
                    //sin
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_SQRT :
                    //sqrt
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_TAN :
                    //tan
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_E :
                //<E>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_F :
                //<F>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_G :
                //<G>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_H :
                //<H>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_I :
                //<I>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_J :
                //<J>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_K :
                //<K>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_T :
                //<T>
                //todo: Create a new object that corresponds to the symbol
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        private void ReduceEvent(LALRParser parser, ReduceEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
            }
            catch (Exception e)
            {
                args.Continue = false;
                //todo: Report message to UI?
            }
        }

        public static Object CreateObject(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_E_PLUS :
                    //<E> ::= <E> '+' <T>
                    //return Convert.ToInt32(token.Tokens[0].UserObject) + Convert.ToInt32(token.Tokens[2].UserObject);
                    return Convert.ToDouble(token.Tokens[0].UserObject) + Convert.ToDouble(token.Tokens[2].UserObject);
                    //double result = Convert.ToDouble(token.Tokens[0].UserObject) + Convert.ToDouble(token.Tokens[2].UserObject);

                case (int)RuleConstants.RULE_E_MINUS :
                    //<E> ::= <E> '-' <T>
                    //return Convert.ToInt32(token.Tokens[0].UserObject) - Convert.ToInt32(token.Tokens[2].UserObject);
                    return Convert.ToDouble(token.Tokens[0].UserObject) - Convert.ToDouble(token.Tokens[2].UserObject);

                case (int)RuleConstants.RULE_E :
                    //<E> ::= <T>
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_T_TIMES :
                    //<T> ::= <T> '*' <F>
                    //return Convert.ToInt32(token.Tokens[0].UserObject) * Convert.ToInt32(token.Tokens[2].UserObject);
                    return Convert.ToDouble(token.Tokens[0].UserObject) * Convert.ToDouble(token.Tokens[2].UserObject);

                case (int)RuleConstants.RULE_T_DIV :
                    //<T> ::= <T> '/' <F>
                    //return Convert.ToInt32(token.Tokens[0].UserObject) / Convert.ToInt32(token.Tokens[2].UserObject);
                    return Convert.ToDouble(token.Tokens[0].UserObject) / Convert.ToDouble(token.Tokens[2].UserObject);

                case (int)RuleConstants.RULE_T :
                    //<T> ::= <F>
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_F_LPAREN_RPAREN :
                    //<F> ::= '(' <E> ')'
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_F_ENTERO :
                    //<F> ::= Entero
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_F_DECIMAL :
                    //<F> ::= Decimal
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_F :
                    //<F> ::= <G>
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_G_SIN_LPAREN_RPAREN :
                    //<G> ::= sin '(' <E> ')'
                    double anguloEnGradosSIN = Convert.ToDouble(token.Tokens[2].UserObject); // VALOR DEL ANGULO A INGRESAR
                    double anguloEnRadianesSIN = anguloEnGradosSIN * Math.PI / 180; // CONVIERTE A RADIANES
                    double seno = Math.Sin(anguloEnRadianesSIN); // CALCULA EL SENO
                    return seno;

                case (int)RuleConstants.RULE_G :
                    //<G> ::= <H>
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_H_COS_LPAREN_RPAREN :
                    //<H> ::= cos '(' <E> ')'
                    double anguloEnGradosCOS = Convert.ToDouble(token.Tokens[2].UserObject); // VALOR DEL ANGULO A INGRESAR
                    double anguloEnRadianesCOS = anguloEnGradosCOS * Math.PI / 180; // CONVIERTE A RADIANES
                    double coseno = Math.Cos(anguloEnRadianesCOS); // CALCULA LA TANGENTE
                    return coseno;

                case (int)RuleConstants.RULE_H :
                    //<H> ::= <I>
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_I_TAN_LPAREN_RPAREN :
                    //<I> ::= tan '(' <E> ')'
                    double anguloEnGradosTAN = Convert.ToDouble(token.Tokens[2].UserObject); // VALOR DEL ANGULO A INGRESAR
                    double anguloEnRadianesTAN = anguloEnGradosTAN * Math.PI / 180; // CONVIERTE A RADIANES
                    double tangente = Math.Tan(anguloEnRadianesTAN); // CALCULA LA TANGENTE
                    return tangente;

                case (int)RuleConstants.RULE_I :
                    //<I> ::= <J>
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_J_EXP_CARET_LPAREN_RPAREN :
                    //<J> ::= exp '^' '(' <E> ')'
                    double number = Convert.ToDouble(token.Tokens[3].UserObject); //VALOR DE X A INGRESAR
                    double exponente = Math.Pow(Math.E, number); //CALCULA EL VALOR DE E ELEVADO A LA X
                    return exponente;

                case (int)RuleConstants.RULE_J :
                    //<J> ::= <K>
                    return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_K_SQRT_LPAREN_COMMA_RPAREN :
                    //<K> ::= sqrt '(' <E> ',' <E> ')'
                    double valor = Convert.ToDouble(token.Tokens[2].UserObject); // Valor al que se debe sacar la raiz
                    double indice = Convert.ToDouble(token.Tokens[4].UserObject); // El segundo <E> es el índice de la raíz (La Enesima)
                    double raiz = Math.Pow(valor, 1.0 / indice);
                    return raiz;

            }
            throw new RuleException("Unknown rule");
        }

        public string resultado;
        private void AcceptEvent(LALRParser parser, AcceptEventArgs args)
        {
            try
            {
                resultado = Convert.ToString(args.Token.UserObject);
            }
            catch (Exception e)
            {
                resultado = "Error";
            }
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '"+args.Token.ToString()+"'";
            resultado = "Error Léxico: " + args.Token.ToString();
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '"+args.UnexpectedToken.ToString()+"'";
            resultado = "Error Sintáctico: " + args.UnexpectedToken.ToString();
        }


    }
}
