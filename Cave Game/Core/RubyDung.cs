using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
using ErrorCode = OpenTK.Graphics.OpenGL.ErrorCode;
using Cave_Game.Core.level;
using NAudio.Wave;

namespace Cave_Game.Core
{
    public class RubyDung : GameWindow
    {
        private Level level = default!;
        private LevelRenderer levelRenderer = default!;
        private Player player = default!;
        private AudioFileReader audioFile = default!;
        private WaveOutEvent outEvent = default!;
        private readonly float[] fogColor =
        {
            14 / 255f,
            11 / 255f,
            10 / 255f,
            1f
        };

        public void CheckGLError(string context)
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
                Console.WriteLine($"OpenGL Error in {context}: {error}");
        }

        public RubyDung(GameWindowSettings gws, NativeWindowSettings nws)
            : base(gws, nws)
        { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.Texture2D);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.ClearColor(0.5f, 0.8f, 1.0f, 0.0f);
            GL.ClearDepth(1.0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Lequal);

            try
            {
                audioFile = new AudioFileReader(AppContext.BaseDirectory + @"Core\SoundTracks\Key.mp3");
                outEvent = new WaveOutEvent();

                outEvent.Init(audioFile);
                outEvent.Play();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading audio: {e.Message}");
            }

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var proj = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(70f),
                Size.X / (float)Size.Y,
                0.05f,
                1000f);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);

            level = new Level(256, 256, 64);
            levelRenderer = new LevelRenderer(level);
            player = new Player(level);

            CursorState = CursorState.Grabbed;
            CheckGLError("OnLoad");
        }

        protected override void OnUnload()
        {
            if (outEvent is not null)
            {
                outEvent.Stop();
                outEvent.Dispose();
            }
            CheckGLError("OnUnload");
            level.Save();
            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

            player.Tick(KeyboardState);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);


            var mouse = MouseState;
            player.Turn(mouse.Delta.X, mouse.Delta.Y);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            MoveCameraToPlayer((float)args.Time);

            GL.Enable(EnableCap.Fog);
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Fog(FogParameter.FogStart, -10f);
            GL.Fog(FogParameter.FogEnd, 20f);
            GL.Fog(FogParameter.FogColor, fogColor);

            GL.Disable(EnableCap.Fog);
            levelRenderer.Render(0);
            GL.Enable(EnableCap.Fog);
            levelRenderer.Render(1);
            GL.Disable(EnableCap.Texture2D);

            SwapBuffers();

            CheckGLError("OnRenderFrame");
        }

        private void MoveCameraToPlayer(float partialTicks)
        {
            GL.Translate(0.0f, 0.0f, -0.3f);
            GL.Rotate(player.XRotation, 1.0f, 0.0f, 0.0f);
            GL.Rotate(player.YRotation, 0.0f, 1.0f, 0.0f);

            double x = player.PrevX + (player.X - player.PrevX) * partialTicks;
            double y = player.PrevY + (player.Y - player.PrevY) * partialTicks;
            double z = player.PrevZ + (player.Z - player.PrevZ) * partialTicks;
            GL.Translate(-x, -y, -z);
        }
    }

  
}
