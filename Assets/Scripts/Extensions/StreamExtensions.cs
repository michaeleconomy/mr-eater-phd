using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public static class StreamExtensions {
    public static void Write(this Stream stream, int i) {
        var bytes = BitConverter.GetBytes(i);
        stream.Write(bytes);
    }

    public static void Write(this Stream stream, ushort i) {
        var bytes = BitConverter.GetBytes(i);
        stream.Write(bytes);
    }

    public static void Write(this Stream stream, byte[] bytes) {
        stream.Write(bytes, 0, bytes.Length);
    }

    public static void WriteWithSize(this Stream stream, string s) {
        var bytes = Encoding.UTF8.GetBytes(s);
        stream.Write(bytes.Length);
        stream.Write(bytes);
    }

    public static int ReadInt(this Stream stream) {
        var bytes = new byte[4];
        stream.Read(bytes, 0, 4);
        return BitConverter.ToInt32(bytes, 0);
    }

    public static ushort ReadUshort(this Stream stream) {
        var bytes = new byte[2];
        stream.Read(bytes, 0, 2);
        return BitConverter.ToUInt16(bytes, 0);
    }

    public static string ReadString(this Stream stream) {
        var length = stream.ReadInt();
        var bytes = new byte[length];
        stream.Read(bytes, 0, length);
        return Encoding.UTF8.GetString(bytes);
    }
}
