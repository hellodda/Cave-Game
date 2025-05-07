using Cave_Game.Core;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using NAudio.Wave;

try
{

    var window = new RubyDung(GameWindowSettings.Default, new NativeWindowSettings
    {
        ClientSize = new Vector2i(1660, 1200),
        Title = "Cave Game",
        API = ContextAPI.OpenGL,
        APIVersion = new Version(4, 6),
        Profile = ContextProfile.Compatability,
        Flags = ContextFlags.Default,
    });

    Console.WriteLine($"OpenGL Version: {GL.GetString(StringName.Version)}");
    Console.WriteLine($"Renderer: {GL.GetString(StringName.Renderer)}");
    Console.WriteLine($"Vendor: {GL.GetString(StringName.Vendor)}");
    Console.WriteLine($"GLSL Version: {GL.GetInteger(GetPName.MajorVersion)}.{GL.GetInteger(GetPName.MinorVersion)}");
    Console.WriteLine("Game Version: 0.0.1a");
    Console.WriteLine("--------------------------------------------------------------------------------------------");

    window.Run();

   
}
catch(Exception ex)
{
    Console.WriteLine($"Fatal Error: {ex.Message}");
}