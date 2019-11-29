using System;
using System.Text;

namespace ExpressionInterpreter.Logic
{
	public class Interpreter
	{
		private double _operandLeft;
		private double _operandRight;
		private char _op;  // Operator                  

		/// <summary>
		/// Eingelesener Text
		/// </summary>
		public string ExpressionText { get; private set; }

		public double OperandLeft
		{
			get { return _operandLeft; }
		}

        public double OperandRight
        {
			get { return _operandRight; }

		}

		public char Op
        {
            get { return _op; }
        }


		public void Parse(string expressionText)
		{
			ExpressionText = expressionText;
			ParseExpressionStringToFields();
		}

        /// <summary>
        /// Wertet den Ausdruck aus und gibt das Ergebnis zurück.
        /// Fehlerhafte Operatoren und Division durch 0 werden über Exceptions zurückgemeldet
        /// </summary>
        public double Calculate()
        {
            double result = 0;

            switch (Op)
            {
                case '+':
                    result = OperandLeft + OperandRight;
                    break;
                case '-':
                    result = OperandLeft - OperandRight;
                    break;
                case '*':
                    result = OperandLeft * OperandRight;
                    break;
                case '/':
                    if (OperandRight == 0)
                    {
                        throw new InvalidOperationException("Division durch 0 ist nicht erlaubt");
                    }
                    else
                    {
                        result = OperandLeft / OperandRight;
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Expressionstring in seine Bestandteile zerlegen und in die Felder speichern.
        /// 
        ///     { }[-]{ }D{D}[,D{D}]{ }(+|-|*|/){ }[-]{ }D{D}[,D{D}]{ }
        ///     
        /// Syntax  OP = +-*/
        ///         Vorzeichen -
        ///         Zahlen double/int
        ///         Trennzeichen Leerzeichen zwischen OP, Vorzeichen und Zahlen
        /// </summary>
        public void ParseExpressionStringToFields()
        {
            if (String.IsNullOrEmpty(ExpressionText))
            {
                throw new ArgumentException("Ausdruck ist null oder empty!");
            }

            int pos = 0;

            // { }[-]{ }D{D}[,D{D}]{ }(+|-|*|/){ }[-]{ }D{D}[,D{D}]{ }

            try
            {
                _operandLeft = ScanNumber(ref pos);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Linker Operand ist fehlerhaft", ex);
            }

            _op = ScanOperator(ref pos);

            try
            {
                _operandRight = ScanNumber(ref pos);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Rechter Operand ist fehlerhaft", ex);
            }
        }

        /// <summary>
        /// DerOperator wird auf Gültigkeit überprüft.
        /// +, -, *, / sind zulässig
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private char ScanOperator(ref int pos)
        {
            SkipBlanks(ref pos);

            if (pos >= ExpressionText.Length)
            {
                throw new ArgumentException("Operator fehlt komplett");
            }

            char op = ExpressionText[pos];

            if (op != '+' &&
                op != '-' &&
                op != '*' &&
                op != '/')
            {
                throw new ArgumentException($"Operator {op} ist fehlerhaft!");
            }

            pos++;
            return op;
        }

		/// <summary>
		/// Ein Double muss mit einer Ziffer beginnen. Gibt es Nachkommastellen,
		/// müssen auch diese mit einer Ziffer beginnen.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		private double ScanNumber(ref int pos)
		{
            SkipBlanks(ref pos);

            if (pos >= ExpressionText.Length)
            {
                throw new ArgumentException("Zahl fehlt komplett");
            }

            double result;
            double sign = ScanSign(ref pos);

            try
            {
                result = ScanInteger(ref pos);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ganzzahlanteil ist fehlerhaft", ex);
            }
            
            if (pos < ExpressionText.Length && ExpressionText[pos].Equals(','))
            {
                pos++;
                int startPos = pos;
                try
                {
                    double decimalPlace = ScanInteger(ref pos);
                    result += decimalPlace / (Math.Pow(10, pos - startPos));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Nachkommaanteil ist fehlerhaft", ex);
                }
            }

            return result *= sign;
        }

        /// <summary>
        /// Es wird das negative Vorzeichen ausgewertet
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private double ScanSign(ref int pos)
        {
            SkipBlanks(ref pos);

            double sign = 1;
            
            if (ExpressionText[pos].Equals('-'))
            {
                sign = -sign;
                pos++;
            }
            return sign;
        }

		/// <summary>
		/// Eine Ganzzahl muss mit einer Ziffer beginnen.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		private int ScanInteger(ref int pos)
		{
            SkipBlanks(ref pos);
            int number = 0;

            if (pos >= ExpressionText.Length || 
                Char.IsDigit(ExpressionText[pos]) == false)
            {
                throw new ArgumentException("Integeranteil fehlt oder beginnt nicht mit Ziffer");
            }
            
            while (pos < ExpressionText.Length && Char.IsDigit(ExpressionText[pos]))
            {
                number = number * 10 + ExpressionText[pos] - '0';
                pos++;
            }

            return number;
        }

		/// <summary>
		/// Setzt die Position weiter, wenn Leerzeichen vorhanden sind
		/// </summary>
		/// <param name="pos"></param>
		private void SkipBlanks(ref int pos)
		{
            while (pos < ExpressionText.Length && Char.IsWhiteSpace(ExpressionText[pos]))
            {
                pos++;
            }
        }

		/// <summary>
		/// Exceptionmessage samt Innerexception-Texten ausgeben
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string GetExceptionTextWithInnerExceptions(Exception ex)
		{
            StringBuilder sb = new StringBuilder();
            sb.Append("Exceptionmessage: ");
            sb.AppendLine(ex.Message);

            Exception run = ex.InnerException;
            int counter = 1;

            while (run != null)
            {
                sb.Append($"Inner Exception {counter}: ");
                sb.AppendLine(run.Message);
                run = run.InnerException;
                counter++;
            }

            return sb.ToString();
		}
	}
}
