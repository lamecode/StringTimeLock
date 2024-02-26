
![Logo](https://lamecode.eu/exclude/linked-files/stringTimeLock.png)


# String Time Lock

A locally hosted application that stores your passwords or otherwise important strings under a master password AND a timed lock. 



## What it can do?
### Password/String storing:
- You can store any amount of passwords or strings
- You can attach a short descriptions to them
- These passwords or strings are saved in a file, encrypted using AES

### What is planned:
- Let user decide how long of a timed lock they want to set up for EACH of the strings.
- Encrypt the strings using two factor encryption (Master password and AES - currently only AES)
- Add option to store master password externally on a thumb (USB) drive.
- Add option to remove stored passwords/strings. This can be currently only done by manually deleting the data files in app directory.

## What it CAN'T do?
- Store any other kind of data except string-based data.
- You cannot force the decryption time, you must wait the time you have chosen when storing it (I mean, unless you have the source code, which you do. But in production, you can't.)

## Installation

At this moment, this application is fully portable. There is no installation required and it can be stored and carried on a thumb drive.
## License

You can use this code, build your projects on top of it or use parts of it in any way, but if you do, you have to credit me on top of the Program.cs code in a //comment.
