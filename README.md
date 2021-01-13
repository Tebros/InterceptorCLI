# InterceptorCLI

Note: Windows 8/8.1 is not supported. This project is designed for 64 bit architectures (x64).

InterceptorCLI is a command line interface for the parent project [Interceptor](https://github.com/jasonpang/Interceptor).  Interceptor is a wrapper library for a mouse and keyboard driver that provides a programming interface.

With the InterceptorCLI it is possible to control mouse clicks and keystrokes via commands in a terminal.

## Getting Started

### Manual Installation

1. Download the latest release [here](https://github.com/Tebros/InterceptorCLI/releases/latest) or build this project by your own. Move the InterceptorCLI.exe and Interceptor.dll files into the same directory of your choice.

2. Download the driver (Interception.zip) from [here](https://github.com/oblitum/Interception/releases/tag/v1.0.1)

3. Unzip [Interception.zip](https://github.com/oblitum/Interception/releases/download/v1.0.1/Interception.zip) to a temporary directory

4. Open a new terminal with administrator privileges (`WIN+X` and click `Command Prompt (Admin)`). Navigate to the temporary directory via the `cd <path>` command.

5. Execute the following command: `install-interception.exe /install`

6. Copy the file "Interception/library/x64/interception.dll" into the same directory, where the InterceptorCLI.exe is located (step 1)

7. Restart your computer. If you want, you can now delete the unzipped directory and the Interception.zip

You can find the installation instruction for Interceptor [here](https://github.com/jasonpang/Interceptor/blob/master/README.md). The installation instruction for the driver can be found [here](https://github.com/oblitum/Interception/blob/v1.0.1/README.md).

### Usage and commands

If you have followed the installation instruction correctly, you only need to execute the InterceptorCLI.exe and start typing.

The workflow is always the same. Firstly you type a command and execute it by pressing the enter key. The program handles the command with its arguments and prints a simple response:

| Response | Description |
|:--|:--|
| OK | The command was successfully executed. |
| TRUE | The answer of your question is "yes". |
| FALSE | The answer of your question is "no" |
| ERROR | An error occurred. The error is displayed in the terminal. |
| EXIT | The program was successfully terminated. |

These are the available commands:

| Command | Description |
|:--|:--|
| load | Load Interceptor |
| setkeyboardfiltermode [mode] | Set the filter mode for the keyboard |
| setmousefiltermode [mode] | Set the filter mode for the mouse |
| setkeypressdelay [delay] | Milliseconds between a key press and release |
| setclickdelay [delay] | Milliseconds between a mouse press and release |
| setscrolldelay [delay] | Milliseconds between the scroll steps |
| sendmouse [mousestate] | Send the given mouse state |
| sendleftclick | Click at the current position with the left mouse button |
| sendrightclick | Click at the current position with the right mouse button |
| sendscroll [amount] | Scrolls amount times. Positive amount = down |
| sendkey [key] [keystate] | Send the given key state for the given key |
| sendkeystroke [key] | Press and release the given key |
| movemouseto [x] [y] [useDriver] | Moves the mouse to the given point. `useDriver` indicates if the driver should be used for it. |
| movemouseby [deltaX] [deltaY] [useDriver] | Moves the mouse, based on the current position. `useDriver` indicates if the driver should be used for it. |
| isloaded | If Interceptor is loaded |
| unload | Unload Interceptor |
| exit | Unload Interceptor and terminate the program |

These are the keyboard filter modes: `None`, `All`, `KeyDown`, `KeyUp`, `KeyE0`, `KeyE1`, `KeyTermsrvSetLED`, `KeyTermsrvShadow`, `KeyTermsrvVKPacket`

These are the mouse filter modes: `None`, `All`, `LeftDown`, `LeftUp`, `RightDown`, `RightUp`, `MiddleDown`, `MiddleUp`, `LeftExtraDown`, `LeftExtraUp`, `RightExtraDown`, `RightExtraUp`, `MouseWheelVertical`, `MouseWheelHorizontal`, `MouseMove`


### Technical Details

The InterceptorCLI listens for line breaks in the standard input stream. Responses are written to the standard output stream with a line break at the end. Errors are written to the standard error stream with a line break at the end.
You can integrate the InterceptorCLI in your own software. 

### Important Notes

1. Only Windows is  supported. I have tested the program under Windows 10.
2. Only the 64 bit architecture (x64) is supported. 
3. To send keys you need to register your keyboard. To do this, just press any key on your keyboard **after** you have executed the `load` command.
4. You firstly have to run the `load` command.
5. Execute the `unload` or exit command if you wish to terminate the program. Please don't just kill the process!
6. I recommend setting the keyboard filter mode to `All`