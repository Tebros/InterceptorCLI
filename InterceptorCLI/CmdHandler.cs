using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Interceptor;

namespace InterceptorCLI
{
    class CmdHandler
    {

        private Dictionary<string, Func<string[], CmdAction>> cmds;
        private Input input;
        private bool debugInStdErr;

        public CmdHandler()
        {
            cmds = new Dictionary<string, Func<string[], CmdAction>>();
            cmds["exit"] = HandleExit;

            cmds["setdebuginstderr"] = HandleSetDebugInStdErr;

            cmds["load"] = HandleLoad;
            cmds["setkeyboardfiltermode"] = HandleSetKeyboardFilterMode;
            cmds["setmousefiltermode"] = HandleSetMouseFilterMode;
            cmds["setkeypressdelay"] = HandleSetKeyPressDelay;
            cmds["setclickdelay"] = HandleSetClickDelay;
            cmds["setscrolldelay"] = HandleSetScrollDelay;
            cmds["sendmouse"] = HandleSendMouse;
            cmds["sendleftclick"] = HandleSendLeftClick;
            cmds["sendrightclick"] = HandleSendRightClick;
            cmds["sendscroll"] = HandleSendScroll;
            cmds["sendkey"] = HandleSendKey;
            cmds["sendkeystroke"] = HandleSendKeystroke;
            cmds["movemouseto"] = HandleMoveMouseTo;
            cmds["movemouseby"] = HandleMoveMouseBy;
            cmds["isloaded"] = HandleIsLoaded;
            cmds["unload"] = HandleUnload;
        }

        public CmdAction Handle(string line)
        {
            int firstSpace = line.IndexOf(' ');

            string cmd;
            string[] args;

            if (firstSpace == -1)
            {
                cmd = line;
                args = new string[0];
            }
            else
            {
                cmd = line.Substring(0, firstSpace);
                if (line.Length > firstSpace + 1)
                {
                    args = line.Substring(firstSpace + 1, line.Length - (firstSpace + 1)).Split(' ');
                }
                else
                {
                    args = new string[0];
                }
            }

            cmd = cmd.ToLower();
            if (this.cmds.ContainsKey(cmd))
            {
                args = args.Where(arg => !string.IsNullOrEmpty(arg)).ToArray();
                try
                {
                    return this.cmds[cmd](args);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"An error occurred: '{e.Message}'");
                    return CmdAction.ERROR;
                }

            }
            else
            {
                Console.Error.WriteLine($"Could not find command '{cmd}'");
                return CmdAction.ERROR;
            }
        }

        private bool CheckArgsLength(string[] args, int reqLength)
        {
            if (args.Length == reqLength)
            {
                return true;
            }
            else
            {
                Console.Error.WriteLine($"This command requires exactly '{reqLength}' arguments!");
                return false;
            }
        }

        private CmdAction HandleExit(string[] args)
        {
            this.debug("Command for 'HandleExit' detected.");

            if (this.input != null)
            {
                this.debug("Call unload command");
                this.input.Unload();
            }

            this.debug("Send exit event");

            return CmdAction.EXIT;
        }

        private CmdAction HandleTest(string[] args)
        {
            this.debug("Command for 'HandleTest' detected.");
            Console.Error.WriteLine($"Test! Args: {string.Join(", ", args)}");
            return CmdAction.OK;
        }

        private CmdAction HandleLoad(string[] args)
        {
            this.debug("Command for 'HandleLoad' detected.");

            if (this.input != null)
            {
                this.debug("Interceptor is already loaded.");
                this.debug("Send unload command");
                this.input.Unload();
            }

            this.input = new Input(this.debug);

            bool res = this.input.Load();

            if (res)
            {
                this.debug("Interceptor loaded");

                this.input.MouseFilterMode = MouseFilterMode.All;
                this.input.KeyboardFilterMode = KeyboardFilterMode.All;
                this.debug("MouseFilterMode and KeyboardFilterMode set to All");
            }
            else
            {
                this.debug("Loading Interceptor failed");
            }

            return res ? CmdAction.TRUE : CmdAction.FALSE;
        }

        private CmdAction HandleSetDebugInStdErr(string[] args)
        {
            this.debug("Command for 'HandleSetDebugInStdErr' detected.");

            if (!this.CheckArgsLength(args, 1)) return CmdAction.ERROR;

            this.debugInStdErr = Boolean.Parse(args[0]);

            return CmdAction.OK;
        }

        private CmdAction HandleSetKeyboardFilterMode(string[] args)
        {
            this.debug("Command for 'HandleSetKeyboardFilterMode' detected.");

            if (!this.CheckArgsLength(args, 1)) return CmdAction.ERROR;

            this.input.KeyboardFilterMode = (KeyboardFilterMode)Enum.Parse(typeof(KeyboardFilterMode), args[0], true);
            this.debug("KeyboardFilterMode set to " + this.input.KeyboardFilterMode.ToString());

            return CmdAction.OK;
        }

        private CmdAction HandleSetMouseFilterMode(string[] args)
        {
            this.debug("Command for 'HandleSetMouseFilterMode' detected.");

            if (!this.CheckArgsLength(args, 1)) return CmdAction.ERROR;

            this.input.MouseFilterMode = (MouseFilterMode)Enum.Parse(typeof(MouseFilterMode), args[0], true);
            this.debug("MouseFilterMode set to " + this.input.MouseFilterMode.ToString());

            return CmdAction.OK;
        }

        private CmdAction HandleSetKeyPressDelay(string[] args)
        {
            this.debug("Command for 'HandleSetKeyPressDelay' detected.");

            if (!this.CheckArgsLength(args, 1)) return CmdAction.ERROR;

            int delay = Convert.ToInt32(args[0]);

            this.input.KeyPressDelay = delay;
            this.debug("KeyPressDelay set to " + delay);

            return CmdAction.OK;
        }

        private CmdAction HandleSetClickDelay(string[] args)
        {
            this.debug("Command for 'HandleSetClickDelay' detected.");

            if (!this.CheckArgsLength(args, 1)) return CmdAction.ERROR;

            int delay = Convert.ToInt32(args[0]);

            this.input.ClickDelay = delay;
            this.debug("ClickDelay set to " + delay);

            return CmdAction.OK;
        }

        private CmdAction HandleSetScrollDelay(string[] args)
        {
            this.debug("Command for 'HandleSetScrollDelay' detected.");

            if (!this.CheckArgsLength(args, 1)) return CmdAction.ERROR;

            int delay = Convert.ToInt32(args[0]);

            this.input.ScrollDelay = delay;
            this.debug("ScrollDelay set to " + delay);

            return CmdAction.OK;
        }

        private CmdAction HandleSendMouse(string[] args)
        {
            this.debug("Command for 'HandleSendMouse' detected.");

            if (!this.CheckArgsLength(args, 1)) return CmdAction.ERROR;

            MouseState mouseState = (MouseState)Enum.Parse(typeof(MouseState), args[0], true);

            this.input.SendMouseEvent(mouseState);
            this.debug("MouseState " + mouseState.ToString() + " performed");

            if (this.input.ClickDelay > 0) Thread.Sleep(this.input.ClickDelay);

            return CmdAction.OK;
        }

        private CmdAction HandleSendLeftClick(string[] args)
        {
            this.debug("Command for 'HandleSendLeftClick' detected.");

            this.input.SendLeftClick();
            this.debug("Left click performed");

            return CmdAction.OK;
        }

        private CmdAction HandleSendRightClick(string[] args)
        {
            this.debug("Command for 'HandleSendRightClick' detected.");

            this.input.SendRightClick();
            this.debug("Right click performed");

            return CmdAction.OK;
        }

        private CmdAction HandleSendScroll(string[] args)
        {
            this.debug("Command for 'HandleSendScroll' detected.");

            if (!this.CheckArgsLength(args, 1)) return CmdAction.ERROR;

            int amount = Convert.ToInt32(args[0]);
            ScrollDirection direction = amount > 0 ? ScrollDirection.Down : ScrollDirection.Up;

            for (int i = 0; i < Math.Abs(amount); i++)
            {
                this.input.ScrollMouse(direction);

                if (this.input.ScrollDelay > 0) Thread.Sleep(this.input.ScrollDelay);
            }
            this.debug("Scroll " + direction.ToString() + " performed " + amount + " times");

            return CmdAction.OK;
        }

        private CmdAction HandleSendKey(string[] args)
        {
            this.debug("Command for 'HandleSendKey' detected.");

            if (!this.CheckArgsLength(args, 2)) return CmdAction.ERROR;

            Keys key = (Keys)Enum.Parse(typeof(Keys), args[0], true);
            KeyState keyState = (KeyState)Enum.Parse(typeof(KeyState), args[1], true);

            this.input.SendKey(key, keyState);
            this.debug("Key " + key.ToString() + " " + keyState.ToString() + " performed");



            return CmdAction.OK;
        }

        private CmdAction HandleSendKeystroke(string[] args)
        {
            this.debug("Command for 'HandleSendKeystroke' detected.");

            if (!this.CheckArgsLength(args, 1)) return CmdAction.ERROR;

            Keys key = (Keys)Enum.Parse(typeof(Keys), args[0], true);

            this.input.SendKey(key);
            this.debug("Key " + key.ToString() + " clicked");

            return CmdAction.OK;
        }

        private CmdAction HandleMoveMouseTo(string[] args)
        {
            this.debug("Command for 'HandleMoveMouseTo' detected.");

            if (!this.CheckArgsLength(args, 3)) return CmdAction.ERROR;

            int x = Convert.ToInt32(args[0]);
            int y = Convert.ToInt32(args[1]);
            bool useDriver = Convert.ToBoolean(args[2]);

            this.input.MoveMouseTo(x, y, useDriver);
            this.debug("Mouse moved to " + x + ", " + y + (useDriver ? " using driver" : " using no driver"));

            return CmdAction.OK;
        }

        private CmdAction HandleMoveMouseBy(string[] args)
        {
            this.debug("Command for 'HandleMoveMouseBy' detected.");

            if (!this.CheckArgsLength(args, 3)) return CmdAction.ERROR;

            int deltaX = Convert.ToInt32(args[0]);
            int deltaY = Convert.ToInt32(args[1]);
            bool useDriver = Convert.ToBoolean(args[2]);

            this.input.MoveMouseBy(deltaX, deltaY, useDriver);
            this.debug("Mouse moved by " + deltaX + ", " + deltaY + (useDriver ? " using driver" : " using no driver"));

            return CmdAction.OK;
        }

        private CmdAction HandleIsLoaded(string[] args)
        {
            this.debug("Command for 'HandleIsLoaded' detected.");

            return this.input.IsLoaded ? CmdAction.TRUE : CmdAction.FALSE;
        }

        private CmdAction HandleUnload(string[] args)
        {
            this.debug("Command for 'HandleUnload' detected.");

            this.input.Unload();
            this.debug("Interceptor unloaded");

            return CmdAction.OK;
        }

        private void debug(String msg)
        {
            if (this.debugInStdErr)
            {
                Console.Error.WriteLine(msg);
            }
        }
    }
}
