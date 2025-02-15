using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace MonoGizmo.Graphics
{
    internal class XNBContentManager : ContentManager
    {
        class FakeGraphicsService : IGraphicsDeviceService
        {
            public FakeGraphicsService(GraphicsDevice graphicDevice)
            {
                GraphicsDevice = graphicDevice;
            }

            public GraphicsDevice GraphicsDevice { get; private set; }

#pragma warning disable 67
            public event EventHandler<EventArgs> DeviceCreated;
            public event EventHandler<EventArgs> DeviceDisposing;
            public event EventHandler<EventArgs> DeviceReset;
            public event EventHandler<EventArgs> DeviceResetting;
#pragma warning restore 67
        }

        class FakeServiceProvider : IServiceProvider
        {
            GraphicsDevice _graphicDevice;
            public FakeServiceProvider(GraphicsDevice graphicDevice)
            {
                _graphicDevice = graphicDevice;
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IGraphicsDeviceService))
                    return new FakeGraphicsService(_graphicDevice);

                throw new NotImplementedException();
            }
        }

        private readonly MemoryStream _xnbStream;

        public XNBContentManager(MemoryStream xnbStream, GraphicsDevice graphicDevice)
            : base(new FakeServiceProvider(graphicDevice), "Content")
        {
            _xnbStream = xnbStream;
        }

        protected override Stream OpenStream(string assetName)
        {
            return new MemoryStream(_xnbStream.GetBuffer(), false);
        }
    }
}
