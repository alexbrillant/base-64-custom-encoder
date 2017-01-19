using System;
using System.Collections.Generic;
using System.Collections;

public class Program
{
    public static void Main(string[] args)
    {
        byte[] bytes = {0x6A, 0x77, 0xC4, 0x6A, 0x77, 0xC4};
        String results = Base64.Encode(bytes);
        Console.WriteLine(results);
    }
}

public static class Base64 {
    private const int BYTE_GROUP_LENGTH = 3;
    private const byte FIRST_6_BITS_MASK = 0xFC;
    private const byte LAST_6_BITS_MASK = 0x3F;
    private const byte FIRST_2_BITS_MASK = 0xc0;
    private const byte LAST_4_BITS_MASK = 0xF;
    private const byte LAST_2_BITS_MASK = 0x3;
    private const byte FIRST_4_BITS_MASK = 0xF0;

    public static String Encode(byte[] bytes) {
        int byteGroupCount = bytes.Length / BYTE_GROUP_LENGTH;
        string encodedBytes = "";

        for (int i = 0; i < byteGroupCount; i++) {
            int firstByteOfGroupIndex = (i * BYTE_GROUP_LENGTH);
            byte byte1 = bytes[firstByteOfGroupIndex],
            byte2 = bytes[firstByteOfGroupIndex + 1],
            byte3 = bytes[firstByteOfGroupIndex + 2];

            int char1 = bitwiseAnd(byte1, FIRST_6_BITS_MASK) >> 2;

            int char2 = bitwiseAnd(byte1, LAST_2_BITS_MASK) << 4;
            char2 += bitwiseAnd(byte2, FIRST_4_BITS_MASK) >> 4;

            int char3 = bitwiseAnd(byte2, LAST_4_BITS_MASK) << 2;
            char3 += bitwiseAnd(byte3, FIRST_2_BITS_MASK) >> 6;

            int char4 = bitwiseAnd(byte3, LAST_6_BITS_MASK);

            encodedBytes += alphabet[char1];
            encodedBytes += alphabet[char2];
            encodedBytes += alphabet[char3];
            encodedBytes += alphabet[char4];
        }
        return encodedBytes;
    }

    private static int bitwiseAnd(byte currentByte, byte mask) {
        BitArray bitArray = new BitArray(new byte[] { currentByte });
        BitArray bitArrayMask = new BitArray(new byte[] { mask });
        bitArrayMask.And(bitArray);
        byte[] bytes = new byte[1];
        bitArrayMask.CopyTo(bytes, 0);
        return (int) bytes[0];
    }

    private static IReadOnlyList<char> alphabet = (IReadOnlyList<char>) new List<char>() {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
        'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
        'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
        'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
        'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
        'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
        'w', 'x', 'y', 'z', '0', '1', '2', '3',
        '4', '5', '6', '7', '8', '9', '/', '+'
    };
}
