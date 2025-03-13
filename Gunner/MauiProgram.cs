
using Microsoft.Extensions.Logging;




namespace Gunner
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Thasadith-Bold.ttf", "ThasadithBold");
                    fonts.AddFont("Thasadith-Boldltalic.ttf", "ThasadithBoldltalic");
                    fonts.AddFont("Thasadith-Italic.ttf", "ThasadithItalic");
                    fonts.AddFont("Thasadith-Regular.ttf", "ThasadithRegular");
                    fonts.AddFont("Kanit-Bold.ttf", "kanitBold");
                    fonts.AddFont("Kanit-Medium.ttf", "kanitMedium");
                    fonts.AddFont("Kanit-Light.ttf", "kanitLight");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });
            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("EditorCustomization", (handler, view) =>
            {
#if ANDROID
        handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("PickerCustomization", (handler, view) =>
            {
#if ANDROID
        handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });




            return builder.Build();
        }




        

    }

}


