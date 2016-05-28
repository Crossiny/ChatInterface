using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace ChatInterface
{
    /// <summary>
    /// Kombinierter StreamReader und StreamWriter mit vereinfachter Funktionalität.
    /// </summary>
    class StreamRW
    {
        StreamReader streamReader;
        StreamWriter streamWriter;

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob der StreamWriter nach jedem Aufruf
        /// von StreamRW.WriteLine(System.String) den Puffer in den zugrunde liegenden
        /// Stream wegschreibt, oder legt diesen fest.
        /// </summary>
        public bool AutoFlush
        {
            set
            {
                streamWriter.AutoFlush = value;
            }
            get
            {
                return streamWriter.AutoFlush;
            }
        }

        public StreamRW(Stream stream)
        {
            streamReader = new StreamReader(stream);
            streamWriter = new StreamWriter(stream);
            AutoFlush = true;
        }

        /// <summary>
        /// Liest eine Zeile von Zeichen aus dem aktuellen Stream und gibt die Daten als Zeichenfolge zurück.
        /// </summary>
        /// <returns>Die nächste Zeile des Eingabestreams, bzw. null, wenn das Ende des Eingabestreams erreicht ist.</returns>
        public string ReadLine()
        {
            return streamReader.ReadLine();
        }

        /// <summary>
        /// Schreibt eine Zeichenfolge, gefolgt von einem Zeichen für den Zeilenabschluss, in die Textzeichenfolge oder den Stream.
        /// </summary>
        /// <param name="Value">Die zu schreibende Zeichenfolge. Wenn value null ist, wird nur das Zeichen für den Zeilenabschluss geschrieben.</param>
        public void WriteLine(string Value)
        {
            streamWriter.WriteLine(Value);
        }
    }
}
