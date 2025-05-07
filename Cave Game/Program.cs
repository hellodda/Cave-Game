using Cave_Game.Core;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

var window = new RubyDung(GameWindowSettings.Default, new NativeWindowSettings
{
    ClientSize = new Vector2i(1660, 1200),
    Title = "Cave Game",
    API = ContextAPI.OpenGL,
    APIVersion = new Version(4, 6),          
    Profile = ContextProfile.Compatability,
    Flags = ContextFlags.Default,   
});

window.Run();

