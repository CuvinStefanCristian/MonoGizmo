using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Content.Pipeline.Builder;
using System.IO;

namespace MonoGizmo.Graphics
{
    internal static class ShaderCompiler
    {
        public static Effect CompileFX(GraphicsDevice gd, string fxcode, TargetPlatform targetPlatform)
        {
            string sourceFile = $"{Directory.GetCurrentDirectory()}\\tmp.txt";
            File.WriteAllText(sourceFile, fxcode);
            EffectImporter importer = new EffectImporter();
            EffectContent content = importer.Import(sourceFile, null);
            EffectProcessor processor = new EffectProcessor
            {
                Defines = targetPlatform.ToString()
            };
            PipelineManager pm = new PipelineManager(string.Empty, string.Empty, string.Empty)
            {
                Platform = targetPlatform
            };
            PipelineProcessorContext ppc = new PipelineProcessorContext(pm, new PipelineBuildEvent());
            CompiledEffectContent cecontent = processor.Process(content, ppc);
            ContentCompiler compiler = new ContentCompiler();
            return new Effect(gd, cecontent.GetEffectCode());
        }
    }
}
