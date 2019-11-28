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

            if (Op.Equals('+'))
            {
                result = OperandLeft + OperandRight;
            }
            else if (Op.Equals('-'))
            {
                result = OperandLeft - OperandRight;
            }
            else if (Op.Equals('*'))
            {
                result = OperandLeft * OperandRight;
            }
            else if (Op.Equals('/'))
            {
                if (OperandRight != 0)
                {
                    result = OperandLeft / OperandRight;
                }
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
            int pos = 0;
            _operandLeft = ScanNumber(ref pos);
            SkipBlanks(ref pos);
            _op = ExpressionText[pos];
            SkipBlanks(ref pos);
            _operandRight = ScanNumber(ref pos);
            SkipBlanks(ref pos);
        }

		/// <summary>
		/// Ein Double muss mit einer Ziffer beginnen. Gibt es Nachkommastellen,
		/// müssen auch diese mit einer Ziffer beginnen.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		private double ScanNumber(ref int pos)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Eine Ganzzahl muss mit einer Ziffer beginnen.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		private int ScanInteger(ref int pos)
		{
			int number = 0;
			while(ExpressionText[pos] != ' ' && ExpressionText[pos] >= 48 && ExpressionText[pos] < 57 && ExpressionText.Length < pos)
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
			while(ExpressionText[pos] != ' ' && ExpressionText.Length < pos)
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
			throw new NotImplementedException();
		}
	}
}
