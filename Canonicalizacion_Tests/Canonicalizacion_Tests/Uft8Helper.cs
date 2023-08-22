using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Canonicalizacion_Tests
{
    class Uft8Helper
    {

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
                i += Canonicalizacion.charCount(c);
                if (!Canonicalizacion.isValidCodePoint(c) || c >= 0xD800 && c <= 0xDBFF || c >= 0xDC00 && c <= 0xDFFF)
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
        public static void writeStringToUtf8(String str, BinaryWriter swOut)
        {
            int length = str.Length;
            int i = 0;
            int c;
            while (i < length)
            {
                c = (int)str[i];
                i += Canonicalizacion.charCount(c);
                if (!Canonicalizacion.isValidCodePoint(c) || c >= 0xD800 && c <= 0xDBFF || c >= 0xDC00 && c <= 0xDFFF)
                {
                    // valid code point: c >= 0x0000 && c <= 0x10FFFF
                    swOut.Write((char)0x3f);
                    continue;
                }
                bool OLD_UTF8 = false;
                int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
                if (OLD_UTF8 && c >= MIN_SUPPLEMENTARY_CODE_POINT)
                {
                    // version 2 or before output 2 question mark characters for 32 bit chars
                    swOut.Write((char)0x3f);
                    swOut.Write((char)0x3f);
                    continue;
                }
                if (c < 0x80)
                {
                    swOut.Write((char)c);
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
                    swOut.Write((char)0x3f);
                    continue;
                }
                byte write;
                int shift = 6 * extraByte;
                write = (byte)((0xFE << (6 - extraByte)) | (c >> shift));
                swOut.Write(Convert.ToChar(write));
                for (int j = extraByte - 1; j >= 0; j--)
                {
                    shift -= 6;
                    write = (byte)(0x80 | ((c >> shift) & 0x3F));
                    swOut.Write(Convert.ToChar(write));
                }

            }

        }


        public static void writeCodePointToUtf8(int c, BinaryWriter swOutput)
        {
            int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
            if (!Canonicalizacion.isValidCodePoint(c) || c >= 0xD800 && c <= 0xDBFF || c >= 0xDC00 && c <= 0xDFFF)
            {
                // valid code point: c >= 0x0000 && c <= 0x10FFFF
                swOutput.Write((char)0x3f);
                return;
            }
            // OLD UTF8?
            bool OLD_UTF8 = false;
            if (OLD_UTF8 && c >= MIN_SUPPLEMENTARY_CODE_POINT)
            {
                // version 2 or before output 2 question mark characters for 32 bit chars
                swOutput.Write((char)0x3f);
                swOutput.Write((char)0x3f);
                return;
            }

            if (c < 0x80)
            {
                // 0x00000000 - 0x0000007F
                // 0xxxxxxx
                swOutput.Write((char)c);
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
                swOutput.Write((char)0x3f);
                return;
            }

            byte write;
            int shift = 6 * extraByte;

            write = (byte)((0xFE << (6 - extraByte)) | (c >> shift));
            swOutput.Write(Convert.ToChar(write));

            for (int i = extraByte - 1; i >= 0; i--)
            {
                shift -= 6;
                write = (byte)(0x80 | ((c >> shift) & 0x3F));
                swOutput.Write(Convert.ToChar(write));
            }
        }
    }
}
