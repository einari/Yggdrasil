using System.Globalization;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static bool StartsWith(this String target, String value)
        {
            if( target.Length < value.Length ) return false;

            var matchCount = 0;
            for (var i = 0; i < value.Length; i++) if (target[i] == value[i]) matchCount++;
            return matchCount == value.Length;
        }


        public static bool Contains(this String target, String value)
        {
            if (target.Length < value.Length) return false;

            for (var a = 0; a < target.Length - value.Length; a++)
            {
                var matchCount = 0;
                for (var i = 0; i < value.Length; i++) if (target[a+i] == value[i]) matchCount++;
                if (matchCount == value.Length) return true;
            }

            return false;
        }

        /* helpers used by the runtime as well as above or eslewhere in corlib */
        internal static unsafe void memset(byte* dest, int val, int len)
        {
            if (len < 8)
            {
                while (len != 0)
                {
                    *dest = (byte)val;
                    ++dest;
                    --len;
                }
                return;
            }
            if (val != 0)
            {
                val = val | (val << 8);
                val = val | (val << 16);
            }
            // align to 4
            int rest = (int)dest & 3;
            if (rest != 0)
            {
                rest = 4 - rest;
                len -= rest;
                do
                {
                    *dest = (byte)val;
                    ++dest;
                    --rest;
                } while (rest != 0);
            }
            while (len >= 16)
            {
                ((int*)dest)[0] = val;
                ((int*)dest)[1] = val;
                ((int*)dest)[2] = val;
                ((int*)dest)[3] = val;
                dest += 16;
                len -= 16;
            }
            while (len >= 4)
            {
                ((int*)dest)[0] = val;
                dest += 4;
                len -= 4;
            }
            // tail bytes
            while (len > 0)
            {
                *dest = (byte)val;
                dest++;
                len--;
            }
        }

        static unsafe void memcpy4(byte* dest, byte* src, int size)
        {
            /*while (size >= 32) {
                // using long is better than int and slower than double
                // FIXME: enable this only on correct alignment or on platforms
                // that can tolerate unaligned reads/writes of doubles
                ((double*)dest) [0] = ((double*)src) [0];
                ((double*)dest) [1] = ((double*)src) [1];
                ((double*)dest) [2] = ((double*)src) [2];
                ((double*)dest) [3] = ((double*)src) [3];
                dest += 32;
                src += 32;
                size -= 32;
            }*/
            while (size >= 16)
            {
                ((int*)dest)[0] = ((int*)src)[0];
                ((int*)dest)[1] = ((int*)src)[1];
                ((int*)dest)[2] = ((int*)src)[2];
                ((int*)dest)[3] = ((int*)src)[3];
                dest += 16;
                src += 16;
                size -= 16;
            }
            while (size >= 4)
            {
                ((int*)dest)[0] = ((int*)src)[0];
                dest += 4;
                src += 4;
                size -= 4;
            }
            while (size > 0)
            {
                ((byte*)dest)[0] = ((byte*)src)[0];
                dest += 1;
                src += 1;
                --size;
            }
        }
        static unsafe void memcpy2(byte* dest, byte* src, int size)
        {
            while (size >= 8)
            {
                ((short*)dest)[0] = ((short*)src)[0];
                ((short*)dest)[1] = ((short*)src)[1];
                ((short*)dest)[2] = ((short*)src)[2];
                ((short*)dest)[3] = ((short*)src)[3];
                dest += 8;
                src += 8;
                size -= 8;
            }
            while (size >= 2)
            {
                ((short*)dest)[0] = ((short*)src)[0];
                dest += 2;
                src += 2;
                size -= 2;
            }
            if (size > 0)
                ((byte*)dest)[0] = ((byte*)src)[0];
        }
        static unsafe void memcpy1(byte* dest, byte* src, int size)
        {
            while (size >= 8)
            {
                ((byte*)dest)[0] = ((byte*)src)[0];
                ((byte*)dest)[1] = ((byte*)src)[1];
                ((byte*)dest)[2] = ((byte*)src)[2];
                ((byte*)dest)[3] = ((byte*)src)[3];
                ((byte*)dest)[4] = ((byte*)src)[4];
                ((byte*)dest)[5] = ((byte*)src)[5];
                ((byte*)dest)[6] = ((byte*)src)[6];
                ((byte*)dest)[7] = ((byte*)src)[7];
                dest += 8;
                src += 8;
                size -= 8;
            }
            while (size >= 2)
            {
                ((byte*)dest)[0] = ((byte*)src)[0];
                ((byte*)dest)[1] = ((byte*)src)[1];
                dest += 2;
                src += 2;
                size -= 2;
            }
            if (size > 0)
                ((byte*)dest)[0] = ((byte*)src)[0];
        }

        internal static unsafe void memcpy(byte* dest, byte* src, int size)
        {
            // FIXME: if pointers are not aligned, try to align them
            // so a faster routine can be used. Handle the case where
            // the pointers can't be reduced to have the same alignment
            // (just ignore the issue on x86?)
            if ((((int)dest | (int)src) & 3) != 0)
            {
                if (((int)dest & 1) != 0 && ((int)src & 1) != 0 && size >= 1)
                {
                    dest[0] = src[0];
                    ++dest;
                    ++src;
                    --size;
                }
                if (((int)dest & 2) != 0 && ((int)src & 2) != 0 && size >= 2)
                {
                    ((short*)dest)[0] = ((short*)src)[0];
                    dest += 2;
                    src += 2;
                    size -= 2;
                }
                if ((((int)dest | (int)src) & 1) != 0)
                {
                    memcpy1(dest, src, size);
                    return;
                }
                if ((((int)dest | (int)src) & 2) != 0)
                {
                    memcpy2(dest, src, size);
                    return;
                }
            }
            memcpy4(dest, src, size);
        }

        internal static unsafe void memcpy_aligned_1(byte* dest, byte* src, int size)
        {
            ((byte*)dest)[0] = ((byte*)src)[0];
        }

        internal static unsafe void memcpy_aligned_2(byte* dest, byte* src, int size)
        {
            ((short*)dest)[0] = ((short*)src)[0];
        }

        internal static unsafe void memcpy_aligned_4(byte* dest, byte* src, int size)
        {
            ((int*)dest)[0] = ((int*)src)[0];
        }

        internal static unsafe void memcpy_aligned_8(byte* dest, byte* src, int size)
        {
            ((long*)dest)[0] = ((long*)src)[0];
        }

        internal static unsafe void CharCopy(char* dest, char* src, int count)
        {
            // Same rules as for memcpy, but with the premise that 
            // chars can only be aligned to even addresses if their
            // enclosing types are correctly aligned
            if ((((int)(byte*)dest | (int)(byte*)src) & 3) != 0)
            {
                if (((int)(byte*)dest & 2) != 0 && ((int)(byte*)src & 2) != 0 && count > 0)
                {
                    ((short*)dest)[0] = ((short*)src)[0];
                    dest++;
                    src++;
                    count--;
                }
                if ((((int)(byte*)dest | (int)(byte*)src) & 2) != 0)
                {
                    memcpy2((byte*)dest, (byte*)src, count * 2);
                    return;
                }
            }
            memcpy4((byte*)dest, (byte*)src, count * 2);
        }



        internal static unsafe void CharCopyReverse(char* dest, char* src, int count)
        {
            dest += count;
            src += count;
            for (int i = count; i > 0; i--)
            {
                dest--;
                src--;
                *dest = *src;
            }
        }

        internal static unsafe void CharCopy(String target, int targetIndex, String source, int sourceIndex, int count)
        {
            fixed (char* dest = target, src = source)
                CharCopy(dest + targetIndex, src + sourceIndex, count);
        }

        internal static unsafe void CharCopy(String target, int targetIndex, Char[] source, int sourceIndex, int count)
        {
            fixed (char* dest = target, src = source)
                CharCopy(dest + targetIndex, src + sourceIndex, count);
        }

        static internal unsafe int IndexOfUnchecked(char value, int startIndex, int count)
        {
            // It helps JIT compiler to optimize comparison
            int value_32 = (int)value;

            fixed (char* start = &start_char)
            {
                char* ptr = start + startIndex;
                char* end_ptr = ptr + (count >> 3 << 3);

                while (ptr != end_ptr)
                {
                    if (*ptr == value_32)
                        return (int)(ptr - start);
                    if (ptr[1] == value_32)
                        return (int)(ptr - start + 1);
                    if (ptr[2] == value_32)
                        return (int)(ptr - start + 2);
                    if (ptr[3] == value_32)
                        return (int)(ptr - start + 3);
                    if (ptr[4] == value_32)
                        return (int)(ptr - start + 4);
                    if (ptr[5] == value_32)
                        return (int)(ptr - start + 5);
                    if (ptr[6] == value_32)
                        return (int)(ptr - start + 6);
                    if (ptr[7] == value_32)
                        return (int)(ptr - start + 7);

                    ptr += 8;
                }

                end_ptr += count & 0x07;
                while (ptr != end_ptr)
                {
                    if (*ptr == value_32)
                        return (int)(ptr - start);

                    ptr++;
                }
                return -1;
            }
        }

        static int length;
        static char start_char;

        // Following method is culture-insensitive
        public static unsafe String Replace(this String input, char oldChar, char newChar)
        {
            if (input.Length == 0 || oldChar == newChar)
                return input;

            int start_pos = IndexOfUnchecked(oldChar, 0, input.Length);
            if (start_pos == -1)
                return input;

            if (start_pos < 4)
                start_pos = 0;

            length = input.Length;

            string tmp = new string(new char[length]);
            fixed (char* dest = tmp, src = &start_char)
            {
                if (start_pos != 0)
                    CharCopy(dest, src, start_pos);

                char* end_ptr = dest + length;
                char* dest_ptr = dest + start_pos;
                char* src_ptr = src + start_pos;

                while (dest_ptr != end_ptr)
                {
                    if (*src_ptr == oldChar)
                        *dest_ptr = newChar;
                    else
                        *dest_ptr = *src_ptr;

                    ++src_ptr;
                    ++dest_ptr;
                }
            }
            return tmp;
        }
    }
}
