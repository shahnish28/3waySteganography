# 3-Way Steganography

## 🔍 Overview

This project implements **three-way image steganography** with encryption to securely hide messages within images. It combines **AES encryption** for data security and **LSB (Least Significant Bit) steganography** for concealing encrypted messages within an image.

## 🚀 Features

- **AES Encryption:** Ensures that the message is securely encrypted before embedding it in the image.
- **LSB Steganography:** Hides the encrypted data in the least significant bits of the image pixels.
- **Message Extraction:** Decodes and decrypts hidden messages from an image.
- **Secure Communication:** Prevents unauthorized access to embedded messages.

## 🛠️ Installation

### Prerequisites

- **.NET SDK** (C# development)
- **Git** (for version control)
- **Visual Studio** or **VS Code** (Recommended IDEs)

### Clone the Repository

```sh
git clone https://github.com/shahnish28/3waySteganography.git
cd 3waySteganography
```

## 📌 Usage

### 1️⃣ Encrypt and Embed a Message

- Encrypt a message using AES before embedding.
- Hide the encrypted message inside an image using LSB.

### 2️⃣ Extract and Decrypt a Message

- Extract the encrypted message from the stego-image.
- Decrypt it to retrieve the original message.

### Run the Program

```sh
dotnet run
```

## 📂 Project Structure

```
3waySteganography/
│── ImageSteganographyConsole/
│   ├── Models/
│   │   ├── Encryption.cs  # Handles AES encryption & decryption
│   │   ├── Steganography.cs  # Handles LSB encoding & decoding
│   ├── Program.cs  # Main execution logic
│── README.md
```

## ⚙️ Functions Explained

### 🔐 `Encryption.cs`

- `` → Encrypts a message using AES.
- `` → Decrypts the message.

### 🖼️ `Steganography.cs`

- `` → Embeds encrypted data into an image.
- `` → Extracts hidden data from an image.

## ❗ Troubleshooting

### **1️⃣ Git Push Error: "Updates were rejected"**

If you see an error like:

```sh
error: failed to push some refs to 'https://github.com/...'
```

Try:

```sh
git pull --rebase origin main
git push origin main
```

### **2️⃣ Git Safe Directory Error**

If you encounter:

```sh
fatal: detected dubious ownership in repository
```

Run:

```sh
git config --global --add safe.directory D:/steganography
```

## 📝 License

This project is open-source and available under the [MIT License](LICENSE).

## 💡 Future Enhancements

- Support for different encryption algorithms.
- GUI implementation for a user-friendly interface.
- Support for multiple file formats (JPEG, PNG, BMP, etc.).

## 📧 Contact

For questions or contributions, contact [**@\_\shahnish28**](https://github.com/shahnish28).

