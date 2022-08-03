namespace Aston {
    static class Program
    {
        public static void Main()
        {
            string fl = "C:\\Users\\attac\\OneDrive\\Desktop\\raylibgame\\Config\\config.lua";
            WindowHandle wh = WindowHandle.FromConfig(fl);
            wh.Run();
        }
    }
}