using System;
using System.Drawing;
using System.IO;
using ImageSteganographyConsole.Models;

namespace ImageSteganographyConsole
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Image Steganography with Encryption");
            Console.WriteLine("1. Hide Message");
            Console.WriteLine("2. Extract Message");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter image path: ");
                string imagePath = Console.ReadLine();

                Console.Write("Enter secret message: ");
                string message = Console.ReadLine();

                Console.Write("Enter encryption key: ");
                string key = Console.ReadLine();

                string encryptedMessage = Encryption.Encrypt(message, key);
                Bitmap image = new Bitmap(imagePath);
                Bitmap encryptedImage = Steganography.EncodeMessage(image, encryptedMessage);

                string outputImage = Path.Combine(Directory.GetCurrentDirectory(), "encrypted_image.png");

                if (encryptedImage != null)
                {
                    encryptedImage.Save(outputImage, System.Drawing.Imaging.ImageFormat.Png);
                    Console.WriteLine($"Message encrypted and saved to {outputImage}");
                }
                else
                {
                    Console.WriteLine("Error: Encrypted image generation failed.");
                }

            }
            else if (choice == "2") {
                Console.Write("Enter encrypted image path: ");
                string imagePath = Console.ReadLine();

                Console.Write("Enter decryption key: ");
                string key = Console.ReadLine();

                Bitmap image = new Bitmap(imagePath);
                string extractedEncryptedMessage = Steganography.DecodeMessage(image);
                string decryptedMessage = Encryption.Decrypt(extractedEncryptedMessage, key);

                Console.WriteLine($"Hidden Message: {decryptedMessage}");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }

            //D:\stegnanography\bin\Debug\net8.0\encrypted_image.png

        }
    }
}