using System;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Ninject;

namespace FractalPainting.App.Actions
{
    public class DragonFractalAction : IUiAction
    {
        private DragonPainter dragonPainter;
        private readonly IDragonPainterFactory dpFactory;
        private readonly Func<Random, DragonSettingsGenerator> dsFactory;

        public DragonFractalAction(IDragonPainterFactory dpFactory, Func<Random, DragonSettingsGenerator> dsFactory)
        {
            this.dpFactory = dpFactory;
            this.dsFactory = dsFactory;
        }

        public string Category => "Фракталы";
        public string Name => "Дракон";
        public string Description => "Дракон Хартера-Хейтуэя";

        public void Perform()
        {
            var dragonSettings = CreateRandomSettings();
            SettingsForm.For(dragonSettings).ShowDialog();

            dragonPainter = dpFactory.Create(dragonSettings);
            dragonPainter.Paint();
        }

        private DragonSettings CreateRandomSettings()
        {
            return dsFactory.Invoke(new Random()).Generate();
        }
    }
}