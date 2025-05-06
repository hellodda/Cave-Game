using Cave_Game.Core;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

var window = new RubyDung(GameWindowSettings.Default, new NativeWindowSettings
{
    ClientSize = new Vector2i(1280, 720),
    Title = "Cave Game",
    API = ContextAPI.OpenGL,
    APIVersion = new Version(3, 2),          
    Profile = ContextProfile.Compatability,
});

window.Run();

