using System;
using System.Drawing;
using System.Text;
using System.Drawing.Imaging;

namespace ImageSteganographyConsole.Models
{
    public class Steganography
    {
        public static Bitmap EncodeMessage(Bitmap bmp, string message)
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(message);
            int msgLength = msgBytes.Length;

            // Convert message length to bytes (4 bytes for int)
            byte[] lengthBytes = BitConverter.GetBytes(msgLength);
            byte[] finalBytes = new byte[lengthBytes.Length + msgBytes.Length];

            // Combine length and message
            Buffer.BlockCopy(lengthBytes, 0, finalBytes, 0, lengthBytes.Length);
            Buffer.BlockCopy(msgBytes, 0, finalBytes, lengthBytes.Length, msgBytes.Length);

            if (finalBytes.Length * 8 > bmp.Width * bmp.Height * 3)
                throw new Exception("Message too long for this image!");

            Bitmap newBmp = new Bitmap(bmp);
            BitmapData bmpData = newBmp.LockBits(new Rectangle(0, 0, newBmp.Width, newBmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                int byteIndex = 0, bitIndex = 0;

                for (int i = 0; i < bmpData.Stride * bmp.Height; i++)
                {
                    if (byteIndex < finalBytes.Length)
                    {
                        ptr[i] = (byte)((ptr[i] & 0xFE) | ((finalBytes[byteIndex] >> (7 - bitIndex)) & 1));
                        bitIndex++;

                        if (bitIndex == 8)
                        {
                            bitIndex = 0;
                            byteIndex++;
                        }
                    }
                    else break;
                }
            }

            newBmp.UnlockBits(bmpData);
            return newBmp;
        }

        public static string DecodeMessage(Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            byte[] extractedBytes = new byte[bmp.Width * bmp.Height * 3 / 8];
            int byteIndex = 0, bitIndex = 0;
            int messageLength = 0;
            bool lengthExtracted = false;

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                for (int i = 0; i < bmpData.Stride * bmp.Height; i++)
                {
                    extractedBytes[byteIndex] = (byte)((extractedBytes[byteIndex] << 1) | (ptr[i] & 1));
                    bitIndex++;

                    if (bitIndex == 8)
                    {
                        bitIndex = 0;
                        byteIndex++;

                        if (!lengthExtracted && byteIndex == 4)
                        {
                            messageLength = BitConverter.ToInt32(extractedBytes, 0);
                            extractedBytes = new byte[messageLength];
                            byteIndex = 0;
                            lengthExtracted = true;
                        }
                        else if (lengthExtracted && byteIndex == messageLength)
                            break;
                    }
                }
            }

            bmp.UnlockBits(bmpData);
            return Encoding.UTF8.GetString(extractedBytes);
        }
    }
}
