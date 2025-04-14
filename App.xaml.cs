namespace CarspotLourd
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Utiliser directement MainPage au lieu de AppShell
            MainPage = new MainPage();
        }
    }
}
