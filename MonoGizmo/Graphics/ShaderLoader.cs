using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MonoGizmo.Graphics
{
    public static class ShaderLoader
    {
        public static Effect LoadPrecompiledEffect(GraphicsDevice graphicsDevice)
        {
            try
            {
                string resourceName = GetShaderResourceName(graphicsDevice);
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream(resourceName) ??
                    throw new Exception($"Embedded resource '{resourceName}' not found.");
                using var mem = new MemoryStream();
                stream.CopyTo(mem);

                var content = new XNBContentManager(mem, graphicsDevice);
                return content.Load<Effect>("Effect");
            }
            catch (Exception ex)
            {
                throw new Exception($"USE HiDef PROFILE!!! \n Failed to load shader for {graphicsDevice.GraphicsProfile} profile. " +
                                  $"Adapter: {graphicsDevice.Adapter.Description}", ex);
            }
        }

        private static string GetShaderResourceName(GraphicsDevice graphicsDevice)
        {
            // Get the MonoGame.Framework assembly
            var monogameAssembly = AppDomain.CurrentDomain
                                          .GetAssemblies()
                                          .FirstOrDefault(a => a.GetName().Name == "MonoGame.Framework");

            if( monogameAssembly == null )
            {
                monogameAssembly = AppDomain.CurrentDomain
                                          .GetAssemblies()
                                          .FirstOrDefault(a => a.GetName().Name == "MonoGame.Forms.NET") ?? throw new Exception("No MonoGame packages detected");

                return "MonoGizmo.Content.apos-shapes.DirectX.xnb";
            }

            // Check for OpenGL-specific type
            bool isOpenGL = monogameAssembly.GetType("MonoGame.OpenGL.GL") != null;

            if (isOpenGL)
            {
                return "MonoGizmo.Content.apos-shapes.OpenGL.xnb";
            }
            else
            {
                return "MonoGizmo.Content.apos-shapes.DirectX.xnb";
            }
        }
    }
}
