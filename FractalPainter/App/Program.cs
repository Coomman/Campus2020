using System;
using System.Windows.Forms;
using FractalPainting.App.Actions;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;

namespace FractalPainting.App
{
    internal static class Program
    {
        private static void KochLoad(StandardKernel container)
        {
            container.Bind<IImageHolder, PictureBoxImageHolder>().To<PictureBoxImageHolder>().InSingletonScope();
            container.Bind<Palette>().ToSelf().InSingletonScope();
        }

        private static void DragonLoad(StandardKernel container)
        {
            container.Bind<IDragonPainterFactory>().ToFactory();
        }

        private static void ImageSettingsLoad(StandardKernel container)
        {
            container.Bind<IObjectSerializer>().To<XmlObjectSerializer>();
            container.Bind<IBlobStorage>().To<FileBlobStorage>();

            container.Bind<AppSettings, IImageDirectoryProvider>()
                .ToMethod(context => context.Kernel.Get<SettingsManager>().Load())
                .InSingletonScope();

            container.Bind<ImageSettings>()
                .ToMethod(context => context.Kernel.Get<AppSettings>().ImageSettings)
                .InSingletonScope();
        }

        private static StandardKernel Load()
        {
            var container = new StandardKernel();

            KochLoad(container);
            DragonLoad(container);
            ImageSettingsLoad(container);

            container.Bind(x => x
                .FromThisAssembly()
                .SelectAllClasses().InNamespaceOf<DragonFractalAction>()
                .BindAllInterfaces());

            container.Bind<Form>().To<MainForm>();

            return container;
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                var container = Load();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(container.Get<Form>());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}