using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Canonicalizacion_Port
{
    class Uft8Helper
    {

        private static readonly char[] cEND_PI = { '?', '>' };
        private static readonly byte[] END_PI = Encoding.Unicode.GetBytes(cEND_PI);

        private static readonly char[] cBEGIN_PI = { '<', '?' };
        private static readonly byte[] BEGIN_PI = Encoding.Unicode.GetBytes(cBEGIN_PI);

        private static readonly char[] cEND_COMM = { '-', '-', '>' };
        private static readonly byte[] END_COMM = Encoding.Unicode.GetBytes(cEND_COMM);

        private static readonly char[] cBEGIN_COMM = { '<', '!', '-', '-' };
        private static readonly byte[] BEGIN_COMM = Encoding.Unicode.GetBytes(cBEGIN_COMM);

        private static readonly char[] cXA = { '&', '#', 'x', 'A', ';' };
        private static readonly byte[] XA = Encoding.Unicode.GetBytes(cXA);

        private static readonly char[] cX9 = { '&', '#', 'x', '9', ';' };
        private static readonly byte[] X9 = Encoding.Unicode.GetBytes(cX9);

        private static readonly char[] cQUOT = { '&', 'q', 'u', 'o', 't', ';' };
        private static readonly byte[] QUOT = Encoding.Unicode.GetBytes(cQUOT);

        private static readonly char[] cXD = { '&', '#', 'x', 'D', ';' };
        private static readonly byte[] XD = Encoding.Unicode.GetBytes(cXD);

        private static readonly char[] cGT = { '&', 'g', 't', ';' };
        private static readonly byte[] GT = Encoding.Unicode.GetBytes(cGT);

        private static readonly char[] cLT = { '&', 'l', 't', ';' };
        private static readonly byte[] LT = Encoding.Unicode.GetBytes(cLT);

        private static readonly char[] cEND_TAG = { '<', '/' };
        private static readonly byte[] END_TAG = Encoding.Unicode.GetBytes(cEND_TAG);

        private static readonly char[] cAMP = { '&', 'a', 'm', 'p', ';' };
        private static readonly byte[] AMP = Encoding.Unicode.GetBytes(cAMP);

        private static readonly char[] cEQUALS_STR = { '=', '\"' };
        private static readonly byte[] EQUALS_STR = Encoding.Unicode.GetBytes(cEQUALS_STR);


        protected const int NODE_BEFORE_DOCUMENT_ELEMENT = -1;
        protected const int NODE_NOT_BEFORE_OR_AFTER_DOCUMENT_ELEMENT = 0;
        protected const int NODE_AFTER_DOCUMENT_ELEMENT = 1;

        //EVALUAR SI PUEDE REEMPLAZARSE POR METODO EN c#
        public static byte[] getStringInUtf8(String str)
        {
            int length = str.Length;
            bool expanded = false;
            byte[] result = new byte[length];
            int i = 0;
            int Iout = 0;
            int c;
            while (i < length)
            {
                //Se cambia codepointat por [i]
                c = (int)str[i];
                i += charCount(c);
                if (!isValidCodePoint(c) || c >= 0xD800 && c <= 0xDBFF || c >= 0xDC00 && c <= 0xDFFF)
                {
                    // valid code point: c >= 0x0000 && c <= 0x10FFFF
                    result[Iout++] = (byte)0x3f;
                    continue;
                }
                // OLD UTF8?
                bool OLD_UTF8 = false;
                int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
                if (OLD_UTF8 && c >= MIN_SUPPLEMENTARY_CODE_POINT)
                {
                    // version 2 or before output 2 question mark characters for 32 bit chars
                    result[Iout++] = (byte)0x3f;
                    result[Iout++] = (byte)0x3f;
                    continue;
                }
                if (c < 0x80)
                {
                    result[Iout++] = (byte)c;
                    continue;
                }
                if (!expanded)
                {
                    byte[] newResult = new byte[6 * length];
                    //VERIFICAR COPY
                    Array.Copy(result, 0, newResult, 0, Iout);
                    result = newResult;
                    expanded = true;
                }
                byte extraByte = 0;
                if (c < 0x800)
                {
                    // 0x00000080 - 0x000007FF
                    // 110xxxxx 10xxxxxx
                    extraByte = 1;
                }
                else if (c < 0x10000)
                {
                    // 0x00000800 - 0x0000FFFF
                    // 1110xxxx 10xxxxxx 10xxxxxx
                    extraByte = 2;
                }
                else if (c < 0x200000)
                {
                    // 0x00010000 - 0x001FFFFF
                    // 11110xxx 10xxxxx 10xxxxxx 10xxxxxx
                    extraByte = 3;
                }
                else if (c < 0x4000000)
                {
                    // 0x00200000 - 0x03FFFFFF
                    // 111110xx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                    // already outside valid Character range, just for completeness
                    extraByte = 4;
                }
                else if (c <= 0x7FFFFFFF)
                {
                    // 0x04000000 - 0x7FFFFFFF
                    // 1111110x 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                    // already outside valid Character range, just for completeness
                    extraByte = 5;
                }
                else
                {
                    // 0x80000000 - 0xFFFFFFFF
                    // case not possible as java has no unsigned int
                    result[Iout++] = 0x3f;
                    continue;
                }
                byte write;
                int shift = 6 * extraByte;
                write = (byte)((0xFE << (6 - extraByte)) | (c >> shift));
                result[Iout++] = write;
                for (int j = extraByte - 1; j >= 0; j--)
                {
                    shift -= 6;
                    write = (byte)(0x80 | ((c >> shift) & 0x3F));
                    result[Iout++] = write;
                }
            }
            if (expanded)
            {
                byte[] newResult = new byte[Iout];
                Array.Copy(result, 0, newResult, 0, Iout);
                result = newResult;
            }
            return result;
        }
        //
        public static void writeStringToUtf8(String str, StreamWriter swOut)
        {
            int length = str.Length;
            int i = 0;
            int c;
            while (i < length)
            {
                c = (int)str[i];
                i += charCount(c);
                if (!isValidCodePoint(c) || c >= 0xD800 && c <= 0xDBFF || c >= 0xDC00 && c <= 0xDFFF)
                {
                    // valid code point: c >= 0x0000 && c <= 0x10FFFF
                    swOut.Write(0x3f);
                    continue;
                }
                bool OLD_UTF8 = false;
                int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
                if (OLD_UTF8 && c >= MIN_SUPPLEMENTARY_CODE_POINT)
                {
                    // version 2 or before output 2 question mark characters for 32 bit chars
                    swOut.Write(0x3f);
                    swOut.Write(0x3f);
                    continue;
                }
                if (c < 0x80)
                {
                    swOut.Write(c);
                    continue;
                }
                byte extraByte = 0;
                if (c < 0x800)
                {
                    // 0x00000080 - 0x000007FF
                    // 110xxxxx 10xxxxxx
                    extraByte = 1;
                }
                else if (c < 0x10000)
                {
                    // 0x00000800 - 0x0000FFFF
                    // 1110xxxx 10xxxxxx 10xxxxxx
                    extraByte = 2;
                }
                else if (c < 0x200000)
                {
                    // 0x00010000 - 0x001FFFFF
                    // 11110xxx 10xxxxx 10xxxxxx 10xxxxxx
                    extraByte = 3;
                }
                else if (c < 0x4000000)
                {
                    // 0x00200000 - 0x03FFFFFF
                    // 111110xx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                    // already outside valid Character range, just for completeness
                    extraByte = 4;
                }
                else if (c <= 0x7FFFFFFF)
                {
                    // 0x04000000 - 0x7FFFFFFF
                    // 1111110x 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                    // already outside valid Character range, just for completeness
                    extraByte = 5;
                }
                else
                {
                    // 0x80000000 - 0xFFFFFFFF
                    // case not possible as java has no unsigned int
                    swOut.Write(0x3f);
                    continue;
                }
                byte write;
                int shift = 6 * extraByte;
                write = (byte)((0xFE << (6 - extraByte)) | (c >> shift));
                swOut.Write(write);
                for (int j = extraByte - 1; j >= 0; j--)
                {
                    shift -= 6;
                    write = (byte)(0x80 | ((c >> shift) & 0x3F));
                    swOut.Write(write);
                }

            }

        }

        //
        /**
         * Outputs an Attribute to the internal Writer.
         *
         * The string value of the node is modified by replacing
         * <UL>
         * <LI>all ampersands (&) with <CODE>&amp;amp;</CODE></LI>
         * <LI>all open angle brackets (<) with <CODE>&amp;lt;</CODE></LI>
         * <LI>all quotation mark characters with <CODE>&amp;quot;</CODE></LI>
         * <LI>and the whitespace characters <CODE>#x9</CODE>, #xA, and #xD, with character
         * references. The character references are written in uppercase
         * hexadecimal with no leading zeroes (for example, <CODE>#xD</CODE> is represented
         * by the character reference <CODE>&amp;#xD;</CODE>)</LI>
         * </UL>
         *
         * @param name
         * @param value
         * @param writer 
         * @throws IOException
         */
        public static void outputAttrToWriter(String name, String value, StreamWriter writer, Dictionary<String, byte[]> cache)
        {
            writer.Write(' ');
            writeByte(name, writer, cache);
            writer.Write(EQUALS_STR.Clone());
            byte[] toWrite;
            int length = value.Length;
            int i = 0;
            while (i < length)
            {
                //Se cambia codePointAt
                int c = (int)value[i];
                i += charCount(c);

                switch (c)
                {

                    case '&':
                        toWrite = AMP;
                        break;

                    case '<':
                        toWrite = LT;
                        break;

                    case '"':
                        toWrite = QUOT;
                        break;

                    case 0x09:    // '\t'
                        toWrite = X9;
                        break;

                    case 0x0A:    // '\n'
                        toWrite = XA;
                        break;

                    case 0x0D:    // '\r'
                        toWrite = XD;
                        break;

                    default:
                        if (c < 0x80)
                        {
                            writer.Write(c);
                        }
                        else
                        {
                            writeCodePointToUtf8(c, writer);
                        }
                        continue;
                }
                writer.Write(toWrite);
            }

            writer.Write('\"');
        }


        /**
         * Outputs a Text of CDATA section to the internal Writer.
         *
         * @param text
         * @param writer writer where to write the things
         * @throws IOException
         */
        protected static void outputTextToWriter(String text, StreamWriter writer)
        {
            int length = text.Length;
            byte[] toWrite;
            for (int i = 0; i < length; )
            {
                int c = (int)text[i];
                i += charCount(c);
                //Se quitaron clones
                switch (c)
                {

                    case '&':
                        toWrite = AMP;
                        break;

                    case '<':
                        toWrite = LT;
                        break;

                    case '>':
                        toWrite = GT;
                        break;

                    case 0xD:
                        toWrite = XD;
                        break;

                    default:
                        if (c < 0x80)
                        {
                            writer.Write(c);
                        }
                        else
                        {
                            writeCodePointToUtf8(c, writer);
                        }
                        continue;
                }
                writer.Write(toWrite);
            }
        }

        /**
         * Method outputCommentToWriter
         *
         * @param currentComment
         * @param writer writer where to write the things
         * @throws IOException
         */
        protected void outputCommentToWriter(XmlComment currentComment, StreamWriter writer, int position)
        {
            if (position == NODE_AFTER_DOCUMENT_ELEMENT)
            {
                writer.Write('\n');
            }
            writer.Write(BEGIN_COMM.Clone());

            String data = currentComment.Data;
            int length = data.Length;

            for (int i = 0; i < length; )
            {
                int c = (int)data[i];
                i += charCount(c);
                if (c == 0x0D)
                {
                    writer.Write(XD.Clone());
                }
                else
                {
                    if (c < 0x80)
                    {
                        writer.Write(c);
                    }
                    else
                    {
                        writeCodePointToUtf8(c, writer);
                    }
                }
            }

            writer.Write(END_COMM.Clone());
            if (position == NODE_BEFORE_DOCUMENT_ELEMENT)
            {
                writer.Write('\n');
            }
        }

        /**
         * Outputs a PI to the internal Writer.
         *
         * @param currentPI
         * @param writer where to write the things
         * @throws IOException
         */
        protected void outputPItoWriter(XmlProcessingInstruction currentPI, StreamWriter writer, int position)
        {
            if (position == NODE_AFTER_DOCUMENT_ELEMENT)
            {
                writer.Write('\n');
            }
            writer.Write(BEGIN_PI.Clone());

            String target = currentPI.Target;
            int length = target.Length;

            for (int i = 0; i < length; )
            {
                int c = (int)target[i];
                i += charCount(c);
                if (c == 0x0D)
                {
                    writer.Write(XD.Clone());
                }
                else
                {
                    if (c < 0x80)
                    {
                        writer.Write(c);
                    }
                    else
                    {
                        writeCodePointToUtf8(c, writer);
                    }
                }
            }

            String data = currentPI.Data;

            length = data.Length;

            if (length > 0)
            {
                writer.Write(' ');

                for (int i = 0; i < length; )
                {
                    //Se reemplaza codetopoint por indice [i]
                    int c = (int)data[i];
                    i += charCount(c);
                    if (c == 0x0D)
                    {
                        writer.Write(XD.Clone());
                    }
                    else
                    {
                        writeCodePointToUtf8(c, writer);
                    }
                }
            }

            writer.Write(END_PI.Clone());
            if (position == NODE_BEFORE_DOCUMENT_ELEMENT)
            {
                writer.Write('\n');
            }
        }

        public static void writeCodePointToUtf8(int c, StreamWriter swOutput)
        {
            int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
            if (!isValidCodePoint(c) || c >= 0xD800 && c <= 0xDBFF || c >= 0xDC00 && c <= 0xDFFF)
            {
                // valid code point: c >= 0x0000 && c <= 0x10FFFF
                swOutput.Write(0x3f);
                return;
            }
            // OLD UTF8?
            bool OLD_UTF8 = false;
            if (OLD_UTF8 && c >= MIN_SUPPLEMENTARY_CODE_POINT)
            {
                // version 2 or before output 2 question mark characters for 32 bit chars
                swOutput.Write(0x3f);
                swOutput.Write(0x3f);
                return;
            }

            if (c < 0x80)
            {
                // 0x00000000 - 0x0000007F
                // 0xxxxxxx
                swOutput.Write(c);
                return;
            }
            byte extraByte = 0;
            if (c < 0x800)
            {
                // 0x00000080 - 0x000007FF
                // 110xxxxx 10xxxxxx
                extraByte = 1;
            }
            else if (c < 0x10000)
            {
                // 0x00000800 - 0x0000FFFF
                // 1110xxxx 10xxxxxx 10xxxxxx
                extraByte = 2;
            }
            else if (c < 0x200000)
            {
                // 0x00010000 - 0x001FFFFF
                // 11110xxx 10xxxxx 10xxxxxx 10xxxxxx
                extraByte = 3;
            }
            else if (c < 0x4000000)
            {
                // 0x00200000 - 0x03FFFFFF
                // 111110xx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                // already outside valid Character range, just for completeness
                extraByte = 4;
            }
            else if (c <= 0x7FFFFFFF)
            {
                // 0x04000000 - 0x7FFFFFFF
                // 1111110x 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx 10xxxxxx
                // already outside valid Character range, just for completeness
                extraByte = 5;
            }
            else
            {
                // 0x80000000 - 0xFFFFFFFF
                // case not possible as java has no unsigned int
                swOutput.Write(0x3f);
                return;
            }

            byte write;
            int shift = 6 * extraByte;

            write = (byte)((0xFE << (6 - extraByte)) | (c >> shift));
            swOutput.Write(write);

            for (int i = extraByte - 1; i >= 0; i--)
            {
                shift -= 6;
                write = (byte)(0x80 | ((c >> shift) & 0x3F));
                swOutput.Write(write);
            }
        }
        //
        public static bool isValidCodePoint(int codePoint)
        {
            int MAX_CODE_POINT = 0X10FFFF;
            // Optimized form of:
            //     codePoint >= MIN_CODE_POINT && codePoint <= MAX_CODE_POINT
            int plane = codePoint >> 16;
            return plane < ((MAX_CODE_POINT + 1) >> 16);
        }

        //
        public static int charCount(int codePoint)
        {
            int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
            return codePoint >= MIN_SUPPLEMENTARY_CODE_POINT ? 2 : 1;
        }
    }
}
